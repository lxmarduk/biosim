using System;
using System.Drawing;
using System.Windows.Forms;

namespace Biosim.UI
{
	public class SideBar : Panel
	{
		public SideBar(Control control) : base()
		{
			Parent = control;
			Size = new Size(300, 150);
			AutoScroll = true;
			BackColor = Color.White;
		}
	}
}

