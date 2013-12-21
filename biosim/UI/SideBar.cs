using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Biosim.UI
{
	public class SideBar : Panel
	{
		CellView view;

		public CellView Cells {
			get {
				return view;
			}
		}

		public SideBar(Control control) : base()
		{
			Parent = control;
			Size = new Size(300, 150);
			AutoScroll = true;
			BackColor = Color.White;

			view = new CellView(this);
		}
	}
}

