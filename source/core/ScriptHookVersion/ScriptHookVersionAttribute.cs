using System;

namespace GTA
{
    public class ScriptHookVersionAttribute : Attribute
    {
        private string _minRequiredVersion;

        public string MinRequiredVersion
        {
            get => _minRequiredVersion;
            set
            {
                _minRequiredVersion = value;
            }
        }
    }
}
