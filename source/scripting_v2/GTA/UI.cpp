using namespace System;
using namespace System::Collections::Generic;
using namespace System::Drawing;

namespace GTA
{
	public enum class HudComponent
	{
		WantedStars = 1,
		WeaponIcon = 2,
		Cash = 3,
		MpCash = 4,
		MpMessage = 5,
		VehicleName = 6,
		AreaName = 7,
		Unused = 8,
		StreetName = 9,
		HelpText = 10,
		FloatingHelpText1 = 11,
		FloatingHelpText2 = 12,
		CashChange = 13,
		Reticle = 14,
		SubtitleText = 15,
		RadioStationsWheel = 16,
		Saving = 17,
		GamingStreamUnusde = 18,
		WeaponWheel = 19,
		WeaponWheelStats = 20,
		DrugsPurse01 = 21,
		DrugsPurse02 = 22,
		DrugsPurse03 = 23,
		DrugsPurse04 = 24,
		MpTagCashFromBank = 25,
		MpTagPackages = 26,
		MpTagCuffKeys = 27,
		MpTagDownloadData = 28,
		MpTagIfPedFollowing = 29,
		MpTagKeyCard = 30,
		MpTagRandomObject = 31,
		MpTagRemoteControl = 32,
		MpTagCashFromSafe = 33,
		MpTagWeaponsPackage = 34,
		MpTagKeys = 35,
		MpVehicle = 36,
		MpVehicleHeli = 37,
		MpVehiclePlane = 38,
		PlayerSwitchAlert = 39,
		MpRankBar = 40,
		DirectorMode = 41,
		ReplayController = 42,
		ReplayMouse = 43,
		ReplayHeader = 44,
		ReplayOptions = 45,
		ReplayHelpText = 46,
		ReplayMiscText = 47,
		ReplayTopLine = 48,
		ReplayBottomLine = 49,
		ReplayLeftBar = 50,
		ReplayTimer = 51
	};

	//public ref class Notification
	//{
	//	int handle;

	//internal:
	//	Notification(int handle)
	//	{
	//		this->handle = handle;
	//	}

