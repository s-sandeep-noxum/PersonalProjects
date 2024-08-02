using System;
using System.Windows.Input;
using WorkItemCreator.Commands;

namespace WorkItemCreator.Data
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
				return openCommand ?? (openCommand = new CommandHandler(() => OpenWorkItem(), () => CanExecute));
			}
		}

		public string State { get; set; }
		public string Tags { get; set; }
		public int WiNumber { get; set; }
		public string WiType { get; set; }
		private void OpenWorkItem()
		{
			throw new NotImplementedException();
		}
	}
}
