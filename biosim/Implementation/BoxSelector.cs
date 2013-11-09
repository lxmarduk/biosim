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
				AbstractCell cell = map.Cells [GetIndex(x, y)];
				return cell;
			} catch (IndexOutOfRangeException e) {
				Debug.PrintError(e.Message);
				return null;
			}
		}

		public override int GetIndex(int x, int y)
		{
			return x + y * map.Width;
		}
		#endregion


	}
}

