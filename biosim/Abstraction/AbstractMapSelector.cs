using System;
using Biosim;
using Biosim.Implementation;

namespace Biosim.Abstraction
{
	public abstract class AbstractMapSelector
	{
		protected Map map;

		public AbstractMapSelector(Map map)
		{
			this.map = map;
		}

		public abstract AbstractCell Select(int x, int y);
		public abstract int GetIndex(int x, int y);
	}
}

