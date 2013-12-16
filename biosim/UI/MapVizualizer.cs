using System;
using System.Drawing;
using System.Windows.Forms;
using Biosim.Implementation;

namespace Biosim
{
	public class MapVizualizer : ScrollableControl
	{
		public static int CellSize = 16;
		Map map;
		Panel panel;

		public Map Map {
			get {
				return map;
			}
		}

		public MapVizualizer(Control control, Map map) : base()
		{
			this.map = map;

			Parent = control;
			Cursor = Cursors.Cross;
			Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
			AutoScroll = true;

			panel = new Panel();
			panel.Parent = this;
			panel.Paint += HandlePaint;

			panel.Location = new Point(0, 0);
			panel.Size = new Size(CellSize * map.Width, CellSize * map.Height);
			panel.MouseClick += HandleMouseClick;
		}

		void HandleMouseClick(object sender, MouseEventArgs e)
		{
			int x = e.X / CellSize;
			int y = e.Y / CellSize;
			Graphics g = panel.CreateGraphics();
			if (map.Selector.Select(x, y).Properties.HasProperty("Alive") && map.Selector.Select(x, y).Properties ["Alive"].Equ((bool)true)) {
				map.Selector.Select(x, y).Properties ["Alive"].Unset();
			} else {
				map.Selector.Select(x, y).Properties ["Alive"].Set();
			}
			/*
			cell = map.Selector.Select(x, y) as Cell;
			if (cell == null) {
				return;
			}
			switch (cell.Shape) {
				case Cell.CellShape.Square:
					cell.Shape = Cell.CellShape.Circle;
					break;
				case Cell.CellShape.Circle:
					cell.Shape = Cell.CellShape.Triangle;
					break;
				case Cell.CellShape.Triangle:
					cell.Shape = Cell.CellShape.Diamond;
					break;
				case Cell.CellShape.Diamond:
					cell.Shape = Cell.CellShape.Square;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			Random rnd = new Random();
			Color c = Color.FromArgb(rnd.Next(0, 255),
				          rnd.Next(0, 255),
				          rnd.Next(0, 255));
			cell.Color = c;//*/
			g.Dispose();
			panel.Refresh();
		}

		void HandlePaint(object sender, PaintEventArgs e)
		{
			//e.Graphics.Clear(Color.Black);
			Image img = new Bitmap(panel.Width, panel.Height);
			Graphics gr = Graphics.FromImage(img);
			for (int i = 0; i < map.Width; ++i) {
				for (int j = 0; j < map.Height; ++j) {
					/*if (map.Selector.Select(i, j).Properties.HasProperty("Alive") && ((bool)map.Selector.Select(i, j).Properties ["Alive"].Value) == true) {
						gr.FillRectangle(Brushes.Lime, new RectangleF(i * CELL_SIZE, j * CELL_SIZE, CELL_SIZE, CELL_SIZE));
					} else {
						gr.FillRectangle(Brushes.Black, new RectangleF(i * CELL_SIZE, j * CELL_SIZE, CELL_SIZE, CELL_SIZE));
					}
					gr.DrawRectangle(Pens.Black, new Rectangle(i * CELL_SIZE, j * CELL_SIZE, CELL_SIZE, CELL_SIZE));*/
					map.Selector.Select(i, j).Draw(gr, new Rectangle(i * CellSize, j * CellSize, CellSize, CellSize));
				}
			}
			e.Graphics.DrawImageUnscaled(img, new Point(0, 0));
			gr.Dispose();
			img.Dispose();
		}

		public void SetMap(Map map)
		{
			this.map = null;
			this.map = map;
			panel.Size = new Size(CellSize * map.Width, CellSize * map.Height);
		}
	}
}

