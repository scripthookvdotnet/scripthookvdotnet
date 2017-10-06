#include "EuphoriaBase.hpp"
#include "NativeMemory.hpp"
#include "Native.hpp"
#include "Ped.hpp"
#include "ScriptDomain.hpp"
#include "Game.hpp"

namespace GTA
{
	using namespace System;
	using namespace Collections::Generic;

	extern void Log(String ^logLevel, ... array<String ^> ^message);

	namespace NaturalMotion
	{
		namespace
		{
			private ref struct NmApply : IScriptTask
			{
			public:
				NmApply(Message ^message, Ped ^target) : _message(message), _target(target)
				{
				}

				virtual void Run()
				{
					__int64 NativeFunc = Native::MemoryAccess::CreateNmMessageFunc;
					__int64 MessageAddress = reinterpret_cast<__int64(*)(__int64)>(*reinterpret_cast<int*>(NativeFunc + 0x22) + NativeFunc + 0x26)(4632);

					if (MessageAddress == 0)
					{
						return;
					}

					reinterpret_cast<__int64(*)(__int64, __int64, int)>(*reinterpret_cast<int*>(NativeFunc + 0x3C) + NativeFunc + 0x40)(MessageAddress, MessageAddress + 24, 64);

					if (!ReferenceEquals(_message->_argumentBools, nullptr))
					{
						for each (KeyValuePair<String ^, bool> ^argument in _message->_argumentBools)
						{
							reinterpret_cast<unsigned char(*)(__int64, __int64, unsigned char)>(Native::MemoryAccess::SetNmBoolAddress)(MessageAddress, ScriptDomain::CurrentDomain->PinString(argument->Key).ToInt64(), argument->Value ? 1 : 0);
						}
					}
					if (!ReferenceEquals(_message->_argumentInts, nullptr))
					{
						for each (KeyValuePair<String ^, int> ^argument in _message->_argumentInts)
						{
							reinterpret_cast<unsigned char(*)(__int64, __int64, int)>(Native::MemoryAccess::SetNmIntAddress)(MessageAddress, ScriptDomain::CurrentDomain->PinString(argument->Key).ToInt64(), argument->Value);
						}
					}
					if (!ReferenceEquals(_message->_argumentFloats, nullptr))
					{
						for each (KeyValuePair<String ^, float> ^argument in _message->_argumentFloats)
						{
							reinterpret_cast<unsigned char(*)(__int64, __int64, float)>(Native::MemoryAccess::SetNmFloatAddress)(MessageAddress, ScriptDomain::CurrentDomain->PinString(argument->Key).ToInt64(), argument->Value);
						}
					}
					if (!ReferenceEquals(_message->_argumentVectors, nullptr))
					{
						for each (KeyValuePair<String ^, Math::Vector3> ^argument in _message->_argumentVectors)
						{
							reinterpret_cast<unsigned char(*)(__int64, __int64, float, float, float)>(Native::MemoryAccess::SetNmVec3Address)(MessageAddress, ScriptDomain::CurrentDomain->PinString(argument->Key).ToInt64(), argument->Value.X, argument->Value.Y, argument->Value.Z);
						}
					}
					if (!ReferenceEquals(_message->_argumentStrings, nullptr))
					{
						for each (KeyValuePair<String ^, String ^> ^argument in _message->_argumentStrings)
						{
							reinterpret_cast<unsigned char(*)(__int64, __int64, __int64)>(Native::MemoryAccess::SetNmStringAddress)(MessageAddress, ScriptDomain::CurrentDomain->PinString(argument->Key).ToInt64(), ScriptDomain::CurrentDomain->PinString(argument->Value).ToInt64());
						}
					}

					__int64 BaseFunc = Native::MemoryAccess::GiveNmMessageFunc;
					__int64 ByteAddr = *reinterpret_cast<int*>(BaseFunc + 0xBC) + BaseFunc + 0xC0;
					__int64 UnkStrAddr = *reinterpret_cast<int*>(BaseFunc + 0xCE) + BaseFunc + 0xD2;
					__int64 _PedAddress = Native::MemoryAccess::GetAddressOfEntity(_target->Handle);
					__int64 PedNmAddress;
					bool v5 = false;
					__int8 v7;
					__int64 v11;
					__int64 v12;
					if (_PedAddress == 0)
						return;
					if (*reinterpret_cast<__int64*>(_PedAddress + 48) == 0)
						return;
					PedNmAddress = reinterpret_cast<__int64(*)(__int64)>(*reinterpret_cast<__int64*>(*reinterpret_cast<__int64*>(_PedAddress) + 88))(_PedAddress);

					int MinHealthOffset = (Game::Version < GameVersion::VER_1_0_877_1_STEAM ? *reinterpret_cast<int*>(BaseFunc + 78) : *reinterpret_cast<int*>(BaseFunc + 157 + *reinterpret_cast<int*>(BaseFunc + 76)));

					if (*reinterpret_cast<__int64*>(_PedAddress + 48) == PedNmAddress && *reinterpret_cast<float*>(_PedAddress + MinHealthOffset) <= *reinterpret_cast<float*>(_PedAddress + 640))
					{
						if ((*reinterpret_cast<int(**)(__int64)> (*reinterpret_cast<__int64*>(PedNmAddress)+152))(PedNmAddress) != -1)
						{
							__int64 PedIntelligenceAddr = *reinterpret_cast<__int64*>(_PedAddress + *reinterpret_cast<int*>(BaseFunc + 147));

							// check whether the ped is currently performing the 'CTaskNMScriptControl' task
							if (*(short *)(reinterpret_cast<__int64(*)(__int64)>(*reinterpret_cast<int*>(BaseFunc + 0xA2) + BaseFunc + 0xA6)(*(__int64 *)(PedIntelligenceAddr + 864)) + 52) == 401)
							{
								v5 = true;
							}
							else
							{
								v7 = *(__int8*)ByteAddr;
								if (v7)
								{
									reinterpret_cast<void(*)(__int64)>(*reinterpret_cast<int*>(BaseFunc + 0xD3) + BaseFunc + 0xD7)(UnkStrAddr);
									v7 = *(__int8*)ByteAddr;
								}
								int count = *reinterpret_cast<int*>(PedIntelligenceAddr + 1064);
								if (v7)
								{
									reinterpret_cast<void(*)(__int64)>(*reinterpret_cast<int*>(BaseFunc + 0xF0) + BaseFunc + 0xF4)(UnkStrAddr);
								}
								for (int i = 0; i < count; i++)
								{
									v11 = *reinterpret_cast<__int64*>(PedIntelligenceAddr + 8 * ((i + *reinterpret_cast<int*>(PedIntelligenceAddr + 1060) + 1) % 16) + 928);
									if (v11)
									{
										if ((*(int(__fastcall **)(__int64))(*reinterpret_cast<__int64*>(v11)+24))(v11) == 132)
										{
											v12 = *reinterpret_cast<__int64*>(v11 + 40);
											if (v12)
											{
												if (*reinterpret_cast<short*>(v12 + 52) == 401)
													v5 = true;
											}
										}
									}
								}
							}
							if (v5 && (*reinterpret_cast<int(**)(__int64)>(*reinterpret_cast<__int64*>(PedNmAddress)+152))(PedNmAddress) != -1)
							{
								reinterpret_cast<void(*)(__int64, __int64, __int64)>(*reinterpret_cast<int*>(BaseFunc + 0x1AA) + BaseFunc + 0x1AE)(PedNmAddress, ScriptDomain::CurrentDomain->PinString(_message->_message).ToInt64(), MessageAddress);//Send Message To Ped
							}
							reinterpret_cast<void(*)(__int64)>(*reinterpret_cast<int*>(BaseFunc + 0x1BB) + BaseFunc + 0x1BF)(MessageAddress);//Free Message Memory
						}
					}
				}

			private:
				Message ^_message;
				Ped ^_target;
			};
		}

