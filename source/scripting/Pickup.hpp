#pragma once

#include "Entity.hpp"
#include "Interface.hpp"

namespace GTA
{
	public ref class Pickup sealed : System::IEquatable<Pickup ^>, IHandleable
	{
	public:
		Pickup(int handle);

		virtual property int Handle
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

		void Delete();
		bool ObjectExists();

		virtual bool Exists();
		static bool Exists(Pickup ^pickup);

		virtual bool Equals(System::Object ^obj) override;
		virtual bool Equals(Pickup ^pickup);

		virtual inline int GetHashCode() override
		{
			return Handle;
		}

		static inline bool operator==(Pickup ^left, Pickup ^right)
		{
			if (ReferenceEquals(left, nullptr))
			{
				return ReferenceEquals(right, nullptr);
			}

			return left->Equals(right);
		}
		static inline bool operator!=(Pickup ^left, Pickup ^right)
		{
			return !operator==(left, right);
		}

	private:
		int _handle;
	};
}