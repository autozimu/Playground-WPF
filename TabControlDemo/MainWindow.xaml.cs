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
using System.IO;
using System.Windows.Markup;

namespace TabControlDemo
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
				TabOpenTab1(sender, null);
			}
			else
			{
				TabOpenTab2(sender, null);
			}
		}

		private void TabClone(TabItem original)
		{
			object clone;
			using (var stream = new MemoryStream())
			{
				XamlWriter.Save(original, stream);
				stream.Seek(0, SeekOrigin.Begin);
				clone = XamlReader.Load(stream);
			}

			tabControl.Items.Add(clone);
			tabControl.SelectedItem = clone;
		}

		private void TabOpenTab1(object sender, ExecutedRoutedEventArgs e)
		{
			TabClone(tab1);
		}

		private void TabOpenTab2(object sender, ExecutedRoutedEventArgs e)
		{
			TabClone(tab2);
		}

		private void TabNewButton_Click(object sender, RoutedEventArgs e)
		{
			Button btn = (Button)sender;
			btn.ContextMenu.PlacementTarget = btn;
			btn.ContextMenu.IsOpen = true;
		}

		Point _dragStartingPoint;
		private void TabItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_dragStartingPoint = e.GetPosition(null);
		}

		bool _isDragging = false;
		private void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			TabItem tab = (TabItem)sender;

			if (e.LeftButton == MouseButtonState.Pressed && !_isDragging)
			{
				Point position = e.GetPosition(null);
				if (Math.Abs(position.X - _dragStartingPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
					Math.Abs(position.Y - _dragStartingPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
				{
					_isDragging = true;

					DragDrop.DoDragDrop(tab, tab, DragDropEffects.All);

					_isDragging = false;
				}
			}
		}

		private void TabItem_Drop(object sender, DragEventArgs e)
		{
			// Keep selected tab to restore later.
			// Otherwise the tab control will use selected index to select content.
			TabItem selectedTab = (TabItem)tabControl.SelectedItem;

			TabItem targetTab = sender as TabItem;
			if (targetTab == null) return;
			if (e.Data.GetDataPresent(typeof(TabItem)) == false) return;
			TabItem sourceTab = (TabItem)e.Data.GetData(typeof(TabItem));

			if (targetTab != sourceTab)
			{
				int targetIdx = tabControl.Items.IndexOf(targetTab);

				tabControl.Items.Remove(sourceTab);
				tabControl.Items.Insert(targetIdx, sourceTab);
			}

			tabControl.SelectedItem = selectedTab;
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
