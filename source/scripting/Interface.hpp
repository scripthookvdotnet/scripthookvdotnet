#pragma once

namespace GTA
{
	public interface class IHandleable
	{
		property int Handle
		{
			 int get();
		}

		bool Exists();
	};

	public interface class ISpatial
	{
		property Math::Vector3 Position
		{
			 Math::Vector3 get();
			void set(Math::Vector3 value);
		}
		property Math::Vector3 Rotation
		{
			 Math::Vector3 get();
			void set(Math::Vector3 value);
		}
	};
}
