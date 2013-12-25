using System;
using System.Windows.Forms;
using System.Drawing;
using Biosim.Abstraction;
using Biosim.Implementation;

namespace Biosim.UI
{
	public static class EditPropertyDialog
	{
		static Form form;
		static EditPropertyWidget w;
		static Button apply;
		static Button cancel;

		public static AbstractProperty EditableProperty {
			get {
				return w.Property;
			}
			set {
				w.Property = value;
			}
		}

		static EditPropertyDialog()
		{
			form = new Form();
			form.Text = "Редагувати властивість";
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.AutoSize = true;
			form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			form.Margin = new Padding(8);
			form.StartPosition = FormStartPosition.CenterScreen;
			form.AcceptButton = apply;

			w = new EditPropertyWidget();
			w.Parent = form;
			w.CanSaveDataChanged += HandleCanSaveDataChanged;

			apply = new Button();
			apply.Text = "Змінити";
			apply.Location = new Point(form.Width - apply.Width, w.Bottom + 8);
			apply.DialogResult = DialogResult.OK;
			apply.Parent = form;
			form.AcceptButton = apply;

			cancel = new Button();
			cancel.Text = "Відмінити";
			cancel.Location = new Point(apply.Left - cancel.Width - 5, apply.Top);
			cancel.DialogResult = DialogResult.Cancel;
			cancel.Parent = form;

			form.DialogResult = DialogResult.Cancel;
		}

		static void HandleCanSaveDataChanged(object sender, EventArgs e)
		{
			if (w.CanSaveData) {
				apply.Enabled = true;
			} else {
				apply.Enabled = false;
			}
		}

		public static DialogResult Show()
		{
			if (form.ShowDialog() == DialogResult.OK) {
				w.SaveData();
				EditableProperty = w.Property.Clone();
				return DialogResult.OK;
			}
			return DialogResult.Cancel;
		}
	}
}

