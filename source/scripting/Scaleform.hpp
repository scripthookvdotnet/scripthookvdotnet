#pragma once

#include "Vector3.hpp"

namespace GTA
{
	public ref class ScaleformArgumentTXD sealed
	{
	public:
		ScaleformArgumentTXD(System::String ^s) : _txd(s)
		{
		}

	internal:
		System::String ^_txd;
	};

	public ref class Scaleform sealed
	{
	public:
		Scaleform(System::String ^scaleformID);
		[System::ObsoleteAttribute("Scaleform(int handle) is obselete, Please Use Scaleform(string scaleformID) instead")]
		Scaleform(int handle);
		~Scaleform();

		property int Handle
		{
			int get();
		}
		property bool IsLoaded
		{
			bool get();
		}
		property bool IsValid
		{
			bool get();
		}

		[System::ObsoleteAttribute("Scaleform.Load(string scaleformID) is obselete, Please Use Scaleform(string scaleformID) instead")]
		bool Load(System::String ^scaleformID);
		void Unload();

		void CallFunction(System::String ^function, ... array<System::Object ^> ^arguments);

		void Render2D();
		void Render2DScreenSpace(System::Drawing::PointF location, System::Drawing::PointF size);
		void Render3D(GTA::Math::Vector3 position, GTA::Math::Vector3 rotation, GTA::Math::Vector3 scale);
		void Render3DAdditive(GTA::Math::Vector3 position, GTA::Math::Vector3 rotation, GTA::Math::Vector3 scale);
		

	private:
		int _handle;
		System::String ^_scaleformID;
	};
}