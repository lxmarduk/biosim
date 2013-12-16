using System;
using System.Windows.Forms;
using System.Reflection;
using Biosim.Implementation;
using System.Drawing;

namespace Biosim.UI
{
	public class MainWindow : Form
	{
		MainToolbar toolbar;
		MapVizualizer mapVis;
		SideBar sidebar;
		Timer playTimer;

		public MainWindow() : base()
		{
			InitializeUi();
			IntProperty p = new IntProperty("Test", 10);
			Utils.Serialize(p);
		}

		void InitializeUi()
		{
			SetClientSizeCore(800, 600);
			Text = Assembly.GetExecutingAssembly().GetName().Name.ToString() + " v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
			DoubleBuffered = true;
			toolbar = new MainToolbar(this);
			toolbar.Visible = true;
			toolbar.ButtonClick += ToolbarButtonClick;

			Map map = new Map(16, 16);
			map.InitializeCells(new Cell("Test"));
			mapVis = new MapVizualizer(this, map);

			sidebar = new SideBar(this);

			NewRuleWidget w = new NewRuleWidget(map.Cells [0]);
			w.Parent = sidebar;
			w.Location = new Point(8, 200);

			Label lbl_mapsize = new Label();
			lbl_mapsize.Text = "Set map size:";
			lbl_mapsize.Location = new Point(8, 8);
			lbl_mapsize.Parent = sidebar;
			TextBox txt_width = new TextBox();
			txt_width.Text = "16";
			txt_width.Location = new Point(8, lbl_mapsize.Bottom + 8);
			txt_width.Parent = sidebar;
			TextBox txt_height = new TextBox();
			txt_height.Text = "16";
			txt_height.Location = new Point(8, txt_width.Bottom + 8);
			txt_height.Parent = sidebar;
			Button btn_size = new Button();
			btn_size.Text = "Set map size";
			btn_size.AutoSize = true;
			btn_size.Location = new Point(8, txt_height.Bottom + 8);
			btn_size.Parent = sidebar;
			btn_size.Click += (object sender, EventArgs e) => {
				map = null;
				map = new Map(Int32.Parse(txt_width.Text), Int32.Parse(txt_height.Text));
				map.InitializeCells(new Cell("Test"));
				mapVis.SetMap(map);
			};

			Resize += HandleResize;

			playTimer = new Timer();
			playTimer.Tick += HandleTick;
			playTimer.Enabled = false;

			OnResize(null);
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
						e.Button.Text = "Pause";
						playTimer.Interval = 500;
						playTimer.Enabled = true;
						break;
					case MainToolbar.PauseButton:
						e.Button.Tag = MainToolbar.PlayButton;
						e.Button.ImageKey = "play";
						e.Button.Text = "Play";
						playTimer.Enabled = false;
						break;
					case MainToolbar.NextStepButton:
						HandleTick(null, null);
						break;
				}
			}
		}
	}
}