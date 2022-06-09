using SetupWizard.GUI.Models;
using Stylet;
using System;
using System.Collections.Generic;

namespace SetupWizard.GUI.ViewModels
{
    public class TaskViewModel : Screen
    {
        public string Key { get; set; } = "";
        public Dictionary<string, int> Session { get; set; } = new();

        #region Bindings

        private string _time = "12:00 PM";
        public string Time {
            get => _time;
            set {
                DateTime = DateTime.Parse(value.ToString());
                SetAndNotify(ref _time, value);
            }
        }

        private DateTime _dateTime = DateTime.Parse("12:00 PM");
        public DateTime DateTime {
            get => _dateTime;
            set => SetAndNotify(ref _dateTime, value);
        }

        private bool _mon = true;
        public bool Mon {
            get => _mon;
            set => SetAndNotify(ref _mon, value);
        }

        private bool _tue = true;
        public bool Tue {
            get => _tue;
            set => SetAndNotify(ref _tue, value);
        }

        private bool _wed = true;
        public bool Wed {
            get => _wed;
            set => SetAndNotify(ref _wed, value);
        }

        private bool _thu = true;
        public bool Thu {
            get => _thu;
            set => SetAndNotify(ref _thu, value);
        }

        private bool _fri = true;
        public bool Fri {
            get => _fri;
            set => SetAndNotify(ref _fri, value);
        }

        private bool _sat = false;
        public bool Sat {
            get => _sat;
            set => SetAndNotify(ref _sat, value);
        }

        private bool _sun = false;
        public bool Sun {
            get => _sun;
            set => SetAndNotify(ref _sun, value);
        }

        private KeyValuePair<ulong, string> _channel;
        public KeyValuePair<ulong, string> Channel {
            get => _channel;
            set => SetAndNotify(ref _channel, value);
        }

        private KeyValuePair<ulong, string> _role;
        public KeyValuePair<ulong, string> Role {
            get => _role;
            set => SetAndNotify(ref _role, value);
        }

        private KeyValuePair<ulong, string> _user;
        public KeyValuePair<ulong, string> User {
            get => _user;
            set => SetAndNotify(ref _user, value);
        }

        private string? _sequence;
        public string? Sequence {
            get => _sequence;
            set => SetAndNotify(ref _sequence, value);
        }

        private string _message = "";
        public string Message {
            get => _message;
            set => SetAndNotify(ref _message, value);
        }

        #endregion

        #region Server Data

        private BindableCollection<KeyValuePair<ulong, string>> _channels = TaskModel.Channels;
        public BindableCollection<KeyValuePair<ulong, string>> Channels {
            get => _channels;
            set => SetAndNotify(ref _channels, value);
        }

        private BindableCollection<KeyValuePair<ulong, string>> _roles = TaskModel.Roles;
        public BindableCollection<KeyValuePair<ulong, string>> Roles {
            get => _roles;
            set => SetAndNotify(ref _roles, value);
        }

        private BindableCollection<KeyValuePair<ulong, string>> _users = TaskModel.Users;
        public BindableCollection<KeyValuePair<ulong, string>> Users {
            get => _users;
            set => SetAndNotify(ref _users, value);
        }

        #endregion

        #region Constructors

        public TaskViewModel()
        {
            DateTime = DateTime.Parse(Time.ToString());
        }

        public TaskViewModel(int key)
        {
            Key = string.Format("0x{0:X}", key.ToString("X6"));
            DateTime = DateTime.Parse(Time.ToString());
        }

        #endregion
    }
}
