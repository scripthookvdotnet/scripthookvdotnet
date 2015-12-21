#include "Light.hpp"
#include "World.hpp"
#include "Script.hpp"
#include "Native.hpp"

namespace GTA
{
	using namespace System;

	Light::Light(Math::Vector3 position, Drawing::Color color, float range, float intensity)
	{
		_isEnabled = true;
		_position = position;
		_color = color;
		_range = range;
		_intensity = intensity;

		_isAttached = false;
		_entity = nullptr;
		_offset = Math::Vector3::Zero;
		_relative = true;

		Parent->PerFrameScriptDrawing += gcnew EventHandler(this, &Light::PerFrameDrawing);
	}
	Light::Light(Entity ^entityAttachedTo, Drawing::Color color, float range, float intensity)
	{
		_isEnabled = true;
		_position = Math::Vector3::Zero;
		_color = color;
		_range = range;
		_intensity = intensity;

		_isAttached = true;
		_entity = entityAttachedTo;
		_offset = Math::Vector3::Zero;
		_relative = true;

		Parent->PerFrameScriptDrawing += gcnew EventHandler(this, &Light::PerFrameDrawing);
	}
	Light::Light(Entity ^entityAttachedTo, Math::Vector3 offset, Drawing::Color color, float range, float intensity)
	{
		_isEnabled = true;
		_position = Math::Vector3::Zero;
		_color = color;
		_range = range;
		_intensity = intensity;

		_isAttached = true;
		_entity = entityAttachedTo;
		_offset = offset;
		_relative = true;

		Parent->PerFrameScriptDrawing += gcnew EventHandler(this, &Light::PerFrameDrawing);
	}
	Light::Light(Entity ^entityAttachedTo, Math::Vector3 offset, bool relativeAttaching, Drawing::Color color, float range, float intensity)
	{
		_isEnabled = true;
		_position = Math::Vector3::Zero;
		_color = color;
		_range = range;
		_intensity = intensity;

		_isAttached = true;
		_entity = entityAttachedTo;
		_offset = offset;
		_relative = relativeAttaching;

		Parent->PerFrameScriptDrawing += gcnew EventHandler(this, &Light::PerFrameDrawing);
	}

	bool Light::IsEnabled::get()
	{
		return _isEnabled;
	}
	void Light::IsEnabled::set(bool value) 
	{
		if (value == _isEnabled)
		{
			return;
		}

		if (value)
		{		
			Parent->PerFrameScriptDrawing += gcnew EventHandler(this, &Light::PerFrameDrawing);
		}
		else
		{
			Parent->PerFrameScriptDrawing -= gcnew EventHandler(this, &Light::PerFrameDrawing);
		}

		_isEnabled = value;
	}
	Drawing::Color Light::Color::get()
	{
		return _color;
	}
	void Light::Color::set(Drawing::Color value)
	{
		_color = value;
	}
	Math::Vector3 Light::Position::get()
	{
		return _position;
	}
	void Light::Position::set(Math::Vector3 value)
	{
		_position = value;
	}
	float Light::Range::get()
	{
		return _range;
	}
	void Light::Range::set(float value)
	{
		_range = value;
	}
	float Light::Intensity::get()
	{
		return _intensity;
	}
	void Light::Intensity::set(float value)
	{
		_intensity = value;
	}
	bool Light::IsAttached::get()
	{
		return _isAttached;
	}

	Entity ^Light::GetEntityAttachedTo()
	{
		if (Entity::Exists(_entity))
		{
			return _entity;
		}
		else
		{
			return nullptr;
		}
	}
	void Light::AttachTo(Entity ^entity)
	{
		Light::AttachTo(entity, Math::Vector3::Zero, true);
	}
	void Light::AttachTo(Entity ^entity, Math::Vector3 offset)
	{
		Light::AttachTo(entity, offset, true);
	}
	void Light::AttachTo(Entity ^entity, Math::Vector3 offset, bool relative)
	{
		if (!Entity::Exists(entity))
		{
			return;
		}

		_entity = entity;
		_offset = offset;
		_relative = relative;
		_isAttached = true;
	}

	void Light::PerFrameDrawing(System::Object^ sender, System::EventArgs^ e)
	{
		if (_isAttached) 
		{
			if (Entity::Exists(_entity))
			{
				if (_relative)
				{
					_position = _entity->GetOffsetInWorldCoords(_offset);
				}
				else
				{
					_position = _entity->Position + _offset;
				}
			}
			else
			{
				IsEnabled = false;
				_entity = nullptr;
				_isAttached = false;
			}
		}

		World::DrawLightWithRange(_position, _color, _range, _intensity);
	}
}