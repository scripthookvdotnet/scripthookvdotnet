//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace SHVDN
{
    public static class SharedVariables
    {
        public static bool DidRequestReloadResetFlag
        {
            get => ScriptHookVDotNet.Domain?.DidRequestReloadResetFlag ?? false;
            set
            {
                ScriptDomain domain = ScriptHookVDotNet.Domain;
                if (domain != null)
                {
                    domain.DidRequestReloadResetFlag = value;
                }
            }
        }
    }
}
