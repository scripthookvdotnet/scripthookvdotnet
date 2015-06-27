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

	int Blip::Alpha::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_BLIP_ALPHA, this->Handle);
	}
	void Blip::Alpha::set(int alpha)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_ALPHA, this->Handle, alpha);
	}
	BlipColor Blip::Color::get()
	{
		return static_cast<BlipColor>(Native::Function::Call<int>(Native::Hash::GET_BLIP_COLOUR, this->Handle));
	}
	void Blip::Color::set(BlipColor color)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_COLOUR, this->Handle, static_cast<int>(color));
	}
	bool Blip::IsFlashing::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_BLIP_FLASHING, this->Handle);
	}
	void Blip::IsFlashing::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_FLASHES, this->Handle, value);
	}
	void Blip::IsFriendly::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_AS_FRIENDLY, this->Handle, value);
	}
	bool Blip::IsOnMinimap::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_BLIP_ON_MINIMAP, this->Handle);
	}
	bool Blip::IsShortRange::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_BLIP_SHORT_RANGE, this->Handle);
	}
	void Blip::IsShortRange::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_AS_SHORT_RANGE, this->Handle, value);
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
	void Blip::ShowRoute::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_ROUTE, this->Handle, value);
	}
	BlipSprite Blip::Sprite::get()
	{
		return static_cast<BlipSprite>(Native::Function::Call<int>(Native::Hash::GET_BLIP_SPRITE, this->Handle));
	}
	void Blip::Sprite::set(BlipSprite sprite)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_SPRITE, this->Handle, static_cast<int>(sprite));
	}
	int Blip::Type::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_BLIP_INFO_ID_TYPE, this->Handle);
	}

	void Blip::HideNumber()
	{
		Native::Function::Call(Native::Hash::HIDE_NUMBER_ON_BLIP, this->Handle);
	}
	void Blip::ShowNumber(int number)
	{
		Native::Function::Call(Native::Hash::SHOW_NUMBER_ON_BLIP, this->Handle, number);
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
}