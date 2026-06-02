using System;
using GTA.Native;

namespace GTA
{
    public sealed partial class Scaleform : IDisposable, INativeValue
    {
        [Obsolete("The Scaleform constructor with a string parameter is obsolete. Use Scaleform.RequestMovie instead.")]
        public Scaleform(string scaleformID)
        {
            Handle = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, scaleformID);
        }
    }
}
