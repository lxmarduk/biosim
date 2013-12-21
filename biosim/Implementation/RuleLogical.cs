using System;
using Biosim.Abstraction;

namespace Biosim
{
	[Serializable]
	public class RuleLogical : IRule
	{
		readonly IRule ruleOne;
		readonly IRule ruleTwo;

		public IRule RuleOne {
			get {
				return ruleOne;
			}
		}

		public IRule RuleTwo {
			get {
				return ruleTwo;
			}
		}

		public RuleLogical(IRule rule1, IRule rule2)
		{
			ruleOne = rule1;
			ruleTwo = rule2;
		}

		#region IRule implementation

		public bool Check(AbstractCell cell)
		{
			return Evaluate(cell);
		}

		protected virtual bool Evaluate(AbstractCell cell)
		{
			return false;
		}

		#endregion

	}
}

