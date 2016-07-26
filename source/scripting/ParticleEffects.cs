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

		public ParticleEffectsAsset(string assetName)
		{
			_assetName = assetName;
		}

		public string AssetName
		{
			get { return _assetName; }
		}

		public bool IsLoaded
		{
			get { return Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, _assetName); }
		}

		public bool StartNonLoopedAtCoord(string effectName, Vector3 pos, Vector3 rot = default(Vector3), float scale = 1.0f,
			InvertAxis invertAxis = InvertAxis.None)
		{
			Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, _assetName);
			return Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, effectName, pos.X, pos.Y, pos.Z, rot.X, rot.Y,
				rot.Z, scale, invertAxis.HasFlag(InvertAxis.X), invertAxis.HasFlag(InvertAxis.Y), invertAxis.HasFlag(InvertAxis.Z));
		}

		public bool StartNonLoopedOnEntity(string effectName, Entity entity, int boneIndex = -1,
			Vector3 off = default(Vector3), Vector3 rot = default(Vector3), float scale = 1.0f,
			InvertAxis invertAxis = InvertAxis.None)
		{
			Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, _assetName);
			return Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_PED_BONE, effectName, entity.Handle, off.X, off.Y, off.Z, rot.X,
				rot.Y, rot.Z, boneIndex, scale, invertAxis.HasFlag(InvertAxis.X), invertAxis.HasFlag(InvertAxis.Y),
				invertAxis.HasFlag(InvertAxis.Z));
		}

		public ParticleEffect StartLoopedAtCoord(string effectName, Vector3 pos, Vector3 rot = default(Vector3),
			float scale = 1.0f, InvertAxis invertAxis = InvertAxis.None)
		{
			Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, _assetName);
			return
				new ParticleEffect(Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, effectName, pos.X, pos.Y, pos.Z, rot.X,
					rot.Y, rot.Z, scale, invertAxis.HasFlag(InvertAxis.X), invertAxis.HasFlag(InvertAxis.Y),
					invertAxis.HasFlag(InvertAxis.Z), true));
		}

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

		static Color NonLoopedColor
		{
			set
			{
				Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_COLOUR, value.R/255f, value.G/255f, value.B/255f);
				Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, value.A / 255f);
			}
		}

		public void Request()
		{
			Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, _assetName);
		}

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
			}

			return true;
		}

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

		public IntPtr MemoryAddress
		{
			get { return MemoryAccess.GetPtfxAddress(Handle); }
		}

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

		public Vector3 Rotation
		{
			set
			{
				//rotation information is stored in a matrix
				Vector3 off = Offset;
				Function.Call(Hash.SET_PARTICLE_FX_LOOPED_OFFSETS, Handle, off.X, off.Y, off.Z, value.X, value.Y, value.Z);
			}
		}

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

		public void Remove()
		{
			Function.Call(Hash.REMOVE_PARTICLE_FX, Handle, true);
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
