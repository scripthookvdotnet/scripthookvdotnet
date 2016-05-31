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

#include "Script.hpp"

using namespace System;
using namespace System::Threading;
using namespace System::Reflection;
using namespace System::Windows::Forms;
using namespace System::Collections::Generic;

namespace GTA
{
	private interface class IScriptTask
	{
		void Run();
	};
    
	private ref class ScriptDomain sealed : public MarshalByRefObject
	{
	public:
		ScriptDomain();
		~ScriptDomain();
		
		static property Script ^ExecutingScript
		{
			Script ^get()
			{
				if (Object::ReferenceEquals(sCurrentDomain, nullptr))
				{
					return nullptr;
				}

				return sCurrentDomain->_executingScript;
			}
		}
		static property ScriptDomain ^CurrentDomain
		{
			inline ScriptDomain ^get()
			{
				return sCurrentDomain;
			}
		}

		static ScriptDomain ^Load(String ^path);
		static void Unload(ScriptDomain ^%domain);

		property System::String ^Name
		{
			inline System::String ^get()
			{
				return _appDomain->FriendlyName;
			}
		}
		property System::AppDomain ^AppDomain
		{
			inline System::AppDomain ^get()
			{
				return _appDomain;
			}
		}

		void Start();
		void Abort();
		static void AbortScript(Script ^script);
		void DoTick();
		void DoKeyboardMessage(Keys key, bool status, bool statusCtrl, bool statusShift, bool statusAlt);

		void PauseKeyboardEvents(bool pause);
		void ExecuteTask(IScriptTask ^task);
		IntPtr PinString(String ^string);
		inline bool IsKeyPressed(Keys key)
		{
			return _keyboardState[static_cast<int>(key)];
		}
		inline System::String ^LookupScriptFilename(Script ^script)
		{
			return LookupScriptFilename(script->GetType());
		}
		String ^LookupScriptFilename(Type ^scripttype);
		Object ^InitializeLifetimeService() override;

	private:
		bool LoadScript(String ^filename);
        bool LoadAssembly(String ^filename);
		bool LoadAssembly(String ^filename, Assembly ^assembly);
		Script ^InstantiateScript(Type ^scripttype);
		void CleanupStrings();

		static ScriptDomain ^sCurrentDomain;

		System::AppDomain ^_appDomain;
		int _executingThreadId;
		Script ^_executingScript;

		List<Script ^> ^_runningScripts = gcnew List<Script ^>();
        System::Collections::Generic::Queue<IScriptTask ^> ^_taskQueue = gcnew System::Collections::Generic::Queue<IScriptTask ^>();
		List<IntPtr> ^_pinnedStrings = gcnew List<IntPtr>();
		List<Tuple<String ^, Type ^> ^> ^_scriptTypes = gcnew List<Tuple<String ^, Type ^> ^>();
		bool _recordKeyboardEvents = true;
		array<bool> ^_keyboardState = gcnew array<bool>(256);

		static initonly array<String ^> ^_referenceAssemblies = gcnew array<String ^> {
			"System.dll",
			"System.Core.dll",
			"System.Drawing.dll",
			"System.Windows.Forms.dll",
			"System.XML.dll",
			"System.XML.Linq.dll",
		};
	};
}
