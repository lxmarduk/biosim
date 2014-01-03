using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using Biosim.Implementation;
using System.Text;

namespace Biosim.UI
{
	public class RealTimeChart : UserControl
	{
		List<SeriesCollection> series;
		double totalMaxValue;
		ToolTip toolTip;

		public List<SeriesCollection> Series {
			get {
				return series;
			}
		}

		public float HorizontalScale {
			get;
			set;
		}

		public int OffsetX {
			get;
			set;
		}

		public int OffsetY {
			get;
			set;
		}

		public bool FullSizedChart {
			get;
			set;
		}

		public RealTimeChart()
		{
			series = new List<SeriesCollection>();
			MinimumSize = new Size(300, 150);
			AutoSize = false;
			Paint += HandlePaint;
			BackColor = Color.White;
			DoubleBuffered = true;
			HorizontalScale = 10.0f;
			OffsetX = 50;
			OffsetY = 30;
			MouseMove += HandleMouseMove;

			toolTip = new ToolTip();
			toolTip.UseAnimation = true;
			toolTip.UseFading = true;
			toolTip.ShowAlways = true;

			FullSizedChart = true;
		}

		void HandleMouseMove(object sender, MouseEventArgs e)
		{
			if (series == null || series.Count == 0) {
				return;
			}
			if (e.X < OffsetX) {
				return;
			}
			int x = (int)((e.X - OffsetX) / HorizontalScale);

			int startIndex = 0;
			if (!FullSizedChart) {
				if (series [0].Count > ((Width - OffsetX - 25) / HorizontalScale)) {
					startIndex = series [0].Count - (int)((Width - OffsetX - 25) / HorizontalScale);
				}
			}
			x += startIndex;
			if (x < 0 || x >= series [0].Count) {
				return;
			}
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Крок: {0:D}\n", x);
			for (int i = 0; i < series.Count; ++i) {
				if (series [i].Count != 0 && x >= 0 && x < series [i].Count) {
					sb.AppendFormat("{0}: {1:F}\n", series [i].Name, series [i] [x]);
				}
			}
			toolTip.SetToolTip(this, sb.ToString());
		}

		public RealTimeChart(Control parent) : this()
		{
			Parent = parent;
		}

		void HandlePaint(object sender, PaintEventArgs e)
		{
			if (series.Count == 0)
				return;
			e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

			totalMaxValue = 0.0;
			for (int i = 0; i < series.Count; ++i) {
				if (series [i].MaxValue > totalMaxValue) {
					totalMaxValue = series [i].MaxValue;
				}
			}
			DrawAxis(e.Graphics);
			for (int i = 0; i < series.Count; ++i) {
				DrawSeries(series [i], e.Graphics);
			}
		}

		void DrawSeries(SeriesCollection s, Graphics g)
		{
			if (s.Count == 0) {
				return;
			}

			Pen colorPen = new Pen(s.Color);
			//colorPen.Width = 2;

			float maxVal = totalMaxValue > GetBiggestNumber() ? (float)totalMaxValue : GetBiggestNumber();
			float scaleY = ((float)Height - OffsetY * 2) / maxVal;

			PointF[] points;

			if (s.Count > 1) {
				int startIndex = 0;
				if (!FullSizedChart) {
					if (s.Count > ((Width - OffsetX - 25) / HorizontalScale)) {
						startIndex = s.Count - (int)((Width - OffsetX - 25) / HorizontalScale);
					}
				}
				points = new PointF[s.Count - startIndex];
				for (int i = 0; i < points.Length; ++i) {
					points [i] = new PointF(i * HorizontalScale + OffsetX, (float)(Height - s [i + startIndex] * scaleY) - OffsetY);
				}
				g.DrawCurve(colorPen, points, 0.1f);
			} else {
				g.DrawLine(colorPen, new Point(OffsetX, Height - OffsetY), new Point(OffsetX, (int)(Height - s [0] * scaleY) - OffsetY));
			}
		}

		void DrawAxis(Graphics g)
		{
			// Y axis
			g.DrawLine(Pens.Black, new Point(OffsetX, Height - OffsetY + 5), new Point(OffsetX, OffsetY - 5));
			// X axis
			g.DrawLine(Pens.Black, new Point(OffsetX - 5, Height - OffsetY), new Point(Width - 30, Height - OffsetY));

			float maxValue = totalMaxValue > GetBiggestNumber() ? (float)totalMaxValue : GetBiggestNumber();
			float scaleY = ((float)Height - OffsetY * 2) / maxValue;
			string maxVal = string.Format("{0:D}", (int)totalMaxValue);
			SizeF strSize = g.MeasureString(maxVal, Font);
			if (strSize.Width > OffsetX) {
				OffsetX = (int)(strSize.Width + 5);
			}
			DrawNumbersY(g);
			DrawNumbersX(g);
			g.FillRectangle(new SolidBrush(BackColor), 
				new RectangleF(OffsetX - strSize.Width - 3, (float)(Height - totalMaxValue * scaleY) - OffsetY, strSize.Width, strSize.Height));
			g.DrawString(maxVal, 
				Font, 
				Brushes.Black, 
				new PointF(
					OffsetX - strSize.Width - 3, 
					(float)(Height - totalMaxValue * scaleY) - OffsetY
				)
			);
		}

