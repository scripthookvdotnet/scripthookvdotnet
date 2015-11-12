///*
//* Copyright (c) 2009-2011 Hazard (hazard_x@gmx.net / twitter.com/HazardX)
//* 
//* Permission is hereby granted, free of charge, to any person obtaining a copy
//* of this software and associated documentation files (the "Software"), to deal
//* in the Software without restriction, including without limitation the rights
//* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//* copies of the Software, and to permit persons to whom the Software is
//* furnished to do so, subject to the following conditions:
//* 
//* The above copyright notice and this permission notice shall be included in
//* all copies or substantial portions of the Software.
//* 
//* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//* THE SOFTWARE.
//*/
//
#include "AnimationSet.h"

#include "Game.hpp"
#include "Ped.hpp"
#include <windows.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>
//#include <d3d9.h>
//#include <d3dx9.h>
#include <vcclr.h>
//
#pragma managed

namespace GTA {
	using System::String;
	using GTA::Game;
	AnimationSet::AnimationSet(System::String^ ModelName){
		pName = ModelName;
	}

	String^ AnimationSet::Name::get() {
		return pName;
	}

	//bool AnimationSet::isInMemory::get() {
	//	char* ptr = PinStringA(pName);
	//	try {
	//		return Script::HaveAnimsLoaded(ptr);
	//	} finally {
	//		FreeString(ptr);
	//	}
	//}

	//void AnimationSet::LoadToMemory() {
	//	char* ptr = PinStringA(pName);
	//	try {
	//	   Scripting::RequestAnims(ptr);
	//	} catch (...) {
	//	} finally {
	//		FreeString(ptr);
	//	}
	//}
	//bool AnimationSet::LoadToMemoryNow() {
	//	char* ptr = PinStringA(pName);
	//	int tries = 0;
	//	try {
	//		Scripting::RequestAnims(ptr);
	//		while (!Scripting::HaveAnimsLoaded(ptr)) {
	//			/*Game::WaitInCurrentScript(0);*/
	//			Scripting::RequestAnims(ptr);
	//			tries++;
	//			if (tries>100) return false;
	//		}
	//	} catch (...) { 
	//		return false; 
	//	} finally {
	//		FreeString(ptr);
	//	}
	//	return true;
	//}

	//void AnimationSet::DisposeFromMemoryNow() {
	//	char* ptr = PinStringA(pName);
	//	try {
	//	   Scripting::RemoveAnims(ptr);
	//	} catch (...) {
	//	} finally {
	//		FreeString(ptr);
	//	}
	//}
	//bool AnimationSet::isPedPlayingAnimation(Ped^ ped, String^ AnimationName) {
	//	OBJECT_NON_EXISTING_CHECK(ped,false);
	//	bool res = false;
	//	char* pAnimSet = PinStringA(pName);
	//	char* pAnimName = PinStringA(AnimationName);
	//	try {
	//		res = Scripting::IsCharPlayingAnim(ped->Handle, pAnimSet, pAnimName);
	//	} finally {
	//		FreeString(pAnimSet);
	//		FreeString(pAnimName);
	//	}
	//	return res;
	//}
	//float AnimationSet::GetPedsCurrentAnimationTime(Ped^ ped, String^ AnimationName) {
	//	OBJECT_NON_EXISTING_CHECK(ped,0.0f);
	//	float res = 0.0f;
	//	char* pAnimSet = PinStringA(pName);
	//	char* pAnimName = PinStringA(AnimationName);
	//	try {
	//		Script::GetCharAnimCurrentTime(ped->Handle, pAnimSet, pAnimName, &res);
	//	} finally {
	//		FreeString(pAnimSet);
	//		FreeString(pAnimName);
	//	}
	//	return res;
	//}

	//bool AnimationSet::operator == (AnimationSet^ left, AnimationSet^ right) {
	//	if isNULL(left) return isNULL(right);
	//	if isNULL(right) return false;
	//	return (left->Name->Equals(right->Name));
	//}
	bool AnimationSet::operator != (AnimationSet^ left, AnimationSet^ right) {
		return !(left == right);
	}
	/*AnimationSet AnimationSet^ (String^ source) {
		return gcnew AnimationSet(source);
	}*/

	String^ AnimationSet::ToString() {
		return Name;
	}

}

//namespace GTA
//{
//	public struct AnimationSet : INamedAsset
//	{
//		/// <inheritdoc />
//		public bool::IsLoaded
//		{
//		}
//
//		/// <inheritdoc />
//			public string Name
//		{
//			get
//		{
//		}
//		}
//
//			public AnimationSet(string name)
//		{
//		}
//
//		/// <inheritdoc />
//		public void Dismiss()
//		{
//		}
//
//		/// <inheritdoc />
//		public void Load()
//		{
//		}
//
//		/// <inheritdoc />
//		public void LoadAndWait()
//		{
//		}
//
//		public static implicit operator AnimationSet(string animationSetName)
//		{
//		}
//
//		public static implicit operator String(AnimationSet animationSet)
//		{
//		}
//
//		public static implicit operator Object(AnimationSet animationSet)
//		{
//		}
//	}
//	///             <summary>
//	/// Represents a game asset. Game assets are blueprints that describe an object. For example, a weapon asset specifies what model a weapon will use, how fast it shoots, how many bullets it has, etc.
//	public interface INamedAsset : IAsset
//	{
//		///             <summary>
//		/// Gets the name of this asset.
//		/// </summary>
//		///             <value>The name of this asset.</value>
//		///             <summary>
//		/// Represents a game asset. Game assets are blueprints that describe an object. For example, a weapon asset specifies what model a weapon will use, how fast it shoots, how many bullets it has, etc.
//		interface IAsset
//		{
//			///             <summary>
//			/// Gets or sets a value indicating whether this asset has been loaded.
//			/// </summary>
//			bool IsLoaded
//			{
//
//			};
//
//			///             <summary>
//			/// Unloads this asset from memory.
//			/// </summary>
//			void Dismiss();
//
//			///             <summary>
//			/// Starts loading this asset into memory.
//			/// </summary>
//			void Load();
//
//			///             <summary>
//			/// Starts loading this asset into memory and waits for it to finish loading.
//			/// </summary>
//			void LoadAndWait();
//		}
//	};
//}