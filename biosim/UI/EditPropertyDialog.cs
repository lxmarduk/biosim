using System;
using System.Windows.Forms;
using System.Drawing;
using Biosim.Abstraction;
using Biosim.Implementation;

namespace Biosim.UI
{
	public sealed class EditPropertyDialog : Form
	{
		EditPropertyWidget w;
		Button apply;
		Button cancel;

		public AbstractProperty EditableProperty {
			get {
				return w.Property;
			}
			set {
				w.Property = value;
			}
		}

		public EditPropertyDialog()
		{
			initializeUI();
		}

		void initializeUI()
		{
            Text = "Редагувати властивість";
			FormBorderStyle = FormBorderStyle.FixedDialog;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Margin = new Padding(8);
			StartPosition = FormStartPosition.CenterScreen;
			AcceptButton = apply;

			w = new EditPropertyWidget();
			w.Parent = this;
			w.CanSaveDataChanged += HandleCanSaveDataChanged;

			apply = new Button();
			apply.Text = "Змінити";
			apply.Location = new Point(Width - apply.Width, w.Bottom + 8);
			apply.DialogResult = DialogResult.OK;
			apply.Parent = this;
			AcceptButton = apply;

			cancel = new Button();
			cancel.Text = "Відмінити";
			cancel.Location = new Point(apply.Left - cancel.Width - 5, apply.Top);
			cancel.DialogResult = DialogResult.Cancel;
			cancel.Parent = this;

			DialogResult = DialogResult.Cancel;
		}

		void HandleCanSaveDataChanged(object sender, EventArgs e)
		{
			if (w.CanSaveData) {
				apply.Enabled = true;
			} else {
				apply.Enabled = false;
			}
		}

		public new DialogResult Show()
		{
			if (ShowDialog() == DialogResult.OK) {
				w.SaveData();
				EditableProperty = w.Property.Clone();
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

