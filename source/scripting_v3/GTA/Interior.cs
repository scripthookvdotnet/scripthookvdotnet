//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	/// <summary>
	/// Represents a static class for general interior stuff.
	/// </summary>
	public static class Interior
	{
		/// <summary>
		/// Culls exterior objects from rendering (g-buffer only) by model hash this frame.
		/// This method is for use in multiplayer apartments which need to cull exterior shell of building etc.
		/// </summary>
		/// <remarks>
		/// The game can cull exterior geometries for up to 32 models at one frame.
		/// If the game already has 32 models to cull object geometries at this frame, the request will be ignored.
		/// </remarks>
		public static void CullExteriorObjectGeometryThisFrame(Model nameHash)
			=> Function.Call(Hash.ENABLE_EXTERIOR_CULL_MODEL_THIS_FRAME, nameHash);

		/// <summary>
		/// <para>
		/// Culls exterior objects from rendering (g-buffer only) by model hash this frame.
		/// This method is for use in multiplayer apartments which need to cull shadows of exterior shell of building etc.
		/// </para>
		/// <para>
		/// Not available in the game versions prior to v1.0.757.2.
		/// </para>
		/// </summary>
		/// <remarks>
		/// The game can cull shadows of exterior objects for up to 32 models at one frame.
		/// If the game already has 32 models to cull object geometries at this frame, the request will be ignored.
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if the method is called in one of the game versions prior to v1.0.757.2.
		/// </exception>
		public static void CullExteriorObjectShadowThisFrame(Model nameHash)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_757_2_Steam, nameof(Interior), nameof(CullExteriorObjectShadowThisFrame));
			Function.Call(Hash.ENABLE_SHADOW_CULL_MODEL_THIS_FRAME, nameHash);
		}

		/// <summary>
		/// Force the game viewport to be registered in the specified interior and room.
		/// </summary>
		/// <param name="interior">The interior proxy.</param>
		/// <param name="roomKey">The room key (name hash), which is defined in a <c>CMloRoomDef</c>.</param>
		public static void ForceRoomForGameViewport(InteriorProxy interior, int roomKey)
			=> Function.Call(Hash.FORCE_ROOM_FOR_GAME_VIEWPORT, interior, roomKey);

		/// <summary>
		/// Associates the game viewport with the specified interior room to ensure that it renders correctly
		/// after a camera cut or warp.
		/// </summary>
		/// <param name="roomName">The room name, which is defined in a <c>CMloRoomDef</c>.</param>
		public static void SetRoomForGameViewport(string roomName)
			=> Function.Call(Hash.SET_ROOM_FOR_GAME_VIEWPORT_BY_NAME, roomName);
		/// <summary>
		/// Associates the game viewport with the specified interior room to ensure that it renders correctly
		/// after a camera cut or warp.
		/// </summary>
		/// <param name="roomKey">The room key (name hash), which is defined in a <c>CMloRoomDef</c>.</param>
		public static void SetRoomForGameViewport(int roomKey)
			=> Function.Call(Hash.SET_ROOM_FOR_GAME_VIEWPORT_BY_KEY, roomKey);

		/// <summary>
		/// Returns the key (name hash) of the interior room currently associated with the game viewport.
		/// </summary>
		/// <returns>
		/// The name hash key of the room the game viewport is in if it is in a <see cref="InteriorProxy"/>;
		/// otherwise, zero.
		/// </returns>
		/// <remarks>
		/// This method gets the name hash from the <see cref="InteriorInstance"/> of <see cref="InteriorProxy"/>
		/// associated with the game viewport. The list of rooms are defined in a <c>CMloRoomDef</c> in a corresponding
		/// ytyp file, and this methods hashes the raw name before returning a value.
		/// </remarks>
		public static int GetRoomKeyForGameViewport()
			=> Function.Call<int>(Hash.GET_ROOM_KEY_FOR_GAME_VIEWPORT);
	}
}
