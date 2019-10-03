using GTA.Math;
using GTA.Native;
using System;
using System.Drawing;

namespace GTA
{
	public abstract class ParticleEffect
	{
		#region Fields
		protected readonly ParticleEffectAsset _asset;
		protected readonly string _effectName;
		protected Vector3 _offset;
		protected Vector3 _rotation;
		protected Color _color;
		protected float _scale;
		protected float _range;
		protected InvertAxis _InvertAxis;
		#endregion

		internal ParticleEffect(ParticleEffectAsset asset, string effectName)
		{
			Handle = -1;
			_asset = asset;
			_effectName = effectName;
		}

		/// <summary>
		/// Gets the Handle of this <see cref="ParticleEffect"/>
		/// </summary>
		/// <value>
		/// The handle, will return -1 when the this <see cref="ParticleEffect"/> is not active
		/// </value>
		public int Handle
		{
			get; protected set;
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ParticleEffect"/> is active.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="ParticleEffect"/> is active; otherwise, <c>false</c>.
		/// </value>
		public bool IsActive => Handle != -1 && Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, Handle);

		public abstract bool Start();

		/// <summary>
		/// Deletes this <see cref="ParticleEffect"/>.
		/// </summary>
		public void Stop()
		{
			if (IsActive)
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, Handle, false);
			}
			Handle = -1;
		}

		/// <summary>
		/// Gets the memory address where this <see cref="ParticleEffect"/> is located in game memory.
		/// </summary>
		public IntPtr MemoryAddress => IsActive ? SHVDN.NativeMemory.GetPtfxAddress(Handle) : IntPtr.Zero;

		/// <summary>
		/// Gets or sets the offset.
		/// If this <see cref="ParticleEffect"/> is attached to an <see cref="Entity"/>, this refers to the offset from the <see cref="Entity"/>; 
		/// otherwise, this refers to its position in World coords
		/// </summary>
		public Vector3 Offset
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address != IntPtr.Zero)
				{
					address = SHVDN.NativeMemory.ReadAddress(address + 32);
					if (address != IntPtr.Zero)
					{
						return _offset = new Vector3(SHVDN.NativeMemory.ReadVector3(address + 144));
					}
				}
				return _offset;
			}
			set
			{
				_offset = value;
				IntPtr address = MemoryAddress;
				if (address != IntPtr.Zero)
				{
					address = SHVDN.NativeMemory.ReadAddress(address + 32);
					if (address != IntPtr.Zero)
					{
						SHVDN.NativeMemory.WriteVector3(address + 144, value.ToArray());
					}
				}
			}
		}

		/// <summary>
		/// Gets or Sets the rotation of this <see cref="ParticleEffect"/>
		/// </summary>
		public Vector3 Rotation
		{
			get
			{
				return _rotation;
			}
			set
			{
				_rotation = value;
				if (IsActive)
				{
					//rotation information is stored in a matrix
					Vector3 off = Offset;
					Function.Call(Hash.SET_PARTICLE_FX_LOOPED_OFFSETS, Handle, off.X, off.Y, off.Z, value.X, value.Y, value.Z);
				}
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="Color"/> of this <see cref="ParticleEffect"/>.
		/// </summary>
		public Color Color
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address != IntPtr.Zero)
				{
					address = SHVDN.NativeMemory.ReadAddress(address + 32) + 320;
					byte r = Convert.ToByte(SHVDN.NativeMemory.ReadFloat(address) * 255f);
					byte g = Convert.ToByte(SHVDN.NativeMemory.ReadFloat(address + 4) * 255f);
					byte b = Convert.ToByte(SHVDN.NativeMemory.ReadFloat(address + 8) * 255f);
					byte a = Convert.ToByte(SHVDN.NativeMemory.ReadFloat(address + 12) * 255f);
					return _color = Color.FromArgb(a, r, g, b);
				}
				return _color;
			}
			set
			{
				_color = value;
				IntPtr address = MemoryAddress;
				if (address != IntPtr.Zero)
				{
					address = SHVDN.NativeMemory.ReadAddress(address + 32) + 320;
					SHVDN.NativeMemory.WriteFloat(address, value.R / 255f);
					SHVDN.NativeMemory.WriteFloat(address + 4, value.G / 255f);
					SHVDN.NativeMemory.WriteFloat(address + 8, value.B / 255f);
					SHVDN.NativeMemory.WriteFloat(address + 12, value.A / 255f);
				}
			}
		}

		/// <summary>
		/// Gets or sets the size scaling factor of this <see cref="ParticleEffect"/>
		/// </summary>
		/// <value>
		/// The scale, default = 1.0f; 
		/// To Decrease the size use a value less than 1.0f;
		/// To Increase the size use a value greater than 1.0f;
		/// </value>
		public float Scale
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address != IntPtr.Zero)
				{
					return _scale = SHVDN.NativeMemory.ReadFloat(SHVDN.NativeMemory.ReadAddress(address + 32) + 336);
				}
				return _scale;
			}
			set
			{
				_scale = value;
				IntPtr address = MemoryAddress;
				if (address != IntPtr.Zero)
				{
					SHVDN.NativeMemory.WriteFloat(SHVDN.NativeMemory.ReadAddress(address + 32) + 336, value);
				}
			}
		}

		public float Range
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address != IntPtr.Zero)
				{
					return _range = SHVDN.NativeMemory.ReadFloat(SHVDN.NativeMemory.ReadAddress(address + 32) + 384);
				}
				return _range;
			}
			set
			{
				_range = value;
				Function.Call(Hash.SET_PARTICLE_FX_LOOPED_FAR_CLIP_DIST, Handle, value);
			}
		}

		/// <summary>
		/// Gets or sets which axis of this <see cref="ParticleEffect"/> should be inverted.
		/// </summary>
		public InvertAxis InvertAxis
		{
			get
			{
				return _InvertAxis;
			}
			set
			{
				_InvertAxis = value;
				if (IsActive)
				{
					Stop();
					Start();
				}
			}
		}

		/// <summary>
		/// Modifys parameters of this <see cref="ParticleEffect"/>.
		/// </summary>
		/// <param name="parameterName">Name of the parameter you want to modify, these are stored inside the effect files.</param>
		/// <param name="value">The new value for the parameter.</param>
		public void SetParameter(string parameterName, float value)
		{
			if (IsActive)
			{
				Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, parameterName, value, 0);
			}
		}

		/// <summary>
		/// Gets the name of the asset this effect is stored in.
		/// </summary>
		public string AssetName => _asset.AssetName;

		/// <summary>
		/// Gets the name of this effect.
		/// </summary>
		public string EffectName => _effectName;

		public override string ToString()
		{
			return string.Format("{0}\\{1}", AssetName, EffectName);
		}

		public static implicit operator InputArgument(ParticleEffect effect)
		{
			//we only need to worry about supplying a particle effect to a native, never returning one
			return new InputArgument(effect.Handle);
		}
	}
}