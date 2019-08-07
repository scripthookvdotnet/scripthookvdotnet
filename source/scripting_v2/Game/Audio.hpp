#pragma once

#include "Vector3.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Entity;
	#pragma endregion

	public enum class AudioFlag
	{
		ActivateSwitchWheelAudio,
		AllowCutsceneOverScreenFade,
		AllowForceRadioAfterRetune,
		AllowPainAndAmbientSpeechToPlayDuringCutscene,
		AllowPlayerAIOnMission,
		AllowPoliceScannerWhenPlayerHasNoControl,
		AllowRadioDuringSwitch,
		AllowRadioOverScreenFade,
		AllowScoreAndRadio,
		AllowScriptedSpeechInSlowMo,
		AvoidMissionCompleteDelay,
		DisableAbortConversationForDeathAndInjury,
		DisableAbortConversationForRagdoll,
		DisableBarks,
		DisableFlightMusic,
		DisableReplayScriptStreamRecording,
		EnableHeadsetBeep,
		ForceConversationInterrupt,
		ForceSeamlessRadioSwitch,
		ForceSniperAudio,
		FrontendRadioDisabled,
		HoldMissionCompleteWhenPrepared,
		IsDirectorModeActive,
		IsPlayerOnMissionForSpeech,
		ListenerReverbDisabled,
		LoadMPData,
		MobileRadioInGame,
		OnlyAllowScriptTriggerPoliceScanner,
		PlayMenuMusic,
		PoliceScannerDisabled,
		ScriptedConvListenerMaySpeak,
		SpeechDucksScore,
		SuppressPlayerScubaBreathing,
		WantedMusicDisabled,
		WantedMusicOnMission
	};

	public ref class Audio sealed abstract
	{
	public:
		static int PlaySoundAt(Math::Vector3 position, System::String ^sound);
		static int PlaySoundAt(Math::Vector3 position, System::String ^sound, System::String ^set);
		static int PlaySoundFromEntity(Entity ^entity, System::String ^sound);
		static int PlaySoundFromEntity(Entity ^entity, System::String ^sound, System::String ^set);
		static int PlaySoundFrontend(System::String ^sound);
		static int PlaySoundFrontend(System::String ^sound, System::String ^set);

		static void StopSound(int id);
		static void ReleaseSound(int id);
		static bool HasSoundFinished(int id);

		static void SetAudioFlag(AudioFlag flag, bool toggle);
		static void SetAudioFlag(System::String ^flag, bool toggle);

	internal:
		static initonly array<System::String ^> ^_audioFlags =
		{ 
			"ActivateSwitchWheelAudio",
			"AllowCutsceneOverScreenFade",
			"AllowForceRadioAfterRetune",
			"AllowPainAndAmbientSpeechToPlayDuringCutscene",
			"AllowPlayerAIOnMission",
			"AllowPoliceScannerWhenPlayerHasNoControl",
			"AllowRadioDuringSwitch",
			"AllowRadioOverScreenFade",
			"AllowScoreAndRadio",
			"AllowScriptedSpeechInSlowMo",
			"AvoidMissionCompleteDelay",
			"DisableAbortConversationForDeathAndInjury",
			"DisableAbortConversationForRagdoll",
			"DisableBarks",
			"DisableFlightMusic",
			"DisableReplayScriptStreamRecording",
			"EnableHeadsetBeep",
			"ForceConversationInterrupt",
			"ForceSeamlessRadioSwitch",
			"ForceSniperAudio",
			"FrontendRadioDisabled",
			"HoldMissionCompleteWhenPrepared",
			"IsDirectorModeActive",
			"IsPlayerOnMissionForSpeech",
			"ListenerReverbDisabled",
			"LoadMPData",
			"MobileRadioInGame",
			"OnlyAllowScriptTriggerPoliceScanner",
			"PlayMenuMusic",
			"PoliceScannerDisabled",
			"ScriptedConvListenerMaySpeak",
			"SpeechDucksScore",
			"SuppressPlayerScubaBreathing",
			"WantedMusicDisabled",
			"WantedMusicOnMission"
		};
	};
}
