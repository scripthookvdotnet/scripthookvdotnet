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
		public static Camera RenderingCamera
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
		 * We don't use 6th argument (RENDERING_OPTION_FLAGS RenderingOptions) for RENDER_SCRIPT_CAMS, but that's not
		 * an oversight
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
	}
}
