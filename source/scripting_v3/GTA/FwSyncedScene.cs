//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
    public sealed class FwSyncedScene : INativeValue, IEquatable<FwSyncedScene>
    {
        internal FwSyncedScene(int handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// Gets the handle for native functions for vehicle path nodes.
        /// </summary>
        public int Handle { get; private set; }

        /// <summary>
        /// Gets the native representation of this <see cref="PathNode"/>.
        /// </summary>
        public ulong NativeValue
        {
            get => (ulong)Handle;
            set => Handle = unchecked((int)value);
        }

        /// <summary>
        /// Gets or sets the current phase. The value range is [0.0, 1.0].
        /// </summary>
        /// <remarks>
        /// Changing the phase of a synced scene will update the phases of any objects attached to it.
        /// </remarks>
        public float Phase
        {
            get => Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Handle);
            set => Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Handle, value);
        }

        /// <summary>
        /// Gets or sets the playback rate. The normal rate is 1.0.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Changing the rate of a synced scene will update the playback speed of any objects attached to it.
        /// </para>
        /// <para>
        /// The getter returns 1.0 if the <see cref="FwSyncedScene"/> does not exist.
        /// </para>
        /// </remarks>
        public float Rate
        {
            get => Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, Handle);
            set => Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Handle, value);
        }

        /// <summary>
        /// Gets or sets a value that indicates whether this <see cref="FwSyncedScene"/> should be looped.
        /// </summary>
        public bool IsLooped
        {
            get => Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_LOOPED, Handle);
            set => Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, Handle, value);
        }

        /// <summary>
        /// Gets or sets a value that indicates whether this <see cref="FwSyncedScene"/> should hold the last frame
        /// instead of stopping the <see cref="FwSyncedScene"/> to discard its resources.
        /// </summary>
        /// <remarks>
        /// When <see cref="IsLooped"/> is set to <see langword="true"/>, this property does not have actual effect.
        /// </remarks>
        public bool HoldsLastFrame
        {
            get => Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_HOLD_LAST_FRAME, Handle);
            set => Function.Call(Hash.SET_SYNCHRONIZED_SCENE_HOLD_LAST_FRAME, Handle, value);
        }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="FwSyncedScene"/> exists.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <see cref="FwSyncedScene"/> exists, otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// Despite how the native function `<c>IS_SYNCHRONIZED_SCENE_RUNNING</c>` is named, which this method queries,
        /// This method is named "Exists" as the native function internally calls
        /// `<c>fwAnimDirectorComponentSyncedScene::IsValidSceneId(fwSyncedSceneId sceneId)</c>`.
        /// </remarks>
        public bool Exists() => Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, Handle);

        /// <summary>
        /// Changes the root position and orientation.
        /// </summary>
        /// <param name="position">The position of the scene root in world coordinates</param>
        /// <param name="orientation">The orientation of the scene root in world coordinates.</param>
        /// <param name="rotOrder">The rotation order to apply.</param>
        public void SetOrigin(Vector3 position, Vector3 orientation,
            EulerRotationOrder rotOrder = EulerRotationOrder.YXZ)
        {
            Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Handle, position.X, position.Y, position.Z,
                orientation.X, orientation.Y, orientation.Z, (int)rotOrder);
        }

        /// <summary>
        /// Attaches this <see cref="FwSyncedScene"/> to the matrix (physics capsule) of an <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to attach.</param>
        /// <remarks>
        /// Since this method does not set the origin position and rotation to the zero vector, you should call
        /// <see cref="SetOrigin"/> with desired offsets before or after calling this method.
        /// </remarks>
        public void AttachTo(Entity entity)
        {
            Function.Call(Hash.ATTACH_SYNCHRONIZED_SCENE_TO_ENTITY, Handle, entity, -1);
        }

        /// <summary>
        /// Attaches this <see cref="FwSyncedScene"/> to an <see cref="EntityBone"/>.
        /// </summary>
        /// <param name="bone">The <see cref="EntityBone"/> to attach.</param>
        /// <remarks>
        /// <inheritdoc cref="AttachTo(Entity)" path="/remarks"/>
        /// </remarks>
        public void AttachTo(EntityBone bone)
        {
            Function.Call(Hash.ATTACH_SYNCHRONIZED_SCENE_TO_ENTITY, Handle, bone.Owner, bone.Index);
        }

        /// <summary>
        /// Detaches this <see cref="FwSyncedScene"/> from an <see cref="Entity"/>.
        /// </summary>
        public void Detach()
        {
            Function.Call(Hash.DETACH_SYNCHRONIZED_SCENE, Handle);
        }

        /*
         *  We intentionally omit `TAKE_OWNERSHIP_OF_SYNCHRONIZED_SCENE` as we aren't convinced of how useful
         *  the native is (yet). Synced scenes have script thread IDs when they are created in
         *  `fwAnimDirectorComponentSyncedScene::StartSynchronizedScene(scrThreadId ScriptId)`, which
         *  `CREATE_SYNCHRONIZED_SCENE` internally calls. The only useful use case would be when you want to grab
         *  the ownership of a synced scene from a script that's not running under SHVDN.
         */

        /// <summary>
        /// Creates a common scene origin which can be used to playback synchronised animations across multiple
        /// <see cref="Ped"/>s and objects.
        /// </summary>
        /// <param name="position">The position of the scene root in world coordinates.</param>
        /// <param name="orientation">The orientation of the scene root in world coordinates.</param>
        /// <param name="rotOrder">The rotation order to apply.</param>
        /// <returns>
        /// A <see cref="FwSyncedScene"/> instance if successfully created, otherwise, <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// You should finish loading a <see cref="CrClipDictionary"/> that you are planning to play on a synchronized
        /// scene instance before you create one using this method. The game releases any
        /// <see cref="FwSyncedScene"/>s that do not have any references to anything every frame (including but may
        /// not be limited to; <see cref="Entity"/>s, or <see cref="Camera"/>s), making <see cref="Exists"/> return
        /// <see langword="false"/> on them.
        /// </remarks>
        public static FwSyncedScene Create(Vector3 position, Vector3 orientation,
            EulerRotationOrder rotOrder = EulerRotationOrder.YXZ)
        {
            int id = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position.X, position.Y, position.Z,
                orientation.X, orientation.Y, orientation.Z, (int)rotOrder);

            return id != -1 ? new FwSyncedScene(id) : null;
        }

        /// <summary>
        /// Creates a common scene origin which can be used to playback synchronised animations across multiple
        /// <see cref="Ped"/>s and objects, at the closest <see cref="Prop"/> within a search sphere.
        /// </summary>
        /// <param name="newPos">The center position for the test sphere.</param>
        /// <param name="radius">The radius for the test sphere.</param>
        /// <param name="propModel">The prop model to consider.</param>
        /// <returns>
        /// A <see cref="FwSyncedScene"/> instance if the method finds some <see cref="Prop"/> with
        /// <paramref name="propModel"/> within the search sphere, and it successfully creates a new synchronized scene
        /// at the closest <see cref="Prop"/>, otherwise, <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// <inheritdoc cref="Create" path="/remarks"/>
        /// </remarks>
        public static FwSyncedScene CreateAtMapObject(Vector3 newPos, float radius, Model propModel)
        {
            int id = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE_AT_MAP_OBJECT, newPos.X, newPos.Y, newPos.Z,
                radius, propModel.Hash);

            return id != -1 ? new FwSyncedScene(id) : null;
        }

        /// <summary>
        /// Determines if an <see cref="object"/> refers to the same synced scene as this <see cref="FwSyncedScene"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to check.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is the same synced scene as
        /// this <see cref="FwSyncedScene"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is FwSyncedScene scene)
            {
                return Handle == scene.Handle;
            }

            return false;
        }

        /// <summary>
        /// Determines if an <see cref="FwSyncedScene"/> refers to the same synced scene as
        /// this <see cref="FwSyncedScene"/>.
        /// </summary>
        /// <param name="scene">The <see cref="FwSyncedScene"/> to check.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="scene"/> is the same synced scene as
        /// this <see cref="FwSyncedScene"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(FwSyncedScene scene) => scene != null && (Handle == scene.Handle);

        /// <summary>
        /// Determines if two <see cref="PathNode"/>s refer to the same path node.
        /// </summary>
        /// <param name="left">The left <see cref="FwSyncedScene"/>.</param>
        /// <param name="right">The right <see cref="FwSyncedScene"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is the same path node as <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(FwSyncedScene left, FwSyncedScene right)
        {
            return left is null ? right is null : left.Equals(right);
        }
        /// <summary>
        /// Determines if two <see cref="PathNode"/>s don't refer to the same path node.
        /// </summary>
        /// <param name="left">The left <see cref="PathNode"/>.</param>
        /// <param name="right">The right <see cref="PathNode"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not the same path node as <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(FwSyncedScene left, FwSyncedScene right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Converts a <see cref="FwSyncedScene"/> to a native input argument.
        /// </summary>
        public static implicit operator InputArgument(FwSyncedScene value)
        {
            return new InputArgument((ulong)(value?.Handle ?? 0));
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
