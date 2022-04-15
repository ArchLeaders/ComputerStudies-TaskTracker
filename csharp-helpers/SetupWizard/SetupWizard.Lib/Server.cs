using System.Text.Json;
using System.Text.Json.Serialization;

namespace SetupWizard.Lib
{
    public class Server
    {
        [JsonPropertyName("tasks")]
        public Dictionary<string, ServerTask> Tasks { get; set; } = new();

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; } = "";

        [JsonPropertyName("vars")]
        public Dictionary<string, string> Vars { get; set; } = new();

        public class ServerTask
        {
            [JsonPropertyName("channel")]
            public ulong Channel { get; set; }

            [JsonPropertyName("days")]
            public string[] Days { get; set; } = new string[0];

            [JsonPropertyName("message")]
            public string Message { get; set; }

            [JsonPropertyName("role")]
            public ulong? Role { get; set; }

            [JsonPropertyName("sequence")]
            public string[] Sequence { get; set; } = new string[0];

            [JsonPropertyName("time")]
            public string Time { get; set; }

            [JsonPropertyName("user")]
            public ulong? User { get; set; }

            public ServerTask(ulong channel, string message, string time)
            {
                Channel = channel;
                Message = message;
                Time = time;
            }
        }

        /// <summary>
        /// Quick serializer
        /// </summary>
        /// <param name="jsonFile"></param>
        public void Write(string jsonFile)
        {

        }
    }
}
