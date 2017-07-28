#include "Model.hpp"
#include "Native.hpp"
#include "NativeMemory.hpp"
#include "Game.hpp"
#include "Script.hpp"

namespace GTA
{
	Model::Model(int hash) : _hash(hash)
	{
	}
	Model::Model(System::String ^name) : _hash(Game::GenerateHash(name))
	{
	}
	Model::Model(Native::PedHash hash) : _hash(static_cast<int>(hash))
	{
	}
	Model::Model(Native::VehicleHash hash) : _hash(static_cast<int>(hash))
	{
	}
	Model::Model(Native::WeaponHash hash) : _hash(static_cast<int>(hash))
	{
	}

	int Model::Hash::get()
	{
		return _hash;
	}
	bool Model::IsValid::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_MODEL_VALID, Hash);
	}
	bool Model::IsInCdImage::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_MODEL_IN_CDIMAGE, Hash);
	}
	bool Model::IsLoaded::get()
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_MODEL_LOADED, Hash);
	}
	bool Model::IsCollisionLoaded::get()
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_COLLISION_FOR_MODEL_LOADED, Hash);
	}
	bool Model::IsBicycle::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_BICYCLE, Hash);
	}
	bool Model::IsBike::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_BIKE, Hash);
	}
	bool Model::IsBoat::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_BOAT, Hash);
	}
	bool Model::IsCar::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_CAR, Hash);
	}
	bool Model::IsHelicopter::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_HELI, Hash);
	}
	bool Model::IsPed::get()
	{
		return Native::MemoryAccess::IsModelAPed(Hash);
	}
	bool Model::IsPlane::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_PLANE, Hash);
	}
	bool Model::IsQuadbike::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_QUADBIKE, Hash);
	}
	bool Model::IsTrain::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_TRAIN, Hash);
	}
	bool Model::IsVehicle::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_MODEL_A_VEHICLE, Hash);
	}
	bool Model::IsCargobob::get()
	{
		switch (static_cast<Native::VehicleHash>(Hash))
		{
			case Native::VehicleHash::Cargobob:
			case Native::VehicleHash::Cargobob2:
			case Native::VehicleHash::Cargobob3:
			case Native::VehicleHash::Cargobob4:
				return true;
		}
		return false;
	}

	void Model::GetDimensions([System::Runtime::InteropServices::OutAttribute] Math::Vector3 %minimum, [System::Runtime::InteropServices::OutAttribute] Math::Vector3 %maximum)
	{
		Native::OutputArgument ^outmin = gcnew Native::OutputArgument();
		Native::OutputArgument ^outmax = gcnew Native::OutputArgument();
		Native::Function::Call(Native::Hash::GET_MODEL_DIMENSIONS, Hash, outmin, outmax);

		minimum = outmin->GetResult<Math::Vector3>();
		maximum = outmax->GetResult<Math::Vector3>();
	}
	Math::Vector3 Model::GetDimensions()
	{
		Math::Vector3 min, max;
		GetDimensions(min, max);

		return Math::Vector3::Subtract(max, min);
	}

	void Model::Request()
	{
		Native::Function::Call(Native::Hash::REQUEST_MODEL, Hash);
	}
	bool Model::Request(int timeout)
	{
		Request();

		const System::DateTime endtime = timeout >= 0 ? System::DateTime::UtcNow + System::TimeSpan(0, 0, 0, 0, timeout) : System::DateTime::MaxValue;

		while (!IsLoaded)
		{
			Script::Yield();

			if (System::DateTime::UtcNow >= endtime)
			{
				return false;
			}
		}

		return true;
	}
	void Model::MarkAsNoLongerNeeded()
	{
		Native::Function::Call(Native::Hash::SET_MODEL_AS_NO_LONGER_NEEDED, Hash);
	}
	bool Model::Equals(Model model)
	{
		return Hash == model.Hash;
	}
}