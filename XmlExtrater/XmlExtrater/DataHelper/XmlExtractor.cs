using System.Xml.Linq;

namespace XmlExtrater.DataHelper
{
	public static class XmlExtractor
	{

		public static Dictionary<string, string>? ExtractXmlTag(string emailText,out int processedPosition)
		{
			processedPosition = emailText.Length;
			Dictionary<string, string> _xmlData = new Dictionary<string, string>();
			if (string.IsNullOrEmpty(emailText))
			{
				return null;
			}

			if (emailText.Contains("<") && emailText.Contains(">"))
			{
				int startIndex = emailText.IndexOf("<");
				int endIndex = emailText.IndexOf(">");
				string tag = emailText.Substring(startIndex + 1, endIndex - startIndex - 1);

				if (!emailText.Contains("</" + tag + ">"))
				{
					return null;
				}

				processedPosition = emailText.IndexOf("</" + tag + ">") + ("</" + tag + ">").Length;

				string value = emailText.Substring(emailText.IndexOf("<" + tag + ">"), emailText.IndexOf("</" + tag + ">") - emailText.IndexOf("<" + tag + ">") + ("</" + tag + ">").Length);


				var xmlData = XElement.Parse(value);
				if (xmlData.Descendants().Count() > 0)
				{
					foreach (var item in xmlData.Descendants())
					{
						_xmlData.Add(item.Name.LocalName, item.Value);
					}
				}
				else
				{
					_xmlData.Add(xmlData.Name.LocalName, xmlData.Value);
				}
			}
			else
			{
				return null;
			}

			return _xmlData;
		}
	}
}
