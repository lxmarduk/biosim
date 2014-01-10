using System;
using System.Windows.Forms;
using System.Drawing;
using Biosim.Abstraction;
using Biosim.Implementation;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Drawing2D;

namespace Biosim.UI
{
	public sealed class NewRuleActionDialog : Form
	{
		Cell editableCell;
		List<RuleWidgetBase> ruleWidgets;
		List<Button> removeButtons;
		ComboBox cellCombo;
		WidgetChooser widgetChooser;
		bool initialized;
		Label lblActionType;
		ComboBox cbActionType;
		ComboBox cbTargetProp;
		string targetProp;
		TextBox targetValue;
		object targetVal;
		Button allDone;
		Button cancel;

		public Cell EditableCell {
			get {
				return editableCell;
			}
			set {
				editableCell = (Cell)value.Clone();
				setEditableCell();
			}
		}

		public RuleAction RuleAction {
			get {
				if (ruleWidgets.Count > 0) {
//					Console.WriteLine("Valid = " + ruleWidgets [0].Valid);
//					Console.WriteLine("Type = " + ruleWidgets [0].GetType());
					if (ruleWidgets [0].Valid) {
						return new RuleAction(EditableCell, ruleWidgets [0].Rule, (ActionType)cbActionType.SelectedIndex, targetProp, targetVal);
					}
				}
				return null;
			}
		}

		public NewRuleActionDialog()
		{
			ruleWidgets = new List<RuleWidgetBase>();
			removeButtons = new List<Button>();
			initializeUI();
		}

		void initializeUI()
		{
			FormBorderStyle = FormBorderStyle.FixedDialog;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Margin = new Padding(8);
			StartPosition = FormStartPosition.CenterScreen;

			SizeChanged += (sender, e) => {
				if (initialized) {
					adjustWidgets();
				}
			};

			Resize += (sender, e) => {
				if (initialized) {
					adjustWidgets();
				}
			};

			#region Cell combobox and label
			Label cellComboLabel = new Label();
			cellComboLabel.Parent = this;
			cellCombo = new ComboBox();
			cellCombo.Parent = this;
			widgetChooser = new WidgetChooser();
			widgetChooser.Parent = this;

			cellCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			cellCombo.DrawMode = DrawMode.OwnerDrawFixed;
			cellCombo.DrawItem += DrawCellItemCombo;

			cellCombo.SelectedIndexChanged += (object sender, EventArgs e) => {
				Cell selectedCell = (Cell)cellCombo.SelectedItem;
				EditableCell = selectedCell;
			};

			cellComboLabel.AutoSize = true;
			cellComboLabel.Text = "Клітина:";
			if (loadCells()) {
				cellCombo.SelectedIndex = 0;
			}
			cellCombo.Width = 100;

			cellCombo.Location = new Point(cellComboLabel.Right + 5, cellComboLabel.Top);
			#endregion

			lblActionType = new Label();
			lblActionType.Text = "Тоді:";
			lblActionType.AutoSize = true;
			lblActionType.Parent = this;
			lblActionType.Visible = false;

			cbActionType = new ComboBox();
			cbActionType.DropDownStyle = ComboBoxStyle.DropDownList;
			cbActionType.Items.Add(Utils.ActionTypeToString(ActionType.NoChange));
			cbActionType.Items.Add(Utils.ActionTypeToString(ActionType.Born));
			cbActionType.Items.Add(Utils.ActionTypeToString(ActionType.Die));
			cbActionType.Items.Add(Utils.ActionTypeToString(ActionType.IncValue));
			cbActionType.Items.Add(Utils.ActionTypeToString(ActionType.DecValue));
			cbActionType.Items.Add(Utils.ActionTypeToString(ActionType.SetValue));
			cbActionType.Items.Add(Utils.ActionTypeToString(ActionType.UnsetValue));
			cbActionType.Items.Add(Utils.ActionTypeToString(ActionType.IncBy));
			cbActionType.Items.Add(Utils.ActionTypeToString(ActionType.DecBy));
			cbActionType.Items.Add(Utils.ActionTypeToString(ActionType.SetValueTo));

			cbTargetProp = new ComboBox();
			cbTargetProp.Parent = this;
			cbTargetProp.Visible = false;
			loadProps();
			cbTargetProp.SelectedIndexChanged += (sender, e) => targetProp = cbTargetProp.Text;
			cbTargetProp.VisibleChanged += (sender, e) => {
				if (!cbTargetProp.Visible) {
					targetProp = String.Empty;
				}
			};


			targetValue = new TextBox();
			targetValue.Parent = this;
			targetValue.Visible = false;
			targetValue.TextChanged += HandleTargetValueChanged;

			cbActionType.SelectedIndex = 0;
			cbActionType.Parent = this;
			cbActionType.Visible = false;
			cbActionType.SelectedIndexChanged += ActionTypeChanged;

			allDone = new Button();
			allDone.Text = "Створити";
			allDone.Parent = this;
			allDone.Visible = false;
			allDone.DialogResult = DialogResult.OK;

			cancel = new Button();
			cancel.Text = "Відмінити";
			cancel.Parent = this;
			cancel.Visible = false;
			cancel.DialogResult = DialogResult.Cancel;

			widgetChooser.Location = new Point(0, cellCombo.Bottom + 5);
			widgetChooser.WidgetChoosen += HandleChoosenWidget;

			initialized = true;
			adjustWidgets();
		}

