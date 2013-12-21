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

		public int Width {
			get;
			private set;
		}

		public int Height {
			get;
			private set;
		}

		public AbstractCell[] Cells;
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

		public Map(int width, int height)
		{
			Width = width;
			Height = height;
			Type = MapType.Box;
			Cells = new AbstractCell[width * height];
			RuleActions = new List<AbstractRuleAction>();
			RuleActions.Add(
				new RuleAction(MainWindow.dummy, 
					new RuleAnd(
						new RuleEquals("Alive", false), 
						new RuleEquals("Neighbours", 3)), 
					ActionType.Born, ""));
			RuleActions.Add(
				new RuleAction(MainWindow.dummy, 
					new RuleAnd(
						new RuleEquals("Alive", true), 
						new RuleOr(
							new RuleGreater("Neighbours", 3), 
							new RuleLess("Neighbours", 2)
						)
					), ActionType.Die, ""));


			foreach (var r in RuleActions) {
				Console.WriteLine(r);
			}
		}

		public void InitializeCells(AbstractCell cell)
		{
			for (int i = 0; i < Width * Height; ++i) {
				Cells [i] = cell.Clone();
			}
		}

		public void SetCell(AbstractCell cell, int x, int y)
		{
			Cells [selector.GetIndex(x, y)] = cell;
			Application.DoEvents();
		}

		public void Process()
		{
			AbstractCell[] cellsCopy = new AbstractCell[Cells.Length];
			for (int i = 0; i < Cells.Length; ++i) {
				cellsCopy [i] = Cells [i].Clone();
			}
			for (int y = 0; y < Height; ++y) {
				for (int x = 0; x < Width; ++x) {
					AbstractCell c = selector.Select(x, y);
				
					foreach (AbstractRuleAction r in RuleActions) {
						c.Properties ["Neighbours"].Unset();
						for (int i = -1; i <= 1; ++i) {
							for (int j = -1; j <= 1; ++j) {
								if (i == 0 && j == 0)
									continue;
								try {
									if (selector.Select(x + i, y + j) != null) {
										if (selector.Select(x + i, y + j).Properties ["Name"].Equ(r.TargetCell.Properties ["Name"].Value)) {
											if (selector.Select(x + i, y + j).Properties ["Alive"].Equ(true)) {
												c.Properties ["Neighbours"].Increment();
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

						switch (r.Process(c)) {
							case ActionType.Born:
								cellsCopy [selector.GetIndex(x, y)] = r.TargetCell.Clone();
								break;
							case ActionType.Die:
								if (c.Properties ["Name"].Equ(r.TargetCell.Properties ["Name"].Value)) {
									cellsCopy [selector.GetIndex(x, y)].Properties ["Alive"].Unset();
								}
								break;
							case ActionType.SetValue:
								cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Set();
								break;
							case ActionType.UnsetValue:
								cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Unset();
								break;
							case ActionType.IncValue:
								cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Increment();
								break;
							case ActionType.DecValue:
								cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Decrement();
								break;
							case ActionType.NoChange:
								break;
						}
					}
					Application.DoEvents();
				}
			}
			Cells = cellsCopy;
		}
	}
}

