#include "Rope.hpp"
#include "Native.hpp"
#include "Vector3.hpp"
#include "Entity.hpp"

namespace GTA
{
	Rope::Rope(int handle) : _handle(handle)
	{
	}

	int Rope::Handle::get()
	{
		return _handle;
	}
	float Rope::Length::get()
	{
		return Native::Function::Call<float>(Native::Hash::_GET_ROPE_LENGTH, Handle);
	}
	void Rope::Length::set(float value)
	{
		Native::Function::Call(Native::Hash::ROPE_FORCE_LENGTH, Handle, value);
	}
	int Rope::VertexCount::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ROPE_VERTEX_COUNT, Handle);
	}

	void Rope::ActivatePhysics()
	{
		Native::Function::Call(Native::Hash::ACTIVATE_PHYSICS, Handle);
	}
	void Rope::ResetLength(bool reset)
	{
		Native::Function::Call(Native::Hash::ROPE_RESET_LENGTH, Handle, reset);
	}
	void Rope::AttachEntities(Entity ^entityOne, Entity ^entityTwo, float length)
	{
		AttachEntities(entityOne, Math::Vector3(), entityTwo, Math::Vector3(), length);
	}
	void Rope::AttachEntities(Entity ^entityOne, Math::Vector3 positionOne, Entity ^entityTwo, Math::Vector3 positionTwo, float length)
	{
		int tmpOne, tmpTwo;
		Native::Function::Call(Native::Hash::ATTACH_ENTITIES_TO_ROPE, Handle, entityOne, entityTwo, positionOne.X, positionOne.Y, positionOne.Z, positionTwo.X, positionTwo.Y, positionTwo.Z, length, 0, 0, &tmpOne, &tmpTwo);
	}
	void Rope::AttachEntity(Entity ^entity)
	{
		AttachEntity(entity, Math::Vector3());
	}
	void Rope::AttachEntity(Entity^ entity, Math::Vector3 position)
	{
		Native::Function::Call(Native::Hash::ATTACH_ROPE_TO_ENTITY, Handle, entity, position.X, position.Y, position.Z, 0);
	}
	void Rope::DetachEntity(Entity^ entity)
	{
		Native::Function::Call(Native::Hash::DETACH_ROPE_FROM_ENTITY, Handle, entity);
	}
	void Rope::PinVertex(int vertex, Math::Vector3 position)
	{
		Native::Function::Call(Native::Hash::PIN_ROPE_VERTEX, Handle, vertex, position.X, position.Y, position.Z);
	}
	void Rope::UnpinVertex(int vertex)
	{
		Native::Function::Call(Native::Hash::UNPIN_ROPE_VERTEX, Handle, vertex);
	}
	Math::Vector3 Rope::GetVertexCoord(int vertex)
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ROPE_VERTEX_COORD, Handle, vertex);
	}

	bool Rope::Exists()
	{
		return Exists(this);
	}
	bool Rope::Exists(Rope ^rope)
	{
		if (!Object::ReferenceEquals(rope, nullptr))
		{
			int handle = rope->Handle;
			return Native::Function::Call<bool>(Native::Hash::DOES_ROPE_EXIST, &handle);
		}
		else
		{
			return false;
		}
	}
	bool Rope::Equals(Rope ^rope)
	{
		return !System::Object::ReferenceEquals(rope, nullptr) && Handle == rope->Handle;
	}
	void Rope::Delete()
	{
		int handle = Handle;
		Native::Function::Call(Native::Hash::DELETE_ROPE, &handle);
	}
}
