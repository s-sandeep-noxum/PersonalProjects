using System.ComponentModel;

namespace BindingTesting.ViewModel
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public InsideViewModel? ViewModel { get; set; }


        public MainWindowModel()
        {
            ViewModel = new InsideViewModel { Name = "Random Name", IsNameEnabled = false};
        }

        public InsideViewModel? GetInsideViewModel() => ViewModel;
    }    
}
