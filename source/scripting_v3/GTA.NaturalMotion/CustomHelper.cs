//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;

namespace GTA.NaturalMotion
{
	/// <summary>
	/// A helper class for building a <seealso cref="Message" /> and sending it to a given <see cref="Ped"/>.
	/// </summary>
	public abstract class CustomHelper
	{
		#region Fields
		private readonly Ped _ped;
		private readonly Message _message;
		#endregion

		/// <summary>
		/// Creates a helper class for building a NaturalMotion Euphoria message to send to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> that the message will be applied to.</param>
		/// <param name="message">The name of the natural motion message.</param>
		protected CustomHelper(Ped target, string message)
		{
			_ped = target;
			_message = new Message(message);
		}

		/// <summary>
		/// Starts this behavior on the <see cref="Ped"/> and loop it until manually aborted.
		/// </summary>
		public void Start()
		{
			_message.SetArgument("start", true);
			_message.SendTo(_ped);
			_message.RemoveArgument("start");
		}
		/// <summary>
		/// Starts this behavior on the <see cref="Ped"/> for a specified duration.
		/// </summary>
		/// <param name="duration">How long to apply the behavior for (-1 for looped).</param>
		public void Start(int duration)
		{
			_message.SetArgument("start", true);
			_message.SendTo(_ped, duration);
			_message.RemoveArgument("start");
		}

		/// <summary>
		/// Updates this behavior on the <see cref="Ped"/> if it already running.
		/// </summary>
		public void Update()
		{
			if (!_ped.IsRagdoll)
			{
				return;
			}

			bool boolWasStartArgumentSet = _message.RemoveArgument("start");
			_message.SendTo(_ped);

			if (boolWasStartArgumentSet)
			{
				_message.SetArgument("start", true);
			}
		}

		/// <summary>
		/// Stops this behavior on the <see cref="Ped"/>.
		/// </summary>
		public void Stop()
		{
			_message.Abort(_ped);
		}

		/// <summary>
		/// Sets an argument to a <see cref="bool"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, bool value)
		{
			_message.SetArgument(argName, value);
		}
		/// <summary>
		/// Sets an argument to a <see cref="int"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, int value)
		{
			_message.SetArgument(argName, value);
		}
		/// <summary>
		/// Sets an argument to a <see cref="float"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, float value)
		{
			_message.SetArgument(argName, value);
		}
		/// <summary>
		/// Sets an argument to a <see cref="string"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, string value)
		{
			_message.SetArgument(argName, value);
		}
		/// <summary>
		/// Sets an argument to a <see cref="Vector3"/> value.
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
