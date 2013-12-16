using System;
using System.Collections.Generic;
using Biosim.Abstraction;
using System.Windows.Forms;

namespace Biosim.Implementation
{
	[Serializable]
	public class Map
	{
		AbstractMapSelector	selector;

		public AbstractMapSelector Selector {
			get {
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
		public List<AbstractRuleAction> ruleActions;

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
			Type = MapType.Continuous;
			Cells = new AbstractCell[width * height];
			ruleActions = new List<AbstractRuleAction>();
			ruleActions.Add(new RuleAction(
				new RuleAnd(new RuleEquals("Alive", true), new RuleOr(
					new RuleGreater("Neighbours", 3),
					new RuleLess("Neighbours", 2)
				)),
				ActionType.UnsetValue,
				"Alive"));
			ruleActions.Add(new RuleAction(
				new RuleAnd(new RuleEquals("Alive", false), new RuleEquals("Neighbours", 3)),
				ActionType.SetValue, 
				"Alive"));
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
			for (int x = 0; x < Width; ++x) {
				for (int y = 0; y < Height; ++y) {
					selector.Select(x, y).Properties ["Neighbours"].Unset();
					for (int i = -1; i <= 1; ++i) {
						for (int j = -1; j <= 1; ++j) {
							if (i == 0 && j == 0)
								continue;
							if (selector.Select(x + i, y + j) != null
							    && selector.Select(x + i, y + j).Properties ["Alive"].Equ(true)) {
								selector.Select(x, y).Properties ["Neighbours"].Increment();
							}
							Application.DoEvents();
						}
					}
				
					foreach (AbstractRuleAction r in ruleActions) {
						switch (r.Process(selector.Select(x, y))) {
							case ActionType.SetValue:
								cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Set();
								break;
							case ActionType.UnsetValue:
								cellsCopy [selector.GetIndex(x, y)].Properties [r.TargetProperty].Unset();
								break;
							case ActionType.NoChange:
								break;
						}
					}
					Application.DoEvents();
				}
			}
			for (int i = 0; i < cellsCopy.Length; ++i) {
				Cells [i] = cellsCopy [i].Clone();
				Application.DoEvents();
			}
		}
	}
}

