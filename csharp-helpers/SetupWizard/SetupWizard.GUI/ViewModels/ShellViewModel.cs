using SetupWizard.GUI.Models;
using SetupWizard.GUI.ViewResources.Helpers;
using Stylet;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

namespace SetupWizard.GUI.ViewModels
{
    public class ShellViewModel : Screen
    {
        /// 
        /// App parameters
        /// 
        public static int MinHeight { get; set; } = 400;
        public static int MinWidth { get; set; } = 600;
        public static bool CanResize { get; set; } = true;
        public string HelpLink { get; set; } = "https://github.com/ArchLeaders/ComputerStudies-TaskTracker";
        public string Title { get; set; } = "Task Tracker - Setup Wizard";

        // Error report settings
        public static bool UseGitHubUpload { get; set; } = true;
        public string GitHubRepo { get; set; } = "computerstudies-tasktracker";
        public string DiscordInvite { get; set; } = "https://github.com/ArchLeaders/ComputerStudies-TaskTracker"; // N/A
        public ulong DiscordReportChannel { get; set; } = 0;

        ///
        /// Actions
        ///
        #region Actions

        public void Edit()
        {
            if (IsTasks && SelectedTask != null)
            {
                // This is very stupid; fix it!!
                TaskViewModel = new()
                {
                    Key = SelectedTask.Key,
                    Time = SelectedTask.Time,
                    DateTime = SelectedTask.DateTime,
                    Mon = SelectedTask.Mon,
                    Tue = SelectedTask.Tue,
                    Wed = SelectedTask.Wed,
                    Thu = SelectedTask.Thu,
                    Fri = SelectedTask.Fri,
                    Channel = SelectedTask.Channel,
                    Role = SelectedTask.Role,
                    User = SelectedTask.User,
                    Sequence = SelectedTask.Sequence,
                    Message = SelectedTask.Message,
                    Channels = SelectedTask.Channels,
                    Roles = SelectedTask.Roles,
                    Users = SelectedTask.Users
                };

                TempTask = SelectedTask;
            }
            else if (IsVars && SelectedVar != null)
            {
                // So is this, but it's 6 billian parameters
                VarViewModel = new()
                {
                    Key = SelectedVar.Key,
                    Value = SelectedVar.Value
                };

                TempVar = SelectedVar;
            }
            else return;

            IsAddingItem = false;
            ItemEditVis = Visibility.Visible;
            ItemDataVis = Visibility.Collapsed;
        }

        public void Remove()
        {
            if (WindowManager == null)
            {
                // This should never be reached
                MessageBox.Show("Could not load WindowManager from ShellViewModel.\nTry restarting the application.", "Error");
                return;
            }

            if (SelectedTask == null && SelectedVar == null)
                return;

            if (!WindowManager.Show("Are you sure you wish to delete the selected item?", "Warning", true))
                return;

            if (IsTasks)
            {
                if (SelectedTask != null)
                    Tasks.Remove(SelectedTask);
            }
            else
            {
                if (SelectedVar != null)
                    Vars.Remove(SelectedVar);
            }
        }

        public void Add()
        {
            if (IsTasks)
            {
                TaskViewModel = new(Tasks.Count);
                TempTask = new(Tasks.Count);
            }
            else if (IsVars)
            {
                VarViewModel = new();
                TempVar = new();
            }
            else return;

            IsAddingItem = true;
            ItemEditVis = Visibility.Visible;
            ItemDataVis = Visibility.Collapsed;
        }

        public void ChangeView()
        {
            if (IsVars)
            {
                VarsVis = Visibility.Visible;
                TasksVis = Visibility.Collapsed;
            }
            else
            {
                VarsVis = Visibility.Collapsed;
                TasksVis = Visibility.Visible;
            }
        }

