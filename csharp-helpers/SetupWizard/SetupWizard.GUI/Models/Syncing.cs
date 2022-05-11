using Acheron.Web;
using Discord;
using Discord.WebSocket;
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

        private CancellationTokenSource CancelToken = new();
        private string TempFile { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Temp\\server.io";

        public async Task<Server> Fetch(ulong settingsChannelId)
        {
            Server server = new();
            await Connect(OnReady);

            async Task OnReady()
            {
                // Download server settings
                IMessageChannel channel = (IMessageChannel)Client.GetChannel(settingsChannelId);
                byte[] bytes = Array.Empty<byte>();

                await foreach (var messages in channel.GetMessagesAsync(1))
                    bytes = new Uri(messages.ToArray()[0].Attachments.ToArray()[0].Url).GetBytes();

                // Deserialize server settings
                File.WriteAllBytes(TempFile, bytes);
                server = JsonSerializer.Deserialize<Server>(bytes);

                // Disbose client
                await Client.DisposeAsync();
            }

            return server;
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

                // Disbose client
                await Client.DisposeAsync();
            }
        }

        public async Task Connect(Func<Task> onReady)
        {
            // Connect the client
            await Client.LoginAsync(TokenType.Bot, User.DiscordBot);
            await Client.StartAsync();
            Client.Ready += onReady;
            Client.Log += Log;
        }

        private Task Log(LogMessage ex)
        {
            Debug.WriteLine(ex.ToString());
            return Task.CompletedTask;
        }
    }
}
