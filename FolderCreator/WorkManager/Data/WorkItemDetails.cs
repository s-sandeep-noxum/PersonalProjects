using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using WorkManager.Commands;

namespace WorkManager.Data
{
	public class WorkItemDetails
	{
		private ICommand openCommand;

		public WorkItemDetails(string description, int wiNumber, string state, string assignedTo, string tags, string wiType)
		{
			Description = description;
			WiNumber = wiNumber;
			State = state;
			AssignedTo = assignedTo;
			Tags = tags;
			WiType = wiType;
		}

		public WorkItemDetails()
		{
		}

		public string AssignedTo { get; set; }

		public bool CanExecute
		{
			get
			{
				// check if executing is allowed, i.e., validate, check if a process is running, etc.
				return true;
			}
		}

		public string Description { get; set; }

		public ICommand OpenCommand
		{
			get
			{
				return openCommand ?? (openCommand = new RelayCommand(OpenWorkItem));
			}
		}

		public string State { get; set; }
		public string Tags { get; set; }
		public int WiNumber { get; set; }
		public string WiType { get; set; }

		private void OpenWorkItem(object selectedProject)
		{

			if (selectedProject is string)
			{
				StringBuilder url = new StringBuilder { Length = 0 };
				url.Append(@"https://noxum.visualstudio.com");
				url.Append(@"/");
				url.Append(selectedProject);
				url.Append(@"/");
				url.Append("_workitems/edit");
				url.Append(@"/");
				url.Append(WiNumber);

				System.Diagnostics.Process.Start(new ProcessStartInfo
				{
					FileName = url.ToString(),
					UseShellExecute = true
				});
			}
		}
	}
}