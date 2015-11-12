#include "Blip.hpp"
#include "Native.hpp"

namespace GTA
{
	Blip::Blip(int handle) : _handle(handle)
	{
	}

	int Blip::Handle::get()
	{
		return _handle;
	}
	int Blip::Alpha::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_BLIP_ALPHA, Handle);
	}
	void Blip::Alpha::set(int alpha)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_ALPHA, Handle, alpha);
	}
	BlipColor Blip::Color::get()
	{
		return static_cast<BlipColor>(Native::Function::Call<int>(Native::Hash::GET_BLIP_COLOUR, Handle));
	}
	void Blip::Color::set(BlipColor color)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_COLOUR, Handle, static_cast<int>(color));
	}
	bool Blip::IsFlashing::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_BLIP_FLASHING, Handle);
	}
	void Blip::IsFlashing::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_FLASHES, Handle, value);
	}
	void Blip::IsFriendly::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_AS_FRIENDLY, Handle, value);
	}
	bool Blip::IsOnMinimap::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_BLIP_ON_MINIMAP, Handle);
	}
	bool Blip::IsShortRange::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_BLIP_SHORT_RANGE, Handle);
	}
	void Blip::IsShortRange::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_AS_SHORT_RANGE, Handle, value);
	}
	void Blip::Name::set(System::String ^value)
	{
		Native::Function::Call(Native::Hash::_0xF9113A30DE5C6670, "STRING");
		Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, value);
		Native::Function::Call(Native::Hash::_0xBC38B49BCB83BC9B, Handle);
	}
	Math::Vector3 Blip::Position::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_BLIP_INFO_ID_COORD, Handle);
	}
	void Blip::Position::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_COORDS, Handle, value.X, value.Y, value.Z);
	}
	void Blip::Scale::set(float scale)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_SCALE, Handle, scale);
	}
	void Blip::ShowRoute::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_ROUTE, Handle, value);
	}
	BlipSprite Blip::Sprite::get()
	{
		return static_cast<BlipSprite>(Native::Function::Call<int>(Native::Hash::GET_BLIP_SPRITE, Handle));
	}
	void Blip::Sprite::set(BlipSprite sprite)
	{
		Native::Function::Call(Native::Hash::SET_BLIP_SPRITE, Handle, static_cast<int>(sprite));
	}
	int Blip::Type::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_BLIP_INFO_ID_TYPE, Handle);
	}

	void Blip::HideNumber()
	{
		Native::Function::Call(Native::Hash::HIDE_NUMBER_ON_BLIP, Handle);
	}
	void Blip::ShowNumber(int number)
	{
		Native::Function::Call(Native::Hash::SHOW_NUMBER_ON_BLIP, Handle, number);
	}

	bool Blip::Exists()
	{
		return Native::Function::Call<bool>(Native::Hash::DOES_BLIP_EXIST, Handle);
	}
	void Blip::Remove()
	{
		int id = Handle;
		Native::Function::Call(Native::Hash::REMOVE_BLIP, &id);
	}
}