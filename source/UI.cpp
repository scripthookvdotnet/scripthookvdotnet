#include "UI.hpp"
#include "Native.hpp"
#include "ScriptDomain.hpp"

#include <Main.h>

namespace GTA
{
	Notification::Notification(int Handle) : mHandle(Handle)
	{
	}

	void Notification::Hide()
	{
		Native::Function::Call(Native::Hash::_REMOVE_NOTIFICATION, this->mHandle);
	}

	Notification ^UI::Notify(System::String ^msg)
	{
		return Notify(msg, false);
	}
	Notification ^UI::Notify(System::String ^msg, bool blinking)
	{
		Native::Function::Call(Native::Hash::_SET_NOTIFICATION_TEXT_ENTRY, "STRING");
		Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, msg);

		return gcnew Notification(Native::Function::Call<int>(Native::Hash::_DRAW_NOTIFICATION, blinking, 1));
	}

	void UI::ShowSubtitle(System::String ^msg)
	{
		ShowSubtitle(msg, 2500);
	}
	void UI::ShowSubtitle(System::String ^msg, int duration)
	{
		Native::Function::Call(Native::Hash::_SET_TEXT_ENTRY_2, "STRING");
		Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, msg);
		Native::Function::Call(Native::Hash::_DRAW_SUBTITLE_TIMED, duration, 1);
	}

	System::Drawing::Point UI::WorldToScreen(Math::Vector3 position)
	{
		float pointX, pointY;

		if (!Native::Function::Call<bool>(Native::Hash::_WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, &pointX, &pointY))
		{
			return System::Drawing::Point();
		}

		return System::Drawing::Point((int)(pointX * UI::WIDTH), (int)(pointY * UI::HEIGHT));
	}

	void UI::DrawTexture(System::String ^filename, int index, int level, int time, System::Drawing::Point pos, System::Drawing::Size size)
	{
		DrawTexture(filename, index, level, time, pos, System::Drawing::PointF(0.5f, 0.5f), size, 0.0f, System::Drawing::Color::White);
	}
	void UI::DrawTexture(System::String ^filename, int index, int level, int time, System::Drawing::Point pos, System::Drawing::Size size, float rotation, System::Drawing::Color color)
	{
		DrawTexture(filename, index, level, time, pos, System::Drawing::PointF(0.5f, 0.5f), size, rotation, color);
	}
	void UI::DrawTexture(System::String ^filename, int index, int level, int time, System::Drawing::Point pos, System::Drawing::PointF center, System::Drawing::Size size, float rotation, System::Drawing::Color color)
	{
		int id;

		if (sTextures->ContainsKey(filename))
		{
			id = sTextures->default[filename];
		}
		else
		{
			id = createTexture(reinterpret_cast<const char *>(ScriptDomain::CurrentDomain->PinString(filename).ToPointer()));

			sTextures->Add(filename, id);
		}

		const float x = static_cast<float>(pos.X) / UI::WIDTH;
		const float y = static_cast<float>(pos.Y) / UI::HEIGHT;
		const float w = static_cast<float>(size.Width) / UI::WIDTH;
		const float h = static_cast<float>(size.Height) / UI::HEIGHT;

		drawTexture(id, index, level, time, w, h, center.X, center.Y, x, y, rotation, 1.0f, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
	}
	void UI::HideHudComponentThisFrame(HudComponent component)
	{
		Native::Function::Call(Native::Hash::HIDE_HUD_COMPONENT_THIS_FRAME, static_cast<int>(component));
	}
	void UI::ShowHudComponentThisFrame(HudComponent component)
	{
		Native::Function::Call(Native::Hash::SHOW_HUD_COMPONENT_THIS_FRAME, static_cast<int>(component));
	}
	bool UI::IsHudComponentActive(HudComponent component)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_HUD_COMPONENT_ACTIVE, static_cast<int>(component));
	}
}