using System;
using System.Windows.Forms;
using Biosim.Abstraction;
using Biosim.Implementation;

namespace Biosim.UI
{
	public class RuleWidgetBase : UserControl
	{
		protected IRule rule;

		public IRule Rule {
			get {
//				Console.WriteLine("Preparing rule...");
				PrepareRule();
//				Console.WriteLine("Rule is \"" + rule + "\"");
				return rule;
			}
			set {
				rule = value;
//				Console.WriteLine("Rule set to " + rule.ToString());
			}
		}

		protected Cell editableCell;

		public Cell EditableCell {
			get {
				return editableCell;
			}
			set {
				SetEditableCell(value);
			}
		}

		public event EventHandler ValidationChanged;

		protected bool valid;

		public bool Valid {
			get {
				CheckValid();
//				Console.WriteLine("RuleWidgetBase valid = " + valid);
				return valid;
			}
			protected set {
				valid = value;
//				Console.WriteLine("Valid set to " + valid);
				if (ValidationChanged != null) {
					ValidationChanged(null, null);
				}
			}
		}

		protected virtual void PrepareRule()
		{
		}

		protected virtual void SetEditableCell(Cell value)
		{
			editableCell = (Cell)value.Clone();
		}

		protected virtual void CheckValid()
		{
		}
	}
}

