using System;
using GTA.Math;
using GTA;

namespace GTA.NaturalMotion
{
	public enum ArmDirections
	{
		Backwards = -1,
		Adaptive = 0,
		Forwards = 1
	}

	public sealed class ActivePoseHelper : CustomHelper
	{
		public ActivePoseHelper(Ped ped) : base(ped, "activePose")
		{
		}

		public string Mask
		{
			set { SetArgument("mask", value); }
		}

		public bool UseGravityCompensation
		{
			set { SetArgument("useGravityCompensation", value); }
		}

		public int AnimSource
		{
			set { SetArgument("animSource", value); }
		}
	}

	public sealed class ApplyImpulseHelper : CustomHelper
	{
		public ApplyImpulseHelper(Ped ped) : base(ped, "applyImpulse")
		{
		}

		public float EqualizeAmount
		{
			set { SetArgument("equalizeAmount", value); }
		}

		public int PartIndex
		{
			set { SetArgument("partIndex", value); }
		}

		public Vector3 Impulse
		{
			set { SetArgument("impulse", value); }
		}

		public Vector3 HitPoint
		{
			set { SetArgument("hitPoint", value); }
		}

		public bool LocalHitPointInfo
		{
			set { SetArgument("localHitPointInfo", value); }
		}

		public bool LocalImpulseInfo
		{
			set { SetArgument("localImpulseInfo", value); }
		}

		public bool AngularImpulse
		{
			set { SetArgument("angularImpulse", value); }
		}
	}

	public sealed class ApplyBulletImpulseHelper : CustomHelper
	{
		public ApplyBulletImpulseHelper(Ped ped) : base(ped, "applyBulletImpulse")
		{
		}

		public float EqualizeAmount
		{
			set { SetArgument("equalizeAmount", value); }
		}

		public int PartIndex
		{
			set { SetArgument("partIndex", value); }
		}

		public Vector3 Impulse
		{
			set { SetArgument("impulse", value); }
		}

		public Vector3 HitPoint
		{
			set { SetArgument("hitPoint", value); }
		}

		public bool LocalHitPointInfo
		{
			set { SetArgument("localHitPointInfo", value); }
		}

		public float ExtraShare
		{
			set { SetArgument("extraShare", value); }
		}
	}

	public sealed class BodyRelaxHelper : CustomHelper
	{
		public BodyRelaxHelper(Ped ped) : base(ped, "bodyRelax")
		{
		}

		public float Relaxation
		{
			set { SetArgument("relaxation", value); }
		}

		public float Damping
		{
			set { SetArgument("damping", value); }
		}

		public string Mask
		{
			set { SetArgument("mask", value); }
		}

		public bool HoldPose
		{
			set { SetArgument("holdPose", value); }
		}

		public bool DisableJointDriving
		{
			set { SetArgument("disableJointDriving", value); }
		}
	}

	public sealed class ConfigureBalanceHelper : CustomHelper
	{
		public ConfigureBalanceHelper(Ped ped) : base(ped, "configureBalance")
		{
		}

		public float StepHeight
		{
			set { SetArgument("stepHeight", value); }
		}

		public float StepHeightInc4Step
		{
			set { SetArgument("stepHeightInc4Step", value); }
		}

		public float LegsApartRestep
		{
			set { SetArgument("legsApartRestep", value); }
		}

		public float LegsTogetherRestep
		{
			set { SetArgument("legsTogetherRestep", value); }
		}

		public float LegsApartMax
		{
			set { SetArgument("legsApartMax", value); }
		}

		public bool TaperKneeStrength
		{
			set { SetArgument("taperKneeStrength", value); }
		}

		public float LegStiffness
		{
			set { SetArgument("legStiffness", value); }
		}

		public float LeftLegSwingDamping
		{
			set { SetArgument("leftLegSwingDamping", value); }
		}

		public float RightLegSwingDamping
		{
			set { SetArgument("rightLegSwingDamping", value); }
		}

		public float OpposeGravityLegs
		{
			set { SetArgument("opposeGravityLegs", value); }
		}

		public float OpposeGravityAnkles
		{
			set { SetArgument("opposeGravityAnkles", value); }
		}

		public float LeanAcc
		{
			set { SetArgument("leanAcc", value); }
		}

		public float HipLeanAcc
		{
			set { SetArgument("hipLeanAcc", value); }
		}

		public float LeanAccMax
		{
			set { SetArgument("leanAccMax", value); }
		}

		public float ResistAcc
		{
			set { SetArgument("resistAcc", value); }
		}

		public float ResistAccMax
		{
			set { SetArgument("resistAccMax", value); }
		}

		public bool FootSlipCompOnMovingFloor
		{
			set { SetArgument("footSlipCompOnMovingFloor", value); }
		}

		public float AnkleEquilibrium
		{
			set { SetArgument("ankleEquilibrium", value); }
		}

		public float ExtraFeetApart
		{
			set { SetArgument("extraFeetApart", value); }
		}

		public float DontStepTime
		{
			set { SetArgument("dontStepTime", value); }
		}

		public float BalanceAbortThreshold
		{
			set { SetArgument("balanceAbortThreshold", value); }
		}

		public float GiveUpHeight
		{
			set { SetArgument("giveUpHeight", value); }
		}

		public float StepClampScale
		{
			set { SetArgument("stepClampScale", value); }
		}

		public float StepClampScaleVariance
		{
			set { SetArgument("stepClampScaleVariance", value); }
		}

		public float PredictionTimeHip
		{
			set { SetArgument("predictionTimeHip", value); }
		}

		public float PredictionTime
		{
			set { SetArgument("predictionTime", value); }
		}

		public float PredictionTimeVariance
		{
			set { SetArgument("predictionTimeVariance", value); }
		}

		public int MaxSteps
		{
			set { SetArgument("maxSteps", value); }
		}

		public float MaxBalanceTime
		{
			set { SetArgument("maxBalanceTime", value); }
		}

		public int ExtraSteps
		{
			set { SetArgument("extraSteps", value); }
		}

		public float ExtraTime
		{
			set { SetArgument("extraTime", value); }
		}

		public int FallType
		{
			set { SetArgument("fallType", value); }
		}

		public float FallMult
		{
			set { SetArgument("fallMult", value); }
		}

		public bool FallReduceGravityComp
		{
			set { SetArgument("fallReduceGravityComp", value); }
		}

		public bool RampHipPitchOnFail
		{
			set { SetArgument("rampHipPitchOnFail", value); }
		}

		public float StableLinSpeedThresh
		{
			set { SetArgument("stableLinSpeedThresh", value); }
		}

		public float StableRotSpeedThresh
		{
			set { SetArgument("stableRotSpeedThresh", value); }
		}

		public bool FailMustCollide
		{
			set { SetArgument("failMustCollide", value); }
		}

		public bool IgnoreFailure
		{
			set { SetArgument("ignoreFailure", value); }
		}

		public float ChangeStepTime
		{
			set { SetArgument("changeStepTime", value); }
		}

		public bool BalanceIndefinitely
		{
			set { SetArgument("balanceIndefinitely", value); }
		}

		public bool MovingFloor
		{
			set { SetArgument("movingFloor", value); }
		}

		public bool AirborneStep
		{
			set { SetArgument("airborneStep", value); }
		}

		public float UseComDirTurnVelThresh
		{
			set { SetArgument("useComDirTurnVelThresh", value); }
		}

		public float MinKneeAngle
		{
			set { SetArgument("minKneeAngle", value); }
		}

		public bool FlatterSwingFeet
		{
			set { SetArgument("flatterSwingFeet", value); }
		}

		public bool FlatterStaticFeet
		{
			set { SetArgument("flatterStaticFeet", value); }
		}

		public bool AvoidLeg
		{
			set { SetArgument("avoidLeg", value); }
		}

		public float AvoidFootWidth
		{
			set { SetArgument("avoidFootWidth", value); }
		}

		public float AvoidFeedback
		{
			set { SetArgument("avoidFeedback", value); }
		}

		public float LeanAgainstVelocity
		{
			set { SetArgument("leanAgainstVelocity", value); }
		}

		public float StepDecisionThreshold
		{
			set { SetArgument("stepDecisionThreshold", value); }
		}

		public bool StepIfInSupport
		{
			set { SetArgument("stepIfInSupport", value); }
		}

		public bool AlwaysStepWithFarthest
		{
			set { SetArgument("alwaysStepWithFarthest", value); }
		}

		public bool StandUp
		{
			set { SetArgument("standUp", value); }
		}

		public float DepthFudge
		{
			set { SetArgument("depthFudge", value); }
		}

		public float DepthFudgeStagger
		{
			set { SetArgument("depthFudgeStagger", value); }
		}

		public float FootFriction
		{
			set { SetArgument("footFriction", value); }
		}

		public float FootFrictionStagger
		{
			set { SetArgument("footFrictionStagger", value); }
		}

		public float BackwardsLeanCutoff
		{
			set { SetArgument("backwardsLeanCutoff", value); }
		}

		public float GiveUpHeightEnd
		{
			set { SetArgument("giveUpHeightEnd", value); }
		}

		public float BalanceAbortThresholdEnd
		{
			set { SetArgument("balanceAbortThresholdEnd", value); }
		}

		public float GiveUpRampDuration
		{
			set { SetArgument("giveUpRampDuration", value); }
		}

		public float LeanToAbort
		{
			set { SetArgument("leanToAbort", value); }
		}
	}

	public sealed class ConfigureBalanceResetHelper : CustomHelper
	{
		public ConfigureBalanceResetHelper(Ped ped) : base(ped, "configureBalanceReset")
		{
		}
	}

	public sealed class ConfigureSelfAvoidanceHelper : CustomHelper
	{
		public ConfigureSelfAvoidanceHelper(Ped ped) : base(ped, "configureSelfAvoidance")
		{
		}

		public bool UseSelfAvoidance
		{
			set { SetArgument("useSelfAvoidance", value); }
		}

		public bool OverwriteDragReduction
		{
			set { SetArgument("overwriteDragReduction", value); }
		}

		public float TorsoSwingFraction
		{
			set { SetArgument("torsoSwingFraction", value); }
		}

		public float MaxTorsoSwingAngleRad
		{
			set { SetArgument("maxTorsoSwingAngleRad", value); }
		}

		public bool SelfAvoidIfInSpineBoundsOnly
		{
			set { SetArgument("selfAvoidIfInSpineBoundsOnly", value); }
		}

		public float SelfAvoidAmount
		{
			set { SetArgument("selfAvoidAmount", value); }
		}

		public bool OverwriteTwist
		{
			set { SetArgument("overwriteTwist", value); }
		}

		public bool UsePolarPathAlgorithm
		{
			set { SetArgument("usePolarPathAlgorithm", value); }
		}

		public float Radius
		{
			set { SetArgument("radius", value); }
		}
	}

	public sealed class ConfigureBulletsHelper : CustomHelper
	{
		public ConfigureBulletsHelper(Ped ped) : base(ped, "configureBullets")
		{
		}

		public bool ImpulseSpreadOverParts
		{
			set { SetArgument("impulseSpreadOverParts", value); }
		}

		public bool ImpulseLeakageStrengthScaled
		{
			set { SetArgument("impulseLeakageStrengthScaled", value); }
		}

		public float ImpulsePeriod
		{
			set { SetArgument("impulsePeriod", value); }
		}

		public float ImpulseTorqueScale
		{
			set { SetArgument("impulseTorqueScale", value); }
		}

		public bool LoosenessFix
		{
			set { SetArgument("loosenessFix", value); }
		}

		public float ImpulseDelay
		{
			set { SetArgument("impulseDelay", value); }
		}

		public float ImpulseReductionPerShot
		{
			set { SetArgument("impulseReductionPerShot", value); }
		}

		public float ImpulseRecovery
		{
			set { SetArgument("impulseRecovery", value); }
		}

		public float ImpulseMinLeakage
		{
			set { SetArgument("impulseMinLeakage", value); }
		}

		public int TorqueMode
		{
			set { SetArgument("torqueMode", value); }
		}

		public int TorqueSpinMode
		{
			set { SetArgument("torqueSpinMode", value); }
		}

		public int TorqueFilterMode
		{
			set { SetArgument("torqueFilterMode", value); }
		}

		public bool TorqueAlwaysSpine3
		{
			set { SetArgument("torqueAlwaysSpine3", value); }
		}

		public float TorqueDelay
		{
			set { SetArgument("torqueDelay", value); }
		}

		public float TorquePeriod
		{
			set { SetArgument("torquePeriod", value); }
		}

		public float TorqueGain
		{
			set { SetArgument("torqueGain", value); }
		}

		public float TorqueCutoff
		{
			set { SetArgument("torqueCutoff", value); }
		}

		public float TorqueReductionPerTick
		{
			set { SetArgument("torqueReductionPerTick", value); }
		}

		public float LiftGain
		{
			set { SetArgument("liftGain", value); }
		}

		public float CounterImpulseDelay
		{
			set { SetArgument("counterImpulseDelay", value); }
		}

		public float CounterImpulseMag
		{
			set { SetArgument("counterImpulseMag", value); }
		}

		public bool CounterAfterMagReached
		{
			set { SetArgument("counterAfterMagReached", value); }
		}

		public bool DoCounterImpulse
		{
			set { SetArgument("doCounterImpulse", value); }
		}

		public float CounterImpulse2Hips
		{
			set { SetArgument("counterImpulse2Hips", value); }
		}

		public float ImpulseNoBalMult
		{
			set { SetArgument("impulseNoBalMult", value); }
		}

		public float ImpulseBalStabStart
		{
			set { SetArgument("impulseBalStabStart", value); }
		}

		public float ImpulseBalStabEnd
		{
			set { SetArgument("impulseBalStabEnd", value); }
		}

		public float ImpulseBalStabMult
		{
			set { SetArgument("impulseBalStabMult", value); }
		}

		public float ImpulseSpineAngStart
		{
			set { SetArgument("impulseSpineAngStart", value); }
		}

		public float ImpulseSpineAngEnd
		{
			set { SetArgument("impulseSpineAngEnd", value); }
		}

		public float ImpulseSpineAngMult
		{
			set { SetArgument("impulseSpineAngMult", value); }
		}

		public float ImpulseVelStart
		{
			set { SetArgument("impulseVelStart", value); }
		}

		public float ImpulseVelEnd
		{
			set { SetArgument("impulseVelEnd", value); }
		}

		public float ImpulseVelMult
		{
			set { SetArgument("impulseVelMult", value); }
		}

		public float ImpulseAirMult
		{
			set { SetArgument("impulseAirMult", value); }
		}

		public float ImpulseAirMultStart
		{
			set { SetArgument("impulseAirMultStart", value); }
		}

		public float ImpulseAirMax
		{
			set { SetArgument("impulseAirMax", value); }
		}

		public float ImpulseAirApplyAbove
		{
			set { SetArgument("impulseAirApplyAbove", value); }
		}

		public bool ImpulseAirOn
		{
			set { SetArgument("impulseAirOn", value); }
		}

		public float ImpulseOneLegMult
		{
			set { SetArgument("impulseOneLegMult", value); }
		}

		public float ImpulseOneLegMultStart
		{
			set { SetArgument("impulseOneLegMultStart", value); }
		}

		public float ImpulseOneLegMax
		{
			set { SetArgument("impulseOneLegMax", value); }
		}

		public float ImpulseOneLegApplyAbove
		{
			set { SetArgument("impulseOneLegApplyAbove", value); }
		}

		public bool ImpulseOneLegOn
		{
			set { SetArgument("impulseOneLegOn", value); }
		}

		public float RbRatio
		{
			set { SetArgument("rbRatio", value); }
		}

		public float RbLowerShare
		{
			set { SetArgument("rbLowerShare", value); }
		}

		public float RbMoment
		{
			set { SetArgument("rbMoment", value); }
		}

		public float RbMaxTwistMomentArm
		{
			set { SetArgument("rbMaxTwistMomentArm", value); }
		}

		public float RbMaxBroomMomentArm
		{
			set { SetArgument("rbMaxBroomMomentArm", value); }
		}

		public float RbRatioAirborne
		{
			set { SetArgument("rbRatioAirborne", value); }
		}

		public float RbMomentAirborne
		{
			set { SetArgument("rbMomentAirborne", value); }
		}

		public float RbMaxTwistMomentArmAirborne
		{
			set { SetArgument("rbMaxTwistMomentArmAirborne", value); }
		}

		public float RbMaxBroomMomentArmAirborne
		{
			set { SetArgument("rbMaxBroomMomentArmAirborne", value); }
		}

		public float RbRatioOneLeg
		{
			set { SetArgument("rbRatioOneLeg", value); }
		}

		public float RbMomentOneLeg
		{
			set { SetArgument("rbMomentOneLeg", value); }
		}

		public float RbMaxTwistMomentArmOneLeg
		{
			set { SetArgument("rbMaxTwistMomentArmOneLeg", value); }
		}

		public float RbMaxBroomMomentArmOneLeg
		{
			set { SetArgument("rbMaxBroomMomentArmOneLeg", value); }
		}

		public int RbTwistAxis
		{
			set { SetArgument("rbTwistAxis", value); }
		}

		public bool RbPivot
		{
			set { SetArgument("rbPivot", value); }
		}
	}

	public sealed class ConfigureBulletsExtraHelper : CustomHelper
	{
		public ConfigureBulletsExtraHelper(Ped ped) : base(ped, "configureBulletsExtra")
		{
		}

		public bool ImpulseSpreadOverParts
		{
			set { SetArgument("impulseSpreadOverParts", value); }
		}

		public float ImpulsePeriod
		{
			set { SetArgument("impulsePeriod", value); }
		}

		public float ImpulseTorqueScale
		{
			set { SetArgument("impulseTorqueScale", value); }
		}

		public bool LoosenessFix
		{
			set { SetArgument("loosenessFix", value); }
		}

		public float ImpulseDelay
		{
			set { SetArgument("impulseDelay", value); }
		}

		public int TorqueMode
		{
			set { SetArgument("torqueMode", value); }
		}

		public int TorqueSpinMode
		{
			set { SetArgument("torqueSpinMode", value); }
		}

		public int TorqueFilterMode
		{
			set { SetArgument("torqueFilterMode", value); }
		}

		public bool TorqueAlwaysSpine3
		{
			set { SetArgument("torqueAlwaysSpine3", value); }
		}

