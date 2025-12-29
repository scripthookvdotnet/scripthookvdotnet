namespace GTA
{
    /// <summary>
    /// An enumeration of common facial animations that can be applied to all peds, as long as the facial animation <see cref="ClipSet"/> of the specified <see cref="Ped"/> is set to the "base" (default) variant.
    /// </summary>
    public enum CommonPedFacialAnimation
    {
        Burning,
        Coughing,
        Dead1,
        Dead2,
        Die1,
        Die2,
        Electrocuted,
        Effort,
        MeleeEffort1,
        MeleeEffort2,
        MeleeEffort3,
        MoodAiming,
        MoodAngry,
        MoodDrivefast,
        MoodDrunk,
        MoodExcited,
        MoodFrustrated,
        MoodHappy,
        MoodInjured,
        MoodNormal,
        MoodSleeping,
        MoodSkydive,
        MoodStressed,
        MoodTalking,
        Pain1,
        Pain2,
        Pain3,
        Pain4,
        Pain5,
        Pain6
    }

    public static class CommonPedFacialAnimationExtensions
    {
        public static string GetAnimationName(this CommonPedFacialAnimation animation)
        {
            return animation switch
            {
                CommonPedFacialAnimation.Burning => "burning_1",
                CommonPedFacialAnimation.Coughing => "coughing_1",
                CommonPedFacialAnimation.Dead1 => "dead_1",
                CommonPedFacialAnimation.Dead2 => "dead_2",
                CommonPedFacialAnimation.Die1 => "die_1",
                CommonPedFacialAnimation.Die2 => "die_2",
                CommonPedFacialAnimation.Electrocuted => "electrocuted_1",
                CommonPedFacialAnimation.Effort => "effort_1",
                CommonPedFacialAnimation.MeleeEffort1 => "melee_effort_1",
                CommonPedFacialAnimation.MeleeEffort2 => "melee_effort_2",
                CommonPedFacialAnimation.MeleeEffort3 => "melee_effort_3",
                CommonPedFacialAnimation.MoodAiming => "mood_aiming_1",
                CommonPedFacialAnimation.MoodAngry => "mood_angry_1",
                CommonPedFacialAnimation.MoodDrivefast => "mood_drivefast_1",
                CommonPedFacialAnimation.MoodDrunk => "mood_drunk_1",
                CommonPedFacialAnimation.MoodExcited => "mood_excited_1",
                CommonPedFacialAnimation.MoodFrustrated => "mood_frustrated_1",
                CommonPedFacialAnimation.MoodHappy => "mood_happy_1",
                CommonPedFacialAnimation.MoodInjured => "mood_injured_1",
                CommonPedFacialAnimation.MoodNormal => "mood_normal_1",
                CommonPedFacialAnimation.MoodSleeping => "mood_sleeping_1",
                CommonPedFacialAnimation.MoodSkydive => "mood_skydive_1",
                CommonPedFacialAnimation.MoodStressed => "mood_stressed_1",
                CommonPedFacialAnimation.MoodTalking => "mood_talking_1",
                CommonPedFacialAnimation.Pain1 => "pain_1",
                CommonPedFacialAnimation.Pain2 => "pain_2",
                CommonPedFacialAnimation.Pain3 => "pain_3",
                CommonPedFacialAnimation.Pain4 => "pain_4",
                CommonPedFacialAnimation.Pain5 => "pain_5",
                CommonPedFacialAnimation.Pain6 => "pain_6",
                _ => "",
            };
        }
    }
}
