using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using Biosim.Abstraction;
using Biosim.Implementation;

namespace Biosim
{
	public static class Utils
	{
		static Utils()
		{
		}

		public static Stream LoadResource(string name)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
		}

		public static string GetSerializationCategory(object o)
		{
			if (o.GetType().Equals(typeof(Map))) {
				return "maps";
			}
			if (o.GetType().Equals(typeof(Cell))) {
				return "cells";
			}
			if (o.GetType().Equals(typeof(AbstractRuleAction))) {
				return "rules";
			}
			return ".";
		}

		public static void Serialize(object o, string fileName)
		{
			BinaryFormatter bin = new BinaryFormatter();
			String category = GetSerializationCategory(o);
			if (!Directory.Exists(category)) {
				Directory.CreateDirectory(category);
			}
			String path = category + "/" + fileName + ".bin";
			FileStream fstr = new FileStream(path, FileMode.Create);
			bin.Serialize(fstr, o);
			fstr.Dispose();
		}

		public static void SerializeMap(Map map, string fileName)
		{
			BinaryFormatter bin = new BinaryFormatter();
			FileStream fstr = new FileStream(fileName, FileMode.Create);
			bin.Serialize(fstr, map);
			fstr.Dispose();
		}

		public static Map DeserializeMap(string fileName)
		{
			try {
				BinaryFormatter bin = new BinaryFormatter();
				FileStream fstr = new FileStream(fileName, FileMode.Open);
				Map result = (Map)bin.Deserialize(fstr);
				fstr.Dispose();
				return result;
			} catch {
				return null;
			}
		}

		public static AbstractRuleAction DeserializeRuleAction(string fileName)
		{
			BinaryFormatter bin = new BinaryFormatter();
			FileStream fstr = new FileStream(fileName, FileMode.Open);
			AbstractRuleAction result = (AbstractRuleAction)bin.Deserialize(fstr);
			fstr.Dispose();
			return result;
		}

		public static AbstractCell DeserializeCell(string fileName)
		{
			BinaryFormatter bin = new BinaryFormatter();
			FileStream fstr = new FileStream(fileName, FileMode.Open);
			AbstractCell result = (AbstractCell)bin.Deserialize(fstr);
			fstr.Dispose();
			return result;
		}

		public static String ActionTypeToString(ActionType a)
		{
			switch (a) {
				case ActionType.NoChange:
					return "Без змін";
				case ActionType.IncValue:
					return "Збільшити значення";
				case ActionType.DecValue:
					return "Зменшити значення";
				case ActionType.SetValue:
					return "Встановити значення";
				case ActionType.UnsetValue:
					return "Стерти значення";
				case ActionType.Born:
					return "Народитися";
				case ActionType.Die:
					return "Померти";
				case ActionType.StatsInc:
					return "Збільшити статистичне значення";
				case ActionType.StatsDec:
					return "Зменшити статистичне значення";
				case ActionType.IncBy:
					return "Збільшити на";
				case ActionType.DecBy:
					return "Зменшити на";
				case ActionType.SetValueTo:
					return "Встановити значення в";
			}
			return "Unknown";
		}

		public static String MapTypeToString(Map.MapType type)
		{
			switch (type) {
				case Map.MapType.Box:
					return "Замкнута";
				case Map.MapType.Continuous:
					return "Відкриті межі";
			}
			return "Unknown";
		}

		public static String CellShapeToString(Cell.CellShape shape)
		{
			switch (shape) {
				case Cell.CellShape.Square:
					return "Квадрат";
				case Cell.CellShape.Circle:
					return "Круг";
				case Cell.CellShape.Triangle:
					return "Трикутник";
				case Cell.CellShape.Diamond:				
					return "Ромб";
			}
			return "WTF?";
		}

		public static int Clamp(int val, int min, int max)
		{
			return val < min ? min : (val > max ? max : val);
		}

		public static Color Darker(Color c)
		{
			int r, g, b;
			r = Clamp(c.R - 100, 0, 255);
			g = Clamp(c.G - 100, 0, 255);
			b = Clamp(c.B - 100, 0, 255);
			return Color.FromArgb(r, g, b);
		}

		public static Color RandomColor()
		{
			Random rand = new Random();
			return Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
		}
	}
}