        public void Save()
        {
            if (VarViewModel != null)
            {
                if (VarViewModel.Key == "" ||
                    VarViewModel.Value == "")
                {

                    if (WindowManager != null)
                        WindowManager.Show("Could not save changes. Make sure that the **Key** and **Value** are both set.", "Notice");
                    else
                        MessageBox.Show("Could not save changes. Make sure that the Key and Value are both set.", "Notice");

                    return;
                }

                for (int i = 0; i < Vars.Count; i++)
                {
                    if (Vars[i].Key == VarViewModel.Key)
                    {
                        if (WindowManager == null)
                        {
                            // This should never be reached
                            MessageBox.Show("Could not load WindowManager from ShellViewModel.\nTry restarting the application.", "Error");
                            return;
                        }

                        if (IsAddingItem)
                            if (!WindowManager.Show($"A variable with the key '{VarViewModel.Key}' already exists. Overwrite it?", "Warning", true))
                                return;

                        Vars[i] = VarViewModel;
                        Cleanup();
                        return;
                    }
                }

                Vars.Add(VarViewModel);
                Cleanup();

                void Cleanup()
                {
                    VarViewModel = null;
                    ItemEditVis = Visibility.Collapsed;
                    ItemDataVis = Visibility.Visible;
                    SelectedVar = VarViewModel;
                }
            }
            else if (TaskViewModel != null)
            {
                if (TaskViewModel.Time == "" ||
                    TaskViewModel.Message == "" ||
                    TaskViewModel.Channel.Key == 0 ||
                    TaskViewModel.Key == "") {

                    if (WindowManager != null)
                        WindowManager.Show("Could not save changes. Make sure that **Time**, **Message**, and **Channel** are all set to valid values.", "Notice", width: 300);
                    else
                        MessageBox.Show("Could not save changes. Make sure that Time, Message, and Channel are all set to valid values.", "Notice");

                    return;
                }

                ItemDataVis = Visibility.Visible;
                ItemEditVis = Visibility.Collapsed;
                TaskViewModel.Time = TaskViewModel.DateTime.ToString("h:mm tt");

                for (int i = 0; i < Tasks.Count; i++)
                {
                    if (Tasks[i].Key == TaskViewModel.Key)
                    {
                        Tasks[i] = TaskViewModel;
                        TaskViewModel = null;
                        return;
                    }
                }

                Tasks.Add(TaskViewModel);
                SelectedTask = TaskViewModel;
                TaskViewModel = null;
            }

            if (SettingsViewModel != null)
                SettingsViewModel.Save();
        }

        public void Cancel()
        {
            if (WindowManager == null)
            {
                // This should never be reached
                MessageBox.Show("Could not load WindowManager from ShellViewModel.\nTry restarting the application.", "Error");
                return;
            }

            if (!WindowManager.Show($"Discard changes? This connot be undone.", "Warning", true))
                return;

            TaskViewModel = null;
            VarViewModel = null;
            ItemEditVis = Visibility.Collapsed;
            ItemDataVis = Visibility.Visible;
            return;
        }

        public async void Sync(string command = "Fetch")
        {
            if (command == "WarnUser")
            {
                if (WindowManager == null)
                {
                    // This should never be reached
                    MessageBox.Show("Could not load WindowManager from ShellViewModel.\nTry restarting the application.", "Error");
                    return;
                }

                if (!WindowManager.Show($"Syncing will delete any changes you have made locally.\nThis **cannot** be undone!\n\nProceed anyway?", "Warning", true, width: 300))
                    return;
            }

            else if (command == "Start")
            {
                // do something
            }

            Server server;
            Syncing syncing = new();

            server = await syncing.Fetch(SettingsViewModel.ServerID);
            ShellViewModel mirror = server.GetShellBase();

            Tasks = mirror.Tasks;
            Vars = mirror.Vars;

            if (TaskViewModel != null)
            {
                TaskViewModel.Channels = TaskModel.Channels;
                TaskViewModel.Roles = TaskModel.Roles;
                TaskViewModel.Users = TaskModel.Users;
            }

            SettingsViewModel.Timezone = server.Timezone;
        }

        #endregion

        ///
        /// Properties
        ///
        #region Properties

        // Misc

        public bool IsAddingItem { get; set; } = false;
        public TaskViewModel? TempTask { get; set; } = null;
        public VarViewModel? TempVar { get; set; } = null;

        // Tasks

        private BindableCollection<TaskViewModel> _tasks = new();
        public BindableCollection<TaskViewModel> Tasks
        {
            get => _tasks;
            set => SetAndNotify(ref _tasks, value);
        }

        private TaskViewModel? _selectedTask;
        public TaskViewModel? SelectedTask
        {
            get => _selectedTask;
            set => SetAndNotify(ref _selectedTask, value);
        }

        private Visibility _tasksVis = Visibility.Visible;
        public Visibility TasksVis
        {
            get => _tasksVis;
            set => SetAndNotify(ref _tasksVis, value);
        }

        private bool _isTasks = true;
        public bool IsTasks
        {
            get => _isTasks;
            set => SetAndNotify(ref _isTasks, value);
        }

        // Vars

        private BindableCollection<VarViewModel> _vars = new();
        public BindableCollection<VarViewModel> Vars
        {
            get => _vars;
            set => SetAndNotify(ref _vars, value);
        }

        private VarViewModel? _selectedVar = null;
        public VarViewModel? SelectedVar
        {
            get => _selectedVar;
            set => SetAndNotify(ref _selectedVar, value);
        }

