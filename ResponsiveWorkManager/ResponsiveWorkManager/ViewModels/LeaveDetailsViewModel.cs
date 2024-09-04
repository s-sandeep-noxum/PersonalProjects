using ResponsiveWorkManager.Commands;
using ResponsiveWorkManager.Connection;
using ResponsiveWorkManager.DataObjects;
using System.Windows;
using System.Windows.Input;

namespace ResponsiveWorkManager.ViewModels
{
	public class LeaveDetailsViewModel : ViewModelBase
	{
		private static DbConnectionContext dbContext = new DbConnectionContext();
		private string comment;
		private DateTime date;
		private string dayType;
		private List<LeaveDetails> leaveDetails;
		private ICommand saveLeaveClick;
		private ICommand refreshClick;
		private LeaveDetails selectedLeaveDetail;
		private string typeOfLeave;
		public LeaveDetailsViewModel()
		{
			ResetFields();
			if (!dbContext.Database.EnsureCreated())
			{
				FillLeaveDetails();
			}
		}		
		public string Comment
		{
			get { return comment; }
			set
			{
				if (comment != value)
				{
					comment = value;
					OnPropertyChanged(nameof(Comment));
				}
			}
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

		public string DayType
		{
			get
			{
				return dayType;
			}
			set
			{
				if (dayType != value)
				{
					dayType = value;
					OnPropertyChanged(nameof(DayType));
				}
			}
		}

		public List<string> DayTypes => new List<string>
						{
							"Full Day",
							"Half Day"
						};

		public List<LeaveDetails> LeaveDetails
		{
			get { return leaveDetails; }
			set
			{
				if (leaveDetails == null || !leaveDetails.SequenceEqual(value))
				{
					leaveDetails = value;
					OnPropertyChanged(nameof(LeaveDetails));
				}
			}
		}
		public ICommand SaveLeaveClick
		{
			get
			{
				return saveLeaveClick ?? (saveLeaveClick = new CommandHandler(() => SaveLeaveDetails(), () => true));
			}
		}

		public ICommand RefreshClick
		{
			get
			{
				return refreshClick ?? (refreshClick = new CommandHandler(() => FillLeaveDetails(), () => true));
			}
		}

		public LeaveDetails SelectedLeaveDetail
		{
			get { return selectedLeaveDetail; }
			set
			{
				if (selectedLeaveDetail != value)
				{
					selectedLeaveDetail = value;
					OnPropertyChanged(nameof(SelectedLeaveDetail));
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
							"Optional Leave",
							"Vacation Leave"
						};

		private void FillLeaveDetails()
		{
			Common.CommonHelper.WaitCursor();
			List<LeaveDetails> _leaveDetails = new List<LeaveDetails>();
			dbContext.LeaveDetails.OrderByDescending(o => o.Date).ToList().ForEach(leave =>
			{
				_leaveDetails.Add(new LeaveDetails
				{
					ID = leave.ID,
					Date = leave.Date.ToShortDateString(),
					DayType = leave.DayType,
					TypeOfLeave = leave.TypeOfLeave,
					Comment = leave.Comment,
					Month = leave.Date.ToString("MMMM"),
					Day = leave.Date.ToString("dddd")
				});
			});

			LeaveDetails = _leaveDetails;
			Common.CommonHelper.NormalCursor();
		}
		private void ResetFields()
		{
			this.Date = DateTime.Now;
			this.DayType = "Full Day";
			this.TypeOfLeave = "Casual Leave";
			this.Comment = string.Empty;
		}
		private void SaveLeaveDetails()
		{
			if (MessageBox.Show("Are you sure you want to save the leave details?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
			{
				return;
			}

			dbContext.LeaveDetails.Add(new Leave
			{
				Date = this.Date,
				DayType = this.DayType,
				TypeOfLeave = this.TypeOfLeave,
				Comment = this.Comment
			});

			if (dbContext.SaveChanges() > 0)
			{
				MessageBox.Show("Leave details saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
				FillLeaveDetails();
				ResetFields();
			}
		}
	}
}
