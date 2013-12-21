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
			try {
				if (x < 0)
					return null;
				if (x >= Map.Width)
					return null;
				if (y < 0)
					return null;
				if (y >= Map.Height)
					return null;
				AbstractCell cell = Map.Cells [GetIndex(x, y)];
				return cell;
			} catch (IndexOutOfRangeException e) {
				Debug.PrintError(e.Message);
				return null;
			}
		}

		public override int GetIndex(int x, int y)
		{
			return x + y * Map.Width;
		}

		#endregion

	}
}

