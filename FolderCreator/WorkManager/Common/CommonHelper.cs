using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WorkManager.Common
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
