//
// Copyright (C) 2015 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum EntityAnimationType
	{
		/// <summary>
		/// Includes scripted animations, such as anims initated by <see cref="TaskInvoker.PlayAnimation(CrClipAsset)"/>,
		/// <c>TASK_SCRIPTED_ANIMATION</c>, and <c>PLAY_ENTITY_ANIM</c>.
		/// </summary>
		Script = 1,
		/// <summary>
		/// Include synced scene animations (<c>TASK_SYNCHRONIZED_SCENE</c> and <c>PLAY_SYNCHRONIZED_ENTITY_ANIM</c>).
		/// </summary>
		SyncedScene = 2,
		/// <summary>
		/// Includes scripted and synchronized scene anims by default.
		/// </summary>
		Default = Script | SyncedScene
	}
}
