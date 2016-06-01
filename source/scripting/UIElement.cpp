#include "UIElement.hpp"
#include "Native.hpp"
#include "ScriptDomain.hpp"

#include <Main.h>

namespace GTA
{
	namespace UI
	{
		using namespace System;
		using namespace System::Collections::Generic;

		Text::Text(String ^caption, Drawing::PointF position, float scale)
		{
			Enabled = true;
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = Drawing::Color::WhiteSmoke;
			Font = GTA::UI::Font::ChaletLondon;
			Alignment = TextAlignment::Left;
			Shadow = false;
			Outline = false;
			WrapWidth = 0.0f;
		}
		Text::Text(String ^caption, Drawing::PointF position, float scale, Drawing::Color color)
		{
			Enabled = true;
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = GTA::UI::Font::ChaletLondon;
			Alignment = TextAlignment::Left;
			Shadow = false;
			Outline = false;
			WrapWidth = 0.0f;
		}
		Text::Text(String ^caption, Drawing::PointF position, float scale, Drawing::Color color, GTA::UI::Font font, TextAlignment alignment)
		{
			Enabled = true;
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = font;
			Alignment = alignment;
			Shadow = false;
			Outline = false;
			WrapWidth = 0.0f;
		}
		Text::Text(String ^caption, Drawing::PointF position, float scale, Drawing::Color color, GTA::UI::Font font, TextAlignment alignment, bool shadow, bool outline)
		{
			Enabled = true;
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = font;
			Alignment = alignment;
			Shadow = shadow;
			Outline = outline;
			WrapWidth = 0.0f;
		}
		Text::Text(String ^caption, Drawing::PointF position, float scale, Drawing::Color color, GTA::UI::Font font, TextAlignment alignment, bool shadow, bool outline, float wrapWidth)
		{
			Enabled = true;
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = font;
			Alignment = alignment;
			Shadow = shadow;
			Outline = outline;
			WrapWidth = wrapWidth;
		}

		bool Text::Centered::get()
		{
			return Alignment == TextAlignment::Center;
		}
		void Text::Centered::set(bool value)
		{
			if (value)
			{
				Alignment = TextAlignment::Center;
			}
		}
		
		void Text::Draw()
		{
			Draw(Drawing::SizeF::Empty);
		}
		void Text::Draw(Drawing::SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			const float x = (Position.X + offset.Width) / Screen::WIDTH;
			const float y = (Position.Y + offset.Height) / Screen::HEIGHT;
			const float w = WrapWidth / Screen::WIDTH;

			if (Shadow)
			{
				Native::Function::Call(Native::Hash::SET_TEXT_DROP_SHADOW);
			}
			if (Outline)
			{
				Native::Function::Call(Native::Hash::SET_TEXT_OUTLINE);
			}
			Native::Function::Call(Native::Hash::SET_TEXT_FONT, static_cast<int>(Font));
			Native::Function::Call(Native::Hash::SET_TEXT_SCALE, Scale, Scale);
			Native::Function::Call(Native::Hash::SET_TEXT_COLOUR, Color.R, Color.G, Color.B, Color.A);
			Native::Function::Call(Native::Hash::SET_TEXT_JUSTIFICATION, static_cast<int>(Alignment));
			if (WrapWidth > 0.0f)
			{
				switch (Alignment)
				{
					case TextAlignment::Center:
						Native::Function::Call(Native::Hash::SET_TEXT_WRAP, x - (w / 2), x + (w / 2));
						break;
					case TextAlignment::Left:
						Native::Function::Call(Native::Hash::SET_TEXT_WRAP, x, x + w);
						break;
					case TextAlignment::Right:
						Native::Function::Call(Native::Hash::SET_TEXT_WRAP, x - w, x);
						break;
				}
			}
			else if (Alignment == TextAlignment::Right)
			{
				Native::Function::Call(Native::Hash::SET_TEXT_WRAP, 0.0f, x);
			}
			Native::Function::Call(Native::Hash::_SET_TEXT_ENTRY, "CELL_EMAIL_BCON");
			const int strLen = 99;
			for (int i = 0; i < Caption->Length; i += strLen)
			{
				System::String ^substr = Caption->Substring(i, System::Math::Min(strLen, Caption->Length - i));
				Native::Function::Call(Native::Hash::ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, substr);
			}
			Native::Function::Call(Native::Hash::_DRAW_TEXT, x, y);
		}

		Rectangle::Rectangle()
		{
			Enabled = true;
			Position = Drawing::PointF();
			Size = Drawing::SizeF(Screen::WIDTH, Screen::HEIGHT);
			Color = Drawing::Color::Transparent;
			Centered = false;
		}
		Rectangle::Rectangle(Drawing::PointF position, Drawing::SizeF size)
		{
			Enabled = true;
			Position = position;
			Size = size;
			Color = Drawing::Color::Transparent;
			Centered = false;
		}
		Rectangle::Rectangle(Drawing::PointF position, Drawing::SizeF size, Drawing::Color color)
		{
			Enabled = true;
			Position = position;
			Size = size;
			Color = color;
			Centered = false;
		}
		Rectangle::Rectangle(Drawing::PointF position, Drawing::SizeF size, Drawing::Color color, bool centered)
		{
			Enabled = true;
			Position = position;
			Size = size;
			Color = color;
			Centered = centered;
		}

