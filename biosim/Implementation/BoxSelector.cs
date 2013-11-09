using System;
using Biosim.Abstraction;


namespace Biosim.Implementation
{
	public class BoxSelector : AbstractMapSelector
	{
		public BoxSelector(Map map) : base(map)
		{
		}

		#region implemented abstract members of Biosim.Abstraction.AbstractMapSelector
		public override AbstractCell Select(int x, int y)
		{
			if (x < 0 || x >= map.Width) {
				return null;
			}
			if (y < 0 || y >= map.Height) {
				return null;
			}
			return map.Cells [x + y * map.Width];
		}
		#endregion

	}
}

