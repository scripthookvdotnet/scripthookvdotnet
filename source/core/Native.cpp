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
#include "NativeMemory.hpp"

#include <NativeCaller.h>

using namespace System;
using namespace System::Collections::Generic;

namespace GTA
{
	namespace Native
	{
		namespace
		{
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
					if (*value != '\0')
					{
						const auto size = static_cast<int>(strlen(reinterpret_cast<const char *>(*value)));
						const auto bytes = gcnew array<Byte>(size);

						Runtime::InteropServices::Marshal::Copy(static_cast<IntPtr>(static_cast<Int64>(*value)), bytes, 0, size);

						return Text::Encoding::UTF8->GetString(bytes);
					}
					else
					{
						return String::Empty;
					}
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

		GlobalVariable::GlobalVariable(IntPtr memoryAddress) : _address(memoryAddress)
		{
		}
		GlobalVariable GlobalVariable::Get(int index)
		{
			IntPtr address = IntPtr(getGlobalPtr(index));
			if (address == IntPtr::Zero)
			{
				throw gcnew IndexOutOfRangeException(String::Format("The global index {0} is outside the range of allowed global indexes", index));
			}
			return GlobalVariable(address);
		}

		generic <typename T>
		T GlobalVariable::Read()
		{
			Type ^type = T::typeid;
			if (type == String::typeid)
			{
				char* address = (char*)_address.ToPointer();
				const auto size = static_cast<int>(strlen(address));
				if (size == 0)
				{
					return static_cast<T>(static_cast<Object^>(String::Empty));
				}
				const auto bytes = gcnew array<Byte>(size);

				Runtime::InteropServices::Marshal::Copy(static_cast<IntPtr>(_address), bytes, 0, size);

				return static_cast<T>(static_cast<Object^>(Text::Encoding::UTF8->GetString(bytes)));
			}
			else
			{
				return static_cast<T>(ObjectFromNative(T::typeid, static_cast<unsigned long long*>(_address.ToPointer())));
			}
		}

		generic <typename T>
		void GlobalVariable::Write(T value)
		{
			Type ^type = T::typeid;
			if (type == String::typeid)
			{
				MemoryAccess::WriteString(_address, reinterpret_cast<String ^>(value));
			}
			else if (type == Math::Vector3::typeid)
			{
				MemoryAccess::WritePaddedVector3(_address, static_cast<Math::Vector3>(value));
			}
			else
			{
				*static_cast<unsigned long long*>(_address.ToPointer()) = ObjectToNative(value);
			}
		}

		GlobalVariable GlobalVariable::GetArrayItem(int index, int itemSize)
		{
			int maxIndex = Read<int>();
			if (index < 0 || index >= maxIndex)
			{
				throw gcnew IndexOutOfRangeException(String::Format("The array index {0} was outside the bounds of the array size"));
			}
			if (itemSize <= 0)
			{
				throw gcnew ArgumentOutOfRangeException("itemSize", "The item size for an array must be a positive number");
			}
			return GlobalVariable(IntPtr(MemoryAddress + 8 + (8 * itemSize*index)));
		}
		GlobalVariable GlobalVariable::GetStructField(int index)
		{
			if (index < 0)
			{
				throw gcnew IndexOutOfRangeException(String::Format("The struct item index cannot be negative"));
			}
			return GlobalVariable(IntPtr(MemoryAddress + (8 * index)));
		}
		IntPtr GlobalVariable::MemoryAddress::get()
		{
			return _address;
		}
	}
}