		void DrawNumbersY(Graphics g)
		{
			float maxValue = totalMaxValue > GetBiggestNumber() ? (float)totalMaxValue : GetBiggestNumber();
			float scaleY = ((float)Height - OffsetY * 2) / maxValue;
			float bigNum = GetBiggestNumber();
			string quart1 = String.Format("{0:D}", (int)(bigNum / 4.0f));
			string half = String.Format("{0:D}", (int)(bigNum / 2.0f));
			string quart2 = String.Format("{0:D}", (int)(bigNum / 2.0f + bigNum / 4.0f));
			string full = String.Format("{0:D}", (int)(bigNum));

			SizeF s = g.MeasureString(half, Font);
			float x = OffsetX - s.Width - 3;
			float y = (Height - bigNum / 2 * scaleY) - OffsetY;
			g.DrawString(half, Font, Brushes.Black, x, y);

			s = g.MeasureString(full, Font);
			x = OffsetX - s.Width - 3;
			y = (Height - bigNum * scaleY) - OffsetY;
			g.DrawString(full, Font, Brushes.Black, x, y);

			s = g.MeasureString(quart1, Font);
			x = OffsetX - s.Width - 3;
			y = (Height - (bigNum / 4.0f) * scaleY) - OffsetY;
			g.DrawString(quart1, Font, Brushes.Black, x, y);

			s = g.MeasureString(quart2, Font);
			x = OffsetX - s.Width - 3;
			y = (Height - (bigNum / 2.0f + bigNum / 4.0f) * scaleY) - OffsetY;
			g.DrawString(quart2, Font, Brushes.Black, x, y);
		}

		void DrawNumbersX(Graphics g)
		{
			if (series.Count > 0) {
				if (series [0].Count > 0) {
					int startIndex = 0;
					if (!FullSizedChart) {
						if (series [0].Count > ((Width - OffsetX * 2) / HorizontalScale)) {
							startIndex = series [0].Count - (int)((Width - OffsetX * 2) / HorizontalScale);
						}
					}
					string str = startIndex.ToString();
					g.DrawString(str, Font, Brushes.Black, OffsetX + 5, Height - OffsetY);
					string lastIndex = series [0].Count.ToString();
					if (startIndex == 0) {
						g.DrawString(lastIndex, Font, Brushes.Black, series [0].Count * HorizontalScale + OffsetX, Height - OffsetY);
					} else {
						g.DrawString(lastIndex, Font, Brushes.Black, Width - OffsetX, Height - OffsetY);
					}
				}
			}
		}

		float GetBiggestNumber()
		{
			if (totalMaxValue < 1) {
				return 1;
			}
			if (totalMaxValue <= 10) {
				return 10.0f;
			}
			if (totalMaxValue <= 100) {
				return ((int)Math.Round(totalMaxValue / 10.0f)) * 10;
			}
			if (totalMaxValue <= 1000) {
				return (float)(Math.Round(totalMaxValue / 100f) * 100);
			}
			if (totalMaxValue <= 10000) {
				return ((int)Math.Round(totalMaxValue / 1000f)) * 1000;
			}
			if (totalMaxValue <= 100000) {
				return ((int)Math.Round(totalMaxValue / 10000f)) * 10000;
			}
			if (totalMaxValue <= 1000000) {
				return ((int)Math.Round(totalMaxValue / 100000f)) * 100000;
			}
			if (totalMaxValue <= 10000000) {
				return ((int)Math.Round(totalMaxValue / 1000000f)) * 1000000;
			}
			return (float)totalMaxValue;
		}

		public void AddY(int seriesIndex, double y)
		{
			if (seriesIndex < 0 || seriesIndex >= series.Count) {
				return;
			}

			series [seriesIndex].Add(y);
			if (FullSizedChart) {
				int finalX = (int)(series [seriesIndex].Count * HorizontalScale + OffsetX);
				if (finalX > Width - 25) {
					Size = new Size(finalX + 25, Height);
				}
			}
			Refresh();
		}

		public void AddY(double y)
		{
			if (series.Count == 0) {
				series.Add(new SeriesCollection("Default"));
			}
			AddY(0, y);
		}

		public int AddSeries(SeriesCollection s)
		{
			series.Add(s);
			return series.Count - 1;
		}
	}
}

