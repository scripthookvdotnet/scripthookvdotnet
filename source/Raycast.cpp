#include "Raycast.hpp"
#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Prop.hpp"
#include "Native.hpp"

namespace GTA
{
	RayCastResult::RayCastResult(int Handle)
	{
		this->mHandle = Handle;
		int hitsomething, enthandle;
		Native::OutputArgument ^Hit = gcnew Native::OutputArgument(), ^UnkVec = gcnew Native::OutputArgument();
		this->mStatus = Native::Function::Call<int>(Native::Hash::_0x3D87450E15D98694, this->mHandle, &hitsomething, Hit, UnkVec, &enthandle);
		this->mHitCoord = Hit->GetResult<Vector3>();
		this->mUnk = UnkVec->GetResult<Vector3>();
		this->mIntersected = hitsomething;
		if (!Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, enthandle))
		{
			this->mHit = nullptr;
		}
		else if (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_A_PED, enthandle))
		{
			this->mHit = gcnew Ped(enthandle);
		}
		else if (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_A_VEHICLE, enthandle))
		{
			this->mHit = gcnew Vehicle(enthandle);
		}
		else if (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_AN_OBJECT, enthandle))
		{
			this->mHit = gcnew Prop(enthandle);
		}

	}
	int RayCastResult::Handle::get()
	{
		return this->mHandle;
	}
	bool RayCastResult::DidHitEntity::get()
	{
		return !ReferenceEquals(this->mHit, nullptr);
	}
	Entity ^RayCastResult::HitEntity::get()
	{
		return this->mHit;
	}
	Vector3 RayCastResult::HitCoords::get()
	{
		return this->mHitCoord;
	}
	Vector3 RayCastResult::UnkVec::get()
	{
		return this->mUnk;
	}
	int RayCastResult::Result::get()
	{
		return this->mStatus;
	}
	int RayCastResult::Intersected::get()
	{
		return this->mIntersected;
	}

}