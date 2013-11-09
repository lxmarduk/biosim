using System;
using System.Windows.Forms;
using System.Reflection;
using Biosim.Implementation;

namespace Biosim.UI
{
	public class MainWindow : Form
	{
		MainToolbar toolbar;
		MapVizualizer mapVis;

		public MainWindow() : base()
		{
			this.SetClientSizeCore(800, 600);
			initializeUI();
		}

		private void initializeUI()
		{
			Text = Assembly.GetExecutingAssembly().GetName().Name.ToString() + " v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
			DoubleBuffered = true;
			toolbar = new MainToolbar(this);
			toolbar.Visible = true;
			toolbar.ButtonClick += toolbarButtonClick;

			Map map = new Map(64, 64);
			map.InitializeCells(new Cell("Test"));
			mapVis = new MapVizualizer(this, map);

			Resize += HandleResize;

			OnResize(null);
		}

		void HandleResize(object sender, EventArgs e)
		{
			mapVis.Left = 0;
			mapVis.Top = toolbar.Height + 7;
			mapVis.Width = ClientSize.Width;
			mapVis.Height = ClientSize.Height - toolbar.Height - 7;
		}
        
		private void toolbarButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			if (e.Button.Tag.GetType().Equals(typeof(int))) {
				switch ((int)e.Button.Tag) {
					case MainToolbar.PLAY_BUTTON:
						e.Button.Tag = MainToolbar.PAUSE_BUTTON;
						e.Button.ImageKey = "pause";
						e.Button.Text = "Pause";
						break;
					case MainToolbar.PAUSE_BUTTON:
						e.Button.Tag = MainToolbar.PLAY_BUTTON;
						e.Button.ImageKey = "play";
						e.Button.Text = "Play";
						break;
					case MainToolbar.NEXT_STEP_BUTTON:
						break;
					default:
						break;
				}
			}
		}
	}
}