	//public:
	//	void Hide()
	//	{
	//		auto task = gcnew SHVDN::NativeFunc(0xBE4390CB40B3E627ull /*_REMOVE_NOTIFICATION*/, (UInt64)handle);
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task);
	//	}
	//};

	//public ref class UI sealed abstract
	//{
	//public:
	//	static const int WIDTH = 1280;
	//	static const int HEIGHT = 720;

	//	static Notification ^Notify(String ^message)
	//	{
	//		return Notify(message, false);
	//	}
	//	static Notification ^Notify(String ^message, bool blinking)
	//	{
	//		auto task1 = gcnew SHVDN::NativeFunc(0x202709F4C58A0424ull /*_SET_NOTIFICATION_TEXT_ENTRY*/, (UInt64)SHVDN::NativeMemory::CellEmailBcon.ToInt64());
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task1);

	//		// TODO: PushLongString
	//		auto task2 = gcnew SHVDN::NativeFunc(0x6C188BE134E074AAull /*_ADD_TEXT_COMPONENT_STRING*/, (UInt64)SHVDN::ScriptDomain::CurrentDomain->PinString(message).ToInt64());
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task2);

	//		auto task3 = gcnew SHVDN::NativeFunc(0x2ED7843F8F801023ull /*_DRAW_NOTIFICATION*/, blinking ? 1ull : 0ull, 1ull);
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task3);

	//		return task3->Result != nullptr ? gcnew Notification(*(int*)task3->Result) : nullptr;
	//	}

	//	static void ShowSubtitle(String ^message)
	//	{
	//		ShowSubtitle(message, 2500);
	//	}
	//	static void ShowSubtitle(String ^message, int duration)
	//	{
	//		auto task1 = gcnew SHVDN::NativeFunc(0xB87A37EEB7FAA67Dull /*_SET_TEXT_ENTRY_2*/, (UInt64)SHVDN::NativeMemory::CellEmailBcon.ToInt64());
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task1);

	//		// TODO: PushLongString
	//		auto task2 = gcnew SHVDN::NativeFunc(0x6C188BE134E074AAull /*_ADD_TEXT_COMPONENT_STRING*/, (UInt64)SHVDN::ScriptDomain::CurrentDomain->PinString(message).ToInt64());
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task2);

	//		auto task3 = gcnew SHVDN::NativeFunc(0x9D77056A530643F6ull /*_DRAW_SUBTITLE_TIMED*/, (UInt64)duration, 1ull);
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task3);
	//	}

	//	static void ShowHelpMessage(String ^message)
	//	{
	//		ShowHelpMessage(message, 5000, true);
	//	}
	//	static void ShowHelpMessage(String ^message, bool sound)
	//	{
	//		ShowHelpMessage(message, 5000, sound);
	//	}
	//	static void ShowHelpMessage(String ^message, int duration)
	//	{
	//		ShowHelpMessage(message, duration, true);
	//	}
	//	static void ShowHelpMessage(String ^message, int duration, bool sound)
	//	{
	//		auto task1 = gcnew SHVDN::NativeFunc(0x8509B634FBE7DA11ull /*_SET_TEXT_COMPONENT_FORMAT*/, (UInt64)SHVDN::NativeMemory::String.ToInt64());
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task1);

	//		// TODO: PushLongString
	//		auto task2 = gcnew SHVDN::NativeFunc(0x6C188BE134E074AAull /*_ADD_TEXT_COMPONENT_STRING*/, (UInt64)SHVDN::ScriptDomain::CurrentDomain->PinString(message).ToInt64());
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task2);

	//		auto task3 = gcnew SHVDN::NativeFunc(0x238FFE5C7B0498A6ull /*_DISPLAY_HELP_TEXT_FROM_STRING_LABEL*/, 0ull, 0ull, sound ? 1ull : 0ull, (UInt64)duration);
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task3);
	//	}

	//	static bool IsHudComponentActive(HudComponent component)
	//	{
	//		auto task = gcnew SHVDN::NativeFunc(0xBC4C9EA5391ECC0Dull /*IS_HUD_COMPONENT_ACTIVE*/, (UInt64)component);
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task);

	//		return task->Result != nullptr && *task->Result != 0;
	//	}
	//	static void ShowHudComponentThisFrame(HudComponent component)
	//	{
	//		auto task = gcnew SHVDN::NativeFunc(0x0B4DF1FA60C0E664ull /*SHOW_HUD_COMPONENT_THIS_FRAME*/, (UInt64)component);
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task);
	//	}
	//	static void HideHudComponentThisFrame(HudComponent component)
	//	{
	//		auto task = gcnew SHVDN::NativeFunc(0x6806C51AD12B83B8ull /*HIDE_HUD_COMPONENT_THIS_FRAME*/, (UInt64)component);
	//		SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task);
	//	}

	//	//static Point WorldToScreen(Math::Vector3 position)
	//	//{
	//	//	float pointX, pointY;

	//	//	auto task = gcnew SHVDN::NativeFunc(0x34E82F05DF2974F5ull /*_WORLD3D_TO_SCREEN2D*/, (UInt64)*(UInt32*)&position.X, (UInt64)*(UInt32*)&position.Y, (UInt64)*(UInt32*)&position.Z, (UInt64)&pointX, (UInt64)&pointY);
	//	//	SHVDN::ScriptDomain::CurrentDomain->ExecuteTask(task);

	//	//	if (task->Result != nullptr && *task->Result != 0)
	//	//		return Point(static_cast<int>(pointX * UI::WIDTH), static_cast<int>(pointY * UI::HEIGHT));
	//	//	else
	//	//		return Point();
	//	//}

	//	static void DrawTexture(String ^filename, int index, int level, int time, Point pos, Size size)
	//	{
	//		DrawTexture(filename, index, level, time, pos, PointF(0.0f, 0.0f), size, 0.0f, Color::White, 1.0f);
	//	}
	//	static void DrawTexture(String ^filename, int index, int level, int time, Point pos, Size size, float rotation, Color color)
	//	{
	//		DrawTexture(filename, index, level, time, pos, PointF(0.0f, 0.0f), size, rotation, color, 1.0f);
	//	}
	//	static void DrawTexture(String ^filename, int index, int level, int time, Point pos, PointF center, Size size, float rotation, Color color)
	//	{
	//		DrawTexture(filename, index, level, time, pos, center, size, rotation, color, 1.0f);
	//	}
	//	static void DrawTexture(String ^filename, int index, int level, int time, Point pos, PointF center, Size size, float rotation, Color color, float aspectRatio)
	//	{
	//		if (!System::IO::File::Exists(filename))
	//		{
	//			throw gcnew System::IO::FileNotFoundException(filename);
	//		}

	//		int id;

	//		if (_textures->ContainsKey(filename))
	//		{
	//			id = _textures->default[filename];
	//		}
	//		else
	//		{
	//			id = SHVDN::NativeMemory::CreateTexture(filename);

	//			_textures->Add(filename, id);
	//		}

	//		const float x = static_cast<float>(pos.X) / UI::WIDTH;
	//		const float y = static_cast<float>(pos.Y) / UI::HEIGHT;
	//		const float w = static_cast<float>(size.Width) / UI::WIDTH;
	//		const float h = static_cast<float>(size.Height) / UI::HEIGHT;

	//		SHVDN::NativeMemory::DrawTexture(id, index, level, time, w, h, center.X, center.Y, x, y, rotation, aspectRatio, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
	//	}

	//internal:
	//	static Dictionary<String ^, int> ^_textures = gcnew Dictionary<String ^, int>();
	//};
}
