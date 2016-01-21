#pragma once

#include "EuphoriaBase.hpp"
#include "EuphoriaHelpers.hpp"

namespace GTA
{
	namespace NaturalMotion
	{
		public ref class Euphoria sealed
		{
		private:
			Ped ^_ped;
			System::Collections::Generic::Dictionary<System::String ^, CustomHelper ^> ^_helperCache;

			generic <typename T> where T : CustomHelper
			T GetHelper(System::String ^message);

		internal:
			Euphoria(Ped ^ped);

		public:
			property ActivePoseHelper ^ActivePose
			{
				ActivePoseHelper ^get()
				{
					return GetHelper<ActivePoseHelper^>("activePose");
				}
			}
			property ApplyImpulseHelper ^ApplyImpulse
			{
				ApplyImpulseHelper ^get()
				{
					return GetHelper<ApplyImpulseHelper^>("applyImpulse");
				}
			}
			property ApplyBulletImpulseHelper ^ApplyBulletImpulse
			{
				ApplyBulletImpulseHelper ^get()
				{
					return GetHelper<ApplyBulletImpulseHelper^>("applyBulletImpulse");
				}
			}
			property BodyRelaxHelper ^BodyRelax
			{
				BodyRelaxHelper ^get()
				{
					return GetHelper<BodyRelaxHelper^>("bodyRelax");
				}
			}
			property ConfigureBalanceHelper ^ConfigureBalance
			{
				ConfigureBalanceHelper ^get()
				{
					return GetHelper<ConfigureBalanceHelper^>("configureBalance");
				}
			}
			property ConfigureBalanceResetHelper ^ConfigureBalanceReset
			{
				ConfigureBalanceResetHelper ^get()
				{
					return GetHelper<ConfigureBalanceResetHelper^>("configureBalanceReset");
				}
			}
			property ConfigureSelfAvoidanceHelper ^ConfigureSelfAvoidance
			{
				ConfigureSelfAvoidanceHelper ^get()
				{
					return GetHelper<ConfigureSelfAvoidanceHelper^>("configureSelfAvoidance");
				}
			}
			property ConfigureBulletsHelper ^ConfigureBullets
			{
				ConfigureBulletsHelper ^get()
				{
					return GetHelper<ConfigureBulletsHelper^>("configureBullets");
				}
			}
			property ConfigureBulletsExtraHelper ^ConfigureBulletsExtra
			{
				ConfigureBulletsExtraHelper ^get()
				{
					return GetHelper<ConfigureBulletsExtraHelper^>("configureBulletsExtra");
				}
			}
			property ConfigureLimitsHelper ^ConfigureLimits
			{
				ConfigureLimitsHelper ^get()
				{
					return GetHelper<ConfigureLimitsHelper^>("configureLimits");
				}
			}
			property ConfigureSoftLimitHelper ^ConfigureSoftLimit
			{
				ConfigureSoftLimitHelper ^get()
				{
					return GetHelper<ConfigureSoftLimitHelper^>("configureSoftLimit");
				}
			}
			property ConfigureShotInjuredArmHelper ^ConfigureShotInjuredArm
			{
				ConfigureShotInjuredArmHelper ^get()
				{
					return GetHelper<ConfigureShotInjuredArmHelper^>("configureShotInjuredArm");
				}
			}
			property ConfigureShotInjuredLegHelper ^ConfigureShotInjuredLeg
			{
				ConfigureShotInjuredLegHelper ^get()
				{
					return GetHelper<ConfigureShotInjuredLegHelper^>("configureShotInjuredLeg");
				}
			}
			property DefineAttachedObjectHelper ^DefineAttachedObject
			{
				DefineAttachedObjectHelper ^get()
				{
					return GetHelper<DefineAttachedObjectHelper^>("defineAttachedObject");
				}
			}
			property ForceToBodyPartHelper ^ForceToBodyPart
			{
				ForceToBodyPartHelper ^get()
				{
					return GetHelper<ForceToBodyPartHelper^>("forceToBodyPart");
				}
			}
			property LeanInDirectionHelper ^LeanInDirection
			{
				LeanInDirectionHelper ^get()
				{
					return GetHelper<LeanInDirectionHelper^>("leanInDirection");
				}
			}
			property LeanRandomHelper ^LeanRandom
			{
				LeanRandomHelper ^get()
				{
					return GetHelper<LeanRandomHelper^>("leanRandom");
				}
			}
			property LeanToPositionHelper ^LeanToPosition
			{
				LeanToPositionHelper ^get()
				{
					return GetHelper<LeanToPositionHelper^>("leanToPosition");
				}
			}
			property LeanTowardsObjectHelper ^LeanTowardsObject
			{
				LeanTowardsObjectHelper ^get()
				{
					return GetHelper<LeanTowardsObjectHelper^>("leanTowardsObject");
				}
			}
			property HipsLeanInDirectionHelper ^HipsLeanInDirection
			{
				HipsLeanInDirectionHelper ^get()
				{
					return GetHelper<HipsLeanInDirectionHelper^>("hipsLeanInDirection");
				}
			}
			property HipsLeanRandomHelper ^HipsLeanRandom
			{
				HipsLeanRandomHelper ^get()
				{
					return GetHelper<HipsLeanRandomHelper^>("hipsLeanRandom");
				}
			}
			property HipsLeanToPositionHelper ^HipsLeanToPosition
			{
				HipsLeanToPositionHelper ^get()
				{
					return GetHelper<HipsLeanToPositionHelper^>("hipsLeanToPosition");
				}
			}
			property HipsLeanTowardsObjectHelper ^HipsLeanTowardsObject
			{
				HipsLeanTowardsObjectHelper ^get()
				{
					return GetHelper<HipsLeanTowardsObjectHelper^>("hipsLeanTowardsObject");
				}
			}
			property ForceLeanInDirectionHelper ^ForceLeanInDirection
			{
				ForceLeanInDirectionHelper ^get()
				{
					return GetHelper<ForceLeanInDirectionHelper^>("forceLeanInDirection");
				}
			}
			property ForceLeanRandomHelper ^ForceLeanRandom
			{
				ForceLeanRandomHelper ^get()
				{
					return GetHelper<ForceLeanRandomHelper^>("forceLeanRandom");
				}
			}
			property ForceLeanToPositionHelper ^ForceLeanToPosition
			{
				ForceLeanToPositionHelper ^get()
				{
					return GetHelper<ForceLeanToPositionHelper^>("forceLeanToPosition");
				}
			}
			property ForceLeanTowardsObjectHelper ^ForceLeanTowardsObject
			{
				ForceLeanTowardsObjectHelper ^get()
				{
					return GetHelper<ForceLeanTowardsObjectHelper^>("forceLeanTowardsObject");
				}
			}
			property SetStiffnessHelper ^SetStiffness
			{
				SetStiffnessHelper ^get()
				{
					return GetHelper<SetStiffnessHelper^>("setStiffness");
				}
			}
			property SetMuscleStiffnessHelper ^SetMuscleStiffness
			{
				SetMuscleStiffnessHelper ^get()
				{
					return GetHelper<SetMuscleStiffnessHelper^>("setMuscleStiffness");
				}
			}
			property SetWeaponModeHelper ^SetWeaponMode
			{
				SetWeaponModeHelper ^get()
				{
					return GetHelper<SetWeaponModeHelper^>("setWeaponMode");
				}
			}
			property RegisterWeaponHelper ^RegisterWeapon
			{
				RegisterWeaponHelper ^get()
				{
					return GetHelper<RegisterWeaponHelper^>("registerWeapon");
				}
			}
			property ShotRelaxHelper ^ShotRelax
			{
				ShotRelaxHelper ^get()
				{
					return GetHelper<ShotRelaxHelper^>("shotRelax");
				}
			}
			property FireWeaponHelper ^FireWeapon
			{
				FireWeaponHelper ^get()
				{
					return GetHelper<FireWeaponHelper^>("fireWeapon");
				}
			}
			property ConfigureConstraintsHelper ^ConfigureConstraints
			{
				ConfigureConstraintsHelper ^get()
				{
					return GetHelper<ConfigureConstraintsHelper^>("configureConstraints");
				}
			}
			property StayUprightHelper ^StayUpright
			{
				StayUprightHelper ^get()
				{
					return GetHelper<StayUprightHelper^>("stayUpright");
				}
			}
			property StopAllBehavioursHelper ^StopAllBehaviours
			{
				StopAllBehavioursHelper ^get()
				{
					return GetHelper<StopAllBehavioursHelper^>("stopAllBehaviours");
				}
			}
			property SetCharacterStrengthHelper ^SetCharacterStrength
			{
				SetCharacterStrengthHelper ^get()
				{
					return GetHelper<SetCharacterStrengthHelper^>("setCharacterStrength");
				}
			}
			property SetCharacterHealthHelper ^SetCharacterHealth
			{
				SetCharacterHealthHelper ^get()
				{
					return GetHelper<SetCharacterHealthHelper^>("setCharacterHealth");
				}
			}
			property SetFallingReactionHelper ^SetFallingReaction
			{
				SetFallingReactionHelper ^get()
				{
					return GetHelper<SetFallingReactionHelper^>("setFallingReaction");
				}
			}
			property SetCharacterUnderwaterHelper ^SetCharacterUnderwater
			{
				SetCharacterUnderwaterHelper ^get()
				{
					return GetHelper<SetCharacterUnderwaterHelper^>("setCharacterUnderwater");
				}
			}
			property SetCharacterCollisionsHelper ^SetCharacterCollisions
			{
				SetCharacterCollisionsHelper ^get()
				{
					return GetHelper<SetCharacterCollisionsHelper^>("setCharacterCollisions");
				}
			}
			property SetCharacterDampingHelper ^SetCharacterDamping
			{
				SetCharacterDampingHelper ^get()
				{
					return GetHelper<SetCharacterDampingHelper^>("setCharacterDamping");
				}
			}
			property SetFrictionScaleHelper ^SetFrictionScale
			{
				SetFrictionScaleHelper ^get()
				{
					return GetHelper<SetFrictionScaleHelper^>("setFrictionScale");
				}
			}
			property AnimPoseHelper ^AnimPose
			{
				AnimPoseHelper ^get()
				{
					return GetHelper<AnimPoseHelper^>("animPose");
				}
			}
			property ArmsWindmillHelper ^ArmsWindmill
			{
				ArmsWindmillHelper ^get()
				{
					return GetHelper<ArmsWindmillHelper^>("armsWindmill");
				}
			}
			property ArmsWindmillAdaptiveHelper ^ArmsWindmillAdaptive
			{
				ArmsWindmillAdaptiveHelper ^get()
				{
					return GetHelper<ArmsWindmillAdaptiveHelper^>("armsWindmillAdaptive");
				}
			}
			property BalancerCollisionsReactionHelper ^BalancerCollisionsReaction
			{
				BalancerCollisionsReactionHelper ^get()
				{
					return GetHelper<BalancerCollisionsReactionHelper^>("balancerCollisionsReaction");
				}
			}
			property BodyBalanceHelper ^BodyBalance
			{
				BodyBalanceHelper ^get()
				{
					return GetHelper<BodyBalanceHelper^>("bodyBalance");
				}
			}
			property BodyFoetalHelper ^BodyFoetal
			{
				BodyFoetalHelper ^get()
				{
					return GetHelper<BodyFoetalHelper^>("bodyFoetal");
				}
			}
			property BodyRollUpHelper ^BodyRollUp
			{
				BodyRollUpHelper ^get()
				{
					return GetHelper<BodyRollUpHelper^>("bodyRollUp");
				}
			}
			property BodyWritheHelper ^BodyWrithe
			{
				BodyWritheHelper ^get()
				{
					return GetHelper<BodyWritheHelper^>("bodyWrithe");
				}
			}
			property BraceForImpactHelper ^BraceForImpact
			{
				BraceForImpactHelper ^get()
				{
					return GetHelper<BraceForImpactHelper^>("braceForImpact");
				}
			}
			property BuoyancyHelper ^Buoyancy
			{
				BuoyancyHelper ^get()
				{
					return GetHelper<BuoyancyHelper^>("buoyancy");
				}
			}
			property CatchFallHelper ^CatchFall
			{
				CatchFallHelper ^get()
				{
					return GetHelper<CatchFallHelper^>("catchFall");
				}
			}
			property ElectrocuteHelper ^Electrocute
			{
				ElectrocuteHelper ^get()
				{
					return GetHelper<ElectrocuteHelper^>("electrocute");
				}
			}
			property FallOverWallHelper ^FallOverWall
			{
				FallOverWallHelper ^get()
				{
					return GetHelper<FallOverWallHelper^>("fallOverWall");
				}
			}
			property GrabHelper ^Grab
			{
				GrabHelper ^get()
				{
					return GetHelper<GrabHelper^>("grab");
				}
			}
			property HeadLookHelper ^HeadLook
			{
				HeadLookHelper ^get()
				{
					return GetHelper<HeadLookHelper^>("headLook");
				}
			}
			property HighFallHelper ^HighFall
			{
				HighFallHelper ^get()
				{
					return GetHelper<HighFallHelper^>("highFall");
				}
			}
			property IncomingTransformsHelper ^IncomingTransforms
			{
				IncomingTransformsHelper ^get()
				{
					return GetHelper<IncomingTransformsHelper^>("incomingTransforms");
				}
			}
			property InjuredOnGroundHelper ^InjuredOnGround
			{
				InjuredOnGroundHelper ^get()
				{
					return GetHelper<InjuredOnGroundHelper^>("injuredOnGround");
				}
			}
			property CarriedHelper ^Carried
			{
				CarriedHelper ^get()
				{
					return GetHelper<CarriedHelper^>("carried");
				}
			}
			property DangleHelper ^Dangle
			{
				DangleHelper ^get()
				{
					return GetHelper<DangleHelper^>("dangle");
				}
			}
			property OnFireHelper ^OnFire
			{
				OnFireHelper ^get()
				{
					return GetHelper<OnFireHelper^>("onFire");
				}
			}
			property PedalLegsHelper ^PedalLegs
			{
				PedalLegsHelper ^get()
				{
					return GetHelper<PedalLegsHelper^>("pedalLegs");
				}
			}
			property PointArmHelper ^PointArm
			{
				PointArmHelper ^get()
				{
					return GetHelper<PointArmHelper^>("pointArm");
				}
			}
			property PointGunHelper ^PointGun
			{
				PointGunHelper ^get()
				{
					return GetHelper<PointGunHelper^>("pointGun");
				}
			}
			property PointGunExtraHelper ^PointGunExtra
			{
				PointGunExtraHelper ^get()
				{
					return GetHelper<PointGunExtraHelper^>("pointGunExtra");
				}
			}
			property RollDownStairsHelper ^RollDownStairs
			{
				RollDownStairsHelper ^get()
				{
					return GetHelper<RollDownStairsHelper^>("rollDownStairs");
				}
			}
			property ShotHelper ^Shot
			{
				ShotHelper ^get()
				{
					return GetHelper<ShotHelper^>("shot");
				}
			}
			property ShotNewBulletHelper ^ShotNewBullet
			{
				ShotNewBulletHelper ^get()
				{
					return GetHelper<ShotNewBulletHelper^>("shotNewBullet");
				}
			}
			property ShotSnapHelper ^ShotSnap
			{
				ShotSnapHelper ^get()
				{
					return GetHelper<ShotSnapHelper^>("shotSnap");
				}
			}
			property ShotShockSpinHelper ^ShotShockSpin
			{
				ShotShockSpinHelper ^get()
				{
					return GetHelper<ShotShockSpinHelper^>("shotShockSpin");
				}
			}
			property ShotFallToKneesHelper ^ShotFallToKnees
			{
				ShotFallToKneesHelper ^get()
				{
					return GetHelper<ShotFallToKneesHelper^>("shotFallToKnees");
				}
			}
			property ShotFromBehindHelper ^ShotFromBehind
			{
				ShotFromBehindHelper ^get()
				{
					return GetHelper<ShotFromBehindHelper^>("shotFromBehind");
				}
			}
			property ShotInGutsHelper ^ShotInGuts
			{
				ShotInGutsHelper ^get()
				{
					return GetHelper<ShotInGutsHelper^>("shotInGuts");
				}
			}
			property ShotHeadLookHelper ^ShotHeadLook
			{
				ShotHeadLookHelper ^get()
				{
					return GetHelper<ShotHeadLookHelper^>("shotHeadLook");
				}
			}
			property ShotConfigureArmsHelper ^ShotConfigureArms
			{
				ShotConfigureArmsHelper ^get()
				{
					return GetHelper<ShotConfigureArmsHelper^>("shotConfigureArms");
				}
			}
			property SmartFallHelper ^SmartFall
			{
				SmartFallHelper ^get()
				{
					return GetHelper<SmartFallHelper^>("smartFall");
				}
			}
			property StaggerFallHelper ^StaggerFall
			{
				StaggerFallHelper ^get()
				{
					return GetHelper<StaggerFallHelper^>("staggerFall");
				}
			}
			property TeeterHelper ^Teeter
			{
				TeeterHelper ^get()
				{
					return GetHelper<TeeterHelper^>("teeter");
				}
			}
			property UpperBodyFlinchHelper ^UpperBodyFlinch
			{
				UpperBodyFlinchHelper ^get()
				{
					return GetHelper<UpperBodyFlinchHelper^>("upperBodyFlinch");
				}
			}
			property YankedHelper ^Yanked
			{
				YankedHelper ^get()
				{
					return GetHelper<YankedHelper^>("yanked");
				}
			}
		};
	}
}
