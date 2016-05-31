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

#include "Model.hpp"
#include "NativeHashes.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Blip;
	ref class Camera;
	ref class Entity;
	ref class Ped;
	ref class PedGroup;
	ref class Player;
	ref class Prop;
	ref class Rope;
	ref class Vehicle;
	#pragma endregion

	namespace Native
	{
		public ref class InputArgument
		{
		public:
			InputArgument(System::Object ^value);

			static inline operator InputArgument ^(bool value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^(bool *value)
			{
				return gcnew InputArgument(System::IntPtr(value));
			}
			static inline operator InputArgument ^(char value)
			{
				return gcnew InputArgument(static_cast<int>(value));
			}
			static inline operator InputArgument ^(unsigned char value)
			{
				return gcnew InputArgument(static_cast<int>(value));
			}
			static inline operator InputArgument ^(short value)
			{
				return gcnew InputArgument(static_cast<int>(value));
			}
			static inline operator InputArgument ^(unsigned short value)
			{
				return gcnew InputArgument(static_cast<int>(value));
			}
			static inline operator InputArgument ^(int value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^(int *value)
			{
				return gcnew InputArgument(System::IntPtr(value));
			}
			static inline operator InputArgument ^(unsigned int value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^(unsigned int *value)
			{
				return gcnew InputArgument(System::IntPtr(value));
			}
			static inline operator InputArgument ^(float value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^(float *value)
			{
				return gcnew InputArgument(System::IntPtr(value));
			}
			static inline operator InputArgument ^(double value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^(System::String ^value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^(const char value[])
			{
				return gcnew InputArgument(gcnew System::String(value));
			}
			static inline operator InputArgument ^(Model model)
			{
				return gcnew InputArgument(model);
			}
			static inline operator InputArgument ^(Blip ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^(Camera ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^(Entity ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^(Ped ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^(PedGroup ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^(Player ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^(Prop ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^(Vehicle ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^(Rope ^object)
			{
				return gcnew InputArgument(object);
			}

			virtual System::String ^ToString() override
			{
				return _data.ToString();
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
