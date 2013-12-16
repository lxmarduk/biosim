using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	[Serializable]
	public class RuleEquals : AbstractRule
	{
		public RuleEquals(String prop, object value) : base(prop, value)
		{
		}

		public override bool Check(AbstractCell cell)
		{
			return cell.Properties [property].Equ(equalityValue);
		}
	}
}

