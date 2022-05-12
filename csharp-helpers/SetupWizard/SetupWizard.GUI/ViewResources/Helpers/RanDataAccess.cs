using SetupWizard.GUI.ViewModels;
using Stylet;
using System;

namespace SetupWizard.GUI.ViewResources.Helpers
{
    internal class RanDataAccess
    {
        public static BindableCollection<TaskViewModel> Get()
        {
            BindableCollection<TaskViewModel> tasks = new();

            tasks.Add(new(0)
            {
                Time = "1:20PM",
                Channel = new(935399672988712973, "data-access"),
                Role = new(935400511773999104, "Admin"),
                User = new(728823685015797770, "ArchLeaders"),
                Sequence = "user(user_a, user_m)",
                Message = "@role $user needs to do somethig at $time_now"
            });

            tasks.Add(new(1)
            {
                Time = "2:50PM",
                Channel = new(935399672988712973, "data-access"),
                Role = new(935400511773999104, "Admin"),
                User = new(728823685015797770, "ArchLeaders"),
                Sequence = "user(user_m)",
                Message = "@role $user needs to do somethig at $time_now"
            });

            tasks.Add(new(2)
            {
                Time = "3:25PM",
                Channel = new(935399672988712973, "data-access"),
                Role = new(935400511773999104, "Admin"),
                User = new(728823685015797770, "ArchLeaders"),
                Sequence = "user(user_m, user_a)",
                Message = "@role $user needs to do somethig at $time_now"
            });

            return tasks;
        }
    }
}
