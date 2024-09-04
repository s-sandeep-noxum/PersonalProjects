﻿namespace ResponsiveWorkManager.DataObjects
{
	public class CalendarYear : IEquatable<CalendarYear>
	{
		public string YearText { get; set; }

		public bool Equals(CalendarYear other)
		{
			return this.YearText == other.YearText;
		}

		public override int GetHashCode()
		{
			return YearText.GetHashCode();
		}
	}
}
