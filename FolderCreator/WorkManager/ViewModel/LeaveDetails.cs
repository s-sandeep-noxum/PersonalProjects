using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Collections.Generic;
using System.Windows.Input;
using WorkManager.Commands;
using WorkManager.PDF;

namespace WorkManager.ViewModel
{
	public class LeaveDetails
	{
		public int ID { get; set; }
		public string TypeOfLeave { get; set; }
		public string DayType { get; set; }
		public string Date { get; set; }
		public string Comment { get; set; }
		public string Month { get; set; }
		public string Day { get; set; }
		private ICommand exportCommand;

		public bool CanExecute
		{
			get
			{
				// check if executing is allowed, i.e., validate, check if a process is running, etc.
				return true;
			}
		}

		public ICommand ExportCommand
		{
			get
			{
				return exportCommand ?? (exportCommand = new RelayCommand(CreatePDF.CreatePDFDocument));
			}
		}
	}
}
