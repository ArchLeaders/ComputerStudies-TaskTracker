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
        public void Save()
        {

        }

        private ShellViewModel ShellViewModel { get; set; }

        private ulong _serverId = 0;
        public ulong ServerID
        {
            get => _serverId;
            set => SetAndNotify(ref _serverId, value);
        }

        private bool _autoSync = true;
        public bool AutoSync
        {
            get => _autoSync;
            set => SetAndNotify(ref _autoSync, value);
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
