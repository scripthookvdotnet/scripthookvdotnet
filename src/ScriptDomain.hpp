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