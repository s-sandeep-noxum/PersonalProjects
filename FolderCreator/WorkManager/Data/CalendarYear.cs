using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkManager.Data
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
