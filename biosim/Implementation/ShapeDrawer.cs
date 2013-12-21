using System;
using Biosim.Abstraction;
using System.Drawing;
using System.Windows.Forms;

namespace Biosim.Implementation
{
	[Serializable]
	public class ShapeDrawer : ICellDrawer
	{
		public Color BorderColor {
			get;
			set;
		}

		public Color FillColor {
			get;
			set;
		}

		public Padding Margins {
			get;
			set;
		}

		public ShapeDrawer()
		{
			BorderColor = Color.Black;
			FillColor = Color.Lime;
			Margins = new Padding(2);
		}

		#region ICellDrawer implementation

		public virtual void Draw(Graphics g, Rectangle rect)
		{
			Rectangle r = new Rectangle(
				              rect.Left + Margins.Left,
				              rect.Top + Margins.Top,
				              rect.Width - Margins.Right - Margins.Left,
				              rect.Height - Margins.Bottom - Margins.Top
			              );
			g.FillRectangle(new SolidBrush(FillColor), r);
			g.DrawRectangle(new Pen(new SolidBrush(BorderColor)), r);
		}

		#endregion

	}
}

