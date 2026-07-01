using System;
namespace GTA
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MinimumRequiredGameBuildAttribute : Attribute
    {
        public int Build { get; }

        public MinimumRequiredGameBuildAttribute(int build)
        {
            Build = build;
        }
    }
}