        private Visibility _varsVis = Visibility.Collapsed;
        public Visibility VarsVis
        {
            get => _varsVis;
            set => SetAndNotify(ref _varsVis, value);
        }

        private bool _isVars = false;
        public bool IsVars
        {
            get => _isVars;
            set => SetAndNotify(ref _isVars, value);
        }

        #endregion

        ///
        /// Bindings
        ///
        #region Bindings

        private Visibility _handledExceptionViewVisibility = Visibility.Collapsed;
        public Visibility HandledExceptionViewVisibility
        {
            get => _handledExceptionViewVisibility;
            set => SetAndNotify(ref _handledExceptionViewVisibility, value);
        }

        private Visibility _taskEditorVisibility = Visibility.Collapsed;
        public Visibility TaskEditorVisibility
        {
            get => _taskEditorVisibility;
            set => SetAndNotify(ref _taskEditorVisibility, value);
        }

        private Visibility _varsEditorVisibility = Visibility.Collapsed;
        public Visibility VarsEditorVisibility
        {
            get => _varsEditorVisibility;
            set => SetAndNotify(ref _varsEditorVisibility, value);
        }

        private Visibility _itemDataVis = Visibility.Visible;
        public Visibility ItemDataVis
        {
            get => _itemDataVis;
            set => SetAndNotify(ref _itemDataVis, value);
        }

        private Visibility _itemEditVis = Visibility.Collapsed;
        public Visibility ItemEditVis
        {
            get => _itemEditVis;
            set => SetAndNotify(ref _itemEditVis, value);
        }

        #endregion

        ///
        /// DataContext
        ///
        #region DataContext

        // Views
        public SettingsViewModel SettingsViewModel { get; set; }

        private HandledExceptionViewModel? _handledExceptionViewModel = null;
        public HandledExceptionViewModel? HandledExceptionViewModel
        {
            get => _handledExceptionViewModel;
            set => SetAndNotify(ref _handledExceptionViewModel, value);
        }

        private TaskViewModel? _taskViewModel = null;
        public TaskViewModel? TaskViewModel
        {
            get => _taskViewModel;
            set => SetAndNotify(ref _taskViewModel, value);
        }

        private VarViewModel? _varViewModel = null;
        public VarViewModel? VarViewModel
        {
            get => _varViewModel;
            set => SetAndNotify(ref _varViewModel, value);
        }

        // App
        public bool CanFullscreen { get; set; } = CanResize;
        public ResizeMode ResizeMode { get; set; } = CanResize ? ResizeMode.CanResize : ResizeMode.CanMinimize;
        public WindowStyle WindowStyle { get; set; } = CanResize ? WindowStyle.None : WindowStyle.SingleBorderWindow;

        public void ThrowException(HandledExceptionViewModel ex)
        {
            if (WindowManager != null)
            {
                WindowManager.Error(ex.Message, ex.StackText, ex.Title);
                HandledExceptionViewModel = ex;
                HandledExceptionViewVisibility = Visibility.Visible;
            }
        }

        public void Help()
        {
            Process proc = new();

            proc.StartInfo.FileName = "explorer.exe";
            proc.StartInfo.Arguments = HelpLink;

            proc.Start();
        }

        public IWindowManager? WindowManager { get; set; }
        public ShellViewModel(IWindowManager? windowManager)
        {
            WindowManager = windowManager;
            SettingsViewModel = new(this);

            if (File.Exists($"{Environment.GetEnvironmentVariable("LOCALAPPDATA")}\\TaskTracker.server"))
                SettingsViewModel.ServerID = ulong.Parse(File.ReadAllText($"{Environment.GetEnvironmentVariable("LOCALAPPDATA")}\\TaskTracker.server"));

            Debug.WriteLine("Initialized Shell");
        }

        ///
        /// Root Error handling
        /// 
        public void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception as Exception ?? new();
            File.WriteAllText("error.log.txt", $"[{DateTime.Now}]\n- {ex.Message}\n[Stack Trace]\n{ex.StackTrace}\n- - - - - - - - - - - - - - -\n\n");
            ThrowException(new(this, "Unhandled Exception", ex.Message, ex.StackTrace ?? "", true));
            Environment.Exit(0);
        }

        public void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception ?? new();
            File.WriteAllText("error.log.txt", $"[{DateTime.Now}]\n- {ex.Message}\n[Stack Trace]\n{ex.StackTrace}\n- - - - - - - - - - - - - - -\n\n");
            ThrowException(new(this, "Unhandled Exception", ex.Message, ex.StackTrace ?? "", true));
            Environment.Exit(0);
        }

        #endregion
    }
}
