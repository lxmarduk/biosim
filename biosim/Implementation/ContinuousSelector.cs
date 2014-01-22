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
			return GetIndex(x, y, Map.Width, Map.Height);
		}

		public override int GetIndex(int x, int y, int width, int height)
		{
			if (x < 0) {
				x = width + x;
			} else if (x >= width) {
				x -= width;
			}
			if (y < 0) {
				y = height + y;
			} else if (y >= height) {
				y -= height;
			}
			return x + y * width;
		}

		#endregion

	}
}

