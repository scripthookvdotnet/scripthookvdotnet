#pragma once

#include "WeaponHashes.hpp"

namespace GTA
{
	public value class WeaponAsset : System::IEquatable<WeaponAsset>
	{
	public:
		WeaponAsset(int weaponHash);
		WeaponAsset(System::UInt32 weaponHash);
		WeaponAsset(Native::WeaponHash weaponHash);

		property int Hash
		{
			int get();
		}
		property bool IsValid
		{
			bool get();
		}
		property bool IsLoaded
		{
			bool get();
		}

		void Request();
		bool Request(int timeout);
		void Dismiss();
		virtual bool Equals(System::Object ^obj) override;
		virtual bool Equals(WeaponAsset weaponAsset);

		virtual inline int GetHashCode() override
		{
			return Hash;
		}
		virtual inline System::String ^ToString() override
		{
			return "0x" + static_cast<System::UInt32>(Hash).ToString("X");
		}
		static inline operator WeaponAsset(int source)
		{
			return WeaponAsset(source);
		}
		static inline operator WeaponAsset(System::UInt32 source)
		{
			return WeaponAsset(source);
		}
		static inline operator WeaponAsset(Native::WeaponHash source)
		{
			return WeaponAsset(source);
		}
		static inline bool operator==(WeaponAsset left, WeaponAsset right)
		{
			return left.Equals(right);
		}
		static inline bool operator!=(WeaponAsset left, WeaponAsset right)
		{
			return !operator==(left, right);
		}

	private:
		int _hash;
	};
}