#include "Model.hpp"
#include "Native.hpp"
#include "Script.hpp"

namespace GTA
{
	Model::Model(int hash) : mHash(hash)
	{
	}
	Model::Model(System::String ^name) : mHash(Native::Function::Call<int>(Native::Hash::GET_HASH_KEY, name))
	{
	}

	int Model::Hash::get()
	{
		return this->mHash;
	}

	bool Model::IsValid::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_MODEL_VALID, this->mHash);
	}
	bool Model::IsInCdImage::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_MODEL_IN_CDIMAGE, this->mHash);
	}
	bool Model::IsLoaded::get()
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_MODEL_LOADED, this->mHash);
	}
	bool Model::IsCollisionLoaded::get()
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_COLLISION_FOR_MODEL_LOADED, this->mHash);
	}

	bool Model::IsBicycle::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_BICYCLE, this->mHash);
	}
	bool Model::IsBike::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_BIKE, this->mHash);
	}
	bool Model::IsBoat::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_BOAT, this->mHash);
	}
	bool Model::IsCar::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_CAR, this->mHash);
	}
	bool Model::IsHelicopter::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_HELI, this->mHash);
	}
	bool Model::IsPed::get()
	{
		return IsValid && !IsVehicle;
	}
	bool Model::IsPlane::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_PLANE, this->mHash);
	}
	bool Model::IsQuadbike::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_QUADBIKE, this->mHash);
	}
	bool Model::IsTrain::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_THIS_MODEL_A_TRAIN, this->mHash);
	}
	bool Model::IsVehicle::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_MODEL_A_VEHICLE, this->mHash);
	}

	void Model::GetDimensions(Math::Vector3 %minimum, Math::Vector3 %maximum)
	{
		Native::OutputArgument ^outmin = gcnew Native::OutputArgument();
		Native::OutputArgument ^outmax = gcnew Native::OutputArgument();
		Native::Function::Call(Native::Hash::GET_MODEL_DIMENSIONS, this->mHash, outmin, outmax);

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
		Native::Function::Call(Native::Hash::REQUEST_MODEL, this->mHash);
	}
	bool Model::Request(int timeout)
	{
		Request();

		const System::DateTime endtime = timeout >= 0 ? System::DateTime::Now + System::TimeSpan(0, 0, 0, 0, timeout) : System::DateTime::MaxValue;

		while (!IsLoaded)
		{
			Script::Wait(0);

			if (System::DateTime::Now >= endtime)
			{
				return false;
			}
		}

		return true;
	}

	System::String ^Model::ToString()
	{
		return "0x" + this->mHash.ToString("X");
	}
}