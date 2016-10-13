using System;
using System.Windows.Forms;
using Biosim.Abstraction;
using Biosim.Implementation;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Biosim.UI
{
	public class EditMapPropertiesDialog : Form
	{
		bool initialized;
		Button apply;
		Button cancel;
		Button addRule;
		Button removeRule;
		//Button editRule;
		NumericUpDown nWidth;
		NumericUpDown nHeight;
		ComboBox cb_mapType;
		ListBox lb_ruleActions;
		ToolTip tt_RuleAction;
		Map editableMap;

		public Map EditableMap {
			get {
				return editableMap;
			}
			set {
				if (!value.Equals(editableMap)) {
					editableMap = value;
					if (EditableMapChanged != null) {
						EditableMapChanged(value, null);
					}
				}
			}
		}

		public event EventHandler EditableMapChanged;

		public EditMapPropertiesDialog()
		{
			Text = "Властивості симуляції";
			FormBorderStyle = FormBorderStyle.FixedDialog;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Margin = new Padding(8);
			StartPosition = FormStartPosition.CenterScreen;
			initializeUI();
			initialized = true;
		}

		void initializeUI()
		{
            #region Labels
            Label lbl_width = new Label();
			Label lbl_height = new Label();
			Label lbl_type = new Label();
			Label lbl_rules = new Label();

			lbl_width.Text = "Ширина:";
			lbl_height.Text = "Висота:";
			lbl_type.Text = "Тип:";
			lbl_rules.Text = "Правила:";

			lbl_width.Width = 75;
			lbl_height.Width = 75;
			lbl_type.Width = 75;
			lbl_rules.Width = 75;

			lbl_width.Location = new Point(4, 4);
			lbl_height.Location = new Point(4, lbl_width.Bottom + 5);
			lbl_type.Location = new Point(4, lbl_height.Bottom + 5);
			lbl_rules.Location = new Point(4, lbl_type.Bottom + 5);

			lbl_width.Parent = this;
			lbl_height.Parent = this;
			lbl_type.Parent = this;
			lbl_rules.Parent = this;
			#endregion

			#region Controls
			nWidth = new NumericUpDown();
			nHeight = new NumericUpDown();
			cb_mapType = new ComboBox();
			lb_ruleActions = new ListBox();

			nWidth.Location = new Point(lbl_width.Right + 5, lbl_width.Top - 2);
			nHeight.Location = new Point(lbl_height.Right + 5, lbl_height.Top - 2);
			cb_mapType.Location = new Point(lbl_type.Right + 5, lbl_type.Top - 2);
			lb_ruleActions.Location = new Point(4, lbl_rules.Bottom + 5);

			cb_mapType.Items.Add(Utils.MapTypeToString(Map.MapType.Box));
			cb_mapType.Items.Add(Utils.MapTypeToString(Map.MapType.Continuous));
			cb_mapType.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_mapType.SelectedIndex = 0;

			lb_ruleActions.DrawMode = DrawMode.OwnerDrawFixed;

			nWidth.Width = 150;
			nHeight.Width = 150;
			cb_mapType.Width = 150;
			lb_ruleActions.Width = 300;
			lb_ruleActions.Height = 150;

			nWidth.Parent = this;
			nHeight.Parent = this;
			cb_mapType.Parent = this;
			lb_ruleActions.Parent = this;
			#endregion

			#region Rule buttons
			addRule = new Button();
			removeRule = new Button();
			//editRule = new Button();

			addRule.Image = new Bitmap(Utils.LoadResource("list-add"));
			removeRule.Image = new Bitmap(Utils.LoadResource("list-remove"));
			//editRule.Image = new Bitmap(Utils.LoadResource("document-properties"));

			Size buttonSize = new Size(36, 36);
			addRule.Size = removeRule.Size /*= editRule.Size*/ = buttonSize;

			addRule.Location = new Point(4, lb_ruleActions.Bottom + 5);
			removeRule.Location = new Point(addRule.Right + 5, addRule.Top);
			//editRule.Location = new Point(lb_ruleActions.Right - editRule.Width, addRule.Top);

			addRule.Parent = removeRule.Parent /*= editRule.Parent*/ = this;
			#endregion

			#region Dialog buttons
			apply = new Button();
			cancel = new Button();

			apply.Text = "Прийняти";
			cancel.Text = "Відхилити";

			apply.DialogResult = DialogResult.OK;
			cancel.DialogResult = DialogResult.Cancel;

			apply.Location = new Point(Width - apply.Width, addRule.Bottom + 10);
			cancel.Location = new Point(apply.Left - cancel.Width - 5, apply.Top);

			apply.Parent = cancel.Parent = this;

			apply.Enabled = EditableMap != null;
			#endregion

			tt_RuleAction = new ToolTip();
			tt_RuleAction.ReshowDelay = 1;

			#region Handlers
			apply.Click += HandleApply;
			EditableMapChanged += HandleEditableMapChanged;
			lb_ruleActions.DrawItem += HandleDrawItem;
			lb_ruleActions.SelectedIndexChanged += HandleSelectedIndexChanged;
			addRule.Click += HandleAddRule;
			removeRule.Click += HandleRemoveRule;
			#endregion

			AcceptButton = apply;
		}

		void HandleRemoveRule(object sender, EventArgs e)
		{
			if (lb_ruleActions.Items.Count > 0) {
				if (lb_ruleActions.SelectedIndex >= 0) {
					if (MessageBox.Show("Ви дійсно хочете видалити правило?!", "Видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
						lb_ruleActions.Items.RemoveAt(lb_ruleActions.SelectedIndex);
					}
				}
			}
		}

		void HandleSelectedIndexChanged(object sender, EventArgs e)
		{
			ListBox l = (sender as ListBox);
			if (l.SelectedIndex >= 0) {
				RuleAction r = (RuleAction)l.Items [l.SelectedIndex];
				int s = l.IndexFromPoint(0, 0);
				int p = l.IndexFromPoint(l.PointToClient(MousePosition));
				int y = (p - s) * l.ItemHeight;
				Console.WriteLine("p = " + p + "; s = " + s + "; y = " + y);
				tt_RuleAction.Show(r.ToString(), lb_ruleActions, 0, y);
			}
		}

		void HandleAddRule(object sender, EventArgs e)
		{
			NewRuleActionDialog d = new NewRuleActionDialog();
			if (d.Show() == DialogResult.OK) {
				lb_ruleActions.Items.Add(d.RuleAction);
			}
			d.Dispose();
		}

		void HandleDrawItem(object sender, DrawItemEventArgs e)
		{
			if ((sender as ListBox).Items.Count == 0) {
				return;
			}
			RuleAction r = (RuleAction)(sender as ListBox).Items [e.Index];
			if (e.State.HasFlag(DrawItemState.Selected)) {
				e.Graphics.FillRectangle(new LinearGradientBrush(e.Bounds, (r.TargetCell as Cell).Color, Utils.Darker((r.TargetCell as Cell).Color), LinearGradientMode.Vertical), e.Bounds);
			} else {
				e.Graphics.FillRectangle(new SolidBrush((r.TargetCell as Cell).Color), e.Bounds);
			}
			e.Graphics.DrawString(r.ToString(), Font, Brushes.Black, e.Bounds);
		}

		void HandleEditableMapChanged(object sender, EventArgs e)
		{
			if (!initialized) {
				// В принципі не можливо, бо initialize встановлюється в конструкторі
				throw new NotImplementedException("Досі не винайдено способу встановлювати значення полів класу до виклику його конструктора... Як?!");
			}

			if (EditableMap == null) {
				throw new NullReferenceException("Необхідно встановити значення EditableMap відмінне від null.");
			}

			apply.Enabled = true;

			nWidth.Value = EditableMap.Width;
			nHeight.Value = EditableMap.Height;
			cb_mapType.SelectedIndex = (int)EditableMap.Type;

			lb_ruleActions.Items.Clear();
			for (int i = 0; i < EditableMap.RuleActions.Count; ++i) {
				lb_ruleActions.Items.Add(EditableMap.RuleActions [i]);
			}
		}

		void HandleApply(object sender, EventArgs e)
		{
			if (EditableMap == null) {
				return;
			}

			EditableMap.Width = (int)nWidth.Value;
			EditableMap.Height = (int)nHeight.Value;
			EditableMap.Type = (Map.MapType)cb_mapType.SelectedIndex;
			EditableMap.RuleActions.Clear();
			for (int i = 0; i < lb_ruleActions.Items.Count; ++i) {
				EditableMap.RuleActions.Add((RuleAction)lb_ruleActions.Items [i]);
			}
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

		public new DialogResult Show()
		{
			return ShowDialog();
		}
	}
}

