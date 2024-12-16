using ResponsiveWorkManager.Commands;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace ResponsiveWorkManager.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		private ICommand logoutClick;
		private ICommand menuItemClick;
		private ViewModelBase selectedWindow;
		private StatusBarViewModel statusBarVM;
		private string versionNumber;

		public MainWindowViewModel()
		{			
			this.StatusBarVM = new StatusBarViewModel();
			this.selectedWindow = new WorkItemsViewModel(this.StatusBarVM);
			this.VersionNumber = $"Ver - {Assembly.GetEntryAssembly().GetName().Version.ToString()}";
		}

		public ICommand LogoutClick
		{
			get
			{
				return logoutClick ?? (logoutClick = new RelayCommand(CloseWindow));
			}
		}

		public ICommand MenuItemClick
		{
			get
			{
				return menuItemClick ?? (menuItemClick = new RelayCommand(CreateWorkItemObject));
			}
		}

		public ViewModelBase SelectedWindow
		{
			get { return selectedWindow; }
			set
			{
				if (selectedWindow != value)
				{
					selectedWindow = value;
					OnPropertyChanged(nameof(SelectedWindow));
				}
			}
		}	

		public StatusBarViewModel StatusBarVM
		{
			get { return statusBarVM; }
			set 
			{
				if (statusBarVM != value)
				{
					statusBarVM = value;
					OnPropertyChanged(nameof(StatusBarVM));
				}
			}
		}

		public string VersionNumber
		{
			get { return versionNumber; }
			set
			{
				if (versionNumber != value)
				{
					versionNumber = value;
					OnPropertyChanged(nameof(versionNumber));
				}
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