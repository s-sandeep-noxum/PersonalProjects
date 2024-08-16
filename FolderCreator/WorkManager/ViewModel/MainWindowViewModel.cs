using IronSoftware.Drawing;
using IronWord;
using IronWord.Models;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using WorkManager.Commands;
using WorkManager.ConnectionItems;
using WorkManager.Data;

namespace WorkManager.ViewModel
{
	public class MainWindowViewModel : INotifyPropertyChanged, IDataErrorInfo
	{
		private List<CalendarYear> _yearData;

		private ICommand cancelClick;
		private Queries currentQuery = new Queries();
		private string description;
		private string folderPath;
		private List<Projects> projects;
		private List<Queries> queries;
		private ICommand saveClick;
		private ICommand searchClick;
		private Projects selectedProject = new Projects();
		private CalendarYear selectedYear;
		private string title;
		private List<WorkItemDetails> wiDetails;

		private object winHandle;
		private string wiNumber;

		private WorkItemDetails workItemDetails;

		public MainWindowViewModel()
		{
			this.FolderPath = @"C:\Users\Sandeep.shenoy\OneDrive - Noxum GmbH\Work Details";			
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ICommand CancelClick
		{
			get
			{
				return cancelClick ?? (cancelClick = new RelayCommand(CloseWindow));
			}
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

		public string Description
		{
			get { return description; }
			set
			{
				description = value;
				OnPropertyChanged(nameof(Description));
			}
		}

		public string Error
		{
			get
			{
				return null;
			}
		}

		public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();

		public string FolderPath
		{
			get
			{
				return folderPath;
			}
			set
			{
				if (folderPath == value) return;
				folderPath = value;
				OnPropertyChanged(nameof(FolderPath));
			}
		}

		public List<Projects> Projects
		{
			get
			{
				if (this.projects == null)
				{
					this.projects = new List<Projects>();
					ProjectHttpClient projectClient = ConnectionClass.Connection.GetClient<ProjectHttpClient>();
					IEnumerable<TeamProjectReference> projectsWithAccess = projectClient.GetProjects().Result;

					foreach (TeamProjectReference p in projectsWithAccess)
					{
						this.projects.Add(new Projects { ProjectText = p.Name });
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

		public ICommand SaveClick
		{
			get
			{
				return saveClick ?? (saveClick = new CommandHandler(() => SaveWindow(), () => CanExecute));
			}
		}

		public ICommand SearchClick
		{
			get
			{
				return searchClick ?? (searchClick = new CommandHandler(() => SearchAndFillWI(), () => CanExecute));
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
					this.Queries = this.GetQueries(selectedProject.ProjectText);
					OnPropertyChanged(nameof(SelectedProject));
				}
			}
		}

		public CalendarYear SelectedYear
		{
			get { return new CalendarYear { YearText = DateTime.Now.Year.ToString() }; }
			set
			{
				if (selectedYear?.YearText == value.YearText) return;
				selectedYear = new CalendarYear { YearText = value.YearText.ToString() };
				OnPropertyChanged(nameof(SelectedYear));
			}
		}
		public string Title
		{
			get { return this.title; }
			set
			{
				this.title = value;
				OnPropertyChanged(nameof(Title));
			}
		}

		public List<WorkItemDetails> WiDetails
		{
			get
			{
				this.wiDetails = GetWorkItems(CurrentQuery?.QueryText);
				return this.wiDetails;
			}
			set
			{
				if (this.wiDetails == value) return;
				this.wiDetails = value;
				OnPropertyChanged(nameof(WiDetails));
			}
		}
		public object WinHandle
		{
			get { return winHandle; }
			set
			{
				if (winHandle == value) return;
				winHandle = value;
				OnPropertyChanged(nameof(WinHandle));
			}
		}

		public string WiNumber
		{
			get { return wiNumber; }
			set
			{
				wiNumber = value;
				OnPropertyChanged(nameof(WiNumber));
			}
		}

		public WorkItemDetails WorkItemDetail
		{
			get
			{
				return workItemDetails;
			}
			set
			{
				workItemDetails = value;
			}
		}
		public List<CalendarYear> YearData
		{
			get { return LoadData(); }
			set
			{
				if (_yearData == value) return;
				_yearData = value;
				OnPropertyChanged(nameof(YearData));
			}
		}

		public string this[string objectName]
		{
			get
			{
				string error = string.Empty;

				switch (objectName)
				{
					case "WiNumber":
						int txtLength = 0;
						if (WiNumber != "" && WiNumber != null)
						{
							txtLength = WiNumber.ToString().Length;
						}
						if (txtLength != 6) error = "Work Item Number Should be 6 digits.";
						break;
				}

				if (error != string.Empty)
				{
					if (ErrorCollection.ContainsKey(objectName))
					{
						this.ErrorCollection.Remove(objectName);
					}
					this.ErrorCollection.Add(objectName, error);
				}
				else if (ErrorCollection.ContainsKey(objectName))
				{
					this.ErrorCollection.Remove(objectName);
				}

				OnPropertyChanged(nameof(ErrorCollection));
				return error;
			}
		}

		public void SearchAndFillWI()
		{
			try
			{
				Application.Current.Dispatcher.Invoke(() =>
				{
					Mouse.OverrideCursor = Cursors.Wait;
				});

				if (string.IsNullOrEmpty(CurrentQuery.QueryText)) return;
				var workItems = GetWorkItems(CurrentQuery.QueryText);
				if (workItems == null || workItems.Count == 0) { MessageBox.Show("Search did not return any result!!"); }
				else { WiDetails = workItems; }
			}
			finally
			{
				Application.Current.Dispatcher.Invoke(() =>
				{
					Mouse.OverrideCursor = null;
				});
			}
		}

		private static string GetAssignedTo(Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem workItem)
		{
			object identityOjbect;
			workItem.Fields.TryGetValue("System.AssignedTo", out identityOjbect);
			if (identityOjbect == null)
			{
				return "Unassigned";
			}

			return ((IdentityRef)identityOjbect).DisplayName;
		}

		private static WorkItemTrackingHttpClient GetMyQueriesFromProject(string ProjectName, out QueryHierarchyItem myQueriesFolder)
		{
			// Create instance of WorkItemTrackingHttpClient using VssConnection
			WorkItemTrackingHttpClient witClient = ConnectionClass.Connection.GetClient<WorkItemTrackingHttpClient>();

			// Get 2 levels of query hierarchy items
			List<QueryHierarchyItem> queryHierarchyItems = witClient.GetQueriesAsync(ProjectName, depth: 2).Result;

			// Search for 'My Queries' folder
			myQueriesFolder = queryHierarchyItems.FirstOrDefault(qhi => qhi.Name.Equals("My Queries"));

			return witClient;
		}

		private static async Task ShowWorkItemDetails(VssConnection connection, int workItemId)
		{
			// Get an instance of the work item tracking client
			WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

			try
			{
				// Get the specified work item
				WorkItem workitem = await witClient.GetWorkItemAsync(workItemId);

				// Output the work item's field values
				foreach (var field in workitem.Fields)
				{
					Console.WriteLine("  {0}: {1}", field.Key, field.Value);
				}
			}
			catch (AggregateException aex)
			{
				VssServiceException vssex = aex.InnerException as VssServiceException;
				if (vssex != null)
				{
					Console.WriteLine(vssex.Message);
				}
			}
		}

		private void CloseWindow(object winHandle)
		{
			var myWindow = (Window)winHandle;
			myWindow.Close();
		}

		private void CreateDescriptionFile(string fileName, string description)
		{
			IronWord.License.LicenseKey = "demo";
			WordDocument wordDocument = new WordDocument();

			string titleText = $"{WiNumber}:{Title}";
			IronWord.Models.TextRun titleRun = new IronWord.Models.TextRun(titleText);
			IronWord.Models.Paragraph title = new IronWord.Models.Paragraph();
			title.Style = new TextStyle()
			{
				FontFamily = "Verdana",
				FontSize = 36,
				TextColor = new IronColor(Color.Blue),
				IsBold = true,
				IsItalic = false,
				IsUnderline = true,
				IsSuperscript = false,
				IsStrikethrough = false,
				IsSubscript = false
			};
			title.AddTextRun(titleRun);

			IronWord.Models.TextRun bodyRun = new IronWord.Models.TextRun(description);
			IronWord.Models.Paragraph body = new IronWord.Models.Paragraph();
			body.AddTextRun(bodyRun);

			wordDocument.AddParagraph(title);
			wordDocument.AddParagraph(body);
			wordDocument.SaveAs(fileName);
		}

		private bool CreateFolders(ref string folderName)
		{
			try
			{
				folderName = FolderPath + "\\" + SelectedYear.YearText + "\\" + GetFolderName();
				bool exists = System.IO.Directory.Exists(folderName);

				if (!exists)
				{
					System.IO.Directory.CreateDirectory(folderName);
				}
				if (!string.IsNullOrEmpty(Description))
				{
					string FileName = folderName.Trim() + "\\" + "Description.docx";
					//CreateDescriptionFile(FileName, description);
					using (FileStream fs = File.Create(FileName))
					{
						Byte[] info = new UTF8Encoding(true).GetBytes(Description);
						// Add some information to the file.
						fs.Write(info, 0, info.Length);
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}
		}

		private string GetFolderName()
		{
			int wiLength = WiNumber.Length;
			int titleLength = Title.Length;

			if (wiLength + titleLength > 60)
			{
				return WiNumber + "-" + Title.Substring(0, titleLength - (wiLength + 1));
			}

			return WiNumber + "-" + Title;
		}

		private List<Queries> GetQueries(string ProjectName)
		{
			List<Queries> savedQueries = new List<Queries>();
			QueryHierarchyItem myQueriesFolder;
			GetMyQueriesFromProject(ProjectName, out myQueriesFolder);

			if (myQueriesFolder != null)
			{
				if (myQueriesFolder.Children != null)
				{
					foreach (var child in myQueriesFolder.Children.OrderBy(x => x.Name))
					{
						savedQueries.Add(new Queries { QueryText = child.Name });
					}
				}
			}
			if (savedQueries.Count > 0)
			{
				return savedQueries.OrderBy(x => x.QueryText).ToList();
			}
			return null;
		}

		private string GetTagValues(WorkItem workItem)
		{
			object identityOjbect;
			workItem.Fields.TryGetValue("System.Tags", out identityOjbect);

			if (identityOjbect == null)
			{
				return string.Empty;
			}

			return (string)identityOjbect;
		}

		private List<WorkItemDetails> GetWorkItems(string selectedQuery)
		{
			try
			{
				if (string.IsNullOrEmpty(selectedQuery)) return null;
				List<WorkItemDetails> workItemDetails = new List<WorkItemDetails>();

				QueryHierarchyItem myQueriesFolder;
				WorkItemTrackingHttpClient witClient = GetMyQueriesFromProject(this.SelectedProject.ProjectText, out myQueriesFolder);

				if (myQueriesFolder != null)
				{
					// See if our 'Open WI' query already exists under 'My Queries' folder.
					QueryHierarchyItem newBugsQuery = null;
					if (myQueriesFolder.Children != null)
					{
						newBugsQuery = myQueriesFolder.Children.FirstOrDefault(qhi => qhi.Name.Equals(selectedQuery));
					}
					// run the 'REST Sample' query
					WorkItemQueryResult result = witClient.QueryByIdAsync(newBugsQuery.Id).Result;

					if (result.WorkItems.Any())
					{
						int skip = 0;
						const int batchSize = 100;
						IEnumerable<WorkItemReference> workItemRefs;
						do
						{
							workItemRefs = result.WorkItems.Skip(skip).Take(batchSize);
							if (workItemRefs.Any())
							{
								// get details for each work item in the batch
								List<WorkItem> workItems = witClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id)).Result;
								foreach (var workItem in workItems)
								{
									workItemDetails.Add(new WorkItemDetails
									{
										WiNumber = (int)workItem.Id,
										Description = (string)workItem.Fields["System.Title"],
										State = (string)workItem.Fields["System.State"],
										AssignedTo = GetAssignedTo(workItem),
										Tags = GetTagValues(workItem),
										WiType = GetWorkItemType(workItem)
									});
								}
							}
							skip += batchSize;
						}
						while (workItemRefs.Count() == batchSize);

						return workItemDetails;
					}
					else
					{
						return null;
					}
				}

				return null;
			}
			catch (Exception ee)
			{
				MessageBox.Show(ee.StackTrace, "Error");
				return null;
			}
		}

		private string GetWorkItemType(WorkItem workItem)
		{
			object identityOjbect;
			workItem.Fields.TryGetValue("System.WorkItemType", out identityOjbect);

			if (identityOjbect == null)
			{
				return string.Empty;
			}

			return (string)identityOjbect;
		}

		private List<CalendarYear> LoadData()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(@"Data\YearData.xml");
			XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("Year");

			_yearData = new List<CalendarYear>();

			foreach (XmlNode node in xmlNodeList)
			{
				_yearData.Add(new CalendarYear { YearText = node.InnerText });
			}

			return _yearData;
		}

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private void SaveWindow()
		{
			if (ErrorCollection.Count > 0)
			{
				MessageBox.Show("Please correct the errors before saving the data.");
				return;
			}
			string folderPath = string.Empty;
			if (CreateFolders(ref folderPath))
			{
				if (MessageBox.Show("Do you want to open the created folder?", "Folder Created", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
				{
					System.Diagnostics.Process.Start("explorer.exe", folderPath);
				}
			}
		}
	}
}