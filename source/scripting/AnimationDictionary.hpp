#pragma once

#include "Game.hpp"

namespace GTA
{
	public ref class AnimationDictionary sealed : System::IEquatable<AnimationDictionary ^>
	{
	public:
		AnimationDictionary(System::String ^name);

		property System::String ^Name
		{
			System::String ^get();
		}
		property bool IsLoaded
		{
			bool get();
		}
		property bool IsValid
		{
			bool get();
		}

		void Request();
		bool Request(int timeout);
		void Dismiss();
		bool Equals(System::Object ^obj) override;
		virtual bool Equals(AnimationDictionary ^animDict);

		inline int GetHashCode() override
		{
			return Game::GenerateHash(Name);
		}
		virtual inline System::String ^ToString() override
		{
			return Name;
		}
		static inline operator AnimationDictionary ^(System::String ^source)
		{
			return gcnew AnimationDictionary(source);
		}
		static inline bool operator==(AnimationDictionary ^left, AnimationDictionary ^right)
		{
			return left->Equals(right);
		}
		static inline bool operator!=(AnimationDictionary ^left, AnimationDictionary ^right)
		{
			return !operator==(left, right);
		}

	private:
		System::String ^_name;
	};
}