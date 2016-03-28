#include "Native.hpp"
#include "Script.hpp"
#include "WeaponAsset.hpp"

namespace GTA
{
	WeaponAsset::WeaponAsset(int hash) : _hash(hash)
	{
	}
	WeaponAsset::WeaponAsset(System::UInt32 hash) : _hash(static_cast<int>(hash))
	{
	}
	WeaponAsset::WeaponAsset(Native::WeaponHash hash) : _hash(static_cast<int>(hash))
	{
	}

	int WeaponAsset::Hash::get()
	{
		return _hash;
	}
	bool WeaponAsset::IsValid::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_WEAPON_VALID, Hash);
	}
	bool WeaponAsset::IsLoaded::get()
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_WEAPON_ASSET_LOADED, Hash);
	}

	void WeaponAsset::Request()
	{
		Native::Function::Call(Native::Hash::REQUEST_WEAPON_ASSET, Hash, 31, 0);
	}
	bool WeaponAsset::Request(int timeout)
	{
		Request();

		const System::DateTime endtime = timeout >= 0 ? System::DateTime::UtcNow + System::TimeSpan(0, 0, 0, 0, timeout) : System::DateTime::MaxValue;

		while (!IsLoaded)
		{
			Script::Yield();

			if (System::DateTime::UtcNow >= endtime)
			{
				return false;
			}
		}

		return true;
	}
	void WeaponAsset::Dismiss()
	{
		Native::Function::Call(Native::Hash::REMOVE_WEAPON_ASSET, Hash);
	}
	bool WeaponAsset::Equals(WeaponAsset model)
	{
		return Hash == model.Hash;
	}
}