using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTA.NaturalMotion
{
	/// <summary>
	/// A Base class for manually building a <see cref="GTA.NaturalMotion.Message"/>
	/// </summary>
	public class Message
	{
		#region Fields

		readonly string _message;
		readonly Dictionary<string, object> _arguments;

		#endregion

		/// <summary>
		/// Creates a class to manually build <see cref="Message"/>s that can be sent to any <see cref="Ped"/>.
		/// </summary>
		/// <param name="message">The name of the natual motion message</param>
		public Message(string message)
		{
			_message = message;
			_arguments = new Dictionary<string, object>();
		}

		/// <summary>
		/// Stops this Natural Motion behavious on the given <see cref="Ped"/>
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> to send the Abort <see cref="Message"/> to</param>
		public void Abort(Ped target)
		{
			MemoryAccess.SendEuphoriaMessage(target.Handle, _message, _arguments);
		}

		/// <summary>
		/// Starts this Natural Motion behaviour on the <see cref="Ped"/> that will loop until manually aborted
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> to send the <see cref="Message"/> to</param>
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


		/// <summary>
		///	Starts this Natural Motion behaviour on the <see cref="Ped"/> for a specified duration.
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> to send the <see cref="Message"/> to</param>
		/// <param name="duration">How long to apply the behaviour for (-1 for looped)</param>
		public void SendTo(Ped target, int duration)
		{
			if (!target.CanRagdoll)
			{
				target.CanRagdoll = true;
			}

			Function.Call(Hash.SET_PED_TO_RAGDOLL, target.Handle, 10000, duration, 1, 1, 1, 0);

			SendTo(target);
		}

		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="bool"/> value
		/// </summary>
		/// <param name="argName">The argument name</param>
		/// <param name="value">The value to set the argument to</param>
		public void SetArgument(string argName, bool value)
		{
			_arguments[argName] = value;
		}

		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="int"/> value
		/// </summary>
		/// <param name="argName">The argument name</param>
		/// <param name="value">The value to set the argument to</param>
		public void SetArgument(string argName, int value)
		{
			_arguments[argName] = value;
		}

		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="float"/> value
		/// </summary>
		/// <param name="argName">The argument name</param>
		/// <param name="value">The value to set the argument to</param>
		public void SetArgument(string argName, float value)
		{
			_arguments[argName] = value;
		}

		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="string"/> value
		/// </summary>
		/// <param name="argName">The argument name</param>
		/// <param name="value">The value to set the argument to</param>
		public void SetArgument(string argName, string value)
		{
			_arguments[argName] = value;
		}

		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="Vector3"/> value
		/// </summary>
		/// <param name="argName">The argument name</param>
		/// <param name="value">The value to set the argument to</param>
		public void SetArgument(string argName, Vector3 value)
		{
			_arguments[argName] = value;
		}

		/// <summary>
		/// Resets all arguments to their default value's
		/// </summary>
		public void ResetArguments()
		{
			_arguments.Clear();
		}

		public override string ToString()
		{
			return _message;
		}
	}

	/// <summary>
	/// A Helper class for building a <seealso cref="GTA.NaturalMotion.Message" /> and sending it to a given <see cref="Ped"/>
	/// </summary>
	public abstract class CustomHelper : Message
	{
		#region Fields

		protected Ped _ped;

		#endregion

		/// <summary>
		/// Creates a Helper class for building Natural Motion messages to send to a given <see cref="Ped"/>
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> that the message will be applied to</param>
		/// <param name="message">The name of the natual motion message</param>
		public CustomHelper(Ped target, string message) : base(message)
		{
			_ped = target;
		}

		/// <summary>
		/// Starts this Natural Motion behaviour on the <see cref="Ped"/> that will loop until manually aborted
		/// </summary>
		public void Start()
		{
			base.SendTo(_ped);
		}

		/// <summary>
		/// Starts this Natural Motion behaviour on the <see cref="Ped"/> for a specified duration.
		/// </summary>
		/// <param name="duration">How long to apply the behaviour for (-1 for looped)</param>
		public void Start(int duration)
		{
			base.SendTo(_ped, duration);
		}

		/// <summary>
		/// Stops this Natural Motion behavious on the <see cref="Ped"/>
		/// </summary>
		public void Stop()
		{
			base.Abort(_ped);
		}
	}
}
