using System;
using System.Windows.Forms;
using System.Drawing;
using Biosim.Abstraction;

namespace Biosim.UI
{
	public class NewRuleWidget : UserControl
	{
		AbstractCell cell;
		Label textIf;
		Label textThen;
		ComboBox properties;
		ComboBox comparison;
		TextBox valueEdit;
		ComboBox actions;

		public NewRuleWidget(AbstractCell c) : base()
		{
			cell = c;
			initializeUi();
		}

		void initializeUi()
		{
			AutoSize = true;
			SetAutoSizeMode(AutoSizeMode.GrowAndShrink);

			BackColor = Color.Red;
			textIf = new Label();
			textIf.Text = "If:";
			textIf.Parent = this;
			textIf.AutoSize = true;
			textIf.Location = new Point(8, 8);
			textThen = new Label();
			textThen.Text = "Then:";
			textThen.Parent = this;
			textThen.AutoSize = true;
			textThen.Location = new Point(8, textIf.Bottom + 8);
			properties = new ComboBox();
			properties.Parent = this;
			properties.Location = new Point(textIf.Right + 4, 5);
			properties.DropDownStyle = ComboBoxStyle.DropDownList;
			foreach (AbstractProperty p in cell.Properties) {
				properties.Items.Add(p.Name);
			}
			properties.SelectedIndex = 0;
			comparison = new ComboBox();
			comparison.Parent = this;
			comparison.DropDownStyle = ComboBoxStyle.DropDownList;
			comparison.Location = new Point(properties.Right + 4, 5);
			comparison.Width = 50;
			comparison.Items.Add("=");
			comparison.Items.Add("<=");
			comparison.Items.Add("<");
			comparison.Items.Add(">=");
			comparison.Items.Add(">");
			comparison.SelectedIndex = 0;

			valueEdit = new TextBox();
			valueEdit.Parent = this;
			valueEdit.Location = new Point(comparison.Right + 4, 4);
			valueEdit.Width = 60;

			actions = new ComboBox();
			actions.Parent = this;
			actions.Location = new Point(textThen.Right + 4, textThen.Top - 3);
			actions.Items.Add("No change");
			actions.Items.Add("Increment Value");
			actions.Items.Add("Decrement Value");
			actions.Items.Add("Set Value");
			actions.Items.Add("Unset Value");
			actions.DropDownStyle = ComboBoxStyle.DropDownList;
			actions.SelectedIndex = 0;

			Update();
		}
	}
}

