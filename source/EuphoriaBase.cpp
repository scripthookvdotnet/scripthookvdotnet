#include "EuphoriaBase.hpp"
#include "NativeMemory.hpp"
#include "Native.hpp"
#include "Ped.hpp"
#include "ScriptDomain.hpp"

namespace GTA
{
	namespace NaturalMotion
	{
		using namespace System;
		using namespace Collections::Generic;

		namespace
		{
			private ref struct NmApply : IScriptTask
			{
			public:
				NmApply(BaseMessage ^message, Ped ^target) : _message(message), _target(target)
				{
				}

				virtual void Run()
				{
#define ISNULL(x) ReferenceEquals(x, nullptr)
#define NULLDICT(x) x->Clear(); x=nullptr
#define NULLDICTCHECK(x) if (!ISNULL(x)) {NULLDICT(x);} else {}

					__int64 NativeFunc = Native::MemoryAccess::CreateNmMessageFunc;
					__int64 MessageAddress = reinterpret_cast<__int64(*)(__int64)>(*reinterpret_cast<int*>(NativeFunc + 0x22) + NativeFunc + 0x26)(4632);
					if (MessageAddress)
					{
						reinterpret_cast<__int64(*)(__int64, __int64, int)>(*reinterpret_cast<int*>(NativeFunc + 0x3C) + NativeFunc + 0x40)(MessageAddress, MessageAddress + 24, 64);
	
						if (!ISNULL(_message->lBool))
						{
							if (_message->lBool->ContainsKey("start"))
							{
								if (_message->lBool["start"])//The start argument initialises to false, so no need to manually set it false
								{
									reinterpret_cast<unsigned char(*)(__int64, __int64, unsigned char)>(Native::MemoryAccess::SetNmBoolAddress)(MessageAddress, ScriptDomain::CurrentDomain->PinString("start").ToInt64(), 1);
								}
								_message->lBool->Remove("start");
							}
							else
							{
								reinterpret_cast<unsigned char(*)(__int64, __int64, unsigned char)>(Native::MemoryAccess::SetNmBoolAddress)(MessageAddress, ScriptDomain::CurrentDomain->PinString("start").ToInt64(), 1);
							}
							for each(KeyValuePair<String^, bool> ^Arg in _message->lBool)
							{
								reinterpret_cast<unsigned char(*)(__int64, __int64, unsigned char)>(Native::MemoryAccess::SetNmBoolAddress)(MessageAddress, ScriptDomain::CurrentDomain->PinString(Arg->Key).ToInt64(), Arg->Value ? 1 : 0);
							}
							NULLDICT(_message->lBool);
						}
						else
						{
							reinterpret_cast<unsigned char(*)(__int64, __int64, unsigned char)>(Native::MemoryAccess::SetNmBoolAddress)(MessageAddress, ScriptDomain::CurrentDomain->PinString("start").ToInt64(), 1);
						}
						if (!ISNULL(_message->lInt))
						{
							for each(KeyValuePair<String^, int> ^Arg in _message->lInt)
							{
								reinterpret_cast<unsigned char(*)(__int64, __int64, int)>(Native::MemoryAccess::SetNmIntAddress)(MessageAddress, ScriptDomain::CurrentDomain->PinString(Arg->Key).ToInt64(), Arg->Value);
							}
							NULLDICT(_message->lInt);
						}
						if (!ISNULL(_message->lFloat))
						{
							for each(KeyValuePair<String^, float> ^Arg in _message->lFloat)
							{
								reinterpret_cast<unsigned char(*)(__int64, __int64, float)>(Native::MemoryAccess::SetNmFloatAddress)(MessageAddress, ScriptDomain::CurrentDomain->PinString(Arg->Key).ToInt64(), Arg->Value);
							}
							NULLDICT(_message->lFloat);
						}
						if (!ISNULL(_message->lString))
						{
							for each(KeyValuePair<String^, String^> ^Arg in _message->lString)
							{
								reinterpret_cast<unsigned char(*)(__int64, __int64, __int64)>(Native::MemoryAccess::SetNmStringAddress)(MessageAddress, ScriptDomain::CurrentDomain->PinString(Arg->Key).ToInt64(), ScriptDomain::CurrentDomain->PinString(Arg->Value).ToInt64());
							}
							NULLDICT(_message->lString);
						}
						if (!ISNULL(_message->lVec))
						{
							for each(KeyValuePair<String^, Math::Vector3> ^Arg in _message->lVec)
							{
								reinterpret_cast<unsigned char(*)(__int64, __int64, float, float, float)>(Native::MemoryAccess::SetNmVec3Address)(MessageAddress, ScriptDomain::CurrentDomain->PinString(Arg->Key).ToInt64(), Arg->Value.X, Arg->Value.Y, Arg->Value.Z);
							}
							NULLDICT(_message->lVec);
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
						PedNmAddress = *reinterpret_cast<__int64*>(_PedAddress + 5016);
						if (*reinterpret_cast<__int64*>(_PedAddress + 48) == PedNmAddress && *reinterpret_cast<float*>(_PedAddress + 5232) <= *reinterpret_cast<float*>(_PedAddress + 640))
						{
							if ((*reinterpret_cast<int(**)(__int64)> (*reinterpret_cast<__int64*>(PedNmAddress)+152))(PedNmAddress) != -1)
							{
								if (*(short *)(reinterpret_cast<__int64(*)(__int64)>(*reinterpret_cast<int*>(BaseFunc + 0xA2) + BaseFunc + 0xA6)(*(__int64 *)(*(__int64 *)(_PedAddress + 4208) + 864)) + 52) == 401)
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
									int count = *reinterpret_cast<int*>(*reinterpret_cast<__int64*>(_PedAddress + 4208) + 1064);
									if (v7)
									{
										reinterpret_cast<void(*)(__int64)>(*reinterpret_cast<int*>(BaseFunc + 0xF0) + BaseFunc + 0xF4)(UnkStrAddr);
									}
									for (int i = 0; i < count; i++)
									{
										v11 = *reinterpret_cast<__int64*>(*reinterpret_cast<__int64*>(_PedAddress + 4208) + 8 * ((i + *reinterpret_cast<int*>(*reinterpret_cast<__int64*>(_PedAddress + 4208) + 1060) + 1) % 16) + 928);
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
				}

			private:
				BaseMessage ^_message;
				Ped ^_target;
			};
		}

		BaseMessage::BaseMessage(String ^message) : _message(message)
		{
		}

		String ^BaseMessage::ToString()
		{
			return _message;
		}

		void BaseMessage::Abort(Ped ^target)
		{
			SetArgument("start", false);

			ScriptDomain::CurrentDomain->ExecuteTask(gcnew NmApply(this, target));
		}
		void BaseMessage::SendTo(Ped ^target, int duration)
		{
			if (!target->CanRagdoll)
			{
				target->CanRagdoll = true;
			}

			Native::Function::Call(Native::Hash::SET_PED_TO_RAGDOLL, target->Handle, 10000, duration, 1, 1, 1, 0);

			SendTo(target);
		}
		void BaseMessage::SendTo(Ped ^target)
		{
			if (!target->IsRagdoll)
			{
				if (!target->CanRagdoll)
				{
					target->CanRagdoll = true;
				}

				Native::Function::Call(Native::Hash::SET_PED_TO_RAGDOLL, target->Handle, 10000, -1, 1, 1, 1, 0);
			}

			ScriptDomain::CurrentDomain->ExecuteTask(gcnew NmApply(this, target));
		}

		void BaseMessage::SetArgument(String ^message, bool value)
		{
			if (ISNULL(lBool))
				lBool = gcnew Dictionary<String ^, bool>();
			lBool->default[message] = value;
		}
		void BaseMessage::SetArgument(String ^message, int value)
		{
			if (ISNULL(lInt))
				lInt = gcnew Dictionary<String ^, int>();
			lInt->default[message] = value;
		}
		void BaseMessage::SetArgument(String ^message, float value)
		{
			if (ISNULL(lFloat))
				lFloat = gcnew Dictionary<String ^, float>();
			lFloat->default[message] = value;
		}
		void BaseMessage::SetArgument(String ^message, double value)
		{
			SetArgument(message, static_cast<float>(value));
		}
		void BaseMessage::SetArgument(String ^message, String ^value)
		{
			if (ISNULL(lString))
				lString = gcnew Dictionary<String ^, String ^>();
			lString->default[message] = value;
		}
		void BaseMessage::SetArgument(String ^message, Math::Vector3 value)
		{
			if (ISNULL(lVec))
				lVec = gcnew Dictionary<String ^, Math::Vector3>();
			lVec->default[message] = value;
		}
		void BaseMessage::ResetArguments()
		{
			NULLDICTCHECK(lBool);
			NULLDICTCHECK(lInt);
			NULLDICTCHECK(lFloat);
			NULLDICTCHECK(lString);
			NULLDICTCHECK(lVec);
		}
		
		BaseHelper::BaseHelper(Ped^ Ped, String ^Message) : BaseMessage(Message), _ped(Ped)
		{
		}

		void BaseHelper::Start()
		{
			BaseMessage::SendTo(_ped);
		}
		void BaseHelper::Start(int duration)
		{
			BaseMessage::SendTo(_ped, duration);
		}
		void BaseHelper::Stop()
		{
			BaseMessage::Abort(_ped);
		}

		CustomHelper::CustomHelper(Ped ^ped, String ^message) : BaseHelper(ped, message)
		{
		}

		void CustomHelper::Start()
		{
			BaseHelper::Start();
		}
		void CustomHelper::Start(int duration)
		{
			BaseHelper::Start(duration);
		}

		void CustomHelper::SetArgument(String ^message, bool value)
		{
			BaseMessage::SetArgument(message, value);
		}
		void CustomHelper::SetArgument(String ^message, int value)
		{
			BaseMessage::SetArgument(message, value);
		}
		void CustomHelper::SetArgument(String ^message, float value)
		{
			BaseMessage::SetArgument(message, value);
		}
		void CustomHelper::SetArgument(String ^message, double value)
		{
			BaseMessage::SetArgument(message, value);
		}
		void CustomHelper::SetArgument(String ^message, String ^value)
		{
			BaseMessage::SetArgument(message, value);
		}
		void CustomHelper::SetArgument(String ^message, Math::Vector3 value)
		{
			BaseMessage::SetArgument(message, value);
		}
		void CustomHelper::ResetArguments()
		{
			BaseMessage::ResetArguments();
		}

		CustomMessage::CustomMessage(String ^message) : BaseMessage(message)
		{
		}

		String ^CustomMessage::Message::get()
		{
			return _message;
		}

		void CustomMessage::Abort(Ped ^target)
		{
			BaseMessage::Abort(target);
		}
		void CustomMessage::SendTo(Ped ^target)
		{
			BaseMessage::SendTo(target);
		}
		void CustomMessage::SendTo(Ped ^target, int Duration)
		{
			BaseMessage::SendTo(target, Duration);
		}

		void CustomMessage::SetArgument(String ^message, bool value)
		{
			BaseMessage::SetArgument(message, value);
		}
		void CustomMessage::SetArgument(String ^message, int value)
		{
			BaseMessage::SetArgument(message, value);
		}
		void CustomMessage::SetArgument(String ^message, float value)
		{
			BaseMessage::SetArgument(message, value);
		}
		void CustomMessage::SetArgument(String ^message, double value)
		{
			BaseMessage::SetArgument(message, value);
		}
		void CustomMessage::SetArgument(String ^message, String ^value)
		{
			BaseMessage::SetArgument(message, value);
		}
		void CustomMessage::SetArgument(String ^message, Math::Vector3 value)
		{
			BaseMessage::SetArgument(message, value);
		}
	}
}