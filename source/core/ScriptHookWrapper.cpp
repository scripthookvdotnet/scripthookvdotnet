#include "ScriptHookWrapper.hpp"

#include <Main.h>

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Collections::ObjectModel;
using namespace System::Runtime::InteropServices;

namespace ScriptHookWrapper
{
	int Wrapper::GameVersion::get()
	{
		return getGameVersion();
	}

	System::IntPtr Wrapper::GetGlobalPtr(int globalId)
	{
		return IntPtr(getGlobalPtr(globalId));
	}
	void Wrapper::NativeInit(System::UInt64 hash)
	{
		nativeInit(hash);
	}
	void Wrapper::NativePush64(System::UInt64 value)
	{
		nativePush64(value);
	}
	System::UInt64* Wrapper::NativeCall()
	{
		return nativeCall();
	}
	int Wrapper::CreateTexture(System::String ^fileName)
	{
		array<Byte>^ utf8Bytes = System::Text::Encoding::UTF8->GetBytes(fileName + "\0");
		pin_ptr<Byte> pinnedBytes = &utf8Bytes[0];

		return createTexture(reinterpret_cast<const char *>(pinnedBytes));
	}
	void Wrapper::DrawTexture(int id, int index, int level, int time, float sizeX, float sizeY, float centerX, float centerY, float posX, float posY, float rotation, float scaleFactor, Drawing::Color color)
	{
		drawTexture(id, index, level, time, sizeX, sizeY, centerX, centerY, posX, posY, rotation, scaleFactor, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
	}
}