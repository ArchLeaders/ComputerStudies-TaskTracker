using SetupWizard.GUI.ViewResources.Helpers;
using Stylet;
using System;
using System.Collections.Generic;
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
            if (IsTasks)
            {
                MessageBox.Show(SelectedTask.Time.ToString());
            }
            else
            {
                MessageBox.Show(SelectedVar.Value);
            }
        }

        public void Remove()
        {

        }

        public void Add()
        {

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

        }

        #endregion

        ///
        /// Properties
        ///
        #region Properties

        // Tasks

        private BindableCollection<TasksViewModel> _tasks;
        public BindableCollection<TasksViewModel> Tasks
        {
            get => _tasks;
            set => SetAndNotify(ref _tasks, value);
        }

        private TasksViewModel _selectedTask = new();
        public TasksViewModel SelectedTask
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

        private BindableCollection<KeyValuePair<string, string>> _vars = new();
        public BindableCollection<KeyValuePair<string, string>> Vars
        {
            get => _vars;
            set => SetAndNotify(ref _vars, value);
        }

        private KeyValuePair<string, string> _selectedVar = new();
        public KeyValuePair<string, string> SelectedVar
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

        private Visibility _itemDataVis = Visibility.Visible;
        public Visibility ItemDataVis
        {
            get => _itemDataVis;
            set => SetAndNotify(ref _itemDataVis, value);
        }


        #endregion

        ///
        /// DataContext
        ///
        #region DataContext

        // Views
        public SettingsViewModel? SettingsViewModel { get; set; } = null;

        private HandledExceptionViewModel? _handledExceptionViewModel = null;
        public HandledExceptionViewModel? HandledExceptionViewModel
        {
            get => _handledExceptionViewModel;
            set => SetAndNotify(ref _handledExceptionViewModel, value);
        }

        // App
        public bool CanFullscreen { get; set; } = CanResize;
        public ResizeMode ResizeMode { get; set; } = CanResize ? ResizeMode.CanResize : ResizeMode.CanMinimize;
        public WindowStyle WindowStyle { get; set; } = CanResize ? WindowStyle.None : WindowStyle.SingleBorderWindow;

        public void ThrowException(HandledExceptionViewModel ex)
        {
            WindowManager.Error(ex.Message, ex.StackText, ex.Title);
            HandledExceptionViewModel = ex;
            HandledExceptionViewVisibility = Visibility.Visible;
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
            Tasks = RanDataAccess.Get();

            Vars.Add(new("user_m", "Marcus"));
            Vars.Add(new("user_a", "Alex"));
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
