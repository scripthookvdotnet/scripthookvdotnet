using GTA.Native;

namespace GTA
{
	public class EntityParticleEffect : ParticleEffect
	{
		#region Fields
		private EntityBone _entityBone;
		#endregion
		internal EntityParticleEffect(ParticleEffectAsset asset, string effectName, Entity entity)
			: base(asset, effectName)
		{
			_entityBone = entity.Bones.Core;
		}
		internal EntityParticleEffect(ParticleEffectAsset asset, string effectName, EntityBone entitybone)
			: base(asset, effectName)
		{
			_entityBone = entitybone;
		}

		/// <summary>
		/// Gets or sets the <see cref="GTA.Entity"/> this <see cref="EntityParticleEffect"/> is attached to.
		/// </summary>
		public Entity Entity
		{
			get
			{
				return _entityBone.Owner;
			}
			set
			{
				_entityBone = value.Bones.Core;
				if (IsActive)
				{
					Stop();
					Start();
				}
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="EntityBone"/> that this <see cref="EntityParticleEffect"/> is attached to.
		/// </summary>
		public EntityBone Bone
		{
			get
			{
				return _entityBone;
			}
			set
			{
				_entityBone = value;
				if (IsActive)
				{
					Stop();
					Start();
				}
			}
		}

		/// <summary>
		/// Starts this <see cref="EntityParticleEffect"/>.
		/// </summary>
		/// <returns><c>true</c> if this <see cref="EntityParticleEffect"/> was sucessfully started; otherwise, <c>false</c>.</returns>
		public override bool Start()
		{
			Stop();
			if (!_asset.SetNextCall())
			{
				return false;
			}

			Hash hash = _entityBone.Owner is Ped ? Hash.START_PARTICLE_FX_LOOPED_ON_PED_BONE : Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY_BONE;

			Handle = Function.Call<int>(hash, _effectName, _entityBone.Owner.Handle, Offset.X, Offset.Y, Offset.Z, Rotation.X,
				Rotation.Y, Rotation.Z, _entityBone.Index, _scale, InvertAxis.HasFlag(InvertAxis.X), InvertAxis.HasFlag(InvertAxis.Y),
				InvertAxis.HasFlag(InvertAxis.Z));

			if (IsActive)
			{
				return true;
			}

			Handle = -1;
			return false;
		}

		/// <summary>
		/// Creates a copy of this <see cref="EntityParticleEffect"/> to another <see cref="GTA.Entity"/> to simplify applying the same effect to many Entities.
		/// </summary>
		/// <param name="entity">The <see cref="GTA.Entity"/> to copy to.</param>
		/// <returns>An <see cref="EntityParticleEffect"/> that has all the same properties as this instance, but for a different <see cref="GTA.Entity"/>.</returns>
		public EntityParticleEffect CopyTo(Entity entity)
		{
			var result = new EntityParticleEffect(_asset, _effectName, entity) {
				Bone = entity.Bones[_entityBone.Index],
				Offset = _offset,
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
		/// <summary>
		/// Creates a copy of this <see cref="EntityParticleEffect"/> to another <see cref="GTA.EntityBone"/> to simplify applying the same effect to many Entities.
		/// </summary>
		/// <param name="entityBone">The <see cref="GTA.EntityBone"/> to copy to.</param>
		/// <returns>An <see cref="EntityParticleEffect"/> that has all the same properties as this instance, but for a different <see cref="GTA.EntityBone"/>.</returns>
		public EntityParticleEffect CopyTo(EntityBone entityBone)
		{
			var result = new EntityParticleEffect(_asset, _effectName, entityBone) {
				Offset = _offset,
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
