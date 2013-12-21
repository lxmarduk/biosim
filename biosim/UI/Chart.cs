using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Biosim.UI
{
	public class Chart : Control
	{
		List<double> series;
		double maxY;

		public List<double> Series {
			get {
				return series;
			}
		}

		public Chart()
		{
			series = new List<double>();
			Size = new Size(300, 100);
			Paint += HandlePaint;
			BackColor = Color.SkyBlue;
		}

		void HandlePaint(object sender, PaintEventArgs e)
		{
			if (series.Count == 0)
				return;
			float scaleX = (float)Width / (float)series.Count;
			float scaleY = (float)Height / (float)maxY;
			if (series.Count > 1) {
				for (int i = 1; i < series.Count; ++i) {
					if (i >= series.Count)
						break;
					e.Graphics.DrawLine(Pens.Red, new Point((int)((i - 1) * scaleX), Height - (int)(series [i - 1] * scaleY)),
						new Point((int)(i * scaleX), Height - (int)(series [i] * scaleY)));
				}
			} else {
				e.Graphics.DrawLine(Pens.Red, new Point(0, Height), new Point(Width, Height - (int)(series [0] * scaleY)));
			}
		}

		public Chart(Control parent) : this()
		{
			Parent = parent;
		}

		public void AddY(double y)
		{
			if (y > maxY)
				maxY = y;
			series.Add(y);
		}
	}
}

