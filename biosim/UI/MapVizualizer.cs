using System;
using System.Drawing;
using System.Windows.Forms;

using Biosim.Implementation;

namespace Biosim
{
	public class MapVizualizer : ScrollableControl
	{
		public static int CELL_SIZE = 8;

		Map map;
		Panel panel;

		public MapVizualizer(Form form, Map map) : base ()
		{
			this.map = map;

			Parent = form;
			Cursor = Cursors.Cross;
			Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
			this.AutoScroll = true;

			panel = new Panel();
			panel.Parent = this;
			panel.Paint += HandlePaint;

			panel.Location = new Point(0, 0);
			panel.Size = new Size(CELL_SIZE * map.Width, CELL_SIZE * map.Height);
			panel.MouseClick += HandleMouseClick;
		}

		void HandleMouseClick(object sender, MouseEventArgs e)
		{
			int x = e.X / CELL_SIZE;
			int y = e.Y / CELL_SIZE;
			Graphics g = panel.CreateGraphics();
			if (map.Selector.Select(x, y).Properties.HasProperty("Alive") && map.Selector.Select(x, y).Properties ["Alive"].Value.Equals((bool)true)) {
				map.Selector.Select(x, y).Properties ["Alive"].Unset();
			} else {
				map.Selector.Select(x, y).Properties ["Alive"].Set();
			}
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
					if (map.Selector.Select(i, j).Properties.HasProperty("Alive") && ((bool)map.Selector.Select(i, j).Properties ["Alive"].Value) == true) {
						gr.FillRectangle(Brushes.Lime, new RectangleF(i * CELL_SIZE, j * CELL_SIZE, CELL_SIZE, CELL_SIZE));
					} else {
						gr.FillRectangle(Brushes.Black, new RectangleF(i * CELL_SIZE, j * CELL_SIZE, CELL_SIZE, CELL_SIZE));
					}
					gr.DrawRectangle(Pens.Black, new Rectangle(i * CELL_SIZE, j * CELL_SIZE, CELL_SIZE, CELL_SIZE));
				}
			}
			e.Graphics.DrawImageUnscaled(img, new Point(0, 0));
			gr.Dispose();
			img.Dispose();
		}
	}
}

