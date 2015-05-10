#pragma once

namespace GTA
{
	public ref class Streaming
	{
	public:
		static void RequestAnimDict(System::String ^animSet);
		static bool HasAnimDictLoaded(System::String ^animSet);
	};
}