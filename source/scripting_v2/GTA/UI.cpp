#include "UI.h"

void GTA::Notification::Hide()
{
	auto task = gcnew SHVDN::NativeFunc(0xBE4390CB40B3E627ull /*_REMOVE_NOTIFICATION*/, (UInt64)_handle);
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task);
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
	auto task1 = gcnew SHVDN::NativeFunc(0x202709F4C58A0424ull /*_SET_NOTIFICATION_TEXT_ENTRY*/, (UInt64)SHVDN::NativeMemory::CellEmailBcon.ToInt64());
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task1);

	// TODO: PushLongString
	auto task2 = gcnew SHVDN::NativeFunc(0x6C188BE134E074AAull /*_ADD_TEXT_COMPONENT_STRING*/, (UInt64)SHVDN::ScriptDomain::CurrentDomain->PinString(message).ToInt64());
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task2);

	auto task3 = gcnew SHVDN::NativeFunc(0x2ED7843F8F801023ull /*_DRAW_NOTIFICATION*/, blinking ? 1ull : 0ull, 1ull);
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task3);

	return task3->Result != nullptr ? gcnew Notification(*(int*)task3->Result) : nullptr;
}

void GTA::UI::ShowSubtitle(String ^message)
{
	ShowSubtitle(message, 2500);
}
void GTA::UI::ShowSubtitle(String ^message, int duration)
{
	auto task1 = gcnew SHVDN::NativeFunc(0xB87A37EEB7FAA67Dull /*_SET_TEXT_ENTRY_2*/, (UInt64)SHVDN::NativeMemory::CellEmailBcon.ToInt64());
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task1);

	// TODO: PushLongString
	auto task2 = gcnew SHVDN::NativeFunc(0x6C188BE134E074AAull /*_ADD_TEXT_COMPONENT_STRING*/, (UInt64)SHVDN::ScriptDomain::CurrentDomain->PinString(message).ToInt64());
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task2);

	auto task3 = gcnew SHVDN::NativeFunc(0x9D77056A530643F6ull /*_DRAW_SUBTITLE_TIMED*/, (UInt64)duration, 1ull);
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task3);
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
	auto task1 = gcnew SHVDN::NativeFunc(0x8509B634FBE7DA11ull /*_SET_TEXT_COMPONENT_FORMAT*/, (UInt64)SHVDN::NativeMemory::String.ToInt64());
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task1);

	// TODO: PushLongString
	auto task2 = gcnew SHVDN::NativeFunc(0x6C188BE134E074AAull /*_ADD_TEXT_COMPONENT_STRING*/, (UInt64)SHVDN::ScriptDomain::CurrentDomain->PinString(message).ToInt64());
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task2);

	auto task3 = gcnew SHVDN::NativeFunc(0x238FFE5C7B0498A6ull /*_DISPLAY_HELP_TEXT_FROM_STRING_LABEL*/, 0ull, 0ull, sound ? 1ull : 0ull, (UInt64)duration);
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task3);
}

bool GTA::UI::IsHudComponentActive(HudComponent component)
{
	auto task = gcnew SHVDN::NativeFunc(0xBC4C9EA5391ECC0Dull /*IS_HUD_COMPONENT_ACTIVE*/, (UInt64)component);
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task);

	return task->Result != nullptr && *task->Result != 0;
}
void GTA::UI::ShowHudComponentThisFrame(HudComponent component)
{
	auto task = gcnew SHVDN::NativeFunc(0x0B4DF1FA60C0E664ull /*SHOW_HUD_COMPONENT_THIS_FRAME*/, (UInt64)component);
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task);
}
void GTA::UI::HideHudComponentThisFrame(HudComponent component)
{
	auto task = gcnew SHVDN::NativeFunc(0x6806C51AD12B83B8ull /*HIDE_HUD_COMPONENT_THIS_FRAME*/, (UInt64)component);
	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task);
}

//Point GTA::UI::WorldToScreen(Math::Vector3 position)
//{
//	float pointX, pointY;
//
//	auto task = gcnew SHVDN::NativeFunc(0x34E82F05DF2974F5ull /*_WORLD3D_TO_SCREEN2D*/, (UInt64)*(UInt32*)&position.X, (UInt64)*(UInt32*)&position.Y, (UInt64)*(UInt32*)&position.Z, (UInt64)&pointX, (UInt64)&pointY);
//	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task);
//
//	if (task->Result != nullptr && *task->Result != 0)
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
