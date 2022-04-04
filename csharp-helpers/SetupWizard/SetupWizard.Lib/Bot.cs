using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetupWizard.Lib
{
    public class Bot
    {
        public DiscordSocketClient? Client { get; private set; }
        public Server? Server { get; private set; }
        public async Task SyncServerData(Server server)
        {
            Server = server;
            Client = new DiscordSocketClient();

            await Client.LoginAsync(TokenType.Bot, Env.Token);
            await Client.StartAsync();
            Client.Ready += OnReady;
            await Task.Delay(-1);
        }

        private async Task OnReady()
        {
            if (Client != null)
            {
                IMessageChannel channel = (IMessageChannel)Client.GetChannel(new Server().ServerID);
                await channel.SendFileAsync(@"settings.json");
            }
        }
    }
}
