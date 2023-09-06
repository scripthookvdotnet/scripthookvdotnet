//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	/// <summary>
	/// Represents the script camera director (<c>camScriptDirector</c> in the exe).
	/// The script camera director is responsible for managing scripted cameras.
	/// </summary>
	public static class ScriptCameraDirector
	{
		/// <summary>
		/// Gets the camera currently rendering to the game screen.
		/// </summary>
		/// <remarks>
		/// This property will return <see langword="null"/> if no scripted camera is rendering to the game screen,
		/// where the current rendering camera (<c>camBaseCamera</c>) does not match any scripted cameras this scripted
		/// camera director (<c>camScriptDirector</c>) is managing.
		/// </remarks>
		public static Camera RenderingCam
		{
			get
			{
				int handle = Function.Call<int>(Hash.GET_RENDERING_CAM);
				// GET_RENDERING_CAM returns -1 if the current rendering camera doesn't match any scripted cameras
				// the camScriptDirector is handling (or there's really no rendering camera, which is a edge case)
				// Game code for cams does never treat negative values as valid cam handles either
				return handle < 0 ? null : new Camera(handle);
			}
		}

		/*
		 * We don't use 6th argument of RENDER_SCRIPT_CAMS or 4th argument of STOP_RENDERING_SCRIPT_CAMS_USING_CATCH_UP
		 * (RENDERING_OPTION_FLAGS RenderingOptions), but that's not an oversight
		 * The way we see it with Cheat Engine, RENDER_SCRIPT_CAMS does set the flag value for the 6th params,
		 * but the game seem to clear of the flag value with zero without reading (even decompiled scripts always leave
		 * the 6th param as zero, which is the default value)
		 */

		/// <summary>
		/// Starts rendering a scripted camera without interpolation.
		/// Tells the game that script thread of the SHVDN runtime (<c>GtaThread</c>, not individual SHVDN scripts)
		/// wants to enable rendering of scripted cameras.
		/// </summary>
		public static void StartRendering(bool shouldLockInterpolationSourceFrame = true)
			=> Function.Call(Hash.RENDER_SCRIPT_CAMS, true, false, 3000, shouldLockInterpolationSourceFrame, 0, 0);
		/// <summary>
		/// Starts rendering a scripted camera while interpolating from the gameplay camera
		/// that the gameplay camera director (<c>camGameplayDirector</c>) is using to a scripted camera.
		/// Tells the game that script thread of the SHVDN runtime (<c>GtaThread</c>, not individual SHVDN scripts)
		/// wants to enable rendering of scripted cameras.
		/// </summary>
		/// <inheritdoc cref="StopRenderingWithInterp"/>
		public static void StartRenderingWithInterp(int interpDuration = 3000,
			bool shouldLockInterpolationSourceFrame = true)
			=> Function.Call(Hash.RENDER_SCRIPT_CAMS, true, true, interpDuration, shouldLockInterpolationSourceFrame,
				0, 0);
		/// <summary>
		/// Stops rendering a scripted camera without interpolation.
		/// Tells the game that script thread of the SHVDN runtime (<c>GtaThread</c>, not individual SHVDN scripts)
		/// wants to disable rendering of scripted cameras.
		/// </summary>
		/// <inheritdoc cref="StopRenderingWithInterp"/>
		public static void StopRendering(bool shouldLockInterpolationSourceFrame = true,
			bool shouldApplyAcrossAllThreads = false)
			=> Function.Call(Hash.RENDER_SCRIPT_CAMS, false, false, 3000, shouldLockInterpolationSourceFrame,
				shouldApplyAcrossAllThreads, 0);
		/// <summary>
		/// Stops rendering a scripted camera while interpolating from the previously rendered scripted camera to
		/// the gameplay camera that the gameplay camera director (<c>camGameplayDirector</c>) is using.
		/// Tells the game that script thread of the SHVDN runtime (<c>GtaThread</c>, not individual SHVDN scripts)
		/// wants to disable rendering of scripted cameras.
		/// </summary>
		/// <param name="interpDuration">The interpolation duration in milliseconds.</param>
		/// <param name="shouldLockInterpolationSourceFrame">
		/// If <see langword="false"/>, the source frame is updated throughout the interpolation,
		/// allowing for fully dynamic interpolation that can reduce the appearance of 'lag' when the source frame is
		/// not static.
		/// </param>
		/// <param name="shouldApplyAcrossAllThreads">
		/// If <see langword="true"/>, a request to stop rendering will be enforced irrespective of whether other
		/// script threads (<c>GtaThread</c>s) expect rendering to be active.
		/// Note that this can result in conflicts between concurrent script threads, so this must be used with caution.
		/// </param>
		/// <remarks>
		/// At least one of the scripts loaded by SHVDN must have created a <see cref="Camera"/> that can be rendered,
		/// so you would want to set <see cref="Camera.IsActive"/> to <see langword="true"/> on your
		/// <see cref="Camera"/>.
		/// Note that rendering is typically not stopped if another script thread (<c>GtaThread</c>) other than
		/// the SHVDN runtime still expects it to be active (see <paramref name="shouldApplyAcrossAllThreads"/>.)
		/// </remarks>
		public static void StopRenderingWithInterp(int interpDuration = 3000,
			bool shouldLockInterpolationSourceFrame = true, bool shouldApplyAcrossAllThreads = false)
			=> Function.Call(Hash.RENDER_SCRIPT_CAMS, false, true, interpDuration, shouldLockInterpolationSourceFrame,
				shouldApplyAcrossAllThreads, 0);

		/// <summary>
		/// Stops rendering a scripted camera and force gameplay camera to blend from scripted camera to gameplay
		/// camera.
		/// Tells the game that script thread of the SHVDN runtime (<c>GtaThread</c>, not individual SHVDN scripts)
		/// wants to disable rendering of scripted cameras.
		/// </summary>
		/// <param name="shouldApplyAcrossAllThreads">
		/// If <see langword="true"/>, a request to stop rendering will be enforced irrespective of whether other
		/// script threads (<c>GtaThread</c>s) expect rendering to be active.
		/// Note that this can result in conflicts between concurrent script threads, so this must be used with caution.
		/// </param>
		/// <param name="distanceToBlend">
		/// Overrides the distance over which the catch up blend occurs in the <see cref="GameplayCamera"/>
		/// (<c>camGameplayDirector</c>).
		/// If zero is specified, default blend distance will be used.
		/// </param>
		/// <param name="blendType">
		/// The blend type to use in the <see cref="GameplayCamera"/> (<c>camGameplayDirector</c>).
		/// </param>
		/// <remarks>
		/// At least one of the scripts loaded by SHVDN must have created a <see cref="Camera"/> that can be rendered,
		/// so you would want to set <see cref="Camera.IsActive"/> to <see langword="true"/> on your
		/// <see cref="Camera"/>.
		/// Note that rendering is typically not stopped if another script thread (<c>GtaThread</c>) other than
		/// the SHVDN runtime still expects it to be active (see <paramref name="shouldApplyAcrossAllThreads"/>.)
		/// </remarks>
		public static void StopRenderingUsingCatchUp(bool shouldApplyAcrossAllThreads = false,
			float distanceToBlend = 0f, CamSplineSmoothingMode blendType = CamSplineSmoothingMode.SlowInOutSmooth)
			=> Function.Call(Hash.STOP_RENDERING_SCRIPT_CAMS_USING_CATCH_UP, shouldApplyAcrossAllThreads,
				distanceToBlend, blendType, 0);

		/// <summary>
		/// Gets a value that indicates whether an interpolation is occuring from a script cam to a gameplay cam.
		/// </summary>
		/// <remarks>
		/// Returns <see langword="true"/> if the interpolating state matches the specific value on the
		/// <see cref="ScriptCameraDirector"/>.
		/// </remarks>
		public static bool IsInterpolatingFromScriptCam
			// Why tf R* adopted plural "CAMS", while the interpolation can only occur from A script cam to
			// A gameplay cam?
			=> Function.Call<bool>(Hash.IS_INTERPOLATING_FROM_SCRIPT_CAMS);

		/// <summary>
		/// Gets a value that indicates whether an interpolation is occurring to a script cam from a gameplay cam.
		/// </summary>
		/// <remarks>
		/// Returns <see langword="true"/> if the interpolating state matches the specific value on the
		/// <see cref="ScriptCameraDirector"/>.
		/// </remarks>
		public static bool IsInterpolatingToScriptCam
			=> Function.Call<bool>(Hash.IS_INTERPOLATING_TO_SCRIPT_CAMS);
	}
}
