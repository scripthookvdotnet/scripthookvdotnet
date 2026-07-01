using GTA.Native;

namespace GTA.Input
{
    public static class Controls
    {
        /// <summary>
        /// Gets the value in the range [0, 255] for the specified <see cref="ControlType"/> and <see cref="ControlAction"/>.
        /// </summary>
        /// <remarks>
        /// Returns 0 if the <paramref name="action"/> is disabled.
        /// </remarks>
        public static int GetControlValue(ControlType type, ControlAction action)
        {
            return Function.Call<int>(Hash.GET_CONTROL_VALUE, (int)type, (int)action);
        }

        /// <summary>
        /// Gets the normalized value in the range [-1f, 1f] for the specified <see cref="ControlType"/> and <see cref="ControlAction"/>.
        /// </summary>
        /// <remarks>
        /// Returns 0.0f if the <paramref name="action"/> is disabled.
        /// </remarks>
        public static float GetControlNormal(ControlType type, ControlAction action)
        {
            return Function.Call<float>(Hash.GET_CONTROL_NORMAL, (int)type, (int)action);
        }

        /// <summary>
        /// Gets the unbounded normalized value for the specified <see cref="ControlType"/> and <see cref="ControlAction"/>.
        /// </summary>
        /// <remarks>
        /// This value is not clamped to [-1f, 1f] and may exceed that range depending on input device behavior.
        /// <para>
        /// Returns 0.0f if the <paramref name="action"/> is disabled.
        /// </para>
        /// </remarks>
        public static float GetControlUnboundNormal(ControlType type, ControlAction action)
        {
            return Function.Call<float>(Hash.GET_CONTROL_UNBOUND_NORMAL, (int)type, (int)action);
        }

        /// <summary>
        /// Gets the normalized value in the range [-1f, 1f] of the specified <see cref="ControlType"/> and <see cref="ControlAction"/> regardless of whether it is disabled.
        /// </summary>
        public static float GetDisabledControlNormal(ControlType type, ControlAction action)
        {
            return Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, (int)type, (int)action);
        }

        /// <summary>
        /// Gets the unbounded normalized value of the specified <see cref="ControlType"/> and <see cref="ControlAction"/> regardless of whether it is disabled.
        /// </summary>
        /// <remarks>
        /// This value is not clamped to [-1f, 1f] and may exceed that range depending on input device behavior.
        /// </remarks>
        public static float GetDisabledControlUnboundNormal(ControlType type, ControlAction action)
        {
            return Function.Call<float>(Hash.GET_DISABLED_CONTROL_UNBOUND_NORMAL, (int)type, (int)action);
        }

        /// <summary>
        /// Returns whether the specified <see cref="ControlAction"/> is enabled and currently held.
        /// </summary>
        /// <remarks>
        /// If you want to check a <see cref="ControlAction"/> regardless of whether it is enabled, use <see cref="IsDisabledControlPressed(ControlType, ControlAction)"/> instead.
        /// <para>
        /// The <paramref name="action"/> is considered pressed when the normalized value is greater than or equal to 0.5f.
        /// </para>
        /// </remarks>
        public static bool IsControlPressed(ControlType type, ControlAction action)
        {
            return Function.Call<bool>(Hash.IS_CONTROL_PRESSED, (int)type, (int)action);
        }

        /// <summary>
        /// Returns whether the specified <see cref="ControlAction"/> is currently held regardless of whether it is disabled.
        /// </summary>
        /// <remarks>
        /// The <paramref name="action"/> is considered pressed when the normalized value is greater than or equal to 0.5f.
        /// </remarks>
        public static bool IsDisabledControlPressed(ControlType type, ControlAction action)
        {
            return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, (int)type, (int)action);
        }

        /// <summary>
        /// Returns whether the specified <see cref="ControlAction"/> was pressed during this frame.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="false"/> if the <paramref name="action"/> is disabled.
        /// </remarks>
        public static bool IsControlJustPressed(ControlType type, ControlAction action)
        {
            return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, (int)type, (int)action);
        }

        /// <summary>
        /// Returns whether the specified <see cref="ControlAction"/> was pressed during this frame regardless of whether it is disabled.
        /// </summary>
        public static bool IsDisabledControlJustPressed(ControlType type, ControlAction action)
        {
            return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, (int)type, (int)action);
        }

        /// <summary>
        /// Returns whether the specified <see cref="ControlAction"/> was released during this frame.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="false"/> if the <paramref name="action"/> is disabled.
        /// </remarks>
        public static bool IsControlJustReleased(ControlType type, ControlAction action)
        {
            return Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, (int)type, (int)action);
        }

        /// <summary>
        /// Returns whether the specified <see cref="ControlAction"/> was released during this frame regardless of whether it is disabled.
        /// </summary>
        public static bool IsDisabledControlJustReleased(ControlType type, ControlAction action)
        {
            return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, (int)type, (int)action);
        }

        /// <summary>
        /// Returns whether the specified <see cref="ControlAction"/> is enabled.
        /// </summary>
        public static bool IsControlEnabled(ControlType type, ControlAction action)
        {
            return Function.Call<bool>(Hash.IS_CONTROL_ENABLED, (int)type, (int)action);
        }

        /// <summary>
        /// Enables the specified <see cref="ControlAction"/> for this frame.
        /// </summary>
        /// <param name="affectRelatedActions">
        /// <see langword="true"/> to also affect related actions in the same input group; otherwise, <see langword="false"/>.
        /// </param>
        public static void EnableControlActionThisFrame(ControlType type, ControlAction action, bool affectRelatedActions = true)
        {
            Function.Call(Hash.ENABLE_CONTROL_ACTION, (int)type, (int)action, affectRelatedActions);
        }

        /// <summary>
        /// Disables the specified <see cref="ControlAction"/> for this frame.
        /// </summary>
        /// <param name="affectRelatedActions">
        /// <see langword="true"/> to also affect related actions in the same input group; otherwise, <see langword="false"/>.
        /// </param>
        public static void DisableControlActionThisFrame(ControlType type, ControlAction action, bool affectRelatedActions = true)
        {
            Function.Call(Hash.DISABLE_CONTROL_ACTION, (int)type, (int)action, affectRelatedActions);
        }

        /// <summary>
        /// Enables all <see cref="ControlAction"/> entries for the specified <see cref="ControlType"/> for this frame.
        /// </summary>
        public static void EnableAllControlActionsThisFrame(ControlType type)
        {
            Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, (int)type);
        }

        /// <summary>
        /// Disables all <see cref="ControlAction"/> entries for the specified <see cref="ControlType"/> for this frame.
        /// </summary>
        public static void DisableAllControlActionsThisFrame(ControlType type)
        {
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, (int)type);
        }

        /// <summary>
        /// Disables all <see cref="ControlAction"/> entries for the specified <see cref="ControlType"/> except <paramref name="action"/>.
        /// </summary>
        /// <remarks>
        /// This method must be called every frame to maintain exclusivity.
        /// </remarks>
        public static void SetInputExclusive(ControlType type, ControlAction action)
        {
            Function.Call(Hash.SET_INPUT_EXCLUSIVE, (int)type, (int)action);
        }

        /// <summary>
        /// Sets the normalized value to be applied for the specified <see cref="ControlAction"/> on the next frame.
        /// </summary>
        /// <param name="value">
        /// The normalized control value to apply.
        /// </param>
        /// <remarks>
        /// If the <paramref name="action"/> is disabled, the value will not be applied.
        /// </remarks>
        public static void SetControlNormalNextFrame(ControlType type, ControlAction action, float value)
        {
            Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, (int)type, (int)action, value);
        }
    }
}
