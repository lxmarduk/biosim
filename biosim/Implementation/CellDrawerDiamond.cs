using System;
using Biosim.Abstraction;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Biosim.Implementation
{
	[Serializable]
	public sealed class CellDrawerDiamond : ShapeDrawer
	{

		#region ICellDrawer implementation

		public override void Draw(Graphics g, Rectangle rect)
		{
			g.CompositingQuality = CompositingQuality.HighSpeed;
			g.SmoothingMode = SmoothingMode.HighQuality;
			Pen borderPen = new Pen(new SolidBrush(BorderColor));
			borderPen.Width = 2;
			Rectangle r = new Rectangle(
				              rect.Left + Margins.Left,
				              rect.Top + Margins.Top,
				              rect.Width - Margins.Right - Margins.Left,
				              rect.Height - Margins.Bottom - Margins.Top
			              );
			PointF[] points = {
				new PointF(r.Left + r.Width / 2, r.Top),
				new PointF(r.Left, r.Top + r.Height / 2),
				new PointF(r.Left + r.Width / 2, r.Bottom),
				new PointF(r.Right, r.Top + r.Height / 2)
			};
			int red = FillColor.R - (FillColor.R / 2);
			red = red < 0 ? 0 : red;
			int green = FillColor.G - (FillColor.G / 2);
			green = green < 0 ? 0 : green;
			int blue = FillColor.B - (FillColor.B / 2);
			blue = blue < 0 ? 0 : blue;
			Color darkerFill = Color.FromArgb(red, green, blue);
			Brush fillBrush = new LinearGradientBrush(r, FillColor, darkerFill, LinearGradientMode.Vertical);
			g.FillPolygon(fillBrush, points);
			g.DrawPolygon(borderPen, points);
		}

		#endregion

	}
}

