#pragma once

#include "Entity.hpp"

namespace GTA
{
	public ref class Pickup sealed
	{
	public:
		Pickup(int handle);

		property int Handle
		{
			int get();
		}
		property bool IsCollected
		{
			bool get();
		}
		property Math::Vector3 Position
		{
			Math::Vector3 get();
		}

		bool Exists();
		bool ObjectExists();
		void Delete();

	private:
		int _handle;
	};
}