		void Rectangle::Draw()
		{
			Draw(Drawing::SizeF::Empty);
		}
		void Rectangle::Draw(Drawing::SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			const float w = Size.Width / Screen::WIDTH;
			const float h = Size.Height / Screen::HEIGHT;
			const float x = ((Position.X + offset.Width) / Screen::WIDTH) + ((!Centered) ?  w * 0.5f : 0.0f);
			const float y = ((Position.Y + offset.Height) / Screen::HEIGHT) + ((!Centered) ? h * 0.5f : 0.0f);

			Native::Function::Call(Native::Hash::DRAW_RECT, x, y, w, h, Color.R, Color.G, Color.B, Color.A);
		}

		Container::Container() : Rectangle(), _items(gcnew List<IElement ^>())
		{
		}
		Container::Container(Drawing::PointF position, Drawing::SizeF size) : Rectangle(position, size), _items(gcnew List<IElement ^>())
		{
		}
		Container::Container(Drawing::PointF position, Drawing::SizeF size, Drawing::Color color) : Rectangle(position, size, color), _items(gcnew List<IElement ^>())
		{
		}
		Container::Container(Drawing::PointF position, Drawing::SizeF size, Drawing::Color color, bool centered) : Rectangle(position, size, color, centered), _items(gcnew List<IElement ^>())
		{
		}

		List<IElement ^> ^Container::Items::get()
		{
			return _items;
		}
		void Container::Items::set(List<IElement ^> ^value)
		{
			_items = value;
		}

		void Container::Draw()
		{
			Draw(Drawing::SizeF::Empty);
		}
		void Container::Draw(Drawing::SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			Rectangle::Draw(offset);

			Drawing::SizeF newOfset = Drawing::SizeF(Rectangle::Position + offset);
			if (Centered)
			{
				newOfset -= Drawing::SizeF(Size.Width / 2.0f, Size.Height / 2.0f);
			}
			for each (IElement ^item in Items)
			{
				item->Draw(newOfset);
			}
		}

		Sprite::Sprite(String ^textureDict, String ^textureName, Drawing::SizeF scale, Drawing::PointF position)
		{
			Enabled = true;
			_textureDict = textureDict;
			_textureName = textureName;
			Scale = scale;
			Position = position;
			Color = Drawing::Color::White;
			Rotation = 0.0F;
			Centered = false;
			Native::Function::Call(Native::Hash::REQUEST_STREAMED_TEXTURE_DICT, _textureDict);
		}
		Sprite::Sprite(String ^textureDict, String ^textureName, Drawing::SizeF scale, Drawing::PointF position, Drawing::Color color)
		{
			Enabled = true;
			_textureDict = textureDict;
			_textureName = textureName;
			Scale = scale;
			Position = position;
			Color = color;
			Rotation = 0.0F;
			Centered = false;
			Native::Function::Call(Native::Hash::REQUEST_STREAMED_TEXTURE_DICT, _textureDict);
		}
		Sprite::Sprite(String ^textureDict, String ^textureName, Drawing::SizeF scale, Drawing::PointF position, Drawing::Color color, float rotation)
		{
			Enabled = true;
			_textureDict = textureDict;
			_textureName = textureName;
			Scale = scale;
			Position = position;
			Color = color;
			Rotation = rotation;
			Centered = false;
			Native::Function::Call(Native::Hash::REQUEST_STREAMED_TEXTURE_DICT, _textureDict);
		}
		Sprite::Sprite(String ^textureDict, String ^textureName, Drawing::SizeF scale, Drawing::PointF position, Drawing::Color color, float rotation, bool centered)
		{
			Enabled = true;
			_textureDict = textureDict;
			_textureName = textureName;
			Scale = scale;
			Position = position;
			Color = color;
			Rotation = rotation;
			Centered = centered;
			Native::Function::Call(Native::Hash::REQUEST_STREAMED_TEXTURE_DICT, _textureDict);
		}
		Sprite::~Sprite()
		{
			Native::Function::Call(Native::Hash::SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, _textureDict);
		}

