using System;
using System.Collections.Generic;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	namespace NaturalMotion
	{
		public class Message
		{
			#region Fields
			string _message;
			Dictionary<string, object> _arguments;
			#endregion

			public Message(string message)
			{
				_message = message;
				_arguments = new Dictionary<string, object>();
			}

			public void Abort(Ped target)
			{
				MemoryAccess.SendEuphoriaMessage(target.Handle, _message, _arguments);
			}
			public void SendTo(Ped target)
			{
				if (!target.IsRagdoll)
				{
					if (!target.CanRagdoll)
					{
						target.CanRagdoll = true;
					}

					Function.Call(Hash.SET_PED_TO_RAGDOLL, target.Handle, 10000, -1, 1, 1, 1, 0);
				}

				SetArgument("start", true);

				MemoryAccess.SendEuphoriaMessage(target.Handle, _message, _arguments);
			}
			public void SendTo(Ped target, int duration)
			{
				if (!target.CanRagdoll)
				{
					target.CanRagdoll = true;
				}

				Function.Call(Hash.SET_PED_TO_RAGDOLL, target.Handle, 10000, duration, 1, 1, 1, 0);

				SendTo(target);
			}

			public void SetArgument(string message, bool value)
			{
				_arguments[message] = value;
			}
			public void SetArgument(string message, int value)
			{
				_arguments[message] = value;
			}
			public void SetArgument(string message, float value)
			{
				_arguments[message] = value;
			}
			public void SetArgument(string message, string value)
			{
				_arguments[message] = value;
			}
			public void SetArgument(string message, Vector3 value)
			{
				_arguments[message] = value;
			}
			public void ResetArguments()
			{
				_arguments.Clear();
			}

			public override string ToString()
			{
				return _message;
			}
		};

		public abstract class CustomHelper : Message
		{
			#region Fields
			protected Ped _ped;
			#endregion

			public CustomHelper(Ped target, string message) : base(message)
			{
				_ped = target;
			}

			public void Start()
			{
				base.SendTo(_ped);
			}
			public void Start(int duration)
			{
				base.SendTo(_ped, duration);
			}
			public void Stop()
			{
				base.Abort(_ped);
			}
			public void Abort()
			{
				base.Abort(_ped);
			}
		};
	}
}
