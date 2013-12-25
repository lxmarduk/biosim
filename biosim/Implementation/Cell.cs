using System;
using System.Drawing;
using Biosim.Abstraction;
using System.Text;

namespace Biosim.Implementation
{
	[Serializable]
	public class Cell : AbstractCell
	{
		[Serializable]
		public enum CellShape
		{
			Square,
			Circle,
			Triangle,
			Diamond
		}

		CellShape shape;
		Color color;

		public Color Color {
			get {
				return color;
			}
			set {
				color = value;
				if (drawer != null) {
					drawer.FillColor = color;
				}
			}
		}

		public CellShape Shape {
			get {
				return shape;
			}
			set {
				shape = value;
				switch (shape) {
					case CellShape.Square:
						drawer = new CellDrawerSquare();
						break;
					case CellShape.Circle:
						drawer = new CellDrawerCircle();
						break;
					case CellShape.Triangle:
						drawer = new CellDrawerTriangle();
						break;
					case CellShape.Diamond:
						drawer = new CellDrawerDiamond();
						break;
				}
				drawer.FillColor = color;
			}
		}

		ShapeDrawer drawer;

		public Cell(string name) : base(name)
		{
			Color = Color.Lime;
			Shape = CellShape.Square;
		}

		#region implemented abstract members of Biosim.Abstraction.AbstractCell

		public override AbstractCell Clone()
		{
			Cell result = new Cell(Properties ["Ім'я"].Value.ToString());
			foreach (AbstractProperty prop in Properties) {
				result.Properties.Add(prop.Clone());
			}
			result.Shape = shape;
			result.Color = Color;
			return result;
		}

		public override void Draw(Graphics g, Rectangle bounds)
		{
			g.DrawRectangle(Pens.Blue, bounds);
			g.FillRectangle(Brushes.DimGray, bounds);
			if ((bool)Properties ["Жива"].Value) {
				drawer.Draw(g, bounds);
			}
		}

		public void DrawIcon(Graphics g, Rectangle bounds)
		{
			drawer.Draw(g, bounds);
		}

		#endregion

		public override string ToString()
		{
			StringBuilder b = new StringBuilder();
			b.Append(GetType().Name);
			b.Append("\n");
			foreach (AbstractProperty p in Properties) {
				b.Append("\t");
				b.Append(p.Name);
				b.Append(": ");
				b.Append(p.Value.ToString());
				b.Append("\n");
			}
			b.Append("\tФорма: ");
			b.Append(Utils.CellShapeToString(Shape));
			b.Append("\n\tКолір: ");
			b.AppendFormat("rgb({0}, {1}, {2})", color.R, color.G, color.B);
			b.Append("\n");
			return b.ToString();
		}
	}
}

