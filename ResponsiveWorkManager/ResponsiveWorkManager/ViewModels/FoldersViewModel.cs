using IronWord.Models;
using IronWord;
using ResponsiveWorkManager.Commands;
using ResponsiveWorkManager.DataObjects;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Drawing;

namespace ResponsiveWorkManager.ViewModels
{
	public class FoldersViewModel : ViewModelBase, IDataErrorInfo
	{
		private string description;
		private string folderPath;

		private ICommand resetClick;
		private ICommand saveClick;
		private CalendarYear selectedYear;
		private string title;
		private string wiNumber;
		private List<CalendarYear> yearData;

		public FoldersViewModel()
		{
			this.FolderPath = ConfigurationManager.AppSettings["SavePath"].ToString();
			this.SelectedYear = new CalendarYear { YearText = DateTime.Now.Year.ToString() };
		}

		public ICommand ResetClick
		{
			get
			{
				return resetClick ?? (resetClick = new CommandHandler(() => ResetWindow(), () => CanExecute));
			}
		}

		private void ResetWindow()
		{
			this.WiNumber = string.Empty;
			this.Title = string.Empty;
			this.Description = string.Empty;			
		}

		public bool CanExecute => true;

		public string Description
		{
			get { return description; }
			set
			{
				description = value;
				OnPropertyChanged(nameof(Description));
			}
		}

		public string Error
		{
			get
			{
				return null;
			}
		}

		public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();

		public string FolderPath
		{
			get
			{
				return folderPath;
			}
			set
			{
				if (folderPath == value) return;
				folderPath = value;
				OnPropertyChanged(nameof(FolderPath));
			}
		}

		public ICommand SaveClick
		{
			get
			{
				return saveClick ?? (saveClick = new CommandHandler(() => SaveWindow(), () => CanExecute));
			}
		}

		public CalendarYear SelectedYear
		{
			get { return new CalendarYear { YearText = selectedYear.YearText.ToString() }; }
			set
			{
				if (selectedYear?.YearText == value.YearText) return;
				selectedYear = new CalendarYear { YearText = value.YearText.ToString() };
				OnPropertyChanged(nameof(SelectedYear));
			}
		}

		public string Title
		{
			get { return this.title; }
			set
			{
				this.title = value;
				OnPropertyChanged(nameof(Title));
			}
		}
		public string WiNumber
		{
			get { return wiNumber; }
			set
			{
				wiNumber = value;
				OnPropertyChanged(nameof(WiNumber));
			}
		}
		public List<CalendarYear> YearData
		{
			get { return FillYearDetails(); }
		}
		public string this[string objectName]
		{
			get
			{
				string error = string.Empty;

				switch (objectName)
				{
					case "WiNumber":
						int txtLength = 0;
						if (WiNumber != "" && WiNumber != null)
						{
							txtLength = WiNumber.ToString().Length;
						}
						if (txtLength < 6) error = "Work Item Number Should be 6 digits.";
						break;
				}

				if (error != string.Empty)
				{
					if (ErrorCollection.ContainsKey(objectName))
					{
						this.ErrorCollection.Remove(objectName);
					}
					this.ErrorCollection.Add(objectName, error);
				}
				else if (ErrorCollection.ContainsKey(objectName))
				{
					this.ErrorCollection.Remove(objectName);
				}

				OnPropertyChanged(nameof(ErrorCollection));
				return error;
			}
		}

		private void CreateDescriptionFile(string fileName, string description)
		{
			IronWord.License.LicenseKey = "demo";
			WordDocument wordDocument = new WordDocument();

			string titleText = $"{WiNumber}:{Title}";
			IronWord.Models.TextRun titleRun = new IronWord.Models.TextRun(titleText);
			IronWord.Models.Paragraph title = new IronWord.Models.Paragraph();
			title.Style = new TextStyle()
			{
				FontFamily = "Verdana",
				FontSize = 20,
				TextColor = new IronColor(Color.Blue),
				IsBold = true,
				IsItalic = false,
				IsUnderline = true,
				IsSuperscript = false,
				IsStrikethrough = false,
				IsSubscript = false
			};
			title.AddTextRun(titleRun);

			IronWord.Models.TextRun bodyRun = new IronWord.Models.TextRun(description);
			IronWord.Models.Paragraph body = new IronWord.Models.Paragraph();
			body.AddTextRun(bodyRun);

			wordDocument.AddParagraph(title);
			wordDocument.AddParagraph(body);
			wordDocument.SaveAs(fileName);
		}

		private bool CreateFolders(ref string folderName)
		{
			try
			{
				folderName = FolderPath + "\\" + SelectedYear.YearText + "\\" + this.GetFolderName();
				bool exists = System.IO.Directory.Exists(folderName);

				if (!exists)
				{
					System.IO.Directory.CreateDirectory(folderName);
				}
				if (!string.IsNullOrEmpty(Description))
				{
					string FileName = folderName.Trim() + "\\" + "Description.docx";
					CreateDescriptionFile(FileName, description);
				}
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}
		}
		private List<CalendarYear> FillYearDetails()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(@"DataObjects\YearData.xml");
			XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("Year");

			yearData = new List<CalendarYear>();

			foreach (XmlNode node in xmlNodeList)
			{
				yearData.Add(new CalendarYear { YearText = node.InnerText });
			}

			return yearData;
		}

		private string GetFolderName()
		{
			int wiLength = WiNumber.Length;
			int titleLength = Title.Length;

			if (wiLength + titleLength > 60)
			{
				return WiNumber + "-" + Title.Substring(0, titleLength - (wiLength + 1));
			}

			return WiNumber + "-" + Title;
		}
		private void SaveWindow()
		{
			if (ErrorCollection.Count > 0)
			{
				MessageBox.Show("Please correct the errors before saving the data.");
				return;
			}
			string folderPath = string.Empty;
			if (this.CreateFolders(ref folderPath))
			{
				if (MessageBox.Show("Do you want to open the created folder?", "Folder Created", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
				{
					System.Diagnostics.Process.Start("explorer.exe", folderPath);
				}
			}
		}
	}
}
