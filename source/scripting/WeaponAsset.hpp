#pragma once

#include "WeaponHashes.hpp"

namespace GTA
{
	public value class WeaponAsset
	{
	public:
		WeaponAsset(int hash);
		WeaponAsset(System::UInt32 hash);
		WeaponAsset(Native::WeaponHash hash);

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
		virtual bool Equals(WeaponAsset model);

		virtual inline int GetHashCode() override
		{
			return Hash;
		}
		virtual inline System::String ^ToString() override
		{
			return "0x" + static_cast<System::UInt32>(Hash).ToString("X");
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