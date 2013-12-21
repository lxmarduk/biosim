using System;
using System.Windows.Forms;
using System.Reflection;
using Biosim.Implementation;
using System.Drawing;
using Biosim.Abstraction;

namespace Biosim.UI
{
	public class MainWindow : Form
	{
		MainToolbar toolbar;
		MapVizualizer mapVis;
		SideBar sidebar;
		Timer playTimer;
		public static Cell dummy = new Cell("Dummy");

		public MainWindow()
		{
			dummy = (Cell)Utils.DeserializeCell("cells/Dummy.bin");
			try {
				InitializeUi();
			} catch (NullReferenceException) {
				Application.Exit();
			}
		}

		void InitializeUi()
		{
			SetClientSizeCore(800, 600);
			Text = Assembly.GetExecutingAssembly().GetName().Name + " v" + Assembly.GetExecutingAssembly().GetName().Version;
			DoubleBuffered = true;
			toolbar = new MainToolbar(this);
			toolbar.Visible = true;
			toolbar.ButtonClick += ToolbarButtonClick;

			NewMapDialog.Show();

			Map map = NewMapDialog.Map;
			if (map == null) {
				throw new NullReferenceException();
			}
			map.InitializeCells(new Cell("Empty"));
			mapVis = new MapVizualizer(this, map);
			mapVis.Controls [0].MouseClick += HandleMouseClick;

			sidebar = new SideBar(this);

			Resize += HandleResize;

			playTimer = new Timer();
			playTimer.Tick += HandleTick;
			playTimer.Enabled = false;

			OnResize(null);
		}

		void HandleMouseClick(object sender, MouseEventArgs e)
		{
			int x = e.X / MapVizualizer.CellSize;
			int y = e.Y / MapVizualizer.CellSize;
			Cell pickedCell; 
			if (e.Button == MouseButtons.Left) {
				if (sidebar.Cells.View.SelectedIndex != -1) {
					pickedCell = (Cell)sidebar.Cells.SelectedCell;
				} else {
					return;
				}
				mapVis.Map.SetCell(pickedCell.Clone(), x, y);
			} else {
				mapVis.Map.Selector.Select(x, y).Properties ["Alive"].Unset();
			}
			mapVis.Refresh();
		}

		void HandleResize(object sender, EventArgs e)
		{
			mapVis.Left = 0;
			mapVis.Top = toolbar.Height + 7;
			mapVis.Width = ClientSize.Width - sidebar.Width;
			mapVis.Height = ClientSize.Height - toolbar.Height - 7;
			sidebar.Top = mapVis.Top;
			sidebar.Left = mapVis.Bounds.Right;
			sidebar.Height = mapVis.Height;
		}

		void HandleTick(object sender, EventArgs e)
		{
			mapVis.Map.Process();
			mapVis.Refresh();
		}

		void ToolbarButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			if (e.Button.Tag.GetType().Equals(typeof(int))) {
				switch ((int)e.Button.Tag) {
					case MainToolbar.PlayButton:
						e.Button.Tag = MainToolbar.PauseButton;
						e.Button.ImageKey = "pause";
						e.Button.Text = "Пауза";
						playTimer.Interval = 100;
						playTimer.Enabled = true;
						break;
					case MainToolbar.PauseButton:
						e.Button.Tag = MainToolbar.PlayButton;
						e.Button.ImageKey = "play";
						e.Button.Text = "Старт";
						playTimer.Enabled = false;
						break;
					case MainToolbar.NextStepButton:
						HandleTick(null, null);
						break;
					case MainToolbar.NewDocumentButton:
						if (MessageBox.Show("Ви впевнені, що хочете почати нову симуляцію?\nВсі не збережені зміни буде відкинуто.", "Нова симуляція", MessageBoxButtons.YesNo) == DialogResult.Yes) {
							if (NewMapDialog.Show() == DialogResult.OK) {
								mapVis.Map = NewMapDialog.Map;
								mapVis.Map.InitializeCells(new Cell("Dummy"));
								mapVis.Refresh();
							}
						}
						break;
				}
			}
		}
	}
}