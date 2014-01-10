using System;
using System.IO;
using System.Windows.Forms;
using Biosim.Abstraction;
using Biosim.Implementation;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace Biosim.UI
{
	public class CellView : UserControl
	{
		ListBox view;

		public ListBox View {
			get {
				return view;
			}
		}

		public AbstractCell SelectedCell {
			get {
				if (view.SelectedIndex != -1) {
					AbstractCell cell = (view.Items [view.SelectedIndex] as AbstractCell);
					return cell != null ? cell.Clone() : null;
				}
				return null;
			}
		}

		public CellView()
		{
			Dock = DockStyle.Top;
			Height = 400;
			view = new ListBox();
			view.Parent = this;
			view.DrawItem += HandleDrawItem;
			view.DrawMode = DrawMode.OwnerDrawFixed;
			view.Dock = DockStyle.Fill;
			view.ItemHeight = 32;

			loadCells();
		}

		void HandleDrawItem(object sender, DrawItemEventArgs e)
		{
			e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			if (e.State.HasFlag(DrawItemState.Selected)) {
				e.Graphics.FillRectangle(new LinearGradientBrush(e.Bounds, Color.AliceBlue, Color.SkyBlue, LinearGradientMode.Vertical), e.Bounds);
			}

			Cell cell = (Cell)view.Items [e.Index];
			cell.DrawIcon(e.Graphics, new Rectangle(e.Bounds.Left + 4, e.Bounds.Top + 4, 24, 24));
			e.Graphics.DrawString(cell.Properties ["Ім'я"].Value.ToString(), 
				view.Font, 
				Brushes.Black, 
				e.Bounds.Left + 32, 
				e.Bounds.Top + 8);

			if (e.State.HasFlag(DrawItemState.Focus)) {
				Pen focusPen = new Pen(Brushes.Black);
				focusPen.DashStyle = DashStyle.Dash;
				e.Graphics.DrawRectangle(focusPen, e.Bounds);
			}
		}

		public CellView(Control parent) : this()
		{
			Parent = parent;
		}

		void loadCells()
		{
			if (!Directory.Exists("cells")) {
				return;
			}

			List<string> files = new List<String>(Directory.GetFiles("cells", "*.bin"));
			foreach (String file in files) {
				try {
					view.Items.Add(Utils.DeserializeCell(file).Clone());
				} catch (Exception e) {
					Console.WriteLine(e.Message);
				}
			}
		}

		public void Reload()
		{
			view.Items.Clear();
			loadCells();
		}

		public void RemoveCell(int selectedIndex)
		{
			view.Items.RemoveAt(selectedIndex);
		}

		public void SerializeCells()
		{
			for (int i = 0; i < View.Items.Count; ++i) {
				Cell c = (View.Items [i] as Cell);
				if (c != null) {
					Utils.Serialize(c, c.Properties ["Ім'я"].Value.ToString());
				}
			}
		}
	}
}

