using IronWord;
using IronWord.Models;
using SixLabors.ImageSharp;

namespace ResponsiveWorkManager.FileCreation
{
	public static class CreateWord
	{
		public static bool CreateDescriptionFile(string fileName, string description, string workItemNumber, string title)
		{
			IronWord.License.LicenseKey = "demo";
			WordDocument wordDocument = new WordDocument();
			fileName = fileName + ".docx";

			try
			{
				string titleText = $"{workItemNumber}:{title}";
				IronWord.Models.TextRun titleRun = new IronWord.Models.TextRun(titleText);
				IronWord.Models.Paragraph docTitle = new IronWord.Models.Paragraph();
				docTitle.Style = new TextStyle()
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
				docTitle.AddTextRun(titleRun);

				IronWord.Models.TextRun bodyRun = new IronWord.Models.TextRun(description);
				IronWord.Models.Paragraph body = new IronWord.Models.Paragraph();
				body.AddTextRun(bodyRun);

				wordDocument.AddParagraph(docTitle);
				wordDocument.AddParagraph(body);
				wordDocument.SaveAs(fileName);
				return true;
			}
			catch
			{
				return false;
			}
			finally
			{
				
			}
		}
	}
}
