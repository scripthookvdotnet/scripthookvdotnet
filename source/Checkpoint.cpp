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
#include "stdafx.h"

#include "Checkpoint.hpp"

#include "Script.hpp"
#include "Native.hpp"
//#include "World.cpp"

#pragma managed

namespace GTA {

	void Checkpoint::DrawCheckpoint(Checkpoint::CheckType check, GTA::Math::Vector3 Position, float radius, int Red, int green, int blue, int alpha, bool AlignWithGround)
	{
		if (Checkpoint::Handle != 0)
		{
			Checkpoint::DeleteCheckpoint(Handle);
		}
		if (AlignWithGround == true)
		{
			Position = GTA::Math::Vector3(Position.X, Position.Y, Position.Z - 1);
		}
		Handle = GTA::Native::Function::Call<int>(Native::Hash::CREATE_CHECKPOINT, 0, Position.X, Position.Y, Position.Z, radius, Red, green, blue, alpha, 0);
		check = check;
		GTA::Checkpoint::Position = Position;
		Radius = radius;
		red1 = Red;
		green1 = green;
		blue1 = blue;
		alpha1 = alpha;
		AlignWithGround1 = AlignWithGround;
	}

	void Checkpoint::DeleteCheckpoint(int Handle)
	{
		GTA::Native::Function::Call(Native::Hash::DELETE_CHECKPOINT, Checkpoint::Handle);
	}
	bool Checkpoint::DoesCheckpointExist(int handle)
	{
		return Checkpoint::exists;
	}
	/*void Checkpoint::setPos(GTA::Math::Vector3 newposition)
	{
		throw gcnew System::NotImplementedException();
	}*/
	/*void Checkpoint::setPos(GTA::Math::Vector3 newposition)
	{
		if (Checkpoint::exists)
		{
			Checkpoint::Handle = Checkpoint::DrawCheckpoint(Checkpoint::check, Checkpoint::chkpointpos, Checkpoint::Radius, Checkpoint::red1, Checkpoint::green1, Checkpoint::blue1, Checkpoint::alpha1, Checkpoint::AlignWithGround1);
		}
	}*/


	//void Checkpoint::Visible::set(bool value) {
	//	if (bVisible == value) return;
	//	if (value) 
	//		/*this->PerFrameDrawing += gcnew EventHandler(this, &Checkpoint::PerFrameDrawing);*/
	//	else
	//		/*this->PerFrameDrawing -= gcnew EventHandler(this, &Checkpoint::PerFrameDrawing);*/
	//	bVisible = value;
	//}

	/*void Checkpoint::PerFrameDrawing(Object^ sender, EventArgs^ e) {
		World::DrawCheckpoint(0, pPosition, 50, pColor);
	}*/

}