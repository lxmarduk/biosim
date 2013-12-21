using System;

namespace Biosim.Abstraction
{
	[Serializable]
	public enum ActionType
	{
		NoChange,
		IncValue,
		DecValue,
		SetValue,
		UnsetValue,
		Born,
		Die
	}

	[Serializable]
	public abstract class AbstractRuleAction
	{
		readonly String targetProperty;
		readonly IRule rule;
		readonly ActionType action;
		readonly AbstractCell target;

		public IRule Rule {
			get { return rule; }
		}

		public ActionType ActionType {
			get {
				return action;
			}
		}

		public String TargetProperty {
			get {
				return targetProperty;
			}
		}

		public AbstractCell TargetCell {
			get {
				return target;
			}
		}

		protected AbstractRuleAction(AbstractCell target, IRule r, ActionType a, String targetProperty)
		{
			rule = r;
			action = a;
			this.targetProperty = targetProperty;
			this.target = target;
		}

		public virtual ActionType Process(AbstractCell cell)
		{
			return (rule.Check(cell) ? action : ActionType.NoChange);
		}

		public override string ToString()
		{
			return string.Format("Якщо {0} То {1} для {2}[{3}]", Rule, Utils.ActionTypeToString(ActionType), target.Properties ["Name"], TargetProperty);
		}
	}
}

