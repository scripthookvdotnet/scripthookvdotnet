#pragma once

#include "Vector3.hpp"
#include "Matrix.hpp"

namespace GTA
{
	namespace Native
	{
		private enum class ModelInfoClassType
		{
			Invalid = 0,
			Object = 1,
			Mlo = 2,
			Time = 3,
			Weapon = 4,
			Vehicle = 5,
			Ped = 6
		};
		private enum class VehicleStructClassType
		{
			Invalid = -1,
			Automobile = 0x0,
			Plane = 0x1,
			Trailer = 0x2,
			QuadBike = 0x3,
			SubmarineCar = 0x5,
			AmphibiousAutomobile = 0x6,
			AmphibiousQuadBike = 0x7,
			Heli = 0x8,
			Blimp = 0x9,
			Autogyro = 0xA,
			Bike = 0xB,
			Bicycle = 0xC,
			Boat = 0xD,
			Train = 0xE,
			Submarine = 0xF
		};

		private ref class MemoryAccess abstract sealed
		{
		internal:
			static int GetGameVersion();

			static char ReadSByte(System::IntPtr address);
			static unsigned char ReadByte(System::IntPtr address);
			static short ReadShort(System::IntPtr address);
			static unsigned short ReadUShort(System::IntPtr address);
			static int ReadInt(System::IntPtr address);
			static unsigned int ReadUInt(System::IntPtr address);
			static float ReadFloat(System::IntPtr address);
			static Math::Vector3 ReadVector3(System::IntPtr address);
			static System::String ^ReadString(System::IntPtr address);
			static System::IntPtr ReadPtr(System::IntPtr address);
			static Math::Matrix ReadMatrix(System::IntPtr address);
			static long long ReadLong(System::IntPtr address);
			static unsigned long long ReadULong(System::IntPtr address);
			static void WriteSByte(System::IntPtr address, char value);
			static void WriteByte(System::IntPtr address, unsigned char value);
			static void WriteShort(System::IntPtr address, short value);
			static void WriteUShort(System::IntPtr address, unsigned short value);
			static void WriteInt(System::IntPtr address, int value);
			static void WriteUInt(System::IntPtr address, unsigned int value);
			static void WriteFloat(System::IntPtr address, float value);
			static void WriteVector3(System::IntPtr address, Math::Vector3 value);
			static void WriteMatrix(System::IntPtr address, Math::Matrix value);
			static void WriteLong(System::IntPtr address, long long value);
			static void WriteULong(System::IntPtr address, unsigned long long value);
			static void SetBit(System::IntPtr address, int bit);
			static void ClearBit(System::IntPtr address, int bit);
			static bool IsBitSet(System::IntPtr address, int bit);
			static unsigned int GetHashKey(System::String ^toHash);
			static System::String ^GetGXTEntryByHash(int Hash);

			static System::IntPtr GetEntityAddress(int handle);
			static System::IntPtr GetPlayerAddress(int handle);
			static System::IntPtr GetCheckpointAddress(int handle);
			static System::IntPtr GetEntityBoneMatrixAddress(int handle, int boneIndex);
			static System::IntPtr GetEntityBonePoseAddress(int handle, int boneIndex);
			static System::IntPtr GetPtfxAddress(int handle);
			static int GetEntityBoneCount(int handle);
			static float ReadWorldGravity();
			static void WriteWorldGravity(float value);
			static int ReadCursorSprite();
			static System::IntPtr GetGameplayCameraAddress();
			static System::IntPtr GetCameraAddress(int handle);

			static array<int> ^GetEntityHandles();
			static array<int> ^GetEntityHandles(Math::Vector3 position, float radius);
			static array<int> ^GetVehicleHandles(array<int> ^modelhashes);
			static array<int> ^GetVehicleHandles(Math::Vector3 position, float radius, array<int> ^modelhashes);
			static array<int> ^GetPedHandles(array<int> ^modelhashes);
			static array<int> ^GetPedHandles(Math::Vector3 position, float radius, array<int> ^modelhashes);
			static array<int> ^GetPropHandles(array<int> ^modelhashes);
			static array<int> ^GetPropHandles(Math::Vector3 position, float radius, array<int> ^modelhashes);
			static array<int> ^GetCheckpointHandles();
			static array<int> ^GetPickupObjectHandles();
			static array<int> ^GetPickupObjectHandles(Math::Vector3 position, float radius);
			static int GetNumberOfVehicles();

			static void SendEuphoriaMessage(int targetHandle, System::String ^message, System::Collections::Generic::Dictionary<System::String ^, System::Object ^> ^_arguments);

			static int CreateTexture(System::String ^filename);
			static void DrawTexture(int id, int index, int level, int time, float sizeX, float sizeY, float centerX, float centerY, float posX, float posY, float rotation, float scaleFactor, System::Drawing::Color color);

			static unsigned int(*_getHashKey)(char* stringPtr, unsigned int initialHash);
			static System::UInt64(*_entityAddressFunc)(int handle);
			static System::UInt64(*_playerAddressFunc)(int handle);
			static System::UInt64(*_ptfxAddressFunc)(int handle);
			static int(*_addEntityToPoolFunc)(System::UInt64 address);
			static System::UInt64(*_entityPositionFunc)(System::UInt64 address, float *position);
			static System::UInt64(*_entityModel1Func)(System::UInt64 address), (*_entityModel2Func)(System::UInt64 address);
			static System::UInt64 *_entityPoolAddress, *_vehiclePoolAddress, *_pedPoolAddress, *_objectPoolAddress, *_cameraPoolAddress, *_pickupObjectPoolAddress;
			static unsigned char(*SetNmBoolAddress)(__int64, __int64, unsigned char);
			static unsigned char(*SetNmIntAddress)(__int64, __int64, int);
			static unsigned char(*SetNmFloatAddress)(__int64, __int64, float);
			static unsigned char(*SetNmVec3Address)(__int64, __int64, float, float, float);
			static unsigned char(*SetNmStringAddress)(__int64, __int64, __int64);
			static System::UInt64(*GetLabelTextByHashFunc)(System::UInt64 address, int labelHash);
			static System::UInt64 GetLabelTextByHashAddr2;
			static System::UInt64 CreateNmMessageFunc, GiveNmMessageFunc;
			static System::UInt64(*CheckpointBaseAddr)();
			static System::UInt64(*CheckpointHandleAddr)(System::UInt64 baseAddr, int Handle);
			static System::UInt64 *checkpointPoolAddress;
			static float *_readWorldGravityAddr, *_writeWorldGravityAddr;
			static System::UInt64 *_gamePlayCameraAddr;
			static property System::Collections::ObjectModel::ReadOnlyCollection<System::Collections::ObjectModel::ReadOnlyCollection<int> ^> ^VehicleModels
			{
				System::Collections::ObjectModel::ReadOnlyCollection<System::Collections::ObjectModel::ReadOnlyCollection<int> ^> ^get()
				{
					return vehicleModels;
				}
			}
			static bool IsModelAPed(int modelHash);
			static bool IsModelAnAmphibiousQuadBike(int modelHash);
			static bool IsModelABlimp(int modelHash);
			static bool IsModelATrailer(int modelHash);
			static int* _cursorSpriteAddr;
			static property System::IntPtr CellEmailBcon{
				System::IntPtr get();
			}
			static property System::IntPtr StringPtr {
				System::IntPtr get();
			}
			static property System::IntPtr NullString {
				System::IntPtr get();
			}
		private:
			static System::IntPtr _cellEmailBconPtr;
			static System::IntPtr _stringPtr;
			static System::IntPtr _nullString;
			static MemoryAccess();
			static unsigned long long GetEntitySkeletonData(int handle);
			static unsigned long long FindCModelInfo(int modelHash);
			static ModelInfoClassType GetModelInfoClass(System::UInt64 address);
			static VehicleStructClassType GetVehicleStructClass(System::UInt64 modelInfoAddress);
			static void GenerateVehicleModelList();
			static System::UInt64 modelHashTable, modelNum2, modelNum3, modelNum4;
			static int modelNum1, vehClassOff;
			static unsigned short modelHashEntries;
			static System::Collections::ObjectModel::ReadOnlyCollection<System::Collections::ObjectModel::ReadOnlyCollection<int> ^> ^vehicleModels;
			static System::UInt64 FindPattern(const char *pattern, const char *mask);
		};
	}
}
