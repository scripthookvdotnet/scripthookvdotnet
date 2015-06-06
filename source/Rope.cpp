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
	float Rope::Length::get()
	{
		return Native::Function::Call<float>(Native::Hash::_0x73040398DFF9A4A6, this->mHandle);
	}
	void Rope::Length::set(float value)
	{
		Native::Function::Call(Native::Hash::ROPE_FORCE_LENGTH, this->mHandle, value);
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
	void Rope::AttachEntities(Entity ^entityOne, Entity ^entityTwo, float length)
	{
		this->AttachEntities(entityOne, Math::Vector3(), entityTwo, Math::Vector3(), length);
	}
	void Rope::AttachEntities(Entity ^entityOne, Math::Vector3 positionOne, Entity ^entityTwo, Math::Vector3 positionTwo, float length)
	{
		int tmpOne;
		int tmpTwo;
		Native::Function::Call(Native::Hash::ATTACH_ENTITIES_TO_ROPE, this->Handle, entityOne, entityTwo, positionOne.X, positionOne.Y, positionOne.Z, positionTwo.X, positionTwo.Y, positionTwo.Z, length, 0, 0, &tmpOne, &tmpTwo);
	}
	void Rope::AttachEntity(Entity ^entity)
	{
		this->AttachEntity(entity, Math::Vector3());
	}
	void Rope::AttachEntity(Entity^ entity, Math::Vector3 position)
	{
		Native::Function::Call(Native::Hash::ATTACH_ROPE_TO_ENTITY, this->Handle, entity, position.X, position.Y, position.Z, 0);
	}
	void Rope::DetachEntity(Entity^ entity)
	{
		Native::Function::Call(Native::Hash::DETACH_ROPE_FROM_ENTITY, this->Handle, entity);
	}

	void Rope::LoadTextures()
	{
		Native::Function::Call(Native::Hash::ROPE_LOAD_TEXTURES);
	}
}
