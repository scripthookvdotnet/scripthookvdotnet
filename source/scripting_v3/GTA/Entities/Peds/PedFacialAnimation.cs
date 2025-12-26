namespace GTA
{
    /// <summary>
    /// Specifies the available facial animation states for a <see cref="Ped"/>.
    /// </summary>
    /// <remarks>
    /// Note that not all facial animations may be supported by every <see cref="Ped"/> model/gender.
    /// </remarks>
    public enum PedFacialAnimation
    {
        Blowkiss_1,
        Burning_1,
        Coughing_1,
        Dead_1,
        Dead_2,
        Die_1,
        Die_2,
        Drinking_1,
        Eating_1,
        Effort_1,
        Effort_2,
        Effort_3,
        Electrocuted_1,
        High_Transition_01,
        High_Transition_02,
        Melee_Effort_1,
        Melee_Effort_2,
        Melee_Effort_3,
        Mood_Aiming_1,
        Mood_Angry_1,
        Mood_Dancing_High_1,
        Mood_Dancing_High_2,
        Mood_Dancing_Low_1,
        Mood_Dancing_Low_2,
        Mood_Dancing_Low_3,
        Mood_Dancing_Medium_1,
        Mood_Dancing_Medium_2,
        Mood_Dancing_Medium_3,
        Mood_Dancing_Medium_4,
        Mood_Dancing_Trance_1,
        Mood_Dancing_Trance_2,
        Mood_Dancing_Trance_3,
        Mood_Drivefast_1,
        Mood_Drunk_1,
        Mood_Excited_1,
        Mood_Frustrated_1,
        Mood_Happy_1,
        Mood_Injured_1,
        Mood_Knockout_1,
        Mood_Normal_1,
        Mood_Skydive_1,
        Mood_Sleeping_1,
        Mood_Smug_1,
        Mood_Stressed_1,
        Mood_Sulk_1,
        Mood_Talking_1,
        Pain_1,
        Pain_2,
        Pain_3,
        Pain_4,
        Pain_5,
        Pain_6,
        Pose_Aiming_1,
        Pose_Angry_1,
        Pose_Happy_1,
        Pose_Injured_1,
        Pose_Normal_1,
        Pose_Smug_1,
        Pose_Stressed_1,
        Pose_Sulk_1,
        Shocked_1,
        Shocked_2,
        Smoking_Exhale_1,
        Smoking_Hold_1,
        Smoking_Inhale_1
    }

    public static class PedFacialAnimationExtensions
    {
        public static string GetAnimationName(this PedFacialAnimation facialAnimation)
            => facialAnimation.ToString().ToLower();
    }
}
