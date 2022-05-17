using SetupWizard.GUI.Models;
using SetupWizard.GUI.ViewResources;
using SetupWizard.GUI.ViewResources.Helpers;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SetupWizard.GUI.ViewModels
{
    public class SettingsViewModel : Screen
    {
        public ShellViewModel ShellViewModel { get; set; }

        private ulong _serverId = 0;
        public ulong ServerID
        {
            get => _serverId;
            set => SetAndNotify(ref _serverId, value);
        }

        private string _timezone = TimeZoneInfo.Local.StandardName;
        public string Timezone
        {
            get => _timezone;
            set => SetAndNotify(ref _timezone, value);
        }

        private LockIcon _lockIcon = new();
        public LockIcon LockIcon
        {
            get => _lockIcon;
            set => SetAndNotify(ref _lockIcon, value);
        }

        public SettingsViewModel(ShellViewModel shell)
        {
            ShellViewModel = shell;
        }
    }
}
