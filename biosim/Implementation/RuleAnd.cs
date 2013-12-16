using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	public class RuleAnd : AbstractRule
	{
		readonly AbstractRule r1;
		readonly AbstractRule r2;

		public RuleAnd(AbstractRule r1, AbstractRule r2) : base(String.Empty, null)
		{
			this.r1 = r1;
			this.r2 = r2;
		}

		public override bool Check(AbstractCell cell)
		{
			bool r1res = r1.Check(cell);
			bool r2res = r2.Check(cell);
			return r1res && r2res;
		}
	}
}

