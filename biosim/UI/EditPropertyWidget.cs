using System;
using System.Windows.Forms;
using System.Drawing;
using Biosim.Abstraction;
using Biosim.Implementation;

namespace Biosim.UI
{
	public class EditPropertyWidget : UserControl
	{
		AbstractProperty prop;
		TextBox txt_name;
		ComboBox cb_type;
		TextBox txt_val;
		ComboBox cb_bool;

		public AbstractProperty Property {
			get {
				return prop;
			}
			set {
				prop = value;
				loadPropertyData();
				check();
			}
		}

		bool canSaveData;

		public bool CanSaveData {
			get {
				return canSaveData;
			}
			private set {
				canSaveData = value;
				if (CanSaveDataChanged != null) {
					CanSaveDataChanged(null, null);
				}
			}
		}

		public event EventHandler CanSaveDataChanged;

		public EditPropertyWidget()
		{
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			intializeUi();
			CanSaveData = true;
		}

		void intializeUi()
		{
			Label lbl_name = new Label();
			lbl_name.Text = "Ім'я:";
			lbl_name.Parent = this;
			lbl_name.Width = 100;

			txt_name = new TextBox();
			txt_name.Text = "";
			txt_name.Width = 200;
			txt_name.Parent = this;
			txt_name.Location = new Point(lbl_name.Right + 5, 0);

			Label lbl_type = new Label();
			lbl_type.Text = "Тип:";
			lbl_type.Parent = this;
			lbl_type.Width = 100;
			lbl_type.Location = new Point(0, lbl_name.Bottom + 5);

			cb_type = new ComboBox();
			cb_type.Items.Add("Текст");
			cb_type.Items.Add("Булеве значення");
			cb_type.Items.Add("Ціле число");
			cb_type.Items.Add("Дробове число");
			cb_type.SelectedIndex = 0;
			cb_type.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_type.Width = 200;
			cb_type.Parent = this;
			cb_type.Location = new Point(lbl_type.Right + 5, lbl_type.Top);
			cb_type.SelectedIndexChanged += HandleSelectedIndexChanged;

			Label lbl_val = new Label();
			lbl_val.Text = "Значення:";
			lbl_val.Parent = this;
			lbl_val.Width = 100;
			lbl_val.Location = new Point(0, lbl_type.Bottom + 5);

			txt_val = new TextBox();
			txt_val.Text = "0";
			txt_val.Width = 200;
			txt_val.Parent = this;
			txt_val.Location = new Point(lbl_val.Right + 5, lbl_val.Top);
			txt_val.TextChanged += HandleTextChanged;

			cb_bool = new ComboBox();
			cb_bool.Items.Add("хибне");
			cb_bool.Items.Add("істинне");
			cb_bool.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_bool.SelectedIndex = 0;
			cb_bool.Width = 200;
			cb_bool.Parent = this;
			cb_bool.Location = txt_val.Location;
			cb_bool.Visible = false;
		}

		void HandleTextChanged(object sender, EventArgs e)
		{
			string t = txt_val.Text;
			int i;
			float f;
			switch (cb_type.SelectedIndex) {
				case 2: //IntProperty
					if (!Int32.TryParse(t, out i)) {
						txt_val.BackColor = Color.IndianRed;
						CanSaveData = false;
					} else {
						txt_val.BackColor = Color.White;
						CanSaveData = true;
					}
					break;
				case 3: //FloatProperty
					int p = txt_val.SelectionStart;
					txt_val.Text = txt_val.Text.Replace(',', '.');
					txt_val.SelectionStart = p;
					t = t.Replace(',', '.');
					if (!float.TryParse(t, out f) || (t.Split(new []{ '.' }, StringSplitOptions.None).Length > 2)) {
						txt_val.BackColor = Color.IndianRed;
						CanSaveData = false;
					} else {
						txt_val.BackColor = Color.White;
						CanSaveData = true;
					}
					break;
			}
		}

		void HandleSelectedIndexChanged(object sender, EventArgs e)
		{
			txt_val.BackColor = Color.White;
			if (cb_type.SelectedIndex == 1) { //bool
				txt_val.Visible = false;
				cb_bool.Visible = true;
				cb_bool.SelectedIndex = 0;
			} else {
				txt_val.Visible = true;
				txt_val.Text = "0";
				cb_bool.Visible = false;
			}
			HandleTextChanged(null, null);
		}

		void loadPropertyData()
		{
			if (prop == null) {
				return;
			}

			txt_name.Text = prop.Name;
			if (prop.GetType().Equals(typeof(StringProperty))) {
				cb_type.SelectedIndex = 0;
				txt_val.Visible = true;
				cb_bool.Visible = false;
				txt_val.Text = prop.Value.ToString();
			} else if (prop.GetType().Equals(typeof(BoolProperty))) {
				cb_type.SelectedIndex = 1;
				txt_val.Visible = false;
				cb_bool.Visible = true;
				cb_bool.SelectedIndex = (bool)prop.Value ? 1 : 0;
			} else if (prop.GetType().Equals(typeof(IntProperty))) {
				cb_type.SelectedIndex = 2;
				txt_val.Visible = true;
				cb_bool.Visible = false;
				txt_val.Text = prop.Value.ToString();
			} else if (prop.GetType().Equals(typeof(FloatProperty))) {
				cb_type.SelectedIndex = 3;
				txt_val.Visible = true;
				cb_bool.Visible = false;
				txt_val.Text = prop.Value.ToString();
			} else {
				return;
			}
		}

		void check()
		{
			if (prop.Name.Equals("Ім'я") || prop.Name.Equals("Жива")) {
				txt_name.Enabled = false;
				cb_type.Enabled = false;
				txt_val.Enabled = true;
			} else if (prop.Name.Equals("Сусіди")) {
				txt_name.Enabled = false;
				cb_type.Enabled = false;
				txt_val.Enabled = false;
			} else {
				txt_name.Enabled = true;
				cb_type.Enabled = true;
				txt_val.Enabled = true;
			}
		}

		public bool SaveData()
		{
			if (!CanSaveData) {
				return false;
			}
			AbstractProperty result = null;
			switch (cb_type.SelectedIndex) {
				case 0: //StringProperty
					result = new StringProperty(txt_name.Text, txt_val.Text);
					break;
				case 1: //BoolProperty
					result = new BoolProperty(txt_name.Text, cb_bool.SelectedIndex == 1);
					break;
				case 2: //IntProperty
					result = new IntProperty(txt_name.Text, Int32.Parse(txt_val.Text));
					break;
				case 3: //FloatProperty
					result = new FloatProperty(txt_name.Text, float.Parse(txt_val.Text));
					break;
			}
			if (result != null) {
				prop = result.Clone();
				return true;
			}
			return false;
		}
	}
}

