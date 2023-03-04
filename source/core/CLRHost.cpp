#include "pch.h"
#include <stdio.h>
#include <windows.h>
#include <mscoree.h>
#include <metahost.h>
#include <string>
#include "hosting.h"
#include "exports.h"
#pragma comment(lib, "MSCorEE.lib")
#pragma warning( disable:4996 )
#define HRCHECK(expr) assert(SUCCEEDED(expr))
#import "mscorlib.tlb" auto_rename
using namespace mscorlib;

const std::wstring EntryAssembly = L"ScriptHookVDotNet.dll";
const std::wstring RuntimeVersion = L"v4.0.30319";
ICorRuntimeHost* RuntimeHost;

ICorRuntimeHost* GetCorRuntimeHost() {
	ICLRRuntimeInfo* pRuntimeInfo = NULL;
	ICorRuntimeHost* pRuntimeHost = NULL;
	ICLRMetaHost* pMetaHost = NULL;
	BOOL bLoadable;
	
	/* Get ICLRMetaHost instance */
	HRCHECK(CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (VOID**)&pMetaHost));

	/* Get ICLRRuntimeInfo instance */
	HRCHECK(pMetaHost->GetRuntime(RuntimeVersion.c_str(), IID_ICLRRuntimeInfo, (VOID**)&pRuntimeInfo));

	/* Check if the specified runtime can be loaded */
	assert(SUCCEEDED(pRuntimeInfo->IsLoadable(&bLoadable)) && bLoadable);

	/* Get ICorRuntimeHost instance */
	HRCHECK(pRuntimeInfo->GetInterface(CLSID_CorRuntimeHost, IID_ICorRuntimeHost, (VOID**)&pRuntimeHost));
	
	return pRuntimeHost;
}

_AppDomainPtr GetDefaultAppDomain(ICorRuntimeHost* pRuntimeHost) {
	IUnknownPtr pAppDomainThunk = NULL;
	HRCHECK(pRuntimeHost->GetDefaultDomain(&pAppDomainThunk));

	/* Equivalent of System.AppDomain.CurrentDomain in C# */
	_AppDomainPtr pDefaultAppDomain = NULL;
	HRCHECK(pAppDomainThunk->QueryInterface(__uuidof(_AppDomain), (LPVOID*)&pDefaultAppDomain));
	return pDefaultAppDomain;
}

void CLR_Startup() {
	auto assemblyPath = GetExeDir() + L"\\" + EntryAssembly;

	// Get runtime host
	RuntimeHost = GetCorRuntimeHost();

	// Start CLR
	HRCHECK(RuntimeHost->Start());

	// Load assembly and execute entrypoint function
	
	auto pDefaultAppDomain = GetDefaultAppDomain(RuntimeHost);
	pDefaultAppDomain->ExecuteAssembly_2(assemblyPath.c_str());

	// Get the function pointer for dispatching events
	assert(CLR_DoInit = (VoidFunc)GetPtr(KEY_CLR_INITFUNC));
	assert(CLR_DoTick = (VoidFunc)GetPtr(KEY_CLR_TICKFUNC));
	assert(CLR_DoKeyboard = (KeyboardFunc)GetPtr(KEY_CLR_KBHFUNC));
}

void CLR_Shutdown() {
	assert(RuntimeHost);
	RuntimeHost->Stop();
}
