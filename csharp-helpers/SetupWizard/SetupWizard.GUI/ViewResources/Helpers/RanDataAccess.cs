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
                Channel = 0,
                Role = 0,
                User = 0,
                Sequence = new() { "user_a", "user_w" },
                Message = "@role $user needs to do somethig at $time_now"
            });

            tasks.Add(new()
            {
                Synced = false,
                Time = TimeOnly.Parse("2:50PM"),
                Days = new() { "tue", "thu" },
                Channel = 6,
                Role = 2,
                User = 5,
                Sequence = new() { "user_w" },
                Message = "@role $user needs to do somethig at $time_now"
            });

            tasks.Add(new()
            {
                Synced = true,
                Time = TimeOnly.Parse("3:25PM"),
                Days = new() { "sat", "mon" },
                Channel = 7,
                Role = 1,
                User = 3,
                Sequence = new() { "user_w", "user_a" },
                Message = "@role $user needs to do somethig at $time_now"
            });

            return tasks;
        }
    }
}
