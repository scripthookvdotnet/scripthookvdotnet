#pragma once

#include "Script.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	private ref class ScriptDomain sealed : public MarshalByRefObject
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
		void HandleException(Exception ^exception);
		IntPtr PinString(String ^string);
		bool IsKeyPressed(Windows::Forms::Keys key);

	private:
		bool LoadScript(String ^filename);
		bool LoadAssembly(String ^filename);
		bool LoadAssembly(String ^filename, Reflection::Assembly ^assembly);
		Script ^InstantiateScript(Type ^scripttype);

		static AppDomain ^sAppDomain;
		static ScriptDomain ^sScriptDomain;
		array<bool> ^mKeyboardState;
		List<IntPtr> ^mPinnedStrings;
		List<Script ^> ^mRunningScripts;
		Dictionary<String ^, Type ^> ^mScriptTypes;
	};
}