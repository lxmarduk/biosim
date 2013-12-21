using System;
using System.Drawing;
using Biosim.Abstraction;

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

		public Color Color {
			get {
				return drawer.FillColor;
			}
			set {
				drawer.FillColor = value;
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
			}
		}

		ShapeDrawer drawer;

		public Cell(string name) : base(name)
		{
			Shape = CellShape.Square;
		}

		#region implemented abstract members of Biosim.Abstraction.AbstractCell

		public override AbstractCell Clone()
		{
			Cell result = new Cell(Properties ["Name"].Value.ToString());
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
			if ((bool)Properties ["Alive"].Value) {
				drawer.Draw(g, bounds);
			}
		}

		public void DrawIcon(Graphics g, Rectangle bounds)
		{
			drawer.Draw(g, bounds);
		}

		#endregion

	}
}

