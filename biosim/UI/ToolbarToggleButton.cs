using System.Windows.Forms;

namespace Biosim.UI
{
	public class ToolbarToggleButton : ToolBarButton
	{
		public ToolbarToggleButton() : this("")
		{
		}

		public ToolbarToggleButton(string text) : base (text)
		{
			Style = ToolBarButtonStyle.ToggleButton;
		}

	}
}

