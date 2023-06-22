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
	public sealed class ParticleEffect : PoolObject
	{
		internal ParticleEffect(int handle, string assetName, string effectName, EntityBone entityBone) : base(handle)
		{
			AssetName = assetName;
			EffectName = effectName;
			Bone = entityBone;
		}

		/// <summary>
		/// Gets the memory address where this <see cref="ParticleEffect"/> is located in game memory.
		/// </summary>
		public IntPtr MemoryAddress => SHVDN.NativeMemory.GetPtfxAddress(Handle);

		/// <summary>
		/// Gets the <see cref="GTA.Entity"/> this <see cref="ParticleEffect"/> is attached to or <see langword="null" /> if there is none.
		/// </summary>
		public Entity Entity => Bone?.Owner;

		/// <summary>
		/// Gets the <see cref="EntityBone"/> that this <see cref="ParticleEffect"/> is attached to or <see langword="null" /> if there is none.
		/// </summary>
		public EntityBone Bone
		{
			get;
		}

		/// <summary>
		/// Gets the name of the asset used for this <see cref="ParticleEffect"/>.
		/// </summary>
		public string AssetName
		{
			get;
		}

		/// <summary>
		/// Gets the name of the effect used for this <see cref="ParticleEffect"/>.
		/// </summary>
		public string EffectName
		{
			get;
		}

		/// <summary>
		/// Gets or sets the offset.
		/// If this <see cref="ParticleEffect"/> is attached to an <see cref="Entity"/>, this refers to the offset from the <see cref="Entity"/>;
		/// otherwise, this refers to its position in World coordinates
		/// </summary>
		public Vector3 Offset
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				address = SHVDN.NativeMemory.ReadAddress(address + 32);
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 144));
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				address = SHVDN.NativeMemory.ReadAddress(address + 32);
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteVector3(address + 144, value.ToInternalFVector3());
			}
		}

		/// <summary>
		/// Sets the rotation of this <see cref="ParticleEffect"/>
		/// </summary>
		public Vector3 Rotation
		{
			set
			{
				// Rotation information is stored in a matrix
				Vector3 currentOffset = Offset;
				Function.Call(Hash.SET_PARTICLE_FX_LOOPED_OFFSETS, Handle, currentOffset.X, currentOffset.Y, currentOffset.Z, value.X, value.Y, value.Z);
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
				if (address == IntPtr.Zero)
				{
					return default;
				}

				address = SHVDN.NativeMemory.ReadAddress(address + 32) + 320;
				byte r = Convert.ToByte(SHVDN.NativeMemory.ReadFloat(address) * 255f);
				byte g = Convert.ToByte(SHVDN.NativeMemory.ReadFloat(address + 4) * 255f);
				byte b = Convert.ToByte(SHVDN.NativeMemory.ReadFloat(address + 8) * 255f);
				byte a = Convert.ToByte(SHVDN.NativeMemory.ReadFloat(address + 12) * 255f);
				return Color.FromArgb(a, r, g, b);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				address = SHVDN.NativeMemory.ReadAddress(address + 32) + 320;
				SHVDN.NativeMemory.WriteFloat(address, value.R / 255f);
				SHVDN.NativeMemory.WriteFloat(address + 4, value.G / 255f);
				SHVDN.NativeMemory.WriteFloat(address + 8, value.B / 255f);
				SHVDN.NativeMemory.WriteFloat(address + 12, value.A / 255f);
			}
		}

		/// <summary>
		/// Gets or sets the size scaling factor of this <see cref="ParticleEffect"/>.
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
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(SHVDN.NativeMemory.ReadAddress(address + 32) + 336);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(SHVDN.NativeMemory.ReadAddress(address + 32) + 336, value);
			}
		}

		/// <summary>
		/// Gets or sets the range of this <see cref="ParticleEffect"/>.
		/// </summary>
		public float Range
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(SHVDN.NativeMemory.ReadAddress(address + 32) + 384);
			}
			set => Function.Call(Hash.SET_PARTICLE_FX_LOOPED_FAR_CLIP_DIST, Handle, value);
		}

		/// <summary>
		/// Modifies parameters of this <see cref="ParticleEffect"/>.
		/// </summary>
		/// <param name="parameterName">Name of the parameter you want to modify, these are stored inside the effect files.</param>
		/// <param name="value">The new value for the parameter.</param>
		public void SetParameter(string parameterName, float value)
		{
			Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, parameterName, value, 0);
		}

		/// <summary>
		/// Stops and removes this <see cref="ParticleEffect"/>.
		/// </summary>
		public override void Delete()
		{
			Function.Call(Hash.REMOVE_PARTICLE_FX, Handle, false);
		}

		/// <summary>
		/// Determines if this <see cref="Checkpoint"/> exists.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="Checkpoint"/> exists; otherwise, <see langword="false" />.</returns>
		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, Handle);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same effect as this <see cref="ParticleEffect"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same effect as this <see cref="ParticleEffect"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is ParticleEffect effect)
			{
				return Handle == effect.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="ParticleEffect"/>s refer to the same effect.
		/// </summary>
		/// <param name="left">The left <see cref="ParticleEffect"/>.</param>
		/// <param name="right">The right <see cref="ParticleEffect"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same effect as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(ParticleEffect left, ParticleEffect right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="ParticleEffect"/>s don't refer to the same effect.
		/// </summary>
		/// <param name="left">The left <see cref="ParticleEffect"/>.</param>
		/// <param name="right">The right <see cref="ParticleEffect"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same effect as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(ParticleEffect left, ParticleEffect right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts a <see cref="ParticleEffect"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(ParticleEffect effect)
		{
			//we only need to worry about supplying a particle effect to a native, never returning one
			return new InputArgument((ulong)effect.Handle);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{0}\\{1}", AssetName, EffectName);
		}
	}
}
