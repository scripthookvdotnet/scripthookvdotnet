//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//
using System.Windows.Forms;
using GTA.Native;

namespace GTA.Input
{
    public static class Controls
    {
        /// <summary>
        /// Gets the raw analog value of an input action in the range [0, 255].
        /// </summary>
        /// <param name="type">The control type context (e.g., <see cref="ControlType.Player"/>).</param>
        /// <param name="action">The input action to check.</param>
        /// <returns>
        /// The analog value (0–255), where 0 is not pressed and 255 is fully pressed.
        /// </returns>
        /// <remarks>
        /// The returned value is 0 if the control is unavailable (e.g. player control is disabled or no player ped is active).
        /// </remarks>
        public static byte GetControlValue(ControlType type, InputAction action) => Function.Call<byte>(Hash.GET_CONTROL_VALUE, type, action);

        /// <summary>
        /// Gets the normalized analog value of an input action in the range [0.0, 1.0].
        /// </summary>
        /// <param name="type">The control type context (e.g., <see cref="ControlType.Player"/>).</param>
        /// <param name="action">The input action to check.</param>
        /// <returns>
        /// A float value between 0.0 and 1.0 representing the analog input.
        /// </returns>
        /// <remarks>
        /// Returns 0.0 if the control is disabled or unavailable.
        /// </remarks>
        public static float GetControlValueNormalized(ControlType type, InputAction action) => Function.Call<float>(Hash.GET_CONTROL_NORMAL, type, action);

        /// <summary>
        /// Sets the analog value of an input action for the next frame.
        /// </summary>
        /// <param name="type">The control type context (e.g., <see cref="ControlType.Player"/>).</param>
        /// <param name="action">The input action to set.</param>
        /// <param name="value">The normalized value to set (0.0 to 1.0).</param>
        /// <remarks>
        /// Has no effect if the control is disabled.
        /// </remarks>
        public static void SetControlValueNormalized(ControlType type, InputAction action, float value) => Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, type, action, value);

        /// <summary>
        /// Checks if an input action is currently pressed.
        /// </summary>
        /// <param name="type">The control type context.</param>
        /// <param name="action">The input action to check.</param>
        /// <returns><see langword="true"/> if the control is currently pressed; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This variant ignores whether the control is disabled.
        /// </remarks>
        public static bool IsControlPressed(ControlType type, InputAction action) => Function.Call<bool>(Hash.IS_CONTROL_PRESSED, type, action);

        /// <summary>
        /// Checks if an input action was just pressed this frame.
        /// </summary>
        /// <param name="type">The control type context.</param>
        /// <param name="action">The input action to check.</param>
        /// <returns><see langword="true"/> if the control was just pressed; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This variant ignores whether the control is disabled.
        /// </remarks>
        public static bool IsControlJustPressed(ControlType type, InputAction action) => Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, type, action);

        /// <summary>
        /// Checks if an input action was just released this frame.
        /// </summary>
        /// <param name="type">The control type context.</param>
        /// <param name="action">The input action to check.</param>
        /// <returns><see langword="true"/> if the control was just released; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This variant ignores whether the control is disabled.
        /// </remarks>
        public static bool IsControlJustReleased(ControlType type, InputAction action) => Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, type, action);

        /// <summary>
        /// Checks if a disabled input action is currently pressed.
        /// </summary>
        /// <param name="type">The control type context.</param>
        /// <param name="action">The input action to check.</param>
        /// <returns><see langword="true"/> if the disabled control is currently pressed; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This variant ignores whether the control is disabled.
        /// </remarks>
        public static bool IsDisabledControlPressed(ControlType type, InputAction action) => Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, type, action);

        /// <summary>
        /// Checks if a disabled input action was just pressed this frame.
        /// </summary>
        /// <param name="type">The control type context.</param>
        /// <param name="action">The input action to check.</param>
        /// <returns><see langword="true"/> if the disabled control was just pressed; otherwise, <see langword="false"/>.</returns>
        public static bool IsDisabledControlJustPressed(ControlType type, InputAction action) => Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, type, action);

        /// <summary>
        /// Checks if a disabled input action was just released this frame.
        /// </summary>
        /// <param name="type">The control type context.</param>
        /// <param name="action">The input action to check.</param>
        /// <returns><see langword="true"/> if the disabled control was just released; otherwise, <see langword="false"/>.</returns>

        /// <summary>
        /// Checks if a disabled input action was just released this frame.
        /// </summary>
        /// <param name="type">The control type context.</param>
        /// <param name="action">The input action to check.</param>
        /// <returns><see langword="true"/> if the disabled control was just released; otherwise, <see langword="false"/>.</returns>
        public static bool IsDisabledControlJustReleased(ControlType type, InputAction action) => Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, type, action);

        /// <summary>
        /// Gets the normalized analog value of a disabled input action (0.0 to 1.0).
        /// </summary>
        /// <param name="type">The control type context.</param>
        /// <param name="action">The input action to check.</param>
        /// <returns>
        /// A float value from 0.0 to 1.0 representing the analog input value, even if the control is disabled.
        /// </returns>
        public static float GetDisabledControlValueNormalized(ControlType type, InputAction action) => Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, type, action);

        /// <summary>
        /// Enables all input actions of the specified control type for the current frame.
        /// </summary>
        /// <param name="type">The control type to enable.</param>
        public static void EnableAllControlActionsThisFrame(ControlType type) => Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, type);

        /// <summary>
        /// Disables all input actions of the specified control type for the current frame.
        /// </summary>
        /// <param name="type">The control type to disable.</param>
        /// <remarks>
        /// Equivalent to <c>DISABLE_ALL_CONTROL_ACTIONS</c>.
        /// </remarks>
        public static void DisableAllControlActionsThisFrame(ControlType type) => Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, type);

        /// <summary>
        /// Checks if an input action is currently enabled.
        /// </summary>
        /// <param name="type">The control type context.</param>
        /// <param name="action">The input action to check.</param>
        /// <returns><see langword="true"/> if the control is enabled; otherwise, <see langword="false"/>.</returns>
        public static bool IsControlEnabled(ControlType type, InputAction action) => Function.Call<bool>(Hash.IS_CONTROL_ENABLED, type, action);

        /// <summary>
        /// Enables an input action for the current frame.
        /// </summary>
        /// <param name="type">The control type context.</param>
        /// <param name="action">The input action to enable.</param>
        public static void EnableControlThisFrame(ControlType type, InputAction action) => Function.Call(Hash.ENABLE_CONTROL_ACTION, type, action, true);

        /// <summary>
        /// Disables an input action for the current frame.
        /// </summary>
        /// <param name="type">The control type context.</param>
        /// <param name="action">The input action to disable.</param>
        public static void DisableControlThisFrame(ControlType type, InputAction action) => Function.Call(Hash.DISABLE_CONTROL_ACTION, type, action, true);

        /// <summary>
        /// Checks if the specified keyboard key is currently pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns><see langword="true"/> if the key is pressed; otherwise, <see langword="false"/>.</returns>
        public static bool IsKeyPressed(Keys key) => SHVDN.ScriptDomain.CurrentDomain.IsKeyPressed(key);
    }
}
