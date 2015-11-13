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
	value class Model;
	#pragma endregion

	namespace Native
	{
		public ref class InputArgument
		{
		public:
			InputArgument(System::Object ^value);
			inline InputArgument(bool value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(int value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(float value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(double value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(System::String ^value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(Model value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(Blip ^value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(Camera ^value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(Entity ^value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(Ped ^value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(PedGroup ^value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(Player ^value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(Prop ^value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(Vehicle ^value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}
			inline InputArgument(Rope ^value) : InputArgument(static_cast<System::Object ^>(value))
			{
			}

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
				return gcnew InputArgument(gcnew System::String(value));
			}
			static inline operator InputArgument ^ (Model model)
			{
				return gcnew InputArgument(model);
			}
			static inline operator InputArgument ^ (Blip ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Camera ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Entity ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Ped ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (PedGroup ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Player ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Prop ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Vehicle ^object)
			{
				return gcnew InputArgument(object);
			}
			static inline operator InputArgument ^ (Rope ^object)
			{
				return gcnew InputArgument(object);
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
			OutputArgument(System::Object ^initvalue);
			inline OutputArgument(bool initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(int initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(float initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(double initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(System::String ^initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(Model initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(Blip ^initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(Camera ^initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(Entity ^initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(Ped ^initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(PedGroup ^initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(Player ^initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(Prop ^initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(Vehicle ^initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
			inline OutputArgument(Rope ^initvalue) : OutputArgument(static_cast<System::Object ^>(initvalue))
			{
			}
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

		public ref class Function abstract sealed
		{
		public:
			generic <typename T>
			static T Call(Hash hash, ... array<InputArgument ^> ^arguments);
			/*static void Call(Hash hash, ... array<int^> ^arguments);*/
			static void Call(Hash hash, ... array<InputArgument ^> ^arguments);

		internal:
			generic <typename T>
			static T Call(System::UInt64 hash, ... array<InputArgument ^> ^arguments);
		};
	}
}