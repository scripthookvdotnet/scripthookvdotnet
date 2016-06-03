#include "Checkpoint.hpp"
#include "Native.hpp"

namespace GTA
{
    using namespace Math;
    using namespace System;

    Checkpoint::Checkpoint(CheckpointIcon icon, Math::Vector3 position, Math::Vector3 pointTo, float radius, System::Drawing::Color color)
        : Checkpoint(icon, position, pointTo, radius, color, 0)
    {
    }
    Checkpoint::Checkpoint(CheckpointIcon icon, Math::Vector3 position, Math::Vector3 pointTo, float radius, System::Drawing::Color color, int reserved) : _icon(icon), _position(position), _pointTo(pointTo), _radius(radius), _color(color), _reserved(reserved)
    {
        _handle = Native::Function::Call<int>(Native::Hash::CREATE_CHECKPOINT, static_cast<int>(icon), position.X, position.Y, position.Z, pointTo.X, pointTo.Y, pointTo.Z, radius, color.R, color.G, color.B, color.A, reserved);
    }
    Checkpoint::~Checkpoint()
    {
        Delete();
    }

    int Checkpoint::Handle::get()
    {
        return _handle;
    }
    CheckpointIcon Checkpoint::Icon::get()
    {
        return _icon;
    }
    Vector3 Checkpoint::Position::get()
    {
        return _position;
    }
    Vector3 Checkpoint::PointTo::get()
    {
        return _pointTo;
    }
    float Checkpoint::Radius::get()
    {
        return _radius;
    }
    System::Drawing::Color Checkpoint::Color::get()
    {
        return _color;
    }
    void Checkpoint::Color::set(System::Drawing::Color color)
    {
        Native::Function::Call(Native::Hash::SET_CHECKPOINT_RGBA, Handle, color.R, color.G, color.B, color.A);
        _color = color;
    }

    void Checkpoint::SetCylinderHeight(float nearHeight, float farHeight, float radius)
    {
        Native::Function::Call(Native::Hash::SET_CHECKPOINT_CYLINDER_HEIGHT, Handle, nearHeight, farHeight, radius);
        _radius = radius;
    }
    void Checkpoint::SetIconColor(System::Drawing::Color color)
    {
        Native::Function::Call(Native::Hash::_SET_CHECKPOINT_ICON_RGBA, Handle, color.R, color.G, color.B, color.A);
    }

    void Checkpoint::Delete()
    {
        if (Exists())
        {
            Native::Function::Call(Native::Hash::DELETE_CHECKPOINT, Handle);
            _handle = -1;
        }
    }

    bool Checkpoint::Exists()
    {
        return (_handle != -1);
    }
    bool Checkpoint::Exists(Checkpoint ^checkpoint)
    {
        return checkpoint->Exists();
    }

    bool Checkpoint::Equals(System::Object ^obj)
    {
        if (obj == nullptr || obj->GetType() != GetType())
            return false;

        return Equals(safe_cast<Checkpoint ^>(obj));
    }
    bool Checkpoint::Equals(GTA::Checkpoint ^checkpoint)
    {
        return !Object::ReferenceEquals(checkpoint, nullptr) && Handle == checkpoint->Handle;
    }
}