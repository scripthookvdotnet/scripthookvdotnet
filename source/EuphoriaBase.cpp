#include "EuphoriaBase.hpp"
#include "NativeMemory.hpp"
#include "Native.hpp"
#include "Ped.hpp"
#include "ScriptDomain.hpp"


namespace GTA
{
	namespace NaturalMotion
	{
		private ref struct NmApply : GTA::IScriptTask
		{
		public:
			NmApply(BaseMessage ^EuphoriaMessage, GTA::Ped ^TargetPed) : _nmMessage(EuphoriaMessage), _targetPed(TargetPed)
			{
			}
			virtual void Run()
			{
				_nmMessage->GiveMessage(_targetPed);
			}
		private:
			BaseMessage ^_nmMessage;
			GTA::Ped ^_targetPed;
		};


		BaseMessage::BaseMessage(String ^Message) : _message(Message), _address((__int64*)Native::MemoryAccess::ScriptNmStructPtrAddress)
		{
		}
		void BaseMessage::iSetArgument(System::String ^message, bool value)
		{
			if (lBool == nullptr)
				lBool = gcnew Dictionary<String ^, bool>();
			lBool->default[message] = value;
		}
		void BaseMessage::iSetArgument(System::String ^message, int value)
		{
			if (lInt == nullptr)
				lInt = gcnew Dictionary<String ^, int>();
			lInt->default[message] = value;
		}
		void BaseMessage::iSetArgument(System::String ^message, float value)
		{
			if (lFloat == nullptr)
				lFloat = gcnew Dictionary<String ^, float>();
			lFloat->default[message] = value;
		}
		void BaseMessage::iSetArgument(System::String ^message, System::String ^value)
		{
			if (lString == nullptr)
				lString = gcnew Dictionary<String ^, String ^>();
			lString->default[message] = value;
		}
		void BaseMessage::iSetArgument(System::String ^message, Math::Vector3 value)
		{
			if (lVec == nullptr)
				lVec = gcnew Dictionary<String ^, Math::Vector3>();
			lVec->default[message] = value;
		}

