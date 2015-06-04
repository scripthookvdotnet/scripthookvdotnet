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
#include "Model.hpp"

namespace GTA
{
	ref class Entity;
	ref class Ped;
	ref class Player;
	ref class Vehicle;
	ref class Blip;
	ref class Prop;

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
			InputArgument(const char value[]);
			InputArgument(Entity ^object);
			InputArgument(Ped ^object);
			InputArgument(Player ^object);
			InputArgument(Vehicle ^object);
			InputArgument(Blip ^object);
			InputArgument(Prop ^object);
			InputArgument(Model model);

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
			static inline operator InputArgument ^ (const char value[])
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^ (Entity ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Ped ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Player ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Vehicle ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Blip ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Prop ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Model model)
			{
				return gcnew InputArgument(model);
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

		protected:
			!OutputArgument();

			unsigned char *mStorage;
		};

		public ref class InOutArgument : public OutputArgument
		{
		public:

			InOutArgument(bool value);
			InOutArgument(int value);
			InOutArgument(float value);
			InOutArgument(double value);
			InOutArgument(Entity ^object);
			InOutArgument(Ped ^object);
			InOutArgument(Player ^object);
			InOutArgument(Vehicle ^object);
			InOutArgument(Blip ^object);
			InOutArgument(Prop ^object);
			InOutArgument(Model model);

			~InOutArgument()
			{
				this->!InOutArgument();
			}

		private:
			!InOutArgument();


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