		void ActionTypeChanged(object sender, EventArgs e)
		{
			targetProp = String.Empty;
			targetVal = null;
			if (cbActionType.SelectedIndex > 2) {
				cbTargetProp.Visible = true;
				cbTargetProp.Location = new Point(cbActionType.Right + 5, cbActionType.Top);
				if (cbActionType.SelectedIndex > 6) {
					targetValue.Visible = true;
					targetValue.Location = new Point(cbTargetProp.Right + 5, cbTargetProp.Top);
					targetProp = cbTargetProp.Text;
					HandleTargetValueChanged(null, null);
					return;
				} else {
					targetValue.Visible = false;
					HandleTargetValueChanged(null, null);
				}
				allDone.Enabled = ruleWidgets [0].Valid;
			} else {
				cbTargetProp.Visible = false;
				HandleTargetValueChanged(null, null);
			}
		}

		void HandleTargetValueChanged(object sender, EventArgs e)
		{
			Console.WriteLine(ruleWidgets [0].Valid);

			if (!cbTargetProp.Visible) {
				allDone.Enabled = ruleWidgets [0].Valid;
				return;
			}
			targetVal = null;
			if (!targetProp.Equals(String.Empty)) {
				if (editableCell.Properties [targetProp].GetType().Equals(typeof(StringProperty))) {
					targetVal = targetValue.Text;
					if (targetVal.Equals(String.Empty)) {
						targetValue.BackColor = Color.IndianRed;
						allDone.Enabled = false;
					} else {
						targetValue.BackColor = Color.White;
						allDone.Enabled = ruleWidgets [0].Valid;
					}
				}
				if (editableCell.Properties [targetProp].GetType().Equals(typeof(IntProperty))) {
					int i;
					if (int.TryParse(targetValue.Text, out i)) {
						targetVal = i;
						targetValue.BackColor = Color.White;
						allDone.Enabled = ruleWidgets [0].Valid;
					} else {
						targetValue.BackColor = Color.IndianRed;
						allDone.Enabled = false;
					}
				}
				if (editableCell.Properties [targetProp].GetType().Equals(typeof(FloatProperty))) {
					float f;
					if (float.TryParse(targetValue.Text, out f)) {
						targetVal = f;
						targetValue.BackColor = Color.White;
						allDone.Enabled = ruleWidgets [0].Valid;
					} else {
						targetValue.BackColor = Color.IndianRed;
						allDone.Enabled = false;
					}
				}
				if (editableCell.Properties [targetProp].GetType().Equals(typeof(BoolProperty))) {
					bool b;
					if (bool.TryParse(targetValue.Text, out b)) {
						targetVal = b;
						targetValue.BackColor = Color.White;
						allDone.Enabled = ruleWidgets [0].Valid;
					} else {
						targetValue.BackColor = Color.IndianRed;
						allDone.Enabled = false;
					}
				}
			}
		}

		void HandleChoosenWidget(object sender, EventArgs e)
		{
			addRuleWidget(widgetChooser.WidgetType);
			widgetChooser.Reset();
			widgetChooser.Visible = false;
			lblActionType.Visible = true;
			cbActionType.Visible = true;
			allDone.Visible = true;
			cancel.Visible = true;
			cbActionType.SelectedIndex = 0;
			cbTargetProp.SelectedIndex = 0;
			allDone.Enabled = ruleWidgets [0].Valid;
		}

		void loadProps()
		{
			if (EditableCell != null) {
				cbTargetProp.Items.Clear();
				for (int i = 0; i < EditableCell.Properties.Count; ++i) {
					cbTargetProp.Items.Add(editableCell.Properties [i].Name);
				}
				cbTargetProp.SelectedIndex = 0;
			}
		}

		void DrawCellItemCombo(object sender, DrawItemEventArgs e)
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

