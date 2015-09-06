#include "Scaleform.hpp"
#include "Native.hpp"

namespace GTA
{
	extern void Log(System::String ^logLevel, ... array<System::String ^> ^message);

	Scaleform::Scaleform()
	{
	}
	Scaleform::Scaleform(int handle) : mHandle(handle)
	{
	}

	int Scaleform::Handle::get()
	{
		return this->mHandle;
	}

	bool Scaleform::Load(System::String ^scaleformID)
	{
		const int handle = Native::Function::Call<int>(Native::Hash::REQUEST_SCALEFORM_MOVIE, scaleformID);

		if (handle == 0)
		{
			return false;
		}

		this->mHandle = handle;
		this->mScaleformID = scaleformID;

		return true;
	}

	void Scaleform::CallFunction(System::String ^function, ... array<System::Object ^> ^arguments)
	{
		Native::Function::Call(Native::Hash::_PUSH_SCALEFORM_MOVIE_FUNCTION, mHandle, function);

		for each(System::Object ^o in arguments)
		{
			if (o->GetType() == int::typeid)
			{
				Native::Function::Call(Native::Hash::_PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_INT, static_cast<int>(o));
			}
			else if (o->GetType() == System::String::typeid)
			{
				Native::Function::Call(Native::Hash::_BEGIN_TEXT_COMPONENT, "STRING");
				Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, static_cast<System::String ^>(o));
				Native::Function::Call(Native::Hash::_END_TEXT_COMPONENT);
			}
			else if (o->GetType() == System::Char::typeid)
			{
				Native::Function::Call(Native::Hash::_BEGIN_TEXT_COMPONENT, "STRING");
				Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, static_cast<char>(o).ToString());
				Native::Function::Call(Native::Hash::_END_TEXT_COMPONENT);
			}
			else if (o->GetType() == System::Single::typeid)
			{
				Native::Function::Call(Native::Hash::_PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_FLOAT, static_cast<float>(o));
			}
			else if (o->GetType() == System::Double::typeid)
			{
				Native::Function::Call(Native::Hash::_PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_FLOAT, static_cast<float>(static_cast<double>(o)));
			}
			else if (o->GetType() == System::Boolean::typeid)
			{
				Native::Function::Call(Native::Hash::_PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_BOOL, static_cast<bool>(o));
			}
			else if (o->GetType() == ScaleformArgumentTXD::typeid)
			{
				Native::Function::Call(Native::Hash::_PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_STRING, static_cast<ScaleformArgumentTXD ^>(o)->txd);
			}
			else
			{
				Log("[ERROR]", System::String::Format("Unknown argument type {0} passed to scaleform with handle {1}.", o->GetType()->Name, this->mHandle));
			}
		}

		Native::Function::Call(Native::Hash::_POP_SCALEFORM_MOVIE_FUNCTION_VOID);
	}

	void Scaleform::Render2D()
	{
		Native::Function::Call(Native::Hash::_0xCF537FDE4FBD4CE5, this->mHandle, 255, 255, 255, 255);
	}
	void Scaleform::Render2DScreenSpace(System::Drawing::PointF location, System::Drawing::PointF size)
	{
		float x = location.X / 1280.0f;
		float y = location.Y / 720.0f;
		float width = size.X / 1280.0f;
		float height = size.Y / 720.0f;

		Native::Function::Call(Native::Hash::DRAW_SCALEFORM_MOVIE, this->mHandle, x + (width / 2.0f), y + (height / 2.0f), width, height, 255, 255, 255, 255);
	}
	void Scaleform::Render3D(GTA::Math::Vector3 position, GTA::Math::Vector3 rotation, GTA::Math::Vector3 scale)
	{
		Native::Function::Call(Native::Hash::_0x1CE592FDC749D6F5, this->mHandle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2.0f, 2.0f, 1.0f, scale.X, scale.Y, scale.Z, 2);
	}
	void Scaleform::Render3DAdditive(GTA::Math::Vector3 position, GTA::Math::Vector3 rotation, GTA::Math::Vector3 scale)
	{
		Native::Function::Call(Native::Hash::_0x87D51D72255D4E78, this->mHandle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2.0f, 2.0f, 1.0f, scale.X, scale.Y, scale.Z, 2);
	}
}