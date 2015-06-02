#pragma once

#include "Vector3.hpp"

namespace GTA
{
	ref class Entity;

	public ref class Rope sealed
	{
	public:
		Rope(int handle);

		property int Handle
		{
			int get();
		}

		void Delete();
		void ActivatePhysics();
		void ResetLength(bool reset);
		void ForceLength(float length);
		void AttachEntities(Entity ^entityOne, Entity ^entityTwo, float length);
		void AttachEntities(Entity ^entityOne, Math::Vector3 positionOne, Entity ^entityTwo, Math::Vector3 positionTwo, float length);
		void AttachEntity(Entity ^entity);
		void AttachEntity(Entity ^entity, Math::Vector3 position);
		void DetachEntity(Entity ^entity);

		static void LoadTextures();

	private:
		int mHandle;
	};
}
