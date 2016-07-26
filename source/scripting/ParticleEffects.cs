using System;
using System.Drawing;
using System.Security.Policy;
using GTA;
using GTA.Native;
using GTA.Math;
using Hash = GTA.Native.Hash;

namespace GTA
{
	[Flags]
	public enum InvertAxis
	{
		None = 0,
		X = 1,
		Y = 2,
		Z = 4
	}

	public class ParticleEffectsAsset
	{
		#region Fields
		private readonly string _assetName;
		#endregion
		/// <summary>
		/// Creates a class used for loading <see cref="ParticleEffectsAsset"/>s than can be used to start <see cref="ParticleEffect"/>s from inside the Asset
		/// </summary>
		/// <param name="assetName">The name of the asset file which contains all the <see cref="ParticleEffect"/>s you are wanting to start</param>
		/// <remarks>The files have the extension *.ypt in OpenIV, use the file name withouth the extension for the <paramref name="assetName"/></remarks>
		public ParticleEffectsAsset(string assetName)
		{
			_assetName = assetName;
		}
		/// <summary>
		/// Gets the name of the this <see cref="ParticleEffectsAsset"/> file
		/// </summary>
		public string AssetName
		{
			get { return _assetName; }
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="ParticleEffectsAsset"/> is Loaded
		/// </summary>
		/// <remarks>Use <see cref="Request()"/> or <see cref="Request(int)"/> to load the asset</remarks>
		public bool IsLoaded
		{
			get { return Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, _assetName); }
		}

