using System;
using System.Windows.Forms;
using Biosim.Implementation;
using System.Drawing;

namespace Biosim.UI
{
	public class CellPreviewWidget : UserControl
	{
		GroupBox previewBox;
		Panel preview;
		Bitmap b;
		Cell cell;

		public Cell PreviewCell {
			get {
				return cell;
			}
			set {
				cell = (Cell)value.Clone();
				Preview(cell);
			}
		}

		public CellPreviewWidget()
		{
			initializeUI();
		}

		void initializeUI()
		{
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			AutoSize = true;

			previewBox = new GroupBox();
			previewBox.Text = "Перегляд";
			previewBox.Parent = this;
			previewBox.SetBounds(0, 0, 150, 150);

			preview = new Panel();
			preview.Parent = previewBox;
			preview.BorderStyle = BorderStyle.Fixed3D;
			preview.Width = 48;
			preview.Height = 48;
			preview.Location = new Point(51, 51);

			b = new Bitmap(48, 48);
		}

		public void Preview(Cell c)
		{
			if (c == null) {
				return;
			}
			Graphics g = Graphics.FromImage(b);
			g.Clear(preview.BackColor);
			c.DrawIcon(g, new Rectangle(7, 7, 32, 32));
			g.Dispose();
			preview.BackgroundImage = b;
			preview.Refresh();
		}
	}
}

