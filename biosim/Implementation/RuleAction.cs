using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	[Serializable]
	public class RuleAction : AbstractRuleAction
	{
		public RuleAction(AbstractCell target, IRule r, ActionType a, String t = "", object v = null) : base(target, r, a, t, v)
		{
		}
	}
}

