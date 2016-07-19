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

#include "Native.hpp"
#include "ScriptDomain.hpp"
#include "Vector2.hpp"
#include "Vector3.hpp"

#include <NativeCaller.h>

using namespace System;

namespace GTA
{
	namespace Native
	{
		#pragma region Functions
		private ref struct NativeTask : IScriptTask
		{
			virtual void Run()
			{
				nativeInit(_hash);

				for each (auto argument in _arguments)
				{
					nativePush64(argument->_data);
				}

				_result = nativeCall();
			}

			UInt64 _hash, *_result;
			array<InputArgument ^> ^_arguments;
		};

		UInt64 ObjectToNative(Object ^value)
		{
			if (Object::ReferenceEquals(value, nullptr))
			{
				return 0;
			}

			auto type = value->GetType();

			if (type->IsEnum)
			{
				value = Convert::ChangeType(value, type = Enum::GetUnderlyingType(type));
			}

			if (type == Boolean::typeid)
			{
				return static_cast<bool>(value) ? 1 : 0;
			}
			if (type == Int32::typeid)
			{
				return static_cast<Int32>(value);
			}
			if (type == UInt32::typeid)
			{
				return static_cast<UInt32>(value);
			}
			if (type == Int64::typeid)
			{
				return static_cast<Int64>(value);
			}
			if (type == UInt64::typeid)
			{
				return static_cast<UInt64>(value);
			}
			if (type == Single::typeid)
			{
				return BitConverter::ToUInt32(BitConverter::GetBytes(static_cast<float>(value)), 0);
			}
			if (type == Double::typeid)
			{
				return BitConverter::ToUInt32(BitConverter::GetBytes(static_cast<float>(static_cast<double>(value))), 0);
			}

			if (type == String::typeid)
			{
				return ScriptDomain::CurrentDomain->PinString(static_cast<String ^>(value)).ToInt64();
			}

			if (type == IntPtr::typeid)
			{
				return static_cast<IntPtr>(value).ToInt64();
			}

			if (INativeValue::typeid->IsAssignableFrom(type))
			{
				return static_cast<INativeValue ^>(value)->NativeValue;
			}

			throw gcnew InvalidCastException(String::Concat("Unable to cast object of type '", type->FullName, "' to native value"));
		}
		Object ^ObjectFromNative(Type ^type, UInt64 *value)
		{
			if (type->IsEnum)
			{
				type = Enum::GetUnderlyingType(type);
			}

			if (type == Boolean::typeid)
			{
				return *reinterpret_cast<const int *>(value) != 0;
			}
			if (type == Int32::typeid)
			{
				return *reinterpret_cast<const Int32 *>(value);
			}
			if (type == UInt32::typeid)
			{
				return *reinterpret_cast<const UInt32 *>(value);
			}
			if (type == Int64::typeid)
			{
				return *reinterpret_cast<const Int64 *>(value);
			}
			if (type == UInt64::typeid)
			{
				return *reinterpret_cast<const UInt64 *>(value);
			}
			if (type == Single::typeid)
			{
				return *reinterpret_cast<const float *>(value);
			}
			if (type == Double::typeid)
			{
				return static_cast<double>(*reinterpret_cast<const float *>(value));
			}

			if (type == String::typeid)
			{
				const auto address = reinterpret_cast<char *>(*value);

				if (address == nullptr)
				{
					return String::Empty;
				}

				const auto size = static_cast<int>(strlen(address));

				if (size == 0)
				{
					return String::Empty;
				}

				const auto bytes = gcnew array<Byte>(size);
				Runtime::InteropServices::Marshal::Copy(IntPtr(address), bytes, 0, bytes->Length);

				return Text::Encoding::UTF8->GetString(bytes);
			}

			if (type == IntPtr::typeid)
			{
				return IntPtr(*reinterpret_cast<const Int64 *>(value));
			}

			if (type == Math::Vector2::typeid)
			{
				const auto data = reinterpret_cast<const float *>(value);

				return gcnew Math::Vector2(data[0], data[2]);
			}
			if (type == Math::Vector3::typeid)
			{
				const auto data = reinterpret_cast<const float *>(value);

				return gcnew Math::Vector3(data[0], data[2], data[4]);
			}

			if (INativeValue::typeid->IsAssignableFrom(type))
			{
				// Warning: Requires classes implementing 'INativeValue' to repeat all constructor work in the setter of 'NativeValue'
				auto result = static_cast<INativeValue ^>(Runtime::Serialization::FormatterServices::GetUninitializedObject(type));
				result->NativeValue = *value;

				return result;
			}

			throw gcnew InvalidCastException(String::Concat("Unable to cast native value to object of type '", type->FullName, "'"));
		}

		InputArgument::InputArgument(Object ^value) : _data(ObjectToNative(value))
		{
		}
		OutputArgument::OutputArgument() : _storage(new unsigned char[24]()), InputArgument(IntPtr(_storage))
		{
		}
		OutputArgument::OutputArgument(Object ^value) : OutputArgument()
		{
			*reinterpret_cast<UInt64 *>(_storage) = ObjectToNative(value);
		}
		OutputArgument::~OutputArgument()
		{
			this->!OutputArgument();
		}
		OutputArgument::!OutputArgument()
		{
			delete[] _storage;
		}

		generic <typename T>
		T OutputArgument::GetResult()
		{
			return static_cast<T>(ObjectFromNative(T::typeid, reinterpret_cast<UINT64 *>(_data)));
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
			const auto task = gcnew NativeTask();
			task->_hash = hash;
			task->_arguments = arguments;

			ScriptDomain::CurrentDomain->ExecuteTask(task);

			return static_cast<T>(ObjectFromNative(T::typeid, task->_result));
		}
		#pragma endregion

