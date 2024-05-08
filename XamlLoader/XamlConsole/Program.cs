using XamlLoader;

public class Program
{
    private static void Main(string[] args)
    {
        ReadAndWriteXml readAndWriteXml = new ReadAndWriteXml();
        readAndWriteXml.LoadImageType(@"G:\Work\Personal\XamlLoader\XamlLoader\ResourseFiles\ImageTypeConfiguration.xaml");
    }
}