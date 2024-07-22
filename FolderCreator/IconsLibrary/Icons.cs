using System.Windows.Controls;
using System.Windows;
namespace IconsLibrary
{
	public class Computer01 : Image
	{
		static Computer01()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Computer01), new FrameworkPropertyMetadata(typeof(Computer01)));
		}
	}

	public class Computer02 : Image
	{
		static Computer02()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Computer02), new FrameworkPropertyMetadata(typeof(Computer02)));
		}
	}

	public class Project : Image
	{
		static Project()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Project), new FrameworkPropertyMetadata(typeof(Project)));
		}
	}
}
