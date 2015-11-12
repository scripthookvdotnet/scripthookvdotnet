/*
* Copyright (c) 2009-2011 Hazard (hazard_x@gmx.net / twitter.com/HazardX)
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
#include "Game.hpp"
#include "Ped.hpp"
#include "stdafx.h"
#pragma once
#pragma managed

namespace GTA {

	public ref class AnimationSet sealed {

	private:
		System::String^ pName;

	/*internal:
		void LoadToMemory();
		bool LoadToMemoryNow();*/
		
	public:
		AnimationSet(System::String^ Name);

		property System::String^ Name { 
			System::String^ get();
		}

	/*	property bool isInMemory { 
			bool get();
		}*/

		/*void DisposeFromMemoryNow();
		bool isPedPlayingAnimation(Ped^ ped, System::String^ AnimationName);
		float GetPedsCurrentAnimationTime(Ped^ ped, System::String^ AnimationName);*/

		/*static bool operator == ( AnimationSet^ left, AnimationSet^ right );*/
		static bool operator != ( AnimationSet^ left, AnimationSet^ right );
		//static operator AnimationSet^ (String^ source);

		virtual System::String^ ToString() override;

	};

}