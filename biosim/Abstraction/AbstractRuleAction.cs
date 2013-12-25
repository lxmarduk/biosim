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
		Die,
		StatsInc,
		StatsDec,
		IncBy,
		DecBy,
		SetValueTo
	}

	[Serializable]
	public enum NeighboursType
	{
		All,
		Cross,
		DiagonalCross,
		LeftRight,
		TopBottom,
		LeftRightCross,
		RightLeftCross
	}

	[Serializable]
	public abstract class AbstractRuleAction
	{
		readonly String targetProperty;
		readonly IRule rule;
		readonly ActionType action;
		readonly AbstractCell target;
		readonly object incValue;

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

		public object IncrementValue {
			get {
				return incValue;
			}
		}

		public NeighboursType NeighboursSelectorType {
			get;
			set;
		}

		protected AbstractRuleAction(AbstractCell target, IRule r, ActionType a, String targetProperty = "", object incValue = null)
		{
			rule = r;
			action = a;
			this.targetProperty = targetProperty;
			this.target = target;
			this.incValue = incValue;
			NeighboursSelectorType = NeighboursType.All;
		}

		public virtual ActionType Process(AbstractCell cell)
		{
			return (rule.Check(cell) ? action : ActionType.NoChange);
		}

		public override string ToString()
		{
			String result;
			if (ActionType == ActionType.StatsDec || ActionType == ActionType.StatsDec) {
				return string.Format("Якщо {0} То {1} для {2}", Rule, Utils.ActionTypeToString(ActionType), TargetProperty);
			}
			if (ActionType == ActionType.SetValueTo || ActionType == ActionType.DecBy || ActionType == ActionType.IncBy) {
				result = string.Format("Якщо {0} То {1} {2} для {3}[{4}]", Rule, Utils.ActionTypeToString(ActionType), IncrementValue, target.Properties ["Name"].Value, TargetProperty);
				return result;
			}
			result = string.Format("Якщо {0} То {1} для {2}[{3}]", Rule, Utils.ActionTypeToString(ActionType), target.Properties ["Name"].Value, TargetProperty);
			return result;
		}
	}
}

