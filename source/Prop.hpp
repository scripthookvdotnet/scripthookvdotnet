#pragma once

#include "Entity.hpp"

namespace GTA
{
	public ref class Prop sealed : public Entity
	{
	public:
		Prop(int id);

		static property Prop ^Any
		{
			Prop ^get();
		}

	private:
		int mID;
	};
}