		/// <summary>
		/// Starts a Particle Effect that runs once at a given position then is destroyed.
		/// </summary>
		/// <param name="effectName">The name of the effect.</param>
		/// <param name="pos">The World position where the effect is.</param>
		/// <param name="rot">What rotation to apply to the effect.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in.</param>
		/// <returns><c>true</c>If the effect was able to start; otherwise, <c>false</c>.</returns>
		public bool StartNonLoopedAtCoord(string effectName, Vector3 pos, Vector3 rot = default(Vector3), float scale = 1.0f,
			InvertAxis invertAxis = InvertAxis.None)
		{
			Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, _assetName);
			return Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, effectName, pos.X, pos.Y, pos.Z, rot.X, rot.Y,
				rot.Z, scale, invertAxis.HasFlag(InvertAxis.X), invertAxis.HasFlag(InvertAxis.Y), invertAxis.HasFlag(InvertAxis.Z));
		}

		/// <summary>
		/// Starts a Particle Effect on an <see cref="Entity"/> that runs once then is destroyed.
		/// </summary>
		/// <param name="effectName">the name of the effect.</param>
		/// <param name="entity">The <see cref="Entity"/> the effect is attached to.</param>
		/// <param name="boneIndex">The <see cref="Entity"/> bone index to attach the effect to, -1 for CORE.</param>
		/// <param name="off">The offset from the bone to attach the effect.</param>
		/// <param name="rot">The rotation, relative to the bone, the effect has.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in. For a car side exahust you may need to flip in the Y Axis</param>
		/// <returns><c>true</c>If the effect was able to start; otherwise, <c>false</c>.</returns>
		public bool StartNonLoopedOnEntity(string effectName, Entity entity, int boneIndex = -1,
			Vector3 off = default(Vector3), Vector3 rot = default(Vector3), float scale = 1.0f,
			InvertAxis invertAxis = InvertAxis.None)
		{
			Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, _assetName);
			return Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_PED_BONE, effectName, entity.Handle, off.X, off.Y, off.Z, rot.X,
				rot.Y, rot.Z, boneIndex, scale, invertAxis.HasFlag(InvertAxis.X), invertAxis.HasFlag(InvertAxis.Y),
				invertAxis.HasFlag(InvertAxis.Z));
		}
		/// <summary>
		/// Starts a <see cref="ParticleEffect"/> that runs looped at a given position.
		/// </summary>
		/// <param name="effectName">The name of the effect.</param>
		/// <param name="pos">The World position where the effect is.</param>
		/// <param name="rot">What rotation to apply to the effect.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in.</param>
		/// <returns>The <see cref="ParticleEffect"/> that can be used to modify and end the effect</returns>
		/// <remarks>If the <see cref="ParticleEffect"/> wasn't able to start, the returned values <see cref="ParticleEffect.Exists()"/>method will return <c>false</c></remarks>
		public ParticleEffect StartLoopedAtCoord(string effectName, Vector3 pos, Vector3 rot = default(Vector3),
			float scale = 1.0f, InvertAxis invertAxis = InvertAxis.None)
		{
			Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, _assetName);
			return
				new ParticleEffect(Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, effectName, pos.X, pos.Y, pos.Z, rot.X,
					rot.Y, rot.Z, scale, invertAxis.HasFlag(InvertAxis.X), invertAxis.HasFlag(InvertAxis.Y),
					invertAxis.HasFlag(InvertAxis.Z), false));
		}
		/// <summary>
		/// Starts a <see cref="ParticleEffect"/> on an <see cref="Entity"/> that runs looped.
		/// </summary>
		/// <param name="effectName">the name of the effect.</param>
		/// <param name="entity">The <see cref="Entity"/> the effect is attached to.</param>
		/// <param name="boneIndex">The <see cref="Entity"/> bone index to attach the effect to, -1 for CORE.</param>
		/// <param name="off">The offset from the bone to attach the effect.</param>
		/// <param name="rot">The rotation, relative to the bone, the effect has.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in. For a car side exahust you may need to flip in the Y Axis</param>
		/// <returns>The <see cref="ParticleEffect"/> that can be used to modify and end the effect</returns>
		/// <remarks>If the <see cref="ParticleEffect"/> wasn't able to start, the returned values <see cref="ParticleEffect.Exists()"/>method will return <c>false</c></remarks>
		public ParticleEffect StartLoopedOnEntity(string effectName, Entity entity, int boneIndex = -1,
			Vector3 off = default(Vector3), Vector3 rot = default(Vector3), float scale = 1.0f,
			InvertAxis invertAxis = InvertAxis.None)
		{
			Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, _assetName);
			Hash hash = entity is Ped ? Hash.START_PARTICLE_FX_LOOPED_ON_PED_BONE : Hash._START_PARTICLE_FX_LOOPED_ON_ENTITY_BONE;
			return new ParticleEffect(Function.Call<int>(hash, effectName, entity.Handle, off.X, off.Y, off.Z, rot.X,
				rot.Y, rot.Z, boneIndex, scale, invertAxis.HasFlag(InvertAxis.X), invertAxis.HasFlag(InvertAxis.Y),
				invertAxis.HasFlag(InvertAxis.Z)));
		}

		/// <summary>
		/// Sets the <see cref="Color"/> for all NonLooped Particle Effects
		/// </summary>
		static Color NonLoopedColor
		{
			set
			{
				Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_COLOUR, value.R/255f, value.G/255f, value.B/255f);
				Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, value.A / 255f);
			}
		}

		/// <summary>
		/// Attempts to load this <see cref="ParticleEffectsAsset"/> into memory so it can be used for starting <see cref="ParticleEffect"/>s.
		/// </summary>
		public void Request()
		{
			Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, _assetName);
		}

		/// <summary>
		/// Attempts to load this <see cref="ParticleEffectsAsset"/> into memory so it can be used for starting <see cref="ParticleEffect"/>s.
		/// </summary>
		/// <param name="timeout">How long in milli-seconds should the game wait while the model hasnt been loaded before giving up</param>
		/// <returns><c>true</c> if the <see cref="ParticleEffectsAsset"/> is Loaded; otherwise, <c>false</c></returns>
		public bool Request(int timeout)
		{
			Request();

			DateTime endtime = timeout >= 0 ? DateTime.UtcNow + new TimeSpan(0, 0, 0, 0, timeout) : DateTime.MaxValue;

			while (!IsLoaded)
			{
				Script.Yield();

				if (DateTime.UtcNow >= endtime)
				{
					return false;
				}
				Request();
			}

			return true;
		}

		/// <summary>
		/// Tells the game we have finished using this <see cref="ParticleEffectsAsset"/> and it can be freed from memory
		/// </summary>
		public void MarkAsNoLongerNeeded()
		{
			Function.Call(Hash._REMOVE_NAMED_PTFX_ASSET, _assetName);
		}

		public override string ToString()
		{
			return _assetName;
		}

		public override int GetHashCode()
		{
			return _assetName.GetHashCode();
		}
	}

	public class ParticleEffect : PoolObject
	{
		internal ParticleEffect(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Gets the memory address where this <see cref="ParticleEffect"/> is located in game memory.
		/// </summary>
		public IntPtr MemoryAddress
		{
			get { return MemoryAccess.GetPtfxAddress(Handle); }
		}


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
					address = MemoryAccess.ReadPtr(address + 32);
					if (address != IntPtr.Zero)
					{
						return MemoryAccess.ReadVector3(address + 144);
					}
				}
				return default(Vector3);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address != IntPtr.Zero)
				{
					address = MemoryAccess.ReadPtr(address + 32);
					if (address != IntPtr.Zero)
					{
						MemoryAccess.WriteVector3(address + 144, value);
					}
				}
			}
		}

		/// <summary>
		/// Sets the rotation of this <see cref="ParticleEffect"/>
		/// </summary>
		public Vector3 Rotation
		{
			set
			{
				//rotation information is stored in a matrix
				Vector3 off = Offset;
				Function.Call(Hash.SET_PARTICLE_FX_LOOPED_OFFSETS, Handle, off.X, off.Y, off.Z, value.X, value.Y, value.Z);
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
					address = MemoryAccess.ReadPtr(address + 32) + 320;
					byte r = Convert.ToByte(MemoryAccess.ReadFloat(address)*255f);
					byte g = Convert.ToByte(MemoryAccess.ReadFloat(address+4)*255f);
					byte b = Convert.ToByte(MemoryAccess.ReadFloat(address+8) * 255f);
					byte a = Convert.ToByte(MemoryAccess.ReadFloat(address+12) * 255f);
					return Color.FromArgb(a, r, g, b);
				}
				return default(Color);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address != IntPtr.Zero)
				{
					address = MemoryAccess.ReadPtr(address + 32) + 320;
					MemoryAccess.WriteFloat(address, value.R / 255f);
					MemoryAccess.WriteFloat(address + 4, value.G / 255f);
					MemoryAccess.WriteFloat(address + 8, value.B / 255f);
					MemoryAccess.WriteFloat(address + 12, value.A / 255f);
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
					return MemoryAccess.ReadFloat(MemoryAccess.ReadPtr(address + 32) + 336);
				}
				return 1.0f;
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address != IntPtr.Zero)
				{
					MemoryAccess.WriteFloat(MemoryAccess.ReadPtr(address + 32) + 336, value);
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
					return MemoryAccess.ReadFloat(MemoryAccess.ReadPtr(address + 32) + 384);
				}
				return 0.0f;
			}
			set { Function.Call(Hash._SET_PARTICLE_FX_LOOPED_RANGE, Handle, value);}
		}

		/// <summary>
		/// Modifys parameters of this <see cref="ParticleEffect"/>
		/// </summary>
		/// <param name="parameterName">Name of the parameter you want to modify, these are stored inside the effect files</param>
		/// <param name="value">The new value for the parameter</param>
		public void SetParameter(string parameterName, float value)
		{
			Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, parameterName, value, 0);
		}

		/// <summary>
		/// Removes this <see cref="ParticleEffect"/>.
		/// </summary>
		public void Remove()
		{
			Function.Call(Hash.REMOVE_PARTICLE_FX, Handle, false);
		}

		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, Handle);
		}

		public static bool Exists(ParticleEffect effect)
		{
			return !ReferenceEquals(effect, null) && effect.Exists();
		}
	}
}
