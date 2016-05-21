#pragma once
#include "Game.hpp"

namespace GTA
{
	public ref class AnimationSet sealed : System::IEquatable<AnimationSet ^>
	{
	public:
		AnimationSet(System::String ^name);

		property System::String ^Name
		{
			System::String ^get();
		}
		property bool IsLoaded
		{
			bool get();
		}

		void Request();
		bool Request(int timeout);
		void Dismiss();
		bool Equals(System::Object ^obj) override;
		virtual bool Equals(AnimationSet ^animDict);

		inline int GetHashCode() override
		{
			return Game::GenerateHash(Name);
		}
		virtual inline System::String ^ToString() override
		{
			return Name;
		}
		static inline operator AnimationSet ^(System::String ^source)
		{
			return gcnew AnimationSet(source);
		}
		static inline bool operator==(AnimationSet ^left, AnimationSet ^right)
		{
			return left->Equals(right);
		}
		static inline bool operator!=(AnimationSet ^left, AnimationSet ^right)
		{
			return !operator==(left, right);
		}

	private:
		System::String ^_name;
	};
}