using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvariantCultureTesting
{
	public class Program
	{
		static void Main(string[] args)
		{


			string data = 1234567.89.ToString("N",new CultureInfo("de-DE"));

			Console.WriteLine(data);
			Console.ReadLine();
		}
	}
}
