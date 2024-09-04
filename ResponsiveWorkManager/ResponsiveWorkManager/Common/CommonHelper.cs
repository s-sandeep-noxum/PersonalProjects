using System.Windows;
using System.Windows.Input;

namespace ResponsiveWorkManager.Common
{
	public static class CommonHelper
	{
		public static void WaitCursor()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Mouse.OverrideCursor = Cursors.Wait;
			});
		}

		public static void NormalCursor()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Mouse.OverrideCursor = null;
			});
		}
	}
}
