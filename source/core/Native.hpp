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

#include "NativeHashes.hpp"
#include "PoolObject.hpp"

namespace GTA
{
	namespace Native
	{
		public interface class INativeValue
		{
		public:
			property System::UInt64 NativeValue
			{
				System::UInt64 get();
			}
		};
		public ref class InputArgument
		{
		public:
			InputArgument(System::Object ^value);

			virtual System::String ^ToString() override
			{
				return _data.ToString();
			}

			// Value Types
			static operator InputArgument ^ (bool value)
			{
				return gcnew InputArgument(value);
			}
			static operator InputArgument ^ (char value)
			{
				return gcnew InputArgument(static_cast<int>(value));
			}
			static operator InputArgument ^ (unsigned char value)
			{
				return gcnew InputArgument(static_cast<int>(value));
			}
			static operator InputArgument ^ (short value)
			{
				return gcnew InputArgument(static_cast<int>(value));
			}
			static operator InputArgument ^ (unsigned short value)
			{
				return gcnew InputArgument(static_cast<int>(value));
			}
			static operator InputArgument ^ (int value)
			{
				return gcnew InputArgument(value);
			}
			static operator InputArgument ^ (unsigned int value)
			{
				return gcnew InputArgument(value);
			}
			static operator InputArgument ^ (float value)
			{
				return gcnew InputArgument(value);
			}
			static operator InputArgument ^ (double value)
			{
				return gcnew InputArgument(value);
			}

			// String Types
			static operator InputArgument ^ (System::String ^value)
			{
				return gcnew InputArgument(value);
			}
			static operator InputArgument ^ (const char *value)
			{
				return gcnew InputArgument(gcnew System::String(value));
			}

			// Pointer Types
			static operator InputArgument ^ (System::IntPtr value)
			{
				return gcnew InputArgument(value);
			}
			static operator InputArgument ^ (bool *value)
			{
				return gcnew InputArgument(System::IntPtr(value));
			}
			static operator InputArgument ^ (int *value)
			{
				return gcnew InputArgument(System::IntPtr(value));
			}
			static operator InputArgument ^ (unsigned int *value)
			{
				return gcnew InputArgument(System::IntPtr(value));
			}
			static operator InputArgument ^ (float *value)
			{
				return gcnew InputArgument(System::IntPtr(value));
			}

			//INativeValue
			static operator InputArgument ^ (INativeValue ^value)
			{
				return gcnew InputArgument(value->NativeValue);
			}
			static operator InputArgument ^ (GTA::PoolObject ^value)
			{
				return gcnew InputArgument(value->Handle);
			}
			static operator InputArgument ^ (System::Enum^ value)
			{
				return gcnew InputArgument(value);
			}

		internal:
			System::UInt64 _data;
		};
		public ref class OutputArgument : public InputArgument
		{
		public:
			OutputArgument();
			OutputArgument(System::Object ^initvalue);
			~OutputArgument();

			generic <typename T>
			T GetResult();

		protected:
			!OutputArgument();

			unsigned char *_storage;
		};

		public ref class Function abstract sealed
		{
		public:
			generic <typename T>
			static T Call(Hash hash, ... array<InputArgument ^> ^arguments);
			static void Call(Hash hash, ... array<InputArgument ^> ^arguments);

		internal:
			generic <typename T>
			static T Call(System::UInt64 hash, ... array<InputArgument ^> ^arguments);
		};
	}
}
