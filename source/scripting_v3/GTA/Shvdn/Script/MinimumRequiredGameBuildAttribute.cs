using System;
namespace GTA
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MinimumRequiredGameBuildAttribute : Attribute
    {
        public MinimumRequiredGameBuildAttribute(int build)
        {
        }
    }
}
