#include "Pickup.hpp"
#include "Native.hpp"

namespace GTA
{
	Pickup::Pickup(int handle) : mHandle(handle)
	{
	}

	int Pickup::Handle::get()
	{
		return this->mHandle;
	}

	Math::Vector3 Pickup::Position::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_PICKUP_COORDS, this->mHandle);
	}

	bool Pickup::IsCollected::get()
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_PICKUP_BEEN_COLLECTED, this->Handle);
	}

	bool Pickup::Exists()
	{
		return Native::Function::Call<bool>(Native::Hash::DOES_PICKUP_EXIST, this->mHandle);
	}

	bool Pickup::ObjectExists()
	{
		return Native::Function::Call<bool>(Native::Hash::DOES_PICKUP_OBJECT_EXIST, this->mHandle);
	}

	void Pickup::Delete()
	{
		return Native::Function::Call(Native::Hash::REMOVE_PICKUP, this->mHandle);
	}
}