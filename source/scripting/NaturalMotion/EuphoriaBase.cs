using System.Collections.Generic;
using GTA.Math;
using GTA.Native;

namespace GTA.NaturalMotion
{
	/// <summary>
	/// A base class for manually building a <see cref="GTA.NaturalMotion.Message"/>.
	/// </summary>
	public class Message
	{
		#region Fields
		readonly string _message;
		readonly Dictionary<string, object> _arguments;
		private static readonly Dictionary<string, object> _stopArgument = new Dictionary<string, object>() { { "start", false } };
		#endregion

		/// <summary>
		/// Creates a class to manually build <see cref="Message"/>s that can be sent to any <see cref="Ped"/>.
		/// </summary>
		/// <param name="message">The name of the natural motion message.</param>
		public Message(string message)
		{
			_message = message;
			_arguments = new Dictionary<string, object>();
		}

		/// <summary>
		/// Stops this Natural Motion behavior on the given <see cref="Ped"/>.
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> to send the Abort <see cref="Message"/> to.</param>
		public void Abort(Ped target)
		{
			MemoryAccess.SendEuphoriaMessage(target.Handle, _message, _stopArgument);
		}

		/// <summary>
		/// Starts this Natural Motion behavior on the <see cref="Ped"/> that will loop until manually aborted.
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> to send the <see cref="Message"/> to.</param>
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
		///	Starts this Natural Motion behavior on the <see cref="Ped"/> for a specified duration.
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> to send the <see cref="Message"/> to.</param>
		/// <param name="duration">How long to apply the behavior for (-1 for looped).</param>
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
		/// Sets a <see cref="Message"/> argument to a <see cref="bool"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, bool value)
		{
			_arguments[argName] = value;
		}
		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="int"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, int value)
		{
			_arguments[argName] = value;
		}
		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="float"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, float value)
		{
			_arguments[argName] = value;
		}
		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="string"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, string value)
		{
			_arguments[argName] = value;
		}
		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="Vector3"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, Vector3 value)
		{
			_arguments[argName] = value;
		}

		/// <summary>
		/// Resets all arguments to their default values.
		/// </summary>
		public void ResetArguments()
		{
			_arguments.Clear();
		}

		/// <summary>
		/// Returns the internal message name.
		/// </summary>
		public override string ToString()
		{
			return _message;
		}
	}

	/// <summary>
	/// A helper class for building a <seealso cref="GTA.NaturalMotion.Message" /> and sending it to a given <see cref="Ped"/>.
	/// </summary>
	public abstract class CustomHelper
	{
		#region Fields
		private readonly Ped _ped;
		private readonly Message _message;
		#endregion

		/// <summary>
		/// Creates a helper class for building Natural Motion messages to send to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> that the message will be applied to.</param>
		/// <param name="message">The name of the natural motion message.</param>
		protected CustomHelper(Ped target, string message)
		{
			_ped = target;
			_message = new Message(message);
		}

		/// <summary>
		/// Starts this Natural Motion behavior on the <see cref="Ped"/> that will loop until manually aborted.
		/// </summary>
		public void Start()
		{
			_message.SendTo(_ped);
			_message.ResetArguments();
		}
		/// <summary>
		/// Starts this Natural Motion behavior on the <see cref="Ped"/> for a specified duration.
		/// </summary>
		/// <param name="duration">How long to apply the behavior for (-1 for looped).</param>
		public void Start(int duration)
		{
			_message.SendTo(_ped, duration);
			_message.ResetArguments();
		}
		/// <summary>
		/// Stops this Natural Motion behavior on the <see cref="Ped"/>.
		/// </summary>
		public void Stop()
		{
			_message.Abort(_ped);
		}

		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="bool"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, bool value)
		{
			_message.SetArgument(argName, value);
		}
		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="int"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, int value)
		{
			_message.SetArgument(argName, value);
		}
		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="float"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, float value)
		{
			_message.SetArgument(argName, value);
		}
		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="string"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, string value)
		{
			_message.SetArgument(argName, value);
		}
		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="Vector3"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, Vector3 value)
		{
			_message.SetArgument(argName, value);
		}

		/// <summary>
		/// Resets all arguments to their default values.
		/// </summary>
		public void ResetArguments()
		{
			_message.ResetArguments();
		}

		/// <summary>
		/// Returns the internal message name.
		/// </summary>
		public override string ToString()
		{
			return _message.ToString();
		}
	}
}
