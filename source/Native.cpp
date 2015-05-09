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

#include "Native.hpp"
#include "NativeCaller.h"
#include "ScriptDomain.hpp"

#include "Entity.hpp"
#include "Prop.hpp"
#include "Blip.hpp"
#include "Ped.hpp"
#include "Player.hpp"
#include "Vector2.hpp"
#include "Vector3.hpp"
#include "Vehicle.hpp"

namespace GTA
{
	namespace Native
	{
		using namespace System;
		using namespace System::Collections::Generic;

		InputArgument::InputArgument(bool value) : mData(value ? 1 : 0)
		{
		}
		InputArgument::InputArgument(int value) : mData(value)
		{
		}
		InputArgument::InputArgument(float value) : mData(BitConverter::ToUInt32(BitConverter::GetBytes(value), 0))
		{
		}
		InputArgument::InputArgument(double value) : mData(BitConverter::ToUInt32(BitConverter::GetBytes(static_cast<float>(value)), 0))
		{
		}
		InputArgument::InputArgument(IntPtr value) : mData(value.ToInt64())
		{
		}
		InputArgument::InputArgument(String ^value) : mData(ScriptDomain::CurrentDomain->PinString(value).ToInt64())
		{
		}
		InputArgument::InputArgument(const char value[]) : mData(ScriptDomain::CurrentDomain->PinString(gcnew String(value)).ToInt64())
		{
		}
		InputArgument::InputArgument(Entity ^object) : mData(object->ID)
		{
		}
		InputArgument::InputArgument(Ped ^object) : mData(object->ID)
		{
		}
		InputArgument::InputArgument(Player ^object) : mData(object->ID)
		{
		}
		InputArgument::InputArgument(Vehicle ^object) : mData(object->ID)
		{
		}
		InputArgument::InputArgument(Blip ^object) : mData(object->ID)
		{
		}
		InputArgument::InputArgument(Prop ^object) : mData(object->ID)
		{
		}
		OutputArgument::OutputArgument() : mStorage(new unsigned char[16]()), InputArgument(IntPtr(this->mStorage))
		{
		}
		OutputArgument::!OutputArgument()
		{
			delete[] this->mStorage;
		}

		Object ^GetResult(Type ^type, PUINT64 value)
		{
			if (type == Boolean::typeid)
			{
				return *reinterpret_cast<int *>(value) != 0;
			}
			if (type == Int32::typeid || type == UInt32::typeid)
			{
				return *reinterpret_cast<int *>(value);
			}
			if (type == Single::typeid)
			{
				return *reinterpret_cast<float *>(value);
			}
			if (type == Double::typeid)
			{
				return static_cast<double>(*reinterpret_cast<float *>(value));
			}
			if (type == String::typeid)
			{
				if (*value != 0)
				{
					return gcnew String(reinterpret_cast<const char *>(*value));
				}
				else
				{
					return String::Empty;
				}
			}

			#pragma pack(push, 1)
			struct NativeVector3
			{
				float x;
				DWORD _paddingx;
				float y;
				DWORD _paddingy;
				float z;
				DWORD _paddingz;
			};
			#pragma pack(pop)

			if (type == Math::Vector2::typeid)
			{
				return gcnew Math::Vector2(reinterpret_cast<NativeVector3 *>(value)->x, reinterpret_cast<NativeVector3 *>(value)->y);
			}
			if (type == Math::Vector3::typeid)
			{
				return gcnew Math::Vector3(reinterpret_cast<NativeVector3 *>(value)->x, reinterpret_cast<NativeVector3 *>(value)->y, reinterpret_cast<NativeVector3 *>(value)->z);
			}

			if (type == Ped::typeid)
			{
				return gcnew Ped(*reinterpret_cast<int *>(value));
			}
			if (type == Player::typeid)
			{
				return gcnew Player(*reinterpret_cast<int *>(value));
			}
			if (type == Vehicle::typeid)
			{
				return gcnew Vehicle(*reinterpret_cast<int *>(value));
			}
			if (type == Blip::typeid)
			{
				return gcnew Blip(*reinterpret_cast<int *>(value));
			}
			if (type == Prop::typeid)
			{
				return gcnew Prop(*reinterpret_cast<int *>(value));
			}

			throw gcnew InvalidCastException(String::Concat("Unable to cast native value to object of type '", type->FullName, "'"));
		}
		generic <typename T>
		T OutputArgument::GetResult()
		{
			return static_cast<T>(Native::GetResult(T::typeid, reinterpret_cast<PUINT64>(this->mData)));
		}

		generic <typename T>
		T Function::Call(Hash hash, ... array<InputArgument ^> ^arguments)
		{
			return Call<T>(static_cast<UInt64>(hash), arguments);
		}
		void Function::Call(Hash hash, ... array<InputArgument ^> ^arguments)
		{
			Call<int>(static_cast<UInt64>(hash), arguments);
		}
		generic <typename T>
		T Function::Call(UInt64 hash, ... array<InputArgument ^> ^arguments)
		{
			if (hash == 0)
			{
				throw gcnew ArgumentException("Missing hash to native function call.", "hash");
			}

			nativeInit(hash);

			for each (InputArgument ^argument in arguments)
			{
				nativePush64(argument->mData);
			}

			const PUINT64 result = nativeCall();

			return static_cast<T>(GetResult(T::typeid, result));
		}
	}
}