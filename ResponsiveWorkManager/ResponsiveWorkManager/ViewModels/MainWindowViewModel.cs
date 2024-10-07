using ResponsiveWorkManager.Commands;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace ResponsiveWorkManager.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		private ViewModelBase selectedWindow;

		private ICommand menuItemClick;
		private ICommand logoutClick;
		private string versionNumber;

		public string VersionNumber
		{
			get { return versionNumber; }
			set
			{
				if (versionNumber != value)
				{
					versionNumber = value;
					OnPropertyChanged("VersionNumber");
				}
			}
		}


		public MainWindowViewModel()
		{
			this.selectedWindow = new WorkItemsViewModel();
			this.VersionNumber = $"Ver - {Assembly.GetEntryAssembly().GetName().Version.ToString()}";
		}
		public ViewModelBase SelectedWindow
		{
			get { return selectedWindow; }
			set
			{
				if (selectedWindow != value)
				{
					selectedWindow = value;
					OnPropertyChanged("SelectedWindow");
				}
			}
		}
		public ICommand MenuItemClick
		{
			get
			{
				return menuItemClick ?? (menuItemClick = new RelayCommand(CreateWorkItemObject));
			}
		}

		public ICommand LogoutClick
		{
			get
			{
				return logoutClick ?? (logoutClick = new RelayCommand(CloseWindow));
			}
		}

		private void CloseWindow(object winHandle)
		{
			var myWindow = (Window)winHandle;
			myWindow.Close();
		}

		private void CreateWorkItemObject(object windowName)
		{
			switch ((WindowName)windowName)
			{
				case WindowName.WorkItems:
					SelectedWindow = new WorkItemsViewModel();
					break;
				case WindowName.Folders:
					SelectedWindow = new FoldersViewModel();
					break;
				case WindowName.LeaveDetails:
					SelectedWindow = new LeaveDetailsViewModel();
					break;
			}
		}

	}
}
