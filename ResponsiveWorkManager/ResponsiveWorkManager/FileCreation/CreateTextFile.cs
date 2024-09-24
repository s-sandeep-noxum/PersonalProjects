namespace ResponsiveWorkManager.FileCreation
{
	public static class CreateTextFile
	{

		public static bool CreateFile(string fileName, string description, string workItemNumber, string title)
		{
			try
			{
				string titleText = $"{workItemNumber}:{title}";
				string bodyText = description;
				fileName = fileName + ".txt";

				System.IO.File.WriteAllText(fileName, titleText + Environment.NewLine + bodyText);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