			Cell drawCell = (Cell)c.Items [e.Index];
			e.Graphics.DrawString(drawCell.Properties ["Ім'я"].Value.ToString(), c.Font, Brushes.Black, e.Bounds);

		}

		bool loadCells()
		{
			if (!Directory.Exists("cells")) {
				return false;
			}

			List<string> files = new List<String>(Directory.GetFiles("cells", "*.bin"));
			if (files.Count == 0) {
				return false;
			}
			foreach (String file in files) {
				try {
					cellCombo.Items.Add(Utils.DeserializeCell(file).Clone());
				} catch (Exception e) {
					Console.WriteLine(e.Message);
				}
			}
			return true;
		}

		public new DialogResult Show()
		{
			if (ShowDialog() == DialogResult.OK) {

			}
			return DialogResult;
		}

		void addRuleWidget(WidgetType t)
		{
			RuleWidgetBase rule = null;
			switch (t) {
				case WidgetType.RuleEquationWidget:
					rule = new RuleEquationWidget();
					break;
				case WidgetType.RuleLogicalWidget:
					rule = new RuleLogicalWidget();
					break;
			}
			if (rule != null) {
				rule.Parent = this;
				rule.SizeChanged += (sender, e) => adjustWidgets();
				ruleWidgets.Add(rule);
				rule.ValidationChanged += (sender, e) => allDone.Enabled = rule.Valid;
				allDone.Enabled = rule.Valid;
			}

			Button b = new Button();
			b.Image = new Bitmap(Utils.LoadResource("list-remove"));
			b.Size = new Size(36, 36);
			b.Location = new Point(rule.Right + 5, rule.Top - 5);
			b.Click += HandleRemove;
			b.Parent = this;
			removeButtons.Add(b);

			adjustWidgets();
			updateButtonTags();
			setEditableCell();
		}

		void HandleRemove(object sender, EventArgs e)
		{
			Button b = (Button)sender;
			RuleWidgetBase r = ruleWidgets [(int)b.Tag];
			ruleWidgets.Remove(r);
			r.Dispose();
			removeButtons.Remove(b);
			b.Dispose();
			updateButtonTags();
			adjustWidgets();
		}

		void adjustWidgets()
		{
			Point startPos = new Point(0, cellCombo.Bottom + 5);
			if (ruleWidgets.Count > 0) {
				ruleWidgets [0].Location = startPos;
				for (int i = 1; i < ruleWidgets.Count; ++i) {
					ruleWidgets [i].Location = new Point(0, ruleWidgets [i - 1].Bottom + 5);
				}
				adjustButtons();
			} else {
				widgetChooser.Location = new Point(0, cellCombo.Bottom + 5);
				widgetChooser.Visible = true;
				allDone.Visible = false;
				cancel.Visible = false;
				lblActionType.Visible = false;
				cbActionType.Visible = false;
				targetValue.Visible = false;
				cbTargetProp.Visible = false;
				targetProp = String.Empty;
				lblActionType.Location = new Point();
				allDone.Location = new Point();
				cancel.Location = new Point();
				cbActionType.Location = new Point();
				targetValue.Location = new Point();
				cbTargetProp.Location = new Point();
			}
		}

		void adjustButtons()
		{
			if (ruleWidgets.Count > 0) {
				for (int i = 0; i < removeButtons.Count; ++i) {
					removeButtons [i].Location = new Point(ruleWidgets [i].Right + 5, ruleWidgets [i].Top - 5);
				}
				lblActionType.Location = new Point(0, ruleWidgets [ruleWidgets.Count - 1].Bottom + 5);
				cbActionType.Location = new Point(lblActionType.Right + 5, ruleWidgets [ruleWidgets.Count - 1].Bottom + 5);
				cbTargetProp.Location = new Point(cbActionType.Right + 5, cbActionType.Top);
				targetProp = cbTargetProp.Text;
				targetValue.Location = new Point(cbTargetProp.Right + 5, cbTargetProp.Top);
				allDone.Location = new Point(ruleWidgets [ruleWidgets.Count - 1].Right - allDone.Width, cbActionType.Bottom + 5);
				cancel.Location = new Point(allDone.Left - cancel.Width - 5, allDone.Top);
			}
		}

		void updateButtonTags()
		{
			for (int i = 0; i < removeButtons.Count; ++i) {
				removeButtons [i].Tag = i;
			}
		}

		void setEditableCell()
		{
			for (int i = 0; i < ruleWidgets.Count; ++i) {
				ruleWidgets [i].EditableCell = (Cell)EditableCell.Clone();
			}
		}
	}
}

