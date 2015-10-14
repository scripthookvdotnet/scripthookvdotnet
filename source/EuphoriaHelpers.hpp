#pragma once

#include "EuphoriaBase.hpp"

namespace GTA
{
	namespace NaturalMotion
	{
		public enum class ArmDirections
		{
			Backwards = -1,
			Adaptive = 0,
			Forwards = 1
		};

		public ref class ActivePoseHelper sealed : public CustomHelper
		{
		public:
			ActivePoseHelper(Ped ^ped) : CustomHelper(ped, "activePose")
			{
			}

			property System::String ^Mask
			{
				void set(System::String ^value)
				{
					SetArgument("mask", value);
				}
			}
			property bool UseGravityCompensation
			{
				void set(bool value)
				{
					SetArgument("useGravityCompensation", value);
				}
			}
			property int AnimSource
			{
				void set(int value)
				{
					SetArgument("animSource", value);
				}
			}
		};
		public ref class ApplyImpulseHelper sealed : public CustomHelper
		{
		public:
			ApplyImpulseHelper(Ped ^ped) : CustomHelper(ped, "applyImpulse")
			{
			}

			property float EqualizeAmount
			{
				void set(float value)
				{
					SetArgument("equalizeAmount", value);
				}
			}
			property int PartIndex
			{
				void set(int value)
				{
					SetArgument("partIndex", value);
				}
			}
			property Math::Vector3 Impulse
			{
				void set(Math::Vector3 value)
				{
					SetArgument("impulse", value);
				}
			}
			property Math::Vector3 HitPoint
			{
				void set(Math::Vector3 value)
				{
					SetArgument("hitPoint", value);
				}
			}
			property bool LocalHitPointInfo
			{
				void set(bool value)
				{
					SetArgument("localHitPointInfo", value);
				}
			}
			property bool LocalImpulseInfo
			{
				void set(bool value)
				{
					SetArgument("localImpulseInfo", value);
				}
			}
			property bool AngularImpulse
			{
				void set(bool value)
				{
					SetArgument("angularImpulse", value);
				}
			}
		};
		public ref class ApplyBulletImpulseHelper sealed : public CustomHelper
		{
		public:
			ApplyBulletImpulseHelper(Ped ^ped) : CustomHelper(ped, "applyBulletImpulse")
			{
			}

			property float EqualizeAmount
			{
				void set(float value)
				{
					SetArgument("equalizeAmount", value);
				}
			}
			property int PartIndex
			{
				void set(int value)
				{
					SetArgument("partIndex", value);
				}
			}
			property Math::Vector3 Impulse
			{
				void set(Math::Vector3 value)
				{
					SetArgument("impulse", value);
				}
			}
			property Math::Vector3 HitPoint
			{
				void set(Math::Vector3 value)
				{
					SetArgument("hitPoint", value);
				}
			}
			property bool LocalHitPointInfo
			{
				void set(bool value)
				{
					SetArgument("localHitPointInfo", value);
				}
			}
			property float ExtraShare
			{
				void set(float value)
				{
					SetArgument("extraShare", value);
				}
			}
		};
		public ref class BodyRelaxHelper sealed : public CustomHelper
		{
		public:
			BodyRelaxHelper(Ped ^ped) : CustomHelper(ped, "bodyRelax")
			{
			}

			property float Relaxation
			{
				void set(float value)
				{
					SetArgument("relaxation", value);
				}
			}
			property float Damping
			{
				void set(float value)
				{
					SetArgument("damping", value);
				}
			}
			property System::String ^Mask
			{
				void set(System::String ^value)
				{
					SetArgument("mask", value);
				}
			}
			property bool HoldPose
			{
				void set(bool value)
				{
					SetArgument("holdPose", value);
				}
			}
			property bool DisableJointDriving
			{
				void set(bool value)
				{
					SetArgument("disableJointDriving", value);
				}
			}
		};
		public ref class ConfigureBalanceHelper sealed : public CustomHelper
		{
		public:
			ConfigureBalanceHelper(Ped ^ped) : CustomHelper(ped, "configureBalance")
			{
			}

			property float StepHeight
			{
				void set(float value)
				{
					SetArgument("stepHeight", value);
				}
			}
			property float StepHeightInc4Step
			{
				void set(float value)
				{
					SetArgument("stepHeightInc4Step", value);
				}
			}
			property float LegsApartRestep
			{
				void set(float value)
				{
					SetArgument("legsApartRestep", value);
				}
			}
			property float LegsTogetherRestep
			{
				void set(float value)
				{
					SetArgument("legsTogetherRestep", value);
				}
			}
			property float LegsApartMax
			{
				void set(float value)
				{
					SetArgument("legsApartMax", value);
				}
			}
			property bool TaperKneeStrength
			{
				void set(bool value)
				{
					SetArgument("taperKneeStrength", value);
				}
			}
			property float LegStiffness
			{
				void set(float value)
				{
					SetArgument("legStiffness", value);
				}
			}
			property float LeftLegSwingDamping
			{
				void set(float value)
				{
					SetArgument("leftLegSwingDamping", value);
				}
			}
			property float RightLegSwingDamping
			{
				void set(float value)
				{
					SetArgument("rightLegSwingDamping", value);
				}
			}
			property float OpposeGravityLegs
			{
				void set(float value)
				{
					SetArgument("opposeGravityLegs", value);
				}
			}
			property float OpposeGravityAnkles
			{
				void set(float value)
				{
					SetArgument("opposeGravityAnkles", value);
				}
			}
			property float LeanAcc
			{
				void set(float value)
				{
					SetArgument("leanAcc", value);
				}
			}
			property float HipLeanAcc
			{
				void set(float value)
				{
					SetArgument("hipLeanAcc", value);
				}
			}
			property float LeanAccMax
			{
				void set(float value)
				{
					SetArgument("leanAccMax", value);
				}
			}
			property float ResistAcc
			{
				void set(float value)
				{
					SetArgument("resistAcc", value);
				}
			}
			property float ResistAccMax
			{
				void set(float value)
				{
					SetArgument("resistAccMax", value);
				}
			}
			property bool FootSlipCompOnMovingFloor
			{
				void set(bool value)
				{
					SetArgument("footSlipCompOnMovingFloor", value);
				}
			}
			property float AnkleEquilibrium
			{
				void set(float value)
				{
					SetArgument("ankleEquilibrium", value);
				}
			}
			property float ExtraFeetApart
			{
				void set(float value)
				{
					SetArgument("extraFeetApart", value);
				}
			}
			property float DontStepTime
			{
				void set(float value)
				{
					SetArgument("dontStepTime", value);
				}
			}
			property float BalanceAbortThreshold
			{
				void set(float value)
				{
					SetArgument("balanceAbortThreshold", value);
				}
			}
			property float GiveUpHeight
			{
				void set(float value)
				{
					SetArgument("giveUpHeight", value);
				}
			}
			property float StepClampScale
			{
				void set(float value)
				{
					SetArgument("stepClampScale", value);
				}
			}
			property float StepClampScaleVariance
			{
				void set(float value)
				{
					SetArgument("stepClampScaleVariance", value);
				}
			}
			property float PredictionTimeHip
			{
				void set(float value)
				{
					SetArgument("predictionTimeHip", value);
				}
			}
			property float PredictionTime
			{
				void set(float value)
				{
					SetArgument("predictionTime", value);
				}
			}
			property float PredictionTimeVariance
			{
				void set(float value)
				{
					SetArgument("predictionTimeVariance", value);
				}
			}
			property int MaxSteps
			{
				void set(int value)
				{
					SetArgument("maxSteps", value);
				}
			}
			property float MaxBalanceTime
			{
				void set(float value)
				{
					SetArgument("maxBalanceTime", value);
				}
			}
			property int ExtraSteps
			{
				void set(int value)
				{
					SetArgument("extraSteps", value);
				}
			}
			property float ExtraTime
			{
				void set(float value)
				{
					SetArgument("extraTime", value);
				}
			}
			property int FallType
			{
				void set(int value)
				{
					SetArgument("fallType", value);
				}
			}
			property float FallMult
			{
				void set(float value)
				{
					SetArgument("fallMult", value);
				}
			}
			property bool FallReduceGravityComp
			{
				void set(bool value)
				{
					SetArgument("fallReduceGravityComp", value);
				}
			}
			property bool RampHipPitchOnFail
			{
				void set(bool value)
				{
					SetArgument("rampHipPitchOnFail", value);
				}
			}
			property float StableLinSpeedThresh
			{
				void set(float value)
				{
					SetArgument("stableLinSpeedThresh", value);
				}
			}
			property float StableRotSpeedThresh
			{
				void set(float value)
				{
					SetArgument("stableRotSpeedThresh", value);
				}
			}
			property bool FailMustCollide
			{
				void set(bool value)
				{
					SetArgument("failMustCollide", value);
				}
			}
			property bool IgnoreFailure
			{
				void set(bool value)
				{
					SetArgument("ignoreFailure", value);
				}
			}
			property float ChangeStepTime
			{
				void set(float value)
				{
					SetArgument("changeStepTime", value);
				}
			}
			property bool BalanceIndefinitely
			{
				void set(bool value)
				{
					SetArgument("balanceIndefinitely", value);
				}
			}
			property bool MovingFloor
			{
				void set(bool value)
				{
					SetArgument("movingFloor", value);
				}
			}
			property bool AirborneStep
			{
				void set(bool value)
				{
					SetArgument("airborneStep", value);
				}
			}
			property float UseComDirTurnVelThresh
			{
				void set(float value)
				{
					SetArgument("useComDirTurnVelThresh", value);
				}
			}
			property float MinKneeAngle
			{
				void set(float value)
				{
					SetArgument("minKneeAngle", value);
				}
			}
			property bool FlatterSwingFeet
			{
				void set(bool value)
				{
					SetArgument("flatterSwingFeet", value);
				}
			}
			property bool FlatterStaticFeet
			{
				void set(bool value)
				{
					SetArgument("flatterStaticFeet", value);
				}
			}
			property bool AvoidLeg
			{
				void set(bool value)
				{
					SetArgument("avoidLeg", value);
				}
			}
			property float AvoidFootWidth
			{
				void set(float value)
				{
					SetArgument("avoidFootWidth", value);
				}
			}
			property float AvoidFeedback
			{
				void set(float value)
				{
					SetArgument("avoidFeedback", value);
				}
			}
			property float LeanAgainstVelocity
			{
				void set(float value)
				{
					SetArgument("leanAgainstVelocity", value);
				}
			}
			property float StepDecisionThreshold
			{
				void set(float value)
				{
					SetArgument("stepDecisionThreshold", value);
				}
			}
			property bool StepIfInSupport
			{
				void set(bool value)
				{
					SetArgument("stepIfInSupport", value);
				}
			}
			property bool AlwaysStepWithFarthest
			{
				void set(bool value)
				{
					SetArgument("alwaysStepWithFarthest", value);
				}
			}
			property bool StandUp
			{
				void set(bool value)
				{
					SetArgument("standUp", value);
				}
			}
			property float DepthFudge
			{
				void set(float value)
				{
					SetArgument("depthFudge", value);
				}
			}
			property float DepthFudgeStagger
			{
				void set(float value)
				{
					SetArgument("depthFudgeStagger", value);
				}
			}
			property float FootFriction
			{
				void set(float value)
				{
					SetArgument("footFriction", value);
				}
			}
			property float FootFrictionStagger
			{
				void set(float value)
				{
					SetArgument("footFrictionStagger", value);
				}
			}
			property float BackwardsLeanCutoff
			{
				void set(float value)
				{
					SetArgument("backwardsLeanCutoff", value);
				}
			}
			property float GiveUpHeightEnd
			{
				void set(float value)
				{
					SetArgument("giveUpHeightEnd", value);
				}
			}
			property float BalanceAbortThresholdEnd
			{
				void set(float value)
				{
					SetArgument("balanceAbortThresholdEnd", value);
				}
			}
			property float GiveUpRampDuration
			{
				void set(float value)
				{
					SetArgument("giveUpRampDuration", value);
				}
			}
			property float LeanToAbort
			{
				void set(float value)
				{
					SetArgument("leanToAbort", value);
				}
			}
		};
		public ref class ConfigureBalanceResetHelper sealed : public CustomHelper
		{
		public:
			ConfigureBalanceResetHelper(Ped ^ped) : CustomHelper(ped, "configureBalanceReset")
			{
			}
		};
		public ref class ConfigureSelfAvoidanceHelper sealed : public CustomHelper
		{
		public:
			ConfigureSelfAvoidanceHelper(Ped ^ped) : CustomHelper(ped, "configureSelfAvoidance")
			{
			}

			property bool UseSelfAvoidance
			{
				void set(bool value)
				{
					SetArgument("useSelfAvoidance", value);
				}
			}
			property bool OverwriteDragReduction
			{
				void set(bool value)
				{
					SetArgument("overwriteDragReduction", value);
				}
			}
			property float TorsoSwingFraction
			{
				void set(float value)
				{
					SetArgument("torsoSwingFraction", value);
				}
			}
			property float MaxTorsoSwingAngleRad
			{
				void set(float value)
				{
					SetArgument("maxTorsoSwingAngleRad", value);
				}
			}
			property bool SelfAvoidIfInSpineBoundsOnly
			{
				void set(bool value)
				{
					SetArgument("selfAvoidIfInSpineBoundsOnly", value);
				}
			}
			property float SelfAvoidAmount
			{
				void set(float value)
				{
					SetArgument("selfAvoidAmount", value);
				}
			}
			property bool OverwriteTwist
			{
				void set(bool value)
				{
					SetArgument("overwriteTwist", value);
				}
			}
			property bool UsePolarPathAlgorithm
			{
				void set(bool value)
				{
					SetArgument("usePolarPathAlgorithm", value);
				}
			}
			property float Radius
			{
				void set(float value)
				{
					SetArgument("radius", value);
				}
			}
		};
		public ref class ConfigureBulletsHelper sealed : public CustomHelper
		{
		public:
			ConfigureBulletsHelper(Ped ^ped) : CustomHelper(ped, "configureBullets")
			{
			}

			property bool ImpulseSpreadOverParts
			{
				void set(bool value)
				{
					SetArgument("impulseSpreadOverParts", value);
				}
			}
			property bool ImpulseLeakageStrengthScaled
			{
				void set(bool value)
				{
					SetArgument("impulseLeakageStrengthScaled", value);
				}
			}
			property float ImpulsePeriod
			{
				void set(float value)
				{
					SetArgument("impulsePeriod", value);
				}
			}
			property float ImpulseTorqueScale
			{
				void set(float value)
				{
					SetArgument("impulseTorqueScale", value);
				}
			}
			property bool LoosenessFix
			{
				void set(bool value)
				{
					SetArgument("loosenessFix", value);
				}
			}
			property float ImpulseDelay
			{
				void set(float value)
				{
					SetArgument("impulseDelay", value);
				}
			}
			property float ImpulseReductionPerShot
			{
				void set(float value)
				{
					SetArgument("impulseReductionPerShot", value);
				}
			}
			property float ImpulseRecovery
			{
				void set(float value)
				{
					SetArgument("impulseRecovery", value);
				}
			}
			property float ImpulseMinLeakage
			{
				void set(float value)
				{
					SetArgument("impulseMinLeakage", value);
				}
			}
			property int TorqueMode
			{
				void set(int value)
				{
					SetArgument("torqueMode", value);
				}
			}
			property int TorqueSpinMode
			{
				void set(int value)
				{
					SetArgument("torqueSpinMode", value);
				}
			}
			property int TorqueFilterMode
			{
				void set(int value)
				{
					SetArgument("torqueFilterMode", value);
				}
			}
			property bool TorqueAlwaysSpine3
			{
				void set(bool value)
				{
					SetArgument("torqueAlwaysSpine3", value);
				}
			}
			property float TorqueDelay
			{
				void set(float value)
				{
					SetArgument("torqueDelay", value);
				}
			}
			property float TorquePeriod
			{
				void set(float value)
				{
					SetArgument("torquePeriod", value);
				}
			}
			property float TorqueGain
			{
				void set(float value)
				{
					SetArgument("torqueGain", value);
				}
			}
			property float TorqueCutoff
			{
				void set(float value)
				{
					SetArgument("torqueCutoff", value);
				}
			}
			property float TorqueReductionPerTick
			{
				void set(float value)
				{
					SetArgument("torqueReductionPerTick", value);
				}
			}
			property float LiftGain
			{
				void set(float value)
				{
					SetArgument("liftGain", value);
				}
			}
			property float CounterImpulseDelay
			{
				void set(float value)
				{
					SetArgument("counterImpulseDelay", value);
				}
			}
			property float CounterImpulseMag
			{
				void set(float value)
				{
					SetArgument("counterImpulseMag", value);
				}
			}
			property bool CounterAfterMagReached
			{
				void set(bool value)
				{
					SetArgument("counterAfterMagReached", value);
				}
			}
			property bool DoCounterImpulse
			{
				void set(bool value)
				{
					SetArgument("doCounterImpulse", value);
				}
			}
			property float CounterImpulse2Hips
			{
				void set(float value)
				{
					SetArgument("counterImpulse2Hips", value);
				}
			}
			property float ImpulseNoBalMult
			{
				void set(float value)
				{
					SetArgument("impulseNoBalMult", value);
				}
			}
			property float ImpulseBalStabStart
			{
				void set(float value)
				{
					SetArgument("impulseBalStabStart", value);
				}
			}
			property float ImpulseBalStabEnd
			{
				void set(float value)
				{
					SetArgument("impulseBalStabEnd", value);
				}
			}
			property float ImpulseBalStabMult
			{
				void set(float value)
				{
					SetArgument("impulseBalStabMult", value);
				}
			}
			property float ImpulseSpineAngStart
			{
				void set(float value)
				{
					SetArgument("impulseSpineAngStart", value);
				}
			}
			property float ImpulseSpineAngEnd
			{
				void set(float value)
				{
					SetArgument("impulseSpineAngEnd", value);
				}
			}
			property float ImpulseSpineAngMult
			{
				void set(float value)
				{
					SetArgument("impulseSpineAngMult", value);
				}
			}
			property float ImpulseVelStart
			{
				void set(float value)
				{
					SetArgument("impulseVelStart", value);
				}
			}
			property float ImpulseVelEnd
			{
				void set(float value)
				{
					SetArgument("impulseVelEnd", value);
				}
			}
			property float ImpulseVelMult
			{
				void set(float value)
				{
					SetArgument("impulseVelMult", value);
				}
			}
			property float ImpulseAirMult
			{
				void set(float value)
				{
					SetArgument("impulseAirMult", value);
				}
			}
			property float ImpulseAirMultStart
			{
				void set(float value)
				{
					SetArgument("impulseAirMultStart", value);
				}
			}
			property float ImpulseAirMax
			{
				void set(float value)
				{
					SetArgument("impulseAirMax", value);
				}
			}
			property float ImpulseAirApplyAbove
			{
				void set(float value)
				{
					SetArgument("impulseAirApplyAbove", value);
				}
			}
			property bool ImpulseAirOn
			{
				void set(bool value)
				{
					SetArgument("impulseAirOn", value);
				}
			}
			property float ImpulseOneLegMult
			{
				void set(float value)
				{
					SetArgument("impulseOneLegMult", value);
				}
			}
			property float ImpulseOneLegMultStart
			{
				void set(float value)
				{
					SetArgument("impulseOneLegMultStart", value);
				}
			}
			property float ImpulseOneLegMax
			{
				void set(float value)
				{
					SetArgument("impulseOneLegMax", value);
				}
			}
			property float ImpulseOneLegApplyAbove
			{
				void set(float value)
				{
					SetArgument("impulseOneLegApplyAbove", value);
				}
			}
			property bool ImpulseOneLegOn
			{
				void set(bool value)
				{
					SetArgument("impulseOneLegOn", value);
				}
			}
			property float RbRatio
			{
				void set(float value)
				{
					SetArgument("rbRatio", value);
				}
			}
			property float RbLowerShare
			{
				void set(float value)
				{
					SetArgument("rbLowerShare", value);
				}
			}
			property float RbMoment
			{
				void set(float value)
				{
					SetArgument("rbMoment", value);
				}
			}
			property float RbMaxTwistMomentArm
			{
				void set(float value)
				{
					SetArgument("rbMaxTwistMomentArm", value);
				}
			}
			property float RbMaxBroomMomentArm
			{
				void set(float value)
				{
					SetArgument("rbMaxBroomMomentArm", value);
				}
			}
			property float RbRatioAirborne
			{
				void set(float value)
				{
					SetArgument("rbRatioAirborne", value);
				}
			}
			property float RbMomentAirborne
			{
				void set(float value)
				{
					SetArgument("rbMomentAirborne", value);
				}
			}
			property float RbMaxTwistMomentArmAirborne
			{
				void set(float value)
				{
					SetArgument("rbMaxTwistMomentArmAirborne", value);
				}
			}
			property float RbMaxBroomMomentArmAirborne
			{
				void set(float value)
				{
					SetArgument("rbMaxBroomMomentArmAirborne", value);
				}
			}
			property float RbRatioOneLeg
			{
				void set(float value)
				{
					SetArgument("rbRatioOneLeg", value);
				}
			}
			property float RbMomentOneLeg
			{
				void set(float value)
				{
					SetArgument("rbMomentOneLeg", value);
				}
			}
			property float RbMaxTwistMomentArmOneLeg
			{
				void set(float value)
				{
					SetArgument("rbMaxTwistMomentArmOneLeg", value);
				}
			}
			property float RbMaxBroomMomentArmOneLeg
			{
				void set(float value)
				{
					SetArgument("rbMaxBroomMomentArmOneLeg", value);
				}
			}
			property int RbTwistAxis
			{
				void set(int value)
				{
					SetArgument("rbTwistAxis", value);
				}
			}
			property bool RbPivot
			{
				void set(bool value)
				{
					SetArgument("rbPivot", value);
				}
			}
		};
		public ref class ConfigureBulletsExtraHelper sealed : public CustomHelper
		{
		public:
			ConfigureBulletsExtraHelper(Ped ^ped) : CustomHelper(ped, "configureBulletsExtra")
			{
			}

			property bool ImpulseSpreadOverParts
			{
				void set(bool value)
				{
					SetArgument("impulseSpreadOverParts", value);
				}
			}
			property float ImpulsePeriod
			{
				void set(float value)
				{
					SetArgument("impulsePeriod", value);
				}
			}
			property float ImpulseTorqueScale
			{
				void set(float value)
				{
					SetArgument("impulseTorqueScale", value);
				}
			}
			property bool LoosenessFix
			{
				void set(bool value)
				{
					SetArgument("loosenessFix", value);
				}
			}
			property float ImpulseDelay
			{
				void set(float value)
				{
					SetArgument("impulseDelay", value);
				}
			}
			property int TorqueMode
			{
				void set(int value)
				{
					SetArgument("torqueMode", value);
				}
			}
			property int TorqueSpinMode
			{
				void set(int value)
				{
					SetArgument("torqueSpinMode", value);
				}
			}
			property int TorqueFilterMode
			{
				void set(int value)
				{
					SetArgument("torqueFilterMode", value);
				}
			}
			property bool TorqueAlwaysSpine3
			{
				void set(bool value)
				{
					SetArgument("torqueAlwaysSpine3", value);
				}
			}
			property float TorqueDelay
			{
				void set(float value)
				{
					SetArgument("torqueDelay", value);
				}
			}
			property float TorquePeriod
			{
				void set(float value)
				{
					SetArgument("torquePeriod", value);
				}
			}
			property float TorqueGain
			{
				void set(float value)
				{
					SetArgument("torqueGain", value);
				}
			}
			property float TorqueCutoff
			{
				void set(float value)
				{
					SetArgument("torqueCutoff", value);
				}
			}
			property float TorqueReductionPerTick
			{
				void set(float value)
				{
					SetArgument("torqueReductionPerTick", value);
				}
			}
			property float LiftGain
			{
				void set(float value)
				{
					SetArgument("liftGain", value);
				}
			}
			property float CounterImpulseDelay
			{
				void set(float value)
				{
					SetArgument("counterImpulseDelay", value);
				}
			}
			property float CounterImpulseMag
			{
				void set(float value)
				{
					SetArgument("counterImpulseMag", value);
				}
			}
			property bool CounterAfterMagReached
			{
				void set(bool value)
				{
					SetArgument("counterAfterMagReached", value);
				}
			}
			property bool DoCounterImpulse
			{
				void set(bool value)
				{
					SetArgument("doCounterImpulse", value);
				}
			}
			property float CounterImpulse2Hips
			{
				void set(float value)
				{
					SetArgument("counterImpulse2Hips", value);
				}
			}
			property float ImpulseNoBalMult
			{
				void set(float value)
				{
					SetArgument("impulseNoBalMult", value);
				}
			}
			property float ImpulseBalStabStart
			{
				void set(float value)
				{
					SetArgument("impulseBalStabStart", value);
				}
			}
			property float ImpulseBalStabEnd
			{
				void set(float value)
				{
					SetArgument("impulseBalStabEnd", value);
				}
			}
			property float ImpulseBalStabMult
			{
				void set(float value)
				{
					SetArgument("impulseBalStabMult", value);
				}
			}
			property float ImpulseSpineAngStart
			{
				void set(float value)
				{
					SetArgument("impulseSpineAngStart", value);
				}
			}
			property float ImpulseSpineAngEnd
			{
				void set(float value)
				{
					SetArgument("impulseSpineAngEnd", value);
				}
			}
			property float ImpulseSpineAngMult
			{
				void set(float value)
				{
					SetArgument("impulseSpineAngMult", value);
				}
			}
			property float ImpulseVelStart
			{
				void set(float value)
				{
					SetArgument("impulseVelStart", value);
				}
			}
			property float ImpulseVelEnd
			{
				void set(float value)
				{
					SetArgument("impulseVelEnd", value);
				}
			}
			property float ImpulseVelMult
			{
				void set(float value)
				{
					SetArgument("impulseVelMult", value);
				}
			}
			property float ImpulseAirMult
			{
				void set(float value)
				{
					SetArgument("impulseAirMult", value);
				}
			}
			property float ImpulseAirMultStart
			{
				void set(float value)
				{
					SetArgument("impulseAirMultStart", value);
				}
			}
			property float ImpulseAirMax
			{
				void set(float value)
				{
					SetArgument("impulseAirMax", value);
				}
			}
			property float ImpulseAirApplyAbove
			{
				void set(float value)
				{
					SetArgument("impulseAirApplyAbove", value);
				}
			}
			property bool ImpulseAirOn
			{
				void set(bool value)
				{
					SetArgument("impulseAirOn", value);
				}
			}
			property float ImpulseOneLegMult
			{
				void set(float value)
				{
					SetArgument("impulseOneLegMult", value);
				}
			}
			property float ImpulseOneLegMultStart
			{
				void set(float value)
				{
					SetArgument("impulseOneLegMultStart", value);
				}
			}
			property float ImpulseOneLegMax
			{
				void set(float value)
				{
					SetArgument("impulseOneLegMax", value);
				}
			}
			property float ImpulseOneLegApplyAbove
			{
				void set(float value)
				{
					SetArgument("impulseOneLegApplyAbove", value);
				}
			}
			property bool ImpulseOneLegOn
			{
				void set(bool value)
				{
					SetArgument("impulseOneLegOn", value);
				}
			}
			property float RbRatio
			{
				void set(float value)
				{
					SetArgument("rbRatio", value);
				}
			}
			property float RbLowerShare
			{
				void set(float value)
				{
					SetArgument("rbLowerShare", value);
				}
			}
			property float RbMoment
			{
				void set(float value)
				{
					SetArgument("rbMoment", value);
				}
			}
			property float RbMaxTwistMomentArm
			{
				void set(float value)
				{
					SetArgument("rbMaxTwistMomentArm", value);
				}
			}
			property float RbMaxBroomMomentArm
			{
				void set(float value)
				{
					SetArgument("rbMaxBroomMomentArm", value);
				}
			}
			property float RbRatioAirborne
			{
				void set(float value)
				{
					SetArgument("rbRatioAirborne", value);
				}
			}
			property float RbMomentAirborne
			{
				void set(float value)
				{
					SetArgument("rbMomentAirborne", value);
				}
			}
			property float RbMaxTwistMomentArmAirborne
			{
				void set(float value)
				{
					SetArgument("rbMaxTwistMomentArmAirborne", value);
				}
			}
			property float RbMaxBroomMomentArmAirborne
			{
				void set(float value)
				{
					SetArgument("rbMaxBroomMomentArmAirborne", value);
				}
			}
			property float RbRatioOneLeg
			{
				void set(float value)
				{
					SetArgument("rbRatioOneLeg", value);
				}
			}
			property float RbMomentOneLeg
			{
				void set(float value)
				{
					SetArgument("rbMomentOneLeg", value);
				}
			}
			property float RbMaxTwistMomentArmOneLeg
			{
				void set(float value)
				{
					SetArgument("rbMaxTwistMomentArmOneLeg", value);
				}
			}
			property float RbMaxBroomMomentArmOneLeg
			{
				void set(float value)
				{
					SetArgument("rbMaxBroomMomentArmOneLeg", value);
				}
			}
			property int RbTwistAxis
			{
				void set(int value)
				{
					SetArgument("rbTwistAxis", value);
				}
			}
			property bool RbPivot
			{
				void set(bool value)
				{
					SetArgument("rbPivot", value);
				}
			}
		};
		public ref class ConfigureLimitsHelper sealed : public CustomHelper
		{
		public:
			ConfigureLimitsHelper(Ped ^ped) : CustomHelper(ped, "configureLimits")
			{
			}

			property System::String ^Mask
			{
				void set(System::String ^value)
				{
					SetArgument("mask", value);
				}
			}
			property bool Enable
			{
				void set(bool value)
				{
					SetArgument("enable", value);
				}
			}
			property bool ToDesired
			{
				void set(bool value)
				{
					SetArgument("toDesired", value);
				}
			}
			property bool Restore
			{
				void set(bool value)
				{
					SetArgument("restore", value);
				}
			}
			property bool ToCurAnimation
			{
				void set(bool value)
				{
					SetArgument("toCurAnimation", value);
				}
			}
			property int Index
			{
				void set(int value)
				{
					SetArgument("index", value);
				}
			}
			property float Lean1
			{
				void set(float value)
				{
					SetArgument("lean1", value);
				}
			}
			property float Lean2
			{
				void set(float value)
				{
					SetArgument("lean2", value);
				}
			}
			property float Twist
			{
				void set(float value)
				{
					SetArgument("twist", value);
				}
			}
			property float Margin
			{
				void set(float value)
				{
					SetArgument("margin", value);
				}
			}
		};
		public ref class ConfigureSoftLimitHelper sealed : public CustomHelper
		{
		public:
			ConfigureSoftLimitHelper(Ped ^ped) : CustomHelper(ped, "configureSoftLimit")
			{
			}

			property int Index
			{
				void set(int value)
				{
					SetArgument("index", value);
				}
			}
			property float Stiffness
			{
				void set(float value)
				{
					SetArgument("stiffness", value);
				}
			}
			property float Damping
			{
				void set(float value)
				{
					SetArgument("damping", value);
				}
			}
			property float LimitAngle
			{
				void set(float value)
				{
					SetArgument("limitAngle", value);
				}
			}
			property int ApproachDirection
			{
				void set(int value)
				{
					SetArgument("approachDirection", value);
				}
			}
			property bool VelocityScaled
			{
				void set(bool value)
				{
					SetArgument("velocityScaled", value);
				}
			}
		};
		public ref class ConfigureShotInjuredArmHelper sealed : public CustomHelper
		{
		public:
			ConfigureShotInjuredArmHelper(Ped ^ped) : CustomHelper(ped, "configureShotInjuredArm")
			{
			}

			property float InjuredArmTime
			{
				void set(float value)
				{
					SetArgument("injuredArmTime", value);
				}
			}
			property float HipYaw
			{
				void set(float value)
				{
					SetArgument("hipYaw", value);
				}
			}
			property float HipRoll
			{
				void set(float value)
				{
					SetArgument("hipRoll", value);
				}
			}
			property float ForceStepExtraHeight
			{
				void set(float value)
				{
					SetArgument("forceStepExtraHeight", value);
				}
			}
			property bool ForceStep
			{
				void set(bool value)
				{
					SetArgument("forceStep", value);
				}
			}
			property bool StepTurn
			{
				void set(bool value)
				{
					SetArgument("stepTurn", value);
				}
			}
			property float VelMultiplierStart
			{
				void set(float value)
				{
					SetArgument("velMultiplierStart", value);
				}
			}
			property float VelMultiplierEnd
			{
				void set(float value)
				{
					SetArgument("velMultiplierEnd", value);
				}
			}
			property float VelForceStep
			{
				void set(float value)
				{
					SetArgument("velForceStep", value);
				}
			}
			property float VelStepTurn
			{
				void set(float value)
				{
					SetArgument("velStepTurn", value);
				}
			}
			property bool VelScales
			{
				void set(bool value)
				{
					SetArgument("velScales", value);
				}
			}
		};
		public ref class ConfigureShotInjuredLegHelper sealed : public CustomHelper
		{
		public:
			ConfigureShotInjuredLegHelper(Ped ^ped) : CustomHelper(ped, "configureShotInjuredLeg")
			{
			}

			property float TimeBeforeCollapseWoundLeg
			{
				void set(float value)
				{
					SetArgument("timeBeforeCollapseWoundLeg", value);
				}
			}
			property float LegInjuryTime
			{
				void set(float value)
				{
					SetArgument("legInjuryTime", value);
				}
			}
			property bool LegForceStep
			{
				void set(bool value)
				{
					SetArgument("legForceStep", value);
				}
			}
			property float LegLimpBend
			{
				void set(float value)
				{
					SetArgument("legLimpBend", value);
				}
			}
			property float LegLiftTime
			{
				void set(float value)
				{
					SetArgument("legLiftTime", value);
				}
			}
			property float LegInjury
			{
				void set(float value)
				{
					SetArgument("legInjury", value);
				}
			}
			property float LegInjuryHipPitch
			{
				void set(float value)
				{
					SetArgument("legInjuryHipPitch", value);
				}
			}
			property float LegInjuryLiftHipPitch
			{
				void set(float value)
				{
					SetArgument("legInjuryLiftHipPitch", value);
				}
			}
			property float LegInjurySpineBend
			{
				void set(float value)
				{
					SetArgument("legInjurySpineBend", value);
				}
			}
			property float LegInjuryLiftSpineBend
			{
				void set(float value)
				{
					SetArgument("legInjuryLiftSpineBend", value);
				}
			}
		};
		public ref class DefineAttachedObjectHelper sealed : public CustomHelper
		{
		public:
			DefineAttachedObjectHelper(Ped ^ped) : CustomHelper(ped, "defineAttachedObject")
			{
			}

			property int PartIndex
			{
				void set(int value)
				{
					SetArgument("partIndex", value);
				}
			}
			property float ObjectMass
			{
				void set(float value)
				{
					SetArgument("objectMass", value);
				}
			}
			property Math::Vector3 WorldPos
			{
				void set(Math::Vector3 value)
				{
					SetArgument("worldPos", value);
				}
			}
		};
		public ref class ForceToBodyPartHelper sealed : public CustomHelper
		{
		public:
			ForceToBodyPartHelper(Ped ^ped) : CustomHelper(ped, "forceToBodyPart")
			{
			}

			property int PartIndex
			{
				void set(int value)
				{
					SetArgument("partIndex", value);
				}
			}
			property Math::Vector3 Force
			{
				void set(Math::Vector3 value)
				{
					SetArgument("force", value);
				}
			}
			property bool ForceDefinedInPartSpace
			{
				void set(bool value)
				{
					SetArgument("forceDefinedInPartSpace", value);
				}
			}
		};
		public ref class LeanInDirectionHelper sealed : public CustomHelper
		{
		public:
			LeanInDirectionHelper(Ped ^ped) : CustomHelper(ped, "leanInDirection")
			{
			}

			property float LeanAmount
			{
				void set(float value)
				{
					SetArgument("leanAmount", value);
				}
			}
			property Math::Vector3 Dir
			{
				void set(Math::Vector3 value)
				{
					SetArgument("dir", value);
				}
			}
		};
		public ref class LeanRandomHelper sealed : public CustomHelper
		{
		public:
			LeanRandomHelper(Ped ^ped) : CustomHelper(ped, "leanRandom")
			{
			}

			property float LeanAmountMin
			{
				void set(float value)
				{
					SetArgument("leanAmountMin", value);
				}
			}
			property float LeanAmountMax
			{
				void set(float value)
				{
					SetArgument("leanAmountMax", value);
				}
			}
			property float ChangeTimeMin
			{
				void set(float value)
				{
					SetArgument("changeTimeMin", value);
				}
			}
			property float ChangeTimeMax
			{
				void set(float value)
				{
					SetArgument("changeTimeMax", value);
				}
			}
		};
		public ref class LeanToPositionHelper sealed : public CustomHelper
		{
		public:
			LeanToPositionHelper(Ped ^ped) : CustomHelper(ped, "leanToPosition")
			{
			}

			property float LeanAmount
			{
				void set(float value)
				{
					SetArgument("leanAmount", value);
				}
			}

			property Math::Vector3 Pos
			{
				void set(Math::Vector3 value)
				{
					SetArgument("pos", value);
				}
			}
		};
		public ref class LeanTowardsObjectHelper sealed : public CustomHelper
		{
		public:
			LeanTowardsObjectHelper(Ped ^ped) : CustomHelper(ped, "leanTowardsObject")
			{
			}

			property float LeanAmount
			{
				void set(float value)
				{
					SetArgument("leanAmount", value);
				}
			}
			property Math::Vector3 Offset
			{
				void set(Math::Vector3 value)
				{
					SetArgument("offset", value);
				}
			}
			property int InstanceIndex
			{
				void set(int value)
				{
					SetArgument("instanceIndex", value);
				}
			}
			property int BoundIndex
			{
				void set(int value)
				{
					SetArgument("boundIndex", value);
				}
			}
		};
		public ref class HipsLeanInDirectionHelper sealed : public CustomHelper
		{
		public:
			HipsLeanInDirectionHelper(Ped ^ped) : CustomHelper(ped, "hipsLeanInDirection")
			{
			}

			property float LeanAmount
			{
				void set(float value)
				{
					SetArgument("leanAmount", value);
				}
			}
			property Math::Vector3 Dir
			{
				void set(Math::Vector3 value)
				{
					SetArgument("dir", value);
				}
			}
		};
		public ref class HipsLeanRandomHelper sealed : public CustomHelper
		{
		public:
			HipsLeanRandomHelper(Ped ^ped) : CustomHelper(ped, "hipsLeanRandom")
			{
			}

			property float LeanAmountMin
			{
				void set(float value)
				{
					SetArgument("leanAmountMin", value);
				}
			}
			property float LeanAmountMax
			{
				void set(float value)
				{
					SetArgument("leanAmountMax", value);
				}
			}
			property float ChangeTimeMin
			{
				void set(float value)
				{
					SetArgument("changeTimeMin", value);
				}
			}
			property float ChangeTimeMax
			{
				void set(float value)
				{
					SetArgument("changeTimeMax", value);
				}
			}
		};
		public ref class HipsLeanToPositionHelper sealed : public CustomHelper
		{
		public:
			HipsLeanToPositionHelper(Ped ^ped) : CustomHelper(ped, "hipsLeanToPosition")
			{
			}

			property float LeanAmount
			{
				void set(float value)
				{
					SetArgument("leanAmount", value);
				}
			}
			property Math::Vector3 Pos
			{
				void set(Math::Vector3 value)
				{
					SetArgument("pos", value);
				}
			}
		};
		public ref class HipsLeanTowardsObjectHelper sealed : public CustomHelper
		{
		public:
			HipsLeanTowardsObjectHelper(Ped ^ped) : CustomHelper(ped, "hipsLeanTowardsObject")
			{
			}

			property float LeanAmount
			{
				void set(float value)
				{
					SetArgument("leanAmount", value);
				}
			}
			property Math::Vector3 Offset
			{
				void set(Math::Vector3 value)
				{
					SetArgument("offset", value);
				}
			}
			property int InstanceIndex
			{
				void set(int value)
				{
					SetArgument("instanceIndex", value);
				}
			}
			property int BoundIndex
			{
				void set(int value)
				{
					SetArgument("boundIndex", value);
				}
			}
		};
		public ref class ForceLeanInDirectionHelper sealed : public CustomHelper
		{
		public:
			ForceLeanInDirectionHelper(Ped ^ped) : CustomHelper(ped, "forceLeanInDirection")
			{
			}

			property float LeanAmount
			{
				void set(float value)
				{
					SetArgument("leanAmount", value);
				}
			}
			property Math::Vector3 Dir
			{
				void set(Math::Vector3 value)
				{
					SetArgument("dir", value);
				}
			}
			property int BodyPart
			{
				void set(int value)
				{
					SetArgument("bodyPart", value);
				}
			}
		};
		public ref class ForceLeanRandomHelper sealed : public CustomHelper
		{
		public:
			ForceLeanRandomHelper(Ped ^ped) : CustomHelper(ped, "forceLeanRandom")
			{
			}

			property float LeanAmountMin
			{
				void set(float value)
				{
					SetArgument("leanAmountMin", value);
				}
			}
			property float LeanAmountMax
			{
				void set(float value)
				{
					SetArgument("leanAmountMax", value);
				}
			}
			property float ChangeTimeMin
			{
				void set(float value)
				{
					SetArgument("changeTimeMin", value);
				}
			}
			property float ChangeTimeMax
			{
				void set(float value)
				{
					SetArgument("changeTimeMax", value);
				}
			}
			property int BodyPart
			{
				void set(int value)
				{
					SetArgument("bodyPart", value);
				}
			}
		};
		public ref class ForceLeanToPositionHelper sealed : public CustomHelper
		{
		public:
			ForceLeanToPositionHelper(Ped ^ped) : CustomHelper(ped, "forceLeanToPosition")
			{
			}

			property float LeanAmount
			{
				void set(float value)
				{
					SetArgument("leanAmount", value);
				}
			}
			property Math::Vector3 Pos
			{
				void set(Math::Vector3 value)
				{
					SetArgument("pos", value);
				}
			}
			property int BodyPart
			{
				void set(int value)
				{
					SetArgument("bodyPart", value);
				}
			}
		};
		public ref class ForceLeanTowardsObjectHelper sealed : public CustomHelper
		{
		public:
			ForceLeanTowardsObjectHelper(Ped ^ped) : CustomHelper(ped, "forceLeanTowardsObject")
			{
			}

			property float LeanAmount
			{
				void set(float value)
				{
					SetArgument("leanAmount", value);
				}
			}
			property Math::Vector3 Offset
			{
				void set(Math::Vector3 value)
				{
					SetArgument("offset", value);
				}
			}
			property int InstanceIndex
			{
				void set(int value)
				{
					SetArgument("instanceIndex", value);
				}
			}
			property int BoundIndex
			{
				void set(int value)
				{
					SetArgument("boundIndex", value);
				}
			}
			property int BodyPart
			{
				void set(int value)
				{
					SetArgument("bodyPart", value);
				}
			}
		};
		public ref class SetStiffnessHelper sealed : public CustomHelper
		{
		public:
			SetStiffnessHelper(Ped ^ped) : CustomHelper(ped, "setStiffness")
			{
			}

			property float BodyStiffness
			{
				void set(float value)
				{
					SetArgument("bodyStiffness", value);
				}
			}
			property float Damping
			{
				void set(float value)
				{
					SetArgument("damping", value);
				}
			}
			property System::String ^Mask
			{
				void set(System::String ^value)
				{
					SetArgument("mask", value);
				}
			}
		};
		public ref class SetMuscleStiffnessHelper sealed : public CustomHelper
		{
		public:
			SetMuscleStiffnessHelper(Ped ^ped) : CustomHelper(ped, "setMuscleStiffness")
			{
			}

			property float MuscleStiffness
			{
				void set(float value)
				{
					SetArgument("muscleStiffness", value);
				}
			}
			property System::String ^Mask
			{
				void set(System::String ^value)
				{
					SetArgument("mask", value);
				}
			}
		};
		public ref class SetWeaponModeHelper sealed : public CustomHelper
		{
		public:
			SetWeaponModeHelper(Ped ^ped) : CustomHelper(ped, "setWeaponMode")
			{
			}

			property int WeaponMode
			{
				void set(int value)
				{
					SetArgument("weaponMode", value);
				}
			}
		};
		public ref class RegisterWeaponHelper sealed : public CustomHelper
		{
		public:
			RegisterWeaponHelper(Ped ^ped) : CustomHelper(ped, "registerWeapon")
			{
			}

			property int Hand
			{
				void set(int value)
				{
					SetArgument("hand", value);
				}
			}
			property int LevelIndex
			{
				void set(int value)
				{
					SetArgument("levelIndex", value);
				}
			}
			property int ConstraintHandle
			{
				void set(int value)
				{
					SetArgument("constraintHandle", value);
				}
			}
			property Math::Vector3 GunToHandA
			{
				void set(Math::Vector3 value)
				{
					SetArgument("gunToHandA", value);
				}
			}
			property Math::Vector3 GunToHandB
			{
				void set(Math::Vector3 value)
				{
					SetArgument("gunToHandB", value);
				}
			}
			property Math::Vector3 GunToHandC
			{
				void set(Math::Vector3 value)
				{
					SetArgument("gunToHandC", value);
				}
			}
			property Math::Vector3 GunToHandD
			{
				void set(Math::Vector3 value)
				{
					SetArgument("gunToHandD", value);
				}
			}
			property Math::Vector3 GunToMuzzleInGun
			{
				void set(Math::Vector3 value)
				{
					SetArgument("gunToMuzzleInGun", value);
				}
			}
			property Math::Vector3 GunToButtInGun
			{
				void set(Math::Vector3 value)
				{
					SetArgument("gunToButtInGun", value);
				}
			}
		};
		public ref class ShotRelaxHelper sealed : public CustomHelper
		{
		public:
			ShotRelaxHelper(Ped ^ped) : CustomHelper(ped, "shotRelax")
			{
			}

			property float RelaxPeriodUpper
			{
				void set(float value)
				{
					SetArgument("relaxPeriodUpper", value);
				}
			}
			property float RelaxPeriodLower
			{
				void set(float value)
				{
					SetArgument("relaxPeriodLower", value);
				}
			}
		};
		public ref class FireWeaponHelper sealed : public CustomHelper
		{
		public:
			FireWeaponHelper(Ped ^ped) : CustomHelper(ped, "fireWeapon")
			{
			}

			property float FiredWeaponStrength
			{
				void set(float value)
				{
					SetArgument("firedWeaponStrength", value);
				}
			}
			property int GunHandEnum
			{
				void set(int value)
				{
					SetArgument("gunHandEnum", value);
				}
			}
			property bool ApplyFireGunForceAtClavicle
			{
				void set(bool value)
				{
					SetArgument("applyFireGunForceAtClavicle", value);
				}
			}
			property float InhibitTime
			{
				void set(float value)
				{
					SetArgument("inhibitTime", value);
				}
			}
			property Math::Vector3 Direction
			{
				void set(Math::Vector3 value)
				{
					SetArgument("direction", value);
				}
			}
			property float Split
			{
				void set(float value)
				{
					SetArgument("split", value);
				}
			}
		};
		public ref class ConfigureConstraintsHelper sealed : public CustomHelper
		{
		public:
			ConfigureConstraintsHelper(Ped ^ped) : CustomHelper(ped, "configureConstraints")
			{
			}

			property bool HandCuffs
			{
				void set(bool value)
				{
					SetArgument("handCuffs", value);
				}
			}
			property bool HandCuffsBehindBack
			{
				void set(bool value)
				{
					SetArgument("handCuffsBehindBack", value);
				}
			}
			property bool LegCuffs
			{
				void set(bool value)
				{
					SetArgument("legCuffs", value);
				}
			}
			property bool RightDominant
			{
				void set(bool value)
				{
					SetArgument("rightDominant", value);
				}
			}
			property int PassiveMode
			{
				void set(int value)
				{
					SetArgument("passiveMode", value);
				}
			}
			property bool BespokeBehaviour
			{
				void set(bool value)
				{
					SetArgument("bespokeBehaviour", value);
				}
			}
			property float Blend2ZeroPose
			{
				void set(float value)
				{
					SetArgument("blend2ZeroPose", value);
				}
			}
		};
		public ref class StayUprightHelper sealed : public CustomHelper
		{
		public:
			StayUprightHelper(Ped ^ped) : CustomHelper(ped, "stayUpright")
			{
			}

			property bool UseForces
			{
				void set(bool value)
				{
					SetArgument("useForces", value);
				}
			}
			property bool UseTorques
			{
				void set(bool value)
				{
					SetArgument("useTorques", value);
				}
			}
			property bool LastStandMode
			{
				void set(bool value)
				{
					SetArgument("lastStandMode", value);
				}
			}
			property float LastStandSinkRate
			{
				void set(float value)
				{
					SetArgument("lastStandSinkRate", value);
				}
			}
			property float LastStandHorizDamping
			{
				void set(float value)
				{
					SetArgument("lastStandHorizDamping", value);
				}
			}
			property float LastStandMaxTime
			{
				void set(float value)
				{
					SetArgument("lastStandMaxTime", value);
				}
			}
			property bool TurnTowardsBullets
			{
				void set(bool value)
				{
					SetArgument("turnTowardsBullets", value);
				}
			}
			property bool VelocityBased
			{
				void set(bool value)
				{
					SetArgument("velocityBased", value);
				}
			}
			property bool TorqueOnlyInAir
			{
				void set(bool value)
				{
					SetArgument("torqueOnlyInAir", value);
				}
			}
			property float ForceStrength
			{
				void set(float value)
				{
					SetArgument("forceStrength", value);
				}
			}
			property float ForceDamping
			{
				void set(float value)
				{
					SetArgument("forceDamping", value);
				}
			}
			property float ForceFeetMult
			{
				void set(float value)
				{
					SetArgument("forceFeetMult", value);
				}
			}
			property float ForceSpine3Share
			{
				void set(float value)
				{
					SetArgument("forceSpine3Share", value);
				}
			}
			property float ForceLeanReduction
			{
				void set(float value)
				{
					SetArgument("forceLeanReduction", value);
				}
			}
			property float ForceInAirShare
			{
				void set(float value)
				{
					SetArgument("forceInAirShare", value);
				}
			}
			property float ForceMin
			{
				void set(float value)
				{
					SetArgument("forceMin", value);
				}
			}
			property float ForceMax
			{
				void set(float value)
				{
					SetArgument("forceMax", value);
				}
			}
			property float ForceSaturationVel
			{
				void set(float value)
				{
					SetArgument("forceSaturationVel", value);
				}
			}
			property float ForceThresholdVel
			{
				void set(float value)
				{
					SetArgument("forceThresholdVel", value);
				}
			}
			property float TorqueStrength
			{
				void set(float value)
				{
					SetArgument("torqueStrength", value);
				}
			}
			property float TorqueDamping
			{
				void set(float value)
				{
					SetArgument("torqueDamping", value);
				}
			}
			property float TorqueSaturationVel
			{
				void set(float value)
				{
					SetArgument("torqueSaturationVel", value);
				}
			}
			property float TorqueThresholdVel
			{
				void set(float value)
				{
					SetArgument("torqueThresholdVel", value);
				}
			}
			property float SupportPosition
			{
				void set(float value)
				{
					SetArgument("supportPosition", value);
				}
			}
			property float NoSupportForceMult
			{
				void set(float value)
				{
					SetArgument("noSupportForceMult", value);
				}
			}
			property float StepUpHelp
			{
				void set(float value)
				{
					SetArgument("stepUpHelp", value);
				}
			}
			property float StayUpAcc
			{
				void set(float value)
				{
					SetArgument("stayUpAcc", value);
				}
			}
			property float StayUpAccMax
			{
				void set(float value)
				{
					SetArgument("stayUpAccMax", value);
				}
			}
		};
		public ref class StopAllBehavioursHelper sealed : public CustomHelper
		{
		public:
			StopAllBehavioursHelper(Ped ^ped) : CustomHelper(ped, "stopAllBehaviours")
			{
			}
		};
		public ref class SetCharacterStrengthHelper sealed : public CustomHelper
		{
		public:
			SetCharacterStrengthHelper(Ped ^ped) : CustomHelper(ped, "setCharacterStrength")
			{
			}

			property float CharacterStrength
			{
				void set(float value)
				{
					SetArgument("characterStrength", value);
				}
			}
		};
		public ref class SetCharacterHealthHelper sealed : public CustomHelper
		{
		public:
			SetCharacterHealthHelper(Ped ^ped) : CustomHelper(ped, "setCharacterHealth")
			{
			}

			property float CharacterHealth
			{
				void set(float value)
				{
					SetArgument("characterHealth", value);
				}
			}
		};
		public ref class SetFallingReactionHelper sealed : public CustomHelper
		{
		public:
			SetFallingReactionHelper(Ped ^ped) : CustomHelper(ped, "setFallingReaction")
			{
			}

			property bool HandsAndKnees
			{
				void set(bool value)
				{
					SetArgument("handsAndKnees", value);
				}
			}
			property bool CallRDS
			{
				void set(bool value)
				{
					SetArgument("callRDS", value);
				}
			}
			property float ComVelRDSThresh
			{
				void set(float value)
				{
					SetArgument("comVelRDSThresh", value);
				}
			}
			property bool ResistRolling
			{
				void set(bool value)
				{
					SetArgument("resistRolling", value);
				}
			}
			property float ArmReduceSpeed
			{
				void set(float value)
				{
					SetArgument("armReduceSpeed", value);
				}
			}
			property float ReachLengthMultiplier
			{
				void set(float value)
				{
					SetArgument("reachLengthMultiplier", value);
				}
			}
			property float InhibitRollingTime
			{
				void set(float value)
				{
					SetArgument("inhibitRollingTime", value);
				}
			}
			property float ChangeFrictionTime
			{
				void set(float value)
				{
					SetArgument("changeFrictionTime", value);
				}
			}
			property float GroundFriction
			{
				void set(float value)
				{
					SetArgument("groundFriction", value);
				}
			}
			property float FrictionMin
			{
				void set(float value)
				{
					SetArgument("frictionMin", value);
				}
			}
			property float FrictionMax
			{
				void set(float value)
				{
					SetArgument("frictionMax", value);
				}
			}
			property bool StopOnSlopes
			{
				void set(bool value)
				{
					SetArgument("stopOnSlopes", value);
				}
			}
			property float StopManual
			{
				void set(float value)
				{
					SetArgument("stopManual", value);
				}
			}
			property float StoppedStrengthDecay
			{
				void set(float value)
				{
					SetArgument("stoppedStrengthDecay", value);
				}
			}
			property float SpineLean1Offset
			{
				void set(float value)
				{
					SetArgument("spineLean1Offset", value);
				}
			}
			property bool RiflePose
			{
				void set(bool value)
				{
					SetArgument("riflePose", value);
				}
			}
			property bool HkHeadAvoid
			{
				void set(bool value)
				{
					SetArgument("hkHeadAvoid", value);
				}
			}
			property bool AntiPropClav
			{
				void set(bool value)
				{
					SetArgument("antiPropClav", value);
				}
			}
			property bool AntiPropWeak
			{
				void set(bool value)
				{
					SetArgument("antiPropWeak", value);
				}
			}
			property bool HeadAsWeakAsArms
			{
				void set(bool value)
				{
					SetArgument("headAsWeakAsArms", value);
				}
			}
			property float SuccessStrength
			{
				void set(float value)
				{
					SetArgument("successStrength", value);
				}
			}
		};
		public ref class SetCharacterUnderwaterHelper sealed : public CustomHelper
		{
		public:
			SetCharacterUnderwaterHelper(Ped ^ped) : CustomHelper(ped, "setCharacterUnderwater")
			{
			}

			property bool Underwater
			{
				void set(bool value)
				{
					SetArgument("underwater", value);
				}
			}
			property float Viscosity
			{
				void set(float value)
				{
					SetArgument("viscosity", value);
				}
			}
			property float GravityFactor
			{
				void set(float value)
				{
					SetArgument("gravityFactor", value);
				}
			}
			property float Stroke
			{
				void set(float value)
				{
					SetArgument("stroke", value);
				}
			}
			property bool LinearStroke
			{
				void set(bool value)
				{
					SetArgument("linearStroke", value);
				}
			}
		};
		public ref class SetCharacterCollisionsHelper sealed : public CustomHelper
		{
		public:
			SetCharacterCollisionsHelper(Ped ^ped) : CustomHelper(ped, "setCharacterCollisions")
			{
			}

			property float Spin
			{
				void set(float value)
				{
					SetArgument("spin", value);
				}
			}
			property float MaxVelocity
			{
				void set(float value)
				{
					SetArgument("maxVelocity", value);
				}
			}
			property bool ApplyToAll
			{
				void set(bool value)
				{
					SetArgument("applyToAll", value);
				}
			}
			property bool ApplyToSpine
			{
				void set(bool value)
				{
					SetArgument("applyToSpine", value);
				}
			}
			property bool ApplyToThighs
			{
				void set(bool value)
				{
					SetArgument("applyToThighs", value);
				}
			}
			property bool ApplyToClavicles
			{
				void set(bool value)
				{
					SetArgument("applyToClavicles", value);
				}
			}
			property bool ApplyToUpperArms
			{
				void set(bool value)
				{
					SetArgument("applyToUpperArms", value);
				}
			}
			property bool FootSlip
			{
				void set(bool value)
				{
					SetArgument("footSlip", value);
				}
			}
			property int VehicleClass
			{
				void set(int value)
				{
					SetArgument("vehicleClass", value);
				}
			}
		};
		public ref class SetCharacterDampingHelper sealed : public CustomHelper
		{
		public:
			SetCharacterDampingHelper(Ped ^ped) : CustomHelper(ped, "setCharacterDamping")
			{
			}

			property float SomersaultThresh
			{
				void set(float value)
				{
					SetArgument("somersaultThresh", value);
				}
			}
			property float SomersaultDamp
			{
				void set(float value)
				{
					SetArgument("somersaultDamp", value);
				}
			}
			property float CartwheelThresh
			{
				void set(float value)
				{
					SetArgument("cartwheelThresh", value);
				}
			}
			property float CartwheelDamp
			{
				void set(float value)
				{
					SetArgument("cartwheelDamp", value);
				}
			}
			property float VehicleCollisionTime
			{
				void set(float value)
				{
					SetArgument("vehicleCollisionTime", value);
				}
			}
			property bool V2
			{
				void set(bool value)
				{
					SetArgument("v2", value);
				}
			}
		};
		public ref class SetFrictionScaleHelper sealed : public CustomHelper
		{
		public:
			SetFrictionScaleHelper(Ped ^ped) : CustomHelper(ped, "setFrictionScale")
			{
			}

			property float Scale
			{
				void set(float value)
				{
					SetArgument("scale", value);
				}
			}
			property float GlobalMin
			{
				void set(float value)
				{
					SetArgument("globalMin", value);
				}
			}
			property float GlobalMax
			{
				void set(float value)
				{
					SetArgument("globalMax", value);
				}
			}
			property System::String ^Mask
			{
				void set(System::String ^value)
				{
					SetArgument("mask", value);
				}
			}
		};
		public ref class AnimPoseHelper sealed : public CustomHelper
		{
		public:
			AnimPoseHelper(Ped ^ped) : CustomHelper(ped, "animPose")
			{
			}

			property float MuscleStiffness
			{
				void set(float value)
				{
					SetArgument("muscleStiffness", value);
				}
			}
			property float Stiffness
			{
				void set(float value)
				{
					SetArgument("stiffness", value);
				}
			}
			property float Damping
			{
				void set(float value)
				{
					SetArgument("damping", value);
				}
			}
			property System::String ^EffectorMask
			{
				void set(System::String ^value)
				{
					SetArgument("effectorMask", value);
				}
			}
			property bool OverideHeadlook
			{
				void set(bool value)
				{
					SetArgument("overideHeadlook", value);
				}
			}
			property bool OveridePointArm
			{
				void set(bool value)
				{
					SetArgument("overidePointArm", value);
				}
			}
			property bool OveridePointGun
			{
				void set(bool value)
				{
					SetArgument("overidePointGun", value);
				}
			}
			property bool UseZMPGravityCompensation
			{
				void set(bool value)
				{
					SetArgument("useZMPGravityCompensation", value);
				}
			}
			property float GravityCompensation
			{
				void set(float value)
				{
					SetArgument("gravityCompensation", value);
				}
			}
			property float MuscleStiffnessLeftArm
			{
				void set(float value)
				{
					SetArgument("muscleStiffnessLeftArm", value);
				}
			}
			property float MuscleStiffnessRightArm
			{
				void set(float value)
				{
					SetArgument("muscleStiffnessRightArm", value);
				}
			}
			property float MuscleStiffnessSpine
			{
				void set(float value)
				{
					SetArgument("muscleStiffnessSpine", value);
				}
			}
			property float MuscleStiffnessLeftLeg
			{
				void set(float value)
				{
					SetArgument("muscleStiffnessLeftLeg", value);
				}
			}
			property float MuscleStiffnessRightLeg
			{
				void set(float value)
				{
					SetArgument("muscleStiffnessRightLeg", value);
				}
			}
			property float StiffnessLeftArm
			{
				void set(float value)
				{
					SetArgument("stiffnessLeftArm", value);
				}
			}
			property float StiffnessRightArm
			{
				void set(float value)
				{
					SetArgument("stiffnessRightArm", value);
				}
			}
			property float StiffnessSpine
			{
				void set(float value)
				{
					SetArgument("stiffnessSpine", value);
				}
			}
			property float StiffnessLeftLeg
			{
				void set(float value)
				{
					SetArgument("stiffnessLeftLeg", value);
				}
			}
			property float StiffnessRightLeg
			{
				void set(float value)
				{
					SetArgument("stiffnessRightLeg", value);
				}
			}
			property float DampingLeftArm
			{
				void set(float value)
				{
					SetArgument("dampingLeftArm", value);
				}
			}
			property float DampingRightArm
			{
				void set(float value)
				{
					SetArgument("dampingRightArm", value);
				}
			}
			property float DampingSpine
			{
				void set(float value)
				{
					SetArgument("dampingSpine", value);
				}
			}
			property float DampingLeftLeg
			{
				void set(float value)
				{
					SetArgument("dampingLeftLeg", value);
				}
			}
			property float DampingRightLeg
			{
				void set(float value)
				{
					SetArgument("dampingRightLeg", value);
				}
			}
			property float GravCompLeftArm
			{
				void set(float value)
				{
					SetArgument("gravCompLeftArm", value);
				}
			}
			property float GravCompRightArm
			{
				void set(float value)
				{
					SetArgument("gravCompRightArm", value);
				}
			}
			property float GravCompSpine
			{
				void set(float value)
				{
					SetArgument("gravCompSpine", value);
				}
			}
			property float GravCompLeftLeg
			{
				void set(float value)
				{
					SetArgument("gravCompLeftLeg", value);
				}
			}
			property float GravCompRightLeg
			{
				void set(float value)
				{
					SetArgument("gravCompRightLeg", value);
				}
			}
			property int ConnectedLeftHand
			{
				void set(int value)
				{
					SetArgument("connectedLeftHand", value);
				}
			}
			property int ConnectedRightHand
			{
				void set(int value)
				{
					SetArgument("connectedRightHand", value);
				}
			}
			property int ConnectedLeftFoot
			{
				void set(int value)
				{
					SetArgument("connectedLeftFoot", value);
				}
			}
			property int ConnectedRightFoot
			{
				void set(int value)
				{
					SetArgument("connectedRightFoot", value);
				}
			}
			property int AnimSource
			{
				void set(int value)
				{
					SetArgument("animSource", value);
				}
			}
			property int DampenSideMotionInstanceIndex
			{
				void set(int value)
				{
					SetArgument("dampenSideMotionInstanceIndex", value);
				}
			}
		};
		public ref class ArmsWindmillHelper sealed : public CustomHelper
		{
		public:
			ArmsWindmillHelper(Ped ^ped) : CustomHelper(ped, "armsWindmill")
			{
			}

			property int LeftPartID
			{
				void set(int value)
				{
					SetArgument("leftPartID", value);
				}
			}
			property float LeftRadius1
			{
				void set(float value)
				{
					SetArgument("leftRadius1", value);
				}
			}
			property float LeftRadius2
			{
				void set(float value)
				{
					SetArgument("leftRadius2", value);
				}
			}
			property float LeftSpeed
			{
				void set(float value)
				{
					SetArgument("leftSpeed", value);
				}
			}
			property Math::Vector3 LeftNormal
			{
				void set(Math::Vector3 value)
				{
					SetArgument("leftNormal", value);
				}
			}
			property Math::Vector3 LeftCentre
			{
				void set(Math::Vector3 value)
				{
					SetArgument("leftCentre", value);
				}
			}
			property int RightPartID
			{
				void set(int value)
				{
					SetArgument("rightPartID", value);
				}
			}
			property float RightRadius1
			{
				void set(float value)
				{
					SetArgument("rightRadius1", value);
				}
			}
			property float RightRadius2
			{
				void set(float value)
				{
					SetArgument("rightRadius2", value);
				}
			}
			property float RightSpeed
			{
				void set(float value)
				{
					SetArgument("rightSpeed", value);
				}
			}
			property Math::Vector3 RightNormal
			{
				void set(Math::Vector3 value)
				{
					SetArgument("rightNormal", value);
				}
			}
			property Math::Vector3 RightCentre
			{
				void set(Math::Vector3 value)
				{
					SetArgument("rightCentre", value);
				}
			}
			property float ShoulderStiffness
			{
				void set(float value)
				{
					SetArgument("shoulderStiffness", value);
				}
			}
			property float ShoulderDamping
			{
				void set(float value)
				{
					SetArgument("shoulderDamping", value);
				}
			}
			property float ElbowStiffness
			{
				void set(float value)
				{
					SetArgument("elbowStiffness", value);
				}
			}
			property float ElbowDamping
			{
				void set(float value)
				{
					SetArgument("elbowDamping", value);
				}
			}
			property float LeftElbowMin
			{
				void set(float value)
				{
					SetArgument("leftElbowMin", value);
				}
			}
			property float RightElbowMin
			{
				void set(float value)
				{
					SetArgument("rightElbowMin", value);
				}
			}
			property float PhaseOffset
			{
				void set(float value)
				{
					SetArgument("phaseOffset", value);
				}
			}
			property float DragReduction
			{
				void set(float value)
				{
					SetArgument("dragReduction", value);
				}
			}
			property float IKtwist
			{
				void set(float value)
				{
					SetArgument("IKtwist", value);
				}
			}
			property float AngVelThreshold
			{
				void set(float value)
				{
					SetArgument("angVelThreshold", value);
				}
			}
			property float AngVelGain
			{
				void set(float value)
				{
					SetArgument("angVelGain", value);
				}
			}
			property int MirrorMode
			{
				void set(int value)
				{
					SetArgument("mirrorMode", value);
				}
			}
			property int AdaptiveMode
			{
				void set(int value)
				{
					SetArgument("adaptiveMode", value);
				}
			}
			property bool ForceSync
			{
				void set(bool value)
				{
					SetArgument("forceSync", value);
				}
			}
			property bool UseLeft
			{
				void set(bool value)
				{
					SetArgument("useLeft", value);
				}
			}
			property bool UseRight
			{
				void set(bool value)
				{
					SetArgument("useRight", value);
				}
			}
			property bool DisableOnImpact
			{
				void set(bool value)
				{
					SetArgument("disableOnImpact", value);
				}
			}
		};
		public ref class ArmsWindmillAdaptiveHelper sealed : public CustomHelper
		{
		public:
			ArmsWindmillAdaptiveHelper(Ped ^ped) : CustomHelper(ped, "armsWindmillAdaptive")
			{
			}

			property float AngSpeed
			{
				void set(float value)
				{
					SetArgument("angSpeed", value);
				}
			}
			property float BodyStiffness
			{
				void set(float value)
				{
					SetArgument("bodyStiffness", value);
				}
			}
			property float Amplitude
			{
				void set(float value)
				{
					SetArgument("amplitude", value);
				}
			}
			property float Phase
			{
				void set(float value)
				{
					SetArgument("phase", value);
				}
			}
			property float ArmStiffness
			{
				void set(float value)
				{
					SetArgument("armStiffness", value);
				}
			}
			property float LeftElbowAngle
			{
				void set(float value)
				{
					SetArgument("leftElbowAngle", value);
				}
			}
			property float RightElbowAngle
			{
				void set(float value)
				{
					SetArgument("rightElbowAngle", value);
				}
			}
			property float Lean1mult
			{
				void set(float value)
				{
					SetArgument("lean1mult", value);
				}
			}
			property float Lean1offset
			{
				void set(float value)
				{
					SetArgument("lean1offset", value);
				}
			}
			property float ElbowRate
			{
				void set(float value)
				{
					SetArgument("elbowRate", value);
				}
			}
			property ArmDirections ArmDirection
			{
				void set(ArmDirections value)
				{
					SetArgument("armDirection", static_cast<int>(value));
				}
			}
			property bool DisableOnImpact
			{
				void set(bool value)
				{
					SetArgument("disableOnImpact", value);
				}
			}
			property bool SetBackAngles
			{
				void set(bool value)
				{
					SetArgument("setBackAngles", value);
				}
			}
			property bool UseAngMom
			{
				void set(bool value)
				{
					SetArgument("useAngMom", value);
				}
			}
			property bool BendLeftElbow
			{
				void set(bool value)
				{
					SetArgument("bendLeftElbow", value);
				}
			}
			property bool BendRightElbow
			{
				void set(bool value)
				{
					SetArgument("bendRightElbow", value);
				}
			}
			property System::String ^Mask
			{
				void set(System::String ^value)
				{
					SetArgument("mask", value);
				}
			}
		};
		public ref class BalancerCollisionsReactionHelper sealed : public CustomHelper
		{
		public:
			BalancerCollisionsReactionHelper(Ped ^ped) : CustomHelper(ped, "balancerCollisionsReaction")
			{
			}

			property int NumStepsTillSlump
			{
				void set(int value)
				{
					SetArgument("numStepsTillSlump", value);
				}
			}
			property float Stable2SlumpTime
			{
				void set(float value)
				{
					SetArgument("stable2SlumpTime", value);
				}
			}
			property float ExclusionZone
			{
				void set(float value)
				{
					SetArgument("exclusionZone", value);
				}
			}
			property float FootFrictionMultStart
			{
				void set(float value)
				{
					SetArgument("footFrictionMultStart", value);
				}
			}
			property float FootFrictionMultRate
			{
				void set(float value)
				{
					SetArgument("footFrictionMultRate", value);
				}
			}
			property float BackFrictionMultStart
			{
				void set(float value)
				{
					SetArgument("backFrictionMultStart", value);
				}
			}
			property float BackFrictionMultRate
			{
				void set(float value)
				{
					SetArgument("backFrictionMultRate", value);
				}
			}
			property float ImpactLegStiffReduction
			{
				void set(float value)
				{
					SetArgument("impactLegStiffReduction", value);
				}
			}
			property float SlumpLegStiffReduction
			{
				void set(float value)
				{
					SetArgument("slumpLegStiffReduction", value);
				}
			}
			property float SlumpLegStiffRate
			{
				void set(float value)
				{
					SetArgument("slumpLegStiffRate", value);
				}
			}
			property float ReactTime
			{
				void set(float value)
				{
					SetArgument("reactTime", value);
				}
			}
			property float ImpactExagTime
			{
				void set(float value)
				{
					SetArgument("impactExagTime", value);
				}
			}
			property float GlanceSpinTime
			{
				void set(float value)
				{
					SetArgument("glanceSpinTime", value);
				}
			}
			property float GlanceSpinMag
			{
				void set(float value)
				{
					SetArgument("glanceSpinMag", value);
				}
			}
			property float GlanceSpinDecayMult
			{
				void set(float value)
				{
					SetArgument("glanceSpinDecayMult", value);
				}
			}
			property int IgnoreColWithIndex
			{
				void set(int value)
				{
					SetArgument("ignoreColWithIndex", value);
				}
			}
			property int SlumpMode
			{
				void set(int value)
				{
					SetArgument("slumpMode", value);
				}
			}
			property int ReboundMode
			{
				void set(int value)
				{
					SetArgument("reboundMode", value);
				}
			}
			property float IgnoreColMassBelow
			{
				void set(float value)
				{
					SetArgument("ignoreColMassBelow", value);
				}
			}
			property int ForwardMode
			{
				void set(int value)
				{
					SetArgument("forwardMode", value);
				}
			}
			property float TimeToForward
			{
				void set(float value)
				{
					SetArgument("timeToForward", value);
				}
			}
			property float ReboundForce
			{
				void set(float value)
				{
					SetArgument("reboundForce", value);
				}
			}
			property bool BraceWall
			{
				void set(bool value)
				{
					SetArgument("braceWall", value);
				}
			}
			property float IgnoreColVolumeBelow
			{
				void set(float value)
				{
					SetArgument("ignoreColVolumeBelow", value);
				}
			}
			property bool FallOverWallDrape
			{
				void set(bool value)
				{
					SetArgument("fallOverWallDrape", value);
				}
			}
			property bool FallOverHighWalls
			{
				void set(bool value)
				{
					SetArgument("fallOverHighWalls", value);
				}
			}
			property bool Snap
			{
				void set(bool value)
				{
					SetArgument("snap", value);
				}
			}
			property float SnapMag
			{
				void set(float value)
				{
					SetArgument("snapMag", value);
				}
			}
			property float SnapDirectionRandomness
			{
				void set(float value)
				{
					SetArgument("snapDirectionRandomness", value);
				}
			}
			property bool SnapLeftArm
			{
				void set(bool value)
				{
					SetArgument("snapLeftArm", value);
				}
			}
			property bool SnapRightArm
			{
				void set(bool value)
				{
					SetArgument("snapRightArm", value);
				}
			}
			property bool SnapLeftLeg
			{
				void set(bool value)
				{
					SetArgument("snapLeftLeg", value);
				}
			}
			property bool SnapRightLeg
			{
				void set(bool value)
				{
					SetArgument("snapRightLeg", value);
				}
			}
			property bool SnapSpine
			{
				void set(bool value)
				{
					SetArgument("snapSpine", value);
				}
			}
			property bool SnapNeck
			{
				void set(bool value)
				{
					SetArgument("snapNeck", value);
				}
			}
			property bool SnapPhasedLegs
			{
				void set(bool value)
				{
					SetArgument("snapPhasedLegs", value);
				}
			}
			property int SnapHipType
			{
				void set(int value)
				{
					SetArgument("snapHipType", value);
				}
			}
			property float UnSnapInterval
			{
				void set(float value)
				{
					SetArgument("unSnapInterval", value);
				}
			}
			property float UnSnapRatio
			{
				void set(float value)
				{
					SetArgument("unSnapRatio", value);
				}
			}
			property bool SnapUseTorques
			{
				void set(bool value)
				{
					SetArgument("snapUseTorques", value);
				}
			}
			property float ImpactWeaknessZeroDuration
			{
				void set(float value)
				{
					SetArgument("impactWeaknessZeroDuration", value);
				}
			}
			property float ImpactWeaknessRampDuration
			{
				void set(float value)
				{
					SetArgument("impactWeaknessRampDuration", value);
				}
			}
			property float ImpactLoosenessAmount
			{
				void set(float value)
				{
					SetArgument("impactLoosenessAmount", value);
				}
			}
			property bool ObjectBehindVictim
			{
				void set(bool value)
				{
					SetArgument("objectBehindVictim", value);
				}
			}
			property Math::Vector3 ObjectBehindVictimPos
			{
				void set(Math::Vector3 value)
				{
					SetArgument("objectBehindVictimPos", value);
				}
			}
			property Math::Vector3 ObjectBehindVictimNormal
			{
				void set(Math::Vector3 value)
				{
					SetArgument("objectBehindVictimNormal", value);
				}
			}
		};
		public ref class BodyBalanceHelper sealed : public CustomHelper
		{
		public:
			BodyBalanceHelper(Ped ^ped) : CustomHelper(ped, "bodyBalance")
			{
			}

			property float ArmStiffness
			{
				void set(float value)
				{
					SetArgument("armStiffness", value);
				}
			}
			property float Elbow
			{
				void set(float value)
				{
					SetArgument("elbow", value);
				}
			}
			property float Shoulder
			{
				void set(float value)
				{
					SetArgument("shoulder", value);
				}
			}
			property float ArmDamping
			{
				void set(float value)
				{
					SetArgument("armDamping", value);
				}
			}
			property bool UseHeadLook
			{
				void set(bool value)
				{
					SetArgument("useHeadLook", value);
				}
			}
			property Math::Vector3 HeadLookPos
			{
				void set(Math::Vector3 value)
				{
					SetArgument("headLookPos", value);
				}
			}
			property int HeadLookInstanceIndex
			{
				void set(int value)
				{
					SetArgument("headLookInstanceIndex", value);
				}
			}
			property float SpineStiffness
			{
				void set(float value)
				{
					SetArgument("spineStiffness", value);
				}
			}
			property float SomersaultAngle
			{
				void set(float value)
				{
					SetArgument("somersaultAngle", value);
				}
			}
			property float SomersaultAngleThreshold
			{
				void set(float value)
				{
					SetArgument("somersaultAngleThreshold", value);
				}
			}
			property float SideSomersaultAngle
			{
				void set(float value)
				{
					SetArgument("sideSomersaultAngle", value);
				}
			}
			property float SideSomersaultAngleThreshold
			{
				void set(float value)
				{
					SetArgument("sideSomersaultAngleThreshold", value);
				}
			}
			property bool BackwardsAutoTurn
			{
				void set(bool value)
				{
					SetArgument("backwardsAutoTurn", value);
				}
			}
			property float TurnWithBumpRadius
			{
				void set(float value)
				{
					SetArgument("turnWithBumpRadius", value);
				}
			}
			property bool BackwardsArms
			{
				void set(bool value)
				{
					SetArgument("backwardsArms", value);
				}
			}
			property bool BlendToZeroPose
			{
				void set(bool value)
				{
					SetArgument("blendToZeroPose", value);
				}
			}
			property bool ArmsOutOnPush
			{
				void set(bool value)
				{
					SetArgument("armsOutOnPush", value);
				}
			}
			property float ArmsOutOnPushMultiplier
			{
				void set(float value)
				{
					SetArgument("armsOutOnPushMultiplier", value);
				}
			}
			property float ArmsOutOnPushTimeout
			{
				void set(float value)
				{
					SetArgument("armsOutOnPushTimeout", value);
				}
			}
			property float ReturningToBalanceArmsOut
			{
				void set(float value)
				{
					SetArgument("returningToBalanceArmsOut", value);
				}
			}
			property float ArmsOutStraightenElbows
			{
				void set(float value)
				{
					SetArgument("armsOutStraightenElbows", value);
				}
			}
			property float ArmsOutMinLean2
			{
				void set(float value)
				{
					SetArgument("armsOutMinLean2", value);
				}
			}
			property float SpineDamping
			{
				void set(float value)
				{
					SetArgument("spineDamping", value);
				}
			}
			property bool UseBodyTurn
			{
				void set(bool value)
				{
					SetArgument("useBodyTurn", value);
				}
			}
			property float ElbowAngleOnContact
			{
				void set(float value)
				{
					SetArgument("elbowAngleOnContact", value);
				}
			}
			property float BendElbowsTime
			{
				void set(float value)
				{
					SetArgument("bendElbowsTime", value);
				}
			}
			property float BendElbowsGait
			{
				void set(float value)
				{
					SetArgument("bendElbowsGait", value);
				}
			}
			property float HipL2ArmL2
			{
				void set(float value)
				{
					SetArgument("hipL2ArmL2", value);
				}
			}
			property float ShoulderL2
			{
				void set(float value)
				{
					SetArgument("shoulderL2", value);
				}
			}
			property float ShoulderL1
			{
				void set(float value)
				{
					SetArgument("shoulderL1", value);
				}
			}
			property float ShoulderTwist
			{
				void set(float value)
				{
					SetArgument("shoulderTwist", value);
				}
			}
			property float HeadLookAtVelProb
			{
				void set(float value)
				{
					SetArgument("headLookAtVelProb", value);
				}
			}
			property float TurnOffProb
			{
				void set(float value)
				{
					SetArgument("turnOffProb", value);
				}
			}
			property float Turn2VelProb
			{
				void set(float value)
				{
					SetArgument("turn2VelProb", value);
				}
			}
			property float TurnAwayProb
			{
				void set(float value)
				{
					SetArgument("turnAwayProb", value);
				}
			}
			property float TurnLeftProb
			{
				void set(float value)
				{
					SetArgument("turnLeftProb", value);
				}
			}
			property float TurnRightProb
			{
				void set(float value)
				{
					SetArgument("turnRightProb", value);
				}
			}
			property float Turn2TargetProb
			{
				void set(float value)
				{
					SetArgument("turn2TargetProb", value);
				}
			}
			property Math::Vector3 AngVelMultiplier
			{
				void set(Math::Vector3 value)
				{
					SetArgument("angVelMultiplier", value);
				}
			}
			property Math::Vector3 AngVelThreshold
			{
				void set(Math::Vector3 value)
				{
					SetArgument("angVelThreshold", value);
				}
			}
			property float BraceDistance
			{
				void set(float value)
				{
					SetArgument("braceDistance", value);
				}
			}
			property float TargetPredictionTime
			{
				void set(float value)
				{
					SetArgument("targetPredictionTime", value);
				}
			}
			property float ReachAbsorbtionTime
			{
				void set(float value)
				{
					SetArgument("reachAbsorbtionTime", value);
				}
			}
			property float BraceStiffness
			{
				void set(float value)
				{
					SetArgument("braceStiffness", value);
				}
			}
			property float MinBraceTime
			{
				void set(float value)
				{
					SetArgument("minBraceTime", value);
				}
			}
			property float TimeToBackwardsBrace
			{
				void set(float value)
				{
					SetArgument("timeToBackwardsBrace", value);
				}
			}
			property float HandsDelayMin
			{
				void set(float value)
				{
					SetArgument("handsDelayMin", value);
				}
			}
			property float HandsDelayMax
			{
				void set(float value)
				{
					SetArgument("handsDelayMax", value);
				}
			}
			property float BraceOffset
			{
				void set(float value)
				{
					SetArgument("braceOffset", value);
				}
			}
			property float MoveRadius
			{
				void set(float value)
				{
					SetArgument("moveRadius", value);
				}
			}
			property float MoveAmount
			{
				void set(float value)
				{
					SetArgument("moveAmount", value);
				}
			}
			property bool MoveWhenBracing
			{
				void set(bool value)
				{
					SetArgument("moveWhenBracing", value);
				}
			}
		};
		public ref class BodyFoetalHelper sealed : public CustomHelper
		{
		public:
			BodyFoetalHelper(Ped ^ped) : CustomHelper(ped, "bodyFoetal")
			{
			}

			property float Stiffness
			{
				void set(float value)
				{
					SetArgument("stiffness", value);
				}
			}
			property float DampingFactor
			{
				void set(float value)
				{
					SetArgument("dampingFactor", value);
				}
			}
			property float Asymmetry
			{
				void set(float value)
				{
					SetArgument("asymmetry", value);
				}
			}
			property int RandomSeed
			{
				void set(int value)
				{
					SetArgument("randomSeed", value);
				}
			}
			property float BackTwist
			{
				void set(float value)
				{
					SetArgument("backTwist", value);
				}
			}
			property System::String ^Mask
			{
				void set(System::String ^value)
				{
					SetArgument("mask", value);
				}
			}
		};
		public ref class BodyRollUpHelper sealed : public CustomHelper
		{
		public:
			BodyRollUpHelper(Ped ^ped) : CustomHelper(ped, "bodyRollUp")
			{
			}

			property float Stiffness
			{
				void set(float value)
				{
					SetArgument("stiffness", value);
				}
			}
			property float UseArmToSlowDown
			{
				void set(float value)
				{
					SetArgument("useArmToSlowDown", value);
				}
			}
			property float ArmReachAmount
			{
				void set(float value)
				{
					SetArgument("armReachAmount", value);
				}
			}
			property System::String ^Mask
			{
				void set(System::String ^value)
				{
					SetArgument("mask", value);
				}
			}
			property float LegPush
			{
				void set(float value)
				{
					SetArgument("legPush", value);
				}
			}
			property float AsymmetricalLegs
			{
				void set(float value)
				{
					SetArgument("asymmetricalLegs", value);
				}
			}
			property float NoRollTimeBeforeSuccess
			{
				void set(float value)
				{
					SetArgument("noRollTimeBeforeSuccess", value);
				}
			}
			property float RollVelForSuccess
			{
				void set(float value)
				{
					SetArgument("rollVelForSuccess", value);
				}
			}
			property float RollVelLinearContribution
			{
				void set(float value)
				{
					SetArgument("rollVelLinearContribution", value);
				}
			}
			property float VelocityScale
			{
				void set(float value)
				{
					SetArgument("velocityScale", value);
				}
			}
			property float VelocityOffset
			{
				void set(float value)
				{
					SetArgument("velocityOffset", value);
				}
			}
			property bool ApplyMinMaxFriction
			{
				void set(bool value)
				{
					SetArgument("applyMinMaxFriction", value);
				}
			}
		};
		public ref class BodyWritheHelper sealed : public CustomHelper
		{
		public:
			BodyWritheHelper(Ped ^ped) : CustomHelper(ped, "bodyWrithe")
			{
			}

			property float ArmStiffness
			{
				void set(float value)
				{
					SetArgument("armStiffness", value);
				}
			}
			property float BackStiffness
			{
				void set(float value)
				{
					SetArgument("backStiffness", value);
				}
			}
			property float LegStiffness
			{
				void set(float value)
				{
					SetArgument("legStiffness", value);
				}
			}
			property float ArmDamping
			{
				void set(float value)
				{
					SetArgument("armDamping", value);
				}
			}
			property float BackDamping
			{
				void set(float value)
				{
					SetArgument("backDamping", value);
				}
			}
			property float LegDamping
			{
				void set(float value)
				{
					SetArgument("legDamping", value);
				}
			}
			property float ArmPeriod
			{
				void set(float value)
				{
					SetArgument("armPeriod", value);
				}
			}
			property float BackPeriod
			{
				void set(float value)
				{
					SetArgument("backPeriod", value);
				}
			}
			property float LegPeriod
			{
				void set(float value)
				{
					SetArgument("legPeriod", value);
				}
			}
			property System::String ^Mask
			{
				void set(System::String ^value)
				{
					SetArgument("mask", value);
				}
			}
			property float ArmAmplitude
			{
				void set(float value)
				{
					SetArgument("armAmplitude", value);
				}
			}
			property float BackAmplitude
			{
				void set(float value)
				{
					SetArgument("backAmplitude", value);
				}
			}
			property float LegAmplitude
			{
				void set(float value)
				{
					SetArgument("legAmplitude", value);
				}
			}
			property float ElbowAmplitude
			{
				void set(float value)
				{
					SetArgument("elbowAmplitude", value);
				}
			}
			property float KneeAmplitude
			{
				void set(float value)
				{
					SetArgument("kneeAmplitude", value);
				}
			}
			property bool RollOverFlag
			{
				void set(bool value)
				{
					SetArgument("rollOverFlag", value);
				}
			}
			property float BlendArms
			{
				void set(float value)
				{
					SetArgument("blendArms", value);
				}
			}
			property float BlendBack
			{
				void set(float value)
				{
					SetArgument("blendBack", value);
				}
			}
			property float BlendLegs
			{
				void set(float value)
				{
					SetArgument("blendLegs", value);
				}
			}
			property bool ApplyStiffness
			{
				void set(bool value)
				{
					SetArgument("applyStiffness", value);
				}
			}
			property bool OnFire
			{
				void set(bool value)
				{
					SetArgument("onFire", value);
				}
			}
			property float ShoulderLean1
			{
				void set(float value)
				{
					SetArgument("shoulderLean1", value);
				}
			}
			property float ShoulderLean2
			{
				void set(float value)
				{
					SetArgument("shoulderLean2", value);
				}
			}
			property float Lean1BlendFactor
			{
				void set(float value)
				{
					SetArgument("lean1BlendFactor", value);
				}
			}
			property float Lean2BlendFactor
			{
				void set(float value)
				{
					SetArgument("lean2BlendFactor", value);
				}
			}
			property float RollTorqueScale
			{
				void set(float value)
				{
					SetArgument("rollTorqueScale", value);
				}
			}
			property float MaxRollOverTime
			{
				void set(float value)
				{
					SetArgument("maxRollOverTime", value);
				}
			}
			property float RollOverRadius
			{
				void set(float value)
				{
					SetArgument("rollOverRadius", value);
				}
			}
		};
		public ref class BraceForImpactHelper sealed : public CustomHelper
		{
		public:
			BraceForImpactHelper(Ped ^ped) : CustomHelper(ped, "braceForImpact")
			{
			}

			property float BraceDistance
			{
				void set(float value)
				{
					SetArgument("braceDistance", value);
				}
			}
			property float TargetPredictionTime
			{
				void set(float value)
				{
					SetArgument("targetPredictionTime", value);
				}
			}
			property float ReachAbsorbtionTime
			{
				void set(float value)
				{
					SetArgument("reachAbsorbtionTime", value);
				}
			}
			property int InstanceIndex
			{
				void set(int value)
				{
					SetArgument("instanceIndex", value);
				}
			}
			property float BodyStiffness
			{
				void set(float value)
				{
					SetArgument("bodyStiffness", value);
				}
			}
			property bool GrabDontLetGo
			{
				void set(bool value)
				{
					SetArgument("grabDontLetGo", value);
				}
			}
			property float GrabStrength
			{
				void set(float value)
				{
					SetArgument("grabStrength", value);
				}
			}
			property float GrabDistance
			{
				void set(float value)
				{
					SetArgument("grabDistance", value);
				}
			}
			property float GrabReachAngle
			{
				void set(float value)
				{
					SetArgument("grabReachAngle", value);
				}
			}
			property float GrabHoldTimer
			{
				void set(float value)
				{
					SetArgument("grabHoldTimer", value);
				}
			}
			property float MaxGrabCarVelocity
			{
				void set(float value)
				{
					SetArgument("maxGrabCarVelocity", value);
				}
			}
			property float LegStiffness
			{
				void set(float value)
				{
					SetArgument("legStiffness", value);
				}
			}
			property float TimeToBackwardsBrace
			{
				void set(float value)
				{
					SetArgument("timeToBackwardsBrace", value);
				}
			}
			property Math::Vector3 Look
			{
				void set(Math::Vector3 value)
				{
					SetArgument("look", value);
				}
			}
			property Math::Vector3 Pos
			{
				void set(Math::Vector3 value)
				{
					SetArgument("pos", value);
				}
			}
			property float MinBraceTime
			{
				void set(float value)
				{
					SetArgument("minBraceTime", value);
				}
			}
			property float HandsDelayMin
			{
				void set(float value)
				{
					SetArgument("handsDelayMin", value);
				}
			}
			property float HandsDelayMax
			{
				void set(float value)
				{
					SetArgument("handsDelayMax", value);
				}
			}
			property bool MoveAway
			{
				void set(bool value)
				{
					SetArgument("moveAway", value);
				}
			}
			property float MoveAwayAmount
			{
				void set(float value)
				{
					SetArgument("moveAwayAmount", value);
				}
			}
			property float MoveAwayLean
			{
				void set(float value)
				{
					SetArgument("moveAwayLean", value);
				}
			}
			property float MoveSideways
			{
				void set(float value)
				{
					SetArgument("moveSideways", value);
				}
			}
			property bool BbArms
			{
				void set(bool value)
				{
					SetArgument("bbArms", value);
				}
			}
			property bool NewBrace
			{
				void set(bool value)
				{
					SetArgument("newBrace", value);
				}
			}
			property bool BraceOnImpact
			{
				void set(bool value)
				{
					SetArgument("braceOnImpact", value);
				}
			}
			property bool Roll2Velocity
			{
				void set(bool value)
				{
					SetArgument("roll2Velocity", value);
				}
			}
			property int RollType
			{
				void set(int value)
				{
					SetArgument("rollType", value);
				}
			}
			property bool SnapImpacts
			{
				void set(bool value)
				{
					SetArgument("snapImpacts", value);
				}
			}
			property float SnapImpact
			{
				void set(float value)
				{
					SetArgument("snapImpact", value);
				}
			}
			property float SnapBonnet
			{
				void set(float value)
				{
					SetArgument("snapBonnet", value);
				}
			}
			property float SnapFloor
			{
				void set(float value)
				{
					SetArgument("snapFloor", value);
				}
			}
			property bool DampVel
			{
				void set(bool value)
				{
					SetArgument("dampVel", value);
				}
			}
			property float DampSpin
			{
				void set(float value)
				{
					SetArgument("dampSpin", value);
				}
			}
			property float DampUpVel
			{
				void set(float value)
				{
					SetArgument("dampUpVel", value);
				}
			}
			property float DampSpinThresh
			{
				void set(float value)
				{
					SetArgument("dampSpinThresh", value);
				}
			}
			property float DampUpVelThresh
			{
				void set(float value)
				{
					SetArgument("dampUpVelThresh", value);
				}
			}
			property bool GsHelp
			{
				void set(bool value)
				{
					SetArgument("gsHelp", value);
				}
			}
			property float GsEndMin
			{
				void set(float value)
				{
					SetArgument("gsEndMin", value);
				}
			}
			property float GsSideMin
			{
				void set(float value)
				{
					SetArgument("gsSideMin", value);
				}
			}
			property float GsSideMax
			{
				void set(float value)
				{
					SetArgument("gsSideMax", value);
				}
			}
			property float GsUpness
			{
				void set(float value)
				{
					SetArgument("gsUpness", value);
				}
			}
			property float GsCarVelMin
			{
				void set(float value)
				{
					SetArgument("gsCarVelMin", value);
				}
			}
			property bool GsScale1Foot
			{
				void set(bool value)
				{
					SetArgument("gsScale1Foot", value);
				}
			}
			property float GsFricScale1
			{
				void set(float value)
				{
					SetArgument("gsFricScale1", value);
				}
			}
			property System::String ^GsFricMask1
			{
				void set(System::String ^value)
				{
					SetArgument("gsFricMask1", value);
				}
			}
			property float GsFricScale2
			{
				void set(float value)
				{
					SetArgument("gsFricScale2", value);
				}
			}
			property System::String ^GsFricMask2
			{
				void set(System::String ^value)
				{
					SetArgument("gsFricMask2", value);
				}
			}
		};
		public ref class BuoyancyHelper sealed : public CustomHelper
		{
		public:
			BuoyancyHelper(Ped ^ped) : CustomHelper(ped, "buoyancy")
			{
			}

			property Math::Vector3 SurfacePoint
			{
				void set(Math::Vector3 value)
				{
					SetArgument("surfacePoint", value);
				}
			}
			property Math::Vector3 SurfaceNormal
			{
				void set(Math::Vector3 value)
				{
					SetArgument("surfaceNormal", value);
				}
			}
			property float Buoyancy
			{
				void set(float value)
				{
					SetArgument("buoyancy", value);
				}
			}
			property float ChestBuoyancy
			{
				void set(float value)
				{
					SetArgument("chestBuoyancy", value);
				}
			}
			property float Damping
			{
				void set(float value)
				{
					SetArgument("damping", value);
				}
			}
			property bool Righting
			{
				void set(bool value)
				{
					SetArgument("righting", value);
				}
			}
			property float RightingStrength
			{
				void set(float value)
				{
					SetArgument("rightingStrength", value);
				}
			}
			property float RightingTime
			{
				void set(float value)
				{
					SetArgument("rightingTime", value);
				}
			}
		};
		public ref class CatchFallHelper sealed : public CustomHelper
		{
		public:
			CatchFallHelper(Ped ^ped) : CustomHelper(ped, "catchFall")
			{
			}

			property float TorsoStiffness
			{
				void set(float value)
				{
					SetArgument("torsoStiffness", value);
				}
			}
			property float LegsStiffness
			{
				void set(float value)
				{
					SetArgument("legsStiffness", value);
				}
			}
			property float ArmsStiffness
			{
				void set(float value)
				{
					SetArgument("armsStiffness", value);
				}
			}
			property float BackwardsMinArmOffset
			{
				void set(float value)
				{
					SetArgument("backwardsMinArmOffset", value);
				}
			}
			property float ForwardMaxArmOffset
			{
				void set(float value)
				{
					SetArgument("forwardMaxArmOffset", value);
				}
			}
			property float ZAxisSpinReduction
			{
				void set(float value)
				{
					SetArgument("zAxisSpinReduction", value);
				}
			}
			property float ExtraSit
			{
				void set(float value)
				{
					SetArgument("extraSit", value);
				}
			}
			property bool UseHeadLook
			{
				void set(bool value)
				{
					SetArgument("useHeadLook", value);
				}
			}
			property System::String ^Mask
			{
				void set(System::String ^value)
				{
					SetArgument("mask", value);
				}
			}
		};
		public ref class ElectrocuteHelper sealed : public CustomHelper
		{
		public:
			ElectrocuteHelper(Ped ^ped) : CustomHelper(ped, "electrocute")
			{
			}

			property float StunMag
			{
				void set(float value)
				{
					SetArgument("stunMag", value);
				}
			}
			property float InitialMult
			{
				void set(float value)
				{
					SetArgument("initialMult", value);
				}
			}
			property float LargeMult
			{
				void set(float value)
				{
					SetArgument("largeMult", value);
				}
			}
			property float LargeMinTime
			{
				void set(float value)
				{
					SetArgument("largeMinTime", value);
				}
			}
			property float LargeMaxTime
			{
				void set(float value)
				{
					SetArgument("largeMaxTime", value);
				}
			}
			property float MovingMult
			{
				void set(float value)
				{
					SetArgument("movingMult", value);
				}
			}
			property float BalancingMult
			{
				void set(float value)
				{
					SetArgument("balancingMult", value);
				}
			}
			property float AirborneMult
			{
				void set(float value)
				{
					SetArgument("airborneMult", value);
				}
			}
			property float MovingThresh
			{
				void set(float value)
				{
					SetArgument("movingThresh", value);
				}
			}
			property float StunInterval
			{
				void set(float value)
				{
					SetArgument("stunInterval", value);
				}
			}
			property float DirectionRandomness
			{
				void set(float value)
				{
					SetArgument("directionRandomness", value);
				}
			}
			property bool LeftArm
			{
				void set(bool value)
				{
					SetArgument("leftArm", value);
				}
			}
			property bool RightArm
			{
				void set(bool value)
				{
					SetArgument("rightArm", value);
				}
			}
			property bool LeftLeg
			{
				void set(bool value)
				{
					SetArgument("leftLeg", value);
				}
			}
			property bool RightLeg
			{
				void set(bool value)
				{
					SetArgument("rightLeg", value);
				}
			}
			property bool Spine
			{
				void set(bool value)
				{
					SetArgument("spine", value);
				}
			}
			property bool Neck
			{
				void set(bool value)
				{
					SetArgument("neck", value);
				}
			}
			property bool PhasedLegs
			{
				void set(bool value)
				{
					SetArgument("phasedLegs", value);
				}
			}
			property bool ApplyStiffness
			{
				void set(bool value)
				{
					SetArgument("applyStiffness", value);
				}
			}
			property bool UseTorques
			{
				void set(bool value)
				{
					SetArgument("useTorques", value);
				}
			}
			property int HipType
			{
				void set(int value)
				{
					SetArgument("hipType", value);
				}
			}
		};
		public ref class FallOverWallHelper sealed : public CustomHelper
		{
		public:
			FallOverWallHelper(Ped ^ped) : CustomHelper(ped, "fallOverWall")
			{
			}

			property float BodyStiffness
			{
				void set(float value)
				{
					SetArgument("bodyStiffness", value);
				}
			}
			property float Damping
			{
				void set(float value)
				{
					SetArgument("damping", value);
				}
			}
			property float MagOfForce
			{
				void set(float value)
				{
					SetArgument("magOfForce", value);
				}
			}
			property float MaxDistanceFromPelToHitPoint
			{
				void set(float value)
				{
					SetArgument("maxDistanceFromPelToHitPoint", value);
				}
			}
			property float MaxForceDist
			{
				void set(float value)
				{
					SetArgument("maxForceDist", value);
				}
			}
			property float StepExclusionZone
			{
				void set(float value)
				{
					SetArgument("stepExclusionZone", value);
				}
			}
			property float MinLegHeight
			{
				void set(float value)
				{
					SetArgument("minLegHeight", value);
				}
			}
			property float BodyTwist
			{
				void set(float value)
				{
					SetArgument("bodyTwist", value);
				}
			}
			property float MaxTwist
			{
				void set(float value)
				{
					SetArgument("maxTwist", value);
				}
			}
			property Math::Vector3 FallOverWallEndA
			{
				void set(Math::Vector3 value)
				{
					SetArgument("fallOverWallEndA", value);
				}
			}
			property Math::Vector3 FallOverWallEndB
			{
				void set(Math::Vector3 value)
				{
					SetArgument("fallOverWallEndB", value);
				}
			}
			property float ForceAngleAbort
			{
				void set(float value)
				{
					SetArgument("forceAngleAbort", value);
				}
			}
			property float ForceTimeOut
			{
				void set(float value)
				{
					SetArgument("forceTimeOut", value);
				}
			}
			property bool MoveArms
			{
				void set(bool value)
				{
					SetArgument("moveArms", value);
				}
			}
			property bool MoveLegs
			{
				void set(bool value)
				{
					SetArgument("moveLegs", value);
				}
			}
			property bool BendSpine
			{
				void set(bool value)
				{
					SetArgument("bendSpine", value);
				}
			}
			property float AngleDirWithWallNormal
			{
				void set(float value)
				{
					SetArgument("angleDirWithWallNormal", value);
				}
			}
			property float LeaningAngleThreshold
			{
				void set(float value)
				{
					SetArgument("leaningAngleThreshold", value);
				}
			}
			property float MaxAngVel
			{
				void set(float value)
				{
					SetArgument("maxAngVel", value);
				}
			}
			property bool AdaptForcesToLowWall
			{
				void set(bool value)
				{
					SetArgument("adaptForcesToLowWall", value);
				}
			}
			property float MaxWallHeight
			{
				void set(float value)
				{
					SetArgument("maxWallHeight", value);
				}
			}
			property float DistanceToSendSuccessMessage
			{
				void set(float value)
				{
					SetArgument("distanceToSendSuccessMessage", value);
				}
			}
			property float RollingBackThr
			{
				void set(float value)
				{
					SetArgument("rollingBackThr", value);
				}
			}
			property float RollingPotential
			{
				void set(float value)
				{
					SetArgument("rollingPotential", value);
				}
			}
			property bool UseArmIK
			{
				void set(bool value)
				{
					SetArgument("useArmIK", value);
				}
			}
			property float ReachDistanceFromHitPoint
			{
				void set(float value)
				{
					SetArgument("reachDistanceFromHitPoint", value);
				}
			}
			property float MinReachDistanceFromHitPoint
			{
				void set(float value)
				{
					SetArgument("minReachDistanceFromHitPoint", value);
				}
			}
			property float AngleTotallyBack
			{
				void set(float value)
				{
					SetArgument("angleTotallyBack", value);
				}
			}
		};
		public ref class GrabHelper sealed : public CustomHelper
		{
		public:
			GrabHelper(Ped ^ped) : CustomHelper(ped, "grab")
			{
			}

			property bool UseLeft
			{
				void set(bool value)
				{
					SetArgument("useLeft", value);
				}
			}
			property bool UseRight
			{
				void set(bool value)
				{
					SetArgument("useRight", value);
				}
			}
			property bool DropWeaponIfNecessary
			{
				void set(bool value)
				{
					SetArgument("dropWeaponIfNecessary", value);
				}
			}
			property float DropWeaponDistance
			{
				void set(float value)
				{
					SetArgument("dropWeaponDistance", value);
				}
			}
			property float GrabStrength
			{
				void set(float value)
				{
					SetArgument("grabStrength", value);
				}
			}
			property float StickyHands
			{
				void set(float value)
				{
					SetArgument("stickyHands", value);
				}
			}
			property int TurnToTarget
			{
				void set(int value)
				{
					SetArgument("turnToTarget", value);
				}
			}
			property float GrabHoldMaxTimer
			{
				void set(float value)
				{
					SetArgument("grabHoldMaxTimer", value);
				}
			}
			property float PullUpTime
			{
				void set(float value)
				{
					SetArgument("pullUpTime", value);
				}
			}
			property float PullUpStrengthRight
			{
				void set(float value)
				{
					SetArgument("pullUpStrengthRight", value);
				}
			}
			property float PullUpStrengthLeft
			{
				void set(float value)
				{
					SetArgument("pullUpStrengthLeft", value);
				}
			}
			property Math::Vector3 Pos1
			{
				void set(Math::Vector3 value)
				{
					SetArgument("pos1", value);
				}
			}
			property Math::Vector3 Pos2
			{
				void set(Math::Vector3 value)
				{
					SetArgument("pos2", value);
				}
			}
			property Math::Vector3 Pos3
			{
				void set(Math::Vector3 value)
				{
					SetArgument("pos3", value);
				}
			}
			property Math::Vector3 Pos4
			{
				void set(Math::Vector3 value)
				{
					SetArgument("pos4", value);
				}
			}
			property Math::Vector3 NormalR
			{
				void set(Math::Vector3 value)
				{
					SetArgument("normalR", value);
				}
			}
			property Math::Vector3 NormalL
			{
				void set(Math::Vector3 value)
				{
					SetArgument("normalL", value);
				}
			}
			property Math::Vector3 NormalR2
			{
				void set(Math::Vector3 value)
				{
					SetArgument("normalR2", value);
				}
			}
			property Math::Vector3 NormalL2
			{
				void set(Math::Vector3 value)
				{
					SetArgument("normalL2", value);
				}
			}
			property bool HandsCollide
			{
				void set(bool value)
				{
					SetArgument("handsCollide", value);
				}
			}
			property bool JustBrace
			{
				void set(bool value)
				{
					SetArgument("justBrace", value);
				}
			}
			property bool UseLineGrab
			{
				void set(bool value)
				{
					SetArgument("useLineGrab", value);
				}
			}
			property bool PointsX4grab
			{
				void set(bool value)
				{
					SetArgument("pointsX4grab", value);
				}
			}
			property bool FromEA
			{
				void set(bool value)
				{
					SetArgument("fromEA", value);
				}
			}
			property bool SurfaceGrab
			{
				void set(bool value)
				{
					SetArgument("surfaceGrab", value);
				}
			}
			property int InstanceIndex
			{
				void set(int value)
				{
					SetArgument("instanceIndex", value);
				}
			}
			property int InstancePartIndex
			{
				void set(int value)
				{
					SetArgument("instancePartIndex", value);
				}
			}
			property bool DontLetGo
			{
				void set(bool value)
				{
					SetArgument("dontLetGo", value);
				}
			}
			property float BodyStiffness
			{
				void set(float value)
				{
					SetArgument("bodyStiffness", value);
				}
			}
			property float ReachAngle
			{
				void set(float value)
				{
					SetArgument("reachAngle", value);
				}
			}
			property float OneSideReachAngle
			{
				void set(float value)
				{
					SetArgument("oneSideReachAngle", value);
				}
			}
			property float GrabDistance
			{
				void set(float value)
				{
					SetArgument("grabDistance", value);
				}
			}
			property float Move2Radius
			{
				void set(float value)
				{
					SetArgument("move2Radius", value);
				}
			}
			property float ArmStiffness
			{
				void set(float value)
				{
					SetArgument("armStiffness", value);
				}
			}
			property float MaxReachDistance
			{
				void set(float value)
				{
					SetArgument("maxReachDistance", value);
				}
			}
			property float OrientationConstraintScale
			{
				void set(float value)
				{
					SetArgument("orientationConstraintScale", value);
				}
			}
			property float MaxWristAngle
			{
				void set(float value)
				{
					SetArgument("maxWristAngle", value);
				}
			}
			property bool UseHeadLookToTarget
			{
				void set(bool value)
				{
					SetArgument("useHeadLookToTarget", value);
				}
			}
			property bool LookAtGrab
			{
				void set(bool value)
				{
					SetArgument("lookAtGrab", value);
				}
			}
			property Math::Vector3 TargetForHeadLook
			{
				void set(Math::Vector3 value)
				{
					SetArgument("targetForHeadLook", value);
				}
			}
		};
		public ref class HeadLookHelper sealed : public CustomHelper
		{
		public:
			HeadLookHelper(Ped ^ped) : CustomHelper(ped, "headLook")
			{
			}

			property float Damping
			{
				void set(float value)
				{
					SetArgument("damping", value);
				}
			}
			property float Stiffness
			{
				void set(float value)
				{
					SetArgument("stiffness", value);
				}
			}
			property int InstanceIndex
			{
				void set(int value)
				{
					SetArgument("instanceIndex", value);
				}
			}
			property Math::Vector3 Vel
			{
				void set(Math::Vector3 value)
				{
					SetArgument("vel", value);
				}
			}
			property Math::Vector3 Pos
			{
				void set(Math::Vector3 value)
				{
					SetArgument("pos", value);
				}
			}
			property bool AlwaysLook
			{
				void set(bool value)
				{
					SetArgument("alwaysLook", value);
				}
			}
			property bool EyesHorizontal
			{
				void set(bool value)
				{
					SetArgument("eyesHorizontal", value);
				}
			}
			property bool AlwaysEyesHorizontal
			{
				void set(bool value)
				{
					SetArgument("alwaysEyesHorizontal", value);
				}
			}
			property bool KeepHeadAwayFromGround
			{
				void set(bool value)
				{
					SetArgument("keepHeadAwayFromGround", value);
				}
			}
			property bool TwistSpine
			{
				void set(bool value)
				{
					SetArgument("twistSpine", value);
				}
			}
		};
		public ref class HighFallHelper sealed : public CustomHelper
		{
		public:
			HighFallHelper(Ped ^ped) : CustomHelper(ped, "highFall")
			{
			}

			property float BodyStiffness
			{
				void set(float value)
				{
					SetArgument("bodyStiffness", value);
				}
			}
			property float Bodydamping
			{
				void set(float value)
				{
					SetArgument("bodydamping", value);
				}
			}
			property float Catchfalltime
			{
				void set(float value)
				{
					SetArgument("catchfalltime", value);
				}
			}
			property float CrashOrLandCutOff
			{
				void set(float value)
				{
					SetArgument("crashOrLandCutOff", value);
				}
			}
			property float PdStrength
			{
				void set(float value)
				{
					SetArgument("pdStrength", value);
				}
			}
			property float PdDamping
			{
				void set(float value)
				{
					SetArgument("pdDamping", value);
				}
			}
			property float ArmAngSpeed
			{
				void set(float value)
				{
					SetArgument("armAngSpeed", value);
				}
			}
			property float ArmAmplitude
			{
				void set(float value)
				{
					SetArgument("armAmplitude", value);
				}
			}
			property float ArmPhase
			{
				void set(float value)
				{
					SetArgument("armPhase", value);
				}
			}
			property bool ArmBendElbows
			{
				void set(bool value)
				{
					SetArgument("armBendElbows", value);
				}
			}
			property float LegRadius
			{
				void set(float value)
				{
					SetArgument("legRadius", value);
				}
			}
			property float LegAngSpeed
			{
				void set(float value)
				{
					SetArgument("legAngSpeed", value);
				}
			}
			property float LegAsymmetry
			{
				void set(float value)
				{
					SetArgument("legAsymmetry", value);
				}
			}
			property float Arms2LegsPhase
			{
				void set(float value)
				{
					SetArgument("arms2LegsPhase", value);
				}
			}
			property int Arms2LegsSync
			{
				void set(int value)
				{
					SetArgument("arms2LegsSync", value);
				}
			}
			property float ArmsUp
			{
				void set(float value)
				{
					SetArgument("armsUp", value);
				}
			}
			property bool OrientateBodyToFallDirection
			{
				void set(bool value)
				{
					SetArgument("orientateBodyToFallDirection", value);
				}
			}
			property bool OrientateTwist
			{
				void set(bool value)
				{
					SetArgument("orientateTwist", value);
				}
			}
			property float OrientateMax
			{
				void set(float value)
				{
					SetArgument("orientateMax", value);
				}
			}
			property bool AlanRickman
			{
				void set(bool value)
				{
					SetArgument("alanRickman", value);
				}
			}
			property bool FowardRoll
			{
				void set(bool value)
				{
					SetArgument("fowardRoll", value);
				}
			}
			property bool UseZeroPose_withFowardRoll
			{
				void set(bool value)
				{
					SetArgument("useZeroPose_withFowardRoll", value);
				}
			}
			property float AimAngleBase
			{
				void set(float value)
				{
					SetArgument("aimAngleBase", value);
				}
			}
			property float FowardVelRotation
			{
				void set(float value)
				{
					SetArgument("fowardVelRotation", value);
				}
			}
			property float FootVelCompScale
			{
				void set(float value)
				{
					SetArgument("footVelCompScale", value);
				}
			}
			property float SideD
			{
				void set(float value)
				{
					SetArgument("sideD", value);
				}
			}
			property float FowardOffsetOfLegIK
			{
				void set(float value)
				{
					SetArgument("fowardOffsetOfLegIK", value);
				}
			}
			property float LegL
			{
				void set(float value)
				{
					SetArgument("legL", value);
				}
			}
			property float CatchFallCutOff
			{
				void set(float value)
				{
					SetArgument("catchFallCutOff", value);
				}
			}
			property float LegStrength
			{
				void set(float value)
				{
					SetArgument("legStrength", value);
				}
			}
			property bool Balance
			{
				void set(bool value)
				{
					SetArgument("balance", value);
				}
			}
			property bool IgnorWorldCollisions
			{
				void set(bool value)
				{
					SetArgument("ignorWorldCollisions", value);
				}
			}
			property bool AdaptiveCircling
			{
				void set(bool value)
				{
					SetArgument("adaptiveCircling", value);
				}
			}
			property bool Hula
			{
				void set(bool value)
				{
					SetArgument("hula", value);
				}
			}
			property float MaxSpeedForRecoverableFall
			{
				void set(float value)
				{
					SetArgument("maxSpeedForRecoverableFall", value);
				}
			}
			property float MinSpeedForBrace
			{
				void set(float value)
				{
					SetArgument("minSpeedForBrace", value);
				}
			}
			property float LandingNormal
			{
				void set(float value)
				{
					SetArgument("landingNormal", value);
				}
			}
		};
		public ref class IncomingTransformsHelper sealed : public CustomHelper
		{
		public:
			IncomingTransformsHelper(Ped ^ped) : CustomHelper(ped, "incomingTransforms")
			{
			}
		};
		public ref class InjuredOnGroundHelper sealed : public CustomHelper
		{
		public:
			InjuredOnGroundHelper(Ped ^ped) : CustomHelper(ped, "injuredOnGround")
			{
			}

			property int NumInjuries
			{
				void set(int value)
				{
					SetArgument("numInjuries", value);
				}
			}
			property int Injury1Component
			{
				void set(int value)
				{
					SetArgument("injury1Component", value);
				}
			}
			property int Injury2Component
			{
				void set(int value)
				{
					SetArgument("injury2Component", value);
				}
			}
			property Math::Vector3 Injury1LocalPosition
			{
				void set(Math::Vector3 value)
				{
					SetArgument("injury1LocalPosition", value);
				}
			}
			property Math::Vector3 Injury2LocalPosition
			{
				void set(Math::Vector3 value)
				{
					SetArgument("injury2LocalPosition", value);
				}
			}
			property Math::Vector3 Injury1LocalNormal
			{
				void set(Math::Vector3 value)
				{
					SetArgument("injury1LocalNormal", value);
				}
			}
			property Math::Vector3 Injury2LocalNormal
			{
				void set(Math::Vector3 value)
				{
					SetArgument("injury2LocalNormal", value);
				}
			}
			property Math::Vector3 AttackerPos
			{
				void set(Math::Vector3 value)
				{
					SetArgument("attackerPos", value);
				}
			}
			property bool DontReachWithLeft
			{
				void set(bool value)
				{
					SetArgument("dontReachWithLeft", value);
				}
			}
			property bool DontReachWithRight
			{
				void set(bool value)
				{
					SetArgument("dontReachWithRight", value);
				}
			}
			property bool StrongRollForce
			{
				void set(bool value)
				{
					SetArgument("strongRollForce", value);
				}
			}
		};
		public ref class CarriedHelper sealed : public CustomHelper
		{
		public:
			CarriedHelper(Ped ^ped) : CustomHelper(ped, "carried")
			{
			}
		};
		public ref class DangleHelper sealed : public CustomHelper
		{
		public:
			DangleHelper(Ped ^ped) : CustomHelper(ped, "dangle")
			{
			}

			property bool DoGrab
			{
				void set(bool value)
				{
					SetArgument("doGrab", value);
				}
			}
			property float GrabFrequency
			{
				void set(float value)
				{
					SetArgument("grabFrequency", value);
				}
			}
		};
		public ref class OnFireHelper sealed : public CustomHelper
		{
		public:
			OnFireHelper(Ped ^ped) : CustomHelper(ped, "onFire")
			{
			}

			property float StaggerTime
			{
				void set(float value)
				{
					SetArgument("staggerTime", value);
				}
			}
			property float StaggerLeanRate
			{
				void set(float value)
				{
					SetArgument("staggerLeanRate", value);
				}
			}
			property float StumbleMaxLeanBack
			{
				void set(float value)
				{
					SetArgument("stumbleMaxLeanBack", value);
				}
			}
			property float StumbleMaxLeanForward
			{
				void set(float value)
				{
					SetArgument("stumbleMaxLeanForward", value);
				}
			}
			property float ArmsWindmillWritheBlend
			{
				void set(float value)
				{
					SetArgument("armsWindmillWritheBlend", value);
				}
			}
			property float SpineStumbleWritheBlend
			{
				void set(float value)
				{
					SetArgument("spineStumbleWritheBlend", value);
				}
			}
			property float LegsStumbleWritheBlend
			{
				void set(float value)
				{
					SetArgument("legsStumbleWritheBlend", value);
				}
			}
			property float ArmsPoseWritheBlend
			{
				void set(float value)
				{
					SetArgument("armsPoseWritheBlend", value);
				}
			}
			property float SpinePoseWritheBlend
			{
				void set(float value)
				{
					SetArgument("spinePoseWritheBlend", value);
				}
			}
			property float LegsPoseWritheBlend
			{
				void set(float value)
				{
					SetArgument("legsPoseWritheBlend", value);
				}
			}
			property bool RollOverFlag
			{
				void set(bool value)
				{
					SetArgument("rollOverFlag", value);
				}
			}
			property float RollTorqueScale
			{
				void set(float value)
				{
					SetArgument("rollTorqueScale", value);
				}
			}
			property float PredictTime
			{
				void set(float value)
				{
					SetArgument("predictTime", value);
				}
			}
			property float MaxRollOverTime
			{
				void set(float value)
				{
					SetArgument("maxRollOverTime", value);
				}
			}
			property float RollOverRadius
			{
				void set(float value)
				{
					SetArgument("rollOverRadius", value);
				}
			}
		};
		public ref class PedalLegsHelper sealed : public CustomHelper
		{
		public:
			PedalLegsHelper(Ped ^ped) : CustomHelper(ped, "pedalLegs")
			{
			}

			property bool PedalLeftLeg
			{
				void set(bool value)
				{
					SetArgument("pedalLeftLeg", value);
				}
			}
			property bool PedalRightLeg
			{
				void set(bool value)
				{
					SetArgument("pedalRightLeg", value);
				}
			}
			property bool BackPedal
			{
				void set(bool value)
				{
					SetArgument("backPedal", value);
				}
			}
			property float Radius
			{
				void set(float value)
				{
					SetArgument("radius", value);
				}
			}
			property float AngularSpeed
			{
				void set(float value)
				{
					SetArgument("angularSpeed", value);
				}
			}
			property float LegStiffness
			{
				void set(float value)
				{
					SetArgument("legStiffness", value);
				}
			}
			property float PedalOffset
			{
				void set(float value)
				{
					SetArgument("pedalOffset", value);
				}
			}
			property int RandomSeed
			{
				void set(int value)
				{
					SetArgument("randomSeed", value);
				}
			}
			property float SpeedAsymmetry
			{
				void set(float value)
				{
					SetArgument("speedAsymmetry", value);
				}
			}
			property bool AdaptivePedal4Dragging
			{
				void set(bool value)
				{
					SetArgument("adaptivePedal4Dragging", value);
				}
			}
			property float AngSpeedMultiplier4Dragging
			{
				void set(float value)
				{
					SetArgument("angSpeedMultiplier4Dragging", value);
				}
			}
			property float RadiusVariance
			{
				void set(float value)
				{
					SetArgument("radiusVariance", value);
				}
			}
			property float LegAngleVariance
			{
				void set(float value)
				{
					SetArgument("legAngleVariance", value);
				}
			}
			property float CentreSideways
			{
				void set(float value)
				{
					SetArgument("centreSideways", value);
				}
			}
			property float CentreForwards
			{
				void set(float value)
				{
					SetArgument("centreForwards", value);
				}
			}
			property float CentreUp
			{
				void set(float value)
				{
					SetArgument("centreUp", value);
				}
			}
			property float Ellipse
			{
				void set(float value)
				{
					SetArgument("ellipse", value);
				}
			}
			property float DragReduction
			{
				void set(float value)
				{
					SetArgument("dragReduction", value);
				}
			}
			property float Spread
			{
				void set(float value)
				{
					SetArgument("spread", value);
				}
			}
			property bool Hula
			{
				void set(bool value)
				{
					SetArgument("hula", value);
				}
			}
		};
		public ref class PointArmHelper sealed : public CustomHelper
		{
		public:
			PointArmHelper(Ped ^ped) : CustomHelper(ped, "pointArm")
			{
			}

			property Math::Vector3 TargetLeft
			{
				void set(Math::Vector3 value)
				{
					SetArgument("targetLeft", value);
				}
			}
			property float TwistLeft
			{
				void set(float value)
				{
					SetArgument("twistLeft", value);
				}
			}
			property float ArmStraightnessLeft
			{
				void set(float value)
				{
					SetArgument("armStraightnessLeft", value);
				}
			}
			property bool UseLeftArm
			{
				void set(bool value)
				{
					SetArgument("useLeftArm", value);
				}
			}
			property float ArmStiffnessLeft
			{
				void set(float value)
				{
					SetArgument("armStiffnessLeft", value);
				}
			}
			property float ArmDampingLeft
			{
				void set(float value)
				{
					SetArgument("armDampingLeft", value);
				}
			}
			property int InstanceIndexLeft
			{
				void set(int value)
				{
					SetArgument("instanceIndexLeft", value);
				}
			}
			property float PointSwingLimitLeft
			{
				void set(float value)
				{
					SetArgument("pointSwingLimitLeft", value);
				}
			}
			property bool UseZeroPoseWhenNotPointingLeft
			{
				void set(bool value)
				{
					SetArgument("useZeroPoseWhenNotPointingLeft", value);
				}
			}
			property Math::Vector3 TargetRight
			{
				void set(Math::Vector3 value)
				{
					SetArgument("targetRight", value);
				}
			}
			property float TwistRight
			{
				void set(float value)
				{
					SetArgument("twistRight", value);
				}
			}
			property float ArmStraightnessRight
			{
				void set(float value)
				{
					SetArgument("armStraightnessRight", value);
				}
			}
			property bool UseRightArm
			{
				void set(bool value)
				{
					SetArgument("useRightArm", value);
				}
			}
			property float ArmStiffnessRight
			{
				void set(float value)
				{
					SetArgument("armStiffnessRight", value);
				}
			}
			property float ArmDampingRight
			{
				void set(float value)
				{
					SetArgument("armDampingRight", value);
				}
			}
			property int InstanceIndexRight
			{
				void set(int value)
				{
					SetArgument("instanceIndexRight", value);
				}
			}
			property float PointSwingLimitRight
			{
				void set(float value)
				{
					SetArgument("pointSwingLimitRight", value);
				}
			}
			property bool UseZeroPoseWhenNotPointingRight
			{
				void set(bool value)
				{
					SetArgument("useZeroPoseWhenNotPointingRight", value);
				}
			}
		};
		public ref class PointGunHelper sealed : public CustomHelper
		{
		public:
			PointGunHelper(Ped ^ped) : CustomHelper(ped, "pointGun")
			{
			}

			property bool EnableRight
			{
				void set(bool value)
				{
					SetArgument("enableRight", value);
				}
			}
			property bool EnableLeft
			{
				void set(bool value)
				{
					SetArgument("enableLeft", value);
				}
			}
			property Math::Vector3 LeftHandTarget
			{
				void set(Math::Vector3 value)
				{
					SetArgument("leftHandTarget", value);
				}
			}
			property int LeftHandTargetIndex
			{
				void set(int value)
				{
					SetArgument("leftHandTargetIndex", value);
				}
			}
			property Math::Vector3 RightHandTarget
			{
				void set(Math::Vector3 value)
				{
					SetArgument("rightHandTarget", value);
				}
			}
			property int RightHandTargetIndex
			{
				void set(int value)
				{
					SetArgument("rightHandTargetIndex", value);
				}
			}
			property float LeadTarget
			{
				void set(float value)
				{
					SetArgument("leadTarget", value);
				}
			}
			property float ArmStiffness
			{
				void set(float value)
				{
					SetArgument("armStiffness", value);
				}
			}
			property float ArmStiffnessDetSupport
			{
				void set(float value)
				{
					SetArgument("armStiffnessDetSupport", value);
				}
			}
			property float ArmDamping
			{
				void set(float value)
				{
					SetArgument("armDamping", value);
				}
			}
			property float GravityOpposition
			{
				void set(float value)
				{
					SetArgument("gravityOpposition", value);
				}
			}
			property float GravOppDetachedSupport
			{
				void set(float value)
				{
					SetArgument("gravOppDetachedSupport", value);
				}
			}
			property float MassMultDetachedSupport
			{
				void set(float value)
				{
					SetArgument("massMultDetachedSupport", value);
				}
			}
			property bool AllowShotLooseness
			{
				void set(bool value)
				{
					SetArgument("allowShotLooseness", value);
				}
			}
			property float ClavicleBlend
			{
				void set(float value)
				{
					SetArgument("clavicleBlend", value);
				}
			}
			property float ElbowAttitude
			{
				void set(float value)
				{
					SetArgument("elbowAttitude", value);
				}
			}
			property int SupportConstraint
			{
				void set(int value)
				{
					SetArgument("supportConstraint", value);
				}
			}
			property float ConstraintMinDistance
			{
				void set(float value)
				{
					SetArgument("constraintMinDistance", value);
				}
			}
			property float MakeConstraintDistance
			{
				void set(float value)
				{
					SetArgument("makeConstraintDistance", value);
				}
			}
			property float ReduceConstraintLengthVel
			{
				void set(float value)
				{
					SetArgument("reduceConstraintLengthVel", value);
				}
			}
			property float BreakingStrength
			{
				void set(float value)
				{
					SetArgument("breakingStrength", value);
				}
			}
			property float BrokenSupportTime
			{
				void set(float value)
				{
					SetArgument("brokenSupportTime", value);
				}
			}
			property float BrokenToSideProb
			{
				void set(float value)
				{
					SetArgument("brokenToSideProb", value);
				}
			}
			property float ConnectAfter
			{
				void set(float value)
				{
					SetArgument("connectAfter", value);
				}
			}
			property float ConnectFor
			{
				void set(float value)
				{
					SetArgument("connectFor", value);
				}
			}
			property int OneHandedPointing
			{
				void set(int value)
				{
					SetArgument("oneHandedPointing", value);
				}
			}
			property bool AlwaysSupport
			{
				void set(bool value)
				{
					SetArgument("alwaysSupport", value);
				}
			}
			property bool PoseUnusedGunArm
			{
				void set(bool value)
				{
					SetArgument("poseUnusedGunArm", value);
				}
			}
			property bool PoseUnusedSupportArm
			{
				void set(bool value)
				{
					SetArgument("poseUnusedSupportArm", value);
				}
			}
			property bool PoseUnusedOtherArm
			{
				void set(bool value)
				{
					SetArgument("poseUnusedOtherArm", value);
				}
			}
			property float MaxAngleAcross
			{
				void set(float value)
				{
					SetArgument("maxAngleAcross", value);
				}
			}
			property float MaxAngleAway
			{
				void set(float value)
				{
					SetArgument("maxAngleAway", value);
				}
			}
			property int FallingLimits
			{
				void set(int value)
				{
					SetArgument("fallingLimits", value);
				}
			}
			property float AcrossLimit
			{
				void set(float value)
				{
					SetArgument("acrossLimit", value);
				}
			}
			property float AwayLimit
			{
				void set(float value)
				{
					SetArgument("awayLimit", value);
				}
			}
			property float UpLimit
			{
				void set(float value)
				{
					SetArgument("upLimit", value);
				}
			}
			property float DownLimit
			{
				void set(float value)
				{
					SetArgument("downLimit", value);
				}
			}
			property int RifleFall
			{
				void set(int value)
				{
					SetArgument("rifleFall", value);
				}
			}
			property int FallingSupport
			{
				void set(int value)
				{
					SetArgument("fallingSupport", value);
				}
			}
			property int FallingTypeSupport
			{
				void set(int value)
				{
					SetArgument("fallingTypeSupport", value);
				}
			}
			property int PistolNeutralType
			{
				void set(int value)
				{
					SetArgument("pistolNeutralType", value);
				}
			}
			property bool NeutralPoint4Pistols
			{
				void set(bool value)
				{
					SetArgument("neutralPoint4Pistols", value);
				}
			}
			property bool NeutralPoint4Rifle
			{
				void set(bool value)
				{
					SetArgument("neutralPoint4Rifle", value);
				}
			}
			property bool CheckNeutralPoint
			{
				void set(bool value)
				{
					SetArgument("checkNeutralPoint", value);
				}
			}
			property Math::Vector3 Point2Side
			{
				void set(Math::Vector3 value)
				{
					SetArgument("point2Side", value);
				}
			}
			property float Add2WeaponDistSide
			{
				void set(float value)
				{
					SetArgument("add2WeaponDistSide", value);
				}
			}
			property Math::Vector3 Point2Connect
			{
				void set(Math::Vector3 value)
				{
					SetArgument("point2Connect", value);
				}
			}
			property float Add2WeaponDistConnect
			{
				void set(float value)
				{
					SetArgument("add2WeaponDistConnect", value);
				}
			}
			property bool UsePistolIK
			{
				void set(bool value)
				{
					SetArgument("usePistolIK", value);
				}
			}
			property bool UseSpineTwist
			{
				void set(bool value)
				{
					SetArgument("useSpineTwist", value);
				}
			}
			property bool UseTurnToTarget
			{
				void set(bool value)
				{
					SetArgument("useTurnToTarget", value);
				}
			}
			property bool UseHeadLook
			{
				void set(bool value)
				{
					SetArgument("useHeadLook", value);
				}
			}
			property float ErrorThreshold
			{
				void set(float value)
				{
					SetArgument("errorThreshold", value);
				}
			}
			property float FireWeaponRelaxTime
			{
				void set(float value)
				{
					SetArgument("fireWeaponRelaxTime", value);
				}
			}
			property float FireWeaponRelaxAmount
			{
				void set(float value)
				{
					SetArgument("fireWeaponRelaxAmount", value);
				}
			}
			property float FireWeaponRelaxDistance
			{
				void set(float value)
				{
					SetArgument("fireWeaponRelaxDistance", value);
				}
			}
			property bool UseIncomingTransforms
			{
				void set(bool value)
				{
					SetArgument("useIncomingTransforms", value);
				}
			}
			property bool MeasureParentOffset
			{
				void set(bool value)
				{
					SetArgument("measureParentOffset", value);
				}
			}
			property Math::Vector3 LeftHandParentOffset
			{
				void set(Math::Vector3 value)
				{
					SetArgument("leftHandParentOffset", value);
				}
			}
			property int LeftHandParentEffector
			{
				void set(int value)
				{
					SetArgument("leftHandParentEffector", value);
				}
			}
			property Math::Vector3 RightHandParentOffset
			{
				void set(Math::Vector3 value)
				{
					SetArgument("rightHandParentOffset", value);
				}
			}
			property int RightHandParentEffector
			{
				void set(int value)
				{
					SetArgument("rightHandParentEffector", value);
				}
			}
			property float PrimaryHandWeaponDistance
			{
				void set(float value)
				{
					SetArgument("primaryHandWeaponDistance", value);
				}
			}
			property bool ConstrainRifle
			{
				void set(bool value)
				{
					SetArgument("constrainRifle", value);
				}
			}
			property float RifleConstraintMinDistance
			{
				void set(float value)
				{
					SetArgument("rifleConstraintMinDistance", value);
				}
			}
			property bool DisableArmCollisions
			{
				void set(bool value)
				{
					SetArgument("disableArmCollisions", value);
				}
			}
			property bool DisableRifleCollisions
			{
				void set(bool value)
				{
					SetArgument("disableRifleCollisions", value);
				}
			}
		};
		public ref class PointGunExtraHelper sealed : public CustomHelper
		{
		public:
			PointGunExtraHelper(Ped ^ped) : CustomHelper(ped, "pointGunExtra")
			{
			}

			property float ConstraintStrength
			{
				void set(float value)
				{
					SetArgument("constraintStrength", value);
				}
			}
			property float ConstraintThresh
			{
				void set(float value)
				{
					SetArgument("constraintThresh", value);
				}
			}
			property int WeaponMask
			{
				void set(int value)
				{
					SetArgument("weaponMask", value);
				}
			}
			property bool TimeWarpActive
			{
				void set(bool value)
				{
					SetArgument("timeWarpActive", value);
				}
			}
			property float TimeWarpStrengthScale
			{
				void set(float value)
				{
					SetArgument("timeWarpStrengthScale", value);
				}
			}
			property float OriStiff
			{
				void set(float value)
				{
					SetArgument("oriStiff", value);
				}
			}
			property float OriDamp
			{
				void set(float value)
				{
					SetArgument("oriDamp", value);
				}
			}
			property float PosStiff
			{
				void set(float value)
				{
					SetArgument("posStiff", value);
				}
			}
			property float PosDamp
			{
				void set(float value)
				{
					SetArgument("posDamp", value);
				}
			}
		};
		public ref class RollDownStairsHelper sealed : public CustomHelper
		{
		public:
			RollDownStairsHelper(Ped ^ped) : CustomHelper(ped, "rollDownStairs")
			{
			}

			property float Stiffness
			{
				void set(float value)
				{
					SetArgument("stiffness", value);
				}
			}
			property float Damping
			{
				void set(float value)
				{
					SetArgument("damping", value);
				}
			}
			property float Forcemag
			{
				void set(float value)
				{
					SetArgument("forcemag", value);
				}
			}
			property float M_useArmToSlowDown
			{
				void set(float value)
				{
					SetArgument("m_useArmToSlowDown", value);
				}
			}
			property bool UseZeroPose
			{
				void set(bool value)
				{
					SetArgument("useZeroPose", value);
				}
			}
			property bool SpinWhenInAir
			{
				void set(bool value)
				{
					SetArgument("spinWhenInAir", value);
				}
			}
			property float M_armReachAmount
			{
				void set(float value)
				{
					SetArgument("m_armReachAmount", value);
				}
			}
			property float M_legPush
			{
				void set(float value)
				{
					SetArgument("m_legPush", value);
				}
			}
			property bool TryToAvoidHeadButtingGround
			{
				void set(bool value)
				{
					SetArgument("tryToAvoidHeadButtingGround", value);
				}
			}
			property float ArmReachLength
			{
				void set(float value)
				{
					SetArgument("armReachLength", value);
				}
			}
			property Math::Vector3 CustomRollDir
			{
				void set(Math::Vector3 value)
				{
					SetArgument("customRollDir", value);
				}
			}
			property bool UseCustomRollDir
			{
				void set(bool value)
				{
					SetArgument("useCustomRollDir", value);
				}
			}
			property float StiffnessDecayTarget
			{
				void set(float value)
				{
					SetArgument("stiffnessDecayTarget", value);
				}
			}
			property float StiffnessDecayTime
			{
				void set(float value)
				{
					SetArgument("stiffnessDecayTime", value);
				}
			}
			property float AsymmetricalLegs
			{
				void set(float value)
				{
					SetArgument("asymmetricalLegs", value);
				}
			}
			property float ZAxisSpinReduction
			{
				void set(float value)
				{
					SetArgument("zAxisSpinReduction", value);
				}
			}
			property float TargetLinearVelocityDecayTime
			{
				void set(float value)
				{
					SetArgument("targetLinearVelocityDecayTime", value);
				}
			}
			property float TargetLinearVelocity
			{
				void set(float value)
				{
					SetArgument("targetLinearVelocity", value);
				}
			}
			property bool OnlyApplyHelperForces
			{
				void set(bool value)
				{
					SetArgument("onlyApplyHelperForces", value);
				}
			}
			property bool UseVelocityOfObjectBelow
			{
				void set(bool value)
				{
					SetArgument("useVelocityOfObjectBelow", value);
				}
			}
			property bool UseRelativeVelocity
			{
				void set(bool value)
				{
					SetArgument("useRelativeVelocity", value);
				}
			}
			property bool ApplyFoetalToLegs
			{
				void set(bool value)
				{
					SetArgument("applyFoetalToLegs", value);
				}
			}
			property float MovementLegsInFoetalPosition
			{
				void set(float value)
				{
					SetArgument("movementLegsInFoetalPosition", value);
				}
			}
			property float MaxAngVelAroundFrontwardAxis
			{
				void set(float value)
				{
					SetArgument("maxAngVelAroundFrontwardAxis", value);
				}
			}
			property float MinAngVel
			{
				void set(float value)
				{
					SetArgument("minAngVel", value);
				}
			}
			property bool ApplyNewRollingCheatingTorques
			{
				void set(bool value)
				{
					SetArgument("applyNewRollingCheatingTorques", value);
				}
			}
			property float MaxAngVel
			{
				void set(float value)
				{
					SetArgument("maxAngVel", value);
				}
			}
			property float MagOfTorqueToRoll
			{
				void set(float value)
				{
					SetArgument("magOfTorqueToRoll", value);
				}
			}
			property bool ApplyHelPerTorqueToAlign
			{
				void set(bool value)
				{
					SetArgument("applyHelPerTorqueToAlign", value);
				}
			}
			property float DelayToAlignBody
			{
				void set(float value)
				{
					SetArgument("delayToAlignBody", value);
				}
			}
			property float MagOfTorqueToAlign
			{
				void set(float value)
				{
					SetArgument("magOfTorqueToAlign", value);
				}
			}
			property float AirborneReduction
			{
				void set(float value)
				{
					SetArgument("airborneReduction", value);
				}
			}
			property bool ApplyMinMaxFriction
			{
				void set(bool value)
				{
					SetArgument("applyMinMaxFriction", value);
				}
			}
			property bool LimitSpinReduction
			{
				void set(bool value)
				{
					SetArgument("limitSpinReduction", value);
				}
			}
		};
		public ref class ShotHelper sealed : public CustomHelper
		{
		public:
			ShotHelper(Ped ^ped) : CustomHelper(ped, "shot")
			{
			}

			property float BodyStiffness
			{
				void set(float value)
				{
					SetArgument("bodyStiffness", value);
				}
			}
			property float SpineDamping
			{
				void set(float value)
				{
					SetArgument("spineDamping", value);
				}
			}
			property float ArmStiffness
			{
				void set(float value)
				{
					SetArgument("armStiffness", value);
				}
			}
			property float InitialNeckStiffness
			{
				void set(float value)
				{
					SetArgument("initialNeckStiffness", value);
				}
			}
			property float InitialNeckDamping
			{
				void set(float value)
				{
					SetArgument("initialNeckDamping", value);
				}
			}
			property float NeckStiffness
			{
				void set(float value)
				{
					SetArgument("neckStiffness", value);
				}
			}
			property float NeckDamping
			{
				void set(float value)
				{
					SetArgument("neckDamping", value);
				}
			}
			property float KMultOnLoose
			{
				void set(float value)
				{
					SetArgument("kMultOnLoose", value);
				}
			}
			property float KMult4Legs
			{
				void set(float value)
				{
					SetArgument("kMult4Legs", value);
				}
			}
			property float LoosenessAmount
			{
				void set(float value)
				{
					SetArgument("loosenessAmount", value);
				}
			}
			property float Looseness4Fall
			{
				void set(float value)
				{
					SetArgument("looseness4Fall", value);
				}
			}
			property float Looseness4Stagger
			{
				void set(float value)
				{
					SetArgument("looseness4Stagger", value);
				}
			}
			property float MinArmsLooseness
			{
				void set(float value)
				{
					SetArgument("minArmsLooseness", value);
				}
			}
			property float MinLegsLooseness
			{
				void set(float value)
				{
					SetArgument("minLegsLooseness", value);
				}
			}
			property float GrabHoldTime
			{
				void set(float value)
				{
					SetArgument("grabHoldTime", value);
				}
			}
			property bool SpineBlendExagCPain
			{
				void set(bool value)
				{
					SetArgument("spineBlendExagCPain", value);
				}
			}
			property float SpineBlendZero
			{
				void set(float value)
				{
					SetArgument("spineBlendZero", value);
				}
			}
			property bool BulletProofVest
			{
				void set(bool value)
				{
					SetArgument("bulletProofVest", value);
				}
			}
			property bool AlwaysResetLooseness
			{
				void set(bool value)
				{
					SetArgument("alwaysResetLooseness", value);
				}
			}
			property bool AlwaysResetNeckLooseness
			{
				void set(bool value)
				{
					SetArgument("alwaysResetNeckLooseness", value);
				}
			}
			property float AngVelScale
			{
				void set(float value)
				{
					SetArgument("angVelScale", value);
				}
			}
			property System::String ^AngVelScaleMask
			{
				void set(System::String ^value)
				{
					SetArgument("angVelScaleMask", value);
				}
			}
			property float FlingWidth
			{
				void set(float value)
				{
					SetArgument("flingWidth", value);
				}
			}
			property float FlingTime
			{
				void set(float value)
				{
					SetArgument("flingTime", value);
				}
			}
			property float TimeBeforeReachForWound
			{
				void set(float value)
				{
					SetArgument("timeBeforeReachForWound", value);
				}
			}
			property float ExagDuration
			{
				void set(float value)
				{
					SetArgument("exagDuration", value);
				}
			}
			property float ExagMag
			{
				void set(float value)
				{
					SetArgument("exagMag", value);
				}
			}
			property float ExagTwistMag
			{
				void set(float value)
				{
					SetArgument("exagTwistMag", value);
				}
			}
			property float ExagSmooth2Zero
			{
				void set(float value)
				{
					SetArgument("exagSmooth2Zero", value);
				}
			}
			property float ExagZeroTime
			{
				void set(float value)
				{
					SetArgument("exagZeroTime", value);
				}
			}
			property float CpainSmooth2Time
			{
				void set(float value)
				{
					SetArgument("cpainSmooth2Time", value);
				}
			}
			property float CpainDuration
			{
				void set(float value)
				{
					SetArgument("cpainDuration", value);
				}
			}
			property float CpainMag
			{
				void set(float value)
				{
					SetArgument("cpainMag", value);
				}
			}
			property float CpainTwistMag
			{
				void set(float value)
				{
					SetArgument("cpainTwistMag", value);
				}
			}
			property float CpainSmooth2Zero
			{
				void set(float value)
				{
					SetArgument("cpainSmooth2Zero", value);
				}
			}
			property bool Crouching
			{
				void set(bool value)
				{
					SetArgument("crouching", value);
				}
			}
			property bool ChickenArms
			{
				void set(bool value)
				{
					SetArgument("chickenArms", value);
				}
			}
			property bool ReachForWound
			{
				void set(bool value)
				{
					SetArgument("reachForWound", value);
				}
			}
			property bool Fling
			{
				void set(bool value)
				{
					SetArgument("fling", value);
				}
			}
			property bool AllowInjuredArm
			{
				void set(bool value)
				{
					SetArgument("allowInjuredArm", value);
				}
			}
			property bool AllowInjuredLeg
			{
				void set(bool value)
				{
					SetArgument("allowInjuredLeg", value);
				}
			}
			property bool AllowInjuredLowerLegReach
			{
				void set(bool value)
				{
					SetArgument("allowInjuredLowerLegReach", value);
				}
			}
			property bool AllowInjuredThighReach
			{
				void set(bool value)
				{
					SetArgument("allowInjuredThighReach", value);
				}
			}
			property bool StableHandsAndNeck
			{
				void set(bool value)
				{
					SetArgument("stableHandsAndNeck", value);
				}
			}
			property bool Melee
			{
				void set(bool value)
				{
					SetArgument("melee", value);
				}
			}
			property int FallingReaction
			{
				void set(int value)
				{
					SetArgument("fallingReaction", value);
				}
			}
			property bool UseExtendedCatchFall
			{
				void set(bool value)
				{
					SetArgument("useExtendedCatchFall", value);
				}
			}
			property float InitialWeaknessZeroDuration
			{
				void set(float value)
				{
					SetArgument("initialWeaknessZeroDuration", value);
				}
			}
			property float InitialWeaknessRampDuration
			{
				void set(float value)
				{
					SetArgument("initialWeaknessRampDuration", value);
				}
			}
			property float InitialNeckDuration
			{
				void set(float value)
				{
					SetArgument("initialNeckDuration", value);
				}
			}
			property float InitialNeckRampDuration
			{
				void set(float value)
				{
					SetArgument("initialNeckRampDuration", value);
				}
			}
			property bool UseCStrModulation
			{
				void set(bool value)
				{
					SetArgument("useCStrModulation", value);
				}
			}
			property float CStrUpperMin
			{
				void set(float value)
				{
					SetArgument("cStrUpperMin", value);
				}
			}
			property float CStrUpperMax
			{
				void set(float value)
				{
					SetArgument("cStrUpperMax", value);
				}
			}
			property float CStrLowerMin
			{
				void set(float value)
				{
					SetArgument("cStrLowerMin", value);
				}
			}
			property float CStrLowerMax
			{
				void set(float value)
				{
					SetArgument("cStrLowerMax", value);
				}
			}
			property float DeathTime
			{
				void set(float value)
				{
					SetArgument("deathTime", value);
				}
			}
		};
		public ref class ShotNewBulletHelper sealed : public CustomHelper
		{
		public:
			ShotNewBulletHelper(Ped ^ped) : CustomHelper(ped, "shotNewBullet")
			{
			}

			property int BodyPart
			{
				void set(int value)
				{
					SetArgument("bodyPart", value);
				}
			}
			property bool LocalHitPointInfo
			{
				void set(bool value)
				{
					SetArgument("localHitPointInfo", value);
				}
			}
			property Math::Vector3 Normal
			{
				void set(Math::Vector3 value)
				{
					SetArgument("normal", value);
				}
			}
			property Math::Vector3 HitPoint
			{
				void set(Math::Vector3 value)
				{
					SetArgument("hitPoint", value);
				}
			}
			property Math::Vector3 BulletVel
			{
				void set(Math::Vector3 value)
				{
					SetArgument("bulletVel", value);
				}
			}
		};
		public ref class ShotSnapHelper sealed : public CustomHelper
		{
		public:
			ShotSnapHelper(Ped ^ped) : CustomHelper(ped, "shotSnap")
			{
			}

			property bool Snap
			{
				void set(bool value)
				{
					SetArgument("snap", value);
				}
			}
			property float SnapMag
			{
				void set(float value)
				{
					SetArgument("snapMag", value);
				}
			}
			property float SnapMovingMult
			{
				void set(float value)
				{
					SetArgument("snapMovingMult", value);
				}
			}
			property float SnapBalancingMult
			{
				void set(float value)
				{
					SetArgument("snapBalancingMult", value);
				}
			}
			property float SnapAirborneMult
			{
				void set(float value)
				{
					SetArgument("snapAirborneMult", value);
				}
			}
			property float SnapMovingThresh
			{
				void set(float value)
				{
					SetArgument("snapMovingThresh", value);
				}
			}
			property float SnapDirectionRandomness
			{
				void set(float value)
				{
					SetArgument("snapDirectionRandomness", value);
				}
			}
			property bool SnapLeftArm
			{
				void set(bool value)
				{
					SetArgument("snapLeftArm", value);
				}
			}
			property bool SnapRightArm
			{
				void set(bool value)
				{
					SetArgument("snapRightArm", value);
				}
			}
			property bool SnapLeftLeg
			{
				void set(bool value)
				{
					SetArgument("snapLeftLeg", value);
				}
			}
			property bool SnapRightLeg
			{
				void set(bool value)
				{
					SetArgument("snapRightLeg", value);
				}
			}
			property bool SnapSpine
			{
				void set(bool value)
				{
					SetArgument("snapSpine", value);
				}
			}
			property bool SnapNeck
			{
				void set(bool value)
				{
					SetArgument("snapNeck", value);
				}
			}
			property bool SnapPhasedLegs
			{
				void set(bool value)
				{
					SetArgument("snapPhasedLegs", value);
				}
			}
			property int SnapHipType
			{
				void set(int value)
				{
					SetArgument("snapHipType", value);
				}
			}
			property bool SnapUseBulletDir
			{
				void set(bool value)
				{
					SetArgument("snapUseBulletDir", value);
				}
			}
			property bool SnapHitPart
			{
				void set(bool value)
				{
					SetArgument("snapHitPart", value);
				}
			}
			property float UnSnapInterval
			{
				void set(float value)
				{
					SetArgument("unSnapInterval", value);
				}
			}
			property float UnSnapRatio
			{
				void set(float value)
				{
					SetArgument("unSnapRatio", value);
				}
			}
			property bool SnapUseTorques
			{
				void set(bool value)
				{
					SetArgument("snapUseTorques", value);
				}
			}
		};
		public ref class ShotShockSpinHelper sealed : public CustomHelper
		{
		public:
			ShotShockSpinHelper(Ped ^ped) : CustomHelper(ped, "shotShockSpin")
			{
			}

			property bool AddShockSpin
			{
				void set(bool value)
				{
					SetArgument("addShockSpin", value);
				}
			}
			property bool RandomizeShockSpinDirection
			{
				void set(bool value)
				{
					SetArgument("randomizeShockSpinDirection", value);
				}
			}
			property bool AlwaysAddShockSpin
			{
				void set(bool value)
				{
					SetArgument("alwaysAddShockSpin", value);
				}
			}
			property float ShockSpinMin
			{
				void set(float value)
				{
					SetArgument("shockSpinMin", value);
				}
			}
			property float ShockSpinMax
			{
				void set(float value)
				{
					SetArgument("shockSpinMax", value);
				}
			}
			property float ShockSpinLiftForceMult
			{
				void set(float value)
				{
					SetArgument("shockSpinLiftForceMult", value);
				}
			}
			property float ShockSpinDecayMult
			{
				void set(float value)
				{
					SetArgument("shockSpinDecayMult", value);
				}
			}
			property float ShockSpinScalePerComponent
			{
				void set(float value)
				{
					SetArgument("shockSpinScalePerComponent", value);
				}
			}
			property float ShockSpinMaxTwistVel
			{
				void set(float value)
				{
					SetArgument("shockSpinMaxTwistVel", value);
				}
			}
			property bool ShockSpinScaleByLeverArm
			{
				void set(bool value)
				{
					SetArgument("shockSpinScaleByLeverArm", value);
				}
			}
			property float ShockSpinAirMult
			{
				void set(float value)
				{
					SetArgument("shockSpinAirMult", value);
				}
			}
			property float ShockSpin1FootMult
			{
				void set(float value)
				{
					SetArgument("shockSpin1FootMult", value);
				}
			}
			property float ShockSpinFootGripMult
			{
				void set(float value)
				{
					SetArgument("shockSpinFootGripMult", value);
				}
			}
			property float BracedSideSpinMult
			{
				void set(float value)
				{
					SetArgument("bracedSideSpinMult", value);
				}
			}
		};
		public ref class ShotFallToKneesHelper sealed : public CustomHelper
		{
		public:
			ShotFallToKneesHelper(Ped ^ped) : CustomHelper(ped, "shotFallToKnees")
			{
			}

			property bool FallToKnees
			{
				void set(bool value)
				{
					SetArgument("fallToKnees", value);
				}
			}
			property bool FtkAlwaysChangeFall
			{
				void set(bool value)
				{
					SetArgument("ftkAlwaysChangeFall", value);
				}
			}
			property float FtkBalanceTime
			{
				void set(float value)
				{
					SetArgument("ftkBalanceTime", value);
				}
			}
			property float FtkHelperForce
			{
				void set(float value)
				{
					SetArgument("ftkHelperForce", value);
				}
			}
			property bool FtkHelperForceOnSpine
			{
				void set(bool value)
				{
					SetArgument("ftkHelperForceOnSpine", value);
				}
			}
			property float FtkLeanHelp
			{
				void set(float value)
				{
					SetArgument("ftkLeanHelp", value);
				}
			}
			property float FtkSpineBend
			{
				void set(float value)
				{
					SetArgument("ftkSpineBend", value);
				}
			}
			property bool FtkStiffSpine
			{
				void set(bool value)
				{
					SetArgument("ftkStiffSpine", value);
				}
			}
			property float FtkImpactLooseness
			{
				void set(float value)
				{
					SetArgument("ftkImpactLooseness", value);
				}
			}
			property float FtkImpactLoosenessTime
			{
				void set(float value)
				{
					SetArgument("ftkImpactLoosenessTime", value);
				}
			}
			property float FtkBendRate
			{
				void set(float value)
				{
					SetArgument("ftkBendRate", value);
				}
			}
			property float FtkHipBlend
			{
				void set(float value)
				{
					SetArgument("ftkHipBlend", value);
				}
			}
			property float FtkLungeProb
			{
				void set(float value)
				{
					SetArgument("ftkLungeProb", value);
				}
			}
			property bool FtkKneeSpin
			{
				void set(bool value)
				{
					SetArgument("ftkKneeSpin", value);
				}
			}
			property float FtkFricMult
			{
				void set(float value)
				{
					SetArgument("ftkFricMult", value);
				}
			}
			property float FtkHipAngleFall
			{
				void set(float value)
				{
					SetArgument("ftkHipAngleFall", value);
				}
			}
			property float FtkPitchForwards
			{
				void set(float value)
				{
					SetArgument("ftkPitchForwards", value);
				}
			}
			property float FtkPitchBackwards
			{
				void set(float value)
				{
					SetArgument("ftkPitchBackwards", value);
				}
			}
			property float FtkFallBelowStab
			{
				void set(float value)
				{
					SetArgument("ftkFallBelowStab", value);
				}
			}
			property float FtkBalanceAbortThreshold
			{
				void set(float value)
				{
					SetArgument("ftkBalanceAbortThreshold", value);
				}
			}
			property int FtkOnKneesArmType
			{
				void set(int value)
				{
					SetArgument("ftkOnKneesArmType", value);
				}
			}
			property float FtkReleaseReachForWound
			{
				void set(float value)
				{
					SetArgument("ftkReleaseReachForWound", value);
				}
			}
			property bool FtkReachForWound
			{
				void set(bool value)
				{
					SetArgument("ftkReachForWound", value);
				}
			}
			property bool FtkReleasePointGun
			{
				void set(bool value)
				{
					SetArgument("ftkReleasePointGun", value);
				}
			}
			property bool FtkFailMustCollide
			{
				void set(bool value)
				{
					SetArgument("ftkFailMustCollide", value);
				}
			}
		};
		public ref class ShotFromBehindHelper sealed : public CustomHelper
		{
		public:
			ShotFromBehindHelper(Ped ^ped) : CustomHelper(ped, "shotFromBehind")
			{
			}

			property bool ShotFromBehind
			{
				void set(bool value)
				{
					SetArgument("shotFromBehind", value);
				}
			}
			property float SfbSpineAmount
			{
				void set(float value)
				{
					SetArgument("sfbSpineAmount", value);
				}
			}
			property float SfbNeckAmount
			{
				void set(float value)
				{
					SetArgument("sfbNeckAmount", value);
				}
			}
			property float SfbHipAmount
			{
				void set(float value)
				{
					SetArgument("sfbHipAmount", value);
				}
			}
			property float SfbKneeAmount
			{
				void set(float value)
				{
					SetArgument("sfbKneeAmount", value);
				}
			}
			property float SfbPeriod
			{
				void set(float value)
				{
					SetArgument("sfbPeriod", value);
				}
			}
			property float SfbForceBalancePeriod
			{
				void set(float value)
				{
					SetArgument("sfbForceBalancePeriod", value);
				}
			}
			property float SfbArmsOnset
			{
				void set(float value)
				{
					SetArgument("sfbArmsOnset", value);
				}
			}
			property float SfbKneesOnset
			{
				void set(float value)
				{
					SetArgument("sfbKneesOnset", value);
				}
			}
			property float SfbNoiseGain
			{
				void set(float value)
				{
					SetArgument("sfbNoiseGain", value);
				}
			}
			property int SfbIgnoreFail
			{
				void set(int value)
				{
					SetArgument("sfbIgnoreFail", value);
				}
			}
		};
		public ref class ShotInGutsHelper sealed : public CustomHelper
		{
		public:
			ShotInGutsHelper(Ped ^ped) : CustomHelper(ped, "shotInGuts")
			{
			}

			property bool ShotInGuts
			{
				void set(bool value)
				{
					SetArgument("shotInGuts", value);
				}
			}
			property float SigSpineAmount
			{
				void set(float value)
				{
					SetArgument("sigSpineAmount", value);
				}
			}
			property float SigNeckAmount
			{
				void set(float value)
				{
					SetArgument("sigNeckAmount", value);
				}
			}
			property float SigHipAmount
			{
				void set(float value)
				{
					SetArgument("sigHipAmount", value);
				}
			}
			property float SigKneeAmount
			{
				void set(float value)
				{
					SetArgument("sigKneeAmount", value);
				}
			}
			property float SigPeriod
			{
				void set(float value)
				{
					SetArgument("sigPeriod", value);
				}
			}
			property float SigForceBalancePeriod
			{
				void set(float value)
				{
					SetArgument("sigForceBalancePeriod", value);
				}
			}
			property float SigKneesOnset
			{
				void set(float value)
				{
					SetArgument("sigKneesOnset", value);
				}
			}
		};
		public ref class ShotHeadLookHelper sealed : public CustomHelper
		{
		public:
			ShotHeadLookHelper(Ped ^ped) : CustomHelper(ped, "shotHeadLook")
			{
			}

			property bool UseHeadLook
			{
				void set(bool value)
				{
					SetArgument("useHeadLook", value);
				}
			}
			property Math::Vector3 HeadLook
			{
				void set(Math::Vector3 value)
				{
					SetArgument("headLook", value);
				}
			}
			property float HeadLookAtWoundMinTimer
			{
				void set(float value)
				{
					SetArgument("headLookAtWoundMinTimer", value);
				}
			}
			property float HeadLookAtWoundMaxTimer
			{
				void set(float value)
				{
					SetArgument("headLookAtWoundMaxTimer", value);
				}
			}
			property float HeadLookAtHeadPosMaxTimer
			{
				void set(float value)
				{
					SetArgument("headLookAtHeadPosMaxTimer", value);
				}
			}
			property float HeadLookAtHeadPosMinTimer
			{
				void set(float value)
				{
					SetArgument("headLookAtHeadPosMinTimer", value);
				}
			}
		};
		public ref class ShotConfigureArmsHelper sealed : public CustomHelper
		{
		public:
			ShotConfigureArmsHelper(Ped ^ped) : CustomHelper(ped, "shotConfigureArms")
			{
			}

			property bool Brace
			{
				void set(bool value)
				{
					SetArgument("brace", value);
				}
			}
			property bool PointGun
			{
				void set(bool value)
				{
					SetArgument("pointGun", value);
				}
			}
			property bool UseArmsWindmill
			{
				void set(bool value)
				{
					SetArgument("useArmsWindmill", value);
				}
			}
			property int ReleaseWound
			{
				void set(int value)
				{
					SetArgument("releaseWound", value);
				}
			}
			property int ReachFalling
			{
				void set(int value)
				{
					SetArgument("reachFalling", value);
				}
			}
			property int ReachFallingWithOneHand
			{
				void set(int value)
				{
					SetArgument("reachFallingWithOneHand", value);
				}
			}
			property int ReachOnFloor
			{
				void set(int value)
				{
					SetArgument("reachOnFloor", value);
				}
			}
			property float AlwaysReachTime
			{
				void set(float value)
				{
					SetArgument("alwaysReachTime", value);
				}
			}
			property float AWSpeedMult
			{
				void set(float value)
				{
					SetArgument("AWSpeedMult", value);
				}
			}
			property float AWRadiusMult
			{
				void set(float value)
				{
					SetArgument("AWRadiusMult", value);
				}
			}
			property float AWStiffnessAdd
			{
				void set(float value)
				{
					SetArgument("AWStiffnessAdd", value);
				}
			}
			property int ReachWithOneHand
			{
				void set(int value)
				{
					SetArgument("reachWithOneHand", value);
				}
			}
			property bool AllowLeftPistolRFW
			{
				void set(bool value)
				{
					SetArgument("allowLeftPistolRFW", value);
				}
			}
			property bool AllowRightPistolRFW
			{
				void set(bool value)
				{
					SetArgument("allowRightPistolRFW", value);
				}
			}
			property bool RfwWithPistol
			{
				void set(bool value)
				{
					SetArgument("rfwWithPistol", value);
				}
			}
			property bool Fling2
			{
				void set(bool value)
				{
					SetArgument("fling2", value);
				}
			}
			property bool Fling2Left
			{
				void set(bool value)
				{
					SetArgument("fling2Left", value);
				}
			}
			property bool Fling2Right
			{
				void set(bool value)
				{
					SetArgument("fling2Right", value);
				}
			}
			property bool Fling2OverrideStagger
			{
				void set(bool value)
				{
					SetArgument("fling2OverrideStagger", value);
				}
			}
			property float Fling2TimeBefore
			{
				void set(float value)
				{
					SetArgument("fling2TimeBefore", value);
				}
			}
			property float Fling2Time
			{
				void set(float value)
				{
					SetArgument("fling2Time", value);
				}
			}
			property float Fling2MStiffL
			{
				void set(float value)
				{
					SetArgument("fling2MStiffL", value);
				}
			}
			property float Fling2MStiffR
			{
				void set(float value)
				{
					SetArgument("fling2MStiffR", value);
				}
			}
			property float Fling2RelaxTimeL
			{
				void set(float value)
				{
					SetArgument("fling2RelaxTimeL", value);
				}
			}
			property float Fling2RelaxTimeR
			{
				void set(float value)
				{
					SetArgument("fling2RelaxTimeR", value);
				}
			}
			property float Fling2AngleMinL
			{
				void set(float value)
				{
					SetArgument("fling2AngleMinL", value);
				}
			}
			property float Fling2AngleMaxL
			{
				void set(float value)
				{
					SetArgument("fling2AngleMaxL", value);
				}
			}
			property float Fling2AngleMinR
			{
				void set(float value)
				{
					SetArgument("fling2AngleMinR", value);
				}
			}
			property float Fling2AngleMaxR
			{
				void set(float value)
				{
					SetArgument("fling2AngleMaxR", value);
				}
			}
			property float Fling2LengthMinL
			{
				void set(float value)
				{
					SetArgument("fling2LengthMinL", value);
				}
			}
			property float Fling2LengthMaxL
			{
				void set(float value)
				{
					SetArgument("fling2LengthMaxL", value);
				}
			}
			property float Fling2LengthMinR
			{
				void set(float value)
				{
					SetArgument("fling2LengthMinR", value);
				}
			}
			property float Fling2LengthMaxR
			{
				void set(float value)
				{
					SetArgument("fling2LengthMaxR", value);
				}
			}
			property bool Bust
			{
				void set(bool value)
				{
					SetArgument("bust", value);
				}
			}
			property float BustElbowLift
			{
				void set(float value)
				{
					SetArgument("bustElbowLift", value);
				}
			}
			property float CupSize
			{
				void set(float value)
				{
					SetArgument("cupSize", value);
				}
			}
			property bool CupBust
			{
				void set(bool value)
				{
					SetArgument("cupBust", value);
				}
			}
		};
		public ref class SmartFallHelper sealed : public CustomHelper
		{
		public:
			SmartFallHelper(Ped ^ped) : CustomHelper(ped, "smartFall")
			{
			}

			property float BodyStiffness
			{
				void set(float value)
				{
					SetArgument("bodyStiffness", value);
				}
			}
			property float Bodydamping
			{
				void set(float value)
				{
					SetArgument("bodydamping", value);
				}
			}
			property float Catchfalltime
			{
				void set(float value)
				{
					SetArgument("catchfalltime", value);
				}
			}
			property float CrashOrLandCutOff
			{
				void set(float value)
				{
					SetArgument("crashOrLandCutOff", value);
				}
			}
			property float PdStrength
			{
				void set(float value)
				{
					SetArgument("pdStrength", value);
				}
			}
			property float PdDamping
			{
				void set(float value)
				{
					SetArgument("pdDamping", value);
				}
			}
			property float ArmAngSpeed
			{
				void set(float value)
				{
					SetArgument("armAngSpeed", value);
				}
			}
			property float ArmAmplitude
			{
				void set(float value)
				{
					SetArgument("armAmplitude", value);
				}
			}
			property float ArmPhase
			{
				void set(float value)
				{
					SetArgument("armPhase", value);
				}
			}
			property bool ArmBendElbows
			{
				void set(bool value)
				{
					SetArgument("armBendElbows", value);
				}
			}
			property float LegRadius
			{
				void set(float value)
				{
					SetArgument("legRadius", value);
				}
			}
			property float LegAngSpeed
			{
				void set(float value)
				{
					SetArgument("legAngSpeed", value);
				}
			}
			property float LegAsymmetry
			{
				void set(float value)
				{
					SetArgument("legAsymmetry", value);
				}
			}
			property float Arms2LegsPhase
			{
				void set(float value)
				{
					SetArgument("arms2LegsPhase", value);
				}
			}
			property int Arms2LegsSync
			{
				void set(int value)
				{
					SetArgument("arms2LegsSync", value);
				}
			}
			property float ArmsUp
			{
				void set(float value)
				{
					SetArgument("armsUp", value);
				}
			}
			property bool OrientateBodyToFallDirection
			{
				void set(bool value)
				{
					SetArgument("orientateBodyToFallDirection", value);
				}
			}
			property bool OrientateTwist
			{
				void set(bool value)
				{
					SetArgument("orientateTwist", value);
				}
			}
			property float OrientateMax
			{
				void set(float value)
				{
					SetArgument("orientateMax", value);
				}
			}
			property bool AlanRickman
			{
				void set(bool value)
				{
					SetArgument("alanRickman", value);
				}
			}
			property bool FowardRoll
			{
				void set(bool value)
				{
					SetArgument("fowardRoll", value);
				}
			}
			property bool UseZeroPose_withFowardRoll
			{
				void set(bool value)
				{
					SetArgument("useZeroPose_withFowardRoll", value);
				}
			}
			property float AimAngleBase
			{
				void set(float value)
				{
					SetArgument("aimAngleBase", value);
				}
			}
			property float FowardVelRotation
			{
				void set(float value)
				{
					SetArgument("fowardVelRotation", value);
				}
			}
			property float FootVelCompScale
			{
				void set(float value)
				{
					SetArgument("footVelCompScale", value);
				}
			}
			property float SideD
			{
				void set(float value)
				{
					SetArgument("sideD", value);
				}
			}
			property float FowardOffsetOfLegIK
			{
				void set(float value)
				{
					SetArgument("fowardOffsetOfLegIK", value);
				}
			}
			property float LegL
			{
				void set(float value)
				{
					SetArgument("legL", value);
				}
			}
			property float CatchFallCutOff
			{
				void set(float value)
				{
					SetArgument("catchFallCutOff", value);
				}
			}
			property float LegStrength
			{
				void set(float value)
				{
					SetArgument("legStrength", value);
				}
			}
			property bool Balance
			{
				void set(bool value)
				{
					SetArgument("balance", value);
				}
			}
			property bool IgnorWorldCollisions
			{
				void set(bool value)
				{
					SetArgument("ignorWorldCollisions", value);
				}
			}
			property bool AdaptiveCircling
			{
				void set(bool value)
				{
					SetArgument("adaptiveCircling", value);
				}
			}
			property bool Hula
			{
				void set(bool value)
				{
					SetArgument("hula", value);
				}
			}
			property float MaxSpeedForRecoverableFall
			{
				void set(float value)
				{
					SetArgument("maxSpeedForRecoverableFall", value);
				}
			}
			property float MinSpeedForBrace
			{
				void set(float value)
				{
					SetArgument("minSpeedForBrace", value);
				}
			}
			property float LandingNormal
			{
				void set(float value)
				{
					SetArgument("landingNormal", value);
				}
			}
			property float RdsForceMag
			{
				void set(float value)
				{
					SetArgument("rdsForceMag", value);
				}
			}
			property float RdsTargetLinVeDecayTime
			{
				void set(float value)
				{
					SetArgument("rdsTargetLinVeDecayTime", value);
				}
			}
			property float RdsTargetLinearVelocity
			{
				void set(float value)
				{
					SetArgument("rdsTargetLinearVelocity", value);
				}
			}
			property bool RdsUseStartingFriction
			{
				void set(bool value)
				{
					SetArgument("rdsUseStartingFriction", value);
				}
			}
			property float RdsStartingFriction
			{
				void set(float value)
				{
					SetArgument("rdsStartingFriction", value);
				}
			}
			property float RdsStartingFrictionMin
			{
				void set(float value)
				{
					SetArgument("rdsStartingFrictionMin", value);
				}
			}
			property float RdsForceVelThreshold
			{
				void set(float value)
				{
					SetArgument("rdsForceVelThreshold", value);
				}
			}
			property int InitialState
			{
				void set(int value)
				{
					SetArgument("initialState", value);
				}
			}
			property bool ChangeExtremityFriction
			{
				void set(bool value)
				{
					SetArgument("changeExtremityFriction", value);
				}
			}
			property bool Teeter
			{
				void set(bool value)
				{
					SetArgument("teeter", value);
				}
			}
			property float TeeterOffset
			{
				void set(float value)
				{
					SetArgument("teeterOffset", value);
				}
			}
			property float StopRollingTime
			{
				void set(float value)
				{
					SetArgument("stopRollingTime", value);
				}
			}
			property float ReboundScale
			{
				void set(float value)
				{
					SetArgument("reboundScale", value);
				}
			}
			property System::String ^ReboundMask
			{
				void set(System::String ^value)
				{
					SetArgument("reboundMask", value);
				}
			}
			property bool ForceHeadAvoid
			{
				void set(bool value)
				{
					SetArgument("forceHeadAvoid", value);
				}
			}
			property float CfZAxisSpinReduction
			{
				void set(float value)
				{
					SetArgument("cfZAxisSpinReduction", value);
				}
			}
			property float SplatWhenStopped
			{
				void set(float value)
				{
					SetArgument("splatWhenStopped", value);
				}
			}
			property float BlendHeadWhenStopped
			{
				void set(float value)
				{
					SetArgument("blendHeadWhenStopped", value);
				}
			}
			property float SpreadLegs
			{
				void set(float value)
				{
					SetArgument("spreadLegs", value);
				}
			}
		};
		public ref class StaggerFallHelper sealed : public CustomHelper
		{
		public:
			StaggerFallHelper(Ped ^ped) : CustomHelper(ped, "staggerFall")
			{
			}

			property float ArmStiffness
			{
				void set(float value)
				{
					SetArgument("armStiffness", value);
				}
			}
			property float ArmDamping
			{
				void set(float value)
				{
					SetArgument("armDamping", value);
				}
			}
			property float SpineDamping
			{
				void set(float value)
				{
					SetArgument("spineDamping", value);
				}
			}
			property float SpineStiffness
			{
				void set(float value)
				{
					SetArgument("spineStiffness", value);
				}
			}
			property float ArmStiffnessStart
			{
				void set(float value)
				{
					SetArgument("armStiffnessStart", value);
				}
			}
			property float ArmDampingStart
			{
				void set(float value)
				{
					SetArgument("armDampingStart", value);
				}
			}
			property float SpineDampingStart
			{
				void set(float value)
				{
					SetArgument("spineDampingStart", value);
				}
			}
			property float SpineStiffnessStart
			{
				void set(float value)
				{
					SetArgument("spineStiffnessStart", value);
				}
			}
			property float TimeAtStartValues
			{
				void set(float value)
				{
					SetArgument("timeAtStartValues", value);
				}
			}
			property float RampTimeFromStartValues
			{
				void set(float value)
				{
					SetArgument("rampTimeFromStartValues", value);
				}
			}
			property float StaggerStepProb
			{
				void set(float value)
				{
					SetArgument("staggerStepProb", value);
				}
			}
			property int StepsTillStartEnd
			{
				void set(int value)
				{
					SetArgument("stepsTillStartEnd", value);
				}
			}
			property float TimeStartEnd
			{
				void set(float value)
				{
					SetArgument("timeStartEnd", value);
				}
			}
			property float RampTimeToEndValues
			{
				void set(float value)
				{
					SetArgument("rampTimeToEndValues", value);
				}
			}
			property float LowerBodyStiffness
			{
				void set(float value)
				{
					SetArgument("lowerBodyStiffness", value);
				}
			}
			property float LowerBodyStiffnessEnd
			{
				void set(float value)
				{
					SetArgument("lowerBodyStiffnessEnd", value);
				}
			}
			property float PredictionTime
			{
				void set(float value)
				{
					SetArgument("predictionTime", value);
				}
			}
			property float PerStepReduction1
			{
				void set(float value)
				{
					SetArgument("perStepReduction1", value);
				}
			}
			property float LeanInDirRate
			{
				void set(float value)
				{
					SetArgument("leanInDirRate", value);
				}
			}
			property float LeanInDirMaxF
			{
				void set(float value)
				{
					SetArgument("leanInDirMaxF", value);
				}
			}
			property float LeanInDirMaxB
			{
				void set(float value)
				{
					SetArgument("leanInDirMaxB", value);
				}
			}
			property float LeanHipsMaxF
			{
				void set(float value)
				{
					SetArgument("leanHipsMaxF", value);
				}
			}
			property float LeanHipsMaxB
			{
				void set(float value)
				{
					SetArgument("leanHipsMaxB", value);
				}
			}
			property float Lean2multF
			{
				void set(float value)
				{
					SetArgument("lean2multF", value);
				}
			}
			property float Lean2multB
			{
				void set(float value)
				{
					SetArgument("lean2multB", value);
				}
			}
			property float PushOffDist
			{
				void set(float value)
				{
					SetArgument("pushOffDist", value);
				}
			}
			property float MaxPushoffVel
			{
				void set(float value)
				{
					SetArgument("maxPushoffVel", value);
				}
			}
			property float HipBendMult
			{
				void set(float value)
				{
					SetArgument("hipBendMult", value);
				}
			}
			property bool AlwaysBendForwards
			{
				void set(bool value)
				{
					SetArgument("alwaysBendForwards", value);
				}
			}
			property float SpineBendMult
			{
				void set(float value)
				{
					SetArgument("spineBendMult", value);
				}
			}
			property bool UseHeadLook
			{
				void set(bool value)
				{
					SetArgument("useHeadLook", value);
				}
			}
			property Math::Vector3 HeadLookPos
			{
				void set(Math::Vector3 value)
				{
					SetArgument("headLookPos", value);
				}
			}
			property int HeadLookInstanceIndex
			{
				void set(int value)
				{
					SetArgument("headLookInstanceIndex", value);
				}
			}
			property float HeadLookAtVelProb
			{
				void set(float value)
				{
					SetArgument("headLookAtVelProb", value);
				}
			}
			property float TurnOffProb
			{
				void set(float value)
				{
					SetArgument("turnOffProb", value);
				}
			}
			property float Turn2TargetProb
			{
				void set(float value)
				{
					SetArgument("turn2TargetProb", value);
				}
			}
			property float Turn2VelProb
			{
				void set(float value)
				{
					SetArgument("turn2VelProb", value);
				}
			}
			property float TurnAwayProb
			{
				void set(float value)
				{
					SetArgument("turnAwayProb", value);
				}
			}
			property float TurnLeftProb
			{
				void set(float value)
				{
					SetArgument("turnLeftProb", value);
				}
			}
			property float TurnRightProb
			{
				void set(float value)
				{
					SetArgument("turnRightProb", value);
				}
			}
			property bool UseBodyTurn
			{
				void set(bool value)
				{
					SetArgument("useBodyTurn", value);
				}
			}
			property bool UpperBodyReaction
			{
				void set(bool value)
				{
					SetArgument("upperBodyReaction", value);
				}
			}
		};
		public ref class TeeterHelper sealed : public CustomHelper
		{
		public:
			TeeterHelper(Ped ^ped) : CustomHelper(ped, "teeter")
			{
			}

			property Math::Vector3 EdgeLeft
			{
				void set(Math::Vector3 value)
				{
					SetArgument("edgeLeft", value);
				}
			}
			property Math::Vector3 EdgeRight
			{
				void set(Math::Vector3 value)
				{
					SetArgument("edgeRight", value);
				}
			}
			property bool UseExclusionZone
			{
				void set(bool value)
				{
					SetArgument("useExclusionZone", value);
				}
			}
			property bool UseHeadLook
			{
				void set(bool value)
				{
					SetArgument("useHeadLook", value);
				}
			}
			property bool CallHighFall
			{
				void set(bool value)
				{
					SetArgument("callHighFall", value);
				}
			}
			property bool LeanAway
			{
				void set(bool value)
				{
					SetArgument("leanAway", value);
				}
			}
			property float PreTeeterTime
			{
				void set(float value)
				{
					SetArgument("preTeeterTime", value);
				}
			}
			property float LeanAwayTime
			{
				void set(float value)
				{
					SetArgument("leanAwayTime", value);
				}
			}
			property float LeanAwayScale
			{
				void set(float value)
				{
					SetArgument("leanAwayScale", value);
				}
			}
			property float TeeterTime
			{
				void set(float value)
				{
					SetArgument("teeterTime", value);
				}
			}
		};
		public ref class UpperBodyFlinchHelper sealed : public CustomHelper
		{
		public:
			UpperBodyFlinchHelper(Ped ^ped) : CustomHelper(ped, "upperBodyFlinch")
			{
			}

			property float HandDistanceLeftRight
			{
				void set(float value)
				{
					SetArgument("handDistanceLeftRight", value);
				}
			}
			property float HandDistanceFrontBack
			{
				void set(float value)
				{
					SetArgument("handDistanceFrontBack", value);
				}
			}
			property float HandDistanceVertical
			{
				void set(float value)
				{
					SetArgument("handDistanceVertical", value);
				}
			}
			property float BodyStiffness
			{
				void set(float value)
				{
					SetArgument("bodyStiffness", value);
				}
			}
			property float BodyDamping
			{
				void set(float value)
				{
					SetArgument("bodyDamping", value);
				}
			}
			property float BackBendAmount
			{
				void set(float value)
				{
					SetArgument("backBendAmount", value);
				}
			}
			property bool UseRightArm
			{
				void set(bool value)
				{
					SetArgument("useRightArm", value);
				}
			}
			property bool UseLeftArm
			{
				void set(bool value)
				{
					SetArgument("useLeftArm", value);
				}
			}
			property float NoiseScale
			{
				void set(float value)
				{
					SetArgument("noiseScale", value);
				}
			}
			property bool NewHit
			{
				void set(bool value)
				{
					SetArgument("newHit", value);
				}
			}
			property bool ProtectHeadToggle
			{
				void set(bool value)
				{
					SetArgument("protectHeadToggle", value);
				}
			}
			property bool DontBraceHead
			{
				void set(bool value)
				{
					SetArgument("dontBraceHead", value);
				}
			}
			property bool ApplyStiffness
			{
				void set(bool value)
				{
					SetArgument("applyStiffness", value);
				}
			}
			property bool HeadLookAwayFromTarget
			{
				void set(bool value)
				{
					SetArgument("headLookAwayFromTarget", value);
				}
			}
			property bool UseHeadLook
			{
				void set(bool value)
				{
					SetArgument("useHeadLook", value);
				}
			}
			property int TurnTowards
			{
				void set(int value)
				{
					SetArgument("turnTowards", value);
				}
			}
			property Math::Vector3 Pos
			{
				void set(Math::Vector3 value)
				{
					SetArgument("pos", value);
				}
			}
		};
		public ref class YankedHelper sealed : public CustomHelper
		{
		public:
			YankedHelper(Ped ^ped) : CustomHelper(ped, "yanked")
			{
			}

			property float ArmStiffness
			{
				void set(float value)
				{
					SetArgument("armStiffness", value);
				}
			}
			property float ArmDamping
			{
				void set(float value)
				{
					SetArgument("armDamping", value);
				}
			}
			property float SpineDamping
			{
				void set(float value)
				{
					SetArgument("spineDamping", value);
				}
			}
			property float SpineStiffness
			{
				void set(float value)
				{
					SetArgument("spineStiffness", value);
				}
			}
			property float ArmStiffnessStart
			{
				void set(float value)
				{
					SetArgument("armStiffnessStart", value);
				}
			}
			property float ArmDampingStart
			{
				void set(float value)
				{
					SetArgument("armDampingStart", value);
				}
			}
			property float SpineDampingStart
			{
				void set(float value)
				{
					SetArgument("spineDampingStart", value);
				}
			}
			property float SpineStiffnessStart
			{
				void set(float value)
				{
					SetArgument("spineStiffnessStart", value);
				}
			}
			property float TimeAtStartValues
			{
				void set(float value)
				{
					SetArgument("timeAtStartValues", value);
				}
			}
			property float RampTimeFromStartValues
			{
				void set(float value)
				{
					SetArgument("rampTimeFromStartValues", value);
				}
			}
			property int StepsTillStartEnd
			{
				void set(int value)
				{
					SetArgument("stepsTillStartEnd", value);
				}
			}
			property float TimeStartEnd
			{
				void set(float value)
				{
					SetArgument("timeStartEnd", value);
				}
			}
			property float RampTimeToEndValues
			{
				void set(float value)
				{
					SetArgument("rampTimeToEndValues", value);
				}
			}
			property float LowerBodyStiffness
			{
				void set(float value)
				{
					SetArgument("lowerBodyStiffness", value);
				}
			}
			property float LowerBodyStiffnessEnd
			{
				void set(float value)
				{
					SetArgument("lowerBodyStiffnessEnd", value);
				}
			}
			property float PerStepReduction
			{
				void set(float value)
				{
					SetArgument("perStepReduction", value);
				}
			}
			property float HipPitchForward
			{
				void set(float value)
				{
					SetArgument("hipPitchForward", value);
				}
			}
			property float HipPitchBack
			{
				void set(float value)
				{
					SetArgument("hipPitchBack", value);
				}
			}
			property float SpineBend
			{
				void set(float value)
				{
					SetArgument("spineBend", value);
				}
			}
			property float FootFriction
			{
				void set(float value)
				{
					SetArgument("footFriction", value);
				}
			}
			property float TurnThresholdMin
			{
				void set(float value)
				{
					SetArgument("turnThresholdMin", value);
				}
			}
			property float TurnThresholdMax
			{
				void set(float value)
				{
					SetArgument("turnThresholdMax", value);
				}
			}
			property bool UseHeadLook
			{
				void set(bool value)
				{
					SetArgument("useHeadLook", value);
				}
			}
			property Math::Vector3 HeadLookPos
			{
				void set(Math::Vector3 value)
				{
					SetArgument("headLookPos", value);
				}
			}
			property int HeadLookInstanceIndex
			{
				void set(int value)
				{
					SetArgument("headLookInstanceIndex", value);
				}
			}
			property float HeadLookAtVelProb
			{
				void set(float value)
				{
					SetArgument("headLookAtVelProb", value);
				}
			}
			property float ComVelRDSThresh
			{
				void set(float value)
				{
					SetArgument("comVelRDSThresh", value);
				}
			}
			property float HulaPeriod
			{
				void set(float value)
				{
					SetArgument("hulaPeriod", value);
				}
			}
			property float HipAmplitude
			{
				void set(float value)
				{
					SetArgument("hipAmplitude", value);
				}
			}
			property float SpineAmplitude
			{
				void set(float value)
				{
					SetArgument("spineAmplitude", value);
				}
			}
			property float MinRelaxPeriod
			{
				void set(float value)
				{
					SetArgument("minRelaxPeriod", value);
				}
			}
			property float MaxRelaxPeriod
			{
				void set(float value)
				{
					SetArgument("maxRelaxPeriod", value);
				}
			}
			property float RollHelp
			{
				void set(float value)
				{
					SetArgument("rollHelp", value);
				}
			}
			property float GroundLegStiffness
			{
				void set(float value)
				{
					SetArgument("groundLegStiffness", value);
				}
			}
			property float GroundArmStiffness
			{
				void set(float value)
				{
					SetArgument("groundArmStiffness", value);
				}
			}
			property float GroundSpineStiffness
			{
				void set(float value)
				{
					SetArgument("groundSpineStiffness", value);
				}
			}
			property float GroundLegDamping
			{
				void set(float value)
				{
					SetArgument("groundLegDamping", value);
				}
			}
			property float GroundArmDamping
			{
				void set(float value)
				{
					SetArgument("groundArmDamping", value);
				}
			}
			property float GroundSpineDamping
			{
				void set(float value)
				{
					SetArgument("groundSpineDamping", value);
				}
			}
			property float GroundFriction
			{
				void set(float value)
				{
					SetArgument("groundFriction", value);
				}
			}
		};
	}
}