		#pragma region Global Variables
		GlobalVariable GlobalVariable::Get(int index)
		{
			IntPtr address(getGlobalPtr(index));

			if (address == IntPtr::Zero)
			{
				throw gcnew IndexOutOfRangeException(String::Format("The index {0} does not correspond to an existing global variable.", index));
			}

			return GlobalVariable(address);
		}

		GlobalVariable::GlobalVariable(IntPtr address) : _address(address)
		{
		}

		generic <typename T>
		T GlobalVariable::Read()
		{
			if (T::typeid == String::typeid)
			{
				const auto size = static_cast<int>(strlen(static_cast<char *>(_address.ToPointer())));

				if (size == 0)
				{
					return reinterpret_cast<T>(String::Empty);
				}

				const auto bytes = gcnew array<Byte>(size);
				Runtime::InteropServices::Marshal::Copy(_address, bytes, 0, bytes->Length);

				return reinterpret_cast<T>(Text::Encoding::UTF8->GetString(bytes));
			}

			return static_cast<T>(ObjectFromNative(T::typeid, static_cast<UInt64 *>(_address.ToPointer())));
		}
		generic <typename T>
		void GlobalVariable::Write(T value)
		{
			if (T::typeid == String::typeid)
			{
				throw gcnew InvalidOperationException("Cannot write string values via 'Write<string>', use 'WriteString' instead.");
			}

			if (T::typeid == Math::Vector2::typeid)
			{
				const auto val = static_cast<Math::Vector2>(value);
				const auto data = static_cast<float *>(_address.ToPointer());

				data[0] = val.X;
				data[2] = val.Y;
				return;
			}
			if (T::typeid == Math::Vector3::typeid)
			{
				const auto val = static_cast<Math::Vector3>(value);
				const auto data = static_cast<float *>(_address.ToPointer());

				data[0] = val.X;
				data[2] = val.Y;
				data[4] = val.Z;
				return;
			}

			*static_cast<UInt64 *>(_address.ToPointer()) = ObjectToNative(value);
		}
		void GlobalVariable::WriteString(String ^value, int maxSize)
		{
			if (maxSize % 8 != 0 || maxSize <= 0 || maxSize > 64)
			{
				throw gcnew ArgumentException("The string maximum size should be one of 8, 16, 24, 32 or 64.", "maxSize");
			}

			auto size = Text::Encoding::UTF8->GetByteCount(value);

			if (size >= maxSize)
			{
				size = maxSize - 1;
			}

			Runtime::InteropServices::Marshal::Copy(Text::Encoding::UTF8->GetBytes(value), 0, _address, size);
			static_cast<char *>(_address.ToPointer())[size] = '\0';
		}

		void GlobalVariable::SetBit(int index)
		{
			if (index < 0 || index > 63)
			{
				throw gcnew IndexOutOfRangeException("The bit index has to be between 0 and 63");
			}

			*static_cast<UInt64 *>(_address.ToPointer()) |= (1ull << index);
		}
		void GlobalVariable::ClearBit(int index)
		{
			if (index < 0 || index > 63)
			{
				throw gcnew IndexOutOfRangeException("The bit index has to be between 0 and 63");
			}

			*static_cast<UInt64 *>(_address.ToPointer()) &= ~(1ull << index);
		}
		bool GlobalVariable::IsBitSet(int index)
		{
			if (index < 0 || index > 63)
			{
				throw gcnew IndexOutOfRangeException("The bit index has to be between 0 and 63");
			}

			return ((*static_cast<UInt64 *>(_address.ToPointer()) >> index) & 1) != 0;
		}

		GlobalVariable GlobalVariable::GetStructField(int index)
		{
			if (index < 0)
			{
				throw gcnew IndexOutOfRangeException("The structure item index cannot be negative.");
			}

			return GlobalVariable(MemoryAddress + (8 * index));
		}

		array<GlobalVariable> ^GlobalVariable::GetArray(int itemSize)
		{
			if (itemSize <= 0)
			{
				throw gcnew ArgumentOutOfRangeException("itemSize", "The item size for an array must be positive.");
			}

			int count = Read<int>();

			// Globals are stored in pages that hold a maximum of 65536 items
			if (count < 1 || count >= 65536 / itemSize)
			{
				throw gcnew InvalidOperationException("The variable does not seem to be an array.");
			}

			auto result = gcnew array<GlobalVariable>(count);

			for (int i = 0; i < count; i++)
			{
				result[i] = GlobalVariable(MemoryAddress + 8 + (8 * itemSize * i));
			}

			return result;
		}
		GlobalVariable GlobalVariable::GetArrayItem(int index, int itemSize)
		{
			if (itemSize <= 0)
			{
				throw gcnew ArgumentOutOfRangeException("itemSize", "The item size for an array must be positive.");
			}

			int count = Read<int>();

			// Globals are stored in pages that hold a maximum of 65536 items
			if (count < 1 || count >= 65536 / itemSize)
			{
				throw gcnew InvalidOperationException("The variable does not seem to be an array.");
			}

			if (index < 0 || index >= count)
			{
				throw gcnew IndexOutOfRangeException(String::Format("The index {0} was outside the array bounds.", index));
			}

			return GlobalVariable(MemoryAddress + 8 + (8 * itemSize * index));
		}
		#pragma endregion
	}
}
