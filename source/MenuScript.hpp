#pragma once

#include "Script.hpp"
#include "Viewport.hpp"

namespace GTA
{
	public ref class MenuScript : Script {
	public:
		MenuScript();
		Viewport ^View;

	internal:
		void UpdateViewport(Object ^Sender, System::EventArgs ^Args);
		void HandleViewportInput(System::Object ^sender, System::Windows::Forms::KeyEventArgs ^e);
	};
}
