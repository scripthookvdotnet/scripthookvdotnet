#include "Raycast.hpp"
#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Prop.hpp"
#include "Native.hpp"

namespace GTA
{
	RaycastResult::RaycastResult(int handle) : _hitEntity(nullptr)
	{
		int hitsomething = 0, enthandle = 0;
		Native::OutputArgument ^hitCoords = gcnew Native::OutputArgument(), ^surfaceNormal = gcnew Native::OutputArgument();
		_result = Native::Function::Call<int>(Native::Hash::_GET_RAYCAST_RESULT, handle, &hitsomething, hitCoords, surfaceNormal, &enthandle);

		_didHit = hitsomething != 0;
		_hitCoords = hitCoords->GetResult<Math::Vector3>();
		_surfaceNormal = surfaceNormal->GetResult<Math::Vector3>();

		if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, enthandle))
		{
			switch (Native::Function::Call<int>(Native::Hash::GET_ENTITY_TYPE, enthandle))
			{
				case 1:
					_hitEntity = gcnew Ped(enthandle);
					break;
				case 2:
					_hitEntity = gcnew Vehicle(enthandle);
					break;
				case 3:
					_hitEntity = gcnew Prop(enthandle);
					break;
			}
		}
	}

	int RaycastResult::Result::get()
	{
		return _result;
	}
	bool RaycastResult::DitHitEntity::get()
	{
		return !ReferenceEquals(_hitEntity, nullptr);
	}
	bool RaycastResult::DitHitAnything::get()
	{
		return _didHit;
	}
	Entity ^RaycastResult::HitEntity::get()
	{
		return _hitEntity;
	}
	Math::Vector3 RaycastResult::HitCoords::get()
	{
		return _hitCoords;
	}
	Math::Vector3 RaycastResult::SurfaceNormal::get()
	{
		return _surfaceNormal;
	}
}