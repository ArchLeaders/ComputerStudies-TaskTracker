using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetupWizard.GUI.ViewModels
{
    public class TasksViewModel : Screen
    {
        private bool _synced;
        public bool Synced
        {
            get => _synced;
            set => SetAndNotify(ref _synced, value);
        }

        private TimeOnly _time;
        public TimeOnly Time
        {
            get => _time;
            set => SetAndNotify(ref _time, value);
        }

        private BindableCollection<string> _days;
        public BindableCollection<string> Days
        {
            get => _days;
            set => SetAndNotify(ref _days, value);
        }

        private uint _channel;
        public uint Channel
        {
            get => _channel;
            set => SetAndNotify(ref _channel, value);
        }

        private uint _role;
        public uint Role
        {
            get => _role;
            set => SetAndNotify(ref _role, value);
        }

        private uint _user;
        public uint User
        {
            get => _user;
            set => SetAndNotify(ref _user, value);
        }

        private BindableCollection<string> _sequence;
        public BindableCollection<string> Sequence
        {
            get => _sequence;
            set => SetAndNotify(ref _sequence, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetAndNotify(ref _message, value);
        }

        private BindableCollection<uint> _channels;
        public BindableCollection<uint> Channels
        {
            get => _channels;
            set => SetAndNotify(ref _channels, value);
        }

        private BindableCollection<uint> _roles;
        public BindableCollection<uint> Roles
        {
            get => _roles;
            set => SetAndNotify(ref _roles, value);
        }

        private BindableCollection<uint> _users;
        public BindableCollection<uint> Users
        {
            get => _users;
            set => SetAndNotify(ref _users, value);
        }
    }
}
