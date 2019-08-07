#pragma once

#include "Vector3.hpp"
#include "Interface.hpp"

#include "PedHashes.hpp"
#include "VehicleHashes.hpp"
#include "WeaponHashes.hpp"

namespace GTA
{
	public value class Model : System::IEquatable<Model>
	{
	public:
		Model(int hash);
		Model(System::String ^name);
		Model(Native::PedHash hash);
		Model(Native::VehicleHash hash);
		Model(Native::WeaponHash hash);

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
		property bool IsCargobob
		{
			bool get();
		}

		void GetDimensions([System::Runtime::InteropServices::OutAttribute] Math::Vector3 %minimum, [System::Runtime::InteropServices::OutAttribute] Math::Vector3 %maximum);
		Math::Vector3 GetDimensions();

		void Request();
		bool Request(int timeout);
		void MarkAsNoLongerNeeded();

		virtual bool Equals(Model model);

		virtual inline int GetHashCode() override
		{
			return Hash;
		}
		virtual inline System::String ^ToString() override
		{
			return "0x" + Hash.ToString("X");
		}

		static inline operator Model(int source)
		{
			return Model(source);
		}
		static inline operator Model(System::String ^source)
		{
			return Model(source);
		}
		static inline operator Model(Native::PedHash source)
		{
			return Model(source);
		}
		static inline operator Model(Native::VehicleHash source)
		{
			return Model(source);
		}
		static inline operator Model(Native::WeaponHash source)
		{
			return Model(source);
		}

		static inline bool operator==(Model left, Model right)
		{
			return left.Equals(right);
		}
		static inline bool operator!=(Model left, Model right)
		{
			return !operator==(left, right);
		}
	private:
		int _hash;
	};
}