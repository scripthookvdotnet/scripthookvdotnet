#include "Controls.hpp"
#include "Native.hpp"

namespace GTA
{
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

	bool Controls::IsControlEnabled(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CONTROL_ENABLED, index, static_cast<int>(control));
	}
	bool Controls::IsControlPressed(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CONTROL_PRESSED, index, static_cast<int>(control));
	}
	bool Controls::IsControlJustPressed(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CONTROL_JUST_PRESSED, index, static_cast<int>(control));
	}
	bool Controls::IsControlJustReleased(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CONTROL_JUST_RELEASED, index, static_cast<int>(control));
	}

	bool Controls::IsLookInverted()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_LOOK_INVERTED);
	}

	void Controls::EnableControlThisFrame(int index, Control control)
	{
		Native::Function::Call(Native::Hash::ENABLE_CONTROL_ACTION, index, static_cast<int>(control), true);
	}
	void Controls::DisableControlThisFrame(int index, Control control)
	{
		Native::Function::Call(Native::Hash::DISABLE_CONTROL_ACTION, index, static_cast<int>(control), true);
	}

	void Controls::DisableAllControlsThisFrame(int index)
	{
		Native::Function::Call(Native::Hash::DISABLE_ALL_CONTROL_ACTIONS, index);
	}
	void Controls::EnableAllControlsThisFrame(int index)
	{
		Native::Function::Call(Native::Hash::ENABLE_ALL_CONTROL_ACTIONS, index);
	}

	float Controls::GetDisabledControlNormal(int index, Control control)
	{
		return Native::Function::Call<float>(Native::Hash::GET_DISABLED_CONTROL_NORMAL, index, static_cast<int>(control));
	}
	float Controls::GetControlNormal(int index, Control control)
	{
		return Native::Function::Call<float>(Native::Hash::GET_CONTROL_NORMAL, index, static_cast<int>(control));
	}
	int Controls::GetControlValue(int index, Control control)
	{
		return Native::Function::Call<int>(Native::Hash::GET_CONTROL_VALUE, index, static_cast<int>(control));
	}

	void Controls::SetControlNormal(int index, Control control, float value)
	{
		Native::Function::Call(Native::Hash::_SET_CONTROL_NORMAL, index, static_cast<int>(control), value);
	}
}