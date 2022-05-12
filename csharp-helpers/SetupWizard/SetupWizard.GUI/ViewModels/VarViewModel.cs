using Stylet;

namespace SetupWizard.GUI.ViewModels
{
    public class VarViewModel : Screen
    {
        private string _key = "placeholder_key";
        public string Key
        {
            get => _key;
            set => SetAndNotify(ref _key, value);
        }

        private string _value = "placeholder_value";
        public string Value
        {
            get => _value;
            set => SetAndNotify(ref _value, value);
        }

        public VarViewModel() { }
        public VarViewModel(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
