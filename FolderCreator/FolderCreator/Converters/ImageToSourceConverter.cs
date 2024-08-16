using System;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WorkManager.Converters
{
	public class ImageToSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter,System.Globalization.CultureInfo culture)
		{
			System.Windows.Controls.Image image = value as System.Windows.Controls.Image;
			if (image != null)
			{
				return image;
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// This converter automatically disposes image streams as soon as images
	/// are loaded, thus avoiding file access exceptions when attempting to delete images.
	/// </summary>
	public sealed class ImageSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string path = value as string;
			if (path != null)
			{
				//create new stream and create bitmap frame
				var bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.StreamSource = new FileStream(path, FileMode.Open, FileAccess.Read);
				//load the image now so we can immediately dispose of the stream
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.EndInit();

				//clean up the stream to avoid file access exceptions when attempting to delete images
				bitmapImage.StreamSource.Dispose();

				return bitmapImage;
			}
			else
			{
				return DependencyProperty.UnsetValue;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
