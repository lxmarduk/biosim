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
			return GetIndex(x, y, Map.Width, Map.Height);
		}

		public override int GetIndex(int x, int y, int width, int height)
		{
			if (x < 0)
				throw new IndexOutOfRangeException();
			if (x >= width)
				throw new IndexOutOfRangeException();
			if (y < 0)
				throw new IndexOutOfRangeException();
			if (y >= height)
				throw new IndexOutOfRangeException();
			return x + y * width;
		}

		#endregion

	}
}

