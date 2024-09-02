using ResponsiveWorkManager.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace ResponsiveWorkManager.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		private ViewModelBase selectedWindow;

		private ICommand menuItemClick;

		public MainWindowViewModel()
		{

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

		private void CreateWorkItemObject(object windowName)
		{
			if(windowName.ToString() == "WorkItems")
			{
				SelectedWindow = new WorkItemsViewModel();
			}
			else if (windowName.ToString() == "Folders")
			{
				SelectedWindow = new FoldersViewModel();
			}
			else if (windowName.ToString() == "LeaveDetails")
			{
				//SelectedWindow = new LeaveDetailsViewModel();
			}			
		}
		
	}
}
