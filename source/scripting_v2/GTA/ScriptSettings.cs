//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;
using System.IO;

namespace GTA
{
	public sealed class ScriptSettings
	{
		#region Fields

		private readonly string _fileName;
		private readonly Dictionary<string, Dictionary<string, List<string>>> _values = new(StringComparer.OrdinalIgnoreCase);
		#endregion

		private ScriptSettings(string fileName)
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
			string tempSectionName = string.Empty;
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

					if (line.Contains("="))
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
			var result = new Dictionary<string, List<Tuple<string, string>>>(StringComparer.Ordinal);

			foreach (KeyValuePair<string, Dictionary<string, List<string>>> sectionAndKeyValuePairs in _values)
			{
				string sectionName = sectionAndKeyValuePairs.Key;
				foreach (KeyValuePair<string, List<string>> keyValuePairs in sectionAndKeyValuePairs.Value)
				{
					string keyName = keyValuePairs.Key;
					List<string> valueList = keyValuePairs.Value;

					foreach (string value in valueList)
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
				foreach (KeyValuePair<string, List<Tuple<string, string>>> section in result)
				{
					writer.WriteLine("[" + section.Key + "]");

					foreach (Tuple<string, string> value in section.Value)
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
			if (!_values.TryGetValue(section, out Dictionary<string, List<string>> keyValuePairs))
			{
				return defaultvalue;
			}
			if (!keyValuePairs.TryGetValue(name, out List<string> valueList))
			{
				return defaultvalue;
			}

			try
			{
				if (typeof(T) == typeof(string))
				{
					// Performs more than 10x better than converting type via Convert.ChangeType
					return (T)(object)valueList[0];
				}
				if (typeof(T).IsEnum)
				{
					return (T)Enum.Parse(typeof(T), valueList[0], true);
				}

				return (T)Convert.ChangeType(valueList[0], typeof(T));
			}
			catch (Exception)
			{
				return defaultvalue;
			}
		}

		/// <summary>
		/// Reads a value from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="key">The name of the key the value is saved at.</param>
		/// <returns>The value at <see paramref="name"/> in <see paramref="section"/>.</returns>
		/// <remarks>If fails to get the value, this method returns <see cref="string.Empty"/>.</remarks>
		public string GetValue(string section, string key)
		{
			return GetValue(section, key, string.Empty);
		}
		/// <summary>
		/// Reads a value from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="key">The name of the key the value is saved at.</param>
		/// <param name="defaultvalue">The fall-back value if the key doesn't exist.</param>
		/// <returns>The value at <see paramref="name"/> in <see paramref="section"/>.</returns>
		public string GetValue(string section, string key, string defaultvalue)
		{
			if (!_values.TryGetValue(section, out Dictionary<string, List<string>> keyValuePairs))
			{
				return defaultvalue;
			}
			if (!keyValuePairs.TryGetValue(key, out List<string> valueList))
			{
				return defaultvalue;
			}

			return valueList[0];
		}

		/// <summary>
		/// Reads all the values at a specified key and section from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="key">The name of the key the values are saved at.</param>
		/// <remarks>
		/// You can set multiple values at a specified section and key by writing key and value pairs
		/// at the same section and key in multiple lines.
		/// </remarks>
		public string[] GetAllValues(string section, string key)
		{
			if (!_values.TryGetValue(section, out Dictionary<string, List<string>> keyValuePairs))
			{
				return Array.Empty<string>();
			}
			if (!keyValuePairs.TryGetValue(key, out List<string> stringValueList))
			{
				return Array.Empty<string>();
			}

			return stringValueList.ToArray();
		}

		/// <summary>
		/// Sets a value in this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the value is saved at.</param>
		/// <param name="value">The value to set the key to.</param>
		/// <remarks>
		/// Overwrites the first value at a specified section and name and ignore the other values
		/// if multiple values are set at a specified section and name.
		/// </remarks>
		public void SetValue<T>(string section, string name, T value)
		{
			SetValue(section, name, value.ToString());
		}
		/// <summary>
		/// Sets a value in this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="key">The name of the key the value is saved at.</param>
		/// <param name="value">The string value to set the key to.</param>
		/// <remarks>
		/// Overwrites the first value at a specified section and name and ignore the other values
		/// if multiple values are set at a specified section and name.
		/// </remarks>
		public void SetValue(string section, string key, string value)
		{
			if (_values.TryGetValue(section, out Dictionary<string, List<string>> keyAndValuePairs) && keyAndValuePairs.TryGetValue(key, out List<string> valueList))
			{
				// Assume the value list already occupies the index 0
				valueList[0] = value;
				return;
			}

			AddNewValueInternal(section, key, value);
		}

		private void AddNewValueInternal(string sectionName, string keyName, string valueString)
		{
			if (_values.TryGetValue(sectionName, out Dictionary<string, List<string>> keyAndValuePairs))
			{
				if (keyAndValuePairs.TryGetValue(keyName, out List<string> valueList))
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
				var newKeyAndValuePairs = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase) { [keyName] = newValueList };

				_values.Add(sectionName, newKeyAndValuePairs);
			}
		}
	}
}
