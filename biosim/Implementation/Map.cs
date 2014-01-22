using System;
using System.Collections.Generic;
using Biosim.Abstraction;
using System.Windows.Forms;
using Biosim.UI;

namespace Biosim.Implementation
{
	[Serializable]
	public class Map
	{
		[NonSerialized]
		AbstractMapSelector	selector;
		[NonSerialized]
		Cell initialCell;

		public AbstractMapSelector Selector {
			get {
				if (selector == null) {
					switch (mapType) {
						case MapType.Box:
							selector = new BoxSelector(this);
							break;
						case MapType.Continuous:
							selector = new ContinuousSelector(this);
							break;
					}
				}
				return selector;
			}
		}

		int width;
		int height;

		public int Width {
			get {
				return width;
			}
			set {
				if (width != value) {
					int prev_width = width;
					Cell[] cellsCopy = new Cell[Cells.Length];
					for (int i = 0; i < Cells.Length; ++i) {
						cellsCopy [i] = (Cell)Cells [i].Clone();
					}
					int min = width < value ? width : value;
					width = value;
					Cells = new Cell[width * height];
					InitializeCells(initialCell);
					for (int y = 0; y < height; ++y) {
						for (int x = 0; x < min; ++x) {
							Cells [selector.GetIndex(x, y)] = (Cell)cellsCopy [selector.GetIndex(x, y, prev_width, height)].Clone();
						}
					}
				}
			}
		}

		public int Height {
			get {
				return height;
			}
			set {
				if (height != value) {
					int prev_height = height;
					Cell[] cellsCopy = new Cell[Cells.Length];
					for (int i = 0; i < Cells.Length; ++i) {
						cellsCopy [i] = (Cell)Cells [i].Clone();
					}
					int min = height < value ? height : value;
					height = value;
					Cells = new Cell[width * height];
					InitializeCells(initialCell);
					for (int y = 0; y < min; ++y) {
						for (int x = 0; x < width; ++x) {
							Cells [selector.GetIndex(x, y)] = (Cell)cellsCopy [selector.GetIndex(x, y, width, prev_height)].Clone();
						}
					}
				}
			}
		}

		public Cell[] Cells;
		public List<AbstractRuleAction> RuleActions;

		[Serializable]
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

		[NonSerialized]
		public int CellsBorn;
		[NonSerialized]
		public int CellsDied;
		[NonSerialized]
		public int CellsAlive;

		public Map(int width, int height)
		{
			this.width = width;
			this.height = height;
			Type = MapType.Box;
			Cells = new Cell[width * height];
			RuleActions = new List<AbstractRuleAction>();
			initialCell = new Cell("Empty");
		}

		public void InitializeCells(AbstractCell cell)
		{
			if (cell == null) {
				initialCell = new Cell("Empty");
			} else {
				initialCell = (Cell)cell.Clone();
			}
			for (int i = 0; i < Width * Height; ++i) {
				Cells [i] = (Cell)initialCell.Clone();
			}
		}

		public void SetCell(Cell cell, int x, int y)
		{
			if (selector.Select(x, y) != null) {
				Cells [selector.GetIndex(x, y)] = (Cell)cell.Clone();
				Application.DoEvents();
			}
		}

