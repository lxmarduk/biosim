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
		OpenFileDialog openFile;
		SaveFileDialog saveFile;
		SeriesCollection aliveCollection;
		SeriesCollection bornCollection;
		SeriesCollection diedCollection;

		public MainWindow()
		{
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

			Map map = new Map(16, 16);
			map.InitializeCells(new Cell("Empty"));
			mapVis = new MapVizualizer(this, map);
			mapVis.Controls [0].MouseClick += HandleMouseClick;

			sidebar = new SideBar(this);

			Resize += HandleResize;

			playTimer = new Timer();
			playTimer.Tick += HandleTick;
			playTimer.Enabled = false;

			openFile = new OpenFileDialog();
			openFile.CheckFileExists = true;
			openFile.Filter = "Binary data|*.bin";
			openFile.FilterIndex = 0;
			openFile.Multiselect = false;
			openFile.RestoreDirectory = true;
			openFile.ShowReadOnly = true;
			openFile.Title = "Відкрити";

			saveFile = new SaveFileDialog();
			saveFile.Filter = "Binary data|*.bin";
			saveFile.FilterIndex = 0;
			saveFile.AddExtension = true;
			saveFile.DefaultExt = ".bin";
			saveFile.CreatePrompt = true;
			saveFile.RestoreDirectory = true;
			saveFile.Title = "Зберегти";

			aliveCollection = new SeriesCollection("Кількість живих");
			aliveCollection.Color = Color.Lime;
			bornCollection = new SeriesCollection("Кількість народжених");
			bornCollection.Color = Color.Blue;
			diedCollection = new SeriesCollection("Кількість померлих");
			diedCollection.Color = Color.DarkOrchid;

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
				mapVis.Map.Selector.Select(x, y).Properties ["Жива"].Unset();
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
			aliveCollection.Add(mapVis.Map.CellsAlive);
			bornCollection.Add(mapVis.Map.CellsBorn);
			diedCollection.Add(mapVis.Map.CellsDied);
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
						NewMapDialog newMapDlg = new NewMapDialog();
						if (MessageBox.Show("Ви впевнені, що хочете почати нову симуляцію?\nВсі не збережені зміни буде відкинуто.", "Нова симуляція", MessageBoxButtons.YesNo) == DialogResult.Yes) {
							if (newMapDlg.Show() == DialogResult.OK) {
								mapVis.Map = newMapDlg.Map;
								mapVis.Map.InitializeCells(new Cell("Empty"));
								mapVis.Refresh();
								aliveCollection.Clear();
								bornCollection.Clear();
								diedCollection.Clear();
							}
						}
						newMapDlg.Dispose();
						break;
					case MainToolbar.OpenDocumentButton:
						if (openFile.ShowDialog() == DialogResult.OK) {
							Map m = Utils.DeserializeMap(openFile.FileName);
							if (m != null) {
								mapVis.Map = m;
								mapVis.Refresh();
								aliveCollection.Clear();
								bornCollection.Clear();
								diedCollection.Clear();
							} else {
								MessageBox.Show("Не можу завантажити симуляцію. Можливо, це не файл симуляції.", "Помилка завантаження.", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
						}
						break;
					case MainToolbar.SaveDocumentButton:
						if (saveFile.ShowDialog() == DialogResult.OK) {
							Utils.SerializeMap(mapVis.Map, saveFile.FileName);
						}
						break;
					case MainToolbar.DocumentProperties:
						break;
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (Controls != null) {
					for (int i = 0; i < Controls.Count; ++i) {
						Controls [i].Dispose();
					}
				}
			}
			base.Dispose(disposing);
		}
	}
}