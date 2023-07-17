//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
	/// <summary>
	/// Represents a blocking object that blocks <see cref="Ped"/>s using navigation mesh paths in the area the object covers when new <see cref="Ped"/> tasks that use navigation meshes (e.g. <see cref="TaskInvoker.WanderAround()"/>) start for them.
	/// Does not create any dynamic objects or <see cref="Prop"/>s.
	/// </summary>
	/// <remarks>If SHVDN runtime stops working, all the <see cref="NavMeshBlockingObject"/>s created via SHVDN will get removed from the game.</remarks>
	public sealed class NavMeshBlockingObject : PoolObject, INativeValue
	{
		internal NavMeshBlockingObject(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Creates a new <see cref="NavMeshBlockingObject"/>.
		/// </summary>
		/// <param name="position">The origin position.</param>
		/// <param name="size">The size.</param>
		/// <param name="headingDegrees">The heading in degrees.</param>
		/// <param name="flags">The flags that specify what types of paths the blocking object will block new <see cref="Ped"/> tasks that use navigation meshes (e.g. <see cref="TaskInvoker.WanderAround()"/>) from using.</param>
		/// <returns>The new <see cref="NavMeshBlockingObject"/> if successfully created; otherwise, <see langword="null"/>.</returns>
		/// <remarks>The new <see cref="NavMeshBlockingObject"/> won't block existing <see cref="Ped"/> tasks that use navigation mesh paths in the area the blocking object covers from using.</remarks>
		public static NavMeshBlockingObject Create(Vector3 position, Vector3 size, float headingDegrees, NavMeshBlockingObjectFlags flags = NavMeshBlockingObjectFlags.AllPaths)
		{
			const float DEG_2_RAD = (float)(System.Math.PI / 180);
			float headingRadians = headingDegrees * DEG_2_RAD;

			// Set the second last parameter bPermanent to false, which determines whether the blocking object will last outside the lifetime of the calling script (scrThread)
			// If the SHVDN runtime stops working, all the blocking objects created via SHVDN will get deleted (stopping any SHVDN scripts working will not automatically remove any blocking objects)
			int handle = Function.Call<int>(Hash.ADD_NAVMESH_BLOCKING_OBJECT,
				position.X,
				position.Y,
				position.Z,
				size.X,
				size.Y,
				size.Z,
				headingRadians,
				false,
				(int)flags);
			return handle != -1 ? new NavMeshBlockingObject(handle) : null;
		}


		/// <summary>
		/// Removes this <see cref="NavMeshBlockingObject"/>.
		/// </summary>
		public override void Delete()
		{
			Function.Call(Hash.REMOVE_NAVMESH_BLOCKING_OBJECT, Handle);
		}

		/// <summary>
		/// Determines if this <see cref="NavMeshBlockingObject"/> exists.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="NavMeshBlockingObject"/> exists; otherwise, <see langword="false"/></returns>.
		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_NAVMESH_BLOCKING_OBJECT_EXIST, Handle);
		}

		/// <summary>
		/// Updates the <see cref="NavMeshBlockingObject"/>.
		/// </summary>
		/// <param name="position">The origin position.</param>
		/// <param name="size">The size.</param>
		/// <param name="headingDegrees">The heading in degrees.</param>
		/// <param name="flags">The flags that specify what types of paths the blocking object will block new peds task that use navigation meshes from using.</param>
		/// <returns>The new <see cref="NavMeshBlockingObject"/> if successfully created; otherwise, <see langword="null"/>.</returns>
		/// <remarks>The updated <see cref="NavMeshBlockingObject"/> won't affect existing <see cref="Ped"/> tasks.</remarks>
		public void Update(Vector3 position, Vector3 size, float headingDegrees, NavMeshBlockingObjectFlags flags)
		{
			const float DEG_2_RAD = (float)(System.Math.PI / 180);
			Function.Call(Hash.UPDATE_NAVMESH_BLOCKING_OBJECT, Handle, position.X, position.Y, position.Z, size.X, size.Y, size.Z, headingDegrees * DEG_2_RAD, (int)flags);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same navigation mesh blocking object as this <see cref="NavMeshBlockingObject"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true"/> if the <paramref name="obj"/> is the same navigation mesh blocking object as this <see cref="NavMeshBlockingObject"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is NavMeshBlockingObject navMeshBlockingObject)
			{
				return Handle == navMeshBlockingObject.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="NavMeshBlockingObject"/>s refer to the same navigation mesh blocking object.
		/// </summary>
		/// <param name="left">The left <see cref="NavMeshBlockingObject"/>.</param>
		/// <param name="right">The right <see cref="NavMeshBlockingObject"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is the same navigation mesh blocking object as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(NavMeshBlockingObject left, NavMeshBlockingObject right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="NavMeshBlockingObject"/>s don't refer to the navigation mesh blocking object.
		/// </summary>
		/// <param name="left">The left <see cref="NavMeshBlockingObject"/>.</param>
		/// <param name="right">The right <see cref="NavMeshBlockingObject"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is not the navigation mesh blocking object as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(NavMeshBlockingObject left, NavMeshBlockingObject right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts an <see cref="NavMeshBlockingObject"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(NavMeshBlockingObject value)
		{
			return new InputArgument((ulong)value.Handle);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
