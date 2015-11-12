#pragma once

namespace GTA
{
	#pragma region Forward Declarations
	ref class Ped;
	ref class Vehicle;
	ref class Entity;
	#pragma endregion

	public ref class Player sealed
	{
	public:
		Player(int handle);

		property int Handle
		{
			int get();
		}
		property bool CanControlCharacter
		{
			bool get();
			void set(bool value);
		}
		property bool CanControlRagdoll
		{
			void set(bool value);
		}
		property bool CanStartMission
		{
			bool get();
		}
		property bool CanUseCover
		{
			void set(bool value);
		}
		property Ped ^Character
		{
			Ped ^get();
		}
		property System::Drawing::Color Color
		{
			System::Drawing::Color get();
		}
		property bool IgnoredByEveryone
		{
			void set(bool value);
		}
		property bool IgnoredByPolice
		{
			void set(bool value);
		}
		property bool IsAiming
		{
			bool get();
		}
		property bool IsAlive
		{
			bool get();
		}
		property bool IsClimbing
		{
			bool get();
		}
		property bool IsDead
		{
			bool get();
		}
		property bool IsInvincible
		{
			bool get();
			void set(bool value);
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
		property bool IsTargettingAnything
		{
			bool get();
		}
		property Vehicle ^LastVehicle
		{
			Vehicle ^get();
		}
		property int Money
		{
			int get();
			void set(int value);
		}
		property System::String ^Name
		{
			System::String ^get();
		}
		property int RemainingSprintTime
		{
			int get();
		}
		property int RemainingUnderwaterTime
		{
			int get();
		}
		property int WantedLevel
		{
			int get();
			void set(int value);
		}

		bool IsTargetting(Entity ^entity);
		Entity ^GetTargetedEntity();

		virtual bool Equals(Player ^player);

		virtual inline int GetHashCode() override
		{
			return Handle;
		}
		static inline bool operator==(Player ^left, Player ^right)
		{
			if (Object::ReferenceEquals(left, nullptr))
			{
				return Object::ReferenceEquals(right, nullptr);
			}

			return left->Equals(right);
		}
		static inline bool operator!=(Player ^left, Player ^right)
		{
			return !operator==(left, right);
		}

	private:
		int _handle;
		Ped ^_ped;
	};
}