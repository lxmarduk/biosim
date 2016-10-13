using System;
using System.Windows.Forms;
using System.Drawing;

namespace Biosim.UI
{
	public class ColorWidget : UserControl
	{
		GroupBox box;
		Panel colorPreview;
		TrackBar range_r;
		TrackBar range_g;
		TrackBar range_b;

		public event EventHandler ColorChanged;

		public Color Color {
			get {
				return colorPreview.BackColor;
			}
			set {
				Reset(value);
			}
		}

		public ColorWidget()
		{
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			AutoSize = true;
			initializeUI();
		}

		void initializeUI()
		{
            box = new GroupBox();
			box.Text = "Колір";
			box.SetBounds(0, 0, 200, 100);
			box.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			box.AutoSize = true;
			box.Parent = this;

			colorPreview = new Panel();
			colorPreview.SetBounds(16, 24, 24, 24);
			colorPreview.BorderStyle = BorderStyle.Fixed3D;
			colorPreview.Parent = box;

			Label lbl_r = new Label();
			lbl_r.Text = "Червоний";
			lbl_r.Location = new Point(colorPreview.Right + 24, colorPreview.Top - 8);
			lbl_r.Parent = box;

			range_r = new TrackBar();
			range_r.Minimum = 0;
			range_r.Maximum = 255;
			range_r.Value = 0;
			range_r.TickStyle = TickStyle.None;
			range_r.Location = new Point(lbl_r.Left, lbl_r.Bottom + 3);
			range_r.Width = 255;
			range_r.Parent = box;
			range_r.ValueChanged += (sender, e) => {
				Color c = colorPreview.BackColor;
				Color r = Color.FromArgb(range_r.Value, c.G, c.B);
				colorPreview.BackColor = r;
				if (ColorChanged != null) {
					ColorChanged(null, null);
				}
			};

			Label lbl_g = new Label();
			lbl_g.Text = "Зелений";
			lbl_g.Location = new Point(colorPreview.Right + 24, range_r.Bottom + 5);
			lbl_g.Parent = box;
			range_g = new TrackBar();
			range_g.Minimum = 0;
			range_g.Maximum = 255;
			range_g.Value = 0;
			range_g.TickStyle = TickStyle.None;
			range_g.Location = new Point(lbl_g.Left, lbl_g.Bottom + 3);
			range_g.Width = 255;
			range_g.Parent = box;
			range_g.ValueChanged += (sender, e) => {
				Color c = colorPreview.BackColor;
				Color r = Color.FromArgb(c.R, range_g.Value, c.B);
				colorPreview.BackColor = r;
				if (ColorChanged != null) {
					ColorChanged(null, null);
				}
			};

			Label lbl_b = new Label();
			lbl_b.Text = "Синій";
			lbl_b.Location = new Point(colorPreview.Right + 24, range_g.Bottom + 5);
			lbl_b.Parent = box;
			range_b = new TrackBar();
			range_b.Minimum = 0;
			range_b.Maximum = 255;
			range_b.Value = 0;
			range_b.TickStyle = TickStyle.None;
			range_b.Location = new Point(lbl_b.Left, lbl_b.Bottom + 3);
			range_b.Width = 255;
			range_b.Parent = box;
			range_b.ValueChanged += (sender, e) => {
				Color c = colorPreview.BackColor;
				Color r = Color.FromArgb(c.R, c.G, range_b.Value);
				colorPreview.BackColor = r;
				if (ColorChanged != null) {
					ColorChanged(null, null);
				}
			};

			Reset(Color.Lime);
		}

		public void Reset(Color c)
		{
			range_r.Value = c.R;
			range_g.Value = c.G;
			range_b.Value = c.B;
			colorPreview.BackColor = c;
		}
	}
}

