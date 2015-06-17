#include "Pickup.hpp"
#include "Native.hpp"

namespace GTA
{
	Pickup::Pickup(int handle) : Entity(handle)
	{
	}

	bool Pickup::IsCollected::get()
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_PICKUP_BEEN_COLLECTED, this->Handle);
	}
}