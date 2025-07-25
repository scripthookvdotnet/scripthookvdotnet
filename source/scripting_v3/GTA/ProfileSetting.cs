namespace GTA
{
    public enum ProfileSetting
    {
        /// <summary>
        /// Represents an invalid or uninitialized profile setting.
        /// Used to indicate that a profile setting value is missing or not applicable.
        /// </summary>
        InvalidProfileSetting = -1,

        /// <summary>
        /// Represents the Controller Targeting Mode (0 = Assisted Aim - Full, 1 = Assisted Aim - Partial, 2 = Free Aim - Assisted, 3 = Free Aim).
        /// </summary>
        TargetingMode = 0,

        /// <summary>
        /// Indicates whether the controller axis is inverted (1 = Enabled, 0 = Disabled).
        /// </summary>
        AxisInversion = 1,

        /// <summary>
        /// Indicates whether controller vibration is enabled (1 = Enabled, 0 = Disabled).
        /// </summary>
        ControllerVibration = 2,

        /// <summary>
        /// Represents the 3rd person controller control type, see <see cref="GTA.ControllerControlType"/>.
        /// </summary>
        ControllerControlConfig = 12,

        /// <summary>
        /// Represents the 3rd person controller aim sensitivity (Range 0-14, 0 = Slowest, 14 = Highest).
        /// </summary>
        ControllerAimSensitivity = 13,

        /// <summary>
        /// Represents the 3rd person controller look-around sensitivity (Range 0-14, 0 = Slowest, 14 = Highest).
        /// </summary>
        LookAroundSensitivity = 14,

        /// <summary>
        /// Indicates whether mouse controls are inverted (1 = Enabled, 0 = Disabled)
        /// </summary>
		MouseInversion = 15,

        /// <summary>
        /// Indicates whether you can move around with your controller while zoomed in (1 = Enabled, 0 = Disabled).
        /// </summary>
        ControllerAllowMovementWhenZoomed = 16,

        /// <summary>
        /// Represents the 1st person controller control type, see <see cref="GTA.ControllerControlType"/>.
        /// </summary>
        ControllerControlConfigFps = 20,

        /// <summary>
        /// Indicates whether the mouse axis is inverted while flying (1 = Enabled, 0 = Disabled).
        /// </summary>
		MouseInversionFlying = 21,

        /// <summary>
        /// Indicates whether the mouse axis is inverted while in a submarine (1 = Enabled, 0 = Disabled).
        /// </summary>
        MouseInversionSub = 22,

        /// <summary>
        /// Indicates how fast the camera centers itself to the car while driving, (Range 0-10). (0 = Slowest, 10 = Fastest).
        /// </summary>
		MouseAutocenterCar = 23,

        /// <summary>
        /// Indicates how fast the camera centers itself to the bike while driving, (Range 0-10). (0 = Slowest, 10 = Fastest).
        /// </summary>
        MouseAutocenterBike = 24,

        /// <summary>
        /// Indicates how fast the camera centers itself to the aircraft while flying, (Range 0-10). (0 = Slowest, 10 = Fastest).
        /// </summary>
        MouseAutocenterPlane = 25,

        /// <summary>
        /// Represents the mouse flying control type (0 = Roll/Pitch, 1 = Yaw/Pitch).
        /// </summary>
		MouseSwapRollYawFlying = 26,

        /// <summary>
        /// Indicates whether subtitles are enabled (1 = Enabled, 0 = Disabled).
        /// </summary>
        DisplaySubtitles = 203,

        /// <summary>
        /// Represents the radar mode, range 0 - 2, (0 = Off, 1 = On, 2 = Blip).
        /// </summary>
        DisplayRadarMode = 204,

        /// <summary>
        /// Indicates whether the HUD is enabled (1 = Enabled, 0 = Disabled).
        /// </summary>
        DisplayHudMode = 205,
        DisplayLanguage = 206,

        /// <summary>
        /// Indicates whether the GPS is enabled (1 = Enabled, 0 = Disabled).
        /// </summary>
        DisplayGps = 207,
        /// <summary>
        /// Indicates whether the Autosaving feature is enabled (1 = Enabled, 0 = Disabled).
        /// </summary>
        DisplayAutosaveMode = 208,

        /// <summary>
        /// Indicates where the camera is placed while in a vehicle and 1st person. (1 = Hood, 0 = Player)
        /// </summary>
        DisplayHandbrakeCam = 209,

        /// <inheritdoc cref="DisplayHandbrakeCam"/>
        DisplayHoodCam = DisplayHandbrakeCam,

        /// <summary>
        /// Legacy version of <see cref="DisplayGamma"/>, exact game version is unknown.
        /// </summary>
        LegacyDoNotUseDisplayGamma = 210,

        ControllerCinematicShooting = 211,

        /// <summary>
        /// Represents the display safezone size (Range 0-10).
        /// </summary>
        /// <remarks>
        /// The safezone is the area the game places HUD elements in.
        /// The larger the safe zone, the smaller the area where hud elements are placed.
        /// </remarks>
        DisplaySafezoneSize = 212,

        /// <summary>
        /// Represents the display gama / brightness (Range 0-30, 0 = Darkest, 30 = Brightest).
        /// </summary>
        DisplayGamma = 213,

        /// <summary>
        /// Represented an unknown display setting in past versions.
        /// </summary>
        DisplayLegacyUnused1 = 214,

        /// <inheritdoc cref="DisplayLegacyUnused1"/>
        DisplayLegacyUnused2 = 215,

        /// <inheritdoc cref="DisplayLegacyUnused1"/>
        DisplayLegacyUnused3 = 216,

        /// <inheritdoc cref="DisplayLegacyUnused1"/>
        DisplayLegacyUnused4 = 217,

        /// <inheritdoc cref="DisplayLegacyUnused1"/>
        DisplayLegacyUnused5 = 218,

        /// <inheritdoc cref="DisplayLegacyUnused1"/>
        DisplayLegacyUnused6 = 219,

        /// <summary>
        /// Represents the camera height while driving (0 = Low, 1 = High).
        /// </summary>
        DisplayCameraHeight = 220,

        /// <summary>
        /// Indicates whether the big radar is enabled (1 = Enabled, 0 = Disabled).
        /// </summary>
        DisplayBigRadar = 221,

        DisplayBigRadarNames = 222,
        ControllerDuckHandbrake = 223,
        DisplayDof = 224,
        ControllerDriveby = 225,

        /// <summary>
        /// Indicates whether screen kill effects are enabled (1 = Enabled, 0 = Disabled).
        /// </summary>
        DisplayScreenKillFx = 226,

        /// <summary>
        /// Represents whether the game uses metric or imperial units (0 = Imperial, 1 = Metric).
        /// </summary>
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

        /// <summary>
        /// The volume of SFX in-game (Range 0-10, 0 = Quietest, 10 = Loudest).
        /// </summary>
        AudioSfxLevel = 300,

        /// <summary>
        /// The volume of Music in SP (Range 0-10, 0 = Quietest, 10 = Loudest).
        /// </summary>
        AudioMusicLevel = 301,
        AudioVoiceOutput = 302,
        AudioGpsSpeech = 303,
        AudioHighDynamicRange = 304,



        AudioSpeakerOutput = 305,

        /// <summary>
        /// The volume of Music in MP (Range 0-10, 0 = Quietest, 10 = Loudest).
        /// </summary>
        AudioMusicLevelInMp = 306,
        AudioInteractiveMusic = 307,

        /// <summary>
        /// The amount of "Boost" applied to dialogue in-game (Range 0-10, 0 = No-Boost, 10 = Most-Boost).
        /// </summary>
        AudioDialogueBoost = 308,
        AudioVoiceSpeakers = 309,
        AudioSsFront = 310,
        AudioSsRear = 311,
        AudioCtrlSpeaker = 312,
        AudioCtrlSpeakerVol = 313,
        AudioPulseHeadset = 314,
        AudioCtrlSpeakerHeadphone = 315,

		AudioUrPlaymode = 316,
		AudioUrAutoscan = 317,

        /// <summary>
        /// Indicates whether audio should be muted if GTA is not in focus (0 = Do-Not-Mute, 1 = Mute-Audio).
        /// </summary>
		AudioMuteOnFocusLoss = 318,

        /// <summary>
        /// Indicates whether the recticle should be shown on screen (0: Show, 1: Hide).
        /// </summary>
        DisplayReticule = 412,
        ControllerConfig = 413,
        DisplayFlickerFilter = 414,

        /// <summary>
        /// Indicates the size of the recticle (Range 0-10, 0 = Smallest, 10 = Largest)
        /// </summary>
        /// <remarks>
        /// Only applies to the simple recticle!
        /// </remarks>
        DisplayReticuleSize = 415,

        /// <summary>
        /// Represents the number of times the player has started a singleplayer game.
        /// </summary>
        NumSingleplayerGamesStarted = 450,

        AmbisonicDecoder = 600,
        //...All these values are used for the Ambisonic decoder 
        AmbisonicDecoderEnd = 677,
        AmbisonicDecoderType = 678,

        /// <summary>
        /// Represents the graphics quality level for PC.
        /// </summary>
        /// <returns>
        /// An integer representing the graphics level in the inclusive (Range 0-4).
        /// (0 = Low, 1 = Medium, 2 = High, 3 = Ultra, 4 = Custom)
        /// </returns>
        PcGraphicsLevel = 700,
        PcSystemLevel = 701,
        PcAudioLevel = 702,

        PcGfxVidOverride = 710,

        PcLastHardwareStatsUploadPosixtimeHigh32 = 711, //s64 value for the last successful hardware stats upload.
        PcLastHardwareStatsUploadPosixtimeLow32 = 712,

        /// <summary>
        /// Indicates whether the user has enabled VoiceChat. 
        /// </summary>
        PcVoiceEnabled = 720,

        /// <summary>
        /// Represents the index of the VoiceChat output device.
        /// </summary>
        PcVoiceOutputDevice = 721,

        /// <summary>
        /// Represents the volume of the VoiceChat (Range 0-10, 0 = Quietest, 10 = Loudest).
        /// </summary>
        PcVoiceOutputVolume = 722,

        /// <summary>
        /// Indicates the activation method of the Microphone (1 = VoiceActivated, 0 = PushToTalk).
        /// </summary>
        PcVoiceTalkEnabled = 723,

        /// <summary>
        /// Represents the index of the VoiceChat input device.
        /// </summary>
        PcVoiceInputDevice = 724,
        PcVoiceChatMode = 725,

        /// <summary>
        /// Represents the volume of the VoiceChat (Range 0-10, 0 = Quietest, 10 = Loudest).
        /// </summary>
        PcVoiceMicVolume = 726,

        /// <summary>
        /// Represents the sensitivity of the VoiceChat microphone (Range 0-10, 0 = Most-Sensitive, 10-Most-Insensitive).
        /// </summary>
        PcVoiceMicSensitivity = 727,

        /// <summary>
        /// Represents the volume of SFX sounds while people are talking in VoiceChat (Range 0-10, 0 = Quietest, 10 = Loudest).
        /// </summary>
        PcVoiceSoundVolume = 728,

        /// <summary>
        /// Represents the volume of Music while people are talking in VoiceChat (Range 0-10, 0 = Quietest, 10 = Loudest).
        /// </summary>
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

        /// <summary>
        /// Indicates whether the player is entitled to special edition content ( 0: Not-Entitled, 1: Entitled)
        /// </summary>
        GamerHasSpecialeditionContent = 866,


        /// <summary>
        /// Indicates that the Rockstar Online Services is not available but the client is still connected to the platform network (0: Running, 1: ROS Down).
        /// </summary>
        RosWentDownNotNet = 901,

        /// <summary>
        /// Indicates whether the player has finished the prologue mission (0 = Not-Completed, 1 = Completed)
        /// </summary>
        PrologueComplete = 903,

        /// <summary>
        /// Represents the version of the Eula.
        /// </summary>
        EulaVersion = 905,

        /// <summary>
        /// Represents the version of the Terms-Of-Service.
        /// </summary>
        TosVersion = 907,

        /// <summary>
        /// Represents the version of the Privacy-Agreement.
        /// </summary>
        PrivacyVersion = 908,

        /// <summary>
        /// Indicates whether Franklin unlocked the companion system of Chop.
        /// </summary>
        SpChopMissionComplete = 939,

        /// <summary>
        /// Indicates how much storage is allocated to the Rockstar Editor, ranges from 0 to 10.
        /// 0: 250MB, 1: 500MB, 2: 750MB, 3: 1GB, 4: 1.5GB, 5: 2GB, 6: 5GB, 7: 10GB, 8: 15GB, 9: 25GB, 10: 50GB.
        /// </summary>
        ReplayMemLimit = 956,

        /// <summary>
        /// Indicates whether the replay mode is enabled (1 = Enabled, 0 = Disabled).
        /// </summary>
        ReplayMode = 957,

        ReplayAutoResumeRecording = 958,

        VideoUploadPause = 959,

        /// <summary>
        /// Indicates the visibility of uploaded videos to YouTube (0: Public, 1: Unlisted, 2: Private)
        /// </summary>
        VideoUploadPrivacy = 960,

        RockstarEditorTooltip = 961,
        VideoExportGraphicsUpgrade = 962,
        /// <summary>
        /// Indicates whether the  R* Editor tutorials have been seen. (1 = Seen, 0 = Not-Seen)
        /// </summary>
        RockstarEditorTutorialFlags = 963,
        ReplayAutoSaveRecording = 964,
        ReplayVideosCreated = 965,
    };
}
