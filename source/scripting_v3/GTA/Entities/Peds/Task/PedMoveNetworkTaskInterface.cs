//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
    public sealed class PedMoveNetworkTaskInterface
    {
        internal PedMoveNetworkTaskInterface(Ped p)
        {
            Ped = p;
        }

        /// <summary>
        /// Gets the <see cref="Ped"/>.
        /// </summary>
        private Ped Ped
        {
            get;
        }

        public bool IsTaskActive => Function.Call<bool>(Hash.IS_TASK_MOVE_NETWORK_ACTIVE, Ped);

        public bool IsReadyForTransition => Function.Call<bool>(Hash.IS_TASK_MOVE_NETWORK_READY_FOR_TRANSITION, Ped);

        public bool RequestStateTransition(string stateName)
            => Function.Call<bool>(Hash.REQUEST_TASK_MOVE_NETWORK_STATE_TRANSITION, Ped, stateName);

        /// <value>
        /// The current script name if if a `<c>CTaskMoVEScripted</c>` is running on the <see cref="Ped"/> and
        /// the network is active on the task; otherwise, the string `<c>Unknown</c>`.
        /// </value>
        public string CurrentScriptStateName
            => Function.Call<string>(Hash.GET_TASK_MOVE_NETWORK_STATE, Ped);

        /// <summary>
        /// <para>
        /// Only available in the game version v1.0.1493.0 and later.
        /// </para>
        /// </summary>
        public void SetNetworkClipSet(AtHashValue clipSet, AtHashValue varClipSet)
        {
            GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_1493_0_Steam,
                nameof(PedMoveNetworkTaskInterface), nameof(SetNetworkClipSet));

            Function.Call(Hash.SET_TASK_MOVE_NETWORK_ANIM_SET, Ped, clipSet, varClipSet);
        }

        public void SetSignalFloat(string signalName, float signal)
        {
            Function.Call(Hash.SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT, Ped, signalName, signal);
        }

        /// <summary>
        /// <para>
        /// Only available in the game version v1.0.1493.0 and later.
        /// </para>
        /// </summary>
        public void SetSignalFloatLerpRate(string signalName, float lerpRate)
        {
            GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_1493_0_Steam,
                nameof(PedMoveNetworkTaskInterface), nameof(SetSignalFloatLerpRate));

            Function.Call(Hash.SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT_LERP_RATE, Ped, signalName, lerpRate);
        }

        public void SetSignalBool(string signalName, bool signal)
        {
            Function.Call(Hash.SET_TASK_MOVE_NETWORK_SIGNAL_BOOL, Ped, signalName, signal);
        }

        /// <summary>
        /// <para>
        /// Only available in the game version v1.0.1493.0 and later.
        /// </para>
        /// </summary>
        public float GetSignalFloat(string signalName)
        {
            GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_1493_0_Steam,
                nameof(PedMoveNetworkTaskInterface), nameof(GetSignalFloat));

            return Function.Call<float>(Hash.GET_TASK_MOVE_NETWORK_SIGNAL_FLOAT, Ped, signalName);
        }

        public bool GetSignalBool(string signalName)
            => Function.Call<bool>(Hash.GET_TASK_MOVE_NETWORK_SIGNAL_BOOL, Ped, signalName);

        public bool GetEvent(string eventName)
            => Function.Call<bool>(Hash.GET_TASK_MOVE_NETWORK_EVENT, Ped, eventName);
    }
}
