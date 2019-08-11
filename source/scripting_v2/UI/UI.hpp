#pragma once

#include "Vector3.hpp"

namespace GTA
{
	public enum class Font
	{
		ChaletLondon = 0,
		HouseScript = 1,
		Monospace = 2,
		ChaletComprimeCologne = 4,
		Pricedown = 7
	};
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

	public ref class Notification
	{
	public:
		void Hide();

	internal:
		Notification(int handle);

	private:
		int _handle;
	};

	public ref class UI sealed abstract
	{
	public:
		static const int WIDTH = 1280;
		static const int HEIGHT = 720;

		static Notification ^Notify(System::String ^message);
		static Notification ^Notify(System::String ^message, bool blinking);

		static void ShowSubtitle(System::String ^message);
		static void ShowSubtitle(System::String ^message, int duration);

		static void ShowHelpMessage(System::String ^message);
		static void ShowHelpMessage(System::String ^message, bool sound);
		static void ShowHelpMessage(System::String ^message, int duration);
		static void ShowHelpMessage(System::String ^message, int duration, bool sound);

		static bool IsHudComponentActive(HudComponent component);
		static void ShowHudComponentThisFrame(HudComponent component);
		static void HideHudComponentThisFrame(HudComponent component);

		static System::Drawing::Point WorldToScreen(Math::Vector3 position);

		static void DrawTexture(System::String ^filename, int index, int level, int time, System::Drawing::Point pos, System::Drawing::Size size);
		static void DrawTexture(System::String ^filename, int index, int level, int time, System::Drawing::Point pos, System::Drawing::Size size, float rotation, System::Drawing::Color color);
		static void DrawTexture(System::String ^filename, int index, int level, int time, System::Drawing::Point pos, System::Drawing::PointF center, System::Drawing::Size size, float rotation, System::Drawing::Color color);
		static void DrawTexture(System::String ^filename, int index, int level, int time, System::Drawing::Point pos, System::Drawing::PointF center, System::Drawing::Size size, float rotation, System::Drawing::Color color, float aspectRatio);

	internal:
		static System::Collections::Generic::Dictionary<System::String ^, int> ^_textures = gcnew System::Collections::Generic::Dictionary<System::String ^, int>();
	};
}