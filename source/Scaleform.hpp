#pragma once

#include "Vector3.hpp"

namespace GTA
{
	using namespace System::Collections::Generic;

	public ref class ScaleformArgumentTXD sealed
	{
	public:
		ScaleformArgumentTXD(System::String ^s) : txd(s)
		{
		}

		System::String ^txd;
	};

	public ref class Scaleform sealed
	{
	public:
		Scaleform::Scaleform(int handle) : mHandle(handle)
		{
		}

		Scaleform::Scaleform()
		{
		}

		property int Handle
		{
			int get();
		}

		bool Load(System::String ^scaleformID);
		void CallFunction(System::String ^function, ... array<Object^> ^arguments);

		void Render2D();
		void Render2DScreenSpace(System::Drawing::PointF location, System::Drawing::PointF size);

		void Render3D(GTA::Math::Vector3 position, GTA::Math::Vector3 rotation, GTA::Math::Vector3 scale);
		void Render3DAdditive(GTA::Math::Vector3 position, GTA::Math::Vector3 rotation, GTA::Math::Vector3 scale);

	private:
		int mHandle;
		System::String ^mScaleformID;
	};
}