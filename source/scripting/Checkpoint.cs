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

		public System.Drawing.Color Color
		{
			set
			{
				Function.Call(Hash.SET_CHECKPOINT_RGBA, Handle, value.R, value.G, value.B, value.A);
			}
		}
		public System.Drawing.Color IconColor
		{
			set
			{
				Function.Call(Hash._SET_CHECKPOINT_ICON_RGBA, Handle, value.R, value.G, value.B, value.A);
			}
		}

		public void SetCylinderHeight(float nearHeight, float farHeight, float radius)
		{
			Function.Call(Hash.SET_CHECKPOINT_CYLINDER_HEIGHT, Handle, nearHeight, farHeight, radius);
		}

		public void Delete()
		{
			Function.Call(Hash.DELETE_CHECKPOINT, Handle);
		}

		public override bool Exists()
		{
			return Handle != 0;
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
