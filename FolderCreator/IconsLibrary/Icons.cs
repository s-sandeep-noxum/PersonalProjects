using System.Windows.Controls;
using System.Windows;
namespace IconsLibrary
{
	public class Computer : Image
	{
		static Computer()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Computer), new FrameworkPropertyMetadata(typeof(Computer)));
		}
	}

	public class ComputerNew : Image
	{
		static ComputerNew()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ComputerNew), new FrameworkPropertyMetadata(typeof(ComputerNew)));
		}
	}
}
