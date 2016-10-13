using System;
using System.Windows.Forms;
using Biosim.Abstraction;
using Biosim.Implementation;
using System.Drawing;

namespace Biosim.UI
{
	public sealed class NewMapDialog : Form
	{
		Button apply;
		Button cancel;
		Label lbl_width;
		NumericUpDown width;
		Label lbl_height;
		NumericUpDown height;
		Label lbl_type;
		ComboBox map_type;

		public Map Map {
			get;
			private set;
		}

		public NewMapDialog()
		{
			initializeUI();
		}

		void initializeUI()
		{
            Text = "Створити мапу";
			FormBorderStyle = FormBorderStyle.FixedDialog;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Margin = new Padding(8);
			StartPosition = FormStartPosition.CenterScreen;

			lbl_width = new Label();
			lbl_width.Text = "Ширина:";
			lbl_width.Location = new System.Drawing.Point(0, 0);
			width = new NumericUpDown();
			width.Value = 16;
			width.Location = new System.Drawing.Point(lbl_width.Right + 5, 0);
			width.Width = 100;

			lbl_height = new Label();
			lbl_height.Text = "Висота:";
			lbl_height.Location = new System.Drawing.Point(0, lbl_width.Bottom + 5);
			height = new NumericUpDown();
			height.Value = 16;
			height.Width = 100;
			height.Location = new System.Drawing.Point(lbl_height.Right + 5, lbl_height.Top);

			lbl_type = new Label();
			lbl_type.Text = "Тип:";
			lbl_type.Location = new System.Drawing.Point(0, lbl_height.Bottom + 5);
			map_type = new ComboBox();
			map_type.Location = new System.Drawing.Point(lbl_type.Right + 5, lbl_type.Top);
			map_type.Items.Add(Utils.MapTypeToString(Map.MapType.Box));
			map_type.Items.Add(Utils.MapTypeToString(Map.MapType.Continuous));
			map_type.DropDownStyle = ComboBoxStyle.DropDownList;
			map_type.SelectedIndex = 0;

			lbl_width.Parent = this;
			lbl_height.Parent = this;
			lbl_type.Parent = this;
			width.Parent = this;
			height.Parent = this;
			map_type.Parent = this;

			apply = new Button();
			apply.Text = "Створити";
			apply.Location = new System.Drawing.Point(Width - apply.Width, map_type.Bottom + 8);
			apply.DialogResult = DialogResult.OK;
			apply.Parent = this;
			AcceptButton = apply;

			cancel = new Button();
			cancel.Text = "Відмінити";
			cancel.Location = new System.Drawing.Point(apply.Left - cancel.Width - 5, apply.Top);
			cancel.DialogResult = DialogResult.Cancel;
			cancel.Parent = this;

			DialogResult = DialogResult.Cancel;
		}

		public new DialogResult Show()
		{
			Map = null;
			if (ShowDialog() == DialogResult.OK) {
				Map = new Map((int)width.Value, (int)height.Value);
				switch (map_type.SelectedIndex) {
					case 0:
						Map.Type = Map.MapType.Box;
						break;
					case 1:
						Map.Type = Map.MapType.Continuous;
						break;
				}
			}
			return DialogResult;
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

