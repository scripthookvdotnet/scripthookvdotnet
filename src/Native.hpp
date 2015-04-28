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

#include "NativeHashes.hpp"

namespace GTA
{
	namespace Native
	{
		public ref class InputArgument
		{
		public:
			InputArgument(bool value);
			InputArgument(int value);
			InputArgument(float value);
			InputArgument(double value);
			InputArgument(System::IntPtr value);
			InputArgument(System::String ^value);

			static inline operator InputArgument ^ (bool value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^ (int value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^ (int *value)
			{
				return gcnew InputArgument(System::IntPtr(value));
			}
			static inline operator InputArgument ^ (float value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^ (float *value)
			{
				return gcnew InputArgument(System::IntPtr(value));
			}
			static inline operator InputArgument ^ (double value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^ (System::String ^value)
			{
				return gcnew InputArgument(value);
			}

			virtual System::String ^ToString() override
			{
				return this->mData.ToString();
			}

		internal:
			System::UInt64 mData;
		};
		public ref class OutputArgument : public InputArgument
		{
		public:
			OutputArgument();
			~OutputArgument()
			{
				this->!OutputArgument();
			}

			generic <typename T>
			T GetResult();

		private:
			!OutputArgument();

			unsigned char *mStorage;
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

		private:
			static System::Collections::Generic::Dictionary<System::String ^, System::UInt64> ^sAddresses = gcnew System::Collections::Generic::Dictionary<System::String ^, System::UInt64>();
		};
	}
}