#pragma once

#include "Entity.hpp"

namespace GTA
{
	public ref class Pickup sealed : public Entity
	{
	public:
		Pickup(int handle);

		property bool IsCollected
		{
			bool get();
		}
	};
}