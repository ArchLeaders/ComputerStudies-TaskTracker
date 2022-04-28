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

        private BindableCollection<string> _days = new();
        public BindableCollection<string> Days
        {
            get => _days;
            set => SetAndNotify(ref _days, value);
        }

        private string _channel = "";
        public string Channel
        {
            get => _channel;
            set => SetAndNotify(ref _channel, value);
        }

        private string _role = "";
        public string Role
        {
            get => _role;
            set => SetAndNotify(ref _role, value);
        }

        private string _user = "";
        public string User
        {
            get => _user;
            set => SetAndNotify(ref _user, value);
        }

        private uint _channelId = 0;
        public uint ChannelId
        {
            get => _channelId;
            set => SetAndNotify(ref _channelId, value);
        }

        private uint _roleId = 0;
        public uint RoleId
        {
            get => _roleId;
            set => SetAndNotify(ref _roleId, value);
        }

        private uint _userId = 0;
        public uint UserId
        {
            get => _userId;
            set => SetAndNotify(ref _userId, value);
        }

        private BindableCollection<string> _sequence = new();
        public BindableCollection<string> Sequence
        {
            get => _sequence;
            set => SetAndNotify(ref _sequence, value);
        }

        private string _message = "";
        public string Message
        {
            get => _message;
            set => SetAndNotify(ref _message, value);
        }

        private BindableCollection<uint> _channels = new();
        public BindableCollection<uint> Channels
        {
            get => _channels;
            set => SetAndNotify(ref _channels, value);
        }

        private BindableCollection<uint> _roles = new();
        public BindableCollection<uint> Roles
        {
            get => _roles;
            set => SetAndNotify(ref _roles, value);
        }

        private BindableCollection<uint> _users = new();
        public BindableCollection<uint> Users
        {
            get => _users;
            set => SetAndNotify(ref _users, value);
        }
    }
}
