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

#pragma once

namespace GTA
{
	public ref class ScriptSettings sealed
	{
	public:
		static ScriptSettings ^Load(System::String ^filename);
		bool Save();

		generic <typename T>
		T GetValue(System::String ^section, System::String ^name, T defaultvalue);
		System::String ^GetValue(System::String ^section, System::String ^name);
		System::String ^GetValue(System::String ^section, System::String ^name, System::String ^defaultvalue);
		array<System::String ^> ^GetAllValues(System::String ^section, System::String ^name);
		generic <typename T>
		void SetValue(System::String ^section, System::String ^name, T value);
		void SetValue(System::String ^section, System::String ^name, System::String ^value);

	private:
		ScriptSettings(System::String ^filename);

		System::String ^_filename;
		System::Collections::Generic::Dictionary<System::String ^, System::String ^> ^_values = gcnew System::Collections::Generic::Dictionary<System::String ^, System::String ^>();
	};
}
