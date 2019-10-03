using GTA.Math;
using GTA.Native;
using System;
using System.Drawing;

namespace GTA
{
	public class ParticleEffectAsset
	{
		#region Fields
		private readonly string _assetName;
		#endregion

		/// <summary>
		/// Creates a class used for loading <see cref="ParticleEffectAsset"/>s than can be used to start <see cref="ParticleEffect"/>s from inside the Asset
		/// </summary>
		/// <param name="assetName">The name of the asset file which contains all the <see cref="ParticleEffect"/>s you are wanting to start</param>
		/// <remarks>The files have the extension *.ypt in OpenIV, use the file name withouth the extension for the <paramref name="assetName"/></remarks>
		public ParticleEffectAsset(string assetName)
		{
			_assetName = assetName;
		}

		/// <summary>
		/// Gets the name of the this <see cref="ParticleEffectAsset"/> file
		/// </summary>
		public string AssetName => _assetName;
		/// <summary>
		/// Gets a value indicating whether this <see cref="ParticleEffectAsset"/> is Loaded
		/// </summary>
		/// <remarks>Use <see cref="Request()"/> or <see cref="Request(int)"/> to load the asset</remarks>
		public bool IsLoaded => Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, _assetName);

		/// <summary>
		/// Starts a Particle Effect that runs once at a given position then is destroyed.
		/// </summary>
		/// <param name="effectName">The name of the effect.</param>
		/// <param name="pos">The World position where the effect is.</param>
		/// <param name="rot">What rotation to apply to the effect.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in.</param>
		/// <returns><c>true</c>If the effect was able to start; otherwise, <c>false</c>.</returns>
		public bool StartNonLoopedAtCoord(string effectName, Vector3 pos, Vector3 rot = default(Vector3), float scale = 1.0f, InvertAxis invertAxis = InvertAxis.None)
		{
			if (!SetNextCall())
			{
				return false;
			}
			Function.Call(Hash.USE_PARTICLE_FX_ASSET, _assetName);
			return Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, effectName, pos.X, pos.Y, pos.Z, rot.X, rot.Y,
				rot.Z, scale, invertAxis.HasFlag(InvertAxis.X), invertAxis.HasFlag(InvertAxis.Y), invertAxis.HasFlag(InvertAxis.Z));
		}

		/// <summary>
		/// Starts a Particle Effect on an <see cref="Entity"/> that runs once then is destroyed.
		/// </summary>
		/// <param name="effectName">the name of the effect.</param>
		/// <param name="entity">The <see cref="Entity"/> the effect is attached to.</param>
		/// <param name="off">The offset from the <paramref name="entity"/> to attach the effect.</param>
		/// <param name="rot">The rotation, relative to the <paramref name="entity"/>, the effect has.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in. For a car side exahust you may need to flip in the Y Axis</param>
		/// <returns><c>true</c>If the effect was able to start; otherwise, <c>false</c>.</returns>
		public bool StartNonLoopedOnEntity(string effectName, Entity entity, Vector3 off = default(Vector3), Vector3 rot = default(Vector3), float scale = 1.0f, InvertAxis invertAxis = InvertAxis.None)
		{
			if (!SetNextCall())
			{
				return false;
			}
			Function.Call(Hash.USE_PARTICLE_FX_ASSET, _assetName);
			return Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_PED_BONE, effectName, entity.Handle, off.X, off.Y, off.Z, rot.X,
				rot.Y, rot.Z, -1, scale, invertAxis.HasFlag(InvertAxis.X), invertAxis.HasFlag(InvertAxis.Y),
				invertAxis.HasFlag(InvertAxis.Z));
		}

		/// <summary>
		/// Starts a Particle Effect on an <see cref="EntityBone"/> that runs once then is destroyed.
		/// </summary>
		/// <param name="effectName">the name of the effect.</param>
		/// <param name="entityBone">The <see cref="EntityBone"/> the effect is attached to.</param>
		/// <param name="off">The offset from the <paramref name="entityBone"/> to attach the effect.</param>
		/// <param name="rot">The rotation, relative to the <paramref name="entityBone"/>, the effect has.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in. For a car side exahust you may need to flip in the Y Axis</param>
		/// <returns><c>true</c>If the effect was able to start; otherwise, <c>false</c>.</returns>
		public bool StartNonLoopedOnEntity(string effectName, EntityBone entityBone,
			Vector3 off = default(Vector3), Vector3 rot = default(Vector3), float scale = 1.0f,
			InvertAxis invertAxis = InvertAxis.None)
		{
			if (!SetNextCall())
			{
				return false;
			}
			Function.Call(Hash.USE_PARTICLE_FX_ASSET, _assetName);
			return Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_PED_BONE, effectName, entityBone.Owner.Handle, off.X, off.Y, off.Z, rot.X,
				rot.Y, rot.Z, entityBone, scale, invertAxis.HasFlag(InvertAxis.X), invertAxis.HasFlag(InvertAxis.Y),
				invertAxis.HasFlag(InvertAxis.Z));
		}

		/// <summary>
		/// Creates a <see cref="ParticleEffect"/> on an <see cref="Entity"/> that runs looped.
		/// </summary>
		/// <param name="effectName">The name of the Effect</param>
		/// <param name="entity">The <see cref="Entity"/> the effect is attached to.</param>
		/// <param name="off">The offset from the <paramref name="entity"/> to attach the effect.</param>
		/// <param name="rot">The rotation, relative to the <paramref name="entity"/>, the effect has.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in. For a car side exahust you may need to flip in the Y Axis.</param>
		/// <param name="startNow">if <c>true</c> attempt to start this effect now; otherwise, the effect will start when <see cref="ParticleEffect.Start"/> is called.</param>
		/// <returns>The <see cref="EntityParticleEffect"/> represented by this that can be used to start/stop/modify this effect</returns>
		public EntityParticleEffect CreateEffectOnEntity(string effectName, Entity entity,
			Vector3 off = default(Vector3), Vector3 rot = default(Vector3), float scale = 1.0f,
			InvertAxis invertAxis = InvertAxis.None, bool startNow = false)
		{
			var result = new EntityParticleEffect(this, effectName, entity) {
				Offset = off,
				Rotation = rot,
				Scale = scale,
				InvertAxis = invertAxis
			};
			if (startNow)
			{
				result.Start();
			}
			return result;
		}
		/// <summary>
		/// Creates a <see cref="ParticleEffect"/> on an <see cref="EntityBone"/> that runs looped.
		/// </summary>
		/// <param name="effectName">The name of the Effect</param>
		/// <param name="entityBone">The <see cref="EntityBone"/> the effect is attached to.</param>
		/// <param name="off">The offset from the <paramref name="entityBone"/> to attach the effect.</param>
		/// <param name="rot">The rotation, relative to the <paramref name="entityBone"/>, the effect has.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in. For a car side exahust you may need to flip in the Y Axis.</param>
		/// <param name="startNow">if <c>true</c> attempt to start this effect now; otherwise, the effect will start when <see cref="ParticleEffect.Start"/> is called.</param>
		/// <returns>The <see cref="EntityParticleEffect"/> represented by this that can be used to start/stop/modify this effect</returns>
		public EntityParticleEffect CreateEffectOnEntity(string effectName, EntityBone entityBone,
			Vector3 off = default(Vector3), Vector3 rot = default(Vector3), float scale = 1.0f,
			InvertAxis invertAxis = InvertAxis.None, bool startNow = false)
		{
			var result = new EntityParticleEffect(this, effectName, entityBone) {
				Offset = off,
				Rotation = rot,
				Scale = scale,
				InvertAxis = invertAxis
			};
			if (startNow)
			{
				result.Start();
			}
			return result;
		}

		/// <summary>
		/// Creates a <see cref="ParticleEffect"/> at a position that runs looped.
		/// </summary>
		/// <param name="effectName">The name of the effect.</param>
		/// <param name="pos">The World position where the effect is.</param>
		/// <param name="rot">What rotation to apply to the effect.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in.</param>
		/// <param name="startNow">if <c>true</c> attempt to start this effect now; otherwise, the effect will start when <see cref="ParticleEffect.Start"/> is called.</param>
		/// <returns>The <see cref="CoordinateParticleEffect"/> represented by this that can be used to start/stop/modify this effect</returns>
		public CoordinateParticleEffect CreateEffectAtCoord(string effectName, Vector3 pos, Vector3 rot = default(Vector3), float scale = 1.0f,
			InvertAxis invertAxis = InvertAxis.None, bool startNow = false)
		{
			var result = new CoordinateParticleEffect(this, effectName, pos) {
				Rotation = rot,
				Scale = scale,
				InvertAxis = invertAxis
			};
			if (startNow)
			{
				result.Start();
			}
			return result;
		}

		/// <summary>
		/// Sets the <see cref="Color"/> for all NonLooped Particle Effects
		/// </summary>
		static Color NonLoopedColor
		{
			set
			{
				Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_COLOUR, value.R / 255f, value.G / 255f, value.B / 255f);
				Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, value.A / 255f);
			}
		}

		/// <summary>
		/// Attempts to load this <see cref="ParticleEffectAsset"/> into memory so it can be used for starting <see cref="ParticleEffect"/>s.
		/// </summary>
		public void Request()
		{
			Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, _assetName);
		}

		/// <summary>
		/// Attempts to load this <see cref="ParticleEffectAsset"/> into memory so it can be used for starting <see cref="ParticleEffect"/>s.
		/// </summary>
		/// <param name="timeout">How long in milliseconds should the game wait while the model hasn't been loaded before giving up</param>
		/// <returns><c>true</c> if the <see cref="ParticleEffectAsset"/> is Loaded; otherwise, <c>false</c></returns>
		public bool Request(int timeout)
		{
			Request();

			DateTime endtime = timeout >= 0 ? DateTime.UtcNow + new TimeSpan(0, 0, 0, 0, timeout) : DateTime.MaxValue;

			while (!IsLoaded)
			{
				Script.Yield();

				if (DateTime.UtcNow >= endtime)
				{
					return false;
				}
				Request();
			}

			return true;
		}

		internal bool SetNextCall()
		{
			Request();
			if (IsLoaded)
			{
				Function.Call(Hash.USE_PARTICLE_FX_ASSET, _assetName);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Tells the game we have finished using this <see cref="ParticleEffectAsset"/> and it can be freed from memory
		/// </summary>
		public void MarkAsNoLongerNeeded()
		{
			Function.Call(Hash._REMOVE_NAMED_PTFX_ASSET, _assetName);
		}

		public override string ToString()
		{
			return _assetName;
		}

		public override int GetHashCode()
		{
			return _assetName.GetHashCode();
		}

		public static implicit operator InputArgument(ParticleEffectAsset asset)
		{
			return new InputArgument(asset._assetName);
		}
	}
}
