//
// Copyright (C) 2015 crosire
//
// This software is  provided 'as-is', without any express  or implied  warranty. In no event will the
// authors be held liable for any damages arising from the use of this software.
// Permission  is granted  to anyone  to use  this software  for  any  purpose,  including  commercial
// applications, and to alter it and redistribute it freely, subject to the following restrictions:
//
//   1. The origin of this software must not be misrepresented; you must not claim that you  wrote the
//      original  software. If you use this  software  in a product, an  acknowledgment in the product
//      documentation would be appreciated but is not required.
//   2. Altered source versions must  be plainly  marked as such, and  must not be  misrepresented  as
//      being the original software.
//   3. This notice may not be removed or altered from any source distribution.
//

using System.Runtime.InteropServices;

namespace SHVDN
{
	/// <summary>
	/// Internal script task responsible for executing script functions.
	/// </summary>
	public unsafe class NativeFunc : IScriptTask
	{
		[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativeInit@@YAX_K@Z")]
		static extern void NativeInit(ulong hash);
		[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativePush64@@YAX_K@Z")]
		static extern void NativePush64(ulong val);
		[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativeCall@@YAPEA_KXZ")]
		static unsafe extern ulong* NativeCall();

		ulong Hash;
		ulong[] Arguments;
		public ulong* Result;

		public NativeFunc(ulong hash, params ulong[] args)
		{
			Hash = hash;
			Arguments = args;
		}

		public void Run()
		{
			Result = Invoke(Hash, Arguments);
		}

		internal static ulong* Invoke(ulong hash, params ulong[] args)
		{
			NativeInit(hash);
			foreach (var arg in args)
				NativePush64(arg);
			return NativeCall();
		}
	}
}
