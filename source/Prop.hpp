#pragma once

#include "Entity.hpp"

namespace GTA
{
	public ref class Prop sealed : public Entity
	{
	public:
		Prop(int handle) : Entity(handle)
		{
		}
	};
}