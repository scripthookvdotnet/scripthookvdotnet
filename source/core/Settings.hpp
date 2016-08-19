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
		/// <summary>
		/// Loads a <see cref="ScriptSettings"/> from the specified file.
		/// </summary>
		/// <param name="filename">The filename to load the settings from.</param>
		static ScriptSettings ^Load(System::String ^filename);	
		/// <summary>
		/// Saves this <see cref="ScriptSettings"/> to file.
		/// </summary>
		/// <returns><c>true</c> if the file saved sucessfully; otherwise, <c>false</c></returns>
		bool Save();

		/// <summary>
		/// Reads a value from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the value is saved under.</param>
		/// <param name="defaultvalue">The fall back value is the key doesnt exist or unable to be casted to the type: <typeparamref name="T"/>.</param>
		/// <returns>The value called <see paramref="name"/> in <see paramref="section"/>.</returns>
		generic <typename T>
		T GetValue(System::String ^section, System::String ^name, T defaultvalue);
		/// <summary>
		/// Reads a value from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the value is saved under.</param>
		/// <returns>The value called <see paramref="name"/> in <see paramref="section"/>.</returns>
		System::String ^GetValue(System::String ^section, System::String ^name);
		/// <summary>
		/// Reads a value from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the value is saved under.</param>
		/// <param name="defaultvalue">The fall back value is the key doesnt exist.</param>
		/// <returns>The value called <see paramref="name"/> in <see paramref="section"/>.</returns>
		System::String ^GetValue(System::String ^section, System::String ^name, System::String ^defaultvalue);

		/// <summary>
		/// Reads all the values under a specified name in a specified section from this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the values are saved under.</param>
		array<System::String ^> ^GetAllValues(System::String ^section, System::String ^name);

		/// <summary>
		/// Sets a value in this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the value is saved under.</param>
		/// <param name="value">The value to set the key to.</param>
		generic <typename T>
		void SetValue(System::String ^section, System::String ^name, T value);
		/// <summary>
		/// Sets a value in this <see cref="ScriptSettings"/>.
		/// </summary>
		/// <param name="section">The section where the value is.</param>
		/// <param name="name">The name of the key the value is saved under.</param>
		/// <param name="value">The value to set the key to.</param>
		void SetValue(System::String ^section, System::String ^name, System::String ^value);

	private:
		ScriptSettings(System::String ^filename);

		System::String ^_filename;
		System::Collections::Generic::Dictionary<System::String ^, System::String ^> ^_values = gcnew System::Collections::Generic::Dictionary<System::String ^, System::String ^>();
	};
}
