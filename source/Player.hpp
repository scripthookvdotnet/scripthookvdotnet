#pragma once

namespace GTA
{
	ref class Ped;
	ref class Vehicle;
	ref class Entity;

	public ref class Player sealed
	{
	public:
		Player(int id);

		property int ID
		{
			int get();
		}
		property System::String ^Name
		{
			System::String ^get();
		}
		property System::Drawing::Color Color
		{
			System::Drawing::Color get();
		}
		property Ped ^Character
		{
			Ped ^get();
		}
		property int WantedLevel
		{
			int get();
			void set(int value);
		}
		property int RemainingSprintTime
		{
			int get();
		}
		property int RemainingUnderwaterTime
		{
			int get();
		}
		property bool IsDead
		{
			bool get();
		}
		property bool IsAlive
		{
			bool get();
		}
		property bool IsOnMission
		{
			bool get();
		}
		property bool IsPlaying
		{
			bool get();
		}
		property bool IsPressingHorn
		{
			bool get();
		}
		property bool IsRidingTrain
		{
			bool get();
		}
		property bool IsClimbing
		{
			bool get();
		}
		property Vehicle ^LastVehicle
		{
			Vehicle ^get();
		}

		property bool IgnoredByEveryone
		{
			void set(bool value);
		}
		property bool CanUseCover
		{
			void set(bool value);
		}
		property bool CanControlRagdoll
		{
			void set(bool value);
		}
		property bool CanControlCharacter
		{
			bool get();
			void set(bool value);
		}

		property bool IsTargettingAnything
		{
			bool get();
		}
		bool IsTargetting(Entity ^entity);
		Entity ^GetTargetedEntity();

		static void SetExplosiveAmmoThisFrame(Player player);
		static void SetFireAmmoThisFrame(Player player);
		static void SetExplosiveMeleeThisFrame(Player player);
		static void SetSuperJumpThisFrame(Player player);

	private:
		int mID;
		Ped ^mPed;
	};
}