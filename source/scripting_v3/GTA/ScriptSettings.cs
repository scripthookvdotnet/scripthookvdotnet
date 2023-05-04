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
		Dictionary<string, Dictionary<string, List<string>>> _values = new Dictionary<string, Dictionary<string, List<string>>>();
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
			string tempSectionName = String.Empty;
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

					if (line.Length == 0 || line.StartsWith(";", StringComparison.Ordinal) || line.StartsWith("//", StringComparison.Ordinal))
					{
						continue;
					}

					if (line.StartsWith("[", StringComparison.Ordinal) && line.Contains("]"))
					{
						tempSectionName = line.Substring(1, line.IndexOf("]", StringComparison.Ordinal) - 1).Trim();
						continue;
					}
					else if (line.Contains("="))
					{
						int index = line.IndexOf("=", StringComparison.Ordinal);
						string key = line.Substring(0, index).Trim();
						string value = line.Substring(index + 1).Trim();

						if (value.Contains("//"))
						{
							value = value.Substring(0, value.IndexOf("//", StringComparison.Ordinal) - 1).TrimEnd();
						}
						if (value.StartsWith("\"", StringComparison.Ordinal) && value.EndsWith("\"", StringComparison.Ordinal))
						{
							value = value.Substring(1, value.Length - 2);
						}

						result.AddNewValueInternal(tempSectionName, key, value);
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

			foreach (var sectonAndKeyValuePairs in _values)
			{
				var sectionName = sectonAndKeyValuePairs.Key;
				foreach (var keyValuePairs in sectonAndKeyValuePairs.Value)
				{
					var keyName = keyValuePairs.Key;
					var valueList = keyValuePairs.Value;

					foreach (var value in valueList)
					{
						if (!result.ContainsKey(sectionName))
						{
							var values = new List<Tuple<string, string>> {
								new Tuple<string, string>(keyName, value)
							};

							result.Add(sectionName, values);
						}
						else
						{
							result[sectionName].Add(new Tuple<string, string>(keyName, value));
						}
					}
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
			if (!_values.TryGetValue(section, out var keyValuePairs))
			{
				return defaultvalue;
			}
			if (!keyValuePairs.TryGetValue(name, out var valueList))
			{
				return defaultvalue;
			}

			try
			{
				if (typeof(T).IsEnum)
				{
					return (T)Enum.Parse(typeof(T), valueList[0], true);
				}
				else
				{
					return (T)Convert.ChangeType(valueList[0], typeof(T));
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
			string internalValue = value.ToString();

			if (_values.TryGetValue(section, out var keyAndValuePairs) && keyAndValuePairs.TryGetValue(name, out var valueList))
			{
				// Assume the value list already occupies the index 0
				valueList[0] = internalValue;
				return;
			}

			AddNewValueInternal(section, name, internalValue);
		}

		/// <summary>
		/// Reads all the values at a specified key and section from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the values are saved at.</param>
		public T[] GetAllValues<T>(string section, string name)
		{
			if (!_values.TryGetValue(section, out var keyValuePairs))
			{
				return Array.Empty<T>();
			}
			if (!keyValuePairs.TryGetValue(name, out var stringValueList))
			{
				return Array.Empty<T>();
			}

			var values = new List<T>();
			foreach (var stringValue in stringValueList)
			{
				try
				{
					if (typeof(T).IsEnum)
					{
						values.Add((T)Enum.Parse(typeof(T), stringValue, true));
					}
					else
					{
						values.Add((T)Convert.ChangeType(stringValue, typeof(T)));
					}
				}
				catch
				{
					continue;
				}
			}

			return values.ToArray();
		}

		private void AddNewValueInternal(string sectionName, string keyName, string valueString)
		{
			if (_values.TryGetValue(sectionName, out var keyAndValuePairs))
			{
				if (keyAndValuePairs.TryGetValue(keyName, out var valueList))
				{
					valueList.Add(valueString);
				}
				else
				{
					var newValueList = new List<string>(1) { valueString };
					keyAndValuePairs.Add(keyName, newValueList);
				}
			}
			else
			{
				var newValueList = new List<string>(1) { valueString };
				var newKeyAndValuePairs = new Dictionary<string, List<string>>() { [keyName] = newValueList };

				_values.Add(sectionName, newKeyAndValuePairs);
			}
		}
	}
}
