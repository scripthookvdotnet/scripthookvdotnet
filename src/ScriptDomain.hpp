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

#pragma once

#include "Script.hpp"

namespace GTA
{
	private ref class ScriptDomain sealed : public System::MarshalByRefObject
	{
	public:
		ScriptDomain();

		static property ScriptDomain ^CurrentDomain
		{
			ScriptDomain ^get()
			{
				return sScriptDomain;
			}
		private:
			void set(ScriptDomain ^value)
			{
				sScriptDomain = value;
			}
		}

		static bool Reload();
		static void Unload();

		void StartScripts();
		void AbortScript(Script ^script);
		void AbortScripts();

	internal:
		void Run();
		void HandleException(System::Exception ^exception);
		System::IntPtr PinString(System::String ^string);
		bool IsKeyPressed(System::Windows::Forms::Keys key);

	private:
		bool LoadScript(System::String ^filename);
		bool LoadAssembly(System::String ^filename);
		bool LoadAssembly(System::String ^filename, System::Reflection::Assembly ^assembly);
		Script ^InstantiateScript(System::Type ^scripttype);

		static System::AppDomain ^sAppDomain;
		static ScriptDomain ^sScriptDomain;
		array<bool> ^mKeyboardState;
		System::Collections::Generic::List<System::IntPtr> ^mPinnedStrings;
		System::Collections::Generic::List<Script ^> ^mRunningScripts;
		System::Collections::Generic::Dictionary<System::String ^, System::Type ^> ^mScriptTypes;
	};
}