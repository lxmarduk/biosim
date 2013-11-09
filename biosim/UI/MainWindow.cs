using System;
using System.Windows.Forms;
using System.Reflection;

namespace Biosim.UI
{
	public class MainWindow : Form
	{
		MainToolbar toolbar;

		public MainWindow() : base()
		{
			this.SetClientSizeCore(800, 600);
			initializeUI();
		}

		private void initializeUI()
		{
			Text = Assembly.GetExecutingAssembly().GetName().Name.ToString() + " v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
			toolbar = new MainToolbar(this);
			toolbar.Visible = true;
			toolbar.ButtonClick += toolbarButtonClick;
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