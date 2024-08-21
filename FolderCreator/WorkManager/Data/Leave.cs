using System;

namespace WorkManager.Data
{
	public class Leave
	{
		public int ID { get; set; }
		public string? TypeOfLeave { get; set; }
		public string? DayType { get; set; }
		public DateTime Date { get; set; }
		public string? Comment { get; set; }
	}
}
