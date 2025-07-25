namespace GTA
{
    public enum ProfileSettings
    {
        /// <summary>
        /// Represents an invalid or uninitialized profile setting.
        /// Used to indicate that a profile setting value is missing or not applicable.
        /// </summary>
        InvalidProfileSetting = -1,
        /// <summary>
        /// Represents the Controller Targeting Mode. Ranges from 0-3 (0 = Assisted Aim - Full, 1 = Assisted Aim - Partial, 2 = Free Aim - Assisted, 3 = Free Aim).
        /// </summary>
        TargetingMode = 0,
        /// <summary>
        /// Indicates whether the controller axis is inverted (1 = enabled, 0 = disabled).
        /// </summary>
        AxisInversion = 1,
        /// <summary>
        /// Indicates whether controller vibration is enabled (1 = enabled, 0 = disabled).
        /// </summary>
        ControllerVibration = 2,

        /// <summary>
        /// Represents the 3rd person controller control type, see <see cref="GTA.ControllerControlType"/>.
        /// </summary>
        ControllerControlConfig = 12,
        /// <summary>
        /// Represents the 3rd person controller aim sensitivity, range 0 - 14.
        /// </summary>
        ControllerAimSensitivity = 13,
        /// <summary>
        /// Represents the 3rd person controller look-around sensitivity, range 0 - 14.
        /// </summary>
        LookAroundSensitivity = 14,

        /// <summary>
        /// Indicates whether mouse controls are inverted (1 = enabled, 0 = disabled)
        /// </summary>
		MouseInversion = 15,

        /// <summary>
        /// Indicated whether you can move around with your controller while zoomed in (1 = enabled, 0 = disabled).
        /// </summary>
        ControllerAllowMovementWhenZoomed = 16,

        /// <summary>
        /// Represents the 1st person controller control type, see <see cref="GTA.ControllerControlType"/>.
        /// </summary>
        ControllerControlConfigFps = 20,

        /// <summary>
        /// Indicates whether the mouse axis is inverted while flying (1 = enabled, 0 = disabled).
        /// </summary>
		MouseInversionFlying = 21,
        /// <summary>
        /// Indicates whether the mouse axis is inverted while in a submarine (1 = enabled, 0 = disabled).
        /// </summary>
        MouseInversionSub = 22,

        /// <summary>
        /// Indicates how fast the camera centers itself to the car while driving, range 0 - 10. (0 = slowest, 10 = fastest).
        /// </summary>
		MouseAutocenterCar = 23,
        /// <summary>
        /// Indicates how fast the camera centers itself to the bike while driving, range 0 - 10. (0 = slowest, 10 = fastest).
        /// </summary>
        MouseAutocenterBike = 24,
        /// <summary>
        /// Indicates how fast the camera centers itself to the aircraft while flying, range 0 - 10. (0 = slowest, 10 = fastest).
        /// </summary>
        MouseAutocenterPlane = 25,

        /// <summary>
        /// Represents the mouse flying control type (0 = Roll/Pitch, 1 = Yaw/Pitch).
        /// </summary>
		MouseSwapRollYawFlying = 26,

        /// <summary>
        /// Indicates whether subtitles are enabled (1 = enabled, 0 = disabled).
        /// </summary>
        DisplaySubtitles = 203,

        /// <summary>
        /// Represents the radar mode, range 0 - 2, (0 = Off, 1 = On, 2 = Blip)
        /// </summary>
        DisplayRadarMode = 204,

        /// <summary>
        /// Indicates whether the HUD is enabled (1 = enabled, 0 = disabled).
        /// </summary>
        DisplayHudMode = 205,
        DisplayLanguage = 206,

        /// <summary>
        /// Indicates whether the GPS is enabled (1 = enabled, 0 = disabled).
        /// </summary>
        DisplayGps = 207,
        /// <summary>
        /// Indicates whether the Autosaving feature is enabled (1 = enabled, 0 = disabled).
        /// </summary>
        DisplayAutosaveMode = 208,
        DisplayHandbrakeCam = 209,

        /// <summary>
        /// Legacy version of <see cref="DisplayGamma"/>, exact game version is unknown.
        /// </summary>
        LegacyDoNotUseDisplayGamma = 210,

        ControllerCinematicShooting = 211,

        /// <summary>
        /// Represents the display safezone size, range 0 - 10.
        /// </summary>
        DisplaySafezoneSize = 212,

        /// <summary>
        /// Represents the display gama / brightness, 0 - 30.
        /// </summary>
        DisplayGamma = 213,

        DisplayLegacyUnused1 = 214,
        DisplayLegacyUnused2 = 215,
        DisplayLegacyUnused3 = 216,
        DisplayLegacyUnused4 = 217,
        DisplayLegacyUnused5 = 218,
        DisplayLegacyUnused6 = 219,

        /// <summary>
        /// Represents the camera height while driving (0 = low, 1 = high)
        /// </summary>
        DisplayCameraHeight = 220,
        /// <summary>
        /// Indicates whether the big radar is enabled (1 = enabled, 0 = disabled).
        /// </summary>
        DisplayBigRadar = 221,
        DisplayBigRadarNames = 222,
        ControllerDuckHandbrake = 223,
        DisplayDof = 224,
        ControllerDriveby = 225,
        /// <summary>
        /// Internal name: DisplaySkfx
        /// </summary>
        DisplayScreenKillFx = 226,
        MeasurementSystem = 227,
        NewDisplayLanguage = 228,

        FpsDefaultAimType = 229,
        FpsPersistantView = 230,
        FpsFieldOfView = 231,
        FpsLookSensitivity = 232,
        FpsAimSensitivity = 233,
        FpsRagdoll = 234,
        FpsCombatroll = 235,
        FpsHeadbob = 236,
        FpsThirdPersonCover = 237,
        FpsAimDeadzone = 238,
        FpsAimAcceleration = 239,
        AimDeadzone = 240,
        AimAcceleration = 241,
        FpsAutoLevel = 242,
        HoodCamera = 243,
        FpsVehAutoCenter = 244,
        FpsRelativeVehicleCameraDrivebyAiming = 245,

        // Gen9 is using values 246-250. Do not use these.

        DisplayTextChat = 251,

        AudioSfxLevel = 300,
        AudioMusicLevel = 301,
        AudioVoiceOutput = 302,
        AudioGpsSpeech = 303,
        AudioHighDynamicRange = 304,
        AudioSpeakerOutput = 305,
        AudioMusicLevelInMp = 306,
        AudioInteractiveMusic = 307,
        AudioDialogueBoost = 308,
        AudioVoiceSpeakers = 309,
        AudioSsFront = 310,
        AudioSsRear = 311,
        AudioCtrlSpeaker = 312,
        AudioCtrlSpeakerVol = 313,
        AudioPulseHeadset = 314,
        AudioCtrlSpeakerHeadphone = 315,

        //RSG_PC
		AudioUrPlaymode = 316,
		AudioUrAutoscan = 317,
		AudioMuteOnFocusLoss = 318,

        DisplayReticule = 412,
        ControllerConfig = 413,
        DisplayFlickerFilter = 414,
        DisplayReticuleSize = 415,

        /// <summary>
        /// Represents the number of times the player has started a singleplayer game.
        /// </summary>
        NumSingleplayerGamesStarted = 450,

        AmbisonicDecoder = 600,
        //...All these values are used for the ambisonic decoder 
        AmbisonicDecoderEnd = 677,
        AmbisonicDecoderType = 678,

        /// <summary>
        /// Represents the graphics quality level for PC.
        /// </summary>
        /// <returns>
        /// An integer representing the graphics level in the inclusive range 0 - 4
        /// (0 = Low, 1 = Medium, 2 = High, 3 = Ultra, 4 = Custom)
        /// </returns>
        PcGraphicsLevel = 700,
        PcSystemLevel = 701,
        PcAudioLevel = 702,

        PcGfxVidOverride = 710,

        PcLastHardwareStatsUploadPosixtimeHigh32 = 711, //s64 value for the last successful hardware stats upload.
        PcLastHardwareStatsUploadPosixtimeLow32 = 712,

        PcVoiceEnabled = 720,
        PcVoiceOutputDevice = 721,
        PcVoiceOutputVolume = 722,
        PcVoiceTalkEnabled = 723,
        PcVoiceInputDevice = 724,
        PcVoiceChatMode = 725,
        PcVoiceMicVolume = 726,
        PcVoiceMicSensitivity = 727,
        PcVoiceSoundVolume = 728,
        PcVoiceMusicVolume = 729,

        MouseType = 750,
        MouseSub = 751,
        MouseDrive = 752,
        MouseFly = 753,
        MouseOnFootScale = 754,
        MouseDrivingScale = 755,
        MousePlaneScale = 756,
        MouseHeliScale = 757,
        KbmToggleAim = 758,
        KbmAltVehMouseControls = 759,
        MouseSubScale = 760,
        MouseWeightScale = 761,
        MouseAcceleration = 762,

        FeedPhone = 800,
        FeedStats = 801,
        FeedCrew = 802,
        FeedFriends = 803,
        FeedSocial = 804,
        FeedStore = 805,
        FeedTooptip = 806,
        FeedDelay = 807,

        StartUpFlow = 810,
        LandingPage = 811,

        GamerPlayedLastGen = 865, // Indicates if the player has played last gen, 1 - 360, 3 - ps3.
        GamerHasSpecialeditionContent = 866,  // Indicates if the player is entitled to special edition content

        FacebookLinkedHint = 899, // Returns a value != 0 if the last Facebook App Permissions call returned successful.
        FacebookUpdates = 900,

        RosWentDownNotNet = 901, // this is set to TRUE when the ROS link goes down but not the platform network conection
        /// <summary>
        /// Indicates whether the player had posted the "Bought GTAV" action/object to facebook.
        /// </summary>
        FacebookPostedBoughtGame = 902,
        PrologueComplete = 903,
        FacebookPostedAllVehiclesDriven = 904,
        EulaVersion = 905,
        TosVersion = 907,
        PrivacyVersion = 908,
        JobActivityIdChar0 = 909, //Id of the UGC activity started so we know if the player has pulled the plug.
        JobActivityIdChar1 = 910, //Id of the UGC activity started so we know if the player has pulled the plug.
        JobActivityIdChar2 = 911, //Id of the UGC activity started so we know if the player has pulled the plug.
        JobActivityIdChar3 = 912, //Id of the UGC activity started so we know if the player has pulled the plug.
        JobActivityIdChar4 = 913, //Id of the UGC activity started so we know if the player has pulled the plug.
        FreemodePrologueCompleteChar0 = 914, //Returns a value !=0 if the prologue was done.
        FreemodePrologueCompleteChar1 = 915, //Returns a value !=0 if the prologue was done.
        FreemodePrologueCompleteChar2 = 916, //Returns a value !=0 if the prologue was done.
        FreemodePrologueCompleteChar3 = 917, //Returns a value !=0 if the prologue was done.
        FreemodePrologueCompleteChar4 = 918, //Returns a value !=0 if the prologue was done.

        /// <summary>
        /// Indicates whether Franklin unlocked the companion system of Chop.
        /// </summary>
        SpChopMissionComplete = 939,

        FreemodeStrandProgressionStatusChar0 = 940,
        FreemodeStrandProgressionStatusChar1 = 941,

        ReplayMemLimit = 956,
        ReplayMode = 957,
        ReplayAutoResumeRecording = 958,

        VideoUploadPause = 959,
        VideoUploadPrivacy = 960,
        RockstarEditorTooltip = 961,
        VideoExportGraphicsUpgrade = 962,
        /// <summary>
        /// Indicates whether the  R* Editor tutorials have been seen. (1 = seen, 0 = not seen)
        /// </summary>
        RockstarEditorTutorialFlags = 963,
        ReplayAutoSaveRecording = 964,
        ReplayVideosCreated = 965,
    };
}
