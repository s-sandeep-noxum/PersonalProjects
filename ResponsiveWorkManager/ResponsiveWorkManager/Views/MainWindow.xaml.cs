using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ResponsiveWorkManager.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if(e.ChangedButton == MouseButton.Left)
			{
				this.DragMove();
			}
		}

		private bool IsMaximized = false;
		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if(e.ClickCount == 2)
			{
				if (IsMaximized)
				{
					WindowState = WindowState.Normal;
					this.Width = 1080;
					this.Height = 720;
					IsMaximized = false;
				}
				else
				{
					WindowState = WindowState.Maximized;
					IsMaximized = true;
				}
			}
		}
	}
}