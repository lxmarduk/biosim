using System;
using System.Windows.Forms;
using System.Drawing;
using Biosim.Abstraction;
using Biosim.Implementation;

namespace Biosim.UI
{
	public sealed class NewCellDialog : Form
	{
		TextBox txt_name;
		Button apply;
		Button cancel;
		ComboBox cb_shape;
		CellPreviewWidget preview;
		Cell previewCell;
		ColorWidget colorSelection;

		public Cell Cell {
			get;
			private set;
		}

		public NewCellDialog()
		{
			initializeUI();
		}

		void initializeUI()
		{
            Text = "Створити клітину";
			FormBorderStyle = FormBorderStyle.FixedDialog;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Margin = new Padding(8);
			StartPosition = FormStartPosition.CenterScreen;

			preview = new CellPreviewWidget();
			preview.Parent = this;

			Label lbl_name = new Label();
			lbl_name.Text = "Ім'я:";
			lbl_name.Parent = this;
			lbl_name.Width = 50;
			lbl_name.Location = new Point(preview.Right + 5, 5);

			txt_name = new TextBox();
			txt_name.Text = "Нова клітина";
			txt_name.Width = 150;
			txt_name.Location = new Point(lbl_name.Right + 5, 5);
			txt_name.Parent = this;

			Label lbl_shape = new Label();
			lbl_shape.Text = "Форма:";
			lbl_shape.Width = 50;
			lbl_shape.Parent = this;
			lbl_shape.Location = new Point(preview.Right + 5, lbl_name.Bottom + 5);
			cb_shape = new ComboBox();
			cb_shape.Items.Add("Квадрат");
			cb_shape.Items.Add("Круг");
			cb_shape.Items.Add("Трикутник");
			cb_shape.Items.Add("Ромб");
			cb_shape.Width = 150;
			cb_shape.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_shape.SelectedIndex = 0;
			cb_shape.Location = new Point(lbl_shape.Right + 5, lbl_shape.Top);
			cb_shape.Parent = this;
			cb_shape.SelectedIndexChanged += HandleSelectedIndexChanged;

			colorSelection = new ColorWidget();
			colorSelection.Parent = this;
			colorSelection.Location = new Point(lbl_shape.Left, lbl_shape.Bottom + 5);
			colorSelection.ColorChanged += (sender, e) => {
				previewCell.Color = colorSelection.Color;
				preview.Preview(previewCell);
			};

			apply = new Button();
			apply.Text = "Створити";
			apply.Location = new Point(colorSelection.Right - apply.Width, colorSelection.Bottom + 5);
			apply.DialogResult = DialogResult.OK;
			apply.Parent = this;
			AcceptButton = apply;

			cancel = new Button();
			cancel.Text = "Відмінити";
			cancel.Location = new Point(apply.Left - cancel.Width - 5, apply.Top);
			cancel.DialogResult = DialogResult.Cancel;
			cancel.Parent = this;

			txt_name.TextChanged += (sender, e) => {
				if (txt_name.Text.Equals(String.Empty)) {
					apply.Enabled = false;
				} else {
					apply.Enabled = true;
				}
			};

			Activated += (sender, e) => HandleSelectedIndexChanged(null, null);
		}

		void HandleSelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cb_shape.SelectedIndex) {
				case 0: //square
					previewCell.Shape = Biosim.Implementation.Cell.CellShape.Square;
					break;
				case 1: //circle
					previewCell.Shape = Biosim.Implementation.Cell.CellShape.Circle;
					break;
				case 2: //triangle
					previewCell.Shape = Biosim.Implementation.Cell.CellShape.Triangle;
					break;
				case 3: //diamond
					previewCell.Shape = Biosim.Implementation.Cell.CellShape.Diamond;
					break;
			}
			preview.PreviewCell = previewCell;
		}

		public new DialogResult Show()
		{
			Cell = null;
			cb_shape.SelectedIndex = 0;
			previewCell = null;
			previewCell = new Cell("Preview");
			preview.PreviewCell = previewCell;
			colorSelection.Reset(previewCell.Color);
			txt_name.Text = "Нова клітина";

			if (ShowDialog() == DialogResult.OK) {
				Cell = new Cell(txt_name.Text);
				switch (cb_shape.SelectedIndex) {
					case 0: //square
						Cell.Shape = Biosim.Implementation.Cell.CellShape.Square;
						break;
					case 1: //circle
						Cell.Shape = Biosim.Implementation.Cell.CellShape.Circle;
						break;
					case 2: //triangle
						Cell.Shape = Biosim.Implementation.Cell.CellShape.Triangle;
						break;
					case 3: //diamond
						Cell.Shape = Biosim.Implementation.Cell.CellShape.Diamond;
						break;
				}
				Cell.Color = colorSelection.Color;
				return DialogResult.OK;
			}
			return DialogResult.Cancel;	
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (Controls != null) {
					for (int i = 0; i < Controls.Count; ++i) {
						Controls [i].Dispose();
					}
				}
			}
			base.Dispose(disposing);
		}
	}
}

