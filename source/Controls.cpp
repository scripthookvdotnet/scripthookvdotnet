#include "Controls.hpp"
#include "Native.hpp"

namespace GTA
{
	bool Controls::IsControlEnabled(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CONTROL_ENABLED, index, static_cast<int>(control));
	}
	bool Controls::IsControlPressed(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CONTROL_PRESSED, index, static_cast<int>(control));
	}
	bool Controls::IsControlReleased(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CONTROL_RELEASED, index, static_cast<int>(control));
	}
	bool Controls::IsControlJustPressed(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CONTROL_JUST_PRESSED, index, static_cast<int>(control));
	}
	bool Controls::IsControlJustReleased(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CONTROL_JUST_RELEASED, index, static_cast<int>(control));
	}
	bool Controls::IsDisabledControlPressed(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_DISABLED_CONTROL_PRESSED, index, static_cast<int>(control));
	}
	bool Controls::IsDisabledControlJustPressed(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_DISABLED_CONTROL_JUST_PRESSED, index, static_cast<int>(control));
	}
	bool Controls::IsDisabledControlJustReleased(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_DISABLED_CONTROL_JUST_RELEASED, index, static_cast<int>(control));
	}
}