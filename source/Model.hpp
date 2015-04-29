#pragma once

namespace GTA
{
	public value class Model
	{
	public:
		Model(int hash);
		Model(System::String ^name);

		property int Hash
		{
			int get();
		}

		property bool IsBicycle
		{
			bool get();
		}
		property bool IsBike
		{
			bool get();
		}
		property bool IsBoat
		{
			bool get();
		}
		property bool IsCar
		{
			bool get();
		}
		property bool IsHelicopter
		{
			bool get();
		}
		property bool IsPlane
		{
			bool get();
		}
		property bool IsQuadbike
		{
			bool get();
		}
		property bool IsTrain
		{
			bool get();
		}
		property bool IsVehicle
		{
			bool get();
		}
		property bool IsInCdImage
		{
			bool get();
		}
		property bool HasLoaded
		{
			bool get();
		}

		void Request();
		void Request(bool blockUntilLoaded);

	private:
		int mHash;
	};
}