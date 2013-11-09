using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	[Serializable]
	public class Map
	{
		[NonSerialized]
		AbstractMapSelector
			selector;

		public int Width {
			get;
			private set;
		}

		public int Height {
			get;
			private set;
		}

		public AbstractCell[] Cells;

		public enum MapType
		{
			Box,
			Continuous
		}

		MapType mapType;
		public MapType Type {
			get {
				return mapType;
			}
			set {
				mapType = value;
				selector = null;
				switch (mapType) {
					case MapType.Box:
						selector = new BoxSelector(this);
						break;
					case MapType.Continuous:
						selector = new ContinuousSelector(this);
						break;
				}
			}
		}

		public AbstractMapSelector Selector {
			get {
				return selector;
			}
		}

		public Map(int width, int height)
		{
			Width = width;
			Height = height;
			Type = MapType.Continuous;
			Cells = new AbstractCell[width * height];
		}

		public void InitializeCells(AbstractCell cell)
		{
			for (int i = 0; i < Width * Height; ++i) {
				Cells [i] = cell.Clone();
			}
		}

		public void SetCell(AbstractCell cell, int x, int y)
		{
			
		}
	}
}

