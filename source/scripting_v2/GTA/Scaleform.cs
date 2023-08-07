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
	public sealed class Scaleform : IDisposable
	{
		private string _scaleformId;

		[Obsolete("Use Scaleform(string scaleformID) in this API version instead. " +
		          "In the v3 API, use one of static constructors such as Scaleform.RequestMovie instead.")]
		public Scaleform(int handle)
		{
			Handle = handle;
		}
		public Scaleform(string scaleformID)
		{
			this._scaleformId = scaleformID;

			Handle = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, scaleformID);
		}

		public void Dispose()
		{
			Unload();
		}

		public int Handle
		{
			get;
			private set;
		}

		public bool IsValid => Handle != 0;
		public bool IsLoaded => Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Handle);

		[Obsolete("Use Scaleform(string scaleformID) in this API version instead. " +
		          "In the v3 API, use one of static constructors such as Scaleform.RequestMovie instead.")]
		public bool Load(string scaleformID)
		{
			int handle = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, scaleformID);
			if (handle == 0)
			{
				return false;
			}

			Handle = handle;
			this._scaleformId = scaleformID;

			return true;
		}
		public void Unload()
		{
			if (!IsLoaded)
			{
				return;
			}

			int handle = Handle;
			unsafe
			{
				Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &handle);
			}
			Handle = handle;
		}

		public void CallFunction(string function, params object[] arguments)
		{
			Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION, Handle, function);

			foreach (object argument in arguments)
			{
				switch (argument)
				{
					case int argInt:
						Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_INT, argInt);
						break;
					case string argString:
						Function.Call(Hash._BEGIN_TEXT_COMPONENT, "STRING");
						Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, argString);
						Function.Call(Hash._END_TEXT_COMPONENT);
						break;
					case char argChar:
						Function.Call(Hash._BEGIN_TEXT_COMPONENT, "STRING");
						Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, argChar.ToString());
						Function.Call(Hash._END_TEXT_COMPONENT);
						break;
					case float argFloat:
						Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_FLOAT, argFloat);
						break;
					case double argDouble:
						Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_FLOAT, (float)argDouble);
						break;
					case bool argBool:
						Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_BOOL, argBool);
						break;
					case ScaleformArgumentTXD argTxd:
						Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_STRING, argTxd._txd);
						break;
					default:
						throw new ArgumentException(
							$"Unknown argument type {argument.GetType().Name} passed to scaleform with handle {Handle.ToString()}.");
				}
			}

			Function.Call(Hash._POP_SCALEFORM_MOVIE_FUNCTION_VOID);
		}

		public void Render2D()
		{
			Function.Call(Hash._0x0DF606929C105BE1, Handle, 255, 255, 255, 255, 0);
		}
		public void Render2DScreenSpace(PointF position, PointF size)
		{
			float x = position.X / UI.WIDTH;
			float y = position.Y / UI.HEIGHT;
			float w = size.X / UI.WIDTH;
			float h = size.Y / UI.HEIGHT;

			Function.Call(Hash.DRAW_SCALEFORM_MOVIE, Handle, x + (w * 0.5f), y + (h * 0.5f), w, h, 255, 255, 255, 255);
		}

		public void Render3D(Vector3 position, Vector3 rotation, Vector3 scale)
		{
			Function.Call(Hash._0x1CE592FDC749D6F5, Handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2.0f, 2.0f, 1.0f, scale.X, scale.Y, scale.Z, 2);
		}
		public void Render3DAdditive(Vector3 position, Vector3 rotation, Vector3 scale)
		{
			Function.Call(Hash._0x87D51D72255D4E78, Handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2.0f, 2.0f, 1.0f, scale.X, scale.Y, scale.Z, 2);
		}
	}
}
