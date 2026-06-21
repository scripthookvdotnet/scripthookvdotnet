//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Drawing;
using GTA.Math;
using GTA.Native;

namespace GTA
{
    public sealed class Checkpoint : PoolObject
    {
        public Checkpoint(int handle) : base(handle)
        {
        }

        /// <summary>
        /// Creates a <see cref="Checkpoint"/> in the world.
        /// </summary>
        /// <param name="icon">The <see cref="CheckpointIcon"/> to display inside the <see cref="Checkpoint"/>.</param>
        /// <param name="position">The position in the World.</param>
        /// <param name="pointTo">The position in the world where this <see cref="Checkpoint"/> should point.</param>
        /// <param name="radius">The radius of the <see cref="Checkpoint"/>.</param>
        /// <param name="color">The color of the <see cref="Checkpoint"/>.</param>
        /// <remarks>returns <see langword="null" /> if the <see cref="Checkpoint"/> could not be created</remarks>
        public static Checkpoint Create(CheckpointIcon icon, Vector3 position, Vector3 pointTo, float radius, Color color)
        {
            int handle = Function.Call<int>(Hash.CREATE_CHECKPOINT,
                (int)icon,
                position.X,
                position.Y,
                position.Z,
                pointTo.X,
                pointTo.Y,
                pointTo.Z,
                radius,
                color.R,
                color.G,
                color.B,
                color.A,
                0);
            return handle != 0 ? new Checkpoint(handle) : null;
        }
        /// <summary>
        /// Creates a <see cref="Checkpoint"/> in the world.
        /// </summary>
        /// <param name="icon">The <see cref="CheckpointCustomIcon"/> to display inside the <see cref="Checkpoint"/>.</param>
        /// <param name="position">The position in the World.</param>
        /// <param name="pointTo">The position in the world where this <see cref="Checkpoint"/> should point.</param>
        /// <param name="radius">The radius of the <see cref="Checkpoint"/>.</param>
        /// <param name="color">The color of the <see cref="Checkpoint"/>.</param>
        /// <remarks>returns <see langword="null" /> if the <see cref="Checkpoint"/> could not be created</remarks>
        public static Checkpoint Create(CheckpointCustomIcon icon, Vector3 position, Vector3 pointTo, float radius, Color color)
        {
            int handle = Function.Call<int>(Hash.CREATE_CHECKPOINT,
                44,
                position.X,
                position.Y,
                position.Z,
                pointTo.X,
                pointTo.Y,
                pointTo.Z,
                radius,
                color.R,
                color.G,
                color.B,
                color.A,
                icon);
            return handle != 0 ? new Checkpoint(handle) : null;
        }

        /// <summary>
        /// Gets the memory address of this <see cref="Checkpoint"/>.
        /// </summary>
        public IntPtr MemoryAddress => SHVDN.NativeMemory.GetCheckpointAddress(Handle);

        private bool TryGetMemoryAddress(out IntPtr address)
        {
            address = MemoryAddress;
            return address != IntPtr.Zero;
        }

        /// <summary>
        /// Gets or sets the position of this <see cref="Checkpoint"/>.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address))
                    return Vector3.Zero;

                return new Vector3(SHVDN.MemDataMarshal.ReadVector3(address));
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))

                    return;

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
                if (!TryGetMemoryAddress(out IntPtr address))
                    return Vector3.Zero;

                return new Vector3(SHVDN.MemDataMarshal.ReadVector3(address + 16));
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))

                    return;

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
                if (!TryGetMemoryAddress(out IntPtr address))
                    return 0;

                return (CheckpointIcon)SHVDN.MemDataMarshal.ReadInt32(address + 56);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))

                    return;

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
                if (!TryGetMemoryAddress(out IntPtr address))
                    return 0;

                return SHVDN.MemDataMarshal.ReadByte(address + 52);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))

                    return;

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
                if (!TryGetMemoryAddress(out IntPtr address))
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + 60);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))

                    return;

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
                if (!TryGetMemoryAddress(out IntPtr address))
                    return System.Drawing.Color.Transparent;

                int offset = Game.FileVersion >= ExeVersionConsts.v1_0_877_1 ? 84 : 80;
                return System.Drawing.Color.FromArgb(SHVDN.MemDataMarshal.ReadInt32(address + offset));
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))

                    return;

                int offset = Game.FileVersion >= ExeVersionConsts.v1_0_877_1 ? 84 : 80;
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
                if (!TryGetMemoryAddress(out IntPtr address))
                    return System.Drawing.Color.Transparent;

                int offset = Game.FileVersion >= ExeVersionConsts.v1_0_877_1 ? 88 : 84;
                return System.Drawing.Color.FromArgb(SHVDN.MemDataMarshal.ReadInt32(address + offset));
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))

                    return;

                int offset = Game.FileVersion >= ExeVersionConsts.v1_0_877_1 ? 88 : 84;
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
                if (!TryGetMemoryAddress(out IntPtr address))
                    return 0.0f;

                int offset = Game.FileVersion >= ExeVersionConsts.v1_0_877_1 ? 80 : 76;
                return SHVDN.MemDataMarshal.ReadFloat(address + offset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))

                    return;

                int offset = Game.FileVersion >= ExeVersionConsts.v1_0_877_1 ? 80 : 76;
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
                if (!TryGetMemoryAddress(out IntPtr address))
                    return 0.0f;

                int offset = Game.FileVersion >= ExeVersionConsts.v1_0_877_1 ? 72 : 68;
                return SHVDN.MemDataMarshal.ReadFloat(address + offset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))

                    return;

                int offset = Game.FileVersion >= ExeVersionConsts.v1_0_877_1 ? 72 : 68;
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
                if (!TryGetMemoryAddress(out IntPtr address))
                    return 0.0f;

                int offset = Game.FileVersion >= ExeVersionConsts.v1_0_877_1 ? 76 : 72;
                return SHVDN.MemDataMarshal.ReadFloat(address + offset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))

                    return;

                int offset = Game.FileVersion >= ExeVersionConsts.v1_0_877_1 ? 76 : 72;
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
