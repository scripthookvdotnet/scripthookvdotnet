using GTA;
using GTA.Math;
using GTA.NaturalMotion;
using System;
using System.Collections.Generic;
using System.Linq;

// Mimic electrocution with the stun gun by sending NM messages to the player ped.
// You can find similar config in physicstasks.ymt (search for "CTaskNMShot").
public class FakeElectrocutionShotDemo : Script
{
	public FakeElectrocutionShotDemo()
	{
		KeyDown += FakeElectrocutionShotDemo_KeyDown;
	}

	// Shot_Base (priority: 0)
	private static void StartShotBaseNmMessages(Ped ped)
	{
		BalancerCollisionsReactionHelper balancerCollisionsReactionNM = ped.Euphoria.BalancerCollisionsReaction;
		balancerCollisionsReactionNM.Start();

		FallOverWallHelper fallOverWallNM = ped.Euphoria.FallOverWall;
		fallOverWallNM.MoveLegs = true;
		fallOverWallNM.MoveArms = false;
		fallOverWallNM.BendSpine = true;
		fallOverWallNM.RollingPotential = 0.3f;
		fallOverWallNM.RollingBackThr = 0.5f;
		fallOverWallNM.ForceTimeOut = 2f;
		fallOverWallNM.MagOfForce = 0.5f;
		fallOverWallNM.BodyTwist = 0.54f;
		fallOverWallNM.Start();

		ShotHelper shotNM = ped.Euphoria.Shot;
		shotNM.Start();

		ShotConfigureArmsHelper ShotConfigureArmsNM = ped.Euphoria.ShotConfigureArms;
		ShotConfigureArmsNM.PointGun = true;
		ShotConfigureArmsNM.Start();

		// You can send NM message to a ped without setting "start" parameter to true so this message can update parameters of the running behavior
		ConfigureBalanceHelper ConfigureBalanceNM = ped.Euphoria.ConfigureBalance;
		ConfigureBalanceNM.FallMult = 40f;
		ConfigureBalanceNM.Update();
	}