		public void Process()
		{
			Cell[] cellsCopy = new Cell[Cells.Length];
			for (int i = 0; i < Cells.Length; ++i) {
				cellsCopy [i] = (Cell)Cells [i].Clone();
			}
			CellsBorn = 0;
			CellsDied = 0;
			CellsAlive = 0;
			for (int y = 0; y < Height; ++y) {
				for (int x = 0; x < Width; ++x) {
					Cell c = (Cell)selector.Select(x, y);
				
					foreach (AbstractRuleAction r in RuleActions) {
						c.Properties ["Сусіди"].Unset();
//						switch (r.NeighboursSelectorType) {
//							case NeighboursType.All:
						for (int i = -1; i <= 1; ++i) {
							for (int j = -1; j <= 1; ++j) {
								if (i == 0 && j == 0)
									continue;
								try {
									if (selector.Select(x + i, y + j) != null) {
										if (selector.Select(x + i, y + j).Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
											if (selector.Select(x + i, y + j).Properties ["Жива"].Equ(true)) {
												c.Properties ["Сусіди"].Increment();
											}
										}
									} else {
										continue;
									}
								} catch (NullReferenceException e) {
									Console.WriteLine(e.Message);
									Console.WriteLine(e.Source);
									continue;
								}
								Application.DoEvents();
							}
						}
//								break;
//							case NeighboursType.Cross:
//								for (int i = -1; i <= 1; ++i) {
//									if (i == 0)
//										continue;
//									if (selector.Select(x + i, y) != null) {
//										if (selector.Select(x + i, y).Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
//											if (selector.Select(x + i, y).Properties ["Жива"].Equ(true)) {
//												c.Properties ["Сусіди"].Increment();
//											}
//										}
//									} else {
//										continue;
//									}
//									if (selector.Select(x, y + i) != null) {
//										if (selector.Select(x, y + i).Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
//											if (selector.Select(x, y + i).Properties ["Жива"].Equ(true)) {
//												c.Properties ["Сусіди"].Increment();
//											}
//										}
//									} else {
//										continue;
//									}
//								}
//								break;
//							case NeighboursType.DiagonalCross:
//								for (int i = -1; i <= 1; ++i) {
//									for (int j = -1; j <= 1; ++j) {
//										if (i == 0 || j == 0)
//											continue;
//										try {
//											if (selector.Select(x + i, y + j) != null) {
//												if (selector.Select(x + i, y + j).Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
//													if (selector.Select(x + i, y + j).Properties ["Жива"].Equ(true)) {
//														c.Properties ["Сусіди"].Increment();
//													}
//												}
//											} else {
//												continue;
//											}
//										} catch (NullReferenceException e) {
//											Console.WriteLine(e.Message);
//											Console.WriteLine(e.Source);
//											continue;
//										}
//										Application.DoEvents();
//									}
//								}
//								break;
//							case NeighboursType.LeftRight:
//								for (int i = -1; i <= 1; ++i) {
//									if (i == 0)
//										continue;
//									if (selector.Select(x + i, y) != null) {
//										if (selector.Select(x + i, y).Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
//											if (selector.Select(x + i, y).Properties ["Жива"].Equ(true)) {
//												c.Properties ["Сусіди"].Increment();
//											}
//										}
//									} else {
//										continue;
//									}
//								}
//								break;
//							case NeighboursType.TopBottom:
//								for (int i = -1; i <= 1; ++i) {
//									if (i == 0)
//										continue;
//									if (selector.Select(x, y + i) != null) {
//										if (selector.Select(x, y + i).Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
//											if (selector.Select(x, y + i).Properties ["Жива"].Equ(true)) {
//												c.Properties ["Сусіди"].Increment();
//											}
//										}
//									} else {
//										continue;
//									}
//								}
//								break;
//							case NeighboursType.LeftRightCross:
//								Cell c1 = (Cell)selector.Select(x - 1, y - 1);
//								Cell c2 = (Cell)selector.Select(x + 1, y + 1);
//								if (c1 != null) {
//									if (c1.Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
//										if (c1.Properties ["Жива"].Equ(true)) {
//											c.Properties ["Сусіди"].Increment();
//										}
//									}
//								}
//								if (c2 != null) {
//									if (c2.Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
//										if (c2.Properties ["Жива"].Equ(true)) {
//											c.Properties ["Сусіди"].Increment();
//										}
//									}
//								}
//								break;
//							case NeighboursType.RightLeftCross:
//								c1 = (Cell)selector.Select(x + 1, y - 1);
//								c2 = (Cell)selector.Select(x - 1, y + 1);
//								if (c1 != null) {
//									if (c1.Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
//										if (c1.Properties ["Жива"].Equ(true)) {
//											c.Properties ["Сусіди"].Increment();
//										}
//									}
//								}
//								if (c2 != null) {
//									if (c2.Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
//										if (c2.Properties ["Жива"].Equ(true)) {
//											c.Properties ["Сусіди"].Increment();
//										}
//									}
//								}
//								break;
//						}
						switch (r.Process(c)) {
							case ActionType.Born:
								cellsCopy [selector.GetIndex(x, y)] = (Cell)r.TargetCell.Clone();
								++CellsBorn;
								break;
							case ActionType.Die:
								if (c.Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
									cellsCopy [selector.GetIndex(x, y)].Properties ["Жива"].Unset();
									++CellsDied;
								}
								break;
							case ActionType.SetValue:
								if (cellsCopy [selector.GetIndex(x, y)] != null) {
									if (cellsCopy [selector.GetIndex(x, y)].Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
										if (cellsCopy [selector.GetIndex(x, y)].Properties.HasProperty(r.TargetProperty)) {
											cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Set();
										}
									}
								}
								break;
							case ActionType.SetValueTo:
								if (cellsCopy [selector.GetIndex(x, y)] != null) {
									if (cellsCopy [selector.GetIndex(x, y)].Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
										if (cellsCopy [selector.GetIndex(x, y)].Properties.HasProperty(r.TargetProperty)) {
											cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Set(r.IncrementValue);
										}
									}
								}
								break;
							case ActionType.UnsetValue:
								if (cellsCopy [selector.GetIndex(x, y)] != null) {
									if (cellsCopy [selector.GetIndex(x, y)].Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
										if (cellsCopy [selector.GetIndex(x, y)].Properties.HasProperty(r.TargetProperty)) {
											cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Unset();
										}
									}
								}
								break;
							case ActionType.IncValue:
								if (cellsCopy [selector.GetIndex(x, y)] != null) {
									if (cellsCopy [selector.GetIndex(x, y)].Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
										if (cellsCopy [selector.GetIndex(x, y)].Properties.HasProperty(r.TargetProperty)) {
											cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Increment();
										}
									}
								}
								break;
							case ActionType.IncBy:
								if (cellsCopy [selector.GetIndex(x, y)] != null) {
									if (cellsCopy [selector.GetIndex(x, y)].Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
										if (cellsCopy [selector.GetIndex(x, y)].Properties.HasProperty(r.TargetProperty)) {
											cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Increment(r.IncrementValue);
										}
									}
								}
								break;
							case ActionType.DecValue:
								if (cellsCopy [selector.GetIndex(x, y)] != null) {
									if (cellsCopy [selector.GetIndex(x, y)].Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
										if (cellsCopy [selector.GetIndex(x, y)].Properties.HasProperty(r.TargetProperty)) {
											cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Decrement();
										}
									}
								}
								break;
							case ActionType.DecBy:
								if (cellsCopy [selector.GetIndex(x, y)] != null) {
									if (cellsCopy [selector.GetIndex(x, y)].Properties ["Ім'я"].Equ(r.TargetCell.Properties ["Ім'я"].Value)) {
										if (cellsCopy [selector.GetIndex(x, y)].Properties.HasProperty(r.TargetProperty)) {
											cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Decrement(r.IncrementValue);
										}
									}
								}
								break;
							case ActionType.NoChange:
								break;
						}
					}
					if (cellsCopy [selector.GetIndex(x, y)].Properties ["Жива"].Equ(true)) {
						++CellsAlive;
					}
					Application.DoEvents();
				}
			}
			Cells = cellsCopy;
		}
	}
}

