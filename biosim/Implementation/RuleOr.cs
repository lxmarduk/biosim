using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	[Serializable]
	public class RuleOr : RuleLogical
	{
		public RuleOr(IRule r1, IRule r2) : base(r1, r2)
		{
		}

		protected override bool Evaluate(AbstractCell cell)
		{
			bool r1res = RuleOne.Check(cell);
			bool r2res = RuleTwo.Check(cell);
			return r1res || r2res;
		}

		public override string ToString()
		{
			return string.Format("({0} або {1})", RuleOne, RuleTwo);
		}
	}
}