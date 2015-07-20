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

#include "Game.hpp"

namespace GTA
{
	ref class Viewport;
	ref class ScriptDomain;
	ref class ScriptSettings;

	using namespace System;

	public ref class Script abstract
	{
	public:
		Script();

		static void Wait(int ms);
		static void Yield();
		[ObsoleteAttribute("Script.IsKeyPressed is obsolete, please use Game.IsKeyPressed instead.")]
		static bool IsKeyPressed(Windows::Forms::Keys key)
		{
			return Game::IsKeyPressed(key);
		}

		event EventHandler ^Tick;
		event Windows::Forms::KeyEventHandler ^KeyUp;
		event Windows::Forms::KeyEventHandler ^KeyDown;

		Windows::Forms::Keys ActivateKey = Windows::Forms::Keys::NumPad5;
		Windows::Forms::Keys BackKey = Windows::Forms::Keys::NumPad0;
		Windows::Forms::Keys LeftKey = Windows::Forms::Keys::NumPad4;
		Windows::Forms::Keys RightKey = Windows::Forms::Keys::NumPad6;
		Windows::Forms::Keys UpKey = Windows::Forms::Keys::NumPad8;
		Windows::Forms::Keys DownKey = Windows::Forms::Keys::NumPad2;

		[AttributeUsage(AttributeTargets::Class, AllowMultiple = true)]
		ref class DependsOn : Attribute
		{
			public:
				DependsOn(...array<Type^>^ dependencies)
				{
					this->mDependencies = dependencies;
				};

				property array<Type^> ^Dependencies
				{
					array<Type^> ^get()
					{
						return this->mDependencies;
					}
				}
			private:
				array<Type^> ^mDependencies;
		};

		property String ^Name
		{
			String ^get()
			{
				return GetType()->FullName;
			}
		}
		property String ^Filename
		{
			String ^get()
			{
				return this->mFilename;
			}
		}
		property Viewport ^View
		{
			Viewport ^get();
		}
		property ScriptSettings ^Settings
		{
			ScriptSettings ^get();
		}

		void Abort();

		virtual String ^ToString() override
		{
			return Name;
		}

		virtual void HandleViewportInput(Object ^sender, Windows::Forms::KeyEventArgs ^e);

	protected:
		property int Interval
		{
			int get();
			void set(int value);
		}

	internal:
		~Script();

		void MainLoop();
		void UpdateViewport(Object ^Sender, EventArgs ^Args);

		int mInterval;
		bool mRunning;
		String ^mFilename;
		ScriptDomain ^mScriptDomain;
		Threading::Thread ^mThread;
		Threading::AutoResetEvent ^mWaitEvent;
		Threading::AutoResetEvent ^mContinueEvent;
		Collections::Concurrent::ConcurrentQueue<Tuple<bool, Windows::Forms::KeyEventArgs ^> ^> ^mKeyboardEvents;
		Viewport ^mViewport;
		ScriptSettings ^mSettings;
	};
}