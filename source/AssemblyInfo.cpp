using namespace System::Reflection;

[assembly:AssemblyTitleAttribute("Community Script Hook V .NET")];
[assembly:AssemblyDescriptionAttribute("An ASI plugin for Grand Theft Auto V, which allows running scripts written in any .NET language in-game.")];
[assembly:AssemblyCopyrightAttribute("Copyright © 2015 crosire")];

#ifdef SHVDN_VERSION
[assembly:AssemblyVersionAttribute(SHVDN_VERSION)];
[assembly:AssemblyFileVersionAttribute(SHVDN_VERSION)];
#endif
