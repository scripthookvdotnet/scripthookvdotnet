//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using SHVDN;
using static SHVDN.NativeMemory;

namespace GTA.NaturalMotion
{
	/// <summary>
	/// A base class for manually building a <see cref="GTA.NaturalMotion.Message"/>.
	/// </summary>
	public class Message
	{
		#region Fields
		private string _message;
		private Dictionary<string, (int value, Type type)> _boolIntFloatArguments;
		private Dictionary<string, object> _stringVector3ArrayArguments;
		private static readonly Dictionary<string, (int value, Type type)> _stopArgument = new Dictionary<string, (int value, Type type)>() { { "start", (0, typeof(bool)) } };
		#endregion

		/// <summary>
		/// Creates a class to manually build <see cref="Message"/>s that can be sent to any <see cref="Ped"/>.
		/// </summary>
		/// <param name="message">The name of the natural motion message.</param>
		public Message(string message)
		{
			_message = message;
			_boolIntFloatArguments = new Dictionary<string, (int value, Type type)>();
			_stringVector3ArrayArguments = new Dictionary<string, object>();
		}
		private Message(string message, Dictionary<string, (int value, Type type)> boolIntFloatArgs, Dictionary<string, object> stringVector3ArrayArgs)
		{
			_message = message;
			_boolIntFloatArguments = boolIntFloatArgs;
			_stringVector3ArrayArguments = stringVector3ArrayArgs;
		}

		/// <summary>
		/// Stops this NaturalMotion behavior on the given <see cref="Ped"/>.
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> to send the Abort <see cref="Message"/> to.</param>
		public void Abort(Ped target)
		{
			if (target == null || !target.Exists())
				return;

			SHVDN.NativeMemory.SendNmMessage(target.Handle, _message, _stopArgument, null);
		}

		/// <summary>
		/// Send message for this NaturalMotion behavior to the <see cref="Ped"/>. Will not start the behavior unless the <c>"start"</c> argument is set.
		/// Starts a <c>CTaskNMControl</c> task if the <see cref="Ped"/> has no such task and the task that will loop until manually aborted.
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> to send the <see cref="Message"/> to.</param>
		/// <remarks>
		/// Although it's tecnically possible to send NM messages to peds that are running a NM task other than <c>CTaskNMControl</c> without starting a <c>CTaskNMControl</c> task,
		/// this method will start a <c>CTaskNMControl</c> task in such case.
		/// </remarks>
		public void SendTo(Ped target)
		{
			if (target == null || !target.Exists())
				return;

			if (!target.IsRagdoll)
			{
				if (!target.CanRagdoll)
				{
					target.CanRagdoll = true;
				}
			}

			if (!SHVDN.NativeMemory.IsTaskNMScriptControlOrEventSwitch2NMActive(target.MemoryAddress))
			{
				// Does not call when a CTaskNMControl task is active or the CEvent (which usually causes some task) related to CTaskNMControl occured for calling SET_PED_TO_RAGDOLL just like in legacy scripts.
				// Otherwise, the ragdoll duration will be overridden.
				Function.Call(Hash.SET_PED_TO_RAGDOLL, target.Handle, 10000, -1, 1, 1, 1, 0);
			}

			SHVDN.NativeMemory.SendNmMessage(target.Handle, _message, _boolIntFloatArguments, _stringVector3ArrayArguments);
		}
		/// <summary>
		///	Starts this NaturalMotion behavior on the <see cref="Ped"/> for a specified duration.
		///	Always starts a new ragdoll task, making impossible to stack multiple Natural Motion behaviors on this <see cref="Ped"/>.
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> to send the <see cref="Message"/> to.</param>
		/// <param name="duration">How long to apply the behavior for (-1 for looped).</param>
		public void SendTo(Ped target, int duration)
		{
			if (target == null || !target.Exists())
				return;

			if (!target.CanRagdoll)
			{
				target.CanRagdoll = true;
			}

			// Always call to specify the new duration
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
			CreateBoolIntFloatArgDictIfNotCreated();

			int valueConverted = value ? 1 : 0;
			_boolIntFloatArguments[argName] = (valueConverted, typeof(bool));
		}
		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="int"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, int value)
		{
			CreateBoolIntFloatArgDictIfNotCreated();

			_boolIntFloatArguments[argName] = (value, typeof(int));
		}
		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="float"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, float value)
		{
			CreateBoolIntFloatArgDictIfNotCreated();

			unsafe
			{
				int valueConverted = *(int*)&value;
				_boolIntFloatArguments[argName] = (valueConverted, typeof(float));
			}
		}
		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="string"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, string value)
		{
			CreateStringVector3ArrayArgDictIfNotCreated();

			_stringVector3ArrayArguments[argName] = value;
		}
		/// <summary>
		/// Sets a <see cref="Message"/> argument to a <see cref="Vector3"/> value.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="value">The value to set the argument to.</param>
		public void SetArgument(string argName, Vector3 value)
		{
			CreateStringVector3ArrayArgDictIfNotCreated();

			_stringVector3ArrayArguments[argName] = value.ToArray();
		}
		/// <summary>
		/// Removes a <see cref="Message"/> argument.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		public bool RemoveArgument(string argName)
		{

			if (_boolIntFloatArguments != null)
			{
				var removedFromBoolIntFloatArgs = _boolIntFloatArguments.Remove(argName);
				if (removedFromBoolIntFloatArgs)
					return true;
			}
			else if (_stringVector3ArrayArguments != null)
			{
				var removedFromStringVector3ArrayArgs = _stringVector3ArrayArguments.Remove(argName);
				if (removedFromStringVector3ArrayArgs)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Resets all arguments to their default values.
		/// </summary>
		public void ResetArguments()
		{
			_boolIntFloatArguments?.Clear();
			_stringVector3ArrayArguments?.Clear();
		}

		public void CreateBoolIntFloatArgDictIfNotCreated()
		{
			if (_boolIntFloatArguments == null)
			{
				_boolIntFloatArguments = new Dictionary<string, (int value, Type type)>();
			}
		}

		public void CreateStringVector3ArrayArgDictIfNotCreated()
		{
			if (_stringVector3ArrayArguments == null)
			{
				_stringVector3ArrayArguments = new Dictionary<string, object>();
			}
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
			_message.SetArgument("start", true);
			_message.SendTo(_ped);
			_message.RemoveArgument("start");
		}
		/// <summary>
		/// Starts this Natural Motion behavior on the <see cref="Ped"/> for a specified duration.
		/// </summary>
		/// <param name="duration">How long to apply the behavior for (-1 for looped).</param>
		public void Start(int duration)
		{
			_message.SetArgument("start", true);
			_message.SendTo(_ped, duration);
			_message.RemoveArgument("start");
		}
		/// <summary>
		/// Updates this NaturalMotion behavior on the <see cref="Ped"/> if the corresponding behavior is running.
		/// </summary>
		public void Update()
		{
			if (!_ped.IsRagdoll)
				return;

			var boolWasStartArgumentSet = _message.RemoveArgument("start");
			_message.SendTo(_ped);

			if (boolWasStartArgumentSet)
				_message.SetArgument("start", true);
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
