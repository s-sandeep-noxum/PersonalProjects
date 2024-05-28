using System.ComponentModel;

namespace BindingTesting.ViewModel
{
    public class InsideViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        private string _name = "Random Name";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }
        public bool IsNameEnabled { get; set; } = false;
    }
}