		public float TorqueDelay
		{
			set { SetArgument("torqueDelay", value); }
		}

		public float TorquePeriod
		{
			set { SetArgument("torquePeriod", value); }
		}

		public float TorqueGain
		{
			set { SetArgument("torqueGain", value); }
		}

		public float TorqueCutoff
		{
			set { SetArgument("torqueCutoff", value); }
		}

		public float TorqueReductionPerTick
		{
			set { SetArgument("torqueReductionPerTick", value); }
		}

		public float LiftGain
		{
			set { SetArgument("liftGain", value); }
		}

		public float CounterImpulseDelay
		{
			set { SetArgument("counterImpulseDelay", value); }
		}

		public float CounterImpulseMag
		{
			set { SetArgument("counterImpulseMag", value); }
		}

		public bool CounterAfterMagReached
		{
			set { SetArgument("counterAfterMagReached", value); }
		}

		public bool DoCounterImpulse
		{
			set { SetArgument("doCounterImpulse", value); }
		}

		public float CounterImpulse2Hips
		{
			set { SetArgument("counterImpulse2Hips", value); }
		}

		public float ImpulseNoBalMult
		{
			set { SetArgument("impulseNoBalMult", value); }
		}

		public float ImpulseBalStabStart
		{
			set { SetArgument("impulseBalStabStart", value); }
		}

		public float ImpulseBalStabEnd
		{
			set { SetArgument("impulseBalStabEnd", value); }
		}

		public float ImpulseBalStabMult
		{
			set { SetArgument("impulseBalStabMult", value); }
		}

		public float ImpulseSpineAngStart
		{
			set { SetArgument("impulseSpineAngStart", value); }
		}

		public float ImpulseSpineAngEnd
		{
			set { SetArgument("impulseSpineAngEnd", value); }
		}

		public float ImpulseSpineAngMult
		{
			set { SetArgument("impulseSpineAngMult", value); }
		}

		public float ImpulseVelStart
		{
			set { SetArgument("impulseVelStart", value); }
		}

		public float ImpulseVelEnd
		{
			set { SetArgument("impulseVelEnd", value); }
		}

		public float ImpulseVelMult
		{
			set { SetArgument("impulseVelMult", value); }
		}

		public float ImpulseAirMult
		{
			set { SetArgument("impulseAirMult", value); }
		}

		public float ImpulseAirMultStart
		{
			set { SetArgument("impulseAirMultStart", value); }
		}

		public float ImpulseAirMax
		{
			set { SetArgument("impulseAirMax", value); }
		}

		public float ImpulseAirApplyAbove
		{
			set { SetArgument("impulseAirApplyAbove", value); }
		}

		public bool ImpulseAirOn
		{
			set { SetArgument("impulseAirOn", value); }
		}

		public float ImpulseOneLegMult
		{
			set { SetArgument("impulseOneLegMult", value); }
		}

		public float ImpulseOneLegMultStart
		{
			set { SetArgument("impulseOneLegMultStart", value); }
		}

		public float ImpulseOneLegMax
		{
			set { SetArgument("impulseOneLegMax", value); }
		}

		public float ImpulseOneLegApplyAbove
		{
			set { SetArgument("impulseOneLegApplyAbove", value); }
		}

		public bool ImpulseOneLegOn
		{
			set { SetArgument("impulseOneLegOn", value); }
		}

		public float RbRatio
		{
			set { SetArgument("rbRatio", value); }
		}

		public float RbLowerShare
		{
			set { SetArgument("rbLowerShare", value); }
		}

		public float RbMoment
		{
			set { SetArgument("rbMoment", value); }
		}

		public float RbMaxTwistMomentArm
		{
			set { SetArgument("rbMaxTwistMomentArm", value); }
		}

		public float RbMaxBroomMomentArm
		{
			set { SetArgument("rbMaxBroomMomentArm", value); }
		}

		public float RbRatioAirborne
		{
			set { SetArgument("rbRatioAirborne", value); }
		}

		public float RbMomentAirborne
		{
			set { SetArgument("rbMomentAirborne", value); }
		}

		public float RbMaxTwistMomentArmAirborne
		{
			set { SetArgument("rbMaxTwistMomentArmAirborne", value); }
		}

		public float RbMaxBroomMomentArmAirborne
		{
			set { SetArgument("rbMaxBroomMomentArmAirborne", value); }
		}

		public float RbRatioOneLeg
		{
			set { SetArgument("rbRatioOneLeg", value); }
		}

		public float RbMomentOneLeg
		{
			set { SetArgument("rbMomentOneLeg", value); }
		}

		public float RbMaxTwistMomentArmOneLeg
		{
			set { SetArgument("rbMaxTwistMomentArmOneLeg", value); }
		}

		public float RbMaxBroomMomentArmOneLeg
		{
			set { SetArgument("rbMaxBroomMomentArmOneLeg", value); }
		}

		public int RbTwistAxis
		{
			set { SetArgument("rbTwistAxis", value); }
		}

		public bool RbPivot
		{
			set { SetArgument("rbPivot", value); }
		}
	}

	public sealed class ConfigureLimitsHelper : CustomHelper
	{
		public ConfigureLimitsHelper(Ped ped) : base(ped, "configureLimits")
		{
		}

		public string Mask
		{
			set { SetArgument("mask", value); }
		}

		public bool Enable
		{
			set { SetArgument("enable", value); }
		}

		public bool ToDesired
		{
			set { SetArgument("toDesired", value); }
		}

		public bool Restore
		{
			set { SetArgument("restore", value); }
		}

		public bool ToCurAnimation
		{
			set { SetArgument("toCurAnimation", value); }
		}

		public int Index
		{
			set { SetArgument("index", value); }
		}

		public float Lean1
		{
			set { SetArgument("lean1", value); }
		}

		public float Lean2
		{
			set { SetArgument("lean2", value); }
		}

		public float Twist
		{
			set { SetArgument("twist", value); }
		}

		public float Margin
		{
			set { SetArgument("margin", value); }
		}
	}

	public sealed class ConfigureSoftLimitHelper : CustomHelper
	{
		public ConfigureSoftLimitHelper(Ped ped) : base(ped, "configureSoftLimit")
		{
		}

		public int Index
		{
			set { SetArgument("index", value); }
		}

		public float Stiffness
		{
			set { SetArgument("stiffness", value); }
		}

		public float Damping
		{
			set { SetArgument("damping", value); }
		}

		public float LimitAngle
		{
			set { SetArgument("limitAngle", value); }
		}

		public int ApproachDirection
		{
			set { SetArgument("approachDirection", value); }
		}

		public bool VelocityScaled
		{
			set { SetArgument("velocityScaled", value); }
		}
	}

	public sealed class ConfigureShotInjuredArmHelper : CustomHelper
	{
		public ConfigureShotInjuredArmHelper(Ped ped) : base(ped, "configureShotInjuredArm")
		{
		}

		public float InjuredArmTime
		{
			set { SetArgument("injuredArmTime", value); }
		}

		public float HipYaw
		{
			set { SetArgument("hipYaw", value); }
		}

		public float HipRoll
		{
			set { SetArgument("hipRoll", value); }
		}

		public float ForceStepExtraHeight
		{
			set { SetArgument("forceStepExtraHeight", value); }
		}

		public bool ForceStep
		{
			set { SetArgument("forceStep", value); }
		}

		public bool StepTurn
		{
			set { SetArgument("stepTurn", value); }
		}

		public float VelMultiplierStart
		{
			set { SetArgument("velMultiplierStart", value); }
		}

		public float VelMultiplierEnd
		{
			set { SetArgument("velMultiplierEnd", value); }
		}

		public float VelForceStep
		{
			set { SetArgument("velForceStep", value); }
		}

		public float VelStepTurn
		{
			set { SetArgument("velStepTurn", value); }
		}

		public bool VelScales
		{
			set { SetArgument("velScales", value); }
		}
	}

	public sealed class ConfigureShotInjuredLegHelper : CustomHelper
	{
		public ConfigureShotInjuredLegHelper(Ped ped) : base(ped, "configureShotInjuredLeg")
		{
		}

		public float TimeBeforeCollapseWoundLeg
		{
			set { SetArgument("timeBeforeCollapseWoundLeg", value); }
		}

		public float LegInjuryTime
		{
			set { SetArgument("legInjuryTime", value); }
		}

		public bool LegForceStep
		{
			set { SetArgument("legForceStep", value); }
		}

		public float LegLimpBend
		{
			set { SetArgument("legLimpBend", value); }
		}

		public float LegLiftTime
		{
			set { SetArgument("legLiftTime", value); }
		}

		public float LegInjury
		{
			set { SetArgument("legInjury", value); }
		}

		public float LegInjuryHipPitch
		{
			set { SetArgument("legInjuryHipPitch", value); }
		}

		public float LegInjuryLiftHipPitch
		{
			set { SetArgument("legInjuryLiftHipPitch", value); }
		}

		public float LegInjurySpineBend
		{
			set { SetArgument("legInjurySpineBend", value); }
		}

		public float LegInjuryLiftSpineBend
		{
			set { SetArgument("legInjuryLiftSpineBend", value); }
		}
	}

	public sealed class DefineAttachedObjectHelper : CustomHelper
	{
		public DefineAttachedObjectHelper(Ped ped) : base(ped, "defineAttachedObject")
		{
		}

		public int PartIndex
		{
			set { SetArgument("partIndex", value); }
		}

		public float ObjectMass
		{
			set { SetArgument("objectMass", value); }
		}

		public Vector3 WorldPos
		{
			set { SetArgument("worldPos", value); }
		}
	}

	public sealed class ForceToBodyPartHelper : CustomHelper
	{
		public ForceToBodyPartHelper(Ped ped) : base(ped, "forceToBodyPart")
		{
		}

		public int PartIndex
		{
			set { SetArgument("partIndex", value); }
		}

		public Vector3 Force
		{
			set { SetArgument("force", value); }
		}

		public bool ForceDefinedInPartSpace
		{
			set { SetArgument("forceDefinedInPartSpace", value); }
		}
	}

	public sealed class LeanInDirectionHelper : CustomHelper
	{
		public LeanInDirectionHelper(Ped ped) : base(ped, "leanInDirection")
		{
		}

		public float LeanAmount
		{
			set { SetArgument("leanAmount", value); }
		}

		public Vector3 Dir
		{
			set { SetArgument("dir", value); }
		}
	}

	public sealed class LeanRandomHelper : CustomHelper
	{
		public LeanRandomHelper(Ped ped) : base(ped, "leanRandom")
		{
		}

		public float LeanAmountMin
		{
			set { SetArgument("leanAmountMin", value); }
		}

		public float LeanAmountMax
		{
			set { SetArgument("leanAmountMax", value); }
		}

		public float ChangeTimeMin
		{
			set { SetArgument("changeTimeMin", value); }
		}

		public float ChangeTimeMax
		{
			set { SetArgument("changeTimeMax", value); }
		}
	}

	public sealed class LeanToPositionHelper : CustomHelper
	{
		public LeanToPositionHelper(Ped ped) : base(ped, "leanToPosition")
		{
		}

		public float LeanAmount
		{
			set { SetArgument("leanAmount", value); }
		}

		public Vector3 Pos
		{
			set { SetArgument("pos", value); }
		}
	}

	public sealed class LeanTowardsObjectHelper : CustomHelper
	{
		public LeanTowardsObjectHelper(Ped ped) : base(ped, "leanTowardsObject")
		{
		}

		public float LeanAmount
		{
			set { SetArgument("leanAmount", value); }
		}

		public Vector3 Offset
		{
			set { SetArgument("offset", value); }
		}

		public int InstanceIndex
		{
			set { SetArgument("instanceIndex", value); }
		}

		public int BoundIndex
		{
			set { SetArgument("boundIndex", value); }
		}
	}

	public sealed class HipsLeanInDirectionHelper : CustomHelper
	{
		public HipsLeanInDirectionHelper(Ped ped) : base(ped, "hipsLeanInDirection")
		{
		}

		public float LeanAmount
		{
			set { SetArgument("leanAmount", value); }
		}

		public Vector3 Dir
		{
			set { SetArgument("dir", value); }
		}
	}

	public sealed class HipsLeanRandomHelper : CustomHelper
	{
		public HipsLeanRandomHelper(Ped ped) : base(ped, "hipsLeanRandom")
		{
		}

		public float LeanAmountMin
		{
			set { SetArgument("leanAmountMin", value); }
		}

		public float LeanAmountMax
		{
			set { SetArgument("leanAmountMax", value); }
		}

		public float ChangeTimeMin
		{
			set { SetArgument("changeTimeMin", value); }
		}

		public float ChangeTimeMax
		{
			set { SetArgument("changeTimeMax", value); }
		}
	}

	public sealed class HipsLeanToPositionHelper : CustomHelper
	{
		public HipsLeanToPositionHelper(Ped ped) : base(ped, "hipsLeanToPosition")
		{
		}

		public float LeanAmount
		{
			set { SetArgument("leanAmount", value); }
		}

		public Vector3 Pos
		{
			set { SetArgument("pos", value); }
		}
	}

	public sealed class HipsLeanTowardsObjectHelper : CustomHelper
	{
		public HipsLeanTowardsObjectHelper(Ped ped) : base(ped, "hipsLeanTowardsObject")
		{
		}

		public float LeanAmount
		{
			set { SetArgument("leanAmount", value); }
		}

		public Vector3 Offset
		{
			set { SetArgument("offset", value); }
		}

		public int InstanceIndex
		{
			set { SetArgument("instanceIndex", value); }
		}

		public int BoundIndex
		{
			set { SetArgument("boundIndex", value); }
		}
	}

	public sealed class ForceLeanInDirectionHelper : CustomHelper
	{
		public ForceLeanInDirectionHelper(Ped ped) : base(ped, "forceLeanInDirection")
		{
		}

		public float LeanAmount
		{
			set { SetArgument("leanAmount", value); }
		}

		public Vector3 Dir
		{
			set { SetArgument("dir", value); }
		}

		public int BodyPart
		{
			set { SetArgument("bodyPart", value); }
		}
	}

	public sealed class ForceLeanRandomHelper : CustomHelper
	{
		public ForceLeanRandomHelper(Ped ped) : base(ped, "forceLeanRandom")
		{
		}

		public float LeanAmountMin
		{
			set { SetArgument("leanAmountMin", value); }
		}

		public float LeanAmountMax
		{
			set { SetArgument("leanAmountMax", value); }
		}

		public float ChangeTimeMin
		{
			set { SetArgument("changeTimeMin", value); }
		}

		public float ChangeTimeMax
		{
			set { SetArgument("changeTimeMax", value); }
		}

		public int BodyPart
		{
			set { SetArgument("bodyPart", value); }
		}
	}

	public sealed class ForceLeanToPositionHelper : CustomHelper
	{
		public ForceLeanToPositionHelper(Ped ped) : base(ped, "forceLeanToPosition")
		{
		}

		public float LeanAmount
		{
			set { SetArgument("leanAmount", value); }
		}

		public Vector3 Pos
		{
			set { SetArgument("pos", value); }
		}

		public int BodyPart
		{
			set { SetArgument("bodyPart", value); }
		}
	}

	public sealed class ForceLeanTowardsObjectHelper : CustomHelper
	{
		public ForceLeanTowardsObjectHelper(Ped ped) : base(ped, "forceLeanTowardsObject")
		{
		}

		public float LeanAmount
		{
			set { SetArgument("leanAmount", value); }
		}

		public Vector3 Offset
		{
			set { SetArgument("offset", value); }
		}

		public int InstanceIndex
		{
			set { SetArgument("instanceIndex", value); }
		}

		public int BoundIndex
		{
			set { SetArgument("boundIndex", value); }
		}

		public int BodyPart
		{
			set { SetArgument("bodyPart", value); }
		}
	}

	public sealed class SetStiffnessHelper : CustomHelper
	{
		public SetStiffnessHelper(Ped ped) : base(ped, "setStiffness")
		{
		}

		public float BodyStiffness
		{
			set { SetArgument("bodyStiffness", value); }
		}

		public float Damping
		{
			set { SetArgument("damping", value); }
		}

		public string Mask
		{
			set { SetArgument("mask", value); }
		}
	}

	public sealed class SetMuscleStiffnessHelper : CustomHelper
	{
		public SetMuscleStiffnessHelper(Ped ped) : base(ped, "setMuscleStiffness")
		{
		}

		public float MuscleStiffness
		{
			set { SetArgument("muscleStiffness", value); }
		}

		public string Mask
		{
			set { SetArgument("mask", value); }
		}
	}

	public sealed class SetWeaponModeHelper : CustomHelper
	{
		public SetWeaponModeHelper(Ped ped) : base(ped, "setWeaponMode")
		{
		}

		public int WeaponMode
		{
			set { SetArgument("weaponMode", value); }
		}
	}

	public sealed class RegisterWeaponHelper : CustomHelper
	{
		public RegisterWeaponHelper(Ped ped) : base(ped, "registerWeapon")
		{
		}

		public int Hand
		{
			set { SetArgument("hand", value); }
		}

		public int LevelIndex
		{
			set { SetArgument("levelIndex", value); }
		}

		public int ConstraintHandle
		{
			set { SetArgument("constraintHandle", value); }
		}

		public Vector3 GunToHandA
		{
			set { SetArgument("gunToHandA", value); }
		}

		public Vector3 GunToHandB
		{
			set { SetArgument("gunToHandB", value); }
		}

		public Vector3 GunToHandC
		{
			set { SetArgument("gunToHandC", value); }
		}

		public Vector3 GunToHandD
		{
			set { SetArgument("gunToHandD", value); }
		}

		public Vector3 GunToMuzzleInGun
		{
			set { SetArgument("gunToMuzzleInGun", value); }
		}

		public Vector3 GunToButtInGun
		{
			set { SetArgument("gunToButtInGun", value); }
		}
	}

	public sealed class ShotRelaxHelper : CustomHelper
	{
		public ShotRelaxHelper(Ped ped) : base(ped, "shotRelax")
		{
		}

		public float RelaxPeriodUpper
		{
			set { SetArgument("relaxPeriodUpper", value); }
		}

		public float RelaxPeriodLower
		{
			set { SetArgument("relaxPeriodLower", value); }
		}
	}

