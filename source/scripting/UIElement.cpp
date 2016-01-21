#include "UIElement.hpp"
#include "Native.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	UIText::UIText(String ^caption, Drawing::Point position, float scale)
	{
		Enabled = true;
		Caption = caption;
		Position = position;
		Scale = scale;
		Color = Drawing::Color::WhiteSmoke;
		Font = GTA::Font::ChaletLondon;
		Centered = false;
		Shadow = false;
		Outline = false;
	}
	UIText::UIText(String ^caption, Drawing::Point position, float scale, Drawing::Color color)
	{
		Enabled = true;
		Caption = caption;
		Position = position;
		Scale = scale;
		Color = color;
		Font = GTA::Font::ChaletLondon;
		Centered = false;
		Shadow = false;
		Outline = false;
	}
	UIText::UIText(String ^caption, Drawing::Point position, float scale, Drawing::Color color, GTA::Font font, bool centered)
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
	UIText::UIText(String ^caption, Drawing::Point position, float scale, Drawing::Color color, GTA::Font font, bool centered, bool shadow, bool outline)
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

	void UIText::Draw()
	{
		Draw(Drawing::Size());
	}
	void UIText::Draw(Drawing::Size offset)
	{
		if (!Enabled)
		{
			return;
		}

		const float x = (static_cast<float>(Position.X) + offset.Width) / UI::WIDTH;
		const float y = (static_cast<float>(Position.Y) + offset.Height) / UI::HEIGHT;

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

	UIRectangle::UIRectangle()
	{
		Enabled = true;
		Position = Drawing::Point();
		Size = Drawing::Size(UI::WIDTH, UI::HEIGHT);
		Color = Drawing::Color::Transparent;
	}
	UIRectangle::UIRectangle(Drawing::Point position, Drawing::Size size)
	{
		Enabled = true;
		Position = position;
		Size = size;
		Color = Drawing::Color::Transparent;
	}
	UIRectangle::UIRectangle(Drawing::Point position, Drawing::Size size, Drawing::Color color)
	{
		Enabled = true;
		Position = position;
		Size = size;
		Color = color;
	}

	void UIRectangle::Draw()
	{
		Draw(Drawing::Size());
	}
	void UIRectangle::Draw(Drawing::Size offset)
	{
		if (!Enabled)
		{
			return;
		}

		const float w = static_cast<float>(Size.Width) / UI::WIDTH;
		const float h = static_cast<float>(Size.Height) / UI::HEIGHT;
		const float x = ((static_cast<float>(Position.X) + offset.Width) / UI::WIDTH) + w * 0.5f;
		const float y = ((static_cast<float>(Position.Y) + offset.Height) / UI::HEIGHT) + h * 0.5f;

		Native::Function::Call(Native::Hash::DRAW_RECT, x, y, w, h, Color.R, Color.G, Color.B, Color.A);
	}

	UIContainer::UIContainer() : UIRectangle(), _items(gcnew List<UIElement ^>())
	{
	}
	UIContainer::UIContainer(Drawing::Point position, Drawing::Size size) : UIRectangle(position, size), _items(gcnew List<UIElement ^>())
	{
	}
	UIContainer::UIContainer(Drawing::Point position, Drawing::Size size, Drawing::Color color) : UIRectangle(position, size, color), _items(gcnew List<UIElement ^>())
	{
	}

	List<UIElement ^> ^UIContainer::Items::get()
	{
		return _items;
	}
	void UIContainer::Items::set(List<UIElement ^> ^value)
	{
		_items = value;
	}

	void UIContainer::Draw()
	{
		Draw(Drawing::Size());
	}
	void UIContainer::Draw(Drawing::Size offset)
	{
		if (!Enabled)
		{
			return;
		}

		UIRectangle::Draw(offset);

		for each (UIElement ^item in Items)
		{
			item->Draw(static_cast<Drawing::Size>(UIRectangle::Position + offset));
		}
	}

	UISprite::UISprite(String ^textureDict, String ^textureName, Drawing::Size scale, Drawing::Point position)
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
	UISprite::UISprite(String ^textureDict, String ^textureName, Drawing::Size scale, Drawing::Point position, Drawing::Color color)
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
	UISprite::UISprite(String ^textureDict, String ^textureName, Drawing::Size scale, Drawing::Point position, Drawing::Color color, float rotation)
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
	UISprite::~UISprite()
	{
		Native::Function::Call(Native::Hash::SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, _textureDict);
	}

	void UISprite::Draw()
	{
		Draw(Drawing::Size());
	}
	void UISprite::Draw(Drawing::Size offset)
	{
		if (!Enabled || !Native::Function::Call<bool>(Native::Hash::HAS_STREAMED_TEXTURE_DICT_LOADED, _textureDict))
		{
			return;
		}

		const float scaleX = static_cast<float>(Scale.Width) / UI::WIDTH;
		const float scaleY = static_cast<float>(Scale.Height) / UI::HEIGHT;
		const float positionX = ((static_cast<float>(Position.X) + offset.Width) / UI::WIDTH) + scaleX * 0.5f;
		const float positionY = ((static_cast<float>(Position.Y) + offset.Height) / UI::HEIGHT) + scaleY * 0.5f;

		Native::Function::Call(Native::Hash::DRAW_SPRITE, _textureDict, _textureName, positionX, positionY, scaleX, scaleY, Rotation, Color.R, Color.G, Color.B, Color.A);
	}
}