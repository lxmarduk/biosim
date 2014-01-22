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
			set {
				SetMap(value);
			}
		}

		public MapVizualizer(Control control, Map map) : base()
		{
			Parent = control;
			Cursor = Cursors.Cross;
			Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
			AutoScroll = true;

			panel = new Panel();
			panel.Paint += HandlePaint;

			Map = map;

			panel.Location = new Point(0, 0);
			panel.Parent = this;
		}

		void HandlePaint(object sender, PaintEventArgs e)
		{
			if (map == null) {
				return;
			}
			Image img = new Bitmap(panel.Width, panel.Height);
			Graphics gr = Graphics.FromImage(img);
			for (int i = 0; i < map.Width; ++i) {
				for (int j = 0; j < map.Height; ++j) {
					if (map.Selector.Select(i, j) != null) {
						map.Selector.Select(i, j).Draw(gr, new Rectangle(i * CellSize, j * CellSize, CellSize, CellSize));
					}
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
			updateSize();
		}

		public void updateSize()
		{
			panel.Size = new Size(CellSize * map.Width, CellSize * map.Height);
		}
	}
}

