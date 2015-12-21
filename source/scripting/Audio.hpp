#pragma once

#include "Vector3.hpp"

namespace GTA
{
	ref class Entity;

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

	public ref class Audio sealed abstract {
	public:
		
		/* TODO
		static bool IsScriptedConversationLoaded();
		static bool IsScriptedConversationOngoing();
		static void PlayPain();
		static void PlayPoliceReport();
		static void PlaySound();
		static bool RequestAmbientAudioBank();
		static bool RequestMissionAudioBank();
		static bool RequestScriptAudioBank();
		static void StopPedSpeaking();
		*/

		static int GetSoundID();
		static bool HasSoundFinished(int id);
		static void PlaySoundFromCoord(System::String ^sound, Math::Vector3 pos);
		static void PlaySoundFromCoord(System::String ^sound, Math::Vector3 pos, System::String ^set);
		static void PlaySoundFromCoord(System::String ^sound, float posX, float posY, float posZ);
		static void PlaySoundFromCoord(System::String ^sound, float posX, float posY, float posZ, System::String ^set);
		static void PlaySoundFromEntity(System::String ^sound, Entity ^entity);
		static void PlaySoundFromEntity(System::String ^sound, Entity ^entity, System::String ^set);
		static void PlaySoundFromEntity(int id, System::String ^sound, Entity ^entity);
		static void PlaySoundFromEntity(int id, System::String ^sound, Entity ^entity, System::String ^set);
		static void PlaySoundFrontend(System::String ^sound);
		static void PlaySoundFrontend(System::String ^sound, System::String ^set);
		static void PlaySoundFrontend(int id, System::String ^sound);
		static void PlaySoundFrontend(int id, System::String ^sound, System::String ^set);
		static void PlaySoundFrontend(int id, System::String ^sound, System::String ^set, int i_1);
		static void ReleaseSoundId(int id);
		static void SetAudioFlag(AudioFlag flag, bool toggle);
		static void SetAudioFlag(System::String ^flag, bool toggle);
		static void StopSound(int id);

	internal:
		static initonly array<System::String ^> ^sAudioFlags = { 
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
