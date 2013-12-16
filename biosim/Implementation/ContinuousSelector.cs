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
			return Map.Cells [GetIndex(x, y)];
		}

		public override int GetIndex(int x, int y)
		{
			if (x < 0) {
				x = Map.Width + x;
			} else if (x >= Map.Width) {
				x -= Map.Width;
			}
			if (y < 0) {
				y = Map.Height + y;
			} else if (y >= Map.Height) {
				y -= Map.Height;
			}
			return x + y * Map.Width;
		}
		#endregion
	}
}

