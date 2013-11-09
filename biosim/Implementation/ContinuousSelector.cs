using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	public class ContinuousSelector : AbstractMapSelector
	{
		public ContinuousSelector(Map map) : base(map)
		{
		}		

		#region implemented abstract members of Biosim.Abstraction.AbstractMapSelector
		public override AbstractCell Select(int x, int y)
		{
			return map.Cells [GetIndex(x, y)];
		}

		public override int GetIndex(int x, int y)
		{
			if (x < 0) {
				x = map.Width + x;
			} else if (x >= map.Width) {
				x -= map.Width;
			}
			if (y < 0) {
				y = map.Height + y;
			} else if (y >= map.Height) {
				y -= map.Height;
			}
			return x + y * map.Width;
		}
		#endregion
	}
}

