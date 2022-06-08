#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using SetupWizard.GUI.ViewModels;
using Stylet;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SetupWizard.GUI.Models
{
    public class TaskModel
    {
        public Dictionary<string, bool> Days { get; set; } = new();
        public Dictionary<string, int> Session { get; set; } = new();
        public Dictionary<string, List<string>> Sequence { get; set; } = new();

        public string Time { get; set; }
        public string Message { get; set; }
        public ulong Channel { get; set; }
        public ulong Role { get; set; }
        public ulong User { get; set; }

        [JsonIgnore()]
        public static BindableCollection<KeyValuePair<ulong, string>> Channels { get; set; } = new();

        [JsonIgnore()]
        public static BindableCollection<KeyValuePair<ulong, string>> Roles { get; set; } = new();

        [JsonIgnore()]
        public static BindableCollection<KeyValuePair<ulong, string>> Users { get; set; } = new();

        [JsonConstructor]
        public TaskModel() { }

        public TaskModel(TaskViewModel task)
        {
            Message = task.Message;

            Time = task.Time;
            Channel = task.Channel.Key;
            Role = task.Role.Key;
            User = task.User.Key;

            Days.Add(nameof(task.Mon), task.Mon);
            Days.Add(nameof(task.Tue), task.Tue);
            Days.Add(nameof(task.Wed), task.Wed);
            Days.Add(nameof(task.Thu), task.Thu);
            Days.Add(nameof(task.Fri), task.Fri);
            Days.Add(nameof(task.Sat), task.Sat);
            Days.Add(nameof(task.Sun), task.Sun);

            if (task.Sequence != null && task.Sequence != "") {
                string[] sqVars = task.Sequence.Replace(" ", "").Split(';');

                foreach (var sqVar in sqVars) {
                    if (sqVar == "")
                        continue;

                    string[] sqFunc = sqVar.Replace(")", "").Split('(');
                    string[] sqArgs = sqFunc[1].Split(',');

                    Sequence.Add(sqFunc[0], sqArgs.ToList());

                    // Set the session variable
                    if (task.Session.ContainsKey(sqFunc[0])) {
                        Session[sqFunc[0]] = task.Session[sqFunc[0]];
                    }
                    else {
                        Session.Add(sqFunc[0], 0);
                    }
                }
            }
        }
    }
}
