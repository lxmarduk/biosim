using System;
using System.Drawing;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	[Serializable]
	public class Cell : AbstractCell
	{
		public enum CellShape
		{
			Square,
			Circle,
			Triangle,
			Diamond
		}

		CellShape shape;

		public CellShape Shape {
			get {
				return shape;
			}
			set {
				shape = value;
			}
		}

		Color color;

		public Color Color {
			get {
				return color;
			}
			set {
				color = value;
			}
		}

		public Cell(string name) : base(name)
		{
			shape = CellShape.Square;
			color = Color.Lime;
		}

		#region implemented abstract members of Biosim.Abstraction.AbstractCell

		public override AbstractCell Clone()
		{
			Cell result = new Cell(this.Properties ["Name"].Value.ToString());
			foreach (AbstractProperty prop in Properties) {
				result.Properties.Add(prop.Clone());
			}
			return result;
		}

		public override void Draw(System.Drawing.Graphics g, System.Drawing.Rectangle bounds)
		{
			g.FillRectangle(Brushes.DimGray, bounds);
			Pen outline = new Pen(Brushes.Black);
			outline.Width = 2;
			if ((bool)Properties ["Alive"].Value) {
				Brush colorBrush = new SolidBrush(Color);
				switch (Shape) {
					case CellShape.Square:
						g.FillRectangle(colorBrush, new Rectangle(bounds.Left + 1, bounds.Top + 1, bounds.Width - 1, bounds.Height - 1));
						g.DrawRectangle(outline, new Rectangle(bounds.Left + 1, bounds.Top + 1, bounds.Width - 1, bounds.Height - 1));
						break;
					case CellShape.Circle:
						g.FillEllipse(colorBrush, new Rectangle(bounds.Left, bounds.Top, bounds.Width - 2, bounds.Height - 2));
						g.DrawEllipse(outline, new Rectangle(bounds.Left, bounds.Top, bounds.Width - 2, bounds.Height - 2));
						break;
					case CellShape.Triangle:
						Point[] triangle = new Point[] { new Point(bounds.Left + bounds.Width / 2, bounds.Top + 1),
							new Point(bounds.Left, bounds.Bottom - 1), new Point(bounds.Right, bounds.Bottom - 1)
						};
						g.FillPolygon(colorBrush, triangle);
						g.DrawPolygon(outline, triangle);
						break;
					case CellShape.Diamond:
						Point[] diamond = new Point[] { new Point(bounds.Left + bounds.Width / 2, bounds.Top),
							new Point(bounds.Left, bounds.Top + bounds.Height / 2), 
							new Point(bounds.Left + bounds.Width / 2, bounds.Bottom), 
							new Point(bounds.Right, bounds.Top + bounds.Width / 2)
						};
						g.FillPolygon(colorBrush, diamond);
						g.DrawPolygon(outline, diamond);
						break;
					default:
						throw new System.ArgumentOutOfRangeException();
				}
			}
		}

		#endregion

	}
}

