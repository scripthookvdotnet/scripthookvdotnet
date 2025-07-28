//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace GTA
{
    /// <summary>
    /// Represents script settings written in the INI format.
    /// </summary>
    /// <remarks>
    /// The file encoding must be one of the following:
    /// <list type="bullet">
    ///   <item><description>UTF-8 (with or without BOM)</description></item>
    ///   <item><description>UTF-16 LE (with BOM)</description></item>
    ///   <item><description>UTF-16 BE (with BOM)</description></item>
    ///   <item><description>UTF-32 LE (with BOM)</description></item>
    ///   <item><description>UTF-32 BE (with BOM)</description></item>
    /// </list>
    /// When no BOM is present, UTF-8 is assumed.
    /// </remarks>

    public sealed class ScriptSettings
    {
        #region Fields
        private readonly string _fileName;
        private readonly Dictionary<string, Dictionary<string, List<string>>> _values = new(StringComparer.OrdinalIgnoreCase);
        #endregion

        ScriptSettings(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Loads a <see cref="ScriptSettings"/> from the specified file.
        /// </summary>
        /// <param name="filename">The filename to load the settings from.</param>
        /// <remarks>
        /// If this method cannot load the file due to <see cref="IOException"/>, the created instance will not contain any setting values.
        /// </remarks>
        public static ScriptSettings Load(string filename)
        {
            var result = new ScriptSettings(filename);

            if (!File.Exists(filename))
                return result;

            try
            {
                using var reader = new StreamReader(filename, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
                string section = string.Empty;
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.Length == 0 || line.StartsWith(";") || line.StartsWith("//"))
                        continue;

                    while (line.EndsWith("\\"))
                    {
                        string nextLine = reader.ReadLine();
                        if (nextLine == null)
                            break;

                        line = line.Substring(0, line.Length - 1) + nextLine.TrimStart();
                    }

                    if (line.StartsWith("[") && line.Contains("]"))
                    {
                        int indexOfSectionEnd = line.IndexOf(']');
                        if (indexOfSectionEnd != -1)
                        {
                            section = line.Substring(1, indexOfSectionEnd - 1).Trim();
                            continue;
                        }
                    }

                    int indexOfEqual = line.IndexOf('=');
                    if (indexOfEqual != -1)
                    {
                        string key = line.Substring(0, indexOfEqual).Trim();
                        string value = line.Substring(indexOfEqual + 1).Trim();

                        value = StripInlineComment(value);

                        if (value.StartsWith("\"") && value.EndsWith("\"") && value.Length >= 2)
                        {
                            value = value.Substring(1, value.Length - 2);
                            value = value.Replace("\\\"", "\"");
                        }

                        result.AddNewValueInternal(section, key, value);
                    }
                    else
                    {
                        string key = StripInlineComment(line).Trim();
                        if (!string.IsNullOrEmpty(key))
                        {
                            result.AddNewValueInternal(section, key, "");
                        }
                    }
                }
            }
            catch (IOException)
            {
                return result;
            }

            return result;
        }

        /// <summary>
        /// Saves this <see cref="ScriptSettings"/> to file.
        /// </summary>
        /// <returns><see langword="true" /> if the file saved successfully; otherwise, <see langword="false" /></returns>
        public bool Save()
        {
            var result = new Dictionary<string, List<(string Key, string Value)>>(StringComparer.Ordinal);

            foreach (var sectionAndKeyValuePairs in _values)
            {
                string sectionName = sectionAndKeyValuePairs.Key;

                foreach (var keyValuePairs in sectionAndKeyValuePairs.Value)
                {
                    string keyName = keyValuePairs.Key;
                    List<string> valueList = keyValuePairs.Value;

                    if (!result.TryGetValue(sectionName, out var list))
                    {
                        list = new List<(string, string)>();
                        result.Add(sectionName, list);
                    }

                    foreach (var value in valueList)
                    {
                        list.Add((keyName, value));
                    }
                }
            }

            try
            {
                using var writer = File.CreateText(_fileName);

                foreach (var section in result)
                {
                    writer.WriteLine($"[{section.Key}]");

                    foreach (var (key, value) in section.Value)
                    {
                        writer.WriteLine($"{key} = {EscapeValue(value)}");
                    }

                    writer.WriteLine();
                }
            }
            catch (IOException)
            {
                return false;
            }

            return true;
        }

        private string EscapeValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            // If value contains whitespace, comment chars, or '=' -> quote it
            if (value.IndexOfAny(new char[] { ' ', '\t', ';', '=', '/' }) != -1)
            {
                // Escape internal quotes
                string escaped = value.Replace("\"", "\\\"");
                return $"\"{escaped}\"";
            }

            return value;
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
            if (!_values.TryGetValue(section, out Dictionary<string, List<string>> keyValuePairs))
                return defaultvalue;

            if (!keyValuePairs.TryGetValue(name, out List<string> valueList) || valueList.Count == 0)
                return defaultvalue;

            string rawValue = valueList[0].Trim();

            try
            {
                Type targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

                if (targetType == typeof(string))
                    return (T)(object)rawValue;

                if (targetType.IsEnum)
                    return (T)Enum.Parse(targetType, rawValue, ignoreCase: true);

                if (targetType == typeof(int) && int.TryParse(rawValue, out int intValue))
                    return (T)(object)intValue;

                if (targetType == typeof(bool) && bool.TryParse(rawValue, out bool boolValue))
                    return (T)(object)boolValue;

                if (targetType == typeof(double) && double.TryParse(rawValue, out double doubleValue))
                    return (T)(object)doubleValue;

                if (targetType == typeof(float) && float.TryParse(rawValue, out float floatValue))
                    return (T)(object)floatValue;

                // fallback for other IConvertible types
                return (T)Convert.ChangeType(rawValue, targetType);
            }
            catch
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
        public T GetValue<T>(string sectionName, string keyName, T defaultValue, IFormatProvider formatProvider) where T : IConvertible
        {
            if (!_values.TryGetValue(sectionName, out Dictionary<string, List<string>> keyValuePairs))
            {
                return defaultValue;
            }
            if (!keyValuePairs.TryGetValue(keyName, out List<string> valueList))
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

                return formatProvider != null ? (T)Convert.ChangeType(valueList[0], typeof(T), formatProvider) : (T)Convert.ChangeType(valueList[0], typeof(T));
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
        public bool TryGetValue<T>(string sectionName, string keyName, out T value) where T : IConvertible
            => TryGetValue(sectionName, keyName, out value, CultureInfo.InvariantCulture);

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
        public bool TryGetValue<T>(string sectionName, string keyName, out T value, IFormatProvider formatProvider) where T : IConvertible
        {
            if (!_values.TryGetValue(sectionName, out Dictionary<string, List<string>> keyValuePairs))
            {
                value = default(T);
                return false;
            }
            if (!keyValuePairs.TryGetValue(keyName, out List<string> valueList))
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

                value = (T)Convert.ChangeType(valueList[0], typeof(T), formatProvider);
                return true;
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
            string internalValue = value.ToString();

            if (_values.TryGetValue(section, out Dictionary<string, List<string>> keyAndValuePairs) && keyAndValuePairs.TryGetValue(name, out List<string> valueList))
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
        public void SetValue<T>(string sectionName, string keyName, T value, string format, IFormatProvider formatProvider) where T : IConvertible, IFormattable
        {
            string internalValue = formatProvider != null ? value.ToString(format, formatProvider) : value.ToString();

            if (_values.TryGetValue(sectionName, out Dictionary<string, List<string>> keyAndValuePairs) && keyAndValuePairs.TryGetValue(keyName, out List<string> valueList))
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
        public T[] GetAllValues<T>(string section, string name)
        {
            if (!_values.TryGetValue(section, out Dictionary<string, List<string>> keyValuePairs))
            {
                return Array.Empty<T>();
            }
            if (!keyValuePairs.TryGetValue(name, out List<string> stringValueList))
            {
                return Array.Empty<T>();
            }

            var values = new List<T>();
            foreach (string stringValue in stringValueList)
            {
                try
                {
                    if (typeof(T) == typeof(string))
                    {
                        values.Add((T)(object)stringValue);
                    }
                    else
                    if (typeof(T).IsEnum)
                    {
                        values.Add((T)Enum.Parse(typeof(T), stringValue, true));
                    }
                    else
                    {
                        var parsedValue = (T)Convert.ChangeType(stringValue, typeof(T));
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
        public T[] GetAllValues<T>(string sectionName, string keyName, IFormatProvider formatProvider) where T : IConvertible
        {
            if (!_values.TryGetValue(sectionName, out Dictionary<string, List<string>> keyValuePairs))
            {
                return Array.Empty<T>();
            }
            if (!keyValuePairs.TryGetValue(keyName, out List<string> stringValueList))
            {
                return Array.Empty<T>();
            }

            var values = new List<T>();
            foreach (string stringValue in stringValueList)
            {
                try
                {
                    if (typeof(T) == typeof(string))
                    {
                        values.Add((T)(object)stringValue);
                    }
                    else if (typeof(T).IsEnum)
                    {
                        values.Add((T)Enum.Parse(typeof(T), stringValue, true));
                    }
                    else
                    {
                        T parsedValue = formatProvider != null ? (T)Convert.ChangeType(stringValue, typeof(T), formatProvider) : (T)Convert.ChangeType(stringValue, typeof(T));
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

        /// <summary>
        /// Gets a value that indicates whether this <see cref="ScriptSettings"/> contains the specified section.
        /// </summary>
        public bool ContainsSection(string section) => _values.ContainsKey(section);

        /// <summary>
        /// Gets a value that indicates whether this <see cref="ScriptSettings"/> contains the specified key at the specified section.
        /// </summary>
        public bool ContainsKey(string sectionName, string keyName) => _values.TryGetValue(sectionName, out Dictionary<string, List<string>> keyValuePairs) && keyValuePairs.ContainsKey(keyName);

        /// <summary>
        /// Gets all the section names this <see cref="ScriptSettings"/> contains.
        /// </summary>
        public string[] GetAllSectionNames()
        {
            string[] result = new string[_values.Count];
            _values.Keys.CopyTo(result, 0);

            return result;
        }

        /// <summary>
        /// Gets all the key names at the specified section name this <see cref="ScriptSettings"/> contains.
        /// </summary>
        /// <param name="sectionName">The section name.</param>
        public string[] GetAllKeyNames(string sectionName)
        {
            if (!_values.TryGetValue(sectionName, out Dictionary<string, List<string>> keyValuePairs))
            {
                return Array.Empty<string>();
            }

            string[] result = new string[keyValuePairs.Count];
            keyValuePairs.Keys.CopyTo(result, 0);

            return result;
        }

        /// <summary>
        /// Removes all the keys of the specified section this <see cref="ScriptSettings"/> has the key.
        /// </summary>
        /// <param name="sectionName">The section name.</param>
        /// <param name="keyName">The name of the key.</param>
        /// <returns><see langword="true"/> if the <see cref="ScriptSettings"/> contained the specified key at the specified section and removed the key; otherwise, <see langword="false"/>.</returns>
        public bool RemoveKey(string sectionName, string keyName)
        {
            if (!_values.TryGetValue(sectionName, out Dictionary<string, List<string>> keyValuePairs))
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

        private static string StripInlineComment(string input)
        {
            bool inQuotes = false;
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == '"') inQuotes = !inQuotes;

                if (!inQuotes)
                {
                    if (input[i] == '/' && input[i + 1] == '/')
                        return input.Substring(0, i).TrimEnd();

                    if (input[i] == ';')
                        return input.Substring(0, i).TrimEnd();
                }
            }
            return input;
        }
    }
}
