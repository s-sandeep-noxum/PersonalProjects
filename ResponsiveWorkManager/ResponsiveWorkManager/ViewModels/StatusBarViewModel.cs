namespace ResponsiveWorkManager.ViewModels
{
	public class StatusBarViewModel : ViewModelBase
	{
		private string endStatus;
		private string longStatus;
		private string middleStatus;

		public StatusBarViewModel()
		{
			LongStatus = "Ready";
			MiddleStatus = "Ready";
			EndStatus = "Ready";
		}

		public StatusBarViewModel(string lStatus,string mStatus,string eStatus)
		{
			this.LongStatus = lStatus;
			this.MiddleStatus = mStatus;
			this.EndStatus = eStatus;
		}

		public string EndStatus
		{
			get { return endStatus; }
			set
			{
				if (endStatus != value)
				{
					endStatus = value;
					OnPropertyChanged(nameof(EndStatus));
				}
			}
		}

		public string LongStatus
		{
			get { return longStatus; }
			set
			{
				if (longStatus != value)
				{
					longStatus = value;
					OnPropertyChanged(nameof(LongStatus));
				}
			}
		}
		public string MiddleStatus
		{
			get { return middleStatus; }
			set
			{
				if (middleStatus != value)
				{
					middleStatus = value;
					OnPropertyChanged(nameof(MiddleStatus));
				}
			}
		}		
	}
}