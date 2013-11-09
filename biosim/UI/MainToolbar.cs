using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Biosim.UI
{
	public class MainToolbar : ToolBar
	{
		public const int PLAY_BUTTON = 0;
		public const int PAUSE_BUTTON = 1;
		public const int NEXT_STEP_BUTTON = 2;
        
		ImageList imageList;
        
		public MainToolbar(Form form) : base()
		{
			imageList = new ImageList();
			buildImages();
			this.ImageList = imageList;
            
			this.Parent = form;
			this.Dock = DockStyle.Top;
			this.Height = 52;
            
			//this.Appearance = ToolBarAppearance.Flat;
			this.ButtonSize = new System.Drawing.Size(52, 52);
            
			ToolbarToggleButton play = new ToolbarToggleButton("Play");
			play.ImageKey = "play";
			play.Tag = PLAY_BUTTON;
			this.Buttons.Add(play);
            
			ToolBarButton next = new ToolBarButton("Next step");
			next.ImageKey = "next_step";
			next.Tag = NEXT_STEP_BUTTON;
			this.Buttons.Add(next);
		}
        
		private void buildImages()
		{
			imageList.ColorDepth = ColorDepth.Depth32Bit;
			imageList.ImageSize = new Size(24, 24);
			imageList.TransparentColor = Color.Fuchsia;
			imageList.Images.Add("play", Image.FromStream(
                Utils.LoadResource("play"))
			);
			imageList.Images.Add("pause", Image.FromStream(
                Utils.LoadResource("pause"))
			);
			imageList.Images.Add("next_step", Image.FromStream(
                Utils.LoadResource("next_step"))
			);
		}
	}
}

