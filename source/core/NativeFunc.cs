//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
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
