/**
* Copyright (C) 2015 crosire
*
* This software is  provided 'as-is', without any express  or implied  warranty. In no event will the
* authors be held liable for any damages arising from the use of this software.
* Permission  is granted  to anyone  to use  this software  for  any  purpose,  including  commercial
* applications, and to alter it and redistribute it freely, subject to the following restrictions:
*
*   1. The origin of this software must not be misrepresented; you must not claim that you  wrote the
*      original  software. If you use this  software  in a product, an  acknowledgment in the product
*      documentation would be appreciated but is not required.
*   2. Altered source versions must  be plainly  marked as such, and  must not be  misrepresented  as
*      being the original software.
*   3. This notice may not be removed or altered from any source distribution.
*/

namespace ScriptHookWrapper
{
	public ref class Wrapper sealed abstract
	{
	public:

		static property int GameVersion
		{
			int get();
		};

		static System::IntPtr GetGlobalPtr(int globalId);
		static void NativeInit(System::UInt64 hash);
		static void NativePush64(System::UInt64 value);
		static System::UInt64* NativeCall();
		static int CreateTexture(System::String ^filename);
		static void DrawTexture(int id, int index, int level, int time, float sizeX, float sizeY, float centerX, float centerY, float posX, float posY, float rotation, float scaleFactor, System::Drawing::Color color);
	};
}