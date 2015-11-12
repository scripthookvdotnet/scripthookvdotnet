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
//#include "stdafx.h"

//#include "Checkpoint.hpp"

#include "Script.hpp"
#include "World.hpp"
#pragma once
#pragma managed

namespace GTA {

	using namespace System;

	ref class EventArgs;

	/*CLASS_ATTRIBUTES*/
	public ref class Checkpoint sealed {

	private:
		System::Drawing::Color pColor;
		bool bVisible;
		float pDiameter;
		GTA::Math::Vector3 pPosition;

		void PerFrameDrawing(Object^ sender, EventArgs^ e);

	internal:


	public:
		property bool Visible {
			bool get() {
				return bVisible;
			}
			void set(bool value);
		}

		Checkpoint(GTA::Math::Vector3 Position, System::Drawing::Color Color, float Diameter) {
			bVisible = false;
			pColor = Color;
			pDiameter = Diameter;
			pPosition = Position;
			Visible = true;
		}
		/*Checkpoint() {
			bVisible = false;
			pColor = System::Drawing::Color::White;
			pDiameter = 3.0F;
			pPosition = GTA::Math::Vector3();
		}*/

		void Disable() {
			Visible = false;
		}

		property System::Drawing::Color Color {
			System::Drawing::Color get() {
				return pColor;
			}
			void set(System::Drawing::Color value) {
				pColor = value;
			}
		};

		property GTA::Math::Vector3 Position {
			GTA::Math::Vector3 get() {
				return pPosition;
			}
			void set(GTA::Math::Vector3 value) {
				pPosition = value;
			}
		}

		property float Diameter {
			float get() {
				return pDiameter;
			}
			void set(float value) {
				pDiameter = value;
			}
		}

	};

}