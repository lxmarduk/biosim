using System;
using System.Windows.Forms;

namespace Biosim.UI
{
	public enum WidgetType
	{
		RuleEquationWidget,
		RuleLogicalWidget
	}

	public class WidgetChooser : UserControl
	{
		ComboBox widgetType;
		Button createButton;

		public WidgetType WidgetType {
			get;
			private set;
		}

		public event EventHandler WidgetChoosen;

		public WidgetChooser()
		{
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			initializeUI();
		}

		void initializeUI()
		{
			widgetType = new ComboBox();
			widgetType.Parent = this;
			widgetType.DropDownStyle = ComboBoxStyle.DropDownList;
			widgetType.Items.Add("Порівняння");
			widgetType.Items.Add("Логічне правило");
			widgetType.SelectedIndex = 0;
			widgetType.Width = 150;

			createButton = new Button();
			createButton.Parent = this;
			createButton.Text = "Створити";
			createButton.Location = new System.Drawing.Point(widgetType.Right + 5, widgetType.Top);
			createButton.Click += (sender, e) => {
				switch (widgetType.SelectedIndex) {
					case 0:
						WidgetType = WidgetType.RuleEquationWidget;
						break;
					case 1:
						WidgetType = WidgetType.RuleLogicalWidget;
						break;
				}
				if (WidgetChoosen != null) {
					WidgetChoosen.Invoke(null, null);
				}
			};
		}

		public void Reset()
		{
			widgetType.SelectedIndex = 0;
			WidgetType = WidgetType.RuleEquationWidget;
		}
	}
}

