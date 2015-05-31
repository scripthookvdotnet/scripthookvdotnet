#include "Ray.hpp"
#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Prop.hpp"
#include "Native.hpp"

namespace GTA
{
	Ray::Ray(int Handle)
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
	Ray ^Ray::Cast(Vector3 source, Vector3 target, IntersectOptions options, int UnkFlags, Entity ^entity)
	{
		return gcnew Ray(Native::Function::Call<int>(Native::Hash::_0x377906D8A31E5586, source.X, source.Y, source.Z, target.X, target.Y, target.Z, static_cast<int>(options), entity == nullptr ? 0 : entity->Handle, UnkFlags));
	}
	int Ray::Handle::get()
	{
		return this->mHandle;
	}
	bool Ray::DidHitEntity::get()
	{
		return !ReferenceEquals(this->mHit, nullptr);
	}
	Entity ^Ray::HitEntity::get()
	{
		return this->mHit;
	}
	Vector3 Ray::HitCoords::get()
	{
		return this->mHitCoord;
	}
	Vector3 Ray::UnkVec::get()
	{
		return this->mUnk;
	}
	int Ray::Result::get()
	{
		return this->mStatus;
	}
	int Ray::Intersected::get()
	{
		return this->mIntersected;
	}

}