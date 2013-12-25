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
	public class RuleLogicalWidget : UserControl
	{
		ComboBox cb_type;
		ComboBox cb_rule1;
		ComboBox cb_rule2;
		Control rule1;
		Control rule2;
		Button btnAdd1;
		Button btnAdd2;
		Button btnRemove1;
		Button btnRemove2;
		Cell editableCell;

		public Cell EditableCell {
			get {
				return editableCell;
			}
			set {
				editableCell = (Cell)value.Clone();
				if (rule1 != null) {
					if (rule1.GetType().Equals(typeof(RuleLogicalWidget))) {
						(rule1 as RuleLogicalWidget).EditableCell = editableCell;
					} else {
						(rule1 as RuleEquationWidget).EditableCell = editableCell;
					}
				}
				if (rule2 != null) {
					if (rule2.GetType().Equals(typeof(RuleLogicalWidget))) {
						(rule2 as RuleLogicalWidget).EditableCell = editableCell;
					} else {
						(rule2 as RuleEquationWidget).EditableCell = editableCell;
					}
				}
				Console.WriteLine(editableCell);
			}
		}

		public bool Valid {
			get {
				if (rule1 != null && rule2 != null) {
					bool r1, r2;
					if (rule1.GetType().Equals(typeof(RuleEquationWidget))) {
						r1 = (rule1 as RuleEquationWidget).Valid;
					} else {
						r1 = (rule1 as RuleLogicalWidget).Valid;
					}
					if (rule2.GetType().Equals(typeof(RuleEquationWidget))) {
						r2 = (rule2 as RuleEquationWidget).Valid;
					} else {
						r2 = (rule2 as RuleLogicalWidget).Valid;
					}
					return r1 && r2;
				}
				return false;
			}
		}

		public RuleLogical Rule {
			get {
				if (Valid) {
					IRule r1;
					if (rule1.GetType().Equals(typeof(RuleEquationWidget))) {
						r1 = (rule1 as RuleEquationWidget).Rule;
					} else {
						r1 = (rule1 as RuleLogicalWidget).Rule;
					}
					IRule r2;
					if (rule2.GetType().Equals(typeof(RuleEquationWidget))) {
						r2 = (rule2 as RuleEquationWidget).Rule;
					} else {
						r2 = (rule2 as RuleLogicalWidget).Rule;
					}
					switch (cb_type.SelectedIndex) {
						case 0: //&&
							return new RuleAnd(r1, r2);
						case 1: //||
							return new RuleOr(r1, r2);
					}
				}
				return null;
			}
		}

		public RuleLogicalWidget()
		{
			initializeUI();
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			BorderStyle = BorderStyle.FixedSingle;
		}

		void initializeUI()
		{
			Label lbl_type = new Label();
			cb_type = new ComboBox();
			Panel r1panel = new Panel();
			Label lbl_rule1 = new Label();
			cb_rule1 = new ComboBox();
			btnAdd1 = new Button();
			Panel r2panel = new Panel();
			Label lbl_rule2 = new Label();
			cb_rule2 = new ComboBox();
			btnAdd2 = new Button();

			r1panel.AutoSize = true;
			r1panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			r2panel.AutoSize = true;
			r2panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			r1panel.SizeChanged += (sender, e) => lbl_type.Top = r1panel.Bottom + 5;
			r1panel.VisibleChanged += (sender, e) => {
				if (r1panel.Visible) {
					lbl_type.Location = new Point(8, r1panel.Bottom + 5);

				} else {
					if (rule1 != null) {
						lbl_type.Location = new Point(8, rule1.Bottom + 5);
					}
				}
				r2panel.Top = lbl_type.Bottom + 5;
				cb_type.Location = new Point(lbl_type.Right + 5, lbl_type.Top - 1);
			};

			r1panel.Parent = this;
			r1panel.Location = new Point(4, 4);
			lbl_rule1.Parent = r1panel;
			lbl_rule1.Location = new Point(4, 10);
			lbl_rule1.Width = 120;
			cb_rule1.Parent = r1panel;
			cb_rule1.Location = new Point(lbl_rule1.Right + 5, lbl_rule1.Top - 1);
			cb_rule1.Width = 100;
			btnAdd1.Parent = r1panel;
			btnAdd1.Location = new Point(cb_rule1.Right + 10, cb_rule1.Top - 8);

			lbl_type.Parent = this;
			lbl_type.Width = 120;
			lbl_type.Location = new Point(8, r1panel.Bottom + 5);
			lbl_type.LocationChanged += (s, e) => {
				if (r2panel.Visible) {
					r2panel.Top = lbl_type.Bottom + 5;
					cb_type.Location = new Point(lbl_type.Right + 5, lbl_type.Top - 1);
				} else {
					if (rule2 != null) {
						rule2.Top = lbl_type.Bottom + 5;
						cb_type.Location = new Point(lbl_type.Right + 5, lbl_type.Top - 1);
					}
				}
				ResumeLayout();
				if (Parent != null) {
					Parent.ResumeLayout();
				}
				Refresh();
				if (Parent != null) {
					Parent.Refresh();
				}
			};
			cb_type.Parent = this;
			cb_type.Location = new Point(lbl_type.Right + 5, lbl_type.Top - 1);
			cb_type.Width = 50;

			r2panel.Parent = this;
			r2panel.Location = new Point(4, lbl_type.Bottom + 5);
			lbl_rule2.Parent = r2panel;
			lbl_rule2.Location = new Point(4, 10);
			lbl_rule2.Width = 120;
			cb_rule2.Parent = r2panel;
			cb_rule2.Location = new Point(lbl_rule2.Right + 5, lbl_rule2.Top - 1);
			cb_rule2.Width = 100;
			btnAdd2.Parent = r2panel;
			btnAdd2.Location = new Point(cb_rule2.Right + 10, cb_rule2.Top - 8);

			lbl_type.Text = "Логічна операція:";

			cb_type.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_type.DrawMode = DrawMode.OwnerDrawFixed;
			cb_type.DrawItem += DrawComboHandler;
			cb_type.Items.Add("і");
			cb_type.Items.Add("або");
			cb_type.SelectedIndex = 0;

			lbl_rule1.Text = "Перше правило:";

			cb_rule1.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_rule1.DrawMode = DrawMode.OwnerDrawFixed;
			cb_rule1.DrawItem += DrawComboHandler;
			cb_rule1.Items.Add("Порівняння");
			cb_rule1.Items.Add("Логічне правило");
			cb_rule1.SelectedIndex = 0;

			btnAdd1.Image = new Bitmap(Utils.LoadResource("list-add"));
			btnAdd1.Size = new Size(36, 36);
			btnAdd1.Click += (sender, ev) => {
				switch (cb_rule1.SelectedIndex) {
					case 0:
						rule1 = new RuleEquationWidget();
						(rule1 as RuleEquationWidget).EditableCell = (Cell)editableCell.Clone();
						rule1.Parent = this;
						rule1.Location = r1panel.Location;
						r1panel.Visible = false;
						Console.WriteLine((rule1 as RuleEquationWidget).EditableCell);
						break;
					case 1:
						rule1 = new RuleLogicalWidget();
						(rule1 as RuleLogicalWidget).EditableCell = (Cell)editableCell.Clone();
						rule1.Parent = this;
						rule1.Location = r1panel.Location;
						r1panel.Visible = false;
						Console.WriteLine((rule1 as RuleLogicalWidget).EditableCell);
						break;
				}

				rule1.SizeChanged += (s, evn) => {
					lbl_type.Location = new Point(8, rule1.Bottom + 5);
					ResumeLayout();
					if (Parent != null) {
						Parent.ResumeLayout();
					}
					Refresh();
					if (Parent != null) {
						Parent.Refresh();
					}
				};

				ResumeLayout();
				if (Parent != null) {
					Parent.ResumeLayout();
				}
				Refresh();
				if (Parent != null) {
					Parent.Refresh();
				}
			};
				

			lbl_rule2.Text = "Друге правило:";
			lbl_rule2.Width = 120;

			cb_rule2.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_rule2.DrawMode = DrawMode.OwnerDrawFixed;
			cb_rule2.DrawItem += DrawComboHandler;
			cb_rule2.Items.Add("Порівняння");
			cb_rule2.Items.Add("Логічне правило");
			cb_rule2.SelectedIndex = 0;

			btnAdd2.Image = new Bitmap(Utils.LoadResource("list-add"));
			btnAdd2.Size = new Size(36, 36);
			btnAdd2.Click += (sen, even) => {
				switch (cb_rule2.SelectedIndex) {
					case 0:
						rule2 = new RuleEquationWidget();
						(rule2 as RuleEquationWidget).EditableCell = (Cell)editableCell.Clone();
						rule2.Parent = this;
						rule2.Location = r2panel.Location;
						r2panel.Visible = false;
						break;
					case 1:
						rule2 = new RuleLogicalWidget();
						(rule2 as RuleLogicalWidget).EditableCell = (Cell)editableCell.Clone();
						rule2.Parent = this;
						rule2.Location = r2panel.Location;
						r2panel.Visible = false;
						break;
				}

				rule2.SizeChanged += (sendr, evnt) => {
					ResumeLayout();
					if (Parent != null) {
						Parent.ResumeLayout();
					}
					Refresh();
					if (Parent != null) {
						Parent.Refresh();
					}
				};

				ResumeLayout();
				if (Parent != null) {
					Parent.ResumeLayout();
				}
				Refresh();
				if (Parent != null) {
					Parent.Refresh();
				}
			};

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
	}
}