	// normal in WeaponSets (priority: 5)
	// NmShotTuningSet is set to normal in WEAPON_STUNGUN config in weapons.meta.
	private static void StartNormalWeapomNmMessages(Ped ped)
	{
		ConfigureBalanceHelper ConfigureBalanceNM = ped.Euphoria.ConfigureBalance;
		ConfigureBalanceNM.StableLinSpeedThresh = 0.7f;
		ConfigureBalanceNM.StableRotSpeedThresh = 0.85f;
		ConfigureBalanceNM.UseComDirTurnVelThresh = 0.6f;
		ConfigureBalanceNM.StepIfInSupport = true;
		ConfigureBalanceNM.BackwardsLeanCutoff = 0.3f;
		ConfigureBalanceNM.ExtraSteps = -1;
		ConfigureBalanceNM.ExtraTime = -1.0f;
		ConfigureBalanceNM.MaxBalanceTime = 2.0f;
		ConfigureBalanceNM.GiveUpHeight = 0.62f;
		ConfigureBalanceNM.LegStiffness = 12.0f;
		ConfigureBalanceNM.MaxSteps = 3;
		ConfigureBalanceNM.LegsTogetherRestep = 1.0f;
		ConfigureBalanceNM.LegsApartRestep = 0.2f;
		ConfigureBalanceNM.ExtraFeetApart = 0f;
		ConfigureBalanceNM.StepDecisionThreshold = 0f;
		ConfigureBalanceNM.DontStepTime = 0.2f;
		ConfigureBalanceNM.Start();

		ShotHelper shotNM = ped.Euphoria.Shot;
		shotNM.InitialNeckStiffness = 5.0f;
		shotNM.InitialNeckDamping = 2.0f;
		shotNM.Looseness4Fall = 0.7f;
		shotNM.Looseness4Stagger = 0.4f;
		shotNM.MinArmsLooseness = 0.1f;
		shotNM.AlwaysResetNeckLooseness = true;
		shotNM.AngVelScale = 0f;
		shotNM.TimeBeforeReachForWound = 0.5f;
		shotNM.CpainSmooth2Time = 0f;
		shotNM.CpainMag = 0f;
		shotNM.CpainTwistMag = 0f;
		shotNM.CpainSmooth2Zero = 0f;
		shotNM.FallingReaction = 3;
		shotNM.InitialWeaknessZeroDuration = 0.1f;
		shotNM.InitialWeaknessRampDuration = 0.2f;
		shotNM.UseCStrModulation = false;
		shotNM.CStrUpperMin = 1.0f;
		shotNM.CStrLowerMin = 0.8f;
		shotNM.BodyStiffness = 10f;
		shotNM.SpineBlendExagCPain = true;
		shotNM.AlwaysResetLooseness = true;
		shotNM.Fling = true;
		shotNM.FlingWidth = 0.5f;
		shotNM.FlingTime = 0.05f;
		shotNM.Start();

		ShotSnapHelper shotSnapNM = ped.Euphoria.ShotSnap;
		shotSnapNM.Snap = true;
		shotSnapNM.SnapMag = 1f;
		shotSnapNM.SnapDirectionRandomness = 0f;
		shotSnapNM.SnapLeftLeg = false;
		shotSnapNM.SnapRightLeg = false;
		shotSnapNM.SnapNeck = true;
		shotSnapNM.UnSnapInterval = 0.05f;
		shotSnapNM.SnapMovingMult = 2f;
		shotSnapNM.SnapBalancingMult = 1f;
		shotSnapNM.SnapAirborneMult = 1.0f;
		shotSnapNM.SnapMovingThresh = 1.0f;
		shotSnapNM.SnapLeftArm = false;
		shotSnapNM.SnapRightArm = false;
		shotSnapNM.SnapSpine = true;
		shotSnapNM.SnapPhasedLegs = true;
		shotSnapNM.SnapHipType = 2;
		shotSnapNM.SnapUseBulletDir = true;
		shotSnapNM.SnapHitPart = false;
		shotSnapNM.UnSnapRatio = 0.3f;
		shotSnapNM.SnapUseTorques = true;
		shotSnapNM.Start();

		ShotShockSpinHelper shotShockSpinNM = ped.Euphoria.ShotShockSpin;
		shotShockSpinNM.AddShockSpin = false;
		shotShockSpinNM.AlwaysAddShockSpin = false;
		shotShockSpinNM.ShockSpinMin = 100f;
		shotShockSpinNM.ShockSpinMax = 100f;
		shotShockSpinNM.BracedSideSpinMult = 2f;
		shotShockSpinNM.Start();

		ConfigureBulletsHelper configureBulletsNM = ped.Euphoria.ConfigureBullets;
		configureBulletsNM.LoosenessFix = true;
		configureBulletsNM.ImpulseReductionPerShot = 0.1f;
		configureBulletsNM.ImpulseRecovery = 0f;
		configureBulletsNM.TorqueAlwaysSpine3 = true;
		configureBulletsNM.RbRatio = 0f;
		configureBulletsNM.RbMaxTwistMomentArm = 1f;
		configureBulletsNM.RbMaxBroomMomentArm = 0f;
		configureBulletsNM.RbRatioAirborne = 0f;
		configureBulletsNM.RbMaxTwistMomentArmAirborne = 1f;
		configureBulletsNM.RbMaxBroomMomentArmAirborne = 0f;
		configureBulletsNM.RbRatioOneLeg = 0f;
		configureBulletsNM.RbMaxTwistMomentArmOneLeg = 1f;
		configureBulletsNM.RbMaxBroomMomentArmOneLeg = 0f;
		configureBulletsNM.RbPivot = false;
		configureBulletsNM.ImpulseAirOn = true;
		configureBulletsNM.ImpulseAirMax = 150f;
		configureBulletsNM.ImpulseAirMult = 1.0f;
		configureBulletsNM.Start();

		ConfigureShotInjuredLegHelper configureShotInjuredLegNM = ped.Euphoria.ConfigureShotInjuredLeg;
		configureShotInjuredLegNM.TimeBeforeCollapseWoundLeg = 0f;
		configureShotInjuredLegNM.LegInjuryTime = 1.4f;
		configureShotInjuredLegNM.LegLimpBend = 0.2f;
		configureShotInjuredLegNM.LegInjury = 0f;
		configureShotInjuredLegNM.LegInjuryHipPitch = -0.2f;
		configureShotInjuredLegNM.LegInjuryHipPitch = -0.1f;
		configureShotInjuredLegNM.Start();

		ShotConfigureArmsHelper shotConfigureArmsNM = ped.Euphoria.ShotConfigureArms;
		shotConfigureArmsNM.Brace = false;
		shotConfigureArmsNM.UseArmsWindmill = false;
		shotConfigureArmsNM.ReleaseWound = 0;
		shotConfigureArmsNM.AlwaysReachTime = 0f;
		shotConfigureArmsNM.AWSpeedMult = 0f;
		shotConfigureArmsNM.AWRadiusMult = 0f;
		shotConfigureArmsNM.AWStiffnessAdd = 0f;
		shotConfigureArmsNM.Start();

		BodyBalanceHelper bodyBalanceNM = ped.Euphoria.BodyBalance;
		bodyBalanceNM.UseBodyTurn = true;
		bodyBalanceNM.SpineStiffness = 12f;
		bodyBalanceNM.Shoulder = 1f;
		bodyBalanceNM.Start();

		ShotInGutsHelper shotInGutsNM = ped.Euphoria.ShotInGuts;
		shotInGutsNM.ShotInGuts = true;
		shotInGutsNM.SigSpineAmount = 2f;
		shotInGutsNM.SigNeckAmount = 0f;
		shotInGutsNM.SigHipAmount = 1f;
		shotInGutsNM.SigKneeAmount = 0.1f;
		shotInGutsNM.SigPeriod = 0.2f;
		shotInGutsNM.SigForceBalancePeriod = 0f;
		shotInGutsNM.SigKneesOnset = 0f;
		shotInGutsNM.Update();

		StayUprightHelper stayUprightNM = ped.Euphoria.StayUpright;
		stayUprightNM.UseForces = true;
		stayUprightNM.UseTorques = true;
		stayUprightNM.LastStandMode = false;
		stayUprightNM.LastStandSinkRate = 0.3f;
		stayUprightNM.LastStandHorizDamping = 0.4f;
		stayUprightNM.LastStandMaxTime = 0.4f;
		stayUprightNM.TurnTowardsBullets = true;
		stayUprightNM.VelocityBased = true;
		stayUprightNM.TorqueOnlyInAir = false;
		stayUprightNM.LastStandHorizDamping = 3.3f;
		stayUprightNM.ForceDamping = -1.0f;
		stayUprightNM.ForceFeetMult = 1.0f;
		stayUprightNM.ForceSpine3Share = 0.3f;
		stayUprightNM.ForceLeanReduction = 1f;
		stayUprightNM.ForceInAirShare = 0.5f;
		stayUprightNM.ForceMin = -1f;
		stayUprightNM.ForceMax = -1f;
		stayUprightNM.ForceSaturationVel = 4.0f;
		stayUprightNM.ForceThresholdVel = 0.5f;
		stayUprightNM.TorqueStrength = 0f;
		stayUprightNM.TorqueDamping = 0.5f;
		stayUprightNM.TorqueSaturationVel = 4.0f;
		stayUprightNM.TorqueThresholdVel = 2.5f;
		stayUprightNM.SupportPosition = 2f;
		stayUprightNM.NoSupportForceMult = 1.0f;
		stayUprightNM.StepUpHelp = 0f;
		stayUprightNM.StayUpAcc = 0.7f;
		stayUprightNM.StayUpAccMax = 5.0f;
		stayUprightNM.Update();

		SmartFallHelper smartFallNM = ped.Euphoria.SmartFall;
		smartFallNM.SplatWhenStopped = 5.0f;
		smartFallNM.BlendHeadWhenStopped = 0.8f;
		smartFallNM.Update();

		SetFallingReactionHelper setFallingReactionNM = ped.Euphoria.SetFallingReaction;
		setFallingReactionNM.AntiPropClav = true;
		setFallingReactionNM.Start();
	}

