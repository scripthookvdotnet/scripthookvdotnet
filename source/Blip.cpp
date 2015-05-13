#include "Blip.hpp"
#include "Native.hpp"

namespace GTA
{
	Blip::Blip(int handle) : mHandle(handle)
	{
	}

	int Blip::Handle::get()
	{
		return this->mHandle;
	}
	Math::Vector3 Blip::Position::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_BLIP_INFO_ID_COORD, this->Handle);
	}
	void Blip::Position::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_COORDS, this->Handle, value.X, value.Y, value.Z);
	}
	void Blip::Scale::set(float scale)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_SCALE, this->Handle, scale);
	}
	bool Blip::IsFlashing::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_BLIP_FLASHING, this->Handle);
	}
	void Blip::IsFlashing::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_FLASHES, this->Handle, value);
	}
	BlipColor Blip::Color::get()
	{
		return BlipColor(Native::Function::Call<int>(Native::Hash::GET_BLIP_COLOUR, this->Handle));
	}
	void Blip::Color::set(BlipColor color)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_COLOUR, this->Handle, (int)color);
	}
	int Blip::Alpha::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_BLIP_ALPHA, this->Handle);
	}
	void Blip::Alpha::set(int alpha)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_ALPHA, this->Handle, alpha);
	}
	bool Blip::Exists()
	{
		return Native::Function::Call<bool>(Native::Hash::DOES_BLIP_EXIST, this->Handle);
	}
	void Blip::Remove()
	{
		int id = this->Handle;
		Native::Function::Call(Native::Hash::REMOVE_BLIP, &id);
	}
	void Blip::SetAsFriendly()
	{
		Native::Function::Call(Native::Hash::SET_BLIP_AS_FRIENDLY, this->Handle, 1);
	}
	void Blip::SetAsHostile()
	{
		Native::Function::Call(Native::Hash::SET_BLIP_AS_FRIENDLY, this->Handle, 0);
	}
	void Blip::ShowRoute()
	{
		Native::Function::Call(Native::Hash::SET_BLIP_ROUTE, this->Handle, 1);
	}
	void Blip::HideRoute()
	{
		Native::Function::Call(Native::Hash::SET_BLIP_ROUTE, this->Handle, 0);
	}
}