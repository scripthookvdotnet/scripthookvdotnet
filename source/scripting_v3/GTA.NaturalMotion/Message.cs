//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GTA.NaturalMotion
{
    /// <summary>
    /// A base class for manually building a NaturalMotion Euphoria message.
    /// </summary>
    public class Message
    {
        #region Fields
        private string _message;
        private Dictionary<string, (int value, Type type)> _boolIntFloatArguments;
        private Dictionary<string, object> _stringVector3ArrayArguments;
        private static readonly Dictionary<string, (int value, Type type)> s_stopArgument = new() { { "start", (0, typeof(bool)) } };
        #endregion

        /// <summary>
        /// Creates a class to manually build a NaturalMotion Euphoria message that can be sent to any <see cref="Ped"/>.
        /// </summary>
        /// <param name="message">The name of the message.</param>
        public Message(string message)
        {
            _message = message;
            _boolIntFloatArguments = new Dictionary<string, (int value, Type type)>();
            _stringVector3ArrayArguments = new Dictionary<string, object>();
        }

        /// <summary>
        /// Stops this behavior on the given <see cref="Ped"/>.
        /// </summary>
        /// <param name="target">The <see cref="Ped"/> to stop the behavior on.</param>
        public void Abort(Ped target)
        {
            if (target == null || !target.Exists())
            {
                return;
            }

            SHVDN.NativeMemory.SendNmMessage(target.Handle, _message, s_stopArgument, null);
        }

        /// <summary>
        /// Sends the message for this behavior to the given <see cref="Ped"/>. Will not start it unless the <c>"start"</c> argument is set.
        /// Starts a <c>CTaskNMControl</c> task if the <see cref="Ped"/> has no such task and loops it until manually aborted.
        /// </summary>
        /// <param name="target">The <see cref="Ped"/> to send the <see cref="Message"/> to.</param>
        /// <remarks>
        /// Although it is technically possible to send NM messages to peds that are running a NM task other than <c>CTaskNMControl</c> without starting a <c>CTaskNMControl</c> task, this method will always start a <c>CTaskNMControl</c> task.
        /// </remarks>
        public void SendTo(Ped target)
        {
            if (target == null || !target.Exists())
            {
                return;
            }

            if (!target.IsRagdoll)
            {
                if (!target.CanRagdoll)
                {
                    target.CanRagdoll = true;
                }
            }

            if (!SHVDN.NativeMemory.IsTaskNmScriptControlOrEventSwitch2NmActive(target.MemoryAddress))
            {
                // Does not call when a CTaskNMControl task is active or the CEvent (which usually causes some task) related to CTaskNMControl occured for calling SET_PED_TO_RAGDOLL just like in legacy scripts.
                // Otherwise, the ragdoll duration will be overridden.
                Function.Call(Hash.SET_PED_TO_RAGDOLL, target.Handle, 10000, -1, 1, 1, 1, 0);
            }

            SHVDN.NativeMemory.SendNmMessage(target.Handle, _message, _boolIntFloatArguments, _stringVector3ArrayArguments);
        }
        /// <summary>
        ///	Starts this behavior on the given <see cref="Ped"/> for a specified duration.
        ///	Always starts a new ragdoll task, making it impossible to stack multiple behaviors on the <see cref="Ped"/>.
        /// </summary>
        /// <param name="target">The <see cref="Ped"/> to send the <see cref="Message"/> to.</param>
        /// <param name="duration">How long to apply the behavior for (-1 for looped).</param>
        public void SendTo(Ped target, int duration)
        {
            if (target == null || !target.Exists())
            {
                return;
            }

            if (!target.CanRagdoll)
            {
                target.CanRagdoll = true;
            }

            // Always call to specify the new duration
            Function.Call(Hash.SET_PED_TO_RAGDOLL, target.Handle, 10000, duration, 1, 1, 1, 0);
            SendTo(target);
        }

        /// <summary>
        /// Sets an argument to a <see cref="bool"/> value.
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
        /// Sets an argument to a <see cref="int"/> value.
        /// </summary>
        /// <param name="argName">The argument name.</param>
        /// <param name="value">The value to set the argument to.</param>
        public void SetArgument(string argName, int value)
        {
            CreateBoolIntFloatArgDictIfNotCreated();

            _boolIntFloatArguments[argName] = (value, typeof(int));
        }
        /// <summary>
        /// Sets an argument to a <see cref="float"/> value.
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
        /// Sets an argument to a <see cref="string"/> value.
        /// </summary>
        /// <param name="argName">The argument name.</param>
        /// <param name="value">The value to set the argument to.</param>
        public void SetArgument(string argName, string value)
        {
            CreateStringVector3ArrayArgDictIfNotCreated();

            _stringVector3ArrayArguments[argName] = value;
        }
        /// <summary>
        /// Sets an argument to a <see cref="Vector3"/> value.
        /// </summary>
        /// <param name="argName">The argument name.</param>
        /// <param name="value">The value to set the argument to.</param>
        public void SetArgument(string argName, Vector3 value)
        {
            CreateStringVector3ArrayArgDictIfNotCreated();

            _stringVector3ArrayArguments[argName] = value.ToArray();
        }

        /// <summary>
        /// Removes an argument.
        /// </summary>
        /// <param name="argName">The argument name.</param>
        public bool RemoveArgument(string argName)
        {
            if (_boolIntFloatArguments != null)
            {
                return _boolIntFloatArguments.Remove(argName);
            }

            if (_stringVector3ArrayArguments == null)
            {
                return false;
            }

            return _stringVector3ArrayArguments.Remove(argName);
        }

        /// <summary>
        /// Resets all arguments to their default values.
        /// </summary>
        public void ResetArguments()
        {
            _boolIntFloatArguments?.Clear();
            _stringVector3ArrayArguments?.Clear();
        }

        /// <summary>
        /// This method is called from internal methods of ScriptHookVDotNet and is
        /// not intended to be used directly from your code.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void CreateBoolIntFloatArgDictIfNotCreated()
        {
            _boolIntFloatArguments ??= new Dictionary<string, (int value, Type type)>();
        }

        /// <summary>
        /// This method is called from internal methods of ScriptHookVDotNet and is
        /// not intended to be used directly from your code.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void CreateStringVector3ArrayArgDictIfNotCreated()
        {
            _stringVector3ArrayArguments ??= new Dictionary<string, object>();
        }

        /// <summary>
        /// Returns the internal message name.
        /// </summary>
        public override string ToString()
        {
            return _message;
        }
    }
}
