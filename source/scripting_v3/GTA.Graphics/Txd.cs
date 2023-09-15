//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using GTA.Native;

namespace GTA.Graphics
{
	/// <summary>
	/// Represents a struct that contains a texture dictionary <see cref="string"/>.
	/// </summary>
	public readonly struct Txd : IEquatable<Txd>, IScriptStreamingResource
	{
		public Txd(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Gets the name of this texture dictionary.
		/// </summary>
		public string Name
		{
			get;
		}

		/// <summary>
		/// Computes the hash of <see cref="Name"/> in the same way as how the game calculates hashes for texture
		/// dictionaries to store in the global <c>rage::fwTxdStore</c> and as how <see cref="Game.GenerateHash(string)"/>
		/// calculates.
		/// May be useful when you want to get the identifier in the same way as how the game handles texture
		/// dictionaries or when you investigate game memory to see how textures are stored in texture dictionaries
		/// (should be in a <c>rage::pgDictionary&lt;rage::grcTexturePC&gt;</c> instance).
		/// </summary>
		/// <returns>The hash value calculated from <see cref="Name"/>.</returns>
		public int HashName() => Game.GenerateHash(Name);

		/// <summary>
		/// Gets a value indicating whether the textures of this <see cref="Txd"/> are loaded
		/// the global <c>rage::fwTxdStore</c> so they are ready to use.
		/// </summary>
		/// <remarks>
		/// You might want to check if this property returns <see langword="true"/> before calling
		/// <see cref="Request()"/>. If already, you should not want to call <see cref="MarkAsNoLongerNeeded()"/>
		/// after your script finished using the texutre dictionary.
		/// Another SHVDN script will crash the game for access violation during some texture
		/// operation if you call <see cref="MarkAsNoLongerNeeded()"/> on a <see cref="Txd"/>
		/// that is loaded before your script tried to use and if the game tries to unload the textures of the texture
		/// dictionary equivalent to said <see cref="Txd"/>.
		/// </remarks>
		public bool IsLoaded => Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, Name);

		/// <summary>
		/// Attempts to load the textures of this <see cref="Txd"/> into memory.
		/// You do not need to call this method if this <see cref="Txd"/> is loaded by another way,
		/// such as <see cref="PedHeadshot"/>.
		/// </summary>
		/// <remarks>
		/// Allocates a <c>CScriptResource_TextureDictionary</c> instance for the SHVDN runtime.
		/// </remarks>
		public void Request()
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, Name);
		}
		/// <summary>
		/// Attempts to load the textures of this <see cref="Txd"/> into memory for a given period of time.
		/// You do not need to call this method if this <see cref="Txd"/> is loaded by another way,
		/// such as <see cref="PedHeadshot"/>.
		/// </summary>
		/// <param name="timeout">The time (in milliseconds) before giving up trying to load this <see cref="Txd"/>.</param>
		/// <returns><see langword="true"/> if this <see cref="Txd"/> is loaded; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// Allocates a <c>CScriptResource_TextureDictionary</c> instance for the SHVDN runtime.
		/// </remarks>
		public bool Request(int timeout)
		{
			Request();

			int startTime = Environment.TickCount;
			int maxElapsedTime = timeout >= 0 ? timeout : int.MaxValue;

			while (!IsLoaded)
			{
				Script.Yield();
				Request();

				if (Environment.TickCount - startTime >= maxElapsedTime)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Tells the game we have finished using this <see cref="Txd"/> and it can be freed from memory.
		/// You do not need to call this method if this <see cref="Txd"/> was loaded by another way before your script
		/// used this <see cref="Txd"/>, such as <see cref="PedHeadshot"/>.
		/// </summary>
		/// <remarks>
		/// Releases a <c>CScriptResource_TextureDictionary</c> instance from the <c>CGameScriptHandler</c> for
		/// the SHVDN runtime.
		/// </remarks>
		public void MarkAsNoLongerNeeded()
		{
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, Name);
		}

		/// <summary>
		/// Returns <see langword="true"/> if the Jenkins-one-at-a-time (joaat) hash value of <see cref="Name"/>
		/// matches that of <paramref name="other"/>, as the game uses joaat hashes as identifiers of texture dictionaries.
		/// </summary>
		public bool Equals(Txd other)
		{
			return HashName() == other.HashName();
		}
		public override bool Equals(object obj)
		{
			if (obj is Txd txd)
			{
				return Equals(txd);
			}

			return false;
		}

		public static bool operator ==(Txd left, Txd right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Txd left, Txd right)
		{
			return !left.Equals(right);
		}

		public static implicit operator InputArgument(Txd value)
		{
			return new InputArgument(value.Name);
		}
		public static explicit operator Txd(string value)
		{
			return new Txd(value);
		}
		public static explicit operator string(Txd value)
		{
			return value.Name;
		}

		public override int GetHashCode()
		{
			return HashName();
		}

		public override string ToString() => Name;
	}
}
