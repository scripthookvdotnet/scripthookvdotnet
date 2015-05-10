#include "Blip.hpp"
#include "Native.hpp"

namespace GTA
{
	Blip::Blip(int id) : mID(id)
	{
	}

	int Blip::ID::get()
	{
		return this->mID;
	}
	Math::Vector3 Blip::Position::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_BLIP_INFO_ID_COORD, this->ID);
	}
	void Blip::Position::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_COORDS, this->ID, value.X, value.Y, value.Z);
	}
	void Blip::Scale::set(float scale)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_SCALE, this->ID, scale);
	}
	bool Blip::IsFlashing::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_BLIP_FLASHING, this->ID);
	}
	void Blip::IsFlashing::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_FLASHES, this->ID, value);
	}
	BlipColor Blip::Color::get()
	{
		return BlipColor(Native::Function::Call<int>(Native::Hash::GET_BLIP_COLOUR, this->ID));
	}
	void Blip::Color::set(BlipColor color)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_COLOUR, this->ID, (int)color);
	}
	int Blip::Alpha::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_BLIP_ALPHA, this->ID);
	}
	void Blip::Alpha::set(int alpha)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_ALPHA, this->ID, alpha);
	}
	bool Blip::Exists()
	{
		return Native::Function::Call<bool>(Native::Hash::DOES_BLIP_EXIST, this->ID);
	}
	void Blip::Remove()
	{
		int id = this->ID;
		Native::Function::Call(Native::Hash::REMOVE_BLIP, &id);
	}
	void Blip::SetAsFriendly()
	{
		Native::Function::Call(Native::Hash::SET_BLIP_AS_FRIENDLY, this->ID, 1);
	}
	void Blip::SetAsHostile()
	{
		Native::Function::Call(Native::Hash::SET_BLIP_AS_FRIENDLY, this->ID, 0);
	}
}