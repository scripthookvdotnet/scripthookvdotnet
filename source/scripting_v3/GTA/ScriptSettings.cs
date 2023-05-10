//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace GTA
{
	public sealed class ScriptSettings
	{
		#region Fields
		readonly string _fileName;
		Dictionary<string, Dictionary<string, List<string>>> _values = new Dictionary<string, Dictionary<string, List<string>>>(StringComparer.OrdinalIgnoreCase);
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
			var result = new Dictionary<string, List<Tuple<string, string>>>(StringComparer.Ordinal);

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
		/// <remarks>
		/// This overload parses using the current culture for the compatibility with scripts built against v3.6.0 or earlier.
		/// Consider using the overload <see cref="GetValue{T}(string, string, T, IFormatProvider)"/> or call this overload with string type and parse the return value with a format provider later
		/// to avoid users having trouble with culture-dependent issues, such as not recognizing a decimal points as a decimal separator for floating-point numbers.
		/// </remarks>
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
				if (typeof(T) == typeof(string))
				{
					// Performs more than 10x better than converting type via Convert.ChangeType
					return (T)(object)valueList[0];
				}
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
		/// Reads a value from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="sectionName">The section name where the value is.</param>
		/// <param name="keyName">The name of the key the value is saved at.</param>
		/// <param name="defaultValue">The fall-back value if the key doesn't exist or casting to type <typeparamref name="T"/> fails.</param>
		/// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
		/// <returns>The value at <see paramref="name"/> in <see paramref="section"/>.</returns>
		public T GetValue<T>(string sectionName, string keyName, T defaultValue, IFormatProvider formatProvider)
		{
			if (!_values.TryGetValue(sectionName, out var keyValuePairs))
			{
				return defaultValue;
			}
			if (!keyValuePairs.TryGetValue(keyName, out var valueList))
			{
				return defaultValue;
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
				else
				{
					return formatProvider != null ? (T)Convert.ChangeType(valueList[0], typeof(T), formatProvider) : (T)Convert.ChangeType(valueList[0], typeof(T));
				}
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// Reads a value from this <see cref="ScriptSettings"/> using <see cref="CultureInfo.InvariantCulture"/>.
		/// </summary>
		/// <param name="sectionName">The section name where the value is.</param>
		/// <param name="keyName">The name of the key the value is saved at.</param>
		/// <param name="value">
		/// When this method returns, contains the value associated with the specified section and key, if the key is found;
		/// otherwise, the default value for the type of the value parameter.
		/// </param>
		/// <returns><see langword="true"/> if the <see cref="ScriptSettings"/> contains a value with the specified section and key; otherwise, <see langword="false"/>.</returns>
		public bool TryGetValue<T>(string sectionName, string keyName, out T value) => TryGetValue(sectionName, keyName, out value, CultureInfo.InvariantCulture);

		/// <summary>
		/// Reads a value from this <see cref="ScriptSettings"/> using <paramref name="formatProvider"/>.
		/// </summary>
		/// <param name="sectionName">The section name where the value is.</param>
		/// <param name="keyName">The name of the key the value is saved at.</param>
		/// <param name="value">
		/// When this method returns, contains the value associated with the specified section and key, if the key is found;
		/// otherwise, the default value for the type of the value parameter.
		/// </param>
		/// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
		/// <returns><see langword="true"/> if the <see cref="ScriptSettings"/> contains a value with the specified section and key; otherwise, <see langword="false"/>.</returns>
		public bool TryGetValue<T>(string sectionName, string keyName, out T value, IFormatProvider formatProvider)
		{
			if (!_values.TryGetValue(sectionName, out var keyValuePairs))
			{
				value = default(T);
				return false;
			}
			if (!keyValuePairs.TryGetValue(keyName, out var valueList))
			{
				value = default(T);
				return false;
			}

			try
			{
				if (typeof(T) == typeof(string))
				{
					value = (T)(object)valueList[0];
					return true;
				}
				if (typeof(T).IsEnum)
				{
					value = (T)Enum.Parse(typeof(T), valueList[0], true);
					return true;
				}
				else
				{
					value = (T)Convert.ChangeType(valueList[0], typeof(T), formatProvider);
					return true;
				}
			}
			catch (Exception)
			{
				value = default(T);
				return false;
			}
		}

		/// <summary>
		/// Sets a value in this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the value is saved at.</param>
		/// <param name="value">The value to set the key to.</param>
		/// <remarks>
		/// <para>
		/// Overwrites the first value at a specified section and name and ignore the other values
		/// if multiple values are set at a specified section and name.
		/// </para>
		/// <para>
		/// This overload parses using the current culture for the compatibility with scripts built against v3.6.0 or earlier.
		/// Consider using the overload <see cref="SetValue{T}(string, string, T, string, IFormatProvider)"/> or call this overload with string type and format the return value later
		/// to avoid users having trouble with culture-dependent issues, such as not recognizing a decimal points as a decimal separator for floating-point numbers.
		/// </para>
		/// </remarks>
		public void SetValue<T>(string section, string name, T value)
		{
			var internalValue = value.ToString();

			if (_values.TryGetValue(section, out var keyAndValuePairs) && keyAndValuePairs.TryGetValue(name, out var valueList))
			{
				// Assume the value list already occupies the index 0
				valueList[0] = internalValue;
				return;
			}

			AddNewValueInternal(section, name, internalValue);
		}
		/// <summary>
		/// Sets a value in this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="sectionName">The section where the value is.</param>
		/// <param name="keyName">The name of the key the value is saved at.</param>
		/// <param name="value">The value to set the key to.</param>
		/// <param name="format">The format to use.</param>
		/// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
		/// <remarks>
		/// Overwrites the first value at a specified section and name and ignore the other values
		/// if multiple values are set at a specified section and name.
		/// </remarks>
		public void SetValue<T>(string sectionName, string keyName, T value, string format, IFormatProvider formatProvider) where T : IFormattable
		{
			var internalValue = formatProvider != null ? value.ToString(format, formatProvider) : value.ToString();

			if (_values.TryGetValue(sectionName, out var keyAndValuePairs) && keyAndValuePairs.TryGetValue(keyName, out var valueList))
			{
				// Assume the value list already occupies the index 0
				valueList[0] = internalValue;
				return;
			}

			AddNewValueInternal(sectionName, keyName, internalValue);
		}

		/// <summary>
		/// Reads all the values at a specified key and section from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the values are saved at.</param>
		/// <remarks>
		/// <para>
		/// You can set multiple values at a specified section and key by writing key and value pairs
		/// at the same section and key in multiple lines.
		/// </para>
		/// <para>
		/// This overload parses using the current culture for the compatibility with scripts built against v3.6.0 or earlier.
		/// Consider using the overload <see cref="GetValue{T}(string, string, T, IFormatProvider)"/> to avoid users having trouble with culture-dependent issues,
		/// such as not recognizing a decimal points as a decimal separator for floating-point numbers.
		/// </para>
		/// </remarks>
		public T[] GetAllValues<T>(string section, string name) => GetAllValues<T>(section, name, null);

		/// <summary>
		/// Reads all the values at a specified key and section from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="sectionName">The section name where the value is.</param>
		/// <param name="keyName">The name of the key the values are saved at.</param>
		/// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
		/// <remarks>
		/// You can set multiple values at a specified section and key by writing key and value pairs
		/// at the same section and key in multiple lines.
		/// </remarks>
		public T[] GetAllValues<T>(string sectionName, string keyName, IFormatProvider formatProvider)
		{
			if (!_values.TryGetValue(sectionName, out var keyValuePairs))
			{
				return Array.Empty<T>();
			}
			if (!keyValuePairs.TryGetValue(keyName, out var stringValueList))
			{
				return Array.Empty<T>();
			}

			var values = new List<T>();
			foreach (var stringValue in stringValueList)
			{
				try
				{
					if (typeof(T) == typeof(string))
					{
						values.Add((T)(object)stringValue);
					}
					if (typeof(T).IsEnum)
					{
						values.Add((T)Enum.Parse(typeof(T), stringValue, true));
					}
					else
					{
						var parsedValue = formatProvider != null ? (T)Convert.ChangeType(stringValue, typeof(T), formatProvider) : (T)Convert.ChangeType(stringValue, typeof(T));
						values.Add(parsedValue);
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
				var newKeyAndValuePairs = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase) { [keyName] = newValueList };

				_values.Add(sectionName, newKeyAndValuePairs);
			}
		}

		/// <summary>
		/// Gets a value that indicates whether this <see cref="ScriptSettings"/> contains the specified section.
		/// </summary>
		public bool ContainsSection(string section) => _values.ContainsKey(section);

		/// <summary>
		/// Gets a value that indicates whether this <see cref="ScriptSettings"/> contains the specified key at the specified section.
		/// </summary>
		public bool ContainsKey(string sectionName, string keyName) => _values.TryGetValue(sectionName, out var keyValuePairs) && keyValuePairs.ContainsKey(keyName);

		/// <summary>
		/// Gets all of the section names this <see cref="ScriptSettings"/> contains.
		/// </summary>
		public string[] GetAllSectionNames()
		{
			var result = new string[_values.Count];
			_values.Keys.CopyTo(result, 0);

			return result;
		}

		/// <summary>
		/// Gets all of the key names at the specified section name this <see cref="ScriptSettings"/> contains.
		/// </summary>
		/// <param name="sectionName">The section name.</param>
		public string[] GetAllKeyNames(string sectionName)
		{
			if (!_values.TryGetValue(sectionName, out var keyValuePairs))
			{
				return Array.Empty<string>();
			}

			var result = new string[keyValuePairs.Count];
			keyValuePairs.Keys.CopyTo(result, 0);

			return result;
		}

		/// <summary>
		/// Removes all of the keys of the specified section this <see cref="ScriptSettings"/> has the key.
		/// </summary>
		/// <param name="sectionName">The section name.</param>
		/// <param name="keyName">The name of the key.</param>
		/// <returns><see langword="true"/> if the <see cref="ScriptSettings"/> contained the specified key at the specified section and removed the key; otherwise, <see langword="false"/>.</returns>
		public bool RemoveKey(string sectionName, string keyName)
		{
			if (!_values.TryGetValue(sectionName, out var keyValuePairs))
			{
				return false;
			}

			return keyValuePairs.Remove(keyName);
		}

		/// <summary>
		/// Removes the specified section if this <see cref="ScriptSettings"/> has the section.
		/// </summary>
		/// <param name="sectionName">The section name where the value is.</param>
		/// <returns><see langword="true"/> if the <see cref="ScriptSettings"/> contained the specified section and removed the section; otherwise, <see langword="false"/>.</returns>
		public bool RemoveSection(string sectionName) => _values.Remove(sectionName);

		/// <summary>
		/// Clears all sections this <see cref="ScriptSettings"/> has.
		/// </summary>
		public void Clear() => _values.Clear();
	}
}