	// Shot_Base (priority: 0)
	private static void StartBackShotNmMessages(Ped ped)
	{
		StayUprightHelper stayUprightNM = ped.Euphoria.StayUpright;
		stayUprightNM.UseForces = true;
		stayUprightNM.TurnTowardsBullets = false;
		stayUprightNM.ForceStrength = 0f;
		stayUprightNM.Start();

		ShotHelper shotNM = ped.Euphoria.Shot;
		shotNM.BodyStiffness = 7f;
		shotNM.Fling = false;
		shotNM.BodyStiffness = 1f;
		shotNM.Update();

		ConfigureBulletsExtraHelper configureBulletsExtraNM = ped.Euphoria.ConfigureBulletsExtra;
		configureBulletsExtraNM.RbRatio = 0f;
		configureBulletsExtraNM.LiftGain = 1f;
		configureBulletsExtraNM.Stop();

		ApplyBulletImpulseHelper applyBulletImpulseNM = ped.Euphoria.ApplyBulletImpulse;
		applyBulletImpulseNM.EqualizeAmount = 0f;
		applyBulletImpulseNM.PartIndex = 0;
		applyBulletImpulseNM.Impulse = Vector3.Zero;
		applyBulletImpulseNM.HitPoint = Vector3.Zero;
		applyBulletImpulseNM.LocalHitPointInfo = false;
		applyBulletImpulseNM.ExtraShare = -2f;
		applyBulletImpulseNM.Stop();

		ShotFromBehindHelper shotFromBehindNM = ped.Euphoria.ShotFromBehind;
		shotFromBehindNM.Stop();

		ConfigureBalanceHelper configureBalanceNM = ped.Euphoria.ConfigureBalance;
		configureBalanceNM.TaperKneeStrength = true;
		configureBalanceNM.LegStiffness = 0f;
		configureBalanceNM.GiveUpHeight = 1f;
		configureBalanceNM.MaxSteps = 1;
		configureBalanceNM.MaxBalanceTime = 1f;
		configureBalanceNM.ExtraSteps = -1;
		configureBalanceNM.FallType = 0;
		configureBalanceNM.FallMult = 100f;
		configureBalanceNM.RampHipPitchOnFail = true;
		configureBalanceNM.StableLinSpeedThresh = 0.01f;
		configureBalanceNM.StableRotSpeedThresh = 0.01f;
		configureBalanceNM.UseComDirTurnVelThresh = 0.1f;
		configureBalanceNM.BalanceAbortThreshold = 0.1f;
		configureBalanceNM.Start();

		ShotConfigureArmsHelper shotConfigureArmsNM = ped.Euphoria.ShotConfigureArms;
		shotConfigureArmsNM.Fling2 = true;
		shotConfigureArmsNM.PointGun = true;
		shotConfigureArmsNM.Start();

		ShotSnapHelper shotSnapNM = ped.Euphoria.ShotSnap;
		shotSnapNM.SnapHitPart = false;
		shotSnapNM.SnapMag = 1.5f;
		shotSnapNM.Update();
	}

