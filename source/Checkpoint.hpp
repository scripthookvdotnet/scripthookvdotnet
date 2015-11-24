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
#include "stdafx.h"

//#include "Checkpoint.cpp"

#include "Script.hpp"
//#include "World.hpp"
#include "Native.hpp"
#pragma once
#pragma managed

namespace GTA {

	using namespace System;

	ref class EventArgs;

	/*CLASS_ATTRIBUTES*/
	public ref class Checkpoint sealed {

	//internal:
	//
		/*void PerFrameDrawing(Object^ sender, EventArgs^ e);*/

	internal:
		/*	property bool exists
			{
				bool get() {
					return Handle12 < > 0;
				}
			}*/
		static Single Radius = 0;
		static int red1 = 0;
		static int green1 = 0;
		static int blue1 = 0;
		static int alpha1 = 0;
		static bool AlignWithGround1 = false;
		static System::Drawing::Color pColor;
		static bool bVisible;
		static float pDiameter;
		static GTA::Math::Vector3 pPosition;
		static GTA::Math::Vector3 chkpointpos;
		/*pPosition = GTA::Math::Vector3::Zero;*/
		static int Handle12 = 0;
		static bool exists;
		/*static GTA::Math::Vector3 chkpointpos;*/

	public:
		property bool Visible {
			bool get() {
				return bVisible;
			}
			void set(bool value) {
				bVisible = value;
			}
		}
	enum class CheckType
		{
			CylinderSingleArrow = 0,
			CylinderDoubleArrow = 1,
			CylinderTripleArrow = 2,
			CylinderCycleArrow = 3,
			CylinderChecker = 4,
			CyclinderFadeSingleArrow = 5,
			CyclinderFadeDoubleArrow = 6,
			CylinderFadeTripleArrow = 7,
			CyclinderFadeCycleArrow = 8,
			CylinderSmallLowChecker = 9,
			ArrowInCircle = 10,
			DoubleArrowInCircle = 11,
			TripleArrowInCircle = 12,
			CycleArrowInCircle = 13,
			CheckerInCircle = 14,
			Arrow = 15,
			DoubleArrow2 = 16,
			DoubleArrow3 = 17,
			CycleArrow2 = 18,
			CheckerFinish = 19,
			CylinderArrowLeft = 20,
			CylinderDoubleLeftArrow = 21,
			CylinderTripleLeftArrow = 22,
			CylinderHightChecker = 24,
			CylinderHighCycleArrow = 28,
			PlaneLeftRollInCircle = 36,
			PlaneRightRollInCircle = 36,
			PlaneForwardInCircle = 36,
			CircleBankLeft = 40,
			CylinderZero = 42,
			Cylinder = 45
		};
		/*Checkpoint(GTA::Math::Vector3 Position, System::Drawing::Color Color, float Diameter) {
			bVisible = false;
			pColor = Color;
			pDiameter = Diameter;
			pPosition = Position;
			Visible = true;

		}*/
		/*Checkpoint() {
			bVisible = false;
			pColor = System::Drawing::Color::White;
			pDiameter = 3.0F;
			pPosition = GTA::Math::Vector3();
		}*/

		/*void Disable() {
			Visible = false;
		}*/
		/*void Disable() {

		}*/
		static void Checkpoint::DrawCheckpoint(Checkpoint::CheckType check, GTA::Math::Vector3 Position, float radius, int Red, int green, int blue, int alpha, bool AlignWithGround);
		static void Checkpoint::DeleteCheckpoint(int Handle);
		property System::Drawing::Color Color {
			System::Drawing::Color get() {
				return pColor;
			}
			void set(System::Drawing::Color value) {
				pColor = value;
			}
		};

		static property GTA::Math::Vector3 Position {
			GTA::Math::Vector3 get() {
				return pPosition;
			}
			void set(GTA::Math::Vector3 value) {
				pPosition = value;

			}
		}

			static property float Diameter {
				float get() {
					return pDiameter;
				}
				void set(float value) {
					pDiameter = value;
				}
			}
			static property int Handle {
				int get() {
					return Handle12;
				}
				void set(int value) {
					Handle12 = value;
				}
			}
			static bool DoesCheckpointExist(int Handle);
		/*	static void setPos(GTA::Math::Vector3 newposition);*/

			property GTA::Math::Vector3 positionofcheckpoint {
				GTA::Math::Vector3 get() {
					return Checkpoint::chkpointpos;
				}
			}
		internal:
			static Checkpoint::CheckType check;
	};
}