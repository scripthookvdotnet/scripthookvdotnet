//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Graphics;
using GTA.Native;

namespace GTA
{
	/// <summary>
	/// Represents a <see cref="Ped"/> headshot.
	/// </summary>
	public class PedHeadshot : INativeValue
	{
		internal PedHeadshot(int handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// Registers a <see cref="Ped"/> to have their headshot taken (64x64 by default).
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to have their headshot taken.</param>
		/// <returns>A <see cref="PedHeadshot"/> instance if sucessfully requested; otherwise, <see langword="null"/>.</returns>
		/// <remarks>
		/// There are 26 slots available for this regular <see cref="PedHeadshot"/> (31 in the game versions prior to v1.0.877.1);
		/// if it's already in use, this request will fail and return <see langword="null"/>.
		/// </remarks>
		public static PedHeadshot Register(Ped ped)
		{
			int handle = Function.Call<int>(Hash.REGISTER_PEDHEADSHOT, ped);
			return handle != 0 ? new PedHeadshot(handle) : null;
		}
		/// <summary>
		/// Registers a <see cref="Ped"/> to have their headshot taken with a high resolution texture (128x128 resolution by default).
		/// Not available in the game versions earlier than v1.0.877.1.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to have their headshot taken.</param>
		/// <returns>
		/// A <see cref="PedHeadshot"/> instance if sucessfully requested; otherwise, <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// There are 7 slot available for transparent <see cref="PedHeadshot"/>; if it's already in use,
		/// this request will fail and return <see langword="null"/>.
		/// </remarks>
		public static PedHeadshot RegisterHiRes(Ped ped)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_877_1_Steam, nameof(PedHeadshot), nameof(RegisterHiRes));

			int handle = Function.Call<int>(Hash.REGISTER_PEDHEADSHOT_HIRES, ped);
			return handle != 0 ? new PedHeadshot(handle) : null;
		}
		/// <summary>
		/// Registers a <see cref="Ped"/> to have their headshot taken with a transparent background (128x128 resolution by default).
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to have their headshot taken with a transparent background.</param>
		/// <returns>
		/// A <see cref="PedHeadshot"/> instance if sucessfully requested; otherwise, <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// There is only one slot available for transparent <see cref="PedHeadshot"/>; if it's already in use,
		/// this request will fail and return <see langword="null"/>.
		/// </remarks>
		public static PedHeadshot RegisterTransparent(Ped ped)
		{
			int handle = Function.Call<int>(Hash.REGISTER_PEDHEADSHOT_TRANSPARENT, ped);
			return handle != 0 ? new PedHeadshot(handle) : null;
		}

		/// <summary>
		/// Gets the handle of this <see cref="PedHeadshot"/>.
		/// </summary>
		public int Handle
		{
			get; private set;
		}

		/// <summary>
		/// Gets the native representation of this <see cref="PedHeadshot"/>.
		/// </summary>
		public ulong NativeValue
		{
			get => (ulong)Handle;
			set => Handle = unchecked((int)value);
		}

		/// <summary>
		/// Returns whether this <see cref="PedHeadshot"/> is a valid one.
		/// At least <see cref="Handle"/> must be reserved for this property to return <see langword="true"/>.
		/// </summary>
		public bool IsValid => Function.Call<bool>(Hash.IS_PEDHEADSHOT_VALID, Handle);
		/// <summary>
		/// Returns whether the texture of this <see cref="PedHeadshot"/> is ready to be used.
		/// At least <see cref="Handle"/> must be reserved for this property to return <see langword="true"/>.
		/// </summary>
		public bool IsReady => Function.Call<bool>(Hash.IS_PEDHEADSHOT_READY, Handle);

		/// <summary>
		/// Gets a <see cref="Txd"/> that represents the texture dictionary name of this <see cref="PedHeadshot"/>.
		/// The texture name is the same as the txd name.
		/// </summary>
		/// <param name="txd">
		/// When this method returns, contains a <see cref="Txd"/> that represents the ped headshot txd name,
		/// if the <see cref="PedHeadshot"/> is ready to be used; otherwise, a <see cref="Txd"/> that contains
		/// the empty string as the name. This parameter is passed uninitialized.
		/// </param>
		/// <returns>
		/// <see langword="true"/> that represents the ped headshot txd name if the <see cref="PedHeadshot"/>
		/// is ready to be used; otherwise, <see langword="false"/>.
		/// </returns>
		public bool TryGetTxd(out Txd txd)
		{
			// Function.Call<string> returns the empty string if the returned pointer point to null in v3 API
			txd = (Txd)Function.Call<string>(Hash.GET_PEDHEADSHOT_TXD_STRING, Handle);
			return string.IsNullOrEmpty(txd.Name);
		}
		/// <summary>
		/// Gets a <see cref="Txd"/> that represents the texture dictionary (txd) name of this <see cref="PedHeadshot"/>
		/// without any status tests. You should not directly use the return value to any methods or native functions
		/// that require texture dictionary names, as this method always returns the txd names for the <see cref="Handle"/>
		/// without ANY status tests.
		/// </summary>
		/// <remarks>
		/// This methods basically returns the same string as <c>pedmugshot_%02d</c>,
		/// where <see cref="Handle"/> is filled in.
		/// </remarks>
		public Txd GetTxdNoStatusCheck() => new($"pedmugshot_{Handle.ToString("D2")}");

		/// <summary>
		/// Releases this <see cref="PedHeadshot"/> associated to a <see cref="Ped"/>.
		/// </summary>
		public void Release() => Function.Call(Hash.UNREGISTER_PEDHEADSHOT, Handle);

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same ped headshot as this <see cref="PedHeadshot"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same ped headshot as this <see cref="PedHeadshot"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is PedHeadshot headshot)
			{
				return Handle == headshot.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="PedHeadshot"/>s refer to the same ped headshot.
		/// </summary>
		/// <param name="left">The left <see cref="PedHeadshot"/>.</param>
		/// <param name="right">The right <see cref="PedHeadshot"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same ped headshot as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(PedHeadshot left, PedHeadshot right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="PedHeadshot"/>s don't refer to the same ped headshot.
		/// </summary>
		/// <param name="left">The left <see cref="PedHeadshot"/>.</param>
		/// <param name="right">The right <see cref="PedHeadshot"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same ped headshot as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(PedHeadshot left, PedHeadshot right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts an <see cref="PedHeadshot"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(PedHeadshot value)
		{
			return new InputArgument((ulong)(value?.Handle ?? 0));
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
