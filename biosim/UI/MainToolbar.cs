using System.Drawing;
using System.Windows.Forms;

namespace Biosim.UI
{
	public class MainToolbar : ToolBar
	{
		public const int PlayButton = 0;
		public const int PauseButton = 1;
		public const int NextStepButton = 2;
		public const int NewDocumentButton = 3;
		ImageList imageList;

		public MainToolbar(Control control) : base()
		{
			imageList = new ImageList();
			buildImages();
			ImageList = imageList;
            
			Parent = control;
			Dock = DockStyle.Top;
			Height = 52;
            
			Appearance = ToolBarAppearance.Flat;
			ButtonSize = new Size(52, 52);
            
			ToolbarToggleButton play = new ToolbarToggleButton("Старт");
			play.ImageKey = "play";
			play.ToolTipText = "Запустити/Зупинити симуляцію";
			play.Tag = PlayButton;
			Buttons.Add(play);
            
			ToolBarButton next = new ToolBarButton("Крок");
			next.ImageKey = "next_step";
			next.ToolTipText = "Симулювати один крок";
			next.Tag = NextStepButton;
			Buttons.Add(next);

			ToolBarButton sep = new ToolBarButton();
			sep.Style = ToolBarButtonStyle.Separator;
			Buttons.Add(sep);

			ToolBarButton newDoc = new ToolBarButton("Створити");
			newDoc.ImageKey = "document-new";
			newDoc.ToolTipText = "Нова симуляція";
			newDoc.Tag = NewDocumentButton;
			Buttons.Add(newDoc);
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
			imageList.Images.Add("document-new", Image.FromStream(
				Utils.LoadResource("document-new"))
			);
		}
	}
}

