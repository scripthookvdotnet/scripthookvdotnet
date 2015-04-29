#pragma once

#include "Model.hpp"
#include "Vector3.hpp"

namespace GTA
{
	public ref class Entity abstract
	{
	public:
		Entity(int id);

		property int ID
		{
			int get();
		}
		property Math::Vector3 Position
		{
			Math::Vector3 get();
			void set(Math::Vector3 value);
		}
		property float Heading
		{
			float get();
			void set(float value);
		}
		property Math::Vector3 Rotation
		{
			Math::Vector3 get();
		}
		property Math::Vector3 Velocity
		{
			Math::Vector3 get();
			void set(Math::Vector3 value);
		}
		property int Health
		{
			int get();
			void set(int value);
		}
		property GTA::Model Model
		{
			GTA::Model get();
		}
		property bool IsDead
		{
			bool get();
		}
		property bool IsAlive
		{
			bool get();
		}
		property bool IsInvincible
		{
			void set(bool value);
		}
		property bool IsVisible
		{
			bool get();
			void set(bool value);
		}
		property bool IsOccluded
		{
			bool get();
		}
		property bool IsOnScreen
		{
			bool get();
		}
		property bool IsUpright
		{
			bool get();
		}
		property bool IsUpsideDown
		{
			bool get();
		}
		property bool IsInAir
		{
			bool get();
		}
		property bool IsInWater
		{
			bool get();
		}
		property bool IsOnFire
		{
			bool get();
			void set(bool value);
		}
		property bool IsRequiredForMission
		{
			bool get();
			void set(bool value);
		}

		bool Exists();
		static bool Exists(Entity ^entity);

	private:
		int mID;
	};
}