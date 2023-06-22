//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum ScriptTaskStatus
	{
		/// <summary>
		/// The task is issued to the <see cref="Ped"/> as a primary task but they're waiting for events or other tasks preventing them from performing it to end.
		/// Eg. When the task is issued while the <see cref="Ped"/> is ragdolling or reacting a shocking event such as a agitating event,
		/// or when an animation task is issued while they are being dragged from their <see cref="Vehicle"/> (and the animation task is primary one).
		/// </summary>
		WaitingToStart = 0,
		/// <summary>
		/// Task is currently being performed by the <see cref="Ped"/> as a primary task.
		/// </summary>
		Performing = 1,
		/// <summary>
		/// The task is dormant because it's temporarily interrupted by some events or other tasks.
		/// Eg. When the <see cref="Ped"/> is srumbling since another <see cref="Entity"/> bumped them after the task had started at all.
		/// </summary>
		Dormant = 2,
		/// <summary>
		/// The script task has nothing to do.
		/// <see cref="ScriptTaskNameHash.Invalid"/> basically goes to this status.
		/// </summary>
		Vacant = 3,
		/// <summary>
		/// The task has been done or not performed yet as a primary task.
		/// Strictly, <see cref="Ped.GetScriptTaskStatus(ScriptTaskNameHash)"/> returns this value
		/// if the specified hash does not match the current one and the specified hash is not <see cref="ScriptTaskNameHash.Any"/>.
		/// </summary>
		Finished = 7,
	}
}
