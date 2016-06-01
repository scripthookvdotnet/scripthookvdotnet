#pragma once

#include "UI.hpp"

namespace GTA
{
	namespace UI
	{
		public interface class IElement
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
			property bool Centered
			{
				bool get();
				void set(bool value);
			}
		};
		public enum class TextAlignment
		{
			Center = 0,
			Left = 1,
			Right = 2,
		};
		public ref class Text : public IElement
		{
		public:
			Text(System::String ^caption, System::Drawing::PointF position, float scale);
			Text(System::String ^caption, System::Drawing::PointF position, float scale, System::Drawing::Color color);
			Text(System::String ^caption, System::Drawing::PointF position, float scale, System::Drawing::Color color, Font font, TextAlignment alignment);
			Text(System::String ^caption, System::Drawing::PointF position, float scale, System::Drawing::Color color, Font font, TextAlignment alignment, bool shadow, bool outline);
			Text(System::String ^caption, System::Drawing::PointF position, float scale, System::Drawing::Color color, Font font, TextAlignment alignment, bool shadow, bool outline, float wrapWidth);

			virtual property bool Enabled;
			virtual property System::Drawing::PointF Position;
			virtual property System::Drawing::Color Color;
			property System::String ^Caption;
			property Font Font;
			property float Scale;
			property TextAlignment Alignment;
			virtual property bool Centered
			{
				virtual bool get();
				virtual void set(bool value);
			}
			
			property bool Shadow;
			property bool Outline;
			property float WrapWidth;

			virtual void Draw();
			virtual void Draw(System::Drawing::SizeF offset);
		};
		public ref class Rectangle : public IElement
		{
		public:
			Rectangle();
			Rectangle(System::Drawing::PointF position, System::Drawing::SizeF size);
			Rectangle(System::Drawing::PointF position, System::Drawing::SizeF size, System::Drawing::Color color);
			Rectangle(System::Drawing::PointF position, System::Drawing::SizeF size, System::Drawing::Color color, bool centered);

			virtual property bool Enabled;
			virtual property System::Drawing::PointF Position;
			property System::Drawing::SizeF Size;
			virtual property System::Drawing::Color Color;
			virtual property bool Centered;

			virtual void Draw();
			virtual void Draw(System::Drawing::SizeF offset);
		};
		public ref class Container : public Rectangle
		{
		public:
			Container();
			Container(System::Drawing::PointF position, System::Drawing::SizeF Size);
			Container(System::Drawing::PointF position, System::Drawing::SizeF Size, System::Drawing::Color color);
			Container(System::Drawing::PointF position, System::Drawing::SizeF Size, System::Drawing::Color color, bool centered);

			property System::Collections::Generic::List<IElement ^> ^Items
			{
				System::Collections::Generic::List<IElement ^> ^get();
				void set(System::Collections::Generic::List<IElement ^> ^value);
			}
					
			virtual void Draw() override;
			virtual void Draw(System::Drawing::SizeF offset) override;

		private:
			System::Collections::Generic::List<IElement ^> ^_items;
		};
		public interface class ISprite : public IElement
		{
		public:
			property System::Drawing::SizeF Scale;
			property float Rotation
			{
				float get();
				void set(float value);
			}
		};
		public ref class Sprite : public ISprite
		{
		public:
			Sprite(System::String ^textureDict, System::String ^textureName, System::Drawing::SizeF scale, System::Drawing::PointF position);
			Sprite(System::String ^textureDict, System::String ^textureName, System::Drawing::SizeF scale, System::Drawing::PointF position, System::Drawing::Color color);
			Sprite(System::String ^textureDict, System::String ^textureName, System::Drawing::SizeF scale, System::Drawing::PointF position, System::Drawing::Color color, float rotation);
			Sprite(System::String ^textureDict, System::String ^textureName, System::Drawing::SizeF scale, System::Drawing::PointF position, System::Drawing::Color color, float rotation, bool centered);
			virtual ~Sprite();

			virtual property bool Enabled;
			virtual property System::Drawing::PointF Position;
			virtual property System::Drawing::Color Color;
			virtual property System::Drawing::SizeF Scale;
			virtual property float Rotation;
			virtual property bool Centered;

			virtual void Draw();
			virtual void Draw(System::Drawing::SizeF offset);

		private:
			static void AddInstance(System::String ^textureDict);
			System::String ^_textureDict;
			System::String ^_textureName;
			static System::Collections::Generic::Dictionary<System::String ^, int> ^_textureDicts = gcnew System::Collections::Generic::Dictionary<System::String ^, int>();
			
		};
		public ref class CustomSprite : public ISprite
		{
		public:
			CustomSprite(System::String ^filename, System::Drawing::SizeF scale, System::Drawing::PointF position);
			CustomSprite(System::String ^filename, System::Drawing::SizeF scale, System::Drawing::PointF position, System::Drawing::Color color);
			CustomSprite(System::String ^filename, System::Drawing::SizeF scale, System::Drawing::PointF position, System::Drawing::Color color, float rotation);
			CustomSprite(System::String ^filename, System::Drawing::SizeF scale, System::Drawing::PointF position, System::Drawing::Color color, float rotation, bool centered);

			virtual property bool Enabled;
			virtual property System::Drawing::PointF Position;
			virtual property System::Drawing::Color Color;
			virtual property System::Drawing::SizeF Scale;
			virtual property float Rotation;
			virtual property bool Centered;


			virtual void Draw();
			virtual void Draw(System::Drawing::SizeF offset);
		private:
			int _id;
			static System::Collections::Generic::Dictionary<System::String ^, int> ^_textures = gcnew System::Collections::Generic::Dictionary<System::String ^, int>();
			static System::Collections::Generic::Dictionary<int, int> ^_lastDraw = gcnew System::Collections::Generic::Dictionary<int, int>();
			static System::Collections::Generic::Dictionary<int, int> ^_indexes = gcnew System::Collections::Generic::Dictionary<int, int>();
			static int _globalLastDrawFrame = 0;
			static int _level;

		};
	}
}