		Message::Message(String ^message) : _message(message)
		{
		}

		void Message::Abort(Ped ^target)
		{
			ScriptDomain::CurrentDomain->ExecuteTask(gcnew NmApply(this, target));
		}
		void Message::SendTo(Ped ^target)
		{
			if (!target->IsRagdoll)
			{
				if (!target->CanRagdoll)
				{
					target->CanRagdoll = true;
				}

				Native::Function::Call(Native::Hash::SET_PED_TO_RAGDOLL, target->Handle, 10000, -1, 1, 1, 1, 0);
			}

			SetArgument("start", true);

			ScriptDomain::CurrentDomain->ExecuteTask(gcnew NmApply(this, target));
		}
		void Message::SendTo(Ped ^target, int duration)
		{
			if (!target->CanRagdoll)
			{
				target->CanRagdoll = true;
			}

			Native::Function::Call(Native::Hash::SET_PED_TO_RAGDOLL, target->Handle, 10000, duration, 1, 1, 1, 0);

			SendTo(target);
		}

		void Message::SetArgument(String ^message, bool value)
		{
			if (ReferenceEquals(_argumentBools, nullptr))
			{
				_argumentBools = gcnew Dictionary<String ^, bool>();
			}

			_argumentBools->default[message] = value;
		}
		void Message::SetArgument(String ^message, int value)
		{
			if (ReferenceEquals(_argumentInts, nullptr))
			{
				_argumentInts = gcnew Dictionary<String ^, int>();
			}

			_argumentInts->default[message] = value;
		}
		void Message::SetArgument(String ^message, float value)
		{
			if (ReferenceEquals(_argumentFloats, nullptr))
			{
				_argumentFloats = gcnew Dictionary<String ^, float>();
			}

			_argumentFloats->default[message] = value;
		}
		void Message::SetArgument(String ^message, double value)
		{
			SetArgument(message, static_cast<float>(value));
		}
		void Message::SetArgument(String ^message, String ^value)
		{
			if (ReferenceEquals(_argumentStrings, nullptr))
			{
				_argumentStrings = gcnew Dictionary<String ^, String ^>();
			}

			_argumentStrings->default[message] = value;
		}
		void Message::SetArgument(String ^message, Math::Vector3 value)
		{
			if (ReferenceEquals(_argumentVectors, nullptr))
			{
				_argumentVectors = gcnew Dictionary<String ^, Math::Vector3>();
			}

			_argumentVectors->default[message] = value;
		}
		void Message::ResetArguments()
		{
			_argumentBools = nullptr;
			_argumentInts = nullptr;
			_argumentFloats = nullptr;
			_argumentVectors = nullptr;
			_argumentStrings = nullptr;
		}
		
		String ^Message::ToString()
		{
			return _message;
		}

		CustomHelper::CustomHelper(Ped ^target, String ^message) : Message(message), _ped(target)
		{
		}

		void CustomHelper::Start()
		{
			Message::SendTo(_ped);
		}
		void CustomHelper::Start(int duration)
		{
			Message::SendTo(_ped, duration);
		}
		void CustomHelper::Stop()
		{
			Message::Abort(_ped);
		}
		void CustomHelper::Abort()
		{
			Message::Abort(_ped);
		}
	}
}