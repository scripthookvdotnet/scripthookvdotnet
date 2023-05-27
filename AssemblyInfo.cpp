using namespace System;
using namespace System::Reflection;

// The file encoding has to be UTF-8 with BOM and literal strings have to be wstring so the ANSI encoding won't have influence on the literal strings
[assembly:AssemblyTitle(L"Community Script Hook V .NET")];
[assembly:AssemblyDescription(L"An ASI plugin for Grand Theft Auto V, which allows running scripts written in any .NET language in-game.")];
[assembly:AssemblyCompany(L"crosire & kagikn & contributors")];
[assembly:AssemblyProduct(L"ScriptHookVDotNet")];
[assembly:AssemblyCopyright(L"Copyright © 2015 crosire & kagikn")];
[assembly:AssemblyVersion(SHVDN_VERSION)];
[assembly:AssemblyFileVersion(SHVDN_VERSION)];
// Sign with a strong name to distinguish from older versions and cause .NET framework runtime to bind the correct assemblies
// There is no version check performed for assemblies without strong names (https://docs.microsoft.com/en-us/dotnet/framework/deployment/how-the-runtime-locates-assemblies)
[assembly:AssemblyKeyFileAttribute(L"PublicKeyToken.snk")];
