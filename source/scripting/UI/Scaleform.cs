using System;
using System.Drawing;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	public sealed class ScaleformArgumentTXD
	{
		#region Fields
		internal string _txd;
		#endregion

		public ScaleformArgumentTXD(string s)
		{
			_txd = s;
		}
	}

	/// <summary>
	/// A class which handles rendering of Scaleform elements.
	/// </summary>
	public sealed class Scaleform : IDisposable, INativeValue
	{
		public Scaleform(string scaleformID)
		{
			_handle = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, scaleformID);
		}

		public void Dispose()
		{
			if (IsLoaded)
			{
				unsafe
				{
					fixed (int* handlePtr = &_handle)
					{
						Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, handlePtr);
					}
				}
			}

			GC.SuppressFinalize(this);
		}

		public int Handle
		{
			get { return _handle; }
		}

		private int _handle;

		public ulong NativeValue
		{
			get
			{
				return (ulong)Handle;
			}
			set
			{
				_handle = unchecked((int)value);
			}
		}

		public bool IsValid
		{
			get
			{
				return Handle != 0;
			}
		}
		public bool IsLoaded
		{
			get
			{
				return Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Handle);
			}
		}

		public void CallFunction(string function, params object[] arguments)
		{
			CallFunctionHead(function, arguments);
			Function.Call(Hash._POP_SCALEFORM_MOVIE_FUNCTION_VOID);
		}

		public int CallFunctionReturn(string function, params object[] arguments)
		{
			CallFunctionHead(function, arguments);
			return Function.Call<int>(Hash._POP_SCALEFORM_MOVIE_FUNCTION);
		}

		internal void CallFunctionHead(string function, params object[] arguments)
		{
			Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION, Handle, function);

			foreach (var argument in arguments)
			{
				if (argument is int)
				{
					Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_INT, (int)argument);
				}
				else if (argument is string)
				{
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, MemoryAccess.StringPtr);
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, (string)argument);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				}
				else if (argument is char)
				{
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, MemoryAccess.StringPtr);
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, argument.ToString());
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				}
				else if (argument is float)
				{
					Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_FLOAT, (float)argument);
				}
				else if (argument is double)
				{
					Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_FLOAT, (float)(double)argument);
				}
				else if (argument is bool)
				{
					Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_BOOL, (bool)argument);
				}
				else if (argument is ScaleformArgumentTXD)
				{
					Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_STRING, ((ScaleformArgumentTXD)argument)._txd);
				}
				else
				{
					throw new ArgumentException(string.Format("Unknown argument type {0} passed to scaleform with handle {1}.", argument.GetType().Name, Handle), "arguments");
				}
			}
		}

		public void Render2D()
		{
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Handle, 255, 255, 255, 255, 0);
		}
		public void Render2DScreenSpace(PointF location, PointF size)
		{
			float x = location.X / UI.Screen.Width;
			float y = location.Y / UI.Screen.Height;
			float width = size.X / UI.Screen.Width;
			float height = size.Y / UI.Screen.Height;

			Function.Call(Hash.DRAW_SCALEFORM_MOVIE, Handle, x + (width / 2.0f), y + (height / 2.0f), width, height, 255, 255, 255, 255);
		}
		public void Render3D(Vector3 position, Vector3 rotation, Vector3 scale)
		{
			Function.Call(Hash._DRAW_SCALEFORM_MOVIE_3D_NON_ADDITIVE, Handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2.0f, 2.0f, 1.0f, scale.X, scale.Y, scale.Z, 2);
		}
		public void Render3DAdditive(Vector3 position, Vector3 rotation, Vector3 scale)
		{
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_3D, Handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2.0f, 2.0f, 1.0f, scale.X, scale.Y, scale.Z, 2);
		}
	}
}
