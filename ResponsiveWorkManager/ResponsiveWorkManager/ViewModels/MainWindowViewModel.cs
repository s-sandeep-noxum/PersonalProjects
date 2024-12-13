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
		private StatusBarViewModel statusBarVM;

		public StatusBarViewModel StatusBarVM
		{
			get { return statusBarVM; }
			set 
			{
				if (statusBarVM != value)
				{
					statusBarVM = value;
					OnPropertyChanged("StatusBarVM");
				}				
			}
		}


		public MainWindowViewModel()
		{
			this.StatusBarVM = new StatusBarViewModel();
			this.selectedWindow = new WorkItemsViewModel(this.StatusBarVM);
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
					SelectedWindow = new WorkItemsViewModel(this.StatusBarVM);
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
