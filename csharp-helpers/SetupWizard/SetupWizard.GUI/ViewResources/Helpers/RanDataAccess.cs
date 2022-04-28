using SetupWizard.GUI.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetupWizard.GUI.ViewResources.Helpers
{
    internal class RanDataAccess
    {
        public static BindableCollection<TasksViewModel> Get()
        {
            BindableCollection<TasksViewModel> tasks = new();

            tasks.Add(new()
            {
                Synced = true,
                Time = TimeOnly.Parse("1:20PM"),
                Days = new() { "sun", "mon" },
                Channel = "data-access",
                Role = "tasks",
                User = "ArchLeaders",
                Sequence = new() { "user_a", "user_w" },
                Message = "@role $user needs to do somethig at $time_now"
            });

            tasks.Add(new()
            {
                Synced = false,
                Time = TimeOnly.Parse("2:50PM"),
                Days = new() { "tue", "thu" },
                Channel = "data-access",
                Role = "tasks",
                User = "ArchLeaders",
                Sequence = new() { "user_w" },
                Message = "@role $user needs to do somethig at $time_now"
            });

            tasks.Add(new()
            {
                Synced = true,
                Time = TimeOnly.Parse("3:25PM"),
                Days = new() { "sat", "mon" },
                Channel = "data-access",
                Role = "tasks",
                User = "ArchLeaders",
                Sequence = new() { "user_w", "user_a" },
                Message = "@role $user needs to do somethig at $time_now"
            });

            return tasks;
        }
    }
}
