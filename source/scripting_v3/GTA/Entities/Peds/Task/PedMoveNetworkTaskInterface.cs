//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
    /// <summary>
    /// Represents a move network task interface/facade class.
    /// </summary>
    // There is no classes that has the substring "Facade" as a part of class names in the game's codebase.
    public sealed class PedMoveNetworkTaskInterface
    {
        internal PedMoveNetworkTaskInterface(Ped p)
        {
            Ped = p;
        }

        /// <summary>
        /// Gets the <see cref="GTA.Ped"/>.
        /// </summary>
        private Ped Ped
        {
            get;
        }

        /// <summary>
        /// Returns <see langword="true"/> if a move network is active.
        /// </summary>
        /// <remarks>
        /// The property searches for a `<c>CTaskMoVEScripted</c>` by searching primary tasks first, then secondary
        /// tasks.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the <see cref="GTA.Ped"/> is processing a move network task
        /// (`<c>CTaskMoVEScripted</c>`) and a move network is active in the found task; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool IsTaskActive => Function.Call<bool>(Hash.IS_TASK_MOVE_NETWORK_ACTIVE, Ped);

        /// <summary>
        /// Returns <see langword="true"/> if a move network is ready for a state transition
        /// </summary>
        public bool IsReadyForTransition => Function.Call<bool>(Hash.IS_TASK_MOVE_NETWORK_READY_FOR_TRANSITION, Ped);


        /// <summary>
        /// Returns <see langword="true"/> if a move network is ready for a state transition.
        /// </summary>
        public bool RequestStateTransition(string stateName)
            => Function.Call<bool>(Hash.REQUEST_TASK_MOVE_NETWORK_STATE_TRANSITION, Ped, stateName);

        /// <summary>
        /// Returns the current state.
        /// </summary>
        /// <remarks>
        /// Returns "<c>Unknown</c>" when <see cref="IsTaskActive"/> returns <see langword="false"/>.
        /// </remarks>
        public string CurrentScriptStateName
            => Function.Call<string>(Hash.GET_TASK_MOVE_NETWORK_STATE, Ped);

        /// <summary>
        /// <para>
        /// Sets a clip set for this move network to use.
        /// </para>
        /// <para>
        /// Only available in the game version v1.0.1493.0 and later.
        /// </para>
        /// </summary>
        /// <param name="clipSet">The hash of the name of the actual clip set to use.</param>
        /// <param name="varClipSet">
        /// The hash of the name of the variable clip set you are setting.
        /// </param>
        public void SetNetworkClipSet(AtHashValue clipSet, AtHashValue varClipSet = default)
        {
            GameVersionNotSupportedException.ThrowIfNotSupported(VersionConstsForGameVersion.v1_0_1493_0,
                nameof(PedMoveNetworkTaskInterface), nameof(SetNetworkClipSet));

            Function.Call(Hash.SET_TASK_MOVE_NETWORK_ANIM_SET, Ped, clipSet, varClipSet);
        }

        /// <summary>
        /// Sets a <see cref="float"/> MoVE signal to the passed value.
        /// </summary>
        public void SetSignalFloat(string signalName, float signal)
        {
            Function.Call(Hash.SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT, Ped, signalName, signal);
        }

        /*
         *  We omit `SET_TASK_MOVE_NETWORK_SIGNAL_LOCAL_FLOAT` because there's nothing different from
         *  `SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT` when `NetworkInterface::IsGameInProgress()` returns false (meaning
         *  the game is not online), or if the ped is not a network clone or the passed "allowOverrideCloneUpdate"
         *  is set to false.
         */

        /// <summary>
        /// <para>
        /// Sets the lerp rate to the passed value.
        /// </para>
        /// <para>
        /// Only available in the game version v1.0.1493.0 and later.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Lerp rate controls rate at which the corresponding MoVE float signal will lerp over frame updates on
        /// the clone from the current value to the target value.
        /// It is assumed that the corresponding MoVE float signal has already been created using
        /// <see cref="SetSignalFloat"/>.
        /// Currently, the lerp rate defaults to a value of `<c>0.5f</c>`. The lerp value has to be above `<c>0.0f</c>`
        /// and below `<c>1.0f</c>`.
        /// If a lerp rate of `<c>1.0f</c>` is applied then no lerping is used and the exact float value will be
        /// synced and applied immediately on the clone.
        /// </remarks>
        public void SetSignalFloatLerpRate(string signalName, float lerpRate)
        {
            GameVersionNotSupportedException.ThrowIfNotSupported(VersionConstsForGameVersion.v1_0_1493_0,
                nameof(PedMoveNetworkTaskInterface), nameof(SetSignalFloatLerpRate));

            Function.Call(Hash.SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT_LERP_RATE, Ped, signalName, lerpRate);
        }

        /// <summary>
        /// Sets a <see cref="bool"/> MoVE signal to the passed value.
        /// </summary>
        public void SetSignalBool(string signalName, bool signal)
        {
            Function.Call(Hash.SET_TASK_MOVE_NETWORK_SIGNAL_BOOL, Ped, signalName, signal);
        }

        /// <summary>
        /// <para>
        /// Gets the value of a <see cref="float"/> type output parameter from the peds scripted MoVE network.
        /// </para>
        /// <para>
        /// Only available in the game version v1.0.1493.0 and later.
        /// </para>
        /// </summary>
        public float GetSignalFloat(string signalName)
        {
            GameVersionNotSupportedException.ThrowIfNotSupported(VersionConstsForGameVersion.v1_0_1493_0,
                nameof(PedMoveNetworkTaskInterface), nameof(GetSignalFloat));

            return Function.Call<float>(Hash.GET_TASK_MOVE_NETWORK_SIGNAL_FLOAT, Ped, signalName);
        }

        /// <summary>
        /// Gets the value of a <see cref="bool"/> type output parameter from the peds scripted MoVE network.
        /// </summary>
        public bool GetSignalBool(string signalName)
            => Function.Call<bool>(Hash.GET_TASK_MOVE_NETWORK_SIGNAL_BOOL, Ped, signalName);

        /// <summary>
        /// Returns <see langword="true"/> if an event with the given name has just fired on
        /// the <see cref="GTA.Ped"/>'s script owned MoVE network.
        /// </summary>
        public bool GetEvent(string eventName)
            => Function.Call<bool>(Hash.GET_TASK_MOVE_NETWORK_EVENT, Ped, eventName);
    }
}
