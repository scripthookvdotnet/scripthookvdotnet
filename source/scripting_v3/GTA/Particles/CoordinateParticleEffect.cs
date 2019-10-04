//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
	public class CoordinateParticleEffect : ParticleEffect
	{
		public CoordinateParticleEffect(ParticleEffectAsset asset, string effectName, Vector3 location) : base(asset, effectName)
		{
			Offset = location;
		}

		/// <summary>
		/// Starts this <see cref="CoordinateParticleEffect"/>.
		/// </summary>
		/// <returns><c>true</c> if this <see cref="CoordinateParticleEffect"/> was sucessfully started; otherwise, <c>false</c>.</returns>
		public override bool Start()
		{
			Stop();
			if (!_asset.SetNextCall())
			{
				return false;
			}

			Handle = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, _effectName, Offset.X, Offset.Y, Offset.Z, Rotation.X,
				Rotation.Y, Rotation.Z, Scale, InvertAxis.HasFlag(InvertAxis.X), InvertAxis.HasFlag(InvertAxis.Y),
				InvertAxis.HasFlag(InvertAxis.Z), false);

			if (IsActive)
			{
				return true;
			}

			Handle = -1;
			return false;
		}

		/// <summary>
		/// Creates a copy of this <see cref="CoordinateParticleEffect"/> to another position to simplify applying the same effect to many positions.
		/// </summary>
		/// <param name="position">The position to copy to.</param>
		/// <returns>A <see cref="CoordinateParticleEffect"/> that has all the same properties as this instance, but for a different position.</returns>
		public CoordinateParticleEffect CopyTo(Vector3 position)
		{
			var result = new CoordinateParticleEffect(_asset, _effectName, position) {
				Rotation = _rotation,
				Color = _color,
				Scale = _scale,
				Range = _range,
				InvertAxis = _InvertAxis
			};
			if (IsActive)
			{
				result.Start();
			}
			return result;
		}
	}
}
