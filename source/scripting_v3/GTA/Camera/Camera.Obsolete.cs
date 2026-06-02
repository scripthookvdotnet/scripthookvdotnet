using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA.Native;

namespace GTA
{
    public sealed partial class Camera : PoolObject, ISpatial
    {
        /// <summary>
        /// Sets a cam active which will be interpolated too from this <see cref="Camera"/>.
        /// </summary>
        [Obsolete("Use Camera.InterpTo(Camera, int, CamFrameInterpolatorCurveType, CamFrameInterpolatorCurveType) instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public void InterpTo(Camera to, int duration, int easePosition, int easeRotation)
        {
            Function.Call(Hash.SET_CAM_ACTIVE_WITH_INTERP, to.Handle, Handle, duration, easePosition, easeRotation);
        }
    }
}
