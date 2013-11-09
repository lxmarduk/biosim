using System;
using Biosim.Implementation;

namespace Biosim.Abstraction
{
	public abstract class AbstractRule
	{
		protected Map map;

		public AbstractRule(Map map)
		{
			this.map = map;
		}

		public virtual void Apply(int x, int y)
		{
		}
	}
}

