//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
    public sealed class Checkpoint : PoolObject
    {
        public Checkpoint(int handle) : base(handle)
        {
        }

        /// <summary>
        /// Gets the memory address of this <see cref="Checkpoint"/>.
        /// </summary>
        public IntPtr MemoryAddress => SHVDN.NativeMemory.GetCheckpointAddress(Handle);

        /// <summary>
        /// Gets or sets the position of this <see cref="Checkpoint"/>.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return Vector3.Zero;
                }

                return new Vector3(SHVDN.MemDataMarshal.ReadVector3(address));
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                SHVDN.MemDataMarshal.WriteVector3(address, value.ToInternalFVector3());
            }
        }

        /// <summary>
        /// Gets or sets the position where this <see cref="Checkpoint"/> points to.
        /// </summary>
        public Vector3 TargetPosition
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return Vector3.Zero;
                }

                return new Vector3(SHVDN.MemDataMarshal.ReadVector3(address + 16));
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                SHVDN.MemDataMarshal.WriteVector3(address + 16, value.ToInternalFVector3());
            }
        }

        /// <summary>
        /// Gets or sets the icon drawn in this <see cref="Checkpoint"/>.
        /// </summary>
        public CheckpointIcon Icon
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0;
                }

                return (CheckpointIcon)SHVDN.MemDataMarshal.ReadInt32(address + 56);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                SHVDN.MemDataMarshal.WriteInt32(address + 56, (int)value);
            }
        }

        /// <summary>
        /// Gets or sets a custom icon to be drawn in this <see cref="Checkpoint"/>.
        /// </summary>
        public CheckpointCustomIcon CustomIcon
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0;
                }

                return SHVDN.MemDataMarshal.ReadByte(address + 52);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                SHVDN.MemDataMarshal.WriteByte(address + 52, value);
                SHVDN.MemDataMarshal.WriteInt32(address + 56, 44); // Sets the icon to a custom icon
            }
        }

        /// <summary>
        /// Gets or sets the radius of this <see cref="Checkpoint"/>.
        /// </summary>
        public float Radius
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0.0f;
                }

                return SHVDN.MemDataMarshal.ReadFloat(address + 60);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                SHVDN.MemDataMarshal.WriteFloat(address + 60, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of this <see cref="Checkpoint"/>.
        /// </summary>
        public System.Drawing.Color Color
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return System.Drawing.Color.Transparent;
                }

                int offset = Game.FileVersion >= VersionConstsForGameVersion.v1_0_877_1 ? 84 : 80;
                return System.Drawing.Color.FromArgb(SHVDN.MemDataMarshal.ReadInt32(address + offset));
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                int offset = Game.FileVersion >= VersionConstsForGameVersion.v1_0_877_1 ? 84 : 80;
                SHVDN.MemDataMarshal.WriteInt32(address + offset, value.ToArgb());
            }
        }
        /// <summary>
        /// Gets or sets the color of the icon in this <see cref="Checkpoint"/>.
        /// </summary>
        public System.Drawing.Color IconColor
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return System.Drawing.Color.Transparent;
                }

                int offset = Game.FileVersion >= VersionConstsForGameVersion.v1_0_877_1 ? 88 : 84;
                return System.Drawing.Color.FromArgb(SHVDN.MemDataMarshal.ReadInt32(address + offset));
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                int offset = Game.FileVersion >= VersionConstsForGameVersion.v1_0_877_1 ? 88 : 84;
                SHVDN.MemDataMarshal.WriteInt32(address + offset, value.ToArgb());
            }
        }

        /// <summary>
        /// Gets or sets the radius of the cylinder in this <see cref="Checkpoint"/>.
        /// </summary>
        public float CylinderRadius
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0.0f;
                }

                int offset = Game.FileVersion >= VersionConstsForGameVersion.v1_0_877_1 ? 80 : 76;
                return SHVDN.MemDataMarshal.ReadFloat(address + offset);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                int offset = Game.FileVersion >= VersionConstsForGameVersion.v1_0_877_1 ? 80 : 76;
                SHVDN.MemDataMarshal.WriteFloat(address + offset, value);
            }
        }
        /// <summary>
        /// Gets or sets the near height of the cylinder of this <see cref="Checkpoint"/>.
        /// </summary>
        public float CylinderNearHeight
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0.0f;
                }

                int offset = Game.FileVersion >= VersionConstsForGameVersion.v1_0_877_1 ? 72 : 68;
                return SHVDN.MemDataMarshal.ReadFloat(address + offset);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                int offset = Game.FileVersion >= VersionConstsForGameVersion.v1_0_877_1 ? 72 : 68;
                SHVDN.MemDataMarshal.WriteFloat(address + offset, value);
            }
        }
        /// <summary>
        /// Gets or sets the far height of the cylinder of this <see cref="Checkpoint"/>.
        /// </summary>
        public float CylinderFarHeight
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0.0f;
                }

                int offset = Game.FileVersion >= VersionConstsForGameVersion.v1_0_877_1 ? 76 : 72;
                return SHVDN.MemDataMarshal.ReadFloat(address + offset);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                int offset = Game.FileVersion >= VersionConstsForGameVersion.v1_0_877_1 ? 76 : 72;
                SHVDN.MemDataMarshal.WriteFloat(address + offset, value);
            }
        }

        /// <summary>
        /// Removes this <see cref="Checkpoint"/>.
        /// </summary>
        public override void Delete()
        {
            Function.Call(Hash.DELETE_CHECKPOINT, Handle);
        }

        /// <summary>
        /// Determines if this <see cref="Checkpoint"/> exists.
        /// </summary>
        /// <returns><see langword="true" /> if this <see cref="Checkpoint"/> exists; otherwise, <see langword="false" />.</returns>
        public override bool Exists()
        {
            return Handle != 0 && MemoryAddress != IntPtr.Zero;
        }

        /// <summary>
        /// Determines if an <see cref="object"/> refers to the same checkpoint as this <see cref="Checkpoint"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to check.</param>
        /// <returns><see langword="true" /> if the <paramref name="obj"/> is the same checkpoint as this <see cref="Checkpoint"/>; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Checkpoint checkpoint)
            {
                return Handle == checkpoint.Handle;
            }

            return false;
        }

        /// <summary>
        /// Determines if two <see cref="Checkpoint"/>s refer to the same checkpoint.
        /// </summary>
        /// <param name="left">The left <see cref="Checkpoint"/>.</param>
        /// <param name="right">The right <see cref="Checkpoint"/>.</param>
        /// <returns><see langword="true" /> if <paramref name="left"/> is the same checkpoint as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(Checkpoint left, Checkpoint right)
        {
            return left?.Equals(right) ?? right is null;
        }
        /// <summary>
        /// Determines if two <see cref="Checkpoint"/>s don't refer to the same checkpoint.
        /// </summary>
        /// <param name="left">The left <see cref="Checkpoint"/>.</param>
        /// <param name="right">The right <see cref="Checkpoint"/>.</param>
        /// <returns><see langword="true" /> if <paramref name="left"/> is not the same checkpoint as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(Checkpoint left, Checkpoint right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Converts a <see cref="Checkpoint"/> to a native input argument.
        /// </summary>
        public static implicit operator InputArgument(Checkpoint value)
        {
            return new InputArgument((ulong)value.Handle);
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
