using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Reflection;

namespace ComboBoxImageTesting.ViewModel
{
	public class ViewModelClass : INotifyPropertyChanged
	{
		private ImageSource CreateImageSourceFromIcon(Icon icon)
		{
			BitmapImage bitmapImage = new BitmapImage();
			if (icon != null)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					bitmapImage.BeginInit();
					bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
					bitmapImage.DecodePixelWidth = 16;
					bitmapImage.DecodePixelHeight = 16;
					icon.Save(memoryStream);
					memoryStream.Seek(0, SeekOrigin.Begin);
					bitmapImage.StreamSource = memoryStream;
					bitmapImage.EndInit();
				}
			}
			return bitmapImage;
		}

		public ViewModelClass()
		{			
			//string iconEMUri = "ComboBoxImageTesting.Icons.Arrow.ico";
			string iconEMUri = "../net6.0-windows/Icons/Icon-VS.png";
			//this.UITypeEditorButtonImage = this.CreateImageSourceFromIcon(new Icon(typeof(System.Windows.Controls.ComboBox), iconEMUri));
			//this.UITypeEditorButtonImage = this.CreateImageSourceFromIcon(new Icon(iconUri));
			//this.UITypeEditorButtonImage = this.CreateImageSourceFromIcon(new Icon(typeof(ComboBox), iconUri));
			this.UITypeEditorButtonImage = this.CreateImageSourceFromIcon(new Icon(iconEMUri));

			if (false)
			{
				this.UITypeEditorButtonImage = this.GetEmbeddedIcon(iconEMUri);
			}
		}

		private ImageSource GetEmbeddedIcon(string resourceName)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
			{
				if (stream != null)
				{
					return CreateImageSourceFromIcon(new Icon(stream));
				}
				else
				{
					throw new ArgumentException($"Resource '{resourceName}' cannot be found.");
				}
			}
		}
		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnNotifyPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private ImageSource uITypeEditorButtonImage = null;

		public ImageSource UITypeEditorButtonImage
		{
			get { return this.uITypeEditorButtonImage; }
			set
			{
				if (this.uITypeEditorButtonImage != value)
				{
					this.uITypeEditorButtonImage = value;
					this.OnNotifyPropertyChanged("UITypeEditorButtonImage");
				}
			}
		}
	}
}
