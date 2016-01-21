#pragma once

#include "Vector3.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Entity;
	#pragma endregion

	public ref class Rope sealed
	{
	public:
		Rope(int handle);

		property int Handle
		{
			int get();
		}
		property float Length
		{
			float get();
			void set(float value);
		}
		property int VertexCount
		{
			int get();
		}

		void ActivatePhysics();
		void ResetLength(bool reset);
		void AttachEntities(Entity ^entityOne, Entity ^entityTwo, float length);
		void AttachEntities(Entity ^entityOne, Math::Vector3 positionOne, Entity ^entityTwo, Math::Vector3 positionTwo, float length);
		void AttachEntity(Entity ^entity);
		void AttachEntity(Entity ^entity, Math::Vector3 position);
		void DetachEntity(Entity ^entity);
		void PinVertex(int vertex, Math::Vector3 position);
		void UnpinVertex(int vertex);
		Math::Vector3 GetVertexCoord(int vertex);

		bool Exists();
		static bool Exists(Rope ^rope);
		void Delete();
		virtual bool Equals(Rope ^rope);

		virtual inline int GetHashCode() override
		{
			return Handle;
		}
		static inline bool operator==(Rope ^left, Rope ^right)
		{
			if (ReferenceEquals(left, nullptr))
			{
				return ReferenceEquals(right, nullptr);
			}

			return left->Equals(right);
		}
		static inline bool operator!=(Rope ^left, Rope ^right)
		{
			return !operator==(left, right);
		}

	private:
		int _handle;
	};
}