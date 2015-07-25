/**
 * Copyright (C) 2015 Crosire
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
	using namespace System::Windows::Forms;
	using namespace System::Collections::Generic;

	ScriptSettings::ScriptSettings(String ^filename) : mFileName(filename)
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
			while (!Object::ReferenceEquals(line = reader->ReadLine(), nullptr))
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

					if (result->mValues->ContainsKey(lookup))
					{
						for (int i = 1; result->mValues->ContainsKey(lookup = String::Format("[{0}]{1}//{2}", section, key, i)->ToUpper()); ++i)
						{
							continue;
						}
					}

					result->mValues->Add(lookup, value);
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

		for each (KeyValuePair<String ^, String ^> data in this->mValues)
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
			writer = IO::File::CreateText(this->mFileName);
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
		T result;
		bool parsed = false;
		String ^value = GetValue(section, name);

		if (T::typeid == String::typeid)
		{
			parsed = true;
			result = reinterpret_cast<T>(value);
		}
		else if (T::typeid == Boolean::typeid)
		{
			parsed = Boolean::TryParse(value, reinterpret_cast<Boolean %>(result));
		}
		else if (T::typeid == Int16::typeid)
		{
			parsed = Int16::TryParse(value, reinterpret_cast<Int16 %>(result));
		}
		else if (T::typeid == UInt16::typeid)
		{
			parsed = UInt16::TryParse(value, reinterpret_cast<UInt16 %>(result));
		}
		else if (T::typeid == Int32::typeid)
		{
			parsed = Int32::TryParse(value, reinterpret_cast<Int32 %>(result));
		}
		else if (T::typeid == UInt32::typeid)
		{
			parsed = UInt32::TryParse(value, reinterpret_cast<UInt32 %>(result));
		}
		else if (T::typeid == Int64::typeid)
		{
			parsed = Int64::TryParse(value, reinterpret_cast<Int64 %>(result));
		}
		else if (T::typeid == UInt64::typeid)
		{
			parsed = UInt64::TryParse(value, reinterpret_cast<UInt64 %>(result));
		}
		else if (T::typeid == Single::typeid)
		{
			parsed = Single::TryParse(value, reinterpret_cast<Single %>(result));
		}
		else if (T::typeid == Double::typeid)
		{
			parsed = Double::TryParse(value, reinterpret_cast<Double %>(result));
		}
		else if (T::typeid == Keys::typeid)
		{
			parsed = Enum::TryParse<Keys>(value, reinterpret_cast<Keys %>(result));
		}

		if (!parsed)
		{
			return defaultvalue;
		}

		return result;
	}
	String ^ScriptSettings::GetValue(String ^section, String ^key)
	{
		return GetValue(section, key, String::Empty);
	}
	String ^ScriptSettings::GetValue(String ^section, String ^key, String ^value)
	{
		String ^lookup = String::Format("[{0}]{1}", section, key)->ToUpper();

		this->mValues->TryGetValue(lookup, value);

		return value;
	}
	array<System::String ^> ^ScriptSettings::GetAllValues(String ^section, String ^key)
	{
		String ^value = nullptr;
		List<String ^> ^values = gcnew List<String ^>();

		value = GetValue(section, key, nullptr);

		if (!Object::ReferenceEquals(value, nullptr))
		{
			values->Add(value);

			for (int i = 1; this->mValues->TryGetValue(String::Format("[{0}]{1}//{2}", section, key, i)->ToUpper(), value); ++i)
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

		if (!this->mValues->ContainsKey(lookup))
		{
			this->mValues->Add(lookup, value);
		}
		else
		{
			this->mValues->default[lookup] = value;
		}
	}
}