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

		public static Map DeserializeMap(string fileName)
		{
			BinaryFormatter bin = new BinaryFormatter();
			FileStream fstr = new FileStream(fileName, FileMode.Open);
			Map result = (Map)bin.Deserialize(fstr);
			fstr.Dispose();
			return result;
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
	}
}

