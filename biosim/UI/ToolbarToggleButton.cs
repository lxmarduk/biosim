using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Biosim.UI
{
	public class ToolbarToggleButton : ToolBarButton
	{
		public ToolbarToggleButton() : this("")
		{
		}

		public ToolbarToggleButton(string text) : base (text)
		{
			this.Style = ToolBarButtonStyle.ToggleButton;
		}

	}
}

