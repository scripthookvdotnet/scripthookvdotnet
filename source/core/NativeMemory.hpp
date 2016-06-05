#pragma once

#include "Vector3.hpp"

namespace GTA
{
	namespace Native
	{
		private ref class MemoryAccess abstract sealed
		{
		internal:
			static int GetGameVersion();

			static unsigned char ReadByte(System::IntPtr address);
			static short ReadShort(System::IntPtr address);
			static int ReadInt(System::IntPtr address);
			static float ReadFloat(System::IntPtr address);
			static Math::Vector3 ReadVector3(System::IntPtr address);
			static System::String ^ReadString(System::IntPtr address);
			static System::IntPtr ReadPtr(System::IntPtr address);
			static void WriteByte(System::IntPtr address, unsigned char value);
			static void WriteShort(System::IntPtr address, short value);
			static void WriteInt(System::IntPtr address, int value);
			static void WriteFloat(System::IntPtr address, float value);
			static void WriteVector3(System::IntPtr address, Math::Vector3 value);
			static void WriteString(System::IntPtr address, System::String ^value);

			static System::IntPtr GetGlobalAddress(int id);
			static System::IntPtr GetEntityAddress(int handle);
			static System::IntPtr GetPlayerAddress(int handle);
			static System::IntPtr GetCheckpointAddress(int handle);

			static array<int> ^GetEntityHandles();
			static array<int> ^GetEntityHandles(Math::Vector3 position, float radius);
			static array<int> ^GetVehicleHandles(array<int> ^modelhashes);
			static array<int> ^GetVehicleHandles(Math::Vector3 position, float radius, array<int> ^modelhashes);
			static array<int> ^GetPedHandles(array<int> ^modelhashes);
			static array<int> ^GetPedHandles(Math::Vector3 position, float radius, array<int> ^modelhashes);
			static array<int> ^GetPropHandles(array<int> ^modelhashes);
			static array<int> ^GetPropHandles(Math::Vector3 position, float radius, array<int> ^modelhashes);
			static array<int> ^GetCheckpointHandles();

			static void SendEuphoriaMessage(int targetHandle, System::String ^message, System::Collections::Generic::Dictionary<System::String ^, System::Object ^> ^_arguments);

			static int CreateTexture(System::String ^filename);
			static void DrawTexture(int id, int index, int level, int time, float sizeX, float sizeY, float centerX, float centerY, float posX, float posY, float rotation, float scaleFactor, System::Drawing::Color color);

			static System::UInt64(*_entityAddressFunc)(int handle);
			static System::UInt64(*_playerAddressFunc)(int handle);
			static int(*_addEntityToPoolFunc)(System::UInt64 address);
			static System::UInt64(*_entityPositionFunc)(System::UInt64 address, float *position);
			static System::UInt64(*_entityModel1Func)(System::UInt64 address), (*_entityModel2Func)(System::UInt64 address);
			static System::UInt64 *_entityPoolAddress, *_vehiclePoolAddress, *_pedPoolAddress, *_objectPoolAddress;
			static unsigned char(*SetNmBoolAddress)(__int64, __int64, unsigned char);
			static unsigned char(*SetNmIntAddress)(__int64, __int64, int);
			static unsigned char(*SetNmFloatAddress)(__int64, __int64, float);
			static unsigned char(*SetNmVec3Address)(__int64, __int64, float, float, float);
			static unsigned char(*SetNmStringAddress)(__int64, __int64, __int64);
			static System::UInt64 CreateNmMessageFunc, GiveNmMessageFunc;
			static System::UInt64(*CheckpointBaseAddr)();
			static System::UInt64(*CheckpointHandleAddr)(System::UInt64 baseAddr, int Handle);
			static System::UInt64 *checkpointPoolAddress;

		private:
			static MemoryAccess();

			static System::UInt64 FindPattern(const char *pattern, const char *mask);
		};
	}
}
