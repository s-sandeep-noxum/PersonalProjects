using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WorkManager.ViewModel
{
	public class LeaveDetailsViewModel : INotifyPropertyChanged
	{
		private DateTime date;

		private string typeOfLeave;

		public event PropertyChangedEventHandler PropertyChanged;

		public LeaveDetailsViewModel()
		{
			Date = DateTime.Now;			
		}
		public DateTime Date
		{
			get { return date; }
			set
			{
				if (date != value)
				{
					date = value;
					OnPropertyChanged(nameof(Date));
				}
			}
		}

		public string TypeOfLeave
		{
			get
			{
				return typeOfLeave;
			}
			set
			{
				if (typeOfLeave != value)
				{
					typeOfLeave = value;
					OnPropertyChanged(nameof(TypeOfLeave));
				}
			}
		}

		public List<string> TypeOfLeaves => new List<string>
						{
							"Casual Leave",
							"Sick Leave",
							"Optional Leave"
						};
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
