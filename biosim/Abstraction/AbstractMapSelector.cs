using System;
using Biosim;
using Biosim.Implementation;

namespace Biosim.Abstraction
{
	public abstract class AbstractMapSelector
	{
		protected Map Map;

		protected AbstractMapSelector(Map map)
		{
			Map = map;
		}

		public abstract AbstractCell Select(int x, int y);

		public abstract int GetIndex(int x, int y);

		public abstract int GetIndex(int x, int y, int width, int height);
	}
}

