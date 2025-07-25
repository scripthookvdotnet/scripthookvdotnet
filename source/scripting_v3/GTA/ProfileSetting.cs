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
        /// Represents the Controller Targeting Mode (0: Assisted Aim - Full, 1: Assisted Aim - Partial, 2: Free Aim - Assisted, 3: Free Aim).
        /// </summary>
        TargetingMode = 0,

        /// <summary>
        /// Indicates whether the controller axis is inverted (1: Enabled, 0: Disabled).
        /// </summary>
        AxisInversion = 1,

        /// <summary>
        /// Indicates whether controller vibration is enabled (1: Enabled, 0: Disabled).
        /// </summary>
        ControllerVibration = 2,

        /// <summary>
        /// Represents the 3rd person controller control type, see <see cref="GTA.ControllerControlType"/>.
        /// </summary>
        ControllerControlConfig = 12,

        /// <summary>
        /// Represents the 3rd person controller aim sensitivity (Range 0-14, 0: Slowest, 14: Highest).
        /// </summary>
        ControllerAimSensitivity = 13,

        /// <summary>
        /// Represents the 3rd person controller look-around sensitivity (Range 0-14, 0: Slowest, 14: Highest).
        /// </summary>
        LookAroundSensitivity = 14,

        /// <summary>
        /// Indicates whether mouse controls are inverted (1: Enabled, 0: Disabled)
        /// </summary>
		MouseInversion = 15,

        /// <summary>
        /// Indicates whether you can move around with your controller while zoomed in (1: Enabled, 0: Disabled).
        /// </summary>
        ControllerAllowMovementWhenZoomed = 16,

        /// <summary>
        /// Represents the 1st person controller control type, see <see cref="GTA.ControllerControlType"/>.
        /// </summary>
        ControllerControlConfigFps = 20,

        /// <summary>
        /// Indicates whether the mouse axis is inverted while flying (1: Enabled, 0: Disabled).
        /// </summary>
		MouseInversionFlying = 21,

        /// <summary>
        /// Indicates whether the mouse axis is inverted while in a submarine (1: Enabled, 0: Disabled).
        /// </summary>
        MouseInversionSub = 22,

        /// <summary>
        /// Indicates how fast the camera centers itself to the car while driving, (Range 0-10). (0: Slowest, 10: Fastest).
        /// </summary>
		MouseAutocenterCar = 23,

        /// <summary>
        /// Indicates how fast the camera centers itself to the bike while driving, (Range 0-10). (0: Slowest, 10: Fastest).
        /// </summary>
        MouseAutocenterBike = 24,

        /// <summary>
        /// Indicates how fast the camera centers itself to the aircraft while flying, (Range 0-10). (0: Slowest, 10: Fastest).
        /// </summary>
        MouseAutocenterPlane = 25,

        /// <summary>
        /// Represents the mouse flying control type (0: Roll/Pitch, 1: Yaw/Pitch).
        /// </summary>
		MouseSwapRollYawFlying = 26,

        /// <summary>
        /// Indicates whether subtitles are enabled (1: Enabled, 0: Disabled).
        /// </summary>
        DisplaySubtitles = 203,

        /// <summary>
        /// Represents the radar mode, range 0 - 2, (0: Off, 1: On, 2: Blip).
        /// </summary>
        DisplayRadarMode = 204,

        /// <summary>
        /// Indicates whether the HUD is enabled (1: Enabled, 0: Disabled).
        /// </summary>
        DisplayHudMode = 205,
        DisplayLanguage = 206,

        /// <summary>
        /// Indicates whether the GPS is enabled (1: Enabled, 0: Disabled).
        /// </summary>
        DisplayGps = 207,
        /// <summary>
        /// Indicates whether the Autosaving feature is enabled (1: Enabled, 0: Disabled).
        /// </summary>
        DisplayAutosaveMode = 208,

        /// <summary>
        /// Indicates where the camera is placed while in a vehicle and 1st person. (1: Hood, 0: Player)
        /// </summary>
        DisplayHandbrakeCam = 209,

        /// <inheritdoc cref="DisplayHandbrakeCam"/>
        DisplayHoodCam = DisplayHandbrakeCam,

        /// <summary>
        /// Legacy version of <see cref="DisplayGamma"/>, exact game version is unknown.
        /// </summary>
        LegacyDisplayGamma = 210,

        /// <summary>
        /// Unknown
        /// </summary>
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
        /// Represents the display gama / brightness (Range 0-30, 0: Darkest, 30: Brightest).
        /// </summary>
        DisplayGamma = 213,

        /// <summary>
        /// Represented an unknown display setting used in past versions.
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
        /// Represents the camera height while driving (0: Low, 1: High).
        /// </summary>
        DisplayCameraHeight = 220,

        /// <summary>
        /// Indicates whether the Big Radar should be used (1: Enabled, 0: Disabled).
        /// </summary>
        DisplayBigRadar = 221,

        /// <summary>
        /// Unknown
        /// </summary>
        DisplayBigRadarNames = 222,

        /// <summary>
        /// Indicates whether the control for the Handbreak is switched with the one for Hydraulics / Duck (0: Normal, 1: Switched).
        /// </summary>
        ControllerDuckHandbrake = 223,

        /// <summary>
        /// Indicates whether Depth Of Field Effects are enabled In-Game (0: Disabled, 1: Enabled).
        /// </summary>
        DisplayDof = 224,

        /// <summary>
        /// Represents the controller controls for Drive-Bys (0: Aim, 1: Aim + Fire).
        /// </summary>
        ControllerDriveby = 225,

        /// <summary>
        /// Indicates whether Screen Kill Effects are enabled (1: Enabled, 0: Disabled).
        /// </summary>
        DisplayScreenKillFx = 226,

        /// <summary>
        /// Represents the measurement system the game uses (0: Imperial, 1: Metric).
        /// </summary>
        MeasurementSystem = 227,
        NewDisplayLanguage = 228,

        /// <summary>
        /// Represents the first person aim type on mouse and keyboard (0: Normal, 1: Iron Sights)
        /// </summary>
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

        /// <summary>
        /// Indicates whether the TextChat in MP should be shown (0: Hidden, 1: Shown)
        /// </summary>
        DisplayTextChat = 251,

        /// <summary>
        /// The volume of SFX in-game (Range 0-10, 0: Quietest, 10: Loudest).
        /// </summary>
        AudioSfxLevel = 300,

        /// <summary>
        /// The volume of Music in SP (Range 0-10, 0: Quietest, 10: Loudest).
        /// </summary>
        AudioMusicLevel = 301,
        AudioVoiceOutput = 302,

        /// <summary>
        /// An unknown parameter, returns one most of the time.
        /// </summary>
        AudioGpsSpeech = 303,

        /// <summary>
        /// Represents the index of the output device used for game audio.
        /// </summary>
        AudioSpeakerOutput = 305,

        /// <summary>
        /// The volume of Music in MP (Range 0-10, 0: Quietest, 10: Loudest).
        /// </summary>
        AudioMusicLevelInMp = 306,
        AudioInteractiveMusic = 307,

        /// <summary>
        /// The amount of "Boost" applied to dialogue in-game (Range 0-10, 0: No-Boost, 10: Most-Boost).
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
        /// Indicates whether audio should be muted if GTA is not in focus (0: Unmuted, 1: Mute-Audio).
        /// </summary>
		AudioMuteOnFocusLoss = 318,

        /// <summary>
        /// Indicates whether the recticle should be shown on screen (0: Show, 1: Hide).
        /// </summary>
        DisplayReticule = 412,
        ControllerConfig = 413,
        DisplayFlickerFilter = 414,

        /// <summary>
        /// Indicates the size of the recticle (Range 0-10, 0: Smallest, 10: Largest)
        /// </summary>
        /// <remarks>
        /// Only applies to the simple recticle!
        /// </remarks>
        DisplayReticuleSize = 415,

        /// <summary>
        /// Represents the number of times the player has started a singleplayer game.
        /// </summary>
        NumSingleplayerGamesStarted = 450,

        //600-678 Are for the Ambisonic decoder
        AmbisonicDecoder = 600,
        AmbisonicDecoderEnd = 677,
        AmbisonicDecoderType = 678,

        /// <summary>
        /// Represents the graphics quality level for PC.
        /// </summary>
        /// <returns>
        /// An integer representing the graphics level. (Range 0-4, 0: Low, 1: Medium, 2: High, 3: Ultra, 4: Custom).
        /// </returns>
        PcGraphicsLevel = 700,
        PcSystemLevel = 701,
        PcAudioLevel = 702,

        /// <summary>.
        /// Indicates whether the game ignores the suggested VRAM limit (0: Disabled, 1: Enabled)
        /// </summary>
        PcGfxVidOverride = 710,

        /// <summary>
        /// If merged with <see cref="PcLastHardwareStatsUploadPosixtimeLow32"/> represents the last time hardware stats were uploaded.
        /// </summary>
        PcLastHardwareStatsUploadPosixtimeHigh32 = 711,
        /// <summary>
        /// If merged with <see cref="PcLastHardwareStatsUploadPosixtimeHigh32"/> represents the last time hardware stats were uploaded.
        /// </summary>
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
        /// Represents the volume of the VoiceChat (Range 0-10, 0: Quietest, 10: Loudest).
        /// </summary>
        PcVoiceOutputVolume = 722,

        /// <summary>
        /// Indicates the activation method of the Microphone (1: VoiceActivated, 0: PushToTalk).
        /// </summary>
        PcVoiceTalkEnabled = 723,

        /// <summary>
        /// Represents the index of the VoiceChat input device.
        /// </summary>
        PcVoiceInputDevice = 724,
        PcVoiceChatMode = 725,

        /// <summary>
        /// Represents the volume of the VoiceChat (Range 0-10, 0: Quietest, 10: Loudest).
        /// </summary>
        PcVoiceMicVolume = 726,

        /// <summary>
        /// Represents the sensitivity of the VoiceChat microphone (Range 0-10, 0: Most-Sensitive, 10: Most-Insensitive).
        /// </summary>
        PcVoiceMicSensitivity = 727,

        /// <summary>
        /// Represents the volume of SFX sounds while people are talking in VoiceChat (Range 0-10, 0: Quietest, 10: Loudest).
        /// </summary>
        PcVoiceSoundVolume = 728,

        /// <summary>
        /// Represents the volume of Music while people are talking in VoiceChat (Range 0-10, 0: Quietest, 10: Loudest).
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

        /// <summary>
        /// Indicates whether you need to hold down the aim button or press it once to aim (0: Hold, 1: Toggle).
        /// </summary>
        KbmToggleAim = 758,
        KbmAltVehMouseControls = 759,
        MouseSubScale = 760,
        MouseWeightScale = 761,
        MouseAcceleration = 762,

        /// <summary>
        /// Indicates whether Phone Alerts should be shown (0: OFF, 1: ON).
        /// </summary>
        FeedPhone = 800,

        /// <summary>
        /// Indicates whether Stats Alerts should be shown (0: OFF, 1: ON).
        /// </summary>
        FeedStats = 801,

        /// <summary>
        /// Indicates whether Crew Updates should be shown (0: OFF, 1: ON).
        /// </summary>
        FeedCrew = 802,

        /// <summary>
        /// Indicates whether Friend Updates should be shown (0: OFF, 1: ON).
        /// </summary>
        FeedFriends = 803,

        /// <summary>
        /// Indicates whether R* Game Service notifications should be shown (0: OFF, 1: ON).
        /// </summary>
        FeedSocial = 804,

        /// <summary>
        /// Indicates whether Store notifications should be shown (0: OFF, 1: ON).
        /// </summary>
        FeedStore = 805,

        /// <summary>
        /// Indicates whether Tooltips should be shown (0: OFF, 1: ON).
        /// </summary>
        FeedTooptip = 806,

        /// <summary>
        /// Represents the delay with notifications are shown (Range 0-9, 0: No Delay, 1: 1 Minute, 2: 2Minutes, 3: 3Minutes, 4: 4Minutes, 5: 5Minutes, 6: 10Minutes, 7: 15Minutes, 8: 30Minutes, 9: 1 Hour).
        /// </summary>
        FeedDelay = 807,

        /// <summary>
        /// Indicates what the game should boot if the landing page is disabled (0: Singleplayer, 1: Multiplayer).
        /// </summary>
        /// <remarks>
        /// Value is ignored if <see cref="LandingPage"/> is set to <value>1</value>.
        /// </remarks>
        StartUpFlow = 810,

        /// <summary>
        /// Indicates whether the Game should show the landing page on start or directly boot into SP or MP (0: Boot into <see cref="StartUpFlow"/>, 1: Show-Landing-Page).
        /// </summary>
        LandingPage = 811,

        /// <summary>
        /// Indicates whether the player is entitled to special edition content (0: Not-Entitled, 1: Entitled).
        /// </summary>
        GamerHasSpecialeditionContent = 866,

        /// <summary>
        /// Indicates that the Rockstar Online Services is not available but the client is still connected to the platform network (0: Running, 1: ROS Down).
        /// </summary>
        RosWentDownNotNet = 901,

        /// <summary>
        /// Indicates whether the player has finished the prologue mission (0: Not-Completed, 1: Completed).
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
        /// Indicates whether the replay mode is enabled (1: Enabled, 0: Disabled).
        /// </summary>
        ReplayMode = 957,

        ReplayAutoResumeRecording = 958,

        VideoUploadPause = 959,

        /// <summary>
        /// Indicates the visibility of uploaded videos to YouTube (0: Public, 1: Unlisted, 2: Private).
        /// </summary>
        VideoUploadPrivacy = 960,

        /// <summary>
        /// Indicates whether tooltips should be shown for the R* Editor (0: Hide, 1: Show).
        /// </summary>
        RockstarEditorTooltip = 961,

        /// <summary>
        /// Indicates whether the Graphic settings should be upgraded for replay exports (0: Use-Current, 1: Upgrade-Graphics).
        /// </summary>
        VideoExportGraphicsUpgrade = 962,

        /// <summary>
        /// Indicates whether the  R* Editor tutorials have been seen (1: Seen, 0: Not-Seen).
        /// </summary>
        RockstarEditorTutorialFlags = 963,

        /// <summary>
        /// Indicates whether an Action Replay should be automatically saved on death (0: Continue, 1: Save Replay).
        /// </summary>
        ReplayAutoSaveRecording = 964,

        /// <summary>
        /// Represents the amount of videos created with the R* Editor.
        /// </summary>
        ReplayVideosCreated = 965,
    };
}
