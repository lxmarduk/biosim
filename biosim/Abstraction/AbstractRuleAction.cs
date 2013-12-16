using System;

namespace Biosim.Abstraction
{
	public enum ActionType
	{
		NoChange,
		IncValue,
		DecValue,
		SetValue,
		UnsetValue
	}

	[Serializable]
	public abstract class AbstractRuleAction
	{
		readonly AbstractRule rule;
		readonly ActionType action;
		readonly String targetProperty;

		public AbstractRule Rule {
			get { return rule; }
		}

		public String TargetProperty {
			get {
				return targetProperty;
			}
		}

		protected AbstractRuleAction(AbstractRule r, ActionType a, String targetProperty)
		{
			rule = r;
			action = a;
			this.targetProperty = targetProperty;
		}

		public virtual ActionType Process(AbstractCell cell)
		{
			return (rule.Check(cell) ? action : ActionType.NoChange);
		}
	}
}

