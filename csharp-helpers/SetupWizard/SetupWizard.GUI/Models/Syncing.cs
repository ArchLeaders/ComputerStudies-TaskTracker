using Acheron.Web;
using Discord;
using Discord.WebSocket;
using SetupWizard.GUI.ViewModels;
using SetupWizard.GUI.ViewResources.Data;
using Stylet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SetupWizard.GUI.Models
{
    public class Syncing
    {
        private DiscordSocketClient Client { get; set; } = new();
        private bool Working { get; set; } = true;
        private string TempFile { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Temp\\server.io";

        public async Task<Server> Fetch(ulong settingsChannelId)
        {
            Server server = new();
            await Connect(OnReady);
            return server;

            async Task OnReady()
            {
                // Download server settings
                IMessageChannel channel = (IMessageChannel)Client.GetChannel(settingsChannelId);
                byte[] bytes = Array.Empty<byte>();

                await foreach (var messages in channel.GetMessagesAsync(1))
                    bytes = new Uri(messages.ToArray()[0].Attachments.ToArray()[0].Url).GetBytes();

                // Deserialize server settings
                File.WriteAllBytes(TempFile, bytes);
                server = JsonSerializer.Deserialize<Server>(bytes) ?? new();

                // Get channels, roles and users
                SocketGuildChannel guildChannel = (SocketGuildChannel)Client.GetChannel(settingsChannelId);
                IGuild guild = guildChannel.Guild;

                TaskModel.Channels.Clear();
                TaskModel.Roles.Clear();

                foreach (ITextChannel serverChannel in await guild.GetTextChannelsAsync())
                {
                    string category = "";

                    if (serverChannel.CategoryId != null)
                        category = $" ({((ICategoryChannel)await Client.GetChannelAsync(serverChannel.CategoryId ?? 0)).Name})";

                    if (category == " (task-tracker-metadata)")
                        continue;

                    TaskModel.Channels.Add(new(serverChannel.Id, $"{serverChannel.Name}{category}"));
                }

                foreach (IRole role in guild.Roles)
                {

                    if (role.Name == "TaskTracker")
                        continue;

                    TaskModel.Roles.Add(new(role.Id, role.Name));
                }

                // The bot does not have access to the users of a guild, so it will be temporarily commented out
                // 
                // foreach (IUser user in await guild.GetUsersAsync())
                // {
                // 
                //     if (user.Username == "TaskTracker#3825")
                //         continue;
                // 
                //     TaskModel.Users.Add(new(user.Id, user.Username));
                // }

                // Complete task
                Working = false;
            }
        }

        public async Task Send(SettingsViewModel settings)
        {
            // Create server
            Server server = new();

            foreach (var task in settings.ShellViewModel.Tasks)
                server.Tasks.Add(new(task));

            foreach (var _var in settings.ShellViewModel.Vars)
                server.Vars.Add(_var.Key, _var.Value);

            server.Timezone = settings.Timezone;

            await Send(settings.ServerID, server);
        }

        public async Task Send(ulong serverId, Server server)
        {
            await Connect(OnReady);

            async Task OnReady()
            {
                // Serialize server settings
                string _json = JsonSerializer.Serialize(server, new JsonSerializerOptions()
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(TempFile, _json);

                // Update server settings
                IMessageChannel channel = (IMessageChannel)Client.GetChannel(serverId);
                await channel.SendFileAsync(TempFile);

                // Complete task
                Working = false;
            }
        }

        public async Task Connect(Func<Task> onReady)
        {
            // Start working
            Working = true;

            // Connect the client
            await Client.LoginAsync(TokenType.Bot, User.DiscordBot);
            await Client.StartAsync();
            Client.Ready += onReady;
            Client.Log += Log;

            while (Working)
                await Task.Delay(1000);

            await Client.DisposeAsync();
        }

        private Task Log(LogMessage ex)
        {
            Debug.WriteLine(ex.ToString());
            return Task.CompletedTask;
        }
    }
}
