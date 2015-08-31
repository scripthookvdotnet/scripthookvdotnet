#pragma once

#include "Vector3.hpp"
#include "ScriptDomain.hpp"

#include <inttypes.h>

namespace GTA
{
	namespace Native
	{
		private ref class MemoryAccess abstract sealed
		{
		public:
			static MemoryAccess();

			static uintptr_t GetAddressOfEntity(int handle);
			static uintptr_t GetAddressOfPlayer(int handle);

			static float GetVehicleRPM(int handle);
			static float GetVehicleAcceleration(int handle);
			static float GetVehicleSteering(int handle);
			static array<int> ^GetVehicleHandles();
			static array<int> ^GetVehicleHandles(int modelhash);
			static array<int> ^GetVehicleHandles(Math::Vector3 position, float radius);
			static array<int> ^GetVehicleHandles(Math::Vector3 position, float radius, int modelHash);
			static array<int> ^GetPedHandles();
			static array<int> ^GetPedHandles(int modelhash);
			static array<int> ^GetPedHandles(Math::Vector3 position, float radius);
			static array<int> ^GetPedHandles(Math::Vector3 position, float radius, int modelHash);
			static array<int> ^GetPropHandles();
			static array<int> ^GetPropHandles(int modelhash);
			static array<int> ^GetPropHandles(Math::Vector3 position, float radius);
			static array<int> ^GetPropHandles(Math::Vector3 position, float radius, int modelHash);
			static array<int> ^GetEntityHandles();
			static array<int> ^GetEntityHandles(Math::Vector3 position, float radius);

		internal:
			static uintptr_t EntityAddressFunc;
			static uintptr_t PlayerAddressFunc;
			static uintptr_t EntityPoolAddress;
			static uintptr_t VehiclePoolAddress;
			static uintptr_t PedPoolAddress;
			static uintptr_t ObjPoolAddress;
			static uintptr_t AddEntToPoolAddress;
			static uintptr_t EntityCoordsAddress;
			static uintptr_t EntityModelAddress1;
			static uintptr_t EntityModelAddress2;

		private:
			static uintptr_t FindPattern(const char *pattern, const char *mask);

			static const char *EntityAddressPattern = "\x33\xFF\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x58";
			static const char *EntityAddressMask = "xxx????xxxxx";
			static const char *PlayerAddressPattern = "\xB2\x01\xE8\x00\x00\x00\x00\x33\xC9\x48\x85\xC0\x74\x3B";
			static const char *PlayerAddressMask = "xxx????xxxxxxx";
			static const char *EntityPoolPattern = "\x4C\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC1\x49\x8B\x41\x08";
			static const char *EntityPoolMask = "xxx????xxxxxxx";
			static const char *VehiclePoolPattern = "\x48\x8B\x05\x00\x00\x00\x00\xF3\x0F\x59\xF6\x48\x8B\x08";
			static const char *VehiclePoolMask = "xxx????xxxxxxx";
			static const char *PedPoolPattern = "\x48\x8B\x05\x00\x00\x00\x00\x41\x0F\xBF\xC8\x0F\xBF\x40\x10";
			static const char *PedPoolMask = "xxx????xxxxxxxx";
			static const char *ObjPoolPattern = "\x48\x8B\x05\x00\x00\x00\x00\x8B\x78\x10\x85\xFF";
			static const char *ObjPoolMask = "xxx????xxxxx";
			static const char *AddEntToPoolPattern = "\x48\xF7\xF9\x49\x8B\x48\x08\x48\x63\xD0\xC1\xE0\x08\x0F\xB6\x1C\x11\x03\xD8";
			static const char *AddEntToPoolMask = "xxxxxxxxxxxxxxxxxxx";
			static const char *EntityCoordsPattern = "\x48\x8B\xC8\xE8\x00\x00\x00\x00\xF3\x0F\x10\x54\x24\x00\xF3\x0F\x10\x4C\x24\x00\xF3\x0F\x10";
			static const char *EntityCoordsMask = "xxxx????xxxxx?xxxxx?xxx";
			static const char *EntityModelPattern = "\x25\xFF\xFF\xFF\x3F\x89\x44\x24\x38\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x03";
			static const char *EntityModelMask = "xxxxxxxxxx????xxxxx";
		};
	}
}