#include "Raycast.hpp"
#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Prop.hpp"
#include "Native.hpp"

namespace GTA
{
	RaycastResult::RaycastResult(int handle)
	{
		int hitsomething = 0, enthandle = 0;
		Native::OutputArgument ^hitCoords = gcnew Native::OutputArgument(), ^unkVec = gcnew Native::OutputArgument();
		this->mResult = Native::Function::Call<int>(Native::Hash::_GET_RAYCAST_RESULT, handle, &hitsomething, hitCoords, unkVec, &enthandle);

		this->mDidHit = hitsomething != 0;
		this->mHitCoords = hitCoords->GetResult<Math::Vector3>();

		if (!Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, enthandle))
		{
			this->mHitEntity = nullptr;
		}
		else if (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_A_PED, enthandle))
		{
			this->mHitEntity = gcnew Ped(enthandle);
		}
		else if (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_A_VEHICLE, enthandle))
		{
			this->mHitEntity = gcnew Vehicle(enthandle);
		}
		else if (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_AN_OBJECT, enthandle))
		{
			this->mHitEntity = gcnew Prop(enthandle);
		}
	}

	int RaycastResult::Result::get()
	{
		return this->mResult;
	}
	bool RaycastResult::DitHitEntity::get()
	{
		return !ReferenceEquals(this->mHitEntity, nullptr);
	}
	bool RaycastResult::DitHitAnything::get()
	{
		return this->mDidHit;
	}
	Entity ^RaycastResult::HitEntity::get()
	{
		return this->mHitEntity;
	}
	Math::Vector3 RaycastResult::HitCoords::get()
	{
		return this->mHitCoords;
	}
}