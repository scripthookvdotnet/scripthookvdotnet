#pragma once

#include "ScriptChild.hpp"
#include "Vector3.hpp"
#include "Entity.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Entity;
	#pragma endregion

	public ref class Light sealed : public ScriptChild
	{
	public:
		Light(Math::Vector3 position, System::Drawing::Color color, float range, float intensity);

		property bool IsEnabled
		{
			bool get();
			void set(bool value);
		}
		property System::Drawing::Color Color
		{
			System::Drawing::Color get();
			void set(System::Drawing::Color value);
		}
		property Math::Vector3 Position
		{
			Math::Vector3 get();
			void set(Math::Vector3 value);
		}
		property float Range
		{
			float get();
			void set(float value);
		}
		property float Intensity
		{
			float get();
			void set(float value);
		}
		property bool IsAttached
		{
			bool get();
		}

		Entity ^GetEntityAttachedTo();
		void AttachTo(Entity ^entity);
		void AttachTo(Entity ^entity, Math::Vector3 offset);
		void AttachTo(Entity ^entity, Math::Vector3 offset, bool relative);

	private:
		bool _isEnabled;
		Math::Vector3 _position;
		System::Drawing::Color _color;
		float _range;
		float _intensity;
		bool _isAttached;
		Entity ^_entity;
		Math::Vector3 _offset;
		bool _relative;

		void PerFrameDrawing(System::Object ^sender, System::EventArgs ^e);
	};
}