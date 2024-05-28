using BindingTesting.ViewModel;
using System.Windows;

namespace BindingTesting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {            
            this.DataContext = new MainWindowModel();    
        }
    }
}