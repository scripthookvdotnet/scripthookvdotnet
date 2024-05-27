//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    public enum ForceAnimAIUpdateState : uint
    {
        Default,
        /// <summary>
        /// Sets additional reset flags to do CameraAIUpdate and PostCameraAnimUpdate on the <see cref="Ped"/>.
        /// </summary>
        CutsceneExit
    }
}
