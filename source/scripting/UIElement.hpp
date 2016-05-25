#pragma once

#include "UI.hpp"

namespace GTA
{
	namespace UI
	{
		public interface class Element
		{
			void Draw();
			void Draw(System::Drawing::SizeF offset);

			property bool Enabled
			{
				bool get();
				void set(bool value);
			}
			property System::Drawing::PointF Position
			{
				System::Drawing::PointF get();
				void set(System::Drawing::PointF value);
			}
			property System::Drawing::Color Color
			{
				System::Drawing::Color get();
				void set(System::Drawing::Color value);
			}
		};

		public ref class Text : public Element
		{
		public:
			Text(System::String ^caption, System::Drawing::PointF position, float scale);
			Text(System::String ^caption, System::Drawing::PointF position, float scale, System::Drawing::Color color);
			Text(System::String ^caption, System::Drawing::PointF position, float scale, System::Drawing::Color color, Font font, bool centered);
			Text(System::String ^caption, System::Drawing::PointF position, float scale, System::Drawing::Color color, Font font, bool centered, bool shadow, bool outline);

			virtual property bool Enabled;
			virtual property System::Drawing::PointF Position;
			virtual property System::Drawing::Color Color;
			property System::String ^Caption;
			property Font Font;
			property float Scale;
			property bool Centered;
			property bool Shadow;
			property bool Outline;

			virtual void Draw();
			virtual void Draw(System::Drawing::SizeF offset);
		};
		public ref class Rectangle : public Element
		{
		public:
			Rectangle();
			Rectangle(System::Drawing::PointF position, System::Drawing::SizeF size);
			Rectangle(System::Drawing::PointF position, System::Drawing::SizeF size, System::Drawing::Color color);

			virtual property bool Enabled;
			virtual property System::Drawing::PointF Position;
			property System::Drawing::SizeF Size;
			virtual property System::Drawing::Color Color;

			virtual void Draw();
			virtual void Draw(System::Drawing::SizeF offset);
		};
		public ref class Container : public Rectangle
		{
		public:
			Container();
			Container(System::Drawing::PointF position, System::Drawing::SizeF Size);
			Container(System::Drawing::PointF position, System::Drawing::SizeF Size, System::Drawing::Color color);

			property System::Collections::Generic::List<Element ^> ^Items
			{
				System::Collections::Generic::List<Element ^> ^get();
				void set(System::Collections::Generic::List<Element ^> ^value);
			}

			virtual void Draw() override;
			virtual void Draw(System::Drawing::SizeF offset) override;

		private:
			System::Collections::Generic::List<Element ^> ^_items;
		};
		public ref class Sprite : public Element
		{
		public:
			Sprite(System::String ^textureDict, System::String ^textureName, System::Drawing::SizeF scale, System::Drawing::PointF position);
			Sprite(System::String ^textureDict, System::String ^textureName, System::Drawing::SizeF scale, System::Drawing::PointF position, System::Drawing::Color color);
			Sprite(System::String ^textureDict, System::String ^textureName, System::Drawing::SizeF scale, System::Drawing::PointF position, System::Drawing::Color color, float rotation);

			virtual ~Sprite();

			virtual property bool Enabled;
			virtual property System::Drawing::PointF Position;
			virtual property System::Drawing::Color Color;
			property System::Drawing::SizeF Scale;
			property float Rotation;

			virtual void Draw();
			virtual void Draw(System::Drawing::SizeF offset);

		private:
			System::String ^_textureDict;
			System::String ^_textureName;
		};
		public ref class CustomSprite : public Element
		{
		public:
			CustomSprite(System::String ^filename, System::Drawing::SizeF scale, System::Drawing::PointF position);
			CustomSprite(System::String ^filename, System::Drawing::SizeF scale, System::Drawing::PointF position, System::Drawing::Color color);
			CustomSprite(System::String ^filename, System::Drawing::SizeF scale, System::Drawing::PointF position, System::Drawing::Color color, float rotation);

			virtual property bool Enabled;
			virtual property System::Drawing::PointF Position;
			virtual property System::Drawing::Color Color;
			property System::Drawing::SizeF Scale;
			property float Rotation;


			virtual void Draw();
			virtual void Draw(System::Drawing::SizeF offset);
		private:
			int _index;
			int _lastDrawFrame;
			int Id;
			static System::Collections::Generic::Dictionary<System::String ^, int> ^_textures = gcnew System::Collections::Generic::Dictionary<System::String ^, int>();

			static int _globalLastDrawFrame = 0;
			static int _level;

		};
	}
}