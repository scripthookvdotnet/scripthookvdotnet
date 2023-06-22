//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// <para>The order in which to apply rotations in world space.</para>
	/// <para>
	/// For instance, <c>Entity.Rotation = new Vector(30f, 45f, 60f);</c>, where the rotation order is the same as <see cref="YXZ"/> in the setter of <c>Entity.Rotation</c>,
	/// will set (almost) the same rotation as <c>Entity.Quaternion = Quaternion.RotationAxis(Vector3.UnitZ, 60f * deg2Rad) * Quaternion.RotationAxis(Vector3.UnitX, 30f * deg2Rad) * Quaternion.RotationAxis(Vector3.UnitY, 45f * deg2Rad);</c>
	/// on condition that <c>deg2Rad</c> is calculated with <c>(System.Math.PI / 180.0)</c>.
	/// </para>
	/// </summary>
	/// <remarks>Applying rotations in the reverse order in local space will get the same result.</remarks>
	public enum EulerRotationOrder
	{
		XYZ = 0,
		XZY = 1,
		YXZ = 2,
		YZX = 3,
		ZXY = 4,
		ZYX = 5,
	}
}
