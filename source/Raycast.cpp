#include "Raycast.hpp"
#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Prop.hpp"
#include "Native.hpp"

namespace GTA
{
	RaycastResult::RaycastResult(int handle) : mHitEntity(nullptr)
	{
		int hitsomething = 0, enthandle = 0;
		Native::OutputArgument ^hitCoords = gcnew Native::OutputArgument(), ^surfaceNormal = gcnew Native::OutputArgument();
		this->mResult = Native::Function::Call<int>(Native::Hash::_GET_RAYCAST_RESULT, handle, &hitsomething, hitCoords, surfaceNormal, &enthandle);

		this->mDidHit = hitsomething != 0;
		this->mHitCoords = hitCoords->GetResult<Math::Vector3>();
		this->mSurfaceNormal = surfaceNormal->GetResult<Math::Vector3>();
		if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, enthandle))
		{
			switch (Native::Function::Call<int>(Native::Hash::GET_ENTITY_TYPE, enthandle))
			{
				case 1:
					this->mHitEntity = gcnew Ped(enthandle);
					break;
				case 2:
					this->mHitEntity = gcnew Vehicle(enthandle);
					break;
				case 3:
					this->mHitEntity = gcnew Prop(enthandle);
					break;
			}
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
	Math::Vector3 RaycastResult::SurfaceNormal::get()
	{
		return this->mSurfaceNormal;
	}
}