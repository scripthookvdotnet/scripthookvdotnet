#pragma once

#include "Vector3.hpp"
#include "Interface.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Ped;
	ref class Vehicle;
	ref class Entity;
	value class Model;
	#pragma endregion
	public enum class ParachuteTint
	{
		None = -1,
		Rainbow = 0,
		Red = 1,
		SeasideStripes = 2,
		WidowMaker = 3,
		Patriot = 4,
		Blue = 5,
		Black = 6,
		Hornet = 7,
		AirFocce = 8,
		Desert = 9,
		Shadow = 10,
		HighAltitude = 11,
		Airbone = 12,
		Sunrise = 13,
	};
	public ref class Player sealed : System::IEquatable<Player ^>, IHandleable
	{
	public:
		Player(int handle);

		virtual property int Handle
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
		property int MaxArmor
		{
			int get();
			void set(int value);
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
		property Math::Vector3 WantedCenterPosition
		{
			Math::Vector3 get();
			void set(Math::Vector3 value);
		}
		property ParachuteTint PrimaryParachuteTint
		{
			ParachuteTint get();
			void set(ParachuteTint value);
		}
		property ParachuteTint ReserveParachuteTint
		{
			ParachuteTint get();
			void set(ParachuteTint value);
		}

		bool ChangeModel(Model model);
		void RefillSpecialAbility();

		void DisableFiringThisFrame();
		bool IsTargetting(Entity ^entity);
		void SetRunSpeedMultThisFrame(float mult);
		void SetSwimSpeedMultThisFrame(float mult);
		Entity ^GetTargetedEntity();
		void SetMayOnlyEnterThisVehicleThisFrame(Vehicle ^vehicle);
		void SetMayNotEnterAnyVehicleThisFrame();
		void SetExplosiveAmmoThisFrame();
		void SetExplosiveMeleeThisFrame();
		void SetSuperJumpThisFrame();
		void SetFireAmmoThisFrame();

		virtual bool Equals(System::Object ^obj) override;
		virtual bool Equals(Player ^player);

		virtual inline bool Exists() = IHandleable::Exists
		{
			// IHandleable forces us to implement this unfortunately,
			// so we'll implement it explicitly and return true
			return true;
		}

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