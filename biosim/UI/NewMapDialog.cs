using System;
using System.Windows.Forms;
using Biosim.Abstraction;
using Biosim.Implementation;

namespace Biosim.UI
{
	public static class NewMapDialog
	{
		static Form form;
		static Button apply;
		static Button cancel;
		static Label lbl_width;
		static NumericUpDown width;
		static Label lbl_height;
		static NumericUpDown height;
		static Label lbl_type;
		static ComboBox map_type;

		public static Map Map {
			get;
			private set;
		}

		static NewMapDialog()
		{
			form = new Form();
			form.Text = "Створити мапу";
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.AutoSize = true;
			form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			form.Margin = new Padding(8);
			form.StartPosition = FormStartPosition.CenterScreen;

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

			lbl_width.Parent = form;
			lbl_height.Parent = form;
			lbl_type.Parent = form;
			width.Parent = form;
			height.Parent = form;
			map_type.Parent = form;

			apply = new Button();
			apply.Text = "Створити";
			apply.Location = new System.Drawing.Point(form.Width - apply.Width, map_type.Bottom + 8);
			apply.DialogResult = DialogResult.OK;
			apply.Parent = form;
			form.AcceptButton = apply;

			cancel = new Button();
			cancel.Text = "Відмінити";
			cancel.Location = new System.Drawing.Point(apply.Left - cancel.Width - 5, apply.Top);
			cancel.DialogResult = DialogResult.Cancel;
			cancel.Parent = form;

			form.DialogResult = DialogResult.Cancel;
		}

		public static DialogResult Show()
		{
			Map = null;
			form.ShowDialog();
			if (form.DialogResult == DialogResult.OK) {
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
			return form.DialogResult;
		}
	}
}