	// Shot_Electrocute (priority: 15)
	private static void StartShotElectrocuteMessages(Ped ped)
	{
		ElectrocuteHelper elecNM = ped.Euphoria.Electrocute;
		elecNM.StunMag = 0.2f;
		elecNM.ApplyStiffness = false;
		elecNM.UseTorques = true;
		elecNM.HipType = 1;
		elecNM.InitialMult = 2f;
		elecNM.LargeMult = 1.5f;
		elecNM.LargeMinTime = 1f;
		elecNM.LargeMaxTime = 2f;
		elecNM.MovingMult = 3.92f;
		elecNM.BalancingMult = 3f;
		elecNM.MovingThresh = 1f;
		elecNM.LeftLeg = true;
		elecNM.RightLeg = true;
		elecNM.Start();

		ForceLeanRandomHelper forceLeanRandomNM = ped.Euphoria.ForceLeanRandom;
		forceLeanRandomNM.LeanAmountMin = 0.1f;
		forceLeanRandomNM.LeanAmountMax = 0.1f;
		forceLeanRandomNM.BodyPart = 10;
		forceLeanRandomNM.Start();

		ShotFallToKneesHelper ShotFallToKneesNM = ped.Euphoria.ShotFallToKnees;
		ShotFallToKneesNM.FallToKnees = true;
		ShotFallToKneesNM.FtkAlwaysChangeFall = true;
		ShotFallToKneesNM.FtkBalanceTime = 1f;
		ShotFallToKneesNM.FtkHelperForce = 100f;
		ShotFallToKneesNM.Start();

		StaggerFallHelper staggerFallNM = ped.Euphoria.StaggerFall;
		staggerFallNM.UpperBodyReaction = false;
		staggerFallNM.Start();

		ShotSnapHelper ShotSnapNM = ped.Euphoria.ShotSnap;
		ShotSnapNM.ResetArguments();

		SetFallingReactionHelper setFallingReactionNM = ped.Euphoria.SetFallingReaction;
		setFallingReactionNM.HandsAndKnees = false;
		setFallingReactionNM.CallRDS = false;
		setFallingReactionNM.Update();

		PointGunHelper pointGunNM = ped.Euphoria.PointGun;
		pointGunNM.ResetArguments();
		pointGunNM.EnableLeft = false;
		pointGunNM.EnableRight = false;
		pointGunNM.Start();

		ConfigureBalanceHelper ConfigureBalanceNM = ped.Euphoria.ConfigureBalance;
		ConfigureBalanceNM.ResetArguments();
		ConfigureBalanceNM.FootFriction = 0.5f;
		ConfigureBalanceNM.Start();
	}

