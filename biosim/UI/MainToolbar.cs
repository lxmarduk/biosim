using System.Drawing;
using System.Windows.Forms;

namespace Biosim.UI
{
	public class MainToolbar : ToolBar
	{
		public const int PlayButton = 0;
		public const int PauseButton = 1;
		public const int NextStepButton = 2;
        
		ImageList imageList;
        
		public MainToolbar(Control control) : base()
		{
			imageList = new ImageList();
			buildImages();
			ImageList = imageList;
            
			Parent = control;
			Dock = DockStyle.Top;
			Height = 52;
            
			//this.Appearance = ToolBarAppearance.Flat;
			ButtonSize = new System.Drawing.Size(52, 52);
            
			ToolbarToggleButton play = new ToolbarToggleButton("Play");
			play.ImageKey = "play";
			play.ToolTipText = "Play/pause simulation";
			play.Tag = PlayButton;
			Buttons.Add(play);
            
			ToolBarButton next = new ToolBarButton("Next step");
			next.ImageKey = "next_step";
			next.ToolTipText = "Simulate one step";
			next.Tag = NextStepButton;
			Buttons.Add(next);

			ToolBarButton sep = new ToolBarButton();
			sep.Style = ToolBarButtonStyle.Separator;
			Buttons.Add(sep);
		}
        
		void buildImages()
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

