using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetupWizard.GUI.ViewModels
{
    public class TaskViewModel : Screen
    {
        public string Key { get; set; } = string.Format("0x{0:X}", 0.ToString("X6"));

        #region Bindings

        private TimeOnly _time = TimeOnly.Parse("12:00PM");
        public TimeOnly Time
        {
            get => _time;
            set
            {
                DateTime = DateTime.Parse(value.ToString());
                SetAndNotify(ref _time, value);
            }
        }

        private DateTime _dateTime = DateTime.Parse("12:00PM");
        public DateTime DateTime
        {
            get => _dateTime;
            set => SetAndNotify(ref _dateTime, value);
        }

        private bool _mon = true;
        public bool Mon
        {
            get => _mon;
            set => SetAndNotify(ref _mon, value);
        }

        private bool _tue = true;
        public bool Tue
        {
            get => _tue;
            set => SetAndNotify(ref _tue, value);
        }

        private bool _wed = true;
        public bool Wed
        {
            get => _wed;
            set => SetAndNotify(ref _wed, value);
        }

        private bool _thu = true;
        public bool Thu
        {
            get => _thu;
            set => SetAndNotify(ref _thu, value);
        }

        private bool _fri = true;
        public bool Fri
        {
            get => _fri;
            set => SetAndNotify(ref _fri, value);
        }

        private bool _sat = false;
        public bool Sat
        {
            get => _sat;
            set => SetAndNotify(ref _sat, value);
        }

        private bool _sun = false;
        public bool Sun
        {
            get => _sun;
            set => SetAndNotify(ref _sun, value);
        }

        private KeyValuePair<uint, string> _channel;
        public KeyValuePair<uint, string> Channel
        {
            get => _channel;
            set => SetAndNotify(ref _channel, value);
        }

        private KeyValuePair<uint, string> _role;
        public KeyValuePair<uint, string> Role
        {
            get => _role;
            set => SetAndNotify(ref _role, value);
        }

        private KeyValuePair<uint, string> _user;
        public KeyValuePair<uint, string> User
        {
            get => _user;
            set => SetAndNotify(ref _user, value);
        }

        private string _sequence = "user(user_0, user_1);";
        public string Sequence
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

        #endregion

        #region Server Data

        private BindableCollection<KeyValuePair<uint, string>> _channels = new()
        {
            { new(0001, "data-access") },
            { new(0002, "system") },
            { new(0003, "chat") },
        };
        public BindableCollection<KeyValuePair<uint, string>> Channels
        {
            get => _channels;
            set => SetAndNotify(ref _channels, value);
        }

        private BindableCollection<KeyValuePair<uint, string>> _roles = new();
        public BindableCollection<KeyValuePair<uint, string>> Roles
        {
            get => _roles;
            set => SetAndNotify(ref _roles, value);
        }

        private BindableCollection<KeyValuePair<uint, string>> _users = new();
        public BindableCollection<KeyValuePair<uint, string>> Users
        {
            get => _users;
            set => SetAndNotify(ref _users, value);
        }

        public void Sync()
        {
            Dictionary<string, Dictionary<uint, string>> serverMetaData = new();

            // Fetch server data

            Channels = new();
            Roles = new();
            Users = new();

            foreach (var data in serverMetaData["channels"])
                Channels.Add(new(data.Key, data.Value));

            foreach (var data in serverMetaData["roles"])
                Roles.Add(new(data.Key, data.Value));

            foreach (var data in serverMetaData["users"])
                Users.Add(new(data.Key, data.Value));
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
        }

        #endregion
    }
}
