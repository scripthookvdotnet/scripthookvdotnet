#pragma once
#include "EuphoriaBase.hpp"
#include "EuphoriaHelpers.hpp"

namespace GTA
{
	namespace NaturalMotion
	{
		using namespace System;
		using namespace System::Collections::Generic;
		public ref class Euphoria sealed {

		private:
			Ped^ ped;
			Dictionary<String ^, BaseHelper^>^ pHelperCache;

			generic <typename HelperType> where HelperType: BaseHelper
				HelperType GetHelper(String ^MessageID);

		internal:
			Euphoria(Ped^ ped);

		public:

			property GTA::NaturalMotion::ActivePoseHelper^ ActivePose {
				GTA::NaturalMotion::ActivePoseHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ActivePoseHelper^>("activePose");
				}
			}

			property GTA::NaturalMotion::ApplyImpulseHelper^ ApplyImpulse {
				GTA::NaturalMotion::ApplyImpulseHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ApplyImpulseHelper^>("applyImpulse");
				}
			}

			property GTA::NaturalMotion::ApplyBulletImpulseHelper^ ApplyBulletImpulse {
				GTA::NaturalMotion::ApplyBulletImpulseHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ApplyBulletImpulseHelper^>("applyBulletImpulse");
				}
			}

			property GTA::NaturalMotion::BodyRelaxHelper^ BodyRelax {
				GTA::NaturalMotion::BodyRelaxHelper^ get() {
					return GetHelper<GTA::NaturalMotion::BodyRelaxHelper^>("bodyRelax");
				}
			}

			property GTA::NaturalMotion::ConfigureBalanceHelper^ ConfigureBalance {
				GTA::NaturalMotion::ConfigureBalanceHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ConfigureBalanceHelper^>("configureBalance");
				}
			}

			property GTA::NaturalMotion::ConfigureBalanceResetHelper^ ConfigureBalanceReset {
				GTA::NaturalMotion::ConfigureBalanceResetHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ConfigureBalanceResetHelper^>("configureBalanceReset");
				}
			}

			property GTA::NaturalMotion::ConfigureSelfAvoidanceHelper^ ConfigureSelfAvoidance {
				GTA::NaturalMotion::ConfigureSelfAvoidanceHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ConfigureSelfAvoidanceHelper^>("configureSelfAvoidance");
				}
			}

			property GTA::NaturalMotion::ConfigureBulletsHelper^ ConfigureBullets {
				GTA::NaturalMotion::ConfigureBulletsHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ConfigureBulletsHelper^>("configureBullets");
				}
			}

			property GTA::NaturalMotion::ConfigureBulletsExtraHelper^ ConfigureBulletsExtra {
				GTA::NaturalMotion::ConfigureBulletsExtraHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ConfigureBulletsExtraHelper^>("configureBulletsExtra");
				}
			}

			property GTA::NaturalMotion::ConfigureLimitsHelper^ ConfigureLimits {
				GTA::NaturalMotion::ConfigureLimitsHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ConfigureLimitsHelper^>("configureLimits");
				}
			}

			property GTA::NaturalMotion::ConfigureSoftLimitHelper^ ConfigureSoftLimit {
				GTA::NaturalMotion::ConfigureSoftLimitHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ConfigureSoftLimitHelper^>("configureSoftLimit");
				}
			}

			property GTA::NaturalMotion::ConfigureShotInjuredArmHelper^ ConfigureShotInjuredArm {
				GTA::NaturalMotion::ConfigureShotInjuredArmHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ConfigureShotInjuredArmHelper^>("configureShotInjuredArm");
				}
			}

			property GTA::NaturalMotion::ConfigureShotInjuredLegHelper^ ConfigureShotInjuredLeg {
				GTA::NaturalMotion::ConfigureShotInjuredLegHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ConfigureShotInjuredLegHelper^>("configureShotInjuredLeg");
				}
			}

			property GTA::NaturalMotion::DefineAttachedObjectHelper^ DefineAttachedObject {
				GTA::NaturalMotion::DefineAttachedObjectHelper^ get() {
					return GetHelper<GTA::NaturalMotion::DefineAttachedObjectHelper^>("defineAttachedObject");
				}
			}

			property GTA::NaturalMotion::ForceToBodyPartHelper^ ForceToBodyPart {
				GTA::NaturalMotion::ForceToBodyPartHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ForceToBodyPartHelper^>("forceToBodyPart");
				}
			}

			property GTA::NaturalMotion::LeanInDirectionHelper^ LeanInDirection {
				GTA::NaturalMotion::LeanInDirectionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::LeanInDirectionHelper^>("leanInDirection");
				}
			}

			property GTA::NaturalMotion::LeanRandomHelper^ LeanRandom {
				GTA::NaturalMotion::LeanRandomHelper^ get() {
					return GetHelper<GTA::NaturalMotion::LeanRandomHelper^>("leanRandom");
				}
			}

			property GTA::NaturalMotion::LeanToPositionHelper^ LeanToPosition {
				GTA::NaturalMotion::LeanToPositionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::LeanToPositionHelper^>("leanToPosition");
				}
			}

			property GTA::NaturalMotion::LeanTowardsObjectHelper^ LeanTowardsObject {
				GTA::NaturalMotion::LeanTowardsObjectHelper^ get() {
					return GetHelper<GTA::NaturalMotion::LeanTowardsObjectHelper^>("leanTowardsObject");
				}
			}

			property GTA::NaturalMotion::HipsLeanInDirectionHelper^ HipsLeanInDirection {
				GTA::NaturalMotion::HipsLeanInDirectionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::HipsLeanInDirectionHelper^>("hipsLeanInDirection");
				}
			}

			property GTA::NaturalMotion::HipsLeanRandomHelper^ HipsLeanRandom {
				GTA::NaturalMotion::HipsLeanRandomHelper^ get() {
					return GetHelper<GTA::NaturalMotion::HipsLeanRandomHelper^>("hipsLeanRandom");
				}
			}

			property GTA::NaturalMotion::HipsLeanToPositionHelper^ HipsLeanToPosition {
				GTA::NaturalMotion::HipsLeanToPositionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::HipsLeanToPositionHelper^>("hipsLeanToPosition");
				}
			}

			property GTA::NaturalMotion::HipsLeanTowardsObjectHelper^ HipsLeanTowardsObject {
				GTA::NaturalMotion::HipsLeanTowardsObjectHelper^ get() {
					return GetHelper<GTA::NaturalMotion::HipsLeanTowardsObjectHelper^>("hipsLeanTowardsObject");
				}
			}

			property GTA::NaturalMotion::ForceLeanInDirectionHelper^ ForceLeanInDirection {
				GTA::NaturalMotion::ForceLeanInDirectionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ForceLeanInDirectionHelper^>("forceLeanInDirection");
				}
			}

			property GTA::NaturalMotion::ForceLeanRandomHelper^ ForceLeanRandom {
				GTA::NaturalMotion::ForceLeanRandomHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ForceLeanRandomHelper^>("forceLeanRandom");
				}
			}

			property GTA::NaturalMotion::ForceLeanToPositionHelper^ ForceLeanToPosition {
				GTA::NaturalMotion::ForceLeanToPositionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ForceLeanToPositionHelper^>("forceLeanToPosition");
				}
			}

			property GTA::NaturalMotion::ForceLeanTowardsObjectHelper^ ForceLeanTowardsObject {
				GTA::NaturalMotion::ForceLeanTowardsObjectHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ForceLeanTowardsObjectHelper^>("forceLeanTowardsObject");
				}
			}

			property GTA::NaturalMotion::SetStiffnessHelper^ SetStiffness {
				GTA::NaturalMotion::SetStiffnessHelper^ get() {
					return GetHelper<GTA::NaturalMotion::SetStiffnessHelper^>("setStiffness");
				}
			}

			property GTA::NaturalMotion::SetMuscleStiffnessHelper^ SetMuscleStiffness {
				GTA::NaturalMotion::SetMuscleStiffnessHelper^ get() {
					return GetHelper<GTA::NaturalMotion::SetMuscleStiffnessHelper^>("setMuscleStiffness");
				}
			}

			property GTA::NaturalMotion::SetWeaponModeHelper^ SetWeaponMode {
				GTA::NaturalMotion::SetWeaponModeHelper^ get() {
					return GetHelper<GTA::NaturalMotion::SetWeaponModeHelper^>("setWeaponMode");
				}
			}

			property GTA::NaturalMotion::RegisterWeaponHelper^ RegisterWeapon {
				GTA::NaturalMotion::RegisterWeaponHelper^ get() {
					return GetHelper<GTA::NaturalMotion::RegisterWeaponHelper^>("registerWeapon");
				}
			}

			property GTA::NaturalMotion::ShotRelaxHelper^ ShotRelax {
				GTA::NaturalMotion::ShotRelaxHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ShotRelaxHelper^>("shotRelax");
				}
			}

			property GTA::NaturalMotion::FireWeaponHelper^ FireWeapon {
				GTA::NaturalMotion::FireWeaponHelper^ get() {
					return GetHelper<GTA::NaturalMotion::FireWeaponHelper^>("fireWeapon");
				}
			}

			property GTA::NaturalMotion::ConfigureConstraintsHelper^ ConfigureConstraints {
				GTA::NaturalMotion::ConfigureConstraintsHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ConfigureConstraintsHelper^>("configureConstraints");
				}
			}

			property GTA::NaturalMotion::StayUprightHelper^ StayUpright {
				GTA::NaturalMotion::StayUprightHelper^ get() {
					return GetHelper<GTA::NaturalMotion::StayUprightHelper^>("stayUpright");
				}
			}

			property GTA::NaturalMotion::StopAllBehavioursHelper^ StopAllBehaviours {
				GTA::NaturalMotion::StopAllBehavioursHelper^ get() {
					return GetHelper<GTA::NaturalMotion::StopAllBehavioursHelper^>("stopAllBehaviours");
				}
			}

			property GTA::NaturalMotion::SetCharacterStrengthHelper^ SetCharacterStrength {
				GTA::NaturalMotion::SetCharacterStrengthHelper^ get() {
					return GetHelper<GTA::NaturalMotion::SetCharacterStrengthHelper^>("setCharacterStrength");
				}
			}

			property GTA::NaturalMotion::SetCharacterHealthHelper^ SetCharacterHealth {
				GTA::NaturalMotion::SetCharacterHealthHelper^ get() {
					return GetHelper<GTA::NaturalMotion::SetCharacterHealthHelper^>("setCharacterHealth");
				}
			}

			property GTA::NaturalMotion::SetFallingReactionHelper^ SetFallingReaction {
				GTA::NaturalMotion::SetFallingReactionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::SetFallingReactionHelper^>("setFallingReaction");
				}
			}

			property GTA::NaturalMotion::SetCharacterUnderwaterHelper^ SetCharacterUnderwater {
				GTA::NaturalMotion::SetCharacterUnderwaterHelper^ get() {
					return GetHelper<GTA::NaturalMotion::SetCharacterUnderwaterHelper^>("setCharacterUnderwater");
				}
			}

			property GTA::NaturalMotion::SetCharacterCollisionsHelper^ SetCharacterCollisions {
				GTA::NaturalMotion::SetCharacterCollisionsHelper^ get() {
					return GetHelper<GTA::NaturalMotion::SetCharacterCollisionsHelper^>("setCharacterCollisions");
				}
			}

			property GTA::NaturalMotion::SetCharacterDampingHelper^ SetCharacterDamping {
				GTA::NaturalMotion::SetCharacterDampingHelper^ get() {
					return GetHelper<GTA::NaturalMotion::SetCharacterDampingHelper^>("setCharacterDamping");
				}
			}

			property GTA::NaturalMotion::SetFrictionScaleHelper^ SetFrictionScale {
				GTA::NaturalMotion::SetFrictionScaleHelper^ get() {
					return GetHelper<GTA::NaturalMotion::SetFrictionScaleHelper^>("setFrictionScale");
				}
			}

			property GTA::NaturalMotion::AnimPoseHelper^ AnimPose {
				GTA::NaturalMotion::AnimPoseHelper^ get() {
					return GetHelper<GTA::NaturalMotion::AnimPoseHelper^>("animPose");
				}
			}

			property GTA::NaturalMotion::ArmsWindmillHelper^ ArmsWindmill {
				GTA::NaturalMotion::ArmsWindmillHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ArmsWindmillHelper^>("armsWindmill");
				}
			}

			property GTA::NaturalMotion::ArmsWindmillAdaptiveHelper^ ArmsWindmillAdaptive {
				GTA::NaturalMotion::ArmsWindmillAdaptiveHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ArmsWindmillAdaptiveHelper^>("armsWindmillAdaptive");
				}
			}

			property GTA::NaturalMotion::BalancerCollisionsReactionHelper^ BalancerCollisionsReaction {
				GTA::NaturalMotion::BalancerCollisionsReactionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::BalancerCollisionsReactionHelper^>("balancerCollisionsReaction");
				}
			}

			property GTA::NaturalMotion::BodyBalanceHelper^ BodyBalance {
				GTA::NaturalMotion::BodyBalanceHelper^ get() {
					return GetHelper<GTA::NaturalMotion::BodyBalanceHelper^>("bodyBalance");
				}
			}

			property GTA::NaturalMotion::BodyFoetalHelper^ BodyFoetal {
				GTA::NaturalMotion::BodyFoetalHelper^ get() {
					return GetHelper<GTA::NaturalMotion::BodyFoetalHelper^>("bodyFoetal");
				}
			}

			property GTA::NaturalMotion::BodyRollUpHelper^ BodyRollUp {
				GTA::NaturalMotion::BodyRollUpHelper^ get() {
					return GetHelper<GTA::NaturalMotion::BodyRollUpHelper^>("bodyRollUp");
				}
			}

			property GTA::NaturalMotion::BodyWritheHelper^ BodyWrithe {
				GTA::NaturalMotion::BodyWritheHelper^ get() {
					return GetHelper<GTA::NaturalMotion::BodyWritheHelper^>("bodyWrithe");
				}
			}

			property GTA::NaturalMotion::BraceForImpactHelper^ BraceForImpact {
				GTA::NaturalMotion::BraceForImpactHelper^ get() {
					return GetHelper<GTA::NaturalMotion::BraceForImpactHelper^>("braceForImpact");
				}
			}

			property GTA::NaturalMotion::BuoyancyHelper^ Buoyancy {
				GTA::NaturalMotion::BuoyancyHelper^ get() {
					return GetHelper<GTA::NaturalMotion::BuoyancyHelper^>("buoyancy");
				}
			}

			property GTA::NaturalMotion::CatchFallHelper^ CatchFall {
				GTA::NaturalMotion::CatchFallHelper^ get() {
					return GetHelper<GTA::NaturalMotion::CatchFallHelper^>("catchFall");
				}
			}

			property GTA::NaturalMotion::ElectrocuteHelper^ Electrocute {
				GTA::NaturalMotion::ElectrocuteHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ElectrocuteHelper^>("electrocute");
				}
			}

			property GTA::NaturalMotion::FallOverWallHelper^ FallOverWall {
				GTA::NaturalMotion::FallOverWallHelper^ get() {
					return GetHelper<GTA::NaturalMotion::FallOverWallHelper^>("fallOverWall");
				}
			}

			property GTA::NaturalMotion::GrabHelper^ Grab {
				GTA::NaturalMotion::GrabHelper^ get() {
					return GetHelper<GTA::NaturalMotion::GrabHelper^>("grab");
				}
			}

			property GTA::NaturalMotion::HeadLookHelper^ HeadLook {
				GTA::NaturalMotion::HeadLookHelper^ get() {
					return GetHelper<GTA::NaturalMotion::HeadLookHelper^>("headLook");
				}
			}

			property GTA::NaturalMotion::HighFallHelper^ HighFall {
				GTA::NaturalMotion::HighFallHelper^ get() {
					return GetHelper<GTA::NaturalMotion::HighFallHelper^>("highFall");
				}
			}

			property GTA::NaturalMotion::IncomingTransformsHelper^ IncomingTransforms {
				GTA::NaturalMotion::IncomingTransformsHelper^ get() {
					return GetHelper<GTA::NaturalMotion::IncomingTransformsHelper^>("incomingTransforms");
				}
			}

			property GTA::NaturalMotion::InjuredOnGroundHelper^ InjuredOnGround {
				GTA::NaturalMotion::InjuredOnGroundHelper^ get() {
					return GetHelper<GTA::NaturalMotion::InjuredOnGroundHelper^>("injuredOnGround");
				}
			}

			property GTA::NaturalMotion::CarriedHelper^ Carried {
				GTA::NaturalMotion::CarriedHelper^ get() {
					return GetHelper<GTA::NaturalMotion::CarriedHelper^>("carried");
				}
			}

			property GTA::NaturalMotion::DangleHelper^ Dangle {
				GTA::NaturalMotion::DangleHelper^ get() {
					return GetHelper<GTA::NaturalMotion::DangleHelper^>("dangle");
				}
			}

			property GTA::NaturalMotion::OnFireHelper^ OnFire {
				GTA::NaturalMotion::OnFireHelper^ get() {
					return GetHelper<GTA::NaturalMotion::OnFireHelper^>("onFire");
				}
			}

			property GTA::NaturalMotion::PedalLegsHelper^ PedalLegs {
				GTA::NaturalMotion::PedalLegsHelper^ get() {
					return GetHelper<GTA::NaturalMotion::PedalLegsHelper^>("pedalLegs");
				}
			}

			property GTA::NaturalMotion::PointArmHelper^ PointArm {
				GTA::NaturalMotion::PointArmHelper^ get() {
					return GetHelper<GTA::NaturalMotion::PointArmHelper^>("pointArm");
				}
			}

			property GTA::NaturalMotion::PointGunHelper^ PointGun {
				GTA::NaturalMotion::PointGunHelper^ get() {
					return GetHelper<GTA::NaturalMotion::PointGunHelper^>("pointGun");
				}
			}

			property GTA::NaturalMotion::PointGunExtraHelper^ PointGunExtra {
				GTA::NaturalMotion::PointGunExtraHelper^ get() {
					return GetHelper<GTA::NaturalMotion::PointGunExtraHelper^>("pointGunExtra");
				}
			}

			property GTA::NaturalMotion::RollDownStairsHelper^ RollDownStairs {
				GTA::NaturalMotion::RollDownStairsHelper^ get() {
					return GetHelper<GTA::NaturalMotion::RollDownStairsHelper^>("rollDownStairs");
				}
			}

			property GTA::NaturalMotion::ShotHelper^ Shot {
				GTA::NaturalMotion::ShotHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ShotHelper^>("shot");
				}
			}

			property GTA::NaturalMotion::ShotNewBulletHelper^ ShotNewBullet {
				GTA::NaturalMotion::ShotNewBulletHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ShotNewBulletHelper^>("shotNewBullet");
				}
			}

			property GTA::NaturalMotion::ShotSnapHelper^ ShotSnap {
				GTA::NaturalMotion::ShotSnapHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ShotSnapHelper^>("shotSnap");
				}
			}

			property GTA::NaturalMotion::ShotShockSpinHelper^ ShotShockSpin {
				GTA::NaturalMotion::ShotShockSpinHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ShotShockSpinHelper^>("shotShockSpin");
				}
			}

			property GTA::NaturalMotion::ShotFallToKneesHelper^ ShotFallToKnees {
				GTA::NaturalMotion::ShotFallToKneesHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ShotFallToKneesHelper^>("shotFallToKnees");
				}
			}

			property GTA::NaturalMotion::ShotFromBehindHelper^ ShotFromBehind {
				GTA::NaturalMotion::ShotFromBehindHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ShotFromBehindHelper^>("shotFromBehind");
				}
			}

			property GTA::NaturalMotion::ShotInGutsHelper^ ShotInGuts {
				GTA::NaturalMotion::ShotInGutsHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ShotInGutsHelper^>("shotInGuts");
				}
			}

			property GTA::NaturalMotion::ShotHeadLookHelper^ ShotHeadLook {
				GTA::NaturalMotion::ShotHeadLookHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ShotHeadLookHelper^>("shotHeadLook");
				}
			}

			property GTA::NaturalMotion::ShotConfigureArmsHelper^ ShotConfigureArms {
				GTA::NaturalMotion::ShotConfigureArmsHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ShotConfigureArmsHelper^>("shotConfigureArms");
				}
			}

			property GTA::NaturalMotion::SmartFallHelper^ SmartFall {
				GTA::NaturalMotion::SmartFallHelper^ get() {
					return GetHelper<GTA::NaturalMotion::SmartFallHelper^>("smartFall");
				}
			}

			property GTA::NaturalMotion::StaggerFallHelper^ StaggerFall {
				GTA::NaturalMotion::StaggerFallHelper^ get() {
					return GetHelper<GTA::NaturalMotion::StaggerFallHelper^>("staggerFall");
				}
			}

			property GTA::NaturalMotion::TeeterHelper^ Teeter {
				GTA::NaturalMotion::TeeterHelper^ get() {
					return GetHelper<GTA::NaturalMotion::TeeterHelper^>("teeter");
				}
			}

			property GTA::NaturalMotion::UpperBodyFlinchHelper^ UpperBodyFlinch {
				GTA::NaturalMotion::UpperBodyFlinchHelper^ get() {
					return GetHelper<GTA::NaturalMotion::UpperBodyFlinchHelper^>("upperBodyFlinch");
				}
			}

			property GTA::NaturalMotion::YankedHelper^ Yanked {
				GTA::NaturalMotion::YankedHelper^ get() {
					return GetHelper<GTA::NaturalMotion::YankedHelper^>("yanked");
				}
			}


		};
	}
}
