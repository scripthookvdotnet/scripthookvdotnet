#include "Rope.hpp"
#include "Native.hpp"
#include "Vector3.hpp"
#include "Entity.hpp"

namespace GTA
{
	Rope::Rope(int handle) : mHandle(handle)
	{
	}

	int Rope::Handle::get()
	{
		return this->mHandle;
	}

	void Rope::Delete()
	{
		int handle = this->Handle;
		Native::Function::Call(Native::Hash::DELETE_ROPE, &handle);
	}
	void Rope::ActivatePhysics()
	{
		Native::Function::Call(Native::Hash::ACTIVATE_PHYSICS, this->Handle);
	}
	void Rope::ResetLength(bool reset)
	{
		Native::Function::Call(Native::Hash::ROPE_RESET_LENGTH, this->Handle, reset);
	}
	void Rope::ForceLength(float length)
	{
		Native::Function::Call(Native::Hash::ROPE_FORCE_LENGTH, this->Handle, length);
	}
	void Rope::AttachEntities(Entity ^entityOne, Entity ^entityTwo, float length)
	{
		AttachEntities(entityOne, Math::Vector3(), entityTwo, Math::Vector3(), length);
	}
	void Rope::AttachEntities(Entity ^entityOne, Math::Vector3 offsetOne, Entity ^entityTwo, Math::Vector3 offsetTwo, float length)
	{
		int tmpOne;
		int tmpTwo;
		Native::Function::Call(Native::Hash::ATTACH_ENTITIES_TO_ROPE, this->Handle, entityOne, entityTwo, offsetOne.X, offsetOne.Y, offsetOne.Z, offsetTwo.X, offsetTwo.Y, offsetTwo.Z, length, 0, 0, &tmpOne, &tmpTwo);
	}
	void Rope::AttachEntity(Entity ^entity)
	{
		this->AttachEntity(entity, Math::Vector3());
	}
	void Rope::AttachEntity(Entity^ entity, Math::Vector3 offset)
	{
		Native::Function::Call(Native::Hash::ATTACH_ROPE_TO_ENTITY, this->Handle, entity, offset.X, offset.Y, offset.Z, 0);
	}
	void Rope::DetachEntity(Entity^ entity)
	{
		Native::Function::Call(Native::Hash::DETACH_ROPE_FROM_ENTITY, this->Handle, entity);
	}
}