	// Shot_LegShot (priority: 21)
	private static void StartLegShot21Messages(Ped ped)
	{
		ConfigureBulletsHelper configureBulletsNM = ped.Euphoria.ConfigureBullets;
		configureBulletsNM.RbLowerShare = 0.5f;
		configureBulletsNM.Update();

		ConfigureShotInjuredLegHelper configureShotInjuredLegNM = ped.Euphoria.ConfigureShotInjuredLeg;
		configureShotInjuredLegNM.TimeBeforeCollapseWoundLeg = 0.5f;
		configureShotInjuredLegNM.LegInjuryTime = 0.5f;
		configureShotInjuredLegNM.LegLimpBend = 0.21f;
		configureShotInjuredLegNM.LegLiftTime = 0.7f;
		configureShotInjuredLegNM.LegInjury = 0.5f;
		configureShotInjuredLegNM.LegInjuryHipPitch = 1f;
		configureShotInjuredLegNM.LegInjuryLiftHipPitch = 1f;
		configureShotInjuredLegNM.LegInjurySpineBend = 1f;
		configureShotInjuredLegNM.LegInjuryLiftSpineBend = 1f;
		configureShotInjuredLegNM.Start();

		ShotConfigureArmsHelper shotConfigureArmsNM = ped.Euphoria.ShotConfigureArms;
		shotConfigureArmsNM.Brace = false;
		shotConfigureArmsNM.PointGun = true;
		shotConfigureArmsNM.UseArmsWindmill = true;
		shotConfigureArmsNM.ReleaseWound = 0;
		shotConfigureArmsNM.AlwaysReachTime = 10f;
		shotConfigureArmsNM.AWSpeedMult = 1f;
		shotConfigureArmsNM.AWRadiusMult = 1f;
		shotConfigureArmsNM.AWStiffnessAdd = 4f;
		shotConfigureArmsNM.ReachWithOneHand = 2;
		shotConfigureArmsNM.AllowLeftPistolRFW = true;
		shotConfigureArmsNM.AllowRightPistolRFW = false;
		shotConfigureArmsNM.RfwWithPistol = false;
		shotConfigureArmsNM.Update();

		ShotHelper shotNM = ped.Euphoria.Shot;
		shotNM.AllowInjuredLeg = true;
		shotNM.AllowInjuredLowerLegReach = true;
		shotNM.AllowInjuredThighReach = true;
		shotNM.BodyStiffness = 14f;
		shotNM.SpineDamping = 1f;
		shotNM.ArmStiffness = 10f;
		shotNM.InitialNeckStiffness = 14f;
		shotNM.InitialNeckDamping = 1f;
		shotNM.NeckStiffness = 16f;
		shotNM.NeckDamping = 2f;
		shotNM.KMultOnLoose = 0f;
		shotNM.KMult4Legs = 0.3f;
		shotNM.LoosenessAmount = 1f;
		shotNM.Looseness4Fall = 0f;
		shotNM.Looseness4Stagger = 0f;
		shotNM.MinArmsLooseness = 0.1f;
		shotNM.MinLegsLooseness = 0.1f;
		shotNM.GrabHoldTime = 2f;
		shotNM.SpineBlendExagCPain = false;
		shotNM.SpineBlendZero = 0.6f;
		shotNM.BulletProofVest = false;
		shotNM.AlwaysResetLooseness = true;
		shotNM.AlwaysResetNeckLooseness = true;
		shotNM.AngVelScale = 1f;
		shotNM.AngVelScaleMask = "fb";
		shotNM.FlingWidth = 0.5f;
		shotNM.TimeBeforeReachForWound = 0f;
		shotNM.ExagDuration = 0f;
		shotNM.ExagMag = 1f;
		shotNM.ExagTwistMag = 0.5f;
		shotNM.ExagSmooth2Zero = 0f;
		shotNM.ExagZeroTime = 0f;
		shotNM.CpainSmooth2Time = 0.2f;
		shotNM.CpainDuration = 0f;
		shotNM.CpainMag = 1f;
		shotNM.CpainTwistMag = 0.5f;
		shotNM.CpainSmooth2Zero = 1.5f;
		shotNM.Crouching = false;
		shotNM.ChickenArms = true;
		shotNM.ReachForWound = true;
		shotNM.Fling = true;
		shotNM.AllowInjuredArm = false;
		shotNM.StableHandsAndNeck = false;
		shotNM.Melee = false;
		shotNM.FallingReaction = 0;
		shotNM.UseExtendedCatchFall = false;
		shotNM.InitialWeaknessZeroDuration = 0f;
		shotNM.InitialWeaknessRampDuration = 0.4f;
		shotNM.InitialNeckDuration = 0f;
		shotNM.InitialNeckRampDuration = 0.4f;
		shotNM.UseCStrModulation = false;
		shotNM.CStrUpperMin = 0.1f;
		shotNM.CStrUpperMax = 1f;
		shotNM.CStrLowerMin = 0.1f;
		shotNM.CStrLowerMax = 1f;
		shotNM.DeathTime = -1f;
		shotNM.Update();

		StayUprightHelper stayUprightNM = ped.Euphoria.StayUpright;
		stayUprightNM.Stop();

		ConfigureBalanceHelper configureBalanceNM = ped.Euphoria.ConfigureBalance;
		configureBalanceNM.StableLinSpeedThresh = 0.35f;
		configureBalanceNM.StableRotSpeedThresh = 0.4f;
		configureBalanceNM.BalanceAbortThreshold = 0.6f;
		configureBalanceNM.Update();

		ShotFallToKneesHelper shotFallToKneesNM = ped.Euphoria.ShotFallToKnees;
		shotFallToKneesNM.Start();

		ShotSnapHelper shotSnapNM = ped.Euphoria.ShotSnap;
		shotSnapNM.Snap = true;
		shotSnapNM.SnapMag = 1f;
		shotSnapNM.SnapMovingMult = 1f;
		shotSnapNM.SnapBalancingMult = 1f;
		shotSnapNM.SnapAirborneMult = 1f;
		shotSnapNM.SnapMovingThresh = 1f;
		shotSnapNM.SnapDirectionRandomness = 0.3f;
		shotSnapNM.SnapLeftArm = false;
		shotSnapNM.SnapRightArm = false;
		shotSnapNM.SnapLeftLeg = true;
		shotSnapNM.SnapLeftLeg = true;
		shotSnapNM.SnapSpine = true;
		shotSnapNM.SnapNeck = false;
		shotSnapNM.SnapPhasedLegs = true;
		shotSnapNM.SnapHipType = 0;
		shotSnapNM.SnapUseBulletDir = true;
		shotSnapNM.SnapHitPart = false;
		shotSnapNM.UnSnapInterval = 0.1f;
		shotSnapNM.UnSnapRatio = 0.7f;
		shotSnapNM.SnapUseTorques = true;
		shotSnapNM.Update();

		HeadLookHelper headLookNM = ped.Euphoria.HeadLook;
		headLookNM.AlwaysLook = true;
		headLookNM.KeepHeadAwayFromGround = true;
		headLookNM.Update();

		ShotHeadLookHelper shotHeadLookNM = ped.Euphoria.ShotHeadLook;
		shotHeadLookNM.UseHeadLook = true;
		shotHeadLookNM.HeadLook = Vector3.Zero;
		shotHeadLookNM.HeadLookAtWoundMinTimer = 0f;
		shotHeadLookNM.HeadLookAtWoundMaxTimer = 10f;
		shotHeadLookNM.HeadLookAtHeadPosMaxTimer = 10f;
		shotHeadLookNM.HeadLookAtHeadPosMinTimer = 10f;
		shotHeadLookNM.Update();

		StaggerFallHelper staggerFallNM = ped.Euphoria.StaggerFall;
		staggerFallNM.Stop();
	}

