#include "EuphoriaBase.hpp"
#include "Native.hpp"
#include "Ped.hpp"
#include "Game.hpp"

#using "ScriptHookVDotNet.asi"

namespace GTA
{
	using namespace System;
	using namespace Collections::Generic;

	extern void Log(String ^logLevel, ... array<String ^> ^message);

	namespace NaturalMotion
	{
		Message::Message(String ^message) : _message(message)
		{
		}

		void Message::Abort(Ped ^target)
		{
			Dictionary<String ^, Object ^> ^stopArgument = gcnew Dictionary<String ^, Object ^>();
			stopArgument->Add("start", false);
			SHVDN::NativeMemory::SendEuphoriaMessage(target->Handle, _message, stopArgument);
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

			SHVDN::NativeMemory::SendEuphoriaMessage(target->Handle, _message, _arguments);
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
			if (ReferenceEquals(_arguments, nullptr))
			{
				_arguments = gcnew Dictionary<String ^, Object ^>();
			}

			_arguments->default[message] = value;
		}
		void Message::SetArgument(String ^message, int value)
		{
			if (ReferenceEquals(_arguments, nullptr))
			{
				_arguments = gcnew Dictionary<String ^, Object ^>();
			}

			_arguments->default[message] = value;
		}
		void Message::SetArgument(String ^message, float value)
		{
			if (ReferenceEquals(_arguments, nullptr))
			{
				_arguments = gcnew Dictionary<String ^, Object ^>();
			}

			_arguments->default[message] = value;
		}
		void Message::SetArgument(String ^message, double value)
		{
			SetArgument(message, static_cast<float>(value));
		}
		void Message::SetArgument(String ^message, String ^value)
		{
			if (ReferenceEquals(_arguments, nullptr))
			{
				_arguments = gcnew Dictionary<String ^, Object ^>();
			}

			_arguments->default[message] = value;
		}
		void Message::SetArgument(String ^message, Math::Vector3 value)
		{
			if (ReferenceEquals(_arguments, nullptr))
			{
				_arguments = gcnew Dictionary<String ^, Object ^>();
			}

			array<float> ^data = gcnew array<float>(3);
			data[0] = value.X;
			data[1] = value.Y;
			data[2] = value.Z;
			_arguments->default[message] = data;
		}
		void Message::ResetArguments()
		{
			_arguments = nullptr;
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