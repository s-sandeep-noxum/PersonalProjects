using System.Collections.Generic;
using System.Windows;

namespace XmlStripper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public Dictionary<string, string> CompleteXmlList { get; set; } = new Dictionary<string, string>();

		private void btnStripXml_Click(object sender, RoutedEventArgs e)
		{
			string emailText = rtbData.Text;
			Dictionary<string, string>? xmlData = new Dictionary<string, string>();
			int processedPosition = 0;
			do
			{
				xmlData = XmlExtrater.DataHelper.XmlExtractor.ExtractXmlTag(emailText, out processedPosition);

				if (xmlData != null)
				{
					foreach (var item in xmlData)
					{
						CompleteXmlList.Add(item.Key, item.Value);
					}
				}
				emailText = emailText.Substring(processedPosition);
			} while (emailText.Length > 0);

			rtbXmlData.Text = string.Join(Environment.NewLine, CompleteXmlList);
		}
	}
}