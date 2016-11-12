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
	#pragma region Forward Declarations
	ref class ScriptDomain;
	ref class ScriptSettings;
	#pragma endregion

	[System::AttributeUsage(System::AttributeTargets::Class, AllowMultiple = true)]
	public ref class RequireScript : System::Attribute
	{
	public:
		RequireScript(System::Type ^dependency) : _dependency(dependency) { }

	internal:
		System::Type ^_dependency;
	};
	[System::AttributeUsage(System::AttributeTargets::Class, AllowMultiple = false)]
	public ref class ScriptAttributes : System::Attribute
	{
	public:
		ScriptAttributes() { }

		property System::String ^Author;
		property System::String ^SupportURL;
	};

	/// <summary>
	/// A base class for all user scripts to inherit.
	/// The Hook will only detect and starts Scripts that inherit directly from this class and have a default(parameterless) public constructor.
	/// </summary>
	public ref class Script abstract
	{
	public:
		Script();

		/// <summary>
		/// Pauses execution of the script for a specific amount of time.
		/// Must be called inside the main script loop - The OnTick or any sub methods of it.
		/// </summary>
		/// <param name="ms">The time in ms to pause for</param>
		static void Wait(int ms);
		/// <summary>
		/// Yields the execution of the script for 1 frame.
		/// </summary>
		static void Yield();

		/// <summary>
		/// An event that is raised every tick of the script. 
		/// Put code that needs to be looped each frame in here.
		/// </summary>
		event System::EventHandler ^Tick;
		/// <summary>
		/// An event that is raised when a key is lifted.
		/// The <see cref="System::Windows::Forms::KeyEventArgs"/> contains the key that was lifted.
		/// </summary>
		event System::Windows::Forms::KeyEventHandler ^KeyUp;
		/// <summary>
		/// An event that is raised when a key is first pressed.
		/// The <see cref="System::Windows::Forms::KeyEventArgs"/> contains the key that was pressed.
		/// </summary>
		event System::Windows::Forms::KeyEventHandler ^KeyDown;
		/// <summary>
		/// An event that is raised when this script gets aborted for any reason.
		/// This should be used for cleaning up anything created during this script
		/// </summary>
		event System::EventHandler ^Aborted;

		/// <summary>
		/// Gets the name of this <see cref="Script"/>.
		/// </summary>
		property System::String ^Name
		{
			System::String ^get()
			{
				return GetType()->FullName;
			}
		}
		/// <summary>
		/// Gets the filename of this <see cref="Script"/>.
		/// </summary>
		property System::String ^Filename
		{
			System::String ^get()
			{
				return _filename;
			}
		}

		/// <summary>
		/// Gets an ini file associated with this <see cref="Script"/>.
		/// The File will be in the same location as this <see cref="Script"/> but with an extension of .ini.
		/// Use this to save and load settings for this <see cref="Script"/>.
		/// </summary>
		property ScriptSettings ^Settings
		{
			ScriptSettings ^get();
		}

		/// <summary>
		/// Gets the Directory where this <see cref="Script"/> is stored.
		/// </summary>
		property System::String ^BaseDirectory
		{
			System::String ^get()
			{
				return System::IO::Path::GetDirectoryName(_filename);
			}
		}
		/// <summary>
		/// Gets the full file path for a file relative to this <see cref="Script"/>.
		/// e.g: GetRelativeFilePath("ScriptFiles\texture1.png") may return "C:\Program Files\Rockstar Games\Grand Theft Auto V\scripts\ScriptFiles\texture1.png"
		/// </summary>
		/// <param name="filePath">The file path relative to the location of this <see cref="Script"/>.</param>
		System::String ^GetRelativeFilePath(System::String ^filePath);

		/// <summary>
		/// Stops execution of this <see cref="Script"/> indefinitely.
		/// </summary>
		void Abort();

		/// <summary>
		/// Returns a string that represents this <see cref="Script"/>.
		/// </summary>
		virtual System::String ^ToString() override
		{
			return Name;
		}

	protected:
		/// <summary>
		/// Gets or sets the interval in ms between <see cref="Tick"/> for this <see cref="Script"/>.
		/// Default value is 0 meaning the event will execute once each frame.
		/// </summary>
		property int Interval
		{
			int get();
			void set(int value);
		}

	internal:
		void Start();
		void MainLoop();

		int _interval = 0;
		bool _running = false;
		System::String ^_filename;
		ScriptDomain ^_scriptdomain;
		System::Threading::Thread ^_thread;
		System::Threading::AutoResetEvent ^_waitEvent = gcnew System::Threading::AutoResetEvent(false);
		System::Threading::AutoResetEvent ^_continueEvent = gcnew System::Threading::AutoResetEvent(false);
		System::Collections::Concurrent::ConcurrentQueue<System::Tuple<bool, System::Windows::Forms::KeyEventArgs ^> ^> ^_keyboardEvents = gcnew System::Collections::Concurrent::ConcurrentQueue<System::Tuple<bool, System::Windows::Forms::KeyEventArgs ^> ^>();
		ScriptSettings ^_settings;
	};
}