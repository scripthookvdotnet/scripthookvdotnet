#include "Model.hpp"
#include "Native.hpp"
#include "NativeCaller.h"

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
	bool Model::IsInCdImage::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_MODEL_IN_CDIMAGE, this->mHash);
	}

	void Model::Request()
	{
		Native::Function::Call(Native::Hash::REQUEST_MODEL, this->mHash);
	}

	bool Model::HasLoaded()
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_MODEL_LOADED, this->mHash);
	}

	void Model::BlockTillLoaded()
	{
		while (!HasLoaded()) scriptWait(0);
	}
}