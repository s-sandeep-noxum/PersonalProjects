using ResponsiveWorkManager.Commands;
using ResponsiveWorkManager.Connection;
using ResponsiveWorkManager.DataObjects;
using System.Windows;
using System.Windows.Input;

namespace ResponsiveWorkManager.ViewModels
{
	public class WorkItemsViewModel : ViewModelBase
	{
		private Queries currentQuery = new Queries();
		private List<Projects> projects;
		private List<Queries> queries;
		private ICommand searchClick;
		private ICommand advancedSearchClick;
		private Projects selectedProject = new Projects();
		private DataObjects.WorkItemDetails workItemDetail;

		private bool isAdvancedSearchVisible;

		public bool IsAdvancedSearchVisible
		{
			get { return isAdvancedSearchVisible; }
			set 
			{ 
				if(isAdvancedSearchVisible != value)
				{
					isAdvancedSearchVisible = value;
					OnPropertyChanged(nameof(IsAdvancedSearchVisible));
				}
			}
		}

		public WorkItemsViewModel()
		{
			this.IsAdvancedSearchVisible = false;
		}

		public bool CanExecute
		{
			get
			{
				// check if executing is allowed, i.e., validate, check if a process is running, etc.
				return true;
			}
		}

		public Queries CurrentQuery
		{
			get
			{
				return currentQuery;
			}
			set
			{
				if (currentQuery.QueryText == value?.QueryText) return;
				currentQuery.QueryText = value?.QueryText;
				OnPropertyChanged(nameof(CurrentQuery));
			}
		}

		public List<Projects> Projects
		{
			get
			{
				if (this.projects == null)
				{
					this.projects = new List<Projects>();
					List<string> projectsWithAccess = VSSConnection.GetProjects();

					foreach (string project in projectsWithAccess)
					{
						this.projects.Add(new Projects { ProjectText = project });
					}
				}
				return this.projects.OrderBy(x => x.ProjectText).ToList();
			}
		}

		public List<Queries> Queries
		{
			get
			{
				return queries;
			}

			set
			{
				if (this.queries != value)
				{
					this.queries = value;
					OnPropertyChanged(nameof(Queries));
				}
			}
		}
		public DataObjects.WorkItemDetails WorkItemDetail
		{
			get
			{
				return workItemDetail;
			}
			set
			{
				workItemDetail = value;
			}
		}

		public ICommand SearchClick
		{
			get
			{
				return searchClick ?? (searchClick = new CommandHandler(() => SearchAndFillWI(), () => CanExecute));
			}
		}
		public ICommand AdvancedSearchClick
		{
			get
			{
				return advancedSearchClick ?? (advancedSearchClick = new CommandHandler(() => EnableDisableAdvancedSearch(), () => CanExecute));
			}
		}

		private void EnableDisableAdvancedSearch()
		{
			if(IsAdvancedSearchVisible)
			{
				IsAdvancedSearchVisible = false;
			}
			else
			{
				IsAdvancedSearchVisible = true;
			}
		}

		public Projects SelectedProject
		{
			get { return this.selectedProject; }
			set
			{
				if (this.selectedProject.ProjectText != value.ProjectText)
				{
					this.selectedProject.ProjectText = value.ProjectText;
					this.Queries = VSSConnection.GetQueries(selectedProject.ProjectText);
					OnPropertyChanged(nameof(SelectedProject));
				}
			}
		}

		private List<DataObjects.WorkItemDetails> wiDetails;

		public List<DataObjects.WorkItemDetails> WiDetails
		{
			get
			{
				this.wiDetails = VSSConnection.GetWorkItems(CurrentQuery?.QueryText, SelectedProject?.ProjectText);
				return this.wiDetails;
			}
			set
			{
				if (this.wiDetails == value) return;
				this.wiDetails = value;
				OnPropertyChanged(nameof(WiDetails));
			}
		}
		public void SearchAndFillWI()
		{
			try
			{
				Common.CommonHelper.WaitCursor();

				if (string.IsNullOrEmpty(CurrentQuery.QueryText)) return;

				var workItems = VSSConnection.GetWorkItems(CurrentQuery.QueryText, SelectedProject.ProjectText);

				if (workItems == null || workItems.Count == 0)
				{
					WiDetails = null;
					MessageBox.Show("Search did not return any result!!");
				}
				else
				{
					WiDetails = workItems;
				}
			}
			finally
			{
				Common.CommonHelper.NormalCursor();
			}
		}
	}
}
