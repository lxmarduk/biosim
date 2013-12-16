using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	public class RuleAction : AbstractRuleAction
	{
		public RuleAction(AbstractRule r, ActionType a, String t) : base(r, a, t)
		{
		}
	}
}

