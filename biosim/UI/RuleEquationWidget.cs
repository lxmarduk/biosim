using System;
using System.IO;
using System.Windows.Forms;
using Biosim.Abstraction;
using Biosim.Implementation;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace Biosim.UI
{
	public class RuleEquationWidget : RuleWidgetBase
	{
		String targetProperty;
		ComboBox cb_props;
		ComboBox cb_equality;
		ComboBox cb_textEq;
		TextBox txt_value;
		CheckBox ch_bool;

		public RuleEquationWidget()
		{
			initializeUI();
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
		}

		void initializeUI()
		{

			Label lbl_if = new Label();
			lbl_if.Text = "Якщо:";
			lbl_if.AutoSize = true;
			lbl_if.Location = new Point(0, 4);
			lbl_if.Parent = this;

			cb_props = new ComboBox();
			cb_props.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_props.DrawMode = DrawMode.OwnerDrawFixed;
			cb_props.DrawItem += DrawComboHandler;
			cb_props.Location = new Point(lbl_if.Right + 5, lbl_if.Top - 1);
			cb_props.Parent = this;
			cb_props.SelectedIndexChanged += PropsIndexChanged;

			cb_equality = new ComboBox();
			cb_equality.Items.Add("=");
			cb_equality.Items.Add("≠");
			cb_equality.Items.Add(">");
			cb_equality.Items.Add("<");
			cb_equality.Items.Add("≥");
			cb_equality.Items.Add("≤");
			cb_equality.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_equality.Width = 40;
			cb_equality.SelectedIndex = 0;
			cb_equality.Location = new Point(cb_props.Right + 5, cb_props.Top);
			cb_equality.Parent = this;
			cb_equality.DrawMode = DrawMode.OwnerDrawFixed;
			cb_equality.DrawItem += DrawComboHandler;

			cb_textEq = new ComboBox();
			cb_textEq.Items.Add("=");
			cb_textEq.Items.Add("≠");
			cb_textEq.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_textEq.Width = 40;
			cb_textEq.SelectedIndex = 0;
			cb_textEq.Location = new Point(cb_props.Right + 5, cb_props.Top);
			cb_textEq.Parent = this;
			cb_textEq.DrawMode = DrawMode.OwnerDrawFixed;
			cb_textEq.DrawItem += DrawComboHandler;
			cb_textEq.Visible = false;

			ch_bool = new CheckBox();
			ch_bool.Location = new Point(cb_props.Right + 5, cb_props.Top);
			ch_bool.Parent = this;
			ch_bool.Visible = false;
			ch_bool.CheckStateChanged += ValidateValue;

			txt_value = new TextBox();
			txt_value.Text = "0";
			txt_value.TextChanged += ValidateValue;
			txt_value.Location = new Point(cb_equality.Right + 5, cb_equality.Top);
			txt_value.Parent = this;

			//cb_equality.SelectedIndexChanged += (sender, e) => prepareEquation();
			//cb_textEq.SelectedIndexChanged += (sender, e) => prepareEquation();
		}

		void loadProps()
		{
			if (EditableCell != null) {
				cb_props.Items.Clear();
				for (int i = 0; i < EditableCell.Properties.Count; ++i) {
					cb_props.Items.Add(EditableCell.Properties [i].Name);
				}
				cb_props.SelectedIndex = 0;
			}
		}

		void DrawComboHandler(object sender, DrawItemEventArgs e)
		{
			ComboBox c = (ComboBox)sender;
			if (c.Items.Count < 1) {
				return;
			}
			e.Graphics.FillRectangle(Brushes.White, e.Bounds);

			if (e.State.HasFlag(DrawItemState.Selected) && !e.State.HasFlag(DrawItemState.ComboBoxEdit)) {
				LinearGradientBrush b = new LinearGradientBrush(e.Bounds, Color.AliceBlue, Color.DeepSkyBlue, LinearGradientMode.Vertical);
				e.Graphics.FillRectangle(b, e.Bounds);
			}

			e.Graphics.DrawString(c.Items [e.Index].ToString(), c.Font, Brushes.Black, e.Bounds);

		}

		protected override void PrepareRule()
		{
			Console.WriteLine("PrepareRule Valid = " + Valid);
			if (!Valid) {
				return;
			}

			int valType = 0;

			if (EditableCell.Properties [targetProperty].GetType().Equals(typeof(StringProperty))) {
				valType = 1;
			} else if (EditableCell.Properties [targetProperty].GetType().Equals(typeof(IntProperty))) {
				valType = 2;
			} else if (EditableCell.Properties [targetProperty].GetType().Equals(typeof(FloatProperty))) {
				valType = 3;
			}

			if (EditableCell.Properties [targetProperty].GetType().Equals(typeof(BoolProperty))) {
				//checkbox
				Rule = new RuleEquals(targetProperty, ch_bool.CheckState == CheckState.Checked);
			} else if (EditableCell.Properties [targetProperty].GetType().Equals(typeof(StringProperty))) {
				switch (cb_textEq.SelectedIndex) {
					case 0: // =
						Rule = new RuleEquals(targetProperty, txt_value.Text);
						break;
					case 1: // !=
						Rule = new RuleNotEquals(targetProperty, txt_value.Text);
						break;
				}
			} else {
				//all
				object eq = null;
				switch (valType) {
					case 1:
						eq = txt_value.Text;
						break;
					case 2:
						eq = Int32.Parse(txt_value.Text);
						break;
					case 3:
						eq = float.Parse(txt_value.Text);
						break;
				}
				switch (cb_equality.SelectedIndex) {
					case 0: // =
						Rule = new RuleEquals(targetProperty, eq);
						break;
					case 1: // !=
						Rule = new RuleNotEquals(targetProperty, eq);
						break;
					case 2: // >
						Rule = new RuleGreater(targetProperty, eq);
						break;
					case 3: // <
						Rule = new RuleLess(targetProperty, eq);
						break;
					case 4: // >=
						Rule = new RuleGreaterEquals(targetProperty, eq);
						break;
					case 5: // <=
						Rule = new RuleLessEquals(targetProperty, eq);
						break;
				}
			}
		}

		void PropsIndexChanged(object sender, EventArgs e)
		{
			ComboBox c = (ComboBox)sender;
			if (c.Items.Count < 1) {
				return;
			}
			if (EditableCell == null) {
				return;
			}
			string propName = c.Items [c.SelectedIndex].ToString();
			targetProperty = propName;

			txt_value.Text = "";
			ch_bool.Checked = false;

			if (EditableCell.Properties [propName].GetType().Equals(typeof(StringProperty))) {
				//eq, not eq
				cb_textEq.Visible = true;
				txt_value.Visible = true;
				cb_equality.Visible = false;
				ch_bool.Visible = false;
			} else if (EditableCell.Properties [propName].GetType().Equals(typeof(BoolProperty))) {
				//checkbox
				cb_textEq.Visible = false;
				cb_equality.Visible = false;
				ch_bool.Visible = true;
				txt_value.Visible = false;
			} else {
				//all
				cb_textEq.Visible = false;
				cb_equality.Visible = true;
				txt_value.Visible = true;
				ch_bool.Visible = false;
			}
			ValidateValue(null, null);
		}

		void ValidateValue(object sender, EventArgs e)
		{
			string t = txt_value.Text;
			int i;
			float f;

			if (EditableCell == null) {
				Valid = false;
				txt_value.BackColor = Color.IndianRed;
			}
			AbstractProperty pr = EditableCell.Properties [targetProperty];
			if (ch_bool.Visible) {
				Valid = true;
			} else if (t.Equals(String.Empty)) {
				txt_value.BackColor = Color.IndianRed;
				Valid = false;
			} else if (pr.GetType().Equals(typeof(IntProperty))) {
				if (!Int32.TryParse(t, out i)) {
					txt_value.BackColor = Color.IndianRed;
					Valid = false;
				} else {
					txt_value.Text = i.ToString();
					txt_value.BackColor = Color.White;
					Valid = true;
				}
			} else if (pr.GetType().Equals(typeof(FloatProperty))) {
				int p = txt_value.SelectionStart;
				txt_value.Text = txt_value.Text.Replace(',', '.');
				txt_value.SelectionStart = p;
				t = t.Replace(',', '.');
				if (!float.TryParse(t, out f) || (t.Split(new []{ '.' }, StringSplitOptions.None).Length > 2)) {
					txt_value.BackColor = Color.IndianRed;
					Valid = false;
				} else {
					txt_value.BackColor = Color.White;
					Valid = true;
				}
			} else {
				txt_value.BackColor = Color.White;
				Valid = true;
			}
			Console.WriteLine("ValidatValue Valid = " + Valid);
		}

		protected override void SetEditableCell(Cell value)
		{
			base.SetEditableCell(value);
			loadProps();
		}
	}
}


