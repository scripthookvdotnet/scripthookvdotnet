using System;
namespace GTA
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RequireGameBuild : Attribute
    {
        public RequireGameBuild(int build)
        {
        }
    }
}
