//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    /// <summary>
    /// An enumeration of all the possible ped types.
    /// </summary>
    public enum PedType
    {
        /// <remarks>
        /// In the game's codebase, this value is only used by `<c>CSelectionWheel::TriggerFadeInEffect</c>`
        /// (the class manages the player selection wheel), `<c>CNetObjPed::GetPedScriptGameStateData</c>`,
        /// `<c>ReplayBufferMarkerMgr::AddMarkerInternal</c>`, and 3 functions of `<c>CEventPlayerDeath</c>`.
        /// </remarks>
        Invalid = -1,
        /// <summary>
        /// The <see cref="PedType"/> for Michael.
        /// </summary>
        Player0,
        /// <summary>
        /// The <see cref="PedType"/> for Franklin.
        /// </summary>
        Player1,
        /// <summary>
        /// The <see cref="PedType"/> for players controlled over the network (not by the local machine)
        /// </summary>
        /// <remarks>
        /// Although this is only relevant when the game is online, player <see cref="Ped"/>s will have this type if
        /// the static network session instance is not null and is active, and the ped type of the player ped is not
        /// the animal ped type in `<c>CPlayerInfo::SetPlayerPed</c>`.
        /// </remarks>
        NetworkPlayer,
        /// <summary>
        /// The <see cref="PedType"/> for Trevor.
        /// </summary>
        Player2,
        /// <summary>
        /// The civilian male <see cref="PedType"/>.
        /// </summary>
        CivMale,
        /// <summary>
        /// The civilian female <see cref="PedType"/>.
        /// </summary>
        CivFemale,
        Cop,
        GangAlbanian,
        GangBiker1,
        GangBiker2,
        GangItalian,
        GangRussian,
        GangRussian2,
        GangIrish,
        GangJamaican,
        GangAfricanAmerican,
        GangKorean,
        GangChineseJapanese,
        GangPuertoRican,
        Dealer,
        Medic,
        Fire,
        Criminal,
        Bum,
        Prostitute,
        Special,
        Mission,
        Swat,
        Animal,
        Army
    }
}
