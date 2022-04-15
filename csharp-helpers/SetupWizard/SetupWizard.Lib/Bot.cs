using Discord;
using Discord.WebSocket;
using System.Text;
using System.Text.Json;

namespace SetupWizard.Lib
{
    public class Bot
    {
        public DiscordSocketClient Client { get; private set; }
        public Server Server { get; private set; }
        private string TempFile { get; set; }

        public Bot(Server server)
        {
            Client = new DiscordSocketClient();
            Server = server;
            TempFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Temp\\{Server.Channel}";

            // Write dummy file to satisfy handler
            File.WriteAllBytes(TempFile, new byte[0]);
        }

        public async Task<bool> Sync()
        {
            // Connect to the Discord client
            await Client.LoginAsync(TokenType.Bot, Env.Token);
            await Client.StartAsync();
            Client.Ready += OnReady;

            // Workaround to keep the app running
            // as BOTS are typicaly only used in Console apps
            // 
            // This gives the application 10 seconds to
            // connect and upload the data file
            // 
            // This will be adjustable from the UI
            await Task.Delay(10000);

            // If true, the BOT ended before the bot connected
            if (File.Exists(TempFile))
            {
                File.Delete(TempFile);
                return false;
            }

            return true;
        }

        private async Task OnReady()
        {
            // Serialize server data to a JSON string
            var json = JsonSerializer.Serialize(Server);

            // Compress with the Yaz0 algorithm for smaller
            // upload sizes to keep under the 8MB limit
            var bytes = Yaz0.Compress(Encoding.Default.GetBytes(json), 9);
            await File.WriteAllBytesAsync(TempFile, bytes);

            if (Client != null)
            {
                // Update server settings
                IMessageChannel channel = (IMessageChannel)Client.GetChannel(Server.Channel);
                await channel.SendFileAsync(TempFile);
            }

            // Delete temp JSON file
            File.Delete(TempFile);
        }
    }
}
