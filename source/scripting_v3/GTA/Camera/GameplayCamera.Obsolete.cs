using System;
using System.ComponentModel;

namespace GTA
{
    public static partial class GameplayCamera
    {
        /// <summary>
        /// Gets the first-person ped aim zoom factor associated with equipped sniper scoped weapon,
        /// or the mobile phone camera, if active.
        /// </summary>
        [Obsolete("GameplayCamera.Zoom is obsolete since it does not suggest the value is relevant only when a first" +
            "person aim camera is used. Use GameplayCamera.FirstPersonAimCamZoomFactor instead."),
            EditorBrowsable(EditorBrowsableState.Never)]
        public static float Zoom => FirstPersonAimCamZoomFactor;
    }
}
