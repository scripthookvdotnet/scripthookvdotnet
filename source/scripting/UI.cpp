#include "UI.hpp"
#include "Native.hpp"
#include "ScriptDomain.hpp"

#include <Main.h>

namespace GTA
{
	using namespace System;
	using namespace System::Drawing;

	Notification::Notification(int handle) : _handle(handle)
	{
	}

	void Notification::Hide()
	{
		Native::Function::Call(Native::Hash::_REMOVE_NOTIFICATION, _handle);
	}

	Notification ^UI::Notify(String ^message)
	{
		return Notify(message, false);
	}
	Notification ^UI::Notify(String ^message, bool blinking)
	{
		Native::Function::Call(Native::Hash::_SET_NOTIFICATION_TEXT_ENTRY, "CELL_EMAIL_BCON");
		Native::Function::PushLongString(message);

		return gcnew Notification(Native::Function::Call<int>(Native::Hash::_DRAW_NOTIFICATION, blinking, 1));
	}

	void UI::ShowSubtitle(String ^message)
	{
		ShowSubtitle(message, 2500);
	}
	void UI::ShowSubtitle(String ^message, int duration)
	{
		Native::Function::Call(Native::Hash::_SET_TEXT_ENTRY_2, "CELL_EMAIL_BCON");
		Native::Function::PushLongString(message);
		Native::Function::Call(Native::Hash::_DRAW_SUBTITLE_TIMED, duration, 1);
	}

	bool UI::IsHudComponentActive(HudComponent component)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_HUD_COMPONENT_ACTIVE, static_cast<int>(component));
	}
	void UI::ShowHudComponentThisFrame(HudComponent component)
	{
		Native::Function::Call(Native::Hash::SHOW_HUD_COMPONENT_THIS_FRAME, static_cast<int>(component));
	}
	void UI::HideHudComponentThisFrame(HudComponent component)
	{
		Native::Function::Call(Native::Hash::HIDE_HUD_COMPONENT_THIS_FRAME, static_cast<int>(component));
	}

	Point UI::WorldToScreen(Math::Vector3 position)
	{
		float pointX, pointY;

		if (!Native::Function::Call<bool>(Native::Hash::_WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, &pointX, &pointY))
		{
			return Point();
		}

		return Point(static_cast<int>(pointX * UI::WIDTH), static_cast<int>(pointY * UI::HEIGHT));
	}

	void UI::DrawTexture(String ^filename, int index, int level, int time, Point pos, Size size)
	{
		DrawTexture(filename, index, level, time, pos, PointF(0.0f, 0.0f), size, 0.0f, Color::White, 1.0f);
	}
	void UI::DrawTexture(String ^filename, int index, int level, int time, Point pos, Size size, float rotation, Color color)
	{
		DrawTexture(filename, index, level, time, pos, PointF(0.0f, 0.0f), size, rotation, color, 1.0f);
	}
	void UI::DrawTexture(String ^filename, int index, int level, int time, Point pos, PointF center, Size size, float rotation, Color color)
	{
		DrawTexture(filename, index, level, time, pos, center, size, rotation, color, 1.0f);
	}
	void UI::DrawTexture(String ^filename, int index, int level, int time, Point pos, PointF center, Size size, float rotation, Color color, float aspectRatio)
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
			id = createTexture(reinterpret_cast<const char *>(ScriptDomain::CurrentDomain->PinString(filename).ToPointer()));

			_textures->Add(filename, id);
		}

		const float x = static_cast<float>(pos.X) / UI::WIDTH;
		const float y = static_cast<float>(pos.Y) / UI::HEIGHT;
		const float w = static_cast<float>(size.Width) / UI::WIDTH;
		const float h = static_cast<float>(size.Height) / UI::HEIGHT;

		drawTexture(id, index, level, time, w, h, center.X, center.Y, x, y, rotation, aspectRatio, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
	}
}