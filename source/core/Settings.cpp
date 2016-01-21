/**
 * Copyright (C) 2015 crosire
 *
 * This software is  provided 'as-is', without any express  or implied  warranty. In no event will the
 * authors be held liable for any damages arising from the use of this software.
 * Permission  is granted  to anyone  to use  this software  for  any  purpose,  including  commercial
 * applications, and to alter it and redistribute it freely, subject to the following restrictions:
 *
 *   1. The origin of this software must not be misrepresented; you must not claim that you  wrote the
 *      original  software. If you use this  software  in a product, an  acknowledgment in the product
 *      documentation would be appreciated but is not required.
 *   2. Altered source versions must  be plainly  marked as such, and  must not be  misrepresented  as
 *      being the original software.
 *   3. This notice may not be removed or altered from any source distribution.
 */

#include "Settings.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	ScriptSettings::ScriptSettings(String ^filename) : _filename(filename), _values(gcnew Dictionary<String ^, String ^>())
	{
	}

	ScriptSettings ^ScriptSettings::Load(String ^filename)
	{
		if (!IO::File::Exists(filename))
		{
			return nullptr;
		}

		String ^line = nullptr;
		String ^section = "";
		IO::StreamReader ^reader = nullptr;
		
		try
		{
			reader = gcnew IO::StreamReader(filename);
		}
		catch (IO::IOException ^)
		{
			return nullptr;
		}

		ScriptSettings ^result = gcnew ScriptSettings(filename);

		try
		{
			while (!ReferenceEquals(line = reader->ReadLine(), nullptr))
			{
				line = line->Trim();

				if (line->Length == 0 || line->StartsWith(";") || line->StartsWith("//"))
				{
					continue;
				}

				if (line->StartsWith("[") && line->Contains("]"))
				{
					section = line->Substring(1, line->IndexOf(']') - 1)->Trim();
					continue;
				}
				else if (line->Contains("="))
				{
					int index = line->IndexOf('=');
					String ^key = line->Substring(0, index)->Trim();
					String ^value = line->Substring(index + 1)->Trim();

					if (value->Contains("//"))
					{
						value = value->Substring(0, value->IndexOf("//") - 1)->TrimEnd();
					}
					if (value->StartsWith("\"") && value->EndsWith("\""))
					{
						value = value->Substring(1, value->Length - 2);
					}

					String ^lookup = String::Format("[{0}]{1}", section, key)->ToUpper();

					if (result->_values->ContainsKey(lookup))
					{
						for (int i = 1; result->_values->ContainsKey(lookup = String::Format("[{0}]{1}//{2}", section, key, i)->ToUpper()); ++i)
						{
							continue;
						}
					}

					result->_values->Add(lookup, value);
				}
			}
		}
		finally
		{
			reader->Close();
		}

		return result;
	}
	bool ScriptSettings::Save()
	{
		Dictionary<String ^, List<Tuple<String ^, String ^> ^> ^> ^result = gcnew Dictionary<String ^, List<Tuple<String ^, String ^> ^> ^>();

		for each (KeyValuePair<String ^, String ^> data in _values)
		{
			String ^key = data.Key->Substring(data.Key->IndexOf("]") + 1);
			String ^section = data.Key->Remove(data.Key->IndexOf("]"))->Substring(1);

			if (!result->ContainsKey(section))
			{
				List<Tuple<String ^, String ^> ^> ^values = gcnew List<Tuple<String ^, String ^> ^>();
				values->Add(gcnew Tuple<String ^, String ^>(key, data.Value));

				result->Add(section, values);
			}
			else
			{
				result->default[section]->Add(gcnew Tuple<String ^, String ^>(key, data.Value));
			}
		}

		IO::StreamWriter ^writer = nullptr;

		try
		{
			writer = IO::File::CreateText(_filename);
		}
		catch (IO::IOException ^)
		{
			return false;
		}

		try
		{
			for each (KeyValuePair<String ^, List<Tuple<String ^, String ^> ^> ^> section in result)
			{
				writer->WriteLine("[" + section.Key + "]");

				for each (Tuple<String ^, String ^> ^value in section.Value)
				{
					writer->WriteLine(value->Item1 + " = " + value->Item2);
				}

				writer->WriteLine();
			}
		}
		catch (IO::IOException ^)
		{
			return false;
		}
		finally
		{
			writer->Close();
		}

		return true;
	}

	generic <typename T>
	T ScriptSettings::GetValue(String ^section, String ^name, T defaultvalue)
	{
		String ^value = GetValue(section, name);

		try
		{
			if (T::typeid->IsEnum)
			{
				return static_cast<T>(Enum::Parse(T::typeid, value, true));
			}
			else
			{
				return static_cast<T>(Convert::ChangeType(value, T::typeid));
			}
		}
		catch (Exception ^)
		{
			return defaultvalue;
		}
	}
	String ^ScriptSettings::GetValue(String ^section, String ^key)
	{
		return GetValue(section, key, String::Empty);
	}
	String ^ScriptSettings::GetValue(String ^section, String ^key, String ^value)
	{
		String ^lookup = String::Format("[{0}]{1}", section, key)->ToUpper();

		_values->TryGetValue(lookup, value);

		return value;
	}
	array<String ^> ^ScriptSettings::GetAllValues(String ^section, String ^key)
	{
		String ^value = nullptr;
		List<String ^> ^values = gcnew List<String ^>();

		value = GetValue(section, key, static_cast<String ^>(nullptr));

		if (!ReferenceEquals(value, nullptr))
		{
			values->Add(value);

			for (int i = 1; _values->TryGetValue(String::Format("[{0}]{1}//{2}", section, key, i)->ToUpper(), value); ++i)
			{
				values->Add(value);
			}
		}

		return values->ToArray();
	}
	generic <typename T>
	void ScriptSettings::SetValue(String ^section, String ^name, T value)
	{
		SetValue(section, name, reinterpret_cast<Object ^>(value)->ToString());
	}
	void ScriptSettings::SetValue(String ^section, String ^key, String ^value)
	{
		String ^lookup = String::Format("[{0}]{1}", section, key)->ToUpper();

		if (!_values->ContainsKey(lookup))
		{
			_values->Add(lookup, value);
		}
		else
		{
			_values->default[lookup] = value;
		}
	}
}
