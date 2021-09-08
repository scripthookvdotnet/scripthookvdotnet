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

		/// <summary>
		/// Loads a <see cref="ScriptSettings"/> from the specified file.
		/// </summary>
		/// <param name="filename">The filename to load the settings from.</param>
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

						string lookup = $"[{section}]{key}//0".ToUpper();

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

		/// <summary>
		/// Saves this <see cref="ScriptSettings"/> to file.
		/// </summary>
		/// <returns><see langword="true" /> if the file saved successfully; otherwise, <see langword="false" /></returns>
		public bool Save()
		{
			var result = new Dictionary<string, List<Tuple<string, string>>>();

			foreach (var data in _values)
			{
				string key = data.Key.Substring(data.Key.IndexOf("]") + 1);
				string section = data.Key.Remove(data.Key.IndexOf("]")).Substring(1);

				// Strip array index
				key = key.Remove(key.LastIndexOf("//"));

				if (!result.ContainsKey(section))
				{
					var values = new List<Tuple<string, string>> {
						new Tuple<string, string>(key, data.Value)
					};

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

		/// <summary>
		/// Reads a value from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the value is saved at.</param>
		/// <param name="defaultvalue">The fall-back value if the key doesn't exist or casting to type <typeparamref name="T"/> fails.</param>
		/// <returns>The value at <see paramref="name"/> in <see paramref="section"/>.</returns>
		public T GetValue<T>(string section, string name, T defaultvalue)
		{
			string lookup = $"[{section}]{name}//0".ToUpper();

			if (!_values.TryGetValue(lookup, out string internalValue))
			{
				return defaultvalue;
			}

			try
			{
				if (typeof(T).IsEnum)
				{
					return (T)Enum.Parse(typeof(T), internalValue, true);
				}
				else
				{
					return (T)Convert.ChangeType(internalValue, typeof(T));
				}
			}
			catch (Exception)
			{
				return defaultvalue;
			}
		}
		/// <summary>
		/// Sets a value in this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the value is saved at.</param>
		/// <param name="value">The value to set the key to.</param>
		public void SetValue<T>(string section, string name, T value)
		{
			string lookup = $"[{section}]{name}//0".ToUpper();
			string internalValue = value.ToString();

			if (!_values.ContainsKey(lookup))
			{
				_values.Add(lookup, internalValue);
			}

			_values[lookup] = internalValue;
		}

		/// <summary>
		/// Reads all the values at a specified key and section from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the values are saved at.</param>
		public T[] GetAllValues<T>(string section, string name)
		{
			var values = new List<T>();

			for (int i = 0; _values.TryGetValue($"[{section}]{name}//{i}".ToUpper(), out string internalValue); ++i)
			{
				try
				{
					if (typeof(T).IsEnum)
					{
						values.Add((T)Enum.Parse(typeof(T), internalValue, true));
					}
					else
					{
						values.Add((T)Convert.ChangeType(internalValue, typeof(T)));
					}
				}
				catch
				{
					continue;
				}
			}

			return values.ToArray();
		}
	}
}
