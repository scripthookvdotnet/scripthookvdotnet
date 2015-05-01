#pragma once

#include "Vector3.hpp"
#include "VehicleHashes.hpp"
#include "PedHashes.hpp"
#include "WeaponHashes.hpp"

namespace GTA
{
	public value class Model
	{
	public:
		Model(int hash);
		Model(Native::VehicleHash hash);
		Model(Native::PedHash hash);
		Model(Native::WeaponHash hash);
		Model(System::String ^name);

		static inline operator Model(int source)
		{
			return Model(source);
		}
		static inline operator Model(System::String ^source)
		{
			return Model(source);
		}

		property int Hash
		{
			int get();
		}

		property bool IsValid
		{
			bool get();
		}
		property bool IsInCdImage
		{
			bool get();
		}
		property bool IsLoaded
		{
			bool get();
		}
		property bool IsCollisionLoaded
		{
			bool get();
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
		property bool IsPed
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

		void GetDimensions(Math::Vector3 %minimum, Math::Vector3 %maximum);
		Math::Vector3 GetDimensions();

		void Request();
		bool Request(int timeout);
		void MarkAsNoLongerNeeded();

		virtual System::String ^ToString() override;

	private:
		int mHash;
	};
}