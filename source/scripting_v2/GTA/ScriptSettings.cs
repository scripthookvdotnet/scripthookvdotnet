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

		/// <summary>
		/// Reads a value from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the value is saved at.</param>
		/// <param name="defaultvalue">The fall-back value if the key doesn't exist or casting to type <typeparamref name="T"/> fails.</param>
		/// <returns>The value at <see paramref="name"/> in <see paramref="section"/>.</returns>
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
			string lookup = $"[{section}]{key}".ToUpper();

			if (_values.TryGetValue(lookup, out string value))
				return value;
			else
				return defaultvalue;
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
			string lookup = $"[{section}]{key}".ToUpper();

			if (!_values.ContainsKey(lookup))
				_values.Add(lookup, value);
			else
				_values[lookup] = value;
		}
	}
}
