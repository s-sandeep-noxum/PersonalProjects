using FolderCreator.ViewModel;
using System.Windows;

namespace FolderCreator.View
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
        public MainWindowViewModel MainWindowViewModel { get; set; } = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainWindowViewModel;            

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MainWindowViewModel.FolderPath = @"C:\Users\Sandeep.shenoy\OneDrive - Noxum GmbH\Work Details";
        }
    }
}