	public sealed class FireWeaponHelper : CustomHelper
	{
		public FireWeaponHelper(Ped ped) : base(ped, "fireWeapon")
		{
		}

		public float FiredWeaponStrength
		{
			set { SetArgument("firedWeaponStrength", value); }
		}

		public int GunHandEnum
		{
			set { SetArgument("gunHandEnum", value); }
		}

		public bool ApplyFireGunForceAtClavicle
		{
			set { SetArgument("applyFireGunForceAtClavicle", value); }
		}

		public float InhibitTime
		{
			set { SetArgument("inhibitTime", value); }
		}

		public Vector3 Direction
		{
			set { SetArgument("direction", value); }
		}

		public float Split
		{
			set { SetArgument("split", value); }
		}
	}

	public sealed class ConfigureConstraintsHelper : CustomHelper
	{
		public ConfigureConstraintsHelper(Ped ped) : base(ped, "configureConstraints")
		{
		}

		public bool HandCuffs
		{
			set { SetArgument("handCuffs", value); }
		}

		public bool HandCuffsBehindBack
		{
			set { SetArgument("handCuffsBehindBack", value); }
		}

		public bool LegCuffs
		{
			set { SetArgument("legCuffs", value); }
		}

		public bool RightDominant
		{
			set { SetArgument("rightDominant", value); }
		}

		public int PassiveMode
		{
			set { SetArgument("passiveMode", value); }
		}

		public bool BespokeBehaviour
		{
			set { SetArgument("bespokeBehaviour", value); }
		}

		public float Blend2ZeroPose
		{
			set { SetArgument("blend2ZeroPose", value); }
		}
	}

	public sealed class StayUprightHelper : CustomHelper
	{
		public StayUprightHelper(Ped ped) : base(ped, "stayUpright")
		{
		}

		public bool UseForces
		{
			set { SetArgument("useForces", value); }
		}

		public bool UseTorques
		{
			set { SetArgument("useTorques", value); }
		}

		public bool LastStandMode
		{
			set { SetArgument("lastStandMode", value); }
		}

		public float LastStandSinkRate
		{
			set { SetArgument("lastStandSinkRate", value); }
		}

		public float LastStandHorizDamping
		{
			set { SetArgument("lastStandHorizDamping", value); }
		}

		public float LastStandMaxTime
		{
			set { SetArgument("lastStandMaxTime", value); }
		}

		public bool TurnTowardsBullets
		{
			set { SetArgument("turnTowardsBullets", value); }
		}

		public bool VelocityBased
		{
			set { SetArgument("velocityBased", value); }
		}

		public bool TorqueOnlyInAir
		{
			set { SetArgument("torqueOnlyInAir", value); }
		}

		public float ForceStrength
		{
			set { SetArgument("forceStrength", value); }
		}

		public float ForceDamping
		{
			set { SetArgument("forceDamping", value); }
		}

		public float ForceFeetMult
		{
			set { SetArgument("forceFeetMult", value); }
		}

		public float ForceSpine3Share
		{
			set { SetArgument("forceSpine3Share", value); }
		}

		public float ForceLeanReduction
		{
			set { SetArgument("forceLeanReduction", value); }
		}

		public float ForceInAirShare
		{
			set { SetArgument("forceInAirShare", value); }
		}

		public float ForceMin
		{
			set { SetArgument("forceMin", value); }
		}

		public float ForceMax
		{
			set { SetArgument("forceMax", value); }
		}

		public float ForceSaturationVel
		{
			set { SetArgument("forceSaturationVel", value); }
		}

		public float ForceThresholdVel
		{
			set { SetArgument("forceThresholdVel", value); }
		}

		public float TorqueStrength
		{
			set { SetArgument("torqueStrength", value); }
		}

		public float TorqueDamping
		{
			set { SetArgument("torqueDamping", value); }
		}

		public float TorqueSaturationVel
		{
			set { SetArgument("torqueSaturationVel", value); }
		}

		public float TorqueThresholdVel
		{
			set { SetArgument("torqueThresholdVel", value); }
		}

		public float SupportPosition
		{
			set { SetArgument("supportPosition", value); }
		}

		public float NoSupportForceMult
		{
			set { SetArgument("noSupportForceMult", value); }
		}

		public float StepUpHelp
		{
			set { SetArgument("stepUpHelp", value); }
		}

		public float StayUpAcc
		{
			set { SetArgument("stayUpAcc", value); }
		}

		public float StayUpAccMax
		{
			set { SetArgument("stayUpAccMax", value); }
		}
	}

	public sealed class StopAllBehavioursHelper : CustomHelper
	{
		public StopAllBehavioursHelper(Ped ped) : base(ped, "stopAllBehaviours")
		{
		}
	}

	public sealed class SetCharacterStrengthHelper : CustomHelper
	{
		public SetCharacterStrengthHelper(Ped ped) : base(ped, "setCharacterStrength")
		{
		}

		public float CharacterStrength
		{
			set { SetArgument("characterStrength", value); }
		}
	}

	public sealed class SetCharacterHealthHelper : CustomHelper
	{
		public SetCharacterHealthHelper(Ped ped) : base(ped, "setCharacterHealth")
		{
		}

		public float CharacterHealth
		{
			set { SetArgument("characterHealth", value); }
		}
	}

	public sealed class SetFallingReactionHelper : CustomHelper
	{
		public SetFallingReactionHelper(Ped ped) : base(ped, "setFallingReaction")
		{
		}

		public bool HandsAndKnees
		{
			set { SetArgument("handsAndKnees", value); }
		}

		public bool CallRDS
		{
			set { SetArgument("callRDS", value); }
		}

		public float ComVelRDSThresh
		{
			set { SetArgument("comVelRDSThresh", value); }
		}

		public bool ResistRolling
		{
			set { SetArgument("resistRolling", value); }
		}

		public float ArmReduceSpeed
		{
			set { SetArgument("armReduceSpeed", value); }
		}

		public float ReachLengthMultiplier
		{
			set { SetArgument("reachLengthMultiplier", value); }
		}

		public float InhibitRollingTime
		{
			set { SetArgument("inhibitRollingTime", value); }
		}

		public float ChangeFrictionTime
		{
			set { SetArgument("changeFrictionTime", value); }
		}

		public float GroundFriction
		{
			set { SetArgument("groundFriction", value); }
		}

		public float FrictionMin
		{
			set { SetArgument("frictionMin", value); }
		}

		public float FrictionMax
		{
			set { SetArgument("frictionMax", value); }
		}

		public bool StopOnSlopes
		{
			set { SetArgument("stopOnSlopes", value); }
		}

		public float StopManual
		{
			set { SetArgument("stopManual", value); }
		}

		public float StoppedStrengthDecay
		{
			set { SetArgument("stoppedStrengthDecay", value); }
		}

		public float SpineLean1Offset
		{
			set { SetArgument("spineLean1Offset", value); }
		}

		public bool RiflePose
		{
			set { SetArgument("riflePose", value); }
		}

		public bool HkHeadAvoid
		{
			set { SetArgument("hkHeadAvoid", value); }
		}

		public bool AntiPropClav
		{
			set { SetArgument("antiPropClav", value); }
		}

		public bool AntiPropWeak
		{
			set { SetArgument("antiPropWeak", value); }
		}

		public bool HeadAsWeakAsArms
		{
			set { SetArgument("headAsWeakAsArms", value); }
		}

		public float SuccessStrength
		{
			set { SetArgument("successStrength", value); }
		}
	}

	public sealed class SetCharacterUnderwaterHelper : CustomHelper
	{
		public SetCharacterUnderwaterHelper(Ped ped) : base(ped, "setCharacterUnderwater")
		{
		}

		public bool Underwater
		{
			set { SetArgument("underwater", value); }
		}

		public float Viscosity
		{
			set { SetArgument("viscosity", value); }
		}

		public float GravityFactor
		{
			set { SetArgument("gravityFactor", value); }
		}

		public float Stroke
		{
			set { SetArgument("stroke", value); }
		}

		public bool LinearStroke
		{
			set { SetArgument("linearStroke", value); }
		}
	}

	public sealed class SetCharacterCollisionsHelper : CustomHelper
	{
		public SetCharacterCollisionsHelper(Ped ped) : base(ped, "setCharacterCollisions")
		{
		}

		public float Spin
		{
			set { SetArgument("spin", value); }
		}

		public float MaxVelocity
		{
			set { SetArgument("maxVelocity", value); }
		}

		public bool ApplyToAll
		{
			set { SetArgument("applyToAll", value); }
		}

		public bool ApplyToSpine
		{
			set { SetArgument("applyToSpine", value); }
		}

		public bool ApplyToThighs
		{
			set { SetArgument("applyToThighs", value); }
		}

		public bool ApplyToClavicles
		{
			set { SetArgument("applyToClavicles", value); }
		}

		public bool ApplyToUpperArms
		{
			set { SetArgument("applyToUpperArms", value); }
		}

		public bool FootSlip
		{
			set { SetArgument("footSlip", value); }
		}

		public int VehicleClass
		{
			set { SetArgument("vehicleClass", value); }
		}
	}

	public sealed class SetCharacterDampingHelper : CustomHelper
	{
		public SetCharacterDampingHelper(Ped ped) : base(ped, "setCharacterDamping")
		{
		}

		public float SomersaultThresh
		{
			set { SetArgument("somersaultThresh", value); }
		}

		public float SomersaultDamp
		{
			set { SetArgument("somersaultDamp", value); }
		}

		public float CartwheelThresh
		{
			set { SetArgument("cartwheelThresh", value); }
		}

		public float CartwheelDamp
		{
			set { SetArgument("cartwheelDamp", value); }
		}

		public float VehicleCollisionTime
		{
			set { SetArgument("vehicleCollisionTime", value); }
		}

		public bool V2
		{
			set { SetArgument("v2", value); }
		}
	}

	public sealed class SetFrictionScaleHelper : CustomHelper
	{
		public SetFrictionScaleHelper(Ped ped) : base(ped, "setFrictionScale")
		{
		}

		public float Scale
		{
			set { SetArgument("scale", value); }
		}

		public float GlobalMin
		{
			set { SetArgument("globalMin", value); }
		}

		public float GlobalMax
		{
			set { SetArgument("globalMax", value); }
		}

		public string Mask
		{
			set { SetArgument("mask", value); }
		}
	}

	public sealed class AnimPoseHelper : CustomHelper
	{
		public AnimPoseHelper(Ped ped) : base(ped, "animPose")
		{
		}

		public float MuscleStiffness
		{
			set { SetArgument("muscleStiffness", value); }
		}

		public float Stiffness
		{
			set { SetArgument("stiffness", value); }
		}

		public float Damping
		{
			set { SetArgument("damping", value); }
		}

		public string EffectorMask
		{
			set { SetArgument("effectorMask", value); }
		}

		public bool OverideHeadlook
		{
			set { SetArgument("overideHeadlook", value); }
		}

		public bool OveridePointArm
		{
			set { SetArgument("overidePointArm", value); }
		}

		public bool OveridePointGun
		{
			set { SetArgument("overidePointGun", value); }
		}

		public bool UseZMPGravityCompensation
		{
			set { SetArgument("useZMPGravityCompensation", value); }
		}

		public float GravityCompensation
		{
			set { SetArgument("gravityCompensation", value); }
		}

		public float MuscleStiffnessLeftArm
		{
			set { SetArgument("muscleStiffnessLeftArm", value); }
		}

		public float MuscleStiffnessRightArm
		{
			set { SetArgument("muscleStiffnessRightArm", value); }
		}

		public float MuscleStiffnessSpine
		{
			set { SetArgument("muscleStiffnessSpine", value); }
		}

		public float MuscleStiffnessLeftLeg
		{
			set { SetArgument("muscleStiffnessLeftLeg", value); }
		}

		public float MuscleStiffnessRightLeg
		{
			set { SetArgument("muscleStiffnessRightLeg", value); }
		}

		public float StiffnessLeftArm
		{
			set { SetArgument("stiffnessLeftArm", value); }
		}

		public float StiffnessRightArm
		{
			set { SetArgument("stiffnessRightArm", value); }
		}

		public float StiffnessSpine
		{
			set { SetArgument("stiffnessSpine", value); }
		}

		public float StiffnessLeftLeg
		{
			set { SetArgument("stiffnessLeftLeg", value); }
		}

		public float StiffnessRightLeg
		{
			set { SetArgument("stiffnessRightLeg", value); }
		}

		public float DampingLeftArm
		{
			set { SetArgument("dampingLeftArm", value); }
		}

		public float DampingRightArm
		{
			set { SetArgument("dampingRightArm", value); }
		}

		public float DampingSpine
		{
			set { SetArgument("dampingSpine", value); }
		}

		public float DampingLeftLeg
		{
			set { SetArgument("dampingLeftLeg", value); }
		}

		public float DampingRightLeg
		{
			set { SetArgument("dampingRightLeg", value); }
		}

		public float GravCompLeftArm
		{
			set { SetArgument("gravCompLeftArm", value); }
		}

		public float GravCompRightArm
		{
			set { SetArgument("gravCompRightArm", value); }
		}

		public float GravCompSpine
		{
			set { SetArgument("gravCompSpine", value); }
		}

		public float GravCompLeftLeg
		{
			set { SetArgument("gravCompLeftLeg", value); }
		}

		public float GravCompRightLeg
		{
			set { SetArgument("gravCompRightLeg", value); }
		}

		public int ConnectedLeftHand
		{
			set { SetArgument("connectedLeftHand", value); }
		}

		public int ConnectedRightHand
		{
			set { SetArgument("connectedRightHand", value); }
		}

		public int ConnectedLeftFoot
		{
			set { SetArgument("connectedLeftFoot", value); }
		}

		public int ConnectedRightFoot
		{
			set { SetArgument("connectedRightFoot", value); }
		}

		public int AnimSource
		{
			set { SetArgument("animSource", value); }
		}

		public int DampenSideMotionInstanceIndex
		{
			set { SetArgument("dampenSideMotionInstanceIndex", value); }
		}
	}

	public sealed class ArmsWindmillHelper : CustomHelper
	{
		public ArmsWindmillHelper(Ped ped) : base(ped, "armsWindmill")
		{
		}

		public int LeftPartID
		{
			set { SetArgument("leftPartID", value); }
		}

		public float LeftRadius1
		{
			set { SetArgument("leftRadius1", value); }
		}

		public float LeftRadius2
		{
			set { SetArgument("leftRadius2", value); }
		}

		public float LeftSpeed
		{
			set { SetArgument("leftSpeed", value); }
		}

		public Vector3 LeftNormal
		{
			set { SetArgument("leftNormal", value); }
		}

		public Vector3 LeftCentre
		{
			set { SetArgument("leftCentre", value); }
		}

		public int RightPartID
		{
			set { SetArgument("rightPartID", value); }
		}

		public float RightRadius1
		{
			set { SetArgument("rightRadius1", value); }
		}

		public float RightRadius2
		{
			set { SetArgument("rightRadius2", value); }
		}

		public float RightSpeed
		{
			set { SetArgument("rightSpeed", value); }
		}

		public Vector3 RightNormal
		{
			set { SetArgument("rightNormal", value); }
		}

		public Vector3 RightCentre
		{
			set { SetArgument("rightCentre", value); }
		}

		public float ShoulderStiffness
		{
			set { SetArgument("shoulderStiffness", value); }
		}

		public float ShoulderDamping
		{
			set { SetArgument("shoulderDamping", value); }
		}

		public float ElbowStiffness
		{
			set { SetArgument("elbowStiffness", value); }
		}

		public float ElbowDamping
		{
			set { SetArgument("elbowDamping", value); }
		}

		public float LeftElbowMin
		{
			set { SetArgument("leftElbowMin", value); }
		}

		public float RightElbowMin
		{
			set { SetArgument("rightElbowMin", value); }
		}

		public float PhaseOffset
		{
			set { SetArgument("phaseOffset", value); }
		}

		public float DragReduction
		{
			set { SetArgument("dragReduction", value); }
		}

		public float IKtwist
		{
			set { SetArgument("IKtwist", value); }
		}

		public float AngVelThreshold
		{
			set { SetArgument("angVelThreshold", value); }
		}

		public float AngVelGain
		{
			set { SetArgument("angVelGain", value); }
		}

		public int MirrorMode
		{
			set { SetArgument("mirrorMode", value); }
		}

		public int AdaptiveMode
		{
			set { SetArgument("adaptiveMode", value); }
		}

		public bool ForceSync
		{
			set { SetArgument("forceSync", value); }
		}

		public bool UseLeft
		{
			set { SetArgument("useLeft", value); }
		}

		public bool UseRight
		{
			set { SetArgument("useRight", value); }
		}

		public bool DisableOnImpact
		{
			set { SetArgument("disableOnImpact", value); }
		}
	}

	public sealed class ArmsWindmillAdaptiveHelper : CustomHelper
	{
		public ArmsWindmillAdaptiveHelper(Ped ped) : base(ped, "armsWindmillAdaptive")
		{
		}

		public float AngSpeed
		{
			set { SetArgument("angSpeed", value); }
		}

		public float BodyStiffness
		{
			set { SetArgument("bodyStiffness", value); }
		}

		public float Amplitude
		{
			set { SetArgument("amplitude", value); }
		}

		public float Phase
		{
			set { SetArgument("phase", value); }
		}

		public float ArmStiffness
		{
			set { SetArgument("armStiffness", value); }
		}

		public float LeftElbowAngle
		{
			set { SetArgument("leftElbowAngle", value); }
		}

		public float RightElbowAngle
		{
			set { SetArgument("rightElbowAngle", value); }
		}

		public float Lean1mult
		{
			set { SetArgument("lean1mult", value); }
		}

		public float Lean1offset
		{
			set { SetArgument("lean1offset", value); }
		}

		public float ElbowRate
		{
			set { SetArgument("elbowRate", value); }
		}

		public ArmDirections ArmDirection
		{
			set { SetArgument("armDirection", (int) value); }
		}

		public bool DisableOnImpact
		{
			set { SetArgument("disableOnImpact", value); }
		}

		public bool SetBackAngles
		{
			set { SetArgument("setBackAngles", value); }
		}

		public bool UseAngMom
		{
			set { SetArgument("useAngMom", value); }
		}

		public bool BendLeftElbow
		{
			set { SetArgument("bendLeftElbow", value); }
		}

		public bool BendRightElbow
		{
			set { SetArgument("bendRightElbow", value); }
		}

		public string Mask
		{
			set { SetArgument("mask", value); }
		}
	}

	public sealed class BalancerCollisionsReactionHelper : CustomHelper
	{
		public BalancerCollisionsReactionHelper(Ped ped) : base(ped, "balancerCollisionsReaction")
		{
		}

		public int NumStepsTillSlump
		{
			set { SetArgument("numStepsTillSlump", value); }
		}

		public float Stable2SlumpTime
		{
			set { SetArgument("stable2SlumpTime", value); }
		}

		public float ExclusionZone
		{
			set { SetArgument("exclusionZone", value); }
		}

		public float FootFrictionMultStart
		{
			set { SetArgument("footFrictionMultStart", value); }
		}

		public float FootFrictionMultRate
		{
			set { SetArgument("footFrictionMultRate", value); }
		}

		public float BackFrictionMultStart
		{
			set { SetArgument("backFrictionMultStart", value); }
		}

		public float BackFrictionMultRate
		{
			set { SetArgument("backFrictionMultRate", value); }
		}

		public float ImpactLegStiffReduction
		{
			set { SetArgument("impactLegStiffReduction", value); }
		}

		public float SlumpLegStiffReduction
		{
			set { SetArgument("slumpLegStiffReduction", value); }
		}

		public float SlumpLegStiffRate
		{
			set { SetArgument("slumpLegStiffRate", value); }
		}

		public float ReactTime
		{
			set { SetArgument("reactTime", value); }
		}

		public float ImpactExagTime
		{
			set { SetArgument("impactExagTime", value); }
		}

		public float GlanceSpinTime
		{
			set { SetArgument("glanceSpinTime", value); }
		}

		public float GlanceSpinMag
		{
			set { SetArgument("glanceSpinMag", value); }
		}

		public float GlanceSpinDecayMult
		{
			set { SetArgument("glanceSpinDecayMult", value); }
		}

		public int IgnoreColWithIndex
		{
			set { SetArgument("ignoreColWithIndex", value); }
		}

		public int SlumpMode
		{
			set { SetArgument("slumpMode", value); }
		}

		public int ReboundMode
		{
			set { SetArgument("reboundMode", value); }
		}

		public float IgnoreColMassBelow
		{
			set { SetArgument("ignoreColMassBelow", value); }
		}

		public int ForwardMode
		{
			set { SetArgument("forwardMode", value); }
		}

		public float TimeToForward
		{
			set { SetArgument("timeToForward", value); }
		}

		public float ReboundForce
		{
			set { SetArgument("reboundForce", value); }
		}

		public bool BraceWall
		{
			set { SetArgument("braceWall", value); }
		}

		public float IgnoreColVolumeBelow
		{
			set { SetArgument("ignoreColVolumeBelow", value); }
		}

		public bool FallOverWallDrape
		{
			set { SetArgument("fallOverWallDrape", value); }
		}

		public bool FallOverHighWalls
		{
			set { SetArgument("fallOverHighWalls", value); }
		}

		public bool Snap
		{
			set { SetArgument("snap", value); }
		}

		public float SnapMag
		{
			set { SetArgument("snapMag", value); }
		}

		public float SnapDirectionRandomness
		{
			set { SetArgument("snapDirectionRandomness", value); }
		}

		public bool SnapLeftArm
		{
			set { SetArgument("snapLeftArm", value); }
		}

		public bool SnapRightArm
		{
			set { SetArgument("snapRightArm", value); }
		}

		public bool SnapLeftLeg
		{
			set { SetArgument("snapLeftLeg", value); }
		}

		public bool SnapRightLeg
		{
			set { SetArgument("snapRightLeg", value); }
		}

		public bool SnapSpine
		{
			set { SetArgument("snapSpine", value); }
		}

		public bool SnapNeck
		{
			set { SetArgument("snapNeck", value); }
		}

		public bool SnapPhasedLegs
		{
			set { SetArgument("snapPhasedLegs", value); }
		}

		public int SnapHipType
		{
			set { SetArgument("snapHipType", value); }
		}

		public float UnSnapInterval
		{
			set { SetArgument("unSnapInterval", value); }
		}

		public float UnSnapRatio
		{
			set { SetArgument("unSnapRatio", value); }
		}

		public bool SnapUseTorques
		{
			set { SetArgument("snapUseTorques", value); }
		}

		public float ImpactWeaknessZeroDuration
		{
			set { SetArgument("impactWeaknessZeroDuration", value); }
		}

		public float ImpactWeaknessRampDuration
		{
			set { SetArgument("impactWeaknessRampDuration", value); }
		}

		public float ImpactLoosenessAmount
		{
			set { SetArgument("impactLoosenessAmount", value); }
		}

		public bool ObjectBehindVictim
		{
			set { SetArgument("objectBehindVictim", value); }
		}

		public Vector3 ObjectBehindVictimPos
		{
			set { SetArgument("objectBehindVictimPos", value); }
		}

		public Vector3 ObjectBehindVictimNormal
		{
			set { SetArgument("objectBehindVictimNormal", value); }
		}
	}

	public sealed class BodyBalanceHelper : CustomHelper
	{
		public BodyBalanceHelper(Ped ped) : base(ped, "bodyBalance")
		{
		}

		public float ArmStiffness
		{
			set { SetArgument("armStiffness", value); }
		}

		public float Elbow
		{
			set { SetArgument("elbow", value); }
		}

		public float Shoulder
		{
			set { SetArgument("shoulder", value); }
		}

		public float ArmDamping
		{
			set { SetArgument("armDamping", value); }
		}

		public bool UseHeadLook
		{
			set { SetArgument("useHeadLook", value); }
		}

		public Vector3 HeadLookPos
		{
			set { SetArgument("headLookPos", value); }
		}

		public int HeadLookInstanceIndex
		{
			set { SetArgument("headLookInstanceIndex", value); }
		}

		public float SpineStiffness
		{
			set { SetArgument("spineStiffness", value); }
		}

		public float SomersaultAngle
		{
			set { SetArgument("somersaultAngle", value); }
		}

		public float SomersaultAngleThreshold
		{
			set { SetArgument("somersaultAngleThreshold", value); }
		}

		public float SideSomersaultAngle
		{
			set { SetArgument("sideSomersaultAngle", value); }
		}

		public float SideSomersaultAngleThreshold
		{
			set { SetArgument("sideSomersaultAngleThreshold", value); }
		}

		public bool BackwardsAutoTurn
		{
			set { SetArgument("backwardsAutoTurn", value); }
		}

		public float TurnWithBumpRadius
		{
			set { SetArgument("turnWithBumpRadius", value); }
		}

		public bool BackwardsArms
		{
			set { SetArgument("backwardsArms", value); }
		}

		public bool BlendToZeroPose
		{
			set { SetArgument("blendToZeroPose", value); }
		}

		public bool ArmsOutOnPush
		{
			set { SetArgument("armsOutOnPush", value); }
		}

		public float ArmsOutOnPushMultiplier
		{
			set { SetArgument("armsOutOnPushMultiplier", value); }
		}

		public float ArmsOutOnPushTimeout
		{
			set { SetArgument("armsOutOnPushTimeout", value); }
		}

		public float ReturningToBalanceArmsOut
		{
			set { SetArgument("returningToBalanceArmsOut", value); }
		}

		public float ArmsOutStraightenElbows
		{
			set { SetArgument("armsOutStraightenElbows", value); }
		}

		public float ArmsOutMinLean2
		{
			set { SetArgument("armsOutMinLean2", value); }
		}

		public float SpineDamping
		{
			set { SetArgument("spineDamping", value); }
		}

		public bool UseBodyTurn
		{
			set { SetArgument("useBodyTurn", value); }
		}

		public float ElbowAngleOnContact
		{
			set { SetArgument("elbowAngleOnContact", value); }
		}

		public float BendElbowsTime
		{
			set { SetArgument("bendElbowsTime", value); }
		}

		public float BendElbowsGait
		{
			set { SetArgument("bendElbowsGait", value); }
		}

		public float HipL2ArmL2
		{
			set { SetArgument("hipL2ArmL2", value); }
		}

		public float ShoulderL2
		{
			set { SetArgument("shoulderL2", value); }
		}

		public float ShoulderL1
		{
			set { SetArgument("shoulderL1", value); }
		}

		public float ShoulderTwist
		{
			set { SetArgument("shoulderTwist", value); }
		}

		public float HeadLookAtVelProb
		{
			set { SetArgument("headLookAtVelProb", value); }
		}

		public float TurnOffProb
		{
			set { SetArgument("turnOffProb", value); }
		}

		public float Turn2VelProb
		{
			set { SetArgument("turn2VelProb", value); }
		}

		public float TurnAwayProb
		{
			set { SetArgument("turnAwayProb", value); }
		}

		public float TurnLeftProb
		{
			set { SetArgument("turnLeftProb", value); }
		}

		public float TurnRightProb
		{
			set { SetArgument("turnRightProb", value); }
		}

		public float Turn2TargetProb
		{
			set { SetArgument("turn2TargetProb", value); }
		}

		public Vector3 AngVelMultiplier
		{
			set { SetArgument("angVelMultiplier", value); }
		}

		public Vector3 AngVelThreshold
		{
			set { SetArgument("angVelThreshold", value); }
		}

		public float BraceDistance
		{
			set { SetArgument("braceDistance", value); }
		}

		public float TargetPredictionTime
		{
			set { SetArgument("targetPredictionTime", value); }
		}

		public float ReachAbsorbtionTime
		{
			set { SetArgument("reachAbsorbtionTime", value); }
		}

		public float BraceStiffness
		{
			set { SetArgument("braceStiffness", value); }
		}

		public float MinBraceTime
		{
			set { SetArgument("minBraceTime", value); }
		}

		public float TimeToBackwardsBrace
		{
			set { SetArgument("timeToBackwardsBrace", value); }
		}

		public float HandsDelayMin
		{
			set { SetArgument("handsDelayMin", value); }
		}

		public float HandsDelayMax
		{
			set { SetArgument("handsDelayMax", value); }
		}

		public float BraceOffset
		{
			set { SetArgument("braceOffset", value); }
		}

		public float MoveRadius
		{
			set { SetArgument("moveRadius", value); }
		}

		public float MoveAmount
		{
			set { SetArgument("moveAmount", value); }
		}

		public bool MoveWhenBracing
		{
			set { SetArgument("moveWhenBracing", value); }
		}
	}

	public sealed class BodyFoetalHelper : CustomHelper
	{
		public BodyFoetalHelper(Ped ped) : base(ped, "bodyFoetal")
		{
		}

		public float Stiffness
		{
			set { SetArgument("stiffness", value); }
		}

		public float DampingFactor
		{
			set { SetArgument("dampingFactor", value); }
		}

		public float Asymmetry
		{
			set { SetArgument("asymmetry", value); }
		}

		public int RandomSeed
		{
			set { SetArgument("randomSeed", value); }
		}

		public float BackTwist
		{
			set { SetArgument("backTwist", value); }
		}

		public string Mask
		{
			set { SetArgument("mask", value); }
		}
	}

	public sealed class BodyRollUpHelper : CustomHelper
	{
		public BodyRollUpHelper(Ped ped) : base(ped, "bodyRollUp")
		{
		}

		public float Stiffness
		{
			set { SetArgument("stiffness", value); }
		}

		public float UseArmToSlowDown
		{
			set { SetArgument("useArmToSlowDown", value); }
		}

		public float ArmReachAmount
		{
			set { SetArgument("armReachAmount", value); }
		}

		public string Mask
		{
			set { SetArgument("mask", value); }
		}

		public float LegPush
		{
			set { SetArgument("legPush", value); }
		}

		public float AsymmetricalLegs
		{
			set { SetArgument("asymmetricalLegs", value); }
		}

		public float NoRollTimeBeforeSuccess
		{
			set { SetArgument("noRollTimeBeforeSuccess", value); }
		}

		public float RollVelForSuccess
		{
			set { SetArgument("rollVelForSuccess", value); }
		}

		public float RollVelLinearContribution
		{
			set { SetArgument("rollVelLinearContribution", value); }
		}

		public float VelocityScale
		{
			set { SetArgument("velocityScale", value); }
		}

		public float VelocityOffset
		{
			set { SetArgument("velocityOffset", value); }
		}

		public bool ApplyMinMaxFriction
		{
			set { SetArgument("applyMinMaxFriction", value); }
		}
	}

	public sealed class BodyWritheHelper : CustomHelper
	{
		public BodyWritheHelper(Ped ped) : base(ped, "bodyWrithe")
		{
		}

		public float ArmStiffness
		{
			set { SetArgument("armStiffness", value); }
		}

		public float BackStiffness
		{
			set { SetArgument("backStiffness", value); }
		}

		public float LegStiffness
		{
			set { SetArgument("legStiffness", value); }
		}

		public float ArmDamping
		{
			set { SetArgument("armDamping", value); }
		}

		public float BackDamping
		{
			set { SetArgument("backDamping", value); }
		}

		public float LegDamping
		{
			set { SetArgument("legDamping", value); }
		}

		public float ArmPeriod
		{
			set { SetArgument("armPeriod", value); }
		}

		public float BackPeriod
		{
			set { SetArgument("backPeriod", value); }
		}

		public float LegPeriod
		{
			set { SetArgument("legPeriod", value); }
		}

		public string Mask
		{
			set { SetArgument("mask", value); }
		}

		public float ArmAmplitude
		{
			set { SetArgument("armAmplitude", value); }
		}

		public float BackAmplitude
		{
			set { SetArgument("backAmplitude", value); }
		}

		public float LegAmplitude
		{
			set { SetArgument("legAmplitude", value); }
		}

		public float ElbowAmplitude
		{
			set { SetArgument("elbowAmplitude", value); }
		}

		public float KneeAmplitude
		{
			set { SetArgument("kneeAmplitude", value); }
		}

		public bool RollOverFlag
		{
			set { SetArgument("rollOverFlag", value); }
		}

		public float BlendArms
		{
			set { SetArgument("blendArms", value); }
		}

		public float BlendBack
		{
			set { SetArgument("blendBack", value); }
		}

		public float BlendLegs
		{
			set { SetArgument("blendLegs", value); }
		}

		public bool ApplyStiffness
		{
			set { SetArgument("applyStiffness", value); }
		}

		public bool OnFire
		{
			set { SetArgument("onFire", value); }
		}

		public float ShoulderLean1
		{
			set { SetArgument("shoulderLean1", value); }
		}

		public float ShoulderLean2
		{
			set { SetArgument("shoulderLean2", value); }
		}

		public float Lean1BlendFactor
		{
			set { SetArgument("lean1BlendFactor", value); }
		}

		public float Lean2BlendFactor
		{
			set { SetArgument("lean2BlendFactor", value); }
		}

		public float RollTorqueScale
		{
			set { SetArgument("rollTorqueScale", value); }
		}

		public float MaxRollOverTime
		{
			set { SetArgument("maxRollOverTime", value); }
		}

		public float RollOverRadius
		{
			set { SetArgument("rollOverRadius", value); }
		}
	}

	public sealed class BraceForImpactHelper : CustomHelper
	{
		public BraceForImpactHelper(Ped ped) : base(ped, "braceForImpact")
		{
		}

		public float BraceDistance
		{
			set { SetArgument("braceDistance", value); }
		}

		public float TargetPredictionTime
		{
			set { SetArgument("targetPredictionTime", value); }
		}

		public float ReachAbsorbtionTime
		{
			set { SetArgument("reachAbsorbtionTime", value); }
		}

		public int InstanceIndex
		{
			set { SetArgument("instanceIndex", value); }
		}

		public float BodyStiffness
		{
			set { SetArgument("bodyStiffness", value); }
		}

		public bool GrabDontLetGo
		{
			set { SetArgument("grabDontLetGo", value); }
		}

		public float GrabStrength
		{
			set { SetArgument("grabStrength", value); }
		}

		public float GrabDistance
		{
			set { SetArgument("grabDistance", value); }
		}

		public float GrabReachAngle
		{
			set { SetArgument("grabReachAngle", value); }
		}

		public float GrabHoldTimer
		{
			set { SetArgument("grabHoldTimer", value); }
		}

		public float MaxGrabCarVelocity
		{
			set { SetArgument("maxGrabCarVelocity", value); }
		}

		public float LegStiffness
		{
			set { SetArgument("legStiffness", value); }
		}

		public float TimeToBackwardsBrace
		{
			set { SetArgument("timeToBackwardsBrace", value); }
		}

		public Vector3 Look
		{
			set { SetArgument("look", value); }
		}

		public Vector3 Pos
		{
			set { SetArgument("pos", value); }
		}

		public float MinBraceTime
		{
			set { SetArgument("minBraceTime", value); }
		}

		public float HandsDelayMin
		{
			set { SetArgument("handsDelayMin", value); }
		}

		public float HandsDelayMax
		{
			set { SetArgument("handsDelayMax", value); }
		}

		public bool MoveAway
		{
			set { SetArgument("moveAway", value); }
		}

		public float MoveAwayAmount
		{
			set { SetArgument("moveAwayAmount", value); }
		}

		public float MoveAwayLean
		{
			set { SetArgument("moveAwayLean", value); }
		}

		public float MoveSideways
		{
			set { SetArgument("moveSideways", value); }
		}

		public bool BbArms
		{
			set { SetArgument("bbArms", value); }
		}

		public bool NewBrace
		{
			set { SetArgument("newBrace", value); }
		}

		public bool BraceOnImpact
		{
			set { SetArgument("braceOnImpact", value); }
		}

		public bool Roll2Velocity
		{
			set { SetArgument("roll2Velocity", value); }
		}

		public int RollType
		{
			set { SetArgument("rollType", value); }
		}

		public bool SnapImpacts
		{
			set { SetArgument("snapImpacts", value); }
		}

		public float SnapImpact
		{
			set { SetArgument("snapImpact", value); }
		}

		public float SnapBonnet
		{
			set { SetArgument("snapBonnet", value); }
		}

		public float SnapFloor
		{
			set { SetArgument("snapFloor", value); }
		}

		public bool DampVel
		{
			set { SetArgument("dampVel", value); }
		}

		public float DampSpin
		{
			set { SetArgument("dampSpin", value); }
		}

		public float DampUpVel
		{
			set { SetArgument("dampUpVel", value); }
		}

		public float DampSpinThresh
		{
			set { SetArgument("dampSpinThresh", value); }
		}

		public float DampUpVelThresh
		{
			set { SetArgument("dampUpVelThresh", value); }
		}

		public bool GsHelp
		{
			set { SetArgument("gsHelp", value); }
		}

		public float GsEndMin
		{
			set { SetArgument("gsEndMin", value); }
		}

		public float GsSideMin
		{
			set { SetArgument("gsSideMin", value); }
		}

		public float GsSideMax
		{
			set { SetArgument("gsSideMax", value); }
		}

		public float GsUpness
		{
			set { SetArgument("gsUpness", value); }
		}

		public float GsCarVelMin
		{
			set { SetArgument("gsCarVelMin", value); }
		}

		public bool GsScale1Foot
		{
			set { SetArgument("gsScale1Foot", value); }
		}

		public float GsFricScale1
		{
			set { SetArgument("gsFricScale1", value); }
		}

		public string GsFricMask1
		{
			set { SetArgument("gsFricMask1", value); }
		}

		public float GsFricScale2
		{
			set { SetArgument("gsFricScale2", value); }
		}

		public string GsFricMask2
		{
			set { SetArgument("gsFricMask2", value); }
		}
	}

	public sealed class BuoyancyHelper : CustomHelper
	{
		public BuoyancyHelper(Ped ped) : base(ped, "buoyancy")
		{
		}

		public Vector3 SurfacePoint
		{
			set { SetArgument("surfacePoint", value); }
		}

		public Vector3 SurfaceNormal
		{
			set { SetArgument("surfaceNormal", value); }
		}

		public float Buoyancy
		{
			set { SetArgument("buoyancy", value); }
		}

		public float ChestBuoyancy
		{
			set { SetArgument("chestBuoyancy", value); }
		}

		public float Damping
		{
			set { SetArgument("damping", value); }
		}

		public bool Righting
		{
			set { SetArgument("righting", value); }
		}

		public float RightingStrength
		{
			set { SetArgument("rightingStrength", value); }
		}

		public float RightingTime
		{
			set { SetArgument("rightingTime", value); }
		}
	}

	public sealed class CatchFallHelper : CustomHelper
	{
		public CatchFallHelper(Ped ped) : base(ped, "catchFall")
		{
		}

		public float TorsoStiffness
		{
			set { SetArgument("torsoStiffness", value); }
		}

		public float LegsStiffness
		{
			set { SetArgument("legsStiffness", value); }
		}

		public float ArmsStiffness
		{
			set { SetArgument("armsStiffness", value); }
		}

		public float BackwardsMinArmOffset
		{
			set { SetArgument("backwardsMinArmOffset", value); }
		}

		public float ForwardMaxArmOffset
		{
			set { SetArgument("forwardMaxArmOffset", value); }
		}

		public float ZAxisSpinReduction
		{
			set { SetArgument("zAxisSpinReduction", value); }
		}

		public float ExtraSit
		{
			set { SetArgument("extraSit", value); }
		}

		public bool UseHeadLook
		{
			set { SetArgument("useHeadLook", value); }
		}

		public string Mask
		{
			set { SetArgument("mask", value); }
		}
	}

	public sealed class ElectrocuteHelper : CustomHelper
	{
		public ElectrocuteHelper(Ped ped) : base(ped, "electrocute")
		{
		}

		public float StunMag
		{
			set { SetArgument("stunMag", value); }
		}

		public float InitialMult
		{
			set { SetArgument("initialMult", value); }
		}

		public float LargeMult
		{
			set { SetArgument("largeMult", value); }
		}

		public float LargeMinTime
		{
			set { SetArgument("largeMinTime", value); }
		}

		public float LargeMaxTime
		{
			set { SetArgument("largeMaxTime", value); }
		}

		public float MovingMult
		{
			set { SetArgument("movingMult", value); }
		}

		public float BalancingMult
		{
			set { SetArgument("balancingMult", value); }
		}

		public float AirborneMult
		{
			set { SetArgument("airborneMult", value); }
		}

		public float MovingThresh
		{
			set { SetArgument("movingThresh", value); }
		}

		public float StunInterval
		{
			set { SetArgument("stunInterval", value); }
		}

		public float DirectionRandomness
		{
			set { SetArgument("directionRandomness", value); }
		}

		public bool LeftArm
		{
			set { SetArgument("leftArm", value); }
		}

		public bool RightArm
		{
			set { SetArgument("rightArm", value); }
		}

		public bool LeftLeg
		{
			set { SetArgument("leftLeg", value); }
		}

		public bool RightLeg
		{
			set { SetArgument("rightLeg", value); }
		}

		public bool Spine
		{
			set { SetArgument("spine", value); }
		}

		public bool Neck
		{
			set { SetArgument("neck", value); }
		}

		public bool PhasedLegs
		{
			set { SetArgument("phasedLegs", value); }
		}

		public bool ApplyStiffness
		{
			set { SetArgument("applyStiffness", value); }
		}

		public bool UseTorques
		{
			set { SetArgument("useTorques", value); }
		}

		public int HipType
		{
			set { SetArgument("hipType", value); }
		}
	}

	public sealed class FallOverWallHelper : CustomHelper
	{
		public FallOverWallHelper(Ped ped) : base(ped, "fallOverWall")
		{
		}

		public float BodyStiffness
		{
			set { SetArgument("bodyStiffness", value); }
		}

		public float Damping
		{
			set { SetArgument("damping", value); }
		}

		public float MagOfForce
		{
			set { SetArgument("magOfForce", value); }
		}

		public float MaxDistanceFromPelToHitPoint
		{
			set { SetArgument("maxDistanceFromPelToHitPoint", value); }
		}

		public float MaxForceDist
		{
			set { SetArgument("maxForceDist", value); }
		}

		public float StepExclusionZone
		{
			set { SetArgument("stepExclusionZone", value); }
		}

		public float MinLegHeight
		{
			set { SetArgument("minLegHeight", value); }
		}

		public float BodyTwist
		{
			set { SetArgument("bodyTwist", value); }
		}

		public float MaxTwist
		{
			set { SetArgument("maxTwist", value); }
		}

		public Vector3 FallOverWallEndA
		{
			set { SetArgument("fallOverWallEndA", value); }
		}

		public Vector3 FallOverWallEndB
		{
			set { SetArgument("fallOverWallEndB", value); }
		}

		public float ForceAngleAbort
		{
			set { SetArgument("forceAngleAbort", value); }
		}

		public float ForceTimeOut
		{
			set { SetArgument("forceTimeOut", value); }
		}

		public bool MoveArms
		{
			set { SetArgument("moveArms", value); }
		}

		public bool MoveLegs
		{
			set { SetArgument("moveLegs", value); }
		}

		public bool BendSpine
		{
			set { SetArgument("bendSpine", value); }
		}

		public float AngleDirWithWallNormal
		{
			set { SetArgument("angleDirWithWallNormal", value); }
		}

		public float LeaningAngleThreshold
		{
			set { SetArgument("leaningAngleThreshold", value); }
		}

		public float MaxAngVel
		{
			set { SetArgument("maxAngVel", value); }
		}

		public bool AdaptForcesToLowWall
		{
			set { SetArgument("adaptForcesToLowWall", value); }
		}

		public float MaxWallHeight
		{
			set { SetArgument("maxWallHeight", value); }
		}

		public float DistanceToSendSuccessMessage
		{
			set { SetArgument("distanceToSendSuccessMessage", value); }
		}

		public float RollingBackThr
		{
			set { SetArgument("rollingBackThr", value); }
		}

		public float RollingPotential
		{
			set { SetArgument("rollingPotential", value); }
		}

		public bool UseArmIK
		{
			set { SetArgument("useArmIK", value); }
		}

		public float ReachDistanceFromHitPoint
		{
			set { SetArgument("reachDistanceFromHitPoint", value); }
		}

		public float MinReachDistanceFromHitPoint
		{
			set { SetArgument("minReachDistanceFromHitPoint", value); }
		}

		public float AngleTotallyBack
		{
			set { SetArgument("angleTotallyBack", value); }
		}
	}

	public sealed class GrabHelper : CustomHelper
	{
		public GrabHelper(Ped ped) : base(ped, "grab")
		{
		}

		public bool UseLeft
		{
			set { SetArgument("useLeft", value); }
		}

		public bool UseRight
		{
			set { SetArgument("useRight", value); }
		}

		public bool DropWeaponIfNecessary
		{
			set { SetArgument("dropWeaponIfNecessary", value); }
		}

		public float DropWeaponDistance
		{
			set { SetArgument("dropWeaponDistance", value); }
		}

		public float GrabStrength
		{
			set { SetArgument("grabStrength", value); }
		}

		public float StickyHands
		{
			set { SetArgument("stickyHands", value); }
		}

		public int TurnToTarget
		{
			set { SetArgument("turnToTarget", value); }
		}

		public float GrabHoldMaxTimer
		{
			set { SetArgument("grabHoldMaxTimer", value); }
		}

		public float PullUpTime
		{
			set { SetArgument("pullUpTime", value); }
		}

		public float PullUpStrengthRight
		{
			set { SetArgument("pullUpStrengthRight", value); }
		}

		public float PullUpStrengthLeft
		{
			set { SetArgument("pullUpStrengthLeft", value); }
		}

		public Vector3 Pos1
		{
			set { SetArgument("pos1", value); }
		}

		public Vector3 Pos2
		{
			set { SetArgument("pos2", value); }
		}

		public Vector3 Pos3
		{
			set { SetArgument("pos3", value); }
		}

		public Vector3 Pos4
		{
			set { SetArgument("pos4", value); }
		}

		public Vector3 NormalR
		{
			set { SetArgument("normalR", value); }
		}

		public Vector3 NormalL
		{
			set { SetArgument("normalL", value); }
		}

		public Vector3 NormalR2
		{
			set { SetArgument("normalR2", value); }
		}

		public Vector3 NormalL2
		{
			set { SetArgument("normalL2", value); }
		}

		public bool HandsCollide
		{
			set { SetArgument("handsCollide", value); }
		}

		public bool JustBrace
		{
			set { SetArgument("justBrace", value); }
		}

		public bool UseLineGrab
		{
			set { SetArgument("useLineGrab", value); }
		}

		public bool PointsX4grab
		{
			set { SetArgument("pointsX4grab", value); }
		}

		public bool FromEA
		{
			set { SetArgument("fromEA", value); }
		}

		public bool SurfaceGrab
		{
			set { SetArgument("surfaceGrab", value); }
		}

		public int InstanceIndex
		{
			set { SetArgument("instanceIndex", value); }
		}

		public int InstancePartIndex
		{
			set { SetArgument("instancePartIndex", value); }
		}

		public bool DontLetGo
		{
			set { SetArgument("dontLetGo", value); }
		}

		public float BodyStiffness
		{
			set { SetArgument("bodyStiffness", value); }
		}

		public float ReachAngle
		{
			set { SetArgument("reachAngle", value); }
		}

		public float OneSideReachAngle
		{
			set { SetArgument("oneSideReachAngle", value); }
		}

		public float GrabDistance
		{
			set { SetArgument("grabDistance", value); }
		}

		public float Move2Radius
		{
			set { SetArgument("move2Radius", value); }
		}

		public float ArmStiffness
		{
			set { SetArgument("armStiffness", value); }
		}

		public float MaxReachDistance
		{
			set { SetArgument("maxReachDistance", value); }
		}

		public float OrientationConstraintScale
		{
			set { SetArgument("orientationConstraintScale", value); }
		}

		public float MaxWristAngle
		{
			set { SetArgument("maxWristAngle", value); }
		}

		public bool UseHeadLookToTarget
		{
			set { SetArgument("useHeadLookToTarget", value); }
		}

		public bool LookAtGrab
		{
			set { SetArgument("lookAtGrab", value); }
		}

		public Vector3 TargetForHeadLook
		{
			set { SetArgument("targetForHeadLook", value); }
		}
	}

	public sealed class HeadLookHelper : CustomHelper
	{
		public HeadLookHelper(Ped ped) : base(ped, "headLook")
		{
		}

		public float Damping
		{
			set { SetArgument("damping", value); }
		}

		public float Stiffness
		{
			set { SetArgument("stiffness", value); }
		}

		public int InstanceIndex
		{
			set { SetArgument("instanceIndex", value); }
		}

		public Vector3 Vel
		{
			set { SetArgument("vel", value); }
		}

		public Vector3 Pos
		{
			set { SetArgument("pos", value); }
		}

		public bool AlwaysLook
		{
			set { SetArgument("alwaysLook", value); }
		}

		public bool EyesHorizontal
		{
			set { SetArgument("eyesHorizontal", value); }
		}

		public bool AlwaysEyesHorizontal
		{
			set { SetArgument("alwaysEyesHorizontal", value); }
		}

		public bool KeepHeadAwayFromGround
		{
			set { SetArgument("keepHeadAwayFromGround", value); }
		}

		public bool TwistSpine
		{
			set { SetArgument("twistSpine", value); }
		}
	}

	public sealed class HighFallHelper : CustomHelper
	{
		public HighFallHelper(Ped ped) : base(ped, "highFall")
		{
		}

		public float BodyStiffness
		{
			set { SetArgument("bodyStiffness", value); }
		}

		public float Bodydamping
		{
			set { SetArgument("bodydamping", value); }
		}

		public float Catchfalltime
		{
			set { SetArgument("catchfalltime", value); }
		}

		public float CrashOrLandCutOff
		{
			set { SetArgument("crashOrLandCutOff", value); }
		}

		public float PdStrength
		{
			set { SetArgument("pdStrength", value); }
		}

		public float PdDamping
		{
			set { SetArgument("pdDamping", value); }
		}

		public float ArmAngSpeed
		{
			set { SetArgument("armAngSpeed", value); }
		}

		public float ArmAmplitude
		{
			set { SetArgument("armAmplitude", value); }
		}

		public float ArmPhase
		{
			set { SetArgument("armPhase", value); }
		}

		public bool ArmBendElbows
		{
			set { SetArgument("armBendElbows", value); }
		}

		public float LegRadius
		{
			set { SetArgument("legRadius", value); }
		}

		public float LegAngSpeed
		{
			set { SetArgument("legAngSpeed", value); }
		}

		public float LegAsymmetry
		{
			set { SetArgument("legAsymmetry", value); }
		}

		public float Arms2LegsPhase
		{
			set { SetArgument("arms2LegsPhase", value); }
		}

		public int Arms2LegsSync
		{
			set { SetArgument("arms2LegsSync", value); }
		}

		public float ArmsUp
		{
			set { SetArgument("armsUp", value); }
		}

		public bool OrientateBodyToFallDirection
		{
			set { SetArgument("orientateBodyToFallDirection", value); }
		}

		public bool OrientateTwist
		{
			set { SetArgument("orientateTwist", value); }
		}

		public float OrientateMax
		{
			set { SetArgument("orientateMax", value); }
		}

		public bool AlanRickman
		{
			set { SetArgument("alanRickman", value); }
		}

		public bool FowardRoll
		{
			set { SetArgument("fowardRoll", value); }
		}

		public bool UseZeroPose_withFowardRoll
		{
			set { SetArgument("useZeroPose_withFowardRoll", value); }
		}

		public float AimAngleBase
		{
			set { SetArgument("aimAngleBase", value); }
		}

		public float FowardVelRotation
		{
			set { SetArgument("fowardVelRotation", value); }
		}

		public float FootVelCompScale
		{
			set { SetArgument("footVelCompScale", value); }
		}

		public float SideD
		{
			set { SetArgument("sideD", value); }
		}

		public float FowardOffsetOfLegIK
		{
			set { SetArgument("fowardOffsetOfLegIK", value); }
		}

		public float LegL
		{
			set { SetArgument("legL", value); }
		}

		public float CatchFallCutOff
		{
			set { SetArgument("catchFallCutOff", value); }
		}

		public float LegStrength
		{
			set { SetArgument("legStrength", value); }
		}

		public bool Balance
		{
			set { SetArgument("balance", value); }
		}

		public bool IgnorWorldCollisions
		{
			set { SetArgument("ignorWorldCollisions", value); }
		}

		public bool AdaptiveCircling
		{
			set { SetArgument("adaptiveCircling", value); }
		}

		public bool Hula
		{
			set { SetArgument("hula", value); }
		}

		public float MaxSpeedForRecoverableFall
		{
			set { SetArgument("maxSpeedForRecoverableFall", value); }
		}

		public float MinSpeedForBrace
		{
			set { SetArgument("minSpeedForBrace", value); }
		}

		public float LandingNormal
		{
			set { SetArgument("landingNormal", value); }
		}
	}

	public sealed class IncomingTransformsHelper : CustomHelper
	{
		public IncomingTransformsHelper(Ped ped) : base(ped, "incomingTransforms")
		{
		}
	}

	public sealed class InjuredOnGroundHelper : CustomHelper
	{
		public InjuredOnGroundHelper(Ped ped) : base(ped, "injuredOnGround")
		{
		}

		public int NumInjuries
		{
			set { SetArgument("numInjuries", value); }
		}

		public int Injury1Component
		{
			set { SetArgument("injury1Component", value); }
		}

		public int Injury2Component
		{
			set { SetArgument("injury2Component", value); }
		}

		public Vector3 Injury1LocalPosition
		{
			set { SetArgument("injury1LocalPosition", value); }
		}

		public Vector3 Injury2LocalPosition
		{
			set { SetArgument("injury2LocalPosition", value); }
		}

		public Vector3 Injury1LocalNormal
		{
			set { SetArgument("injury1LocalNormal", value); }
		}

		public Vector3 Injury2LocalNormal
		{
			set { SetArgument("injury2LocalNormal", value); }
		}

		public Vector3 AttackerPos
		{
			set { SetArgument("attackerPos", value); }
		}

		public bool DontReachWithLeft
		{
			set { SetArgument("dontReachWithLeft", value); }
		}

		public bool DontReachWithRight
		{
			set { SetArgument("dontReachWithRight", value); }
		}

		public bool StrongRollForce
		{
			set { SetArgument("strongRollForce", value); }
		}
	}

	public sealed class CarriedHelper : CustomHelper
	{
		public CarriedHelper(Ped ped) : base(ped, "carried")
		{
		}
	}

	public sealed class DangleHelper : CustomHelper
	{
		public DangleHelper(Ped ped) : base(ped, "dangle")
		{
		}

		public bool DoGrab
		{
			set { SetArgument("doGrab", value); }
		}

		public float GrabFrequency
		{
			set { SetArgument("grabFrequency", value); }
		}
	}

	public sealed class OnFireHelper : CustomHelper
	{
		public OnFireHelper(Ped ped) : base(ped, "onFire")
		{
		}

		public float StaggerTime
		{
			set { SetArgument("staggerTime", value); }
		}

		public float StaggerLeanRate
		{
			set { SetArgument("staggerLeanRate", value); }
		}

		public float StumbleMaxLeanBack
		{
			set { SetArgument("stumbleMaxLeanBack", value); }
		}

		public float StumbleMaxLeanForward
		{
			set { SetArgument("stumbleMaxLeanForward", value); }
		}

		public float ArmsWindmillWritheBlend
		{
			set { SetArgument("armsWindmillWritheBlend", value); }
		}

		public float SpineStumbleWritheBlend
		{
			set { SetArgument("spineStumbleWritheBlend", value); }
		}

		public float LegsStumbleWritheBlend
		{
			set { SetArgument("legsStumbleWritheBlend", value); }
		}

		public float ArmsPoseWritheBlend
		{
			set { SetArgument("armsPoseWritheBlend", value); }
		}

		public float SpinePoseWritheBlend
		{
			set { SetArgument("spinePoseWritheBlend", value); }
		}

		public float LegsPoseWritheBlend
		{
			set { SetArgument("legsPoseWritheBlend", value); }
		}

		public bool RollOverFlag
		{
			set { SetArgument("rollOverFlag", value); }
		}

		public float RollTorqueScale
		{
			set { SetArgument("rollTorqueScale", value); }
		}

		public float PredictTime
		{
			set { SetArgument("predictTime", value); }
		}

		public float MaxRollOverTime
		{
			set { SetArgument("maxRollOverTime", value); }
		}

		public float RollOverRadius
		{
			set { SetArgument("rollOverRadius", value); }
		}
	}

	public sealed class PedalLegsHelper : CustomHelper
	{
		public PedalLegsHelper(Ped ped) : base(ped, "pedalLegs")
		{
		}

		public bool PedalLeftLeg
		{
			set { SetArgument("pedalLeftLeg", value); }
		}

		public bool PedalRightLeg
		{
			set { SetArgument("pedalRightLeg", value); }
		}

		public bool BackPedal
		{
			set { SetArgument("backPedal", value); }
		}

		public float Radius
		{
			set { SetArgument("radius", value); }
		}

		public float AngularSpeed
		{
			set { SetArgument("angularSpeed", value); }
		}

		public float LegStiffness
		{
			set { SetArgument("legStiffness", value); }
		}

		public float PedalOffset
		{
			set { SetArgument("pedalOffset", value); }
		}

		public int RandomSeed
		{
			set { SetArgument("randomSeed", value); }
		}

		public float SpeedAsymmetry
		{
			set { SetArgument("speedAsymmetry", value); }
		}

		public bool AdaptivePedal4Dragging
		{
			set { SetArgument("adaptivePedal4Dragging", value); }
		}

		public float AngSpeedMultiplier4Dragging
		{
			set { SetArgument("angSpeedMultiplier4Dragging", value); }
		}

		public float RadiusVariance
		{
			set { SetArgument("radiusVariance", value); }
		}

		public float LegAngleVariance
		{
			set { SetArgument("legAngleVariance", value); }
		}

		public float CentreSideways
		{
			set { SetArgument("centreSideways", value); }
		}

		public float CentreForwards
		{
			set { SetArgument("centreForwards", value); }
		}

		public float CentreUp
		{
			set { SetArgument("centreUp", value); }
		}

		public float Ellipse
		{
			set { SetArgument("ellipse", value); }
		}

		public float DragReduction
		{
			set { SetArgument("dragReduction", value); }
		}

		public float Spread
		{
			set { SetArgument("spread", value); }
		}

		public bool Hula
		{
			set { SetArgument("hula", value); }
		}
	}

	public sealed class PointArmHelper : CustomHelper
	{
		public PointArmHelper(Ped ped) : base(ped, "pointArm")
		{
		}

		public Vector3 TargetLeft
		{
			set { SetArgument("targetLeft", value); }
		}

		public float TwistLeft
		{
			set { SetArgument("twistLeft", value); }
		}

		public float ArmStraightnessLeft
		{
			set { SetArgument("armStraightnessLeft", value); }
		}

		public bool UseLeftArm
		{
			set { SetArgument("useLeftArm", value); }
		}

		public float ArmStiffnessLeft
		{
			set { SetArgument("armStiffnessLeft", value); }
		}

		public float ArmDampingLeft
		{
			set { SetArgument("armDampingLeft", value); }
		}

		public int InstanceIndexLeft
		{
			set { SetArgument("instanceIndexLeft", value); }
		}

		public float PointSwingLimitLeft
		{
			set { SetArgument("pointSwingLimitLeft", value); }
		}

		public bool UseZeroPoseWhenNotPointingLeft
		{
			set { SetArgument("useZeroPoseWhenNotPointingLeft", value); }
		}

		public Vector3 TargetRight
		{
			set { SetArgument("targetRight", value); }
		}

		public float TwistRight
		{
			set { SetArgument("twistRight", value); }
		}

		public float ArmStraightnessRight
		{
			set { SetArgument("armStraightnessRight", value); }
		}

		public bool UseRightArm
		{
			set { SetArgument("useRightArm", value); }
		}

		public float ArmStiffnessRight
		{
			set { SetArgument("armStiffnessRight", value); }
		}

		public float ArmDampingRight
		{
			set { SetArgument("armDampingRight", value); }
		}

		public int InstanceIndexRight
		{
			set { SetArgument("instanceIndexRight", value); }
		}

		public float PointSwingLimitRight
		{
			set { SetArgument("pointSwingLimitRight", value); }
		}

		public bool UseZeroPoseWhenNotPointingRight
		{
			set { SetArgument("useZeroPoseWhenNotPointingRight", value); }
		}
	}

	public sealed class PointGunHelper : CustomHelper
	{
		public PointGunHelper(Ped ped) : base(ped, "pointGun")
		{
		}

		public bool EnableRight
		{
			set { SetArgument("enableRight", value); }
		}

		public bool EnableLeft
		{
			set { SetArgument("enableLeft", value); }
		}

		public Vector3 LeftHandTarget
		{
			set { SetArgument("leftHandTarget", value); }
		}

		public int LeftHandTargetIndex
		{
			set { SetArgument("leftHandTargetIndex", value); }
		}

		public Vector3 RightHandTarget
		{
			set { SetArgument("rightHandTarget", value); }
		}

		public int RightHandTargetIndex
		{
			set { SetArgument("rightHandTargetIndex", value); }
		}

		public float LeadTarget
		{
			set { SetArgument("leadTarget", value); }
		}

		public float ArmStiffness
		{
			set { SetArgument("armStiffness", value); }
		}

		public float ArmStiffnessDetSupport
		{
			set { SetArgument("armStiffnessDetSupport", value); }
		}

		public float ArmDamping
		{
			set { SetArgument("armDamping", value); }
		}

		public float GravityOpposition
		{
			set { SetArgument("gravityOpposition", value); }
		}

		public float GravOppDetachedSupport
		{
			set { SetArgument("gravOppDetachedSupport", value); }
		}

		public float MassMultDetachedSupport
		{
			set { SetArgument("massMultDetachedSupport", value); }
		}

		public bool AllowShotLooseness
		{
			set { SetArgument("allowShotLooseness", value); }
		}

		public float ClavicleBlend
		{
			set { SetArgument("clavicleBlend", value); }
		}

		public float ElbowAttitude
		{
			set { SetArgument("elbowAttitude", value); }
		}

		public int SupportConstraint
		{
			set { SetArgument("supportConstraint", value); }
		}

		public float ConstraintMinDistance
		{
			set { SetArgument("constraintMinDistance", value); }
		}

		public float MakeConstraintDistance
		{
			set { SetArgument("makeConstraintDistance", value); }
		}

		public float ReduceConstraintLengthVel
		{
			set { SetArgument("reduceConstraintLengthVel", value); }
		}

		public float BreakingStrength
		{
			set { SetArgument("breakingStrength", value); }
		}

		public float BrokenSupportTime
		{
			set { SetArgument("brokenSupportTime", value); }
		}

		public float BrokenToSideProb
		{
			set { SetArgument("brokenToSideProb", value); }
		}

		public float ConnectAfter
		{
			set { SetArgument("connectAfter", value); }
		}

		public float ConnectFor
		{
			set { SetArgument("connectFor", value); }
		}

		public int OneHandedPointing
		{
			set { SetArgument("oneHandedPointing", value); }
		}

		public bool AlwaysSupport
		{
			set { SetArgument("alwaysSupport", value); }
		}

		public bool PoseUnusedGunArm
		{
			set { SetArgument("poseUnusedGunArm", value); }
		}

		public bool PoseUnusedSupportArm
		{
			set { SetArgument("poseUnusedSupportArm", value); }
		}

		public bool PoseUnusedOtherArm
		{
			set { SetArgument("poseUnusedOtherArm", value); }
		}

		public float MaxAngleAcross
		{
			set { SetArgument("maxAngleAcross", value); }
		}

		public float MaxAngleAway
		{
			set { SetArgument("maxAngleAway", value); }
		}

		public int FallingLimits
		{
			set { SetArgument("fallingLimits", value); }
		}

		public float AcrossLimit
		{
			set { SetArgument("acrossLimit", value); }
		}

		public float AwayLimit
		{
			set { SetArgument("awayLimit", value); }
		}

		public float UpLimit
		{
			set { SetArgument("upLimit", value); }
		}

		public float DownLimit
		{
			set { SetArgument("downLimit", value); }
		}

		public int RifleFall
		{
			set { SetArgument("rifleFall", value); }
		}

		public int FallingSupport
		{
			set { SetArgument("fallingSupport", value); }
		}

		public int FallingTypeSupport
		{
			set { SetArgument("fallingTypeSupport", value); }
		}

		public int PistolNeutralType
		{
			set { SetArgument("pistolNeutralType", value); }
		}

		public bool NeutralPoint4Pistols
		{
			set { SetArgument("neutralPoint4Pistols", value); }
		}

		public bool NeutralPoint4Rifle
		{
			set { SetArgument("neutralPoint4Rifle", value); }
		}

		public bool CheckNeutralPoint
		{
			set { SetArgument("checkNeutralPoint", value); }
		}

		public Vector3 Point2Side
		{
			set { SetArgument("point2Side", value); }
		}

		public float Add2WeaponDistSide
		{
			set { SetArgument("add2WeaponDistSide", value); }
		}

		public Vector3 Point2Connect
		{
			set { SetArgument("point2Connect", value); }
		}

		public float Add2WeaponDistConnect
		{
			set { SetArgument("add2WeaponDistConnect", value); }
		}

		public bool UsePistolIK
		{
			set { SetArgument("usePistolIK", value); }
		}

		public bool UseSpineTwist
		{
			set { SetArgument("useSpineTwist", value); }
		}

		public bool UseTurnToTarget
		{
			set { SetArgument("useTurnToTarget", value); }
		}

		public bool UseHeadLook
		{
			set { SetArgument("useHeadLook", value); }
		}

		public float ErrorThreshold
		{
			set { SetArgument("errorThreshold", value); }
		}

		public float FireWeaponRelaxTime
		{
			set { SetArgument("fireWeaponRelaxTime", value); }
		}

		public float FireWeaponRelaxAmount
		{
			set { SetArgument("fireWeaponRelaxAmount", value); }
		}

		public float FireWeaponRelaxDistance
		{
			set { SetArgument("fireWeaponRelaxDistance", value); }
		}

		public bool UseIncomingTransforms
		{
			set { SetArgument("useIncomingTransforms", value); }
		}

		public bool MeasureParentOffset
		{
			set { SetArgument("measureParentOffset", value); }
		}

		public Vector3 LeftHandParentOffset
		{
			set { SetArgument("leftHandParentOffset", value); }
		}

		public int LeftHandParentEffector
		{
			set { SetArgument("leftHandParentEffector", value); }
		}

		public Vector3 RightHandParentOffset
		{
			set { SetArgument("rightHandParentOffset", value); }
		}

		public int RightHandParentEffector
		{
			set { SetArgument("rightHandParentEffector", value); }
		}

		public float PrimaryHandWeaponDistance
		{
			set { SetArgument("primaryHandWeaponDistance", value); }
		}

		public bool ConstrainRifle
		{
			set { SetArgument("constrainRifle", value); }
		}

		public float RifleConstraintMinDistance
		{
			set { SetArgument("rifleConstraintMinDistance", value); }
		}

		public bool DisableArmCollisions
		{
			set { SetArgument("disableArmCollisions", value); }
		}

		public bool DisableRifleCollisions
		{
			set { SetArgument("disableRifleCollisions", value); }
		}
	}

	public sealed class PointGunExtraHelper : CustomHelper
	{
		public PointGunExtraHelper(Ped ped) : base(ped, "pointGunExtra")
		{
		}

		public float ConstraintStrength
		{
			set { SetArgument("constraintStrength", value); }
		}

		public float ConstraintThresh
		{
			set { SetArgument("constraintThresh", value); }
		}

		public int WeaponMask
		{
			set { SetArgument("weaponMask", value); }
		}

		public bool TimeWarpActive
		{
			set { SetArgument("timeWarpActive", value); }
		}

		public float TimeWarpStrengthScale
		{
			set { SetArgument("timeWarpStrengthScale", value); }
		}

		public float OriStiff
		{
			set { SetArgument("oriStiff", value); }
		}

		public float OriDamp
		{
			set { SetArgument("oriDamp", value); }
		}

		public float PosStiff
		{
			set { SetArgument("posStiff", value); }
		}

		public float PosDamp
		{
			set { SetArgument("posDamp", value); }
		}
	}

	public sealed class RollDownStairsHelper : CustomHelper
	{
		public RollDownStairsHelper(Ped ped) : base(ped, "rollDownStairs")
		{
		}

		public float Stiffness
		{
			set { SetArgument("stiffness", value); }
		}

		public float Damping
		{
			set { SetArgument("damping", value); }
		}

		public float Forcemag
		{
			set { SetArgument("forcemag", value); }
		}

		public float M_useArmToSlowDown
		{
			set { SetArgument("m_useArmToSlowDown", value); }
		}

		public bool UseZeroPose
		{
			set { SetArgument("useZeroPose", value); }
		}

		public bool SpinWhenInAir
		{
			set { SetArgument("spinWhenInAir", value); }
		}

		public float M_armReachAmount
		{
			set { SetArgument("m_armReachAmount", value); }
		}

		public float M_legPush
		{
			set { SetArgument("m_legPush", value); }
		}

		public bool TryToAvoidHeadButtingGround
		{
			set { SetArgument("tryToAvoidHeadButtingGround", value); }
		}

		public float ArmReachLength
		{
			set { SetArgument("armReachLength", value); }
		}

		public Vector3 CustomRollDir
		{
			set { SetArgument("customRollDir", value); }
		}

		public bool UseCustomRollDir
		{
			set { SetArgument("useCustomRollDir", value); }
		}

		public float StiffnessDecayTarget
		{
			set { SetArgument("stiffnessDecayTarget", value); }
		}

		public float StiffnessDecayTime
		{
			set { SetArgument("stiffnessDecayTime", value); }
		}

		public float AsymmetricalLegs
		{
			set { SetArgument("asymmetricalLegs", value); }
		}

		public float ZAxisSpinReduction
		{
			set { SetArgument("zAxisSpinReduction", value); }
		}

		public float TargetLinearVelocityDecayTime
		{
			set { SetArgument("targetLinearVelocityDecayTime", value); }
		}

		public float TargetLinearVelocity
		{
			set { SetArgument("targetLinearVelocity", value); }
		}

		public bool OnlyApplyHelperForces
		{
			set { SetArgument("onlyApplyHelperForces", value); }
		}

		public bool UseVelocityOfObjectBelow
		{
			set { SetArgument("useVelocityOfObjectBelow", value); }
		}

		public bool UseRelativeVelocity
		{
			set { SetArgument("useRelativeVelocity", value); }
		}

		public bool ApplyFoetalToLegs
		{
			set { SetArgument("applyFoetalToLegs", value); }
		}

		public float MovementLegsInFoetalPosition
		{
			set { SetArgument("movementLegsInFoetalPosition", value); }
		}

		public float MaxAngVelAroundFrontwardAxis
		{
			set { SetArgument("maxAngVelAroundFrontwardAxis", value); }
		}

		public float MinAngVel
		{
			set { SetArgument("minAngVel", value); }
		}

		public bool ApplyNewRollingCheatingTorques
		{
			set { SetArgument("applyNewRollingCheatingTorques", value); }
		}

		public float MaxAngVel
		{
			set { SetArgument("maxAngVel", value); }
		}

		public float MagOfTorqueToRoll
		{
			set { SetArgument("magOfTorqueToRoll", value); }
		}

		public bool ApplyHelPerTorqueToAlign
		{
			set { SetArgument("applyHelPerTorqueToAlign", value); }
		}

		public float DelayToAlignBody
		{
			set { SetArgument("delayToAlignBody", value); }
		}

		public float MagOfTorqueToAlign
		{
			set { SetArgument("magOfTorqueToAlign", value); }
		}

		public float AirborneReduction
		{
			set { SetArgument("airborneReduction", value); }
		}

		public bool ApplyMinMaxFriction
		{
			set { SetArgument("applyMinMaxFriction", value); }
		}

		public bool LimitSpinReduction
		{
			set { SetArgument("limitSpinReduction", value); }
		}
	}

	public sealed class ShotHelper : CustomHelper
	{
		public ShotHelper(Ped ped) : base(ped, "shot")
		{
		}

		public float BodyStiffness
		{
			set { SetArgument("bodyStiffness", value); }
		}

		public float SpineDamping
		{
			set { SetArgument("spineDamping", value); }
		}

		public float ArmStiffness
		{
			set { SetArgument("armStiffness", value); }
		}

		public float InitialNeckStiffness
		{
			set { SetArgument("initialNeckStiffness", value); }
		}

		public float InitialNeckDamping
		{
			set { SetArgument("initialNeckDamping", value); }
		}

		public float NeckStiffness
		{
			set { SetArgument("neckStiffness", value); }
		}

		public float NeckDamping
		{
			set { SetArgument("neckDamping", value); }
		}

		public float KMultOnLoose
		{
			set { SetArgument("kMultOnLoose", value); }
		}

		public float KMult4Legs
		{
			set { SetArgument("kMult4Legs", value); }
		}

		public float LoosenessAmount
		{
			set { SetArgument("loosenessAmount", value); }
		}

		public float Looseness4Fall
		{
			set { SetArgument("looseness4Fall", value); }
		}

		public float Looseness4Stagger
		{
			set { SetArgument("looseness4Stagger", value); }
		}

		public float MinArmsLooseness
		{
			set { SetArgument("minArmsLooseness", value); }
		}

		public float MinLegsLooseness
		{
			set { SetArgument("minLegsLooseness", value); }
		}

		public float GrabHoldTime
		{
			set { SetArgument("grabHoldTime", value); }
		}

		public bool SpineBlendExagCPain
		{
			set { SetArgument("spineBlendExagCPain", value); }
		}

		public float SpineBlendZero
		{
			set { SetArgument("spineBlendZero", value); }
		}

		public bool BulletProofVest
		{
			set { SetArgument("bulletProofVest", value); }
		}

		public bool AlwaysResetLooseness
		{
			set { SetArgument("alwaysResetLooseness", value); }
		}

		public bool AlwaysResetNeckLooseness
		{
			set { SetArgument("alwaysResetNeckLooseness", value); }
		}

		public float AngVelScale
		{
			set { SetArgument("angVelScale", value); }
		}

		public string AngVelScaleMask
		{
			set { SetArgument("angVelScaleMask", value); }
		}

		public float FlingWidth
		{
			set { SetArgument("flingWidth", value); }
		}

		public float FlingTime
		{
			set { SetArgument("flingTime", value); }
		}

		public float TimeBeforeReachForWound
		{
			set { SetArgument("timeBeforeReachForWound", value); }
		}

		public float ExagDuration
		{
			set { SetArgument("exagDuration", value); }
		}

		public float ExagMag
		{
			set { SetArgument("exagMag", value); }
		}

		public float ExagTwistMag
		{
			set { SetArgument("exagTwistMag", value); }
		}

		public float ExagSmooth2Zero
		{
			set { SetArgument("exagSmooth2Zero", value); }
		}

		public float ExagZeroTime
		{
			set { SetArgument("exagZeroTime", value); }
		}

		public float CpainSmooth2Time
		{
			set { SetArgument("cpainSmooth2Time", value); }
		}

		public float CpainDuration
		{
			set { SetArgument("cpainDuration", value); }
		}

		public float CpainMag
		{
			set { SetArgument("cpainMag", value); }
		}

		public float CpainTwistMag
		{
			set { SetArgument("cpainTwistMag", value); }
		}

		public float CpainSmooth2Zero
		{
			set { SetArgument("cpainSmooth2Zero", value); }
		}

		public bool Crouching
		{
			set { SetArgument("crouching", value); }
		}

		public bool ChickenArms
		{
			set { SetArgument("chickenArms", value); }
		}

		public bool ReachForWound
		{
			set { SetArgument("reachForWound", value); }
		}

		public bool Fling
		{
			set { SetArgument("fling", value); }
		}

		public bool AllowInjuredArm
		{
			set { SetArgument("allowInjuredArm", value); }
		}

		public bool AllowInjuredLeg
		{
			set { SetArgument("allowInjuredLeg", value); }
		}

		public bool AllowInjuredLowerLegReach
		{
			set { SetArgument("allowInjuredLowerLegReach", value); }
		}

		public bool AllowInjuredThighReach
		{
			set { SetArgument("allowInjuredThighReach", value); }
		}

		public bool StableHandsAndNeck
		{
			set { SetArgument("stableHandsAndNeck", value); }
		}

		public bool Melee
		{
			set { SetArgument("melee", value); }
		}

		public int FallingReaction
		{
			set { SetArgument("fallingReaction", value); }
		}

		public bool UseExtendedCatchFall
		{
			set { SetArgument("useExtendedCatchFall", value); }
		}

		public float InitialWeaknessZeroDuration
		{
			set { SetArgument("initialWeaknessZeroDuration", value); }
		}

		public float InitialWeaknessRampDuration
		{
			set { SetArgument("initialWeaknessRampDuration", value); }
		}

		public float InitialNeckDuration
		{
			set { SetArgument("initialNeckDuration", value); }
		}

		public float InitialNeckRampDuration
		{
			set { SetArgument("initialNeckRampDuration", value); }
		}

		public bool UseCStrModulation
		{
			set { SetArgument("useCStrModulation", value); }
		}

		public float CStrUpperMin
		{
			set { SetArgument("cStrUpperMin", value); }
		}

		public float CStrUpperMax
		{
			set { SetArgument("cStrUpperMax", value); }
		}

		public float CStrLowerMin
		{
			set { SetArgument("cStrLowerMin", value); }
		}

		public float CStrLowerMax
		{
			set { SetArgument("cStrLowerMax", value); }
		}

		public float DeathTime
		{
			set { SetArgument("deathTime", value); }
		}
	}

	public sealed class ShotNewBulletHelper : CustomHelper
	{
		public ShotNewBulletHelper(Ped ped) : base(ped, "shotNewBullet")
		{
		}

		public int BodyPart
		{
			set { SetArgument("bodyPart", value); }
		}

		public bool LocalHitPointInfo
		{
			set { SetArgument("localHitPointInfo", value); }
		}

		public Vector3 Normal
		{
			set { SetArgument("normal", value); }
		}

		public Vector3 HitPoint
		{
			set { SetArgument("hitPoint", value); }
		}

		public Vector3 BulletVel
		{
			set { SetArgument("bulletVel", value); }
		}
	}

	public sealed class ShotSnapHelper : CustomHelper
	{
		public ShotSnapHelper(Ped ped) : base(ped, "shotSnap")
		{
		}

		public bool Snap
		{
			set { SetArgument("snap", value); }
		}

		public float SnapMag
		{
			set { SetArgument("snapMag", value); }
		}

		public float SnapMovingMult
		{
			set { SetArgument("snapMovingMult", value); }
		}

		public float SnapBalancingMult
		{
			set { SetArgument("snapBalancingMult", value); }
		}

		public float SnapAirborneMult
		{
			set { SetArgument("snapAirborneMult", value); }
		}

		public float SnapMovingThresh
		{
			set { SetArgument("snapMovingThresh", value); }
		}

		public float SnapDirectionRandomness
		{
			set { SetArgument("snapDirectionRandomness", value); }
		}

		public bool SnapLeftArm
		{
			set { SetArgument("snapLeftArm", value); }
		}

		public bool SnapRightArm
		{
			set { SetArgument("snapRightArm", value); }
		}

		public bool SnapLeftLeg
		{
			set { SetArgument("snapLeftLeg", value); }
		}

		public bool SnapRightLeg
		{
			set { SetArgument("snapRightLeg", value); }
		}

		public bool SnapSpine
		{
			set { SetArgument("snapSpine", value); }
		}

		public bool SnapNeck
		{
			set { SetArgument("snapNeck", value); }
		}

		public bool SnapPhasedLegs
		{
			set { SetArgument("snapPhasedLegs", value); }
		}

		public int SnapHipType
		{
			set { SetArgument("snapHipType", value); }
		}

		public bool SnapUseBulletDir
		{
			set { SetArgument("snapUseBulletDir", value); }
		}

		public bool SnapHitPart
		{
			set { SetArgument("snapHitPart", value); }
		}

		public float UnSnapInterval
		{
			set { SetArgument("unSnapInterval", value); }
		}

		public float UnSnapRatio
		{
			set { SetArgument("unSnapRatio", value); }
		}

		public bool SnapUseTorques
		{
			set { SetArgument("snapUseTorques", value); }
		}
	}

	public sealed class ShotShockSpinHelper : CustomHelper
	{
		public ShotShockSpinHelper(Ped ped) : base(ped, "shotShockSpin")
		{
		}

		public bool AddShockSpin
		{
			set { SetArgument("addShockSpin", value); }
		}

		public bool RandomizeShockSpinDirection
		{
			set { SetArgument("randomizeShockSpinDirection", value); }
		}

		public bool AlwaysAddShockSpin
		{
			set { SetArgument("alwaysAddShockSpin", value); }
		}

		public float ShockSpinMin
		{
			set { SetArgument("shockSpinMin", value); }
		}

		public float ShockSpinMax
		{
			set { SetArgument("shockSpinMax", value); }
		}

		public float ShockSpinLiftForceMult
		{
			set { SetArgument("shockSpinLiftForceMult", value); }
		}

		public float ShockSpinDecayMult
		{
			set { SetArgument("shockSpinDecayMult", value); }
		}

		public float ShockSpinScalePerComponent
		{
			set { SetArgument("shockSpinScalePerComponent", value); }
		}

		public float ShockSpinMaxTwistVel
		{
			set { SetArgument("shockSpinMaxTwistVel", value); }
		}

		public bool ShockSpinScaleByLeverArm
		{
			set { SetArgument("shockSpinScaleByLeverArm", value); }
		}

		public float ShockSpinAirMult
		{
			set { SetArgument("shockSpinAirMult", value); }
		}

		public float ShockSpin1FootMult
		{
			set { SetArgument("shockSpin1FootMult", value); }
		}

		public float ShockSpinFootGripMult
		{
			set { SetArgument("shockSpinFootGripMult", value); }
		}

		public float BracedSideSpinMult
		{
			set { SetArgument("bracedSideSpinMult", value); }
		}
	}

	public sealed class ShotFallToKneesHelper : CustomHelper
	{
		public ShotFallToKneesHelper(Ped ped) : base(ped, "shotFallToKnees")
		{
		}

		public bool FallToKnees
		{
			set { SetArgument("fallToKnees", value); }
		}

		public bool FtkAlwaysChangeFall
		{
			set { SetArgument("ftkAlwaysChangeFall", value); }
		}

		public float FtkBalanceTime
		{
			set { SetArgument("ftkBalanceTime", value); }
		}

		public float FtkHelperForce
		{
			set { SetArgument("ftkHelperForce", value); }
		}

		public bool FtkHelperForceOnSpine
		{
			set { SetArgument("ftkHelperForceOnSpine", value); }
		}

		public float FtkLeanHelp
		{
			set { SetArgument("ftkLeanHelp", value); }
		}

		public float FtkSpineBend
		{
			set { SetArgument("ftkSpineBend", value); }
		}

		public bool FtkStiffSpine
		{
			set { SetArgument("ftkStiffSpine", value); }
		}

		public float FtkImpactLooseness
		{
			set { SetArgument("ftkImpactLooseness", value); }
		}

		public float FtkImpactLoosenessTime
		{
			set { SetArgument("ftkImpactLoosenessTime", value); }
		}

		public float FtkBendRate
		{
			set { SetArgument("ftkBendRate", value); }
		}

		public float FtkHipBlend
		{
			set { SetArgument("ftkHipBlend", value); }
		}

		public float FtkLungeProb
		{
			set { SetArgument("ftkLungeProb", value); }
		}

		public bool FtkKneeSpin
		{
			set { SetArgument("ftkKneeSpin", value); }
		}

		public float FtkFricMult
		{
			set { SetArgument("ftkFricMult", value); }
		}

		public float FtkHipAngleFall
		{
			set { SetArgument("ftkHipAngleFall", value); }
		}

		public float FtkPitchForwards
		{
			set { SetArgument("ftkPitchForwards", value); }
		}

		public float FtkPitchBackwards
		{
			set { SetArgument("ftkPitchBackwards", value); }
		}

		public float FtkFallBelowStab
		{
			set { SetArgument("ftkFallBelowStab", value); }
		}

		public float FtkBalanceAbortThreshold
		{
			set { SetArgument("ftkBalanceAbortThreshold", value); }
		}

		public int FtkOnKneesArmType
		{
			set { SetArgument("ftkOnKneesArmType", value); }
		}

		public float FtkReleaseReachForWound
		{
			set { SetArgument("ftkReleaseReachForWound", value); }
		}

		public bool FtkReachForWound
		{
			set { SetArgument("ftkReachForWound", value); }
		}

		public bool FtkReleasePointGun
		{
			set { SetArgument("ftkReleasePointGun", value); }
		}

		public bool FtkFailMustCollide
		{
			set { SetArgument("ftkFailMustCollide", value); }
		}
	}

	public sealed class ShotFromBehindHelper : CustomHelper
	{
		public ShotFromBehindHelper(Ped ped) : base(ped, "shotFromBehind")
		{
		}

		public bool ShotFromBehind
		{
			set { SetArgument("shotFromBehind", value); }
		}

		public float SfbSpineAmount
		{
			set { SetArgument("sfbSpineAmount", value); }
		}

		public float SfbNeckAmount
		{
			set { SetArgument("sfbNeckAmount", value); }
		}

		public float SfbHipAmount
		{
			set { SetArgument("sfbHipAmount", value); }
		}

		public float SfbKneeAmount
		{
			set { SetArgument("sfbKneeAmount", value); }
		}

		public float SfbPeriod
		{
			set { SetArgument("sfbPeriod", value); }
		}

		public float SfbForceBalancePeriod
		{
			set { SetArgument("sfbForceBalancePeriod", value); }
		}

		public float SfbArmsOnset
		{
			set { SetArgument("sfbArmsOnset", value); }
		}

		public float SfbKneesOnset
		{
			set { SetArgument("sfbKneesOnset", value); }
		}

		public float SfbNoiseGain
		{
			set { SetArgument("sfbNoiseGain", value); }
		}

		public int SfbIgnoreFail
		{
			set { SetArgument("sfbIgnoreFail", value); }
		}
	}

	public sealed class ShotInGutsHelper : CustomHelper
	{
		public ShotInGutsHelper(Ped ped) : base(ped, "shotInGuts")
		{
		}

		public bool ShotInGuts
		{
			set { SetArgument("shotInGuts", value); }
		}

		public float SigSpineAmount
		{
			set { SetArgument("sigSpineAmount", value); }
		}

		public float SigNeckAmount
		{
			set { SetArgument("sigNeckAmount", value); }
		}

		public float SigHipAmount
		{
			set { SetArgument("sigHipAmount", value); }
		}

		public float SigKneeAmount
		{
			set { SetArgument("sigKneeAmount", value); }
		}

		public float SigPeriod
		{
			set { SetArgument("sigPeriod", value); }
		}

		public float SigForceBalancePeriod
		{
			set { SetArgument("sigForceBalancePeriod", value); }
		}

		public float SigKneesOnset
		{
			set { SetArgument("sigKneesOnset", value); }
		}
	}

	public sealed class ShotHeadLookHelper : CustomHelper
	{
		public ShotHeadLookHelper(Ped ped) : base(ped, "shotHeadLook")
		{
		}

		public bool UseHeadLook
		{
			set { SetArgument("useHeadLook", value); }
		}

		public Vector3 HeadLook
		{
			set { SetArgument("headLook", value); }
		}

		public float HeadLookAtWoundMinTimer
		{
			set { SetArgument("headLookAtWoundMinTimer", value); }
		}

		public float HeadLookAtWoundMaxTimer
		{
			set { SetArgument("headLookAtWoundMaxTimer", value); }
		}

		public float HeadLookAtHeadPosMaxTimer
		{
			set { SetArgument("headLookAtHeadPosMaxTimer", value); }
		}

		public float HeadLookAtHeadPosMinTimer
		{
			set { SetArgument("headLookAtHeadPosMinTimer", value); }
		}
	}

	public sealed class ShotConfigureArmsHelper : CustomHelper
	{
		public ShotConfigureArmsHelper(Ped ped) : base(ped, "shotConfigureArms")
		{
		}

		public bool Brace
		{
			set { SetArgument("brace", value); }
		}

		public bool PointGun
		{
			set { SetArgument("pointGun", value); }
		}

		public bool UseArmsWindmill
		{
			set { SetArgument("useArmsWindmill", value); }
		}

		public int ReleaseWound
		{
			set { SetArgument("releaseWound", value); }
		}

		public int ReachFalling
		{
			set { SetArgument("reachFalling", value); }
		}

		public int ReachFallingWithOneHand
		{
			set { SetArgument("reachFallingWithOneHand", value); }
		}

		public int ReachOnFloor
		{
			set { SetArgument("reachOnFloor", value); }
		}

		public float AlwaysReachTime
		{
			set { SetArgument("alwaysReachTime", value); }
		}

		public float AWSpeedMult
		{
			set { SetArgument("AWSpeedMult", value); }
		}

		public float AWRadiusMult
		{
			set { SetArgument("AWRadiusMult", value); }
		}

		public float AWStiffnessAdd
		{
			set { SetArgument("AWStiffnessAdd", value); }
		}

		public int ReachWithOneHand
		{
			set { SetArgument("reachWithOneHand", value); }
		}

		public bool AllowLeftPistolRFW
		{
			set { SetArgument("allowLeftPistolRFW", value); }
		}

		public bool AllowRightPistolRFW
		{
			set { SetArgument("allowRightPistolRFW", value); }
		}

		public bool RfwWithPistol
		{
			set { SetArgument("rfwWithPistol", value); }
		}

		public bool Fling2
		{
			set { SetArgument("fling2", value); }
		}

		public bool Fling2Left
		{
			set { SetArgument("fling2Left", value); }
		}

		public bool Fling2Right
		{
			set { SetArgument("fling2Right", value); }
		}

		public bool Fling2OverrideStagger
		{
			set { SetArgument("fling2OverrideStagger", value); }
		}

		public float Fling2TimeBefore
		{
			set { SetArgument("fling2TimeBefore", value); }
		}

		public float Fling2Time
		{
			set { SetArgument("fling2Time", value); }
		}

		public float Fling2MStiffL
		{
			set { SetArgument("fling2MStiffL", value); }
		}

		public float Fling2MStiffR
		{
			set { SetArgument("fling2MStiffR", value); }
		}