		void BaseMessage::SetArguments()
		{
			if (lBool != nullptr)
			{
				if (!lBool->ContainsKey("start"))
				{
					reinterpret_cast<unsigned char(*)(__int64, __int64, unsigned char)>(Native::MemoryAccess::SetNmBoolAddress)(*_address, ScriptDomain::CurrentDomain->PinString("start").ToInt64(), 1);
				}
				for each(KeyValuePair<String^, bool> ^Arg in lBool)
				{
					reinterpret_cast<unsigned char(*)(__int64, __int64, unsigned char)>(Native::MemoryAccess::SetNmBoolAddress)(*_address, ScriptDomain::CurrentDomain->PinString(Arg->Key).ToInt64(), Arg->Value ? 1 : 0);
				}
			}
			else
			{
				reinterpret_cast<unsigned char(*)(__int64, __int64, unsigned char)>(Native::MemoryAccess::SetNmBoolAddress)(*_address, ScriptDomain::CurrentDomain->PinString("start").ToInt64(), 1);
			}
			if (lInt != nullptr)
			{
				for each(KeyValuePair<String^, int> ^Arg in lInt)
				{
					reinterpret_cast<unsigned char(*)(__int64, __int64, int)>(Native::MemoryAccess::SetNmIntAddress)(*_address, ScriptDomain::CurrentDomain->PinString(Arg->Key).ToInt64(), Arg->Value);
				}
			}
			if (lFloat != nullptr)
			{
				for each(KeyValuePair<String^, float> ^Arg in lFloat)
				{
					reinterpret_cast<unsigned char(*)(__int64, __int64, float)>(Native::MemoryAccess::SetNmFloatAddress)(*_address, ScriptDomain::CurrentDomain->PinString(Arg->Key).ToInt64(), Arg->Value);
				}
			}
			if (lString != nullptr)
			{
				for each(KeyValuePair<String^, String^> ^Arg in lString)
				{
					reinterpret_cast<unsigned char(*)(__int64, __int64, __int64)>(Native::MemoryAccess::SetNmStringAddress)(*_address, ScriptDomain::CurrentDomain->PinString(Arg->Key).ToInt64(), ScriptDomain::CurrentDomain->PinString(Arg->Value).ToInt64());
				}
			}
			if (lVec != nullptr)
			{
				for each(KeyValuePair<String^, Math::Vector3> ^Arg in lVec)
				{
					reinterpret_cast<unsigned char(*)(__int64, __int64, float, float, float)>(Native::MemoryAccess::SetNmVec3Address)(*_address, ScriptDomain::CurrentDomain->PinString(Arg->Key).ToInt64(), Arg->Value.X, Arg->Value.Y, Arg->Value.Z);
				}
			}
		}
		void BaseMessage::pApplyTo(GTA::Ped ^TargetPed, int duration)
		{
			if (!TargetPed->CanRagdoll)
				TargetPed->CanRagdoll = true;
			Native::Function::Call(Native::Hash::SET_PED_TO_RAGDOLL, TargetPed->Handle, 10000, duration, 1, 1, 1, 0);
			pApplyTo(TargetPed);
		}
		void BaseMessage::pApplyTo(GTA::Ped ^TargetPed)
		{
			if (!TargetPed->IsRagdoll)
			{
				if (!TargetPed->CanRagdoll)
					TargetPed->CanRagdoll = true;
				Native::Function::Call(Native::Hash::SET_PED_TO_RAGDOLL, TargetPed->Handle, 10000, -1, 1, 1, 1, 0);
			}
			ScriptDomain::CurrentDomain->ExecuteTask(gcnew NmApply(this, TargetPed));
		}
		void BaseMessage::GiveMessage(GTA::Ped ^TargetPed)
		{
			__int64 NativeFunc = Native::MemoryAccess::CreateNmMessageFunc;
			__int64 TempAddr = reinterpret_cast<__int64(*)(__int64)>(*reinterpret_cast<int*>(NativeFunc + 0x22) + NativeFunc + 0x26)(4632);
			if (TempAddr)
			{
				reinterpret_cast<__int64(*)(__int64, __int64, int)>(*reinterpret_cast<int*>(NativeFunc + 0x3C) + NativeFunc + 0x40)(TempAddr, TempAddr + 24, 64);
				*_address = TempAddr;
				SetArguments();

				__int64 BaseFunc = Native::MemoryAccess::GiveNmMessageFunc;
				__int64 ByteAddr = *reinterpret_cast<int*>(BaseFunc + 0xBC) + BaseFunc + 0xC0;
				__int64 UnkStrAddr = *reinterpret_cast<int*>(BaseFunc + 0xCE) + BaseFunc + 0xD2;
				__int64 _PedAddress = Native::MemoryAccess::GetAddressOfEntity(TargetPed->Handle);
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
					if (*_address)
					{
						if ((*reinterpret_cast<int(**)(__int64)> (*reinterpret_cast<__int64*>(PedNmAddress) + 152i64))(PedNmAddress) != -1)
						{
							if (*(short *)(reinterpret_cast<__int64(*)(__int64)>(*reinterpret_cast<int*>(BaseFunc + 0xA2) + BaseFunc + 0xA6)(*(__int64 *)(*(__int64 *)(_PedAddress + 4208) + 864i64)) + 52) == 401)
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
									v11 = *reinterpret_cast<__int64*>(*reinterpret_cast<__int64*>(_PedAddress + 4208) + 8i64 * ((i + *reinterpret_cast<int*>(*reinterpret_cast<__int64*>(_PedAddress + 4208) + 1060i64) + 1) % 16) + 928);
									if (v11)
									{
										if ((*(int(__fastcall **)(__int64))(*reinterpret_cast<__int64*>(v11) + 24i64))(v11) == 132)
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
							if (*_address)
							{
								if (v5 && (*reinterpret_cast<int(**)(__int64)>(**reinterpret_cast<__int64**>(_PedAddress + 5016) + 152i64))(*reinterpret_cast<__int64*>(_PedAddress + 5016)) != -1)
								{
									reinterpret_cast<void(*)(__int64, __int64, __int64)>(*reinterpret_cast<int*>(BaseFunc + 0x1AA) + BaseFunc + 0x1AE)(*reinterpret_cast<__int64*>(_PedAddress + 5016), ScriptDomain::CurrentDomain->PinString(_message).ToInt64(), *_address);
								}
								if (*_address)
								{
									reinterpret_cast<void(*)(__int64)>(*reinterpret_cast<int*>(BaseFunc + 0x1BB) + BaseFunc + 0x1BF)(*_address);
								}
							}
							*_address = 0;
						}
					}
				}

			}
		}

		BaseHelper::BaseHelper(GTA::Ped^ Ped, String ^Message) : BaseMessage(Message), pPed(Ped)
		{
		}
		void BaseHelper::pStart()
		{
			pApplyTo(pPed);
		}
		void BaseHelper::pStart(int duration)
		{
			pApplyTo(pPed, duration);
		}
		void BaseHelper::Stop()
		{
			iSetArgument("start", false);
			GiveMessage(pPed);
			iSetArgument("start", true);
		}

		CustomMessage::CustomMessage(String ^Message) :BaseMessage(Message)
		{
		}

		String ^CustomMessage::Message::get()
		{
			return _message;
		}
		void CustomMessage::SendTo(GTA::Ped^ TargetPed, int Duration)
		{
			pApplyTo(TargetPed, Duration);
		}
		void CustomMessage::SendTo(GTA::Ped ^TargetPed)
		{
			pApplyTo(TargetPed);
		}
		void CustomMessage::Abort(GTA::Ped ^TargetPed)
		{
			iSetArgument("start", false);
			GiveMessage(TargetPed);
			iSetArgument("start", true);
		}

		CustomHelper::CustomHelper(GTA::Ped^ Ped, String ^Message) :BaseHelper(Ped, Message)
		{
		}

		void CustomHelper::Start()
		{
			pStart();
		}
		void CustomHelper::Start(int Duration)
		{
			pStart(Duration);
		}
	}
}