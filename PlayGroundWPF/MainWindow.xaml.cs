using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.CommandWpf;

namespace PlayGroundWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
			this.DataContext = this;
        }

		TabItem NewTab1()
		{
			return new TabItem()
			{
				Header = "Tab Name 1",
				Content = "Tab Content 1"
			};
		}

		TabItem NewTab2()
		{
			return new TabItem()
			{
				Header = "Tab Name 2",
				Content = "Tab Content 2"
			};
		}

		private void TabClose(object sender, ExecutedRoutedEventArgs e)
		{
			TabItem tab = (TabItem)e.OriginalSource;
			tabControl.Items.Remove(tab);
		}

		private void TabCloseAll(object sender, ExecutedRoutedEventArgs e)
		{
			tabControl.Items.Clear();
		}

		private void TabCloseAllOthers(object sender, ExecutedRoutedEventArgs e)
		{
			TabItem tab = (TabItem)e.OriginalSource;
			for (int i = tabControl.Items.Count - 1; i >= 0; i--)
			{
				if (tabControl.Items[i] != tab)
				{
					tabControl.Items.RemoveAt(i);
				}
			}
		}

		private void TabOpen(object sender, ExecutedRoutedEventArgs e)
		{
			TabItem tab;

			Random r = new Random();
			if (r.Next() % 2 == 0)
			{
				tab = NewTab1();
			}
			else
			{
				tab = NewTab2();
			}

			tabControl.Items.Add(tab);
			tabControl.SelectedItem = tab;
		}

		private void TabOpenTab1(object sender, ExecutedRoutedEventArgs e)
		{
			TabItem tab = NewTab1();
			tabControl.Items.Add(tab);
			tabControl.SelectedItem = tab;
		}

		private void TabOpenTab2(object sender, ExecutedRoutedEventArgs e)
		{
			TabItem tab = NewTab2();
			tabControl.Items.Add(tab);
			tabControl.SelectedItem = tab;
		}

		private void TabNewButton_Click(object sender, RoutedEventArgs e)
		{
			((Button)sender).ContextMenu.IsOpen = true;
		}
	}

	public static class TabCommands
	{
		public static RoutedUICommand Close = new RoutedUICommand(
			"Close",
			"TabClose",
			typeof(TabItem));

		public static RoutedUICommand CloseAll = new RoutedUICommand(
			"Close All",
            "CloseAll",
			typeof(TabItem));

		public static RoutedUICommand CloseAllOthers = new RoutedUICommand(
			"Close All Others",
			"TabCloseAllOthers",
			typeof(TabItem));

		public static RoutedUICommand Open = new RoutedUICommand(
			"Open",
			"Tab Open",
			typeof(TabItem));

		public static RoutedUICommand Tab1 = new RoutedUICommand(
			"Tab 1",
			"Tab1",
			typeof(TabControl));

		public static RoutedUICommand Tab2 = new RoutedUICommand(
			"Tab 2",
			"Tab2",
			typeof(TabControl));
	}
}
