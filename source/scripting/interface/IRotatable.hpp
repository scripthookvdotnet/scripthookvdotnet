#pragma once

#include "Vector3.hpp"

namespace GTA
{

	public interface class IRotatable
	{
		property Math::Vector3 Rotation
		{
			 Math::Vector3 get();
			void set(Math::Vector3 value);
		}
	};
}
