//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;
using System.IO;

namespace GTA
{
	public sealed class ScriptSettings
	{
		#region Fields
		readonly string _fileName;
		Dictionary<string, string> _values = new Dictionary<string, string>();
		#endregion

		ScriptSettings(string fileName)
		{
			_fileName = fileName;
		}

		public static ScriptSettings Load(string filename)
		{
			var result = new ScriptSettings(filename);

			if (!File.Exists(filename))
			{
				return result;
			}

			string line = null;
			string section = String.Empty;
			StreamReader reader = null;

			try
			{
				reader = new StreamReader(filename);
			}
			catch (IOException)
			{
				return result;
			}

			try
			{
				while (!ReferenceEquals(line = reader.ReadLine(), null))
				{
					line = line.Trim();

					if (line.Length == 0 || line.StartsWith(";") || line.StartsWith("//"))
					{
						continue;
					}

					if (line.StartsWith("[") && line.Contains("]"))
					{
						section = line.Substring(1, line.IndexOf(']') - 1).Trim();
						continue;
					}
					else if (line.Contains("="))
					{
						int index = line.IndexOf('=');
						string key = line.Substring(0, index).Trim();
						string value = line.Substring(index + 1).Trim();

						if (value.Contains("//"))
						{
							value = value.Substring(0, value.IndexOf("//") - 1).TrimEnd();
						}
						if (value.StartsWith("\"") && value.EndsWith("\""))
						{
							value = value.Substring(1, value.Length - 2);
						}

						string lookup = $"[{section}]{key}".ToUpper();

						if (result._values.ContainsKey(lookup))
						{
							for (int i = 1; result._values.ContainsKey(lookup = $"[{section}]{key}//{i}".ToUpper()); ++i)
							{
								continue;
							}
						}

						result._values.Add(lookup, value);
					}
				}
			}
			finally
			{
				reader.Close();
			}

			return result;
		}

		public bool Save()
		{
			var result = new Dictionary<string, List<Tuple<string, string>>>();

			foreach (var data in _values)
			{
				string key = data.Key.Substring(data.Key.IndexOf("]") + 1);
				string section = data.Key.Remove(data.Key.IndexOf("]")).Substring(1);

				if (!result.ContainsKey(section))
				{
					var values = new List<Tuple<string, string>>();
					values.Add(new Tuple<string, string>(key, data.Value));

					result.Add(section, values);
				}
				else
				{
					result[section].Add(new Tuple<string, string>(key, data.Value));
				}
			}

			StreamWriter writer = null;

			try
			{
				writer = File.CreateText(_fileName);
			}
			catch (IOException)
			{
				return false;
			}

			try
			{
				foreach (var section in result)
				{
					writer.WriteLine("[" + section.Key + "]");

					foreach (var value in section.Value)
					{
						writer.WriteLine(value.Item1 + " = " + value.Item2);
					}

					writer.WriteLine();
				}
			}
			catch (IOException)
			{
				return false;
			}
			finally
			{
				writer.Close();
			}

			return true;
		}

		public T GetValue<T>(string section, string name, T defaultvalue)
		{
			string value = GetValue(section, name);

			try
			{
				if (typeof(T).IsEnum)
				{
					return (T)Enum.Parse(typeof(T), value, true);
				}
				else
				{
					return (T)Convert.ChangeType(value, typeof(T));
				}
			}
			catch (Exception)
			{
				return defaultvalue;
			}
		}
		public string GetValue(string section, string key)
		{
			return GetValue(section, key, string.Empty);
		}
		public string GetValue(string section, string key, string defaultvalue)
		{
			string lookup = $"[{section}]{key}".ToUpper();

			if (_values.TryGetValue(lookup, out string value))
				return value;
			else
				return defaultvalue;
		}

		public string[] GetAllValues(string section, string key)
		{
			var values = new List<string>();
			string value = GetValue(section, key, null);

			if (!ReferenceEquals(value, null))
			{
				values.Add(value);

				for (int i = 1; _values.TryGetValue($"[{section}]{key}//{i}".ToUpper(), out value); ++i)
					values.Add(value);
			}

			return values.ToArray();
		}

		public void SetValue<T>(string section, string name, T value)
		{
			SetValue(section, name, value.ToString());
		}
		public void SetValue(string section, string key, string value)
		{
			string lookup = $"[{section}]{key}".ToUpper();

			if (!_values.ContainsKey(lookup))
				_values.Add(lookup, value);
			else
				_values[lookup] = value;
		}
	}
}
