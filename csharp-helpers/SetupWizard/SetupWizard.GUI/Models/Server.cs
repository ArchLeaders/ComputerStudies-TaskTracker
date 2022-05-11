using SetupWizard.GUI.ViewModels;
using Stylet;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SetupWizard.GUI.Models
{
    public class Server
    {
        /// <summary>
        /// Bindable dictionary of Channels
        /// </summary>
        [JsonIgnore()]
        public BindableCollection<KeyValuePair<uint, string>>? Channels { get; set; }

        /// <summary>
        /// Bindable dictionary of Users in the Server
        /// </summary>
        [JsonIgnore()]
        public BindableCollection<KeyValuePair<uint, string>>? Users { get; set; }

        /// <summary>
        /// Bindable dictionary of Roles in the Server
        /// </summary>
        [JsonIgnore()]
        public BindableCollection<KeyValuePair<uint, string>>? Roles { get; set; }

        /// <summary>
        /// Timezone used when setting off the tasks
        /// </summary>
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; } = "";

        /// <summary>
        /// Bindable list of tasks
        /// </summary>
        [JsonPropertyName("tasks")]
        public BindableCollection<TaskViewModel>? Tasks { get; set; }

        /// <summary>
        /// Bindable dictionary of vars
        /// </summary>
        [JsonPropertyName("vars")]
        public BindableCollection<KeyValuePair<string, string>>? Vars { get; set; }
    }
}