		void Sprite::Draw()
		{
			Draw(Drawing::SizeF::Empty);
		}
		void Sprite::Draw(Drawing::SizeF offset)
		{
			if (!Enabled || !Native::Function::Call<bool>(Native::Hash::HAS_STREAMED_TEXTURE_DICT_LOADED, _textureDict))
			{
				return;
			}

			const float scaleX = Scale.Width / Screen::WIDTH;
			const float scaleY = Scale.Height / Screen::HEIGHT;
			const float positionX = ((Position.X + offset.Width) / Screen::WIDTH) + ((!Centered) ? scaleX * 0.5f : 0.0f);
			const float positionY = ((Position.Y + offset.Height) / Screen::HEIGHT) + ((!Centered) ? scaleY * 0.5f : 0.0f);

			Native::Function::Call(Native::Hash::DRAW_SPRITE, _textureDict, _textureName, positionX, positionY, scaleX, scaleY, Rotation, Color.R, Color.G, Color.B, Color.A);
		}
		CustomSprite::CustomSprite(System::String ^filename, System::Drawing::SizeF scale, System::Drawing::PointF position, System::Drawing::Color color, float rotation, bool centered)
		{
			if (!IO::File::Exists(filename))
			{
				throw gcnew IO::FileNotFoundException(filename);
			}

			if (_textures->ContainsKey(filename))
			{
				_id = _textures->default[filename];
			}
			else
			{
				_id = createTexture(reinterpret_cast<const char *>(ScriptDomain::CurrentDomain->PinString(filename).ToPointer()));

				_textures->Add(filename, _id);
			}
			Enabled = true;
			Scale = scale;
			Position = position;
			Color = color;
			Rotation = rotation;
			Centered = centered;
		}
		CustomSprite::CustomSprite(String ^filename, Drawing::SizeF scale, Drawing::PointF position, Drawing::Color color, float rotation)
		{
			if (!IO::File::Exists(filename))
			{
				throw gcnew IO::FileNotFoundException(filename);
			}

			if (_textures->ContainsKey(filename))
			{
				_id = _textures->default[filename];
			}
			else
			{
				_id = createTexture(reinterpret_cast<const char *>(ScriptDomain::CurrentDomain->PinString(filename).ToPointer()));

				_textures->Add(filename, _id);
			}
			Enabled = true;
			Scale = scale;
			Position = position;
			Color = color;
			Rotation = rotation;
			Centered = false;
		}
		CustomSprite::CustomSprite(String ^filename, Drawing::SizeF scale, Drawing::PointF position, Drawing::Color color)
		{
			if (!IO::File::Exists(filename))
			{
				throw gcnew IO::FileNotFoundException(filename);
			}

			if (_textures->ContainsKey(filename))
			{
				_id = _textures->default[filename];
			}
			else
			{
				_id = createTexture(reinterpret_cast<const char *>(ScriptDomain::CurrentDomain->PinString(filename).ToPointer()));

				_textures->Add(filename, _id);
			}
			Enabled = true;
			Scale = scale;
			Position = position;
			Color = color;
			Rotation = 0.0f;
			Centered = false;
		}
		CustomSprite::CustomSprite(String ^filename, Drawing::SizeF scale, Drawing::PointF position)
		{
			if (!IO::File::Exists(filename))
			{
				throw gcnew IO::FileNotFoundException(filename);
			}

			if (_textures->ContainsKey(filename))
			{
				_id = _textures->default[filename];
			}
			else
			{
				_id = createTexture(reinterpret_cast<const char *>(ScriptDomain::CurrentDomain->PinString(filename).ToPointer()));

				_textures->Add(filename, _id);
			}
			Enabled = true;
			Scale = scale;
			Position = position;
			Color = Drawing::Color::White;
			Rotation = 0.0f;
			Centered = false;
		}

		void CustomSprite::Draw()
		{
			Draw(Drawing::SizeF::Empty);
		}
		
		void CustomSprite::Draw(Drawing::SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			int FrameCount = Native::Function::Call<int>(Native::Hash::GET_FRAME_COUNT);
			if (_lastDrawFrame != FrameCount)
			{
				//reset index to 0 if on a new frame
				_lastDrawFrame = FrameCount;
				_index = 0;
			}
			if (_globalLastDrawFrame != FrameCount)
			{
				//reset level to 0 for all textures if on a new frame.
				//this means that textures are drawn in the order their draw call is called
				_globalLastDrawFrame = FrameCount;
				_level = 0;
			}
			float aspectRatio = Native::Function::Call<float>(Native::Hash::_GET_SCREEN_ASPECT_RATIO, 0);

			const float scaleX = Scale.Width / Screen::WIDTH;
			const float scaleY = Scale.Height / Screen::HEIGHT;
			const float positionX = ((Position.X + offset.Width) / Screen::WIDTH) + ((!Centered) ? scaleX * 0.5f : 0.0f);
			const float positionY = ((Position.Y + offset.Height) / Screen::HEIGHT) + ((!Centered) ? scaleY * 0.5f : 0.0f);

			drawTexture(_id, _index++, _level++, 100, scaleX, scaleY / aspectRatio, 0.5f, 0.5f, positionX, positionY, Rotation, aspectRatio, Color.R / 255.0f, Color.G / 255.0f, Color.B / 255.0f, Color.A / 255.0f);
		}

	}
}