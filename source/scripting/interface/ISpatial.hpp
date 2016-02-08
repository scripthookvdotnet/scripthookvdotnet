#pragma once

#include "Vector3.hpp"

namespace GTA
{

	public interface class ISpatial
	{
		property Math::Vector3 Position
		{
			 Math::Vector3 get();
			void set(Math::Vector3 value);
		}
	};
}