		public float Fling2RelaxTimeL
		{
			set { SetArgument("fling2RelaxTimeL", value); }
		}

		public float Fling2RelaxTimeR
		{
			set { SetArgument("fling2RelaxTimeR", value); }
		}

		public float Fling2AngleMinL
		{
			set { SetArgument("fling2AngleMinL", value); }
		}

		public float Fling2AngleMaxL
		{
			set { SetArgument("fling2AngleMaxL", value); }
		}

		public float Fling2AngleMinR
		{
			set { SetArgument("fling2AngleMinR", value); }
		}

		public float Fling2AngleMaxR
		{
			set { SetArgument("fling2AngleMaxR", value); }
		}

		public float Fling2LengthMinL
		{
			set { SetArgument("fling2LengthMinL", value); }
		}

		public float Fling2LengthMaxL
		{
			set { SetArgument("fling2LengthMaxL", value); }
		}

		public float Fling2LengthMinR
		{
			set { SetArgument("fling2LengthMinR", value); }
		}

		public float Fling2LengthMaxR
		{
			set { SetArgument("fling2LengthMaxR", value); }
		}

		public bool Bust
		{
			set { SetArgument("bust", value); }
		}

		public float BustElbowLift
		{
			set { SetArgument("bustElbowLift", value); }
		}

		public float CupSize
		{
			set { SetArgument("cupSize", value); }
		}

		public bool CupBust
		{
			set { SetArgument("cupBust", value); }
		}
	}

	public sealed class SmartFallHelper : CustomHelper
	{
		public SmartFallHelper(Ped ped) : base(ped, "smartFall")
		{
		}

		public float BodyStiffness
		{
			set { SetArgument("bodyStiffness", value); }
		}

		public float Bodydamping
		{
			set { SetArgument("bodydamping", value); }
		}

		public float Catchfalltime
		{
			set { SetArgument("catchfalltime", value); }
		}

		public float CrashOrLandCutOff
		{
			set { SetArgument("crashOrLandCutOff", value); }
		}

		public float PdStrength
		{
			set { SetArgument("pdStrength", value); }
		}

		public float PdDamping
		{
			set { SetArgument("pdDamping", value); }
		}

		public float ArmAngSpeed
		{
			set { SetArgument("armAngSpeed", value); }
		}

		public float ArmAmplitude
		{
			set { SetArgument("armAmplitude", value); }
		}

		public float ArmPhase
		{
			set { SetArgument("armPhase", value); }
		}

		public bool ArmBendElbows
		{
			set { SetArgument("armBendElbows", value); }
		}

		public float LegRadius
		{
			set { SetArgument("legRadius", value); }
		}

		public float LegAngSpeed
		{
			set { SetArgument("legAngSpeed", value); }
		}

		public float LegAsymmetry
		{
			set { SetArgument("legAsymmetry", value); }
		}

		public float Arms2LegsPhase
		{
			set { SetArgument("arms2LegsPhase", value); }
		}

		public int Arms2LegsSync
		{
			set { SetArgument("arms2LegsSync", value); }
		}

		public float ArmsUp
		{
			set { SetArgument("armsUp", value); }
		}

		public bool OrientateBodyToFallDirection
		{
			set { SetArgument("orientateBodyToFallDirection", value); }
		}

		public bool OrientateTwist
		{
			set { SetArgument("orientateTwist", value); }
		}

		public float OrientateMax
		{
			set { SetArgument("orientateMax", value); }
		}

		public bool AlanRickman
		{
			set { SetArgument("alanRickman", value); }
		}

		public bool FowardRoll
		{
			set { SetArgument("fowardRoll", value); }
		}

		public bool UseZeroPose_withFowardRoll
		{
			set { SetArgument("useZeroPose_withFowardRoll", value); }
		}

		public float AimAngleBase
		{
			set { SetArgument("aimAngleBase", value); }
		}

		public float FowardVelRotation
		{
			set { SetArgument("fowardVelRotation", value); }
		}

		public float FootVelCompScale
		{
			set { SetArgument("footVelCompScale", value); }
		}

		public float SideD
		{
			set { SetArgument("sideD", value); }
		}

		public float FowardOffsetOfLegIK
		{
			set { SetArgument("fowardOffsetOfLegIK", value); }
		}

		public float LegL
		{
			set { SetArgument("legL", value); }
		}

		public float CatchFallCutOff
		{
			set { SetArgument("catchFallCutOff", value); }
		}

		public float LegStrength
		{
			set { SetArgument("legStrength", value); }
		}

		public bool Balance
		{
			set { SetArgument("balance", value); }
		}

		public bool IgnorWorldCollisions
		{
			set { SetArgument("ignorWorldCollisions", value); }
		}

		public bool AdaptiveCircling
		{
			set { SetArgument("adaptiveCircling", value); }
		}

		public bool Hula
		{
			set { SetArgument("hula", value); }
		}

		public float MaxSpeedForRecoverableFall
		{
			set { SetArgument("maxSpeedForRecoverableFall", value); }
		}

		public float MinSpeedForBrace
		{
			set { SetArgument("minSpeedForBrace", value); }
		}

		public float LandingNormal
		{
			set { SetArgument("landingNormal", value); }
		}

		public float RdsForceMag
		{
			set { SetArgument("rdsForceMag", value); }
		}

		public float RdsTargetLinVeDecayTime
		{
			set { SetArgument("rdsTargetLinVeDecayTime", value); }
		}

		public float RdsTargetLinearVelocity
		{
			set { SetArgument("rdsTargetLinearVelocity", value); }
		}

		public bool RdsUseStartingFriction
		{
			set { SetArgument("rdsUseStartingFriction", value); }
		}

		public float RdsStartingFriction
		{
			set { SetArgument("rdsStartingFriction", value); }
		}

		public float RdsStartingFrictionMin
		{
			set { SetArgument("rdsStartingFrictionMin", value); }
		}

		public float RdsForceVelThreshold
		{
			set { SetArgument("rdsForceVelThreshold", value); }
		}

		public int InitialState
		{
			set { SetArgument("initialState", value); }
		}

		public bool ChangeExtremityFriction
		{
			set { SetArgument("changeExtremityFriction", value); }
		}

		public bool Teeter
		{
			set { SetArgument("teeter", value); }
		}

		public float TeeterOffset
		{
			set { SetArgument("teeterOffset", value); }
		}

		public float StopRollingTime
		{
			set { SetArgument("stopRollingTime", value); }
		}

		public float ReboundScale
		{
			set { SetArgument("reboundScale", value); }
		}

		public string ReboundMask
		{
			set { SetArgument("reboundMask", value); }
		}

		public bool ForceHeadAvoid
		{
			set { SetArgument("forceHeadAvoid", value); }
		}

		public float CfZAxisSpinReduction
		{
			set { SetArgument("cfZAxisSpinReduction", value); }
		}

		public float SplatWhenStopped
		{
			set { SetArgument("splatWhenStopped", value); }
		}

		public float BlendHeadWhenStopped
		{
			set { SetArgument("blendHeadWhenStopped", value); }
		}

		public float SpreadLegs
		{
			set { SetArgument("spreadLegs", value); }
		}
	}

	public sealed class StaggerFallHelper : CustomHelper
	{
		public StaggerFallHelper(Ped ped) : base(ped, "staggerFall")
		{
		}

		public float ArmStiffness
		{
			set { SetArgument("armStiffness", value); }
		}

		public float ArmDamping
		{
			set { SetArgument("armDamping", value); }
		}

		public float SpineDamping
		{
			set { SetArgument("spineDamping", value); }
		}

		public float SpineStiffness
		{
			set { SetArgument("spineStiffness", value); }
		}

		public float ArmStiffnessStart
		{
			set { SetArgument("armStiffnessStart", value); }
		}

		public float ArmDampingStart
		{
			set { SetArgument("armDampingStart", value); }
		}

		public float SpineDampingStart
		{
			set { SetArgument("spineDampingStart", value); }
		}

		public float SpineStiffnessStart
		{
			set { SetArgument("spineStiffnessStart", value); }
		}

		public float TimeAtStartValues
		{
			set { SetArgument("timeAtStartValues", value); }
		}

		public float RampTimeFromStartValues
		{
			set { SetArgument("rampTimeFromStartValues", value); }
		}

		public float StaggerStepProb
		{
			set { SetArgument("staggerStepProb", value); }
		}

		public int StepsTillStartEnd
		{
			set { SetArgument("stepsTillStartEnd", value); }
		}

		public float TimeStartEnd
		{
			set { SetArgument("timeStartEnd", value); }
		}

		public float RampTimeToEndValues
		{
			set { SetArgument("rampTimeToEndValues", value); }
		}

		public float LowerBodyStiffness
		{
			set { SetArgument("lowerBodyStiffness", value); }
		}

		public float LowerBodyStiffnessEnd
		{
			set { SetArgument("lowerBodyStiffnessEnd", value); }
		}

		public float PredictionTime
		{
			set { SetArgument("predictionTime", value); }
		}

		public float PerStepReduction1
		{
			set { SetArgument("perStepReduction1", value); }
		}

		public float LeanInDirRate
		{
			set { SetArgument("leanInDirRate", value); }
		}

		public float LeanInDirMaxF
		{
			set { SetArgument("leanInDirMaxF", value); }
		}

		public float LeanInDirMaxB
		{
			set { SetArgument("leanInDirMaxB", value); }
		}

		public float LeanHipsMaxF
		{
			set { SetArgument("leanHipsMaxF", value); }
		}

		public float LeanHipsMaxB
		{
			set { SetArgument("leanHipsMaxB", value); }
		}

		public float Lean2multF
		{
			set { SetArgument("lean2multF", value); }
		}

		public float Lean2multB
		{
			set { SetArgument("lean2multB", value); }
		}

		public float PushOffDist
		{
			set { SetArgument("pushOffDist", value); }
		}

		public float MaxPushoffVel
		{
			set { SetArgument("maxPushoffVel", value); }
		}

		public float HipBendMult
		{
			set { SetArgument("hipBendMult", value); }
		}

		public bool AlwaysBendForwards
		{
			set { SetArgument("alwaysBendForwards", value); }
		}

		public float SpineBendMult
		{
			set { SetArgument("spineBendMult", value); }
		}

		public bool UseHeadLook
		{
			set { SetArgument("useHeadLook", value); }
		}

		public Vector3 HeadLookPos
		{
			set { SetArgument("headLookPos", value); }
		}

		public int HeadLookInstanceIndex
		{
			set { SetArgument("headLookInstanceIndex", value); }
		}

		public float HeadLookAtVelProb
		{
			set { SetArgument("headLookAtVelProb", value); }
		}

		public float TurnOffProb
		{
			set { SetArgument("turnOffProb", value); }
		}

		public float Turn2TargetProb
		{
			set { SetArgument("turn2TargetProb", value); }
		}

		public float Turn2VelProb
		{
			set { SetArgument("turn2VelProb", value); }
		}

		public float TurnAwayProb
		{
			set { SetArgument("turnAwayProb", value); }
		}

		public float TurnLeftProb
		{
			set { SetArgument("turnLeftProb", value); }
		}

		public float TurnRightProb
		{
			set { SetArgument("turnRightProb", value); }
		}

		public bool UseBodyTurn
		{
			set { SetArgument("useBodyTurn", value); }
		}

		public bool UpperBodyReaction
		{
			set { SetArgument("upperBodyReaction", value); }
		}
	}

	public sealed class TeeterHelper : CustomHelper
	{
		public TeeterHelper(Ped ped) : base(ped, "teeter")
		{
		}

		public Vector3 EdgeLeft
		{
			set { SetArgument("edgeLeft", value); }
		}

		public Vector3 EdgeRight
		{
			set { SetArgument("edgeRight", value); }
		}

		public bool UseExclusionZone
		{
			set { SetArgument("useExclusionZone", value); }
		}

		public bool UseHeadLook
		{
			set { SetArgument("useHeadLook", value); }
		}

		public bool CallHighFall
		{
			set { SetArgument("callHighFall", value); }
		}

		public bool LeanAway
		{
			set { SetArgument("leanAway", value); }
		}

		public float PreTeeterTime
		{
			set { SetArgument("preTeeterTime", value); }
		}

		public float LeanAwayTime
		{
			set { SetArgument("leanAwayTime", value); }
		}

		public float LeanAwayScale
		{
			set { SetArgument("leanAwayScale", value); }
		}

		public float TeeterTime
		{
			set { SetArgument("teeterTime", value); }
		}
	}

	public sealed class UpperBodyFlinchHelper : CustomHelper
	{
		public UpperBodyFlinchHelper(Ped ped) : base(ped, "upperBodyFlinch")
		{
		}

		public float HandDistanceLeftRight
		{
			set { SetArgument("handDistanceLeftRight", value); }
		}

		public float HandDistanceFrontBack
		{
			set { SetArgument("handDistanceFrontBack", value); }
		}

		public float HandDistanceVertical
		{
			set { SetArgument("handDistanceVertical", value); }
		}

		public float BodyStiffness
		{
			set { SetArgument("bodyStiffness", value); }
		}

		public float BodyDamping
		{
			set { SetArgument("bodyDamping", value); }
		}

		public float BackBendAmount
		{
			set { SetArgument("backBendAmount", value); }
		}

		public bool UseRightArm
		{
			set { SetArgument("useRightArm", value); }
		}

		public bool UseLeftArm
		{
			set { SetArgument("useLeftArm", value); }
		}

		public float NoiseScale
		{
			set { SetArgument("noiseScale", value); }
		}

		public bool NewHit
		{
			set { SetArgument("newHit", value); }
		}

		public bool ProtectHeadToggle
		{
			set { SetArgument("protectHeadToggle", value); }
		}

		public bool DontBraceHead
		{
			set { SetArgument("dontBraceHead", value); }
		}

		public bool ApplyStiffness
		{
			set { SetArgument("applyStiffness", value); }
		}

		public bool HeadLookAwayFromTarget
		{
			set { SetArgument("headLookAwayFromTarget", value); }
		}

		public bool UseHeadLook
		{
			set { SetArgument("useHeadLook", value); }
		}

		public int TurnTowards
		{
			set { SetArgument("turnTowards", value); }
		}

		public Vector3 Pos
		{
			set { SetArgument("pos", value); }
		}
	}

	public sealed class YankedHelper : CustomHelper
	{
		public YankedHelper(Ped ped) : base(ped, "yanked")
		{
		}

		public float ArmStiffness
		{
			set { SetArgument("armStiffness", value); }
		}

		public float ArmDamping
		{
			set { SetArgument("armDamping", value); }
		}

		public float SpineDamping
		{
			set { SetArgument("spineDamping", value); }
		}

		public float SpineStiffness
		{
			set { SetArgument("spineStiffness", value); }
		}

		public float ArmStiffnessStart
		{
			set { SetArgument("armStiffnessStart", value); }
		}

		public float ArmDampingStart
		{
			set { SetArgument("armDampingStart", value); }
		}

		public float SpineDampingStart
		{
			set { SetArgument("spineDampingStart", value); }
		}

		public float SpineStiffnessStart
		{
			set { SetArgument("spineStiffnessStart", value); }
		}

		public float TimeAtStartValues
		{
			set { SetArgument("timeAtStartValues", value); }
		}

		public float RampTimeFromStartValues
		{
			set { SetArgument("rampTimeFromStartValues", value); }
		}

		public int StepsTillStartEnd
		{
			set { SetArgument("stepsTillStartEnd", value); }
		}

		public float TimeStartEnd
		{
			set { SetArgument("timeStartEnd", value); }
		}

		public float RampTimeToEndValues
		{
			set { SetArgument("rampTimeToEndValues", value); }
		}

		public float LowerBodyStiffness
		{
			set { SetArgument("lowerBodyStiffness", value); }
		}

		public float LowerBodyStiffnessEnd
		{
			set { SetArgument("lowerBodyStiffnessEnd", value); }
		}

		public float PerStepReduction
		{
			set { SetArgument("perStepReduction", value); }
		}

		public float HipPitchForward
		{
			set { SetArgument("hipPitchForward", value); }
		}

		public float HipPitchBack
		{
			set { SetArgument("hipPitchBack", value); }
		}

		public float SpineBend
		{
			set { SetArgument("spineBend", value); }
		}

		public float FootFriction
		{
			set { SetArgument("footFriction", value); }
		}

		public float TurnThresholdMin
		{
			set { SetArgument("turnThresholdMin", value); }
		}

		public float TurnThresholdMax
		{
			set { SetArgument("turnThresholdMax", value); }
		}

		public bool UseHeadLook
		{
			set { SetArgument("useHeadLook", value); }
		}

		public Vector3 HeadLookPos
		{
			set { SetArgument("headLookPos", value); }
		}

		public int HeadLookInstanceIndex
		{
			set { SetArgument("headLookInstanceIndex", value); }
		}

		public float HeadLookAtVelProb
		{
			set { SetArgument("headLookAtVelProb", value); }
		}

		public float ComVelRDSThresh
		{
			set { SetArgument("comVelRDSThresh", value); }
		}

		public float HulaPeriod
		{
			set { SetArgument("hulaPeriod", value); }
		}

		public float HipAmplitude
		{
			set { SetArgument("hipAmplitude", value); }
		}

		public float SpineAmplitude
		{
			set { SetArgument("spineAmplitude", value); }
		}

		public float MinRelaxPeriod
		{
			set { SetArgument("minRelaxPeriod", value); }
		}

		public float MaxRelaxPeriod
		{
			set { SetArgument("maxRelaxPeriod", value); }
		}

		public float RollHelp
		{
			set { SetArgument("rollHelp", value); }
		}

		public float GroundLegStiffness
		{
			set { SetArgument("groundLegStiffness", value); }
		}

		public float GroundArmStiffness
		{
			set { SetArgument("groundArmStiffness", value); }
		}

		public float GroundSpineStiffness
		{
			set { SetArgument("groundSpineStiffness", value); }
		}

		public float GroundLegDamping
		{
			set { SetArgument("groundLegDamping", value); }
		}

		public float GroundArmDamping
		{
			set { SetArgument("groundArmDamping", value); }
		}

		public float GroundSpineDamping
		{
			set { SetArgument("groundSpineDamping", value); }
		}

		public float GroundFriction
		{
			set { SetArgument("groundFriction", value); }
		}
	}
}
