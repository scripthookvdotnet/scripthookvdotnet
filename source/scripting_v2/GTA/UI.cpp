/**
 * Copyright (C) 2015 crosire & contributors
 * License: https://github.com/crosire/scripthookvdotnet#license
 */

#include "UI.h"

void GTA::Notification::Hide()
{
	SHVDN::NativeFunc::Invoke(0xBE4390CB40B3E627 /*THEFEED_REMOVE_ITEM*/, _handle);
}

GTA::Notification::Notification(int handle) : _handle(handle)
{
}

GTA::Notification ^GTA::UI::Notify(String ^message)
{
	return Notify(message, false);
}
GTA::Notification ^GTA::UI::Notify(String ^message, bool blinking)
{
	SHVDN::NativeFunc::Invoke(0x202709F4C58A0424 /*BEGIN_TEXT_COMMAND_THEFEED_POST*/, SHVDN::NativeMemory::CellEmailBcon);
	SHVDN::NativeFunc::PushLongString(message);
	auto res = SHVDN::NativeFunc::Invoke(0x2ED7843F8F801023 /*END_TEXT_COMMAND_THEFEED_POST_TICKER*/, blinking, true);

	return res != nullptr ? gcnew Notification(*(int*)res) : nullptr;
}

void GTA::UI::ShowSubtitle(String ^message)
{
	ShowSubtitle(message, 2500);
}
void GTA::UI::ShowSubtitle(String ^message, int duration)
{
	SHVDN::NativeFunc::Invoke(0xB87A37EEB7FAA67D /*BEGIN_TEXT_COMMAND_PRINT*/, SHVDN::NativeMemory::CellEmailBcon);
	SHVDN::NativeFunc::PushLongString(message);
	SHVDN::NativeFunc::Invoke(0x9D77056A530643F6 /*END_TEXT_COMMAND_PRINT*/, duration, true);
}

void GTA::UI::ShowHelpMessage(String ^message)
{
	ShowHelpMessage(message, 5000, true);
}
void GTA::UI::ShowHelpMessage(String ^message, bool sound)
{
	ShowHelpMessage(message, 5000, sound);
}
void GTA::UI::ShowHelpMessage(String ^message, int duration)
{
	ShowHelpMessage(message, duration, true);
}
void GTA::UI::ShowHelpMessage(String ^message, int duration, bool sound)
{
	SHVDN::NativeFunc::Invoke(0x8509B634FBE7DA11 /*BEGIN_TEXT_COMMAND_DISPLAY_HELP*/, SHVDN::NativeMemory::String);
	SHVDN::NativeFunc::PushLongString(message);
	SHVDN::NativeFunc::Invoke(0x238FFE5C7B0498A6 /*END_TEXT_COMMAND_DISPLAY_HELP*/, 0, false, sound, duration);
}

bool GTA::UI::IsHudComponentActive(HudComponent component)
{
	auto res = SHVDN::NativeFunc::Invoke(0xBC4C9EA5391ECC0D /*IS_HUD_COMPONENT_ACTIVE*/, (UInt64)component);
	return res != nullptr && *res != 0;
}
void GTA::UI::ShowHudComponentThisFrame(HudComponent component)
{
	SHVDN::NativeFunc::Invoke(0x0B4DF1FA60C0E664 /*SHOW_HUD_COMPONENT_THIS_FRAME*/, (UInt64)component);
}
void GTA::UI::HideHudComponentThisFrame(HudComponent component)
{
	SHVDN::NativeFunc::Invoke(0x6806C51AD12B83B8 /*HIDE_HUD_COMPONENT_THIS_FRAME*/, (UInt64)component);
}

//Point GTA::UI::WorldToScreen(Math::Vector3 position)
//{
//	float pointX, pointY;
//
//	auto res = SHVDN::NativeFunc::Invoke(0x34E82F05DF2974F5 /*_WORLD3D_TO_SCREEN2D*/, (UInt64)*(UInt32*)&position.X, (UInt64)*(UInt32*)&position.Y, (UInt64)*(UInt32*)&position.Z, (UInt64)&pointX, (UInt64)&pointY);
//	if (res != nullptr && *res != 0)
//		return Point(static_cast<int>(pointX * UI::WIDTH), static_cast<int>(pointY * UI::HEIGHT));
//	else
//		return Point();
//}

void GTA::UI::DrawTexture(String ^filename, int index, int level, int time, Point pos, Size size)
{
	DrawTexture(filename, index, level, time, pos, PointF(0.0f, 0.0f), size, 0.0f, Color::White, 1.0f);
}
void GTA::UI::DrawTexture(String ^filename, int index, int level, int time, Point pos, Size size, float rotation, Color color)
{
	DrawTexture(filename, index, level, time, pos, PointF(0.0f, 0.0f), size, rotation, color, 1.0f);
}
void GTA::UI::DrawTexture(String ^filename, int index, int level, int time, Point pos, PointF center, Size size, float rotation, Color color)
{
	DrawTexture(filename, index, level, time, pos, center, size, rotation, color, 1.0f);
}
void GTA::UI::DrawTexture(String ^filename, int index, int level, int time, Point pos, PointF center, Size size, float rotation, Color color, float aspectRatio)
{
	if (!System::IO::File::Exists(filename))
	{
		throw gcnew System::IO::FileNotFoundException(filename);
	}

	int id;

	if (_textures->ContainsKey(filename))
	{
		id = _textures->default[filename];
	}
	else
	{
		id = SHVDN::NativeMemory::CreateTexture(filename);

		_textures->Add(filename, id);
	}

	const float x = static_cast<float>(pos.X) / UI::WIDTH;
	const float y = static_cast<float>(pos.Y) / UI::HEIGHT;
	const float w = static_cast<float>(size.Width) / UI::WIDTH;
	const float h = static_cast<float>(size.Height) / UI::HEIGHT;

	SHVDN::NativeMemory::DrawTexture(id, index, level, time, w, h, center.X, center.Y, x, y, rotation, aspectRatio, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
}
