using System;
using System.Windows.Forms;
using System.Drawing;
using Biosim.Abstraction;
using Biosim.Implementation;

namespace Biosim.UI
{
	public sealed class NewPropertyDialog : Form
	{
		Button apply;
		Button cancel;
		ComboBox prop_type;
		TextBox txt_name;
		TextBox txt_initialValue;
		ComboBox boolDropDown;

		public AbstractProperty Property {
			get;
			private set;
		}

		public NewPropertyDialog()
		{
			Text = "Створити властивість";
			FormBorderStyle = FormBorderStyle.FixedDialog;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Margin = new Padding(8);
			StartPosition = FormStartPosition.CenterScreen;
			AcceptButton = apply;

			initializeUI();
		}

		void initializeUI()
		{

			Label lbl_name = new Label();
			lbl_name.Width = 100;
			lbl_name.Text = "Ім'я:";
			lbl_name.Parent = this;
			txt_name = new TextBox();
			txt_name.Width = 200;
			txt_name.Text = "Нова властивість";
			txt_name.Location = new Point(lbl_name.Right + 5, 0);
			txt_name.Parent = this;

			Label lbl_type = new Label();
			lbl_type.Text = "Тип:";
			lbl_type.Width = 100;
			lbl_type.Location = new Point(0, lbl_name.Bottom + 5);
			lbl_type.Parent = this;
			prop_type = new ComboBox();
			prop_type.Width = 200;
			prop_type.Location = new Point(lbl_type.Right + 5, lbl_type.Top);
			prop_type.Items.Add("Текст");
			prop_type.Items.Add("Булеве значення");
			prop_type.Items.Add("Ціле число");
			prop_type.Items.Add("Дробове число");
			prop_type.DropDownStyle = ComboBoxStyle.DropDownList;
			prop_type.SelectedIndex = 0;
			prop_type.Parent = this;
			prop_type.SelectedIndexChanged += HandleSelectedIndexChanged;

			Label lbl_init = new Label();
			lbl_init.Width = 100;
			lbl_init.Text = "Початкове значення:";
			lbl_init.Location = new Point(0, lbl_type.Bottom + 5);
			lbl_init.Parent = this;
			txt_initialValue = new TextBox();
			txt_initialValue.Width = 200;
			txt_initialValue.Text = "0";
			txt_initialValue.Location = new Point(lbl_init.Right + 5, lbl_init.Top);
			txt_initialValue.Parent = this;
			txt_initialValue.TextChanged += HandleTextChanged;
			boolDropDown = new ComboBox();
			boolDropDown.Width = 200;
			boolDropDown.Location = new Point(lbl_init.Right + 5, lbl_init.Top);
			boolDropDown.Visible = false;
			boolDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
			boolDropDown.Items.Add("хибне");
			boolDropDown.Items.Add("істинне");
			boolDropDown.Parent = this;

			apply = new Button();
			apply.Text = "Створити";
			apply.Location = new Point(Width - apply.Width, txt_initialValue.Bottom + 8);
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

		void HandleSelectedIndexChanged(object sender, EventArgs e)
		{
			txt_initialValue.BackColor = Color.White;
			if (prop_type.SelectedIndex == 1) { //bool
				txt_initialValue.Visible = false;
				boolDropDown.Visible = true;
				boolDropDown.SelectedIndex = 0;
			} else {
				txt_initialValue.Visible = true;
				txt_initialValue.Text = "0";
				boolDropDown.Visible = false;
			}
			HandleTextChanged(null, null);
		}

		void HandleTextChanged(object sender, EventArgs e)
		{
			string t = txt_initialValue.Text;
			int i;
			float f;
			switch (prop_type.SelectedIndex) {
				case 2: //IntProperty
					if (!Int32.TryParse(t, out i)) {
						txt_initialValue.BackColor = Color.IndianRed;
						apply.Enabled = false;
					} else {
						txt_initialValue.Text = i.ToString();
						txt_initialValue.BackColor = Color.White;
						apply.Enabled = true;
					}
					break;
				case 3: //FloatProperty
					int p = txt_initialValue.SelectionStart;
					txt_initialValue.Text = txt_initialValue.Text.Replace(',', '.');
					txt_initialValue.SelectionStart = p;
					t = t.Replace(',', '.');
					if (!float.TryParse(t, out f) || (t.Split(new []{ '.' }, StringSplitOptions.None).Length > 2)) {
						txt_initialValue.BackColor = Color.IndianRed;
						apply.Enabled = false;
					} else {
						txt_initialValue.BackColor = Color.White;
						apply.Enabled = true;
					}
					break;
			}
		}

		public new DialogResult Show()
		{
			Property = null;
			AbstractProperty result = null;

			txt_name.Text = "Нова властивість";
			prop_type.SelectedIndex = 0;
			txt_initialValue.Text = "0";
			txt_initialValue.BackColor = Color.White;
			txt_initialValue.Visible = true;
			boolDropDown.Visible = false;

			txt_name.Focus();
			txt_name.SelectAll();
			ShowDialog();
			if (DialogResult == DialogResult.OK) {
				switch (prop_type.SelectedIndex) {
					case 0: //StringProperty
						result = new StringProperty(txt_name.Text, txt_initialValue.Text);
						break;
					case 1: //BoolProperty
						result = new BoolProperty(txt_name.Text, boolDropDown.SelectedIndex == 1);
						break;
					case 2: //IntProperty
						result = new IntProperty(txt_name.Text, Int32.Parse(txt_initialValue.Text));
						break;
					case 3: //FloatProperty
						result = new FloatProperty(txt_name.Text, float.Parse(txt_initialValue.Text));
						break;
				}
			}
			if (result != null) {
				Property = result.Clone();
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

