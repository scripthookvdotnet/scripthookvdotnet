using System;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	public enum CheckpointIcon
	{
		Traditional = 0,
		SmallArrow = 5,
		DoubleArrow = 6,
		TripleArrow = 7,
		CycleArrow = 8,
		ArrowInCircle = 10,
		DoubleArrowInCircle = 11,
		TripleArrowInCircle = 12,
		CycleArrowInCircle = 13,
		CheckerInCircle = 14,
		Arrow = 15
	}

	public sealed class Checkpoint : PoolObject, IEquatable<Checkpoint>
	{
		public Checkpoint(int handle) : base(handle)
		{
		}

		public IntPtr MemoryAddress
		{
			get
			{
				return MemoryAccess.GetCheckpointAddress(Handle);
			}
		}

		public Vector3 Position
		{
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return Vector3.Zero;
				}
				return MemoryAccess.ReadVector3(memoryAddress);
			}
			set
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return;
				}
				MemoryAccess.WriteVector3(memoryAddress, value);
			}
		}
		public Vector3 TargetPosition
		{
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return Vector3.Zero;
				}
				return MemoryAccess.ReadVector3(memoryAddress + 16);
			}
			set
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return;
				}
				MemoryAccess.WriteVector3(memoryAddress + 16, value);
			}
		}

		public CheckpointIcon Icon
		{
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return (CheckpointIcon)0;
				}
				return (CheckpointIcon)MemoryAccess.ReadInt(memoryAddress + 56);
			}
			set
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return;
				}
				MemoryAccess.WriteInt(memoryAddress + 56, (int)value);
			}
		}

		public byte Reserved
		{
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return 0;
				}
				return MemoryAccess.ReadByte(memoryAddress + 52);
			}
			set
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return;
				}
				MemoryAccess.WriteByte(memoryAddress + 52, value);
			}
		}

		public float Radius
		{
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return 0.0f;
				}
				return MemoryAccess.ReadFloat(memoryAddress + 60);
			}
			set
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return;
				}
				MemoryAccess.WriteFloat(memoryAddress + 60, value);
			}
		}

		public System.Drawing.Color Color
		{
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return System.Drawing.Color.Transparent;
				}
				return System.Drawing.Color.FromArgb(MemoryAccess.ReadInt(memoryAddress + 80));
			}
			set
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return;
				}
				MemoryAccess.WriteInt(memoryAddress + 80, Color.ToArgb());
			}
		}
		public System.Drawing.Color IconColor
		{
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return System.Drawing.Color.Transparent;
				}
				return System.Drawing.Color.FromArgb(MemoryAccess.ReadInt(memoryAddress + 84));
			}
			set
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return;
				}
				MemoryAccess.WriteInt(memoryAddress + 84, Color.ToArgb());
			}
		}

		public float CylinderNearHeight
		{
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return 0.0f;
				}
				return MemoryAccess.ReadFloat(memoryAddress + 68);
			}
			set
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return;
				}
				MemoryAccess.WriteFloat(memoryAddress + 68, value);
			}
		}
		public float CylinderFarHeight
		{
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return 0.0f;
				}
				return MemoryAccess.ReadFloat(memoryAddress + 72);
			}
			set
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return;
				}
				MemoryAccess.WriteFloat(memoryAddress + 72, value);
			}
		}
		public float CylinderRadius
		{
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return 0.0f;
				}
				return MemoryAccess.ReadFloat(memoryAddress + 76);
			}
			set
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return;
				}
				MemoryAccess.WriteFloat(memoryAddress + 76, value);
			}
		}


		public void Delete()
		{
			Function.Call(Hash.DELETE_CHECKPOINT, Handle);
		}

		public override bool Exists()
		{
			return Handle != 0 && MemoryAddress != IntPtr.Zero;
		}
		public static bool Exists(Checkpoint checkpoint)
		{
			return !ReferenceEquals(checkpoint, null) && checkpoint.Exists();
		}

		public bool Equals(Checkpoint obj)
		{
			return !ReferenceEquals(obj, null) && Handle == obj.Handle;
		}
		public override bool Equals(object obj)
		{
			return !ReferenceEquals(obj, null) && obj.GetType() == GetType() && Equals((Checkpoint)obj);
		}

		public sealed override int GetHashCode()
		{
			return Handle;
		}

		public static bool operator ==(Checkpoint left, Checkpoint right)
		{
			return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);
		}
		public static bool operator !=(Checkpoint left, Checkpoint right)
		{
			return !(left == right);
		}
	}
}