	private void FakeElectrocutionShotDemo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
	{
		Ped playerPed = Game.LocalPlayerPed;

		if (e.KeyCode == System.Windows.Forms.Keys.J)
		{
			// Switch the player ped to a ragdoll so they can recieve NaturalMotion messages.
			// This method starts a new ragdoll task.
			 playerPed.Ragdoll(5000, RagdollType.ScriptControl);

			// Send the player ped message so they will move like when a stun gun bullet hits them in the chest
			playerPed.ApplyRelativeForceRelativeOffset(new Vector3(0f, -1.5f, 0f), Vector3.Zero, ForceType.ExternalImpulse, RagdollComponent.Spine3, true, false, false);
			StartShotBaseNmMessages(playerPed);
			StartNormalWeapomNmMessages(playerPed);
			StartShotElectrocuteMessages(playerPed);
		}
		else if (e.KeyCode == System.Windows.Forms.Keys.K)
		{
			// Send the player ped message so they will move like when a stun gun bullet hits them in the left leg, but never start a new ragdoll task
			playerPed.ApplyRelativeForceRelativeOffset(new Vector3(0f, -1.5f, 0f), Vector3.Zero, ForceType.ExternalImpulse, RagdollComponent.ShinRight, true, false, false);
			StartLegShot21Messages(playerPed);
		}
	}
}
