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
			Centered = false;
			Shadow = false;
			Outline = false;
		}
		Text::Text(String ^caption, Drawing::PointF position, float scale, Drawing::Color color)
		{
			Enabled = true;
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = GTA::UI::Font::ChaletLondon;
			Centered = false;
			Shadow = false;
			Outline = false;
		}
		Text::Text(String ^caption, Drawing::PointF position, float scale, Drawing::Color color, GTA::UI::Font font, bool centered)
		{
			Enabled = true;
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = font;
			Centered = centered;
			Shadow = false;
			Outline = false;
		}
		Text::Text(String ^caption, Drawing::PointF position, float scale, Drawing::Color color, GTA::UI::Font font, bool centered, bool shadow, bool outline)
		{
			Enabled = true;
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = font;
			Centered = centered;
			Shadow = shadow;
			Outline = outline;
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
			Native::Function::Call(Native::Hash::SET_TEXT_CENTRE, Centered ? 1 : 0);
			Native::Function::Call(Native::Hash::_SET_TEXT_ENTRY, "STRING");
			Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, Caption);
			Native::Function::Call(Native::Hash::_DRAW_TEXT, x, y);
		}

		Rectangle::Rectangle()
		{
			Enabled = true;
			Position = Drawing::PointF();
			Size = Drawing::SizeF(Screen::WIDTH, Screen::HEIGHT);
			Color = Drawing::Color::Transparent;
		}
		Rectangle::Rectangle(Drawing::PointF position, Drawing::SizeF size)
		{
			Enabled = true;
			Position = position;
			Size = size;
			Color = Drawing::Color::Transparent;
		}
		Rectangle::Rectangle(Drawing::PointF position, Drawing::SizeF size, Drawing::Color color)
		{
			Enabled = true;
			Position = position;
			Size = size;
			Color = color;
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
			const float x = ((Position.X + offset.Width) / Screen::WIDTH) + w * 0.5f;
			const float y = ((Position.Y + offset.Height) / Screen::HEIGHT) + h * 0.5f;

			Native::Function::Call(Native::Hash::DRAW_RECT, x, y, w, h, Color.R, Color.G, Color.B, Color.A);
		}

		Container::Container() : Rectangle(), _items(gcnew List<Element ^>())
		{
		}
		Container::Container(Drawing::PointF position, Drawing::SizeF size) : Rectangle(position, size), _items(gcnew List<Element ^>())
		{
		}
		Container::Container(Drawing::PointF position, Drawing::SizeF size, Drawing::Color color) : Rectangle(position, size, color), _items(gcnew List<Element ^>())
		{
		}

		List<Element ^> ^Container::Items::get()
		{
			return _items;
		}
		void Container::Items::set(List<Element ^> ^value)
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

			for each (Element ^item in Items)
			{
				item->Draw(Drawing::SizeF(Rectangle::Position + offset));
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
			const float positionX = ((Position.X + offset.Width) / Screen::WIDTH) + scaleX * 0.5f;
			const float positionY = ((Position.Y + offset.Height) / Screen::HEIGHT) + scaleY * 0.5f;

			Native::Function::Call(Native::Hash::DRAW_SPRITE, _textureDict, _textureName, positionX, positionY, scaleX, scaleY, Rotation, Color.R, Color.G, Color.B, Color.A);
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
			const float positionX = ((Position.X + offset.Width) / Screen::WIDTH) + scaleX * 0.5f;
			const float positionY = ((Position.Y + offset.Height) / Screen::HEIGHT) + scaleY * 0.5f;

			drawTexture(_id, _index++, _level++, 100, scaleX, scaleY / aspectRatio, 0.5f, 0.5f, positionX, positionY, Rotation, aspectRatio, Color.R / 255.0f, Color.G / 255.0f, Color.B / 255.0f, Color.A / 255.0f);
		}

	}
}