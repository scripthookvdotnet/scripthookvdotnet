//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Drawing;

namespace GTA
{
	/// <summary>
	/// A class which handles rendering of Scaleform elements.
	/// </summary>
	public sealed class Scaleform : IDisposable, INativeValue
	{
		internal Scaleform(int handle)
		{
			Handle = handle;
		}
		[Obsolete("The Scaleform constructor with a string parameter is obsolete. Use Scaleform.RequestMovie instead.")]
		public Scaleform(string scaleformID)
		{
			Handle = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, scaleformID);
		}

		/// <summary>
		/// Requests a scaleform movie that is streamed in.
		/// </summary>
		/// <returns>A <see cref="Scaleform"/> instance if successfully created; otherwise, <see langword="null"/>.</returns>
		/// <remarks>
		/// Only allows 1 instance of a movie active at one time, so you cannot create multiple instances for the same movie.
		/// </remarks>
		public static Scaleform RequestMovie(string fileName)
		{
			// Note: REQUEST_SCALEFORM_MOVIE_INSTANCE is an alias of REQUEST_SCALEFORM_MOVIE and both have the same handler address,
			// so both behave exactly the same
			int handle = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, fileName);
			return handle != 0 ? new Scaleform(handle) : null;
		}
		/// <summary>
		/// Requests a scaleform movie that is streamed in and that is set to ignore super widescreen adjustments.
		/// Not available in v1.0.335.2 or v1.0.350.1.
		/// </summary>
		/// <returns>A <see cref="Scaleform"/> instance if successfully created; otherwise, <see langword="null"/>.</returns>
		/// <remarks>
		/// Only allows 1 instance of a movie active at one time, so you cannot create multiple instances for the same movie.
		/// </remarks>
		public static Scaleform RequestMovieIgnoreSuperWidescreenAdjustment(string fileName)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(
				GameVersion.v1_0_372_2_Steam,
				nameof(Scaleform),
				nameof(RequestMovieIgnoreSuperWidescreenAdjustment)
				);

			int handle = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, fileName);
			return handle != 0 ? new Scaleform(handle) : null;
		}
		/// <summary>
		/// Requests a scaleform movie that is streamed in and that will not render when the game is paused.
		/// </summary>
		/// <returns>A <see cref="Scaleform"/> instance if successfully created; otherwise, <see langword="null"/>.</returns>
		/// <remarks>
		/// Only allows 1 instance of a movie active at one time, so you cannot create multiple instances for the same movie.
		/// </remarks>
		public static Scaleform RequestMovieSkipRenderWhilePaused(string fileName)
		{
			int handle = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_SKIP_RENDER_WHILE_PAUSED, fileName);
			return handle != 0 ? new Scaleform(handle) : null;
		}

		public void Dispose()
		{
			if (IsLoaded)
			{
				unsafe
				{
					int handle = Handle;
					Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &handle);
				}
			}
		}

		public int Handle
		{
			get;
			private set;
		}

		public ulong NativeValue
		{
			get => (ulong)Handle;
			set => Handle = unchecked((int)value);
		}

		public bool IsValid => Handle != 0;
		public bool IsLoaded => Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Handle);

		void CallFunctionHead(string function, params object[] arguments)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, function);

			foreach (object argument in arguments)
			{
				switch (argument)
				{
					case int argInt:
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, argInt);
						break;
					case string argString:
						Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, SHVDN.NativeMemory.String);
						Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, argString);
						Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
						break;
					case char argChar:
						Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, SHVDN.NativeMemory.String);
						Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, argChar.ToString());
						Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
						break;
					case float argFloat:
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, argFloat);
						break;
					case double argDouble:
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)argDouble);
						break;
					case bool argBool:
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, (bool)argBool);
						break;
					case ScaleformArgumentTXD argTxd:
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_TEXTURE_NAME_STRING, argTxd._txd);
						break;
					default:
						throw new ArgumentException(
							$"Unknown argument type {argument.GetType().Name} passed to scaleform with handle {Handle.ToString()}.", nameof(arguments));
				}
			}
		}

		public void CallFunction(string function, params object[] arguments)
		{
			CallFunctionHead(function, arguments);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}
		public int CallFunctionReturn(string function, params object[] arguments)
		{
			CallFunctionHead(function, arguments);
			return Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
		}

		public void Render2D()
		{
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Handle, 255, 255, 255, 255, 0);
		}
		public void Render2DScreenSpace(PointF position, PointF size)
		{
			float x = position.X / UI.Screen.Width;
			float y = position.Y / UI.Screen.Height;
			float w = size.X / UI.Screen.Width;
			float h = size.Y / UI.Screen.Height;

			Function.Call(Hash.DRAW_SCALEFORM_MOVIE, Handle, x + (w * 0.5f), y + (h * 0.5f), w, h, 255, 255, 255, 255);
		}

		public void Render3D(Vector3 position, Vector3 rotation, Vector3 scale)
		{
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_3D_SOLID, Handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2.0f, 2.0f, 1.0f, scale.X, scale.Y, scale.Z, 2);
		}
		public void Render3DAdditive(Vector3 position, Vector3 rotation, Vector3 scale)
		{
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_3D, Handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2.0f, 2.0f, 1.0f, scale.X, scale.Y, scale.Z, 2);
		}

		/// <summary>
		/// Gets a <see cref="Scaleform"/> instance for the passed handle if it is valid.
		/// </summary>
		/// <param name="handle">
		/// The handle value.
		/// Should be between 1 and 20 since native functions for scaleform only accepts the values between 1 and 20
		/// as scaleform handles (hardcoded limit).
		/// </param>
		/// <returns>
		/// A <see cref="Scaleform"/> instance if the passed handle is valid; otherwise, <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// Strictly, this method returns a <see cref="Scaleform"/> instance
		/// if the <c>CGameScriptHandler</c> for the SHVDN runtime has a <c>CScriptResource_ScaleformMovie</c> for the passed handle.
		/// </remarks>
		public static Scaleform FromHandle(int handle)
			=> SHVDN.NativeMemory.IsScaleformMovieHandleValid((uint)handle) ? new Scaleform(handle) : null;

		public static implicit operator InputArgument(Scaleform value)
		{
			return new InputArgument((ulong)value.Handle);
		}
	}
}
