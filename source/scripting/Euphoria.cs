using System;
using System.Collections.Generic;
using GTA;

namespace GTA.NaturalMotion
{
	public sealed class Euphoria
	{
		#region Fields

		readonly Ped _ped;
		readonly Dictionary<string, CustomHelper> _helperCache;

		#endregion

		internal Euphoria(Ped ped)
		{
			_ped = ped;
			_helperCache = new Dictionary<string, CustomHelper>();
		}

		internal T GetHelper<T>(string message) where T : CustomHelper
		{
			CustomHelper h;

			if (!_helperCache.TryGetValue(message, out h))
			{
				h = (CustomHelper) Activator.CreateInstance(typeof(T), _ped);

				_helperCache.Add(message, h);
			}

			return (T) h;
		}

		public ActivePoseHelper ActivePose
		{
			get { return GetHelper<ActivePoseHelper>("activePose"); }
		}

		public ApplyImpulseHelper ApplyImpulse
		{
			get { return GetHelper<ApplyImpulseHelper>("applyImpulse"); }
		}

		public ApplyBulletImpulseHelper ApplyBulletImpulse
		{
			get { return GetHelper<ApplyBulletImpulseHelper>("applyBulletImpulse"); }
		}

		public BodyRelaxHelper BodyRelax
		{
			get { return GetHelper<BodyRelaxHelper>("bodyRelax"); }
		}

		public ConfigureBalanceHelper ConfigureBalance
		{
			get { return GetHelper<ConfigureBalanceHelper>("configureBalance"); }
		}

		public ConfigureBalanceResetHelper ConfigureBalanceReset
		{
			get { return GetHelper<ConfigureBalanceResetHelper>("configureBalanceReset"); }
		}

		public ConfigureSelfAvoidanceHelper ConfigureSelfAvoidance
		{
			get { return GetHelper<ConfigureSelfAvoidanceHelper>("configureSelfAvoidance"); }
		}

		public ConfigureBulletsHelper ConfigureBullets
		{
			get { return GetHelper<ConfigureBulletsHelper>("configureBullets"); }
		}

		public ConfigureBulletsExtraHelper ConfigureBulletsExtra
		{
			get { return GetHelper<ConfigureBulletsExtraHelper>("configureBulletsExtra"); }
		}

		public ConfigureLimitsHelper ConfigureLimits
		{
			get { return GetHelper<ConfigureLimitsHelper>("configureLimits"); }
		}

		public ConfigureSoftLimitHelper ConfigureSoftLimit
		{
			get { return GetHelper<ConfigureSoftLimitHelper>("configureSoftLimit"); }
		}

		public ConfigureShotInjuredArmHelper ConfigureShotInjuredArm
		{
			get { return GetHelper<ConfigureShotInjuredArmHelper>("configureShotInjuredArm"); }
		}

		public ConfigureShotInjuredLegHelper ConfigureShotInjuredLeg
		{
			get { return GetHelper<ConfigureShotInjuredLegHelper>("configureShotInjuredLeg"); }
		}

		public DefineAttachedObjectHelper DefineAttachedObject
		{
			get { return GetHelper<DefineAttachedObjectHelper>("defineAttachedObject"); }
		}

		public ForceToBodyPartHelper ForceToBodyPart
		{
			get { return GetHelper<ForceToBodyPartHelper>("forceToBodyPart"); }
		}

		public LeanInDirectionHelper LeanInDirection
		{
			get { return GetHelper<LeanInDirectionHelper>("leanInDirection"); }
		}

		public LeanRandomHelper LeanRandom
		{
			get { return GetHelper<LeanRandomHelper>("leanRandom"); }
		}

		public LeanToPositionHelper LeanToPosition
		{
			get { return GetHelper<LeanToPositionHelper>("leanToPosition"); }
		}

		public LeanTowardsObjectHelper LeanTowardsObject
		{
			get { return GetHelper<LeanTowardsObjectHelper>("leanTowardsObject"); }
		}

		public HipsLeanInDirectionHelper HipsLeanInDirection
		{
			get { return GetHelper<HipsLeanInDirectionHelper>("hipsLeanInDirection"); }
		}

		public HipsLeanRandomHelper HipsLeanRandom
		{
			get { return GetHelper<HipsLeanRandomHelper>("hipsLeanRandom"); }
		}

		public HipsLeanToPositionHelper HipsLeanToPosition
		{
			get { return GetHelper<HipsLeanToPositionHelper>("hipsLeanToPosition"); }
		}

		public HipsLeanTowardsObjectHelper HipsLeanTowardsObject
		{
			get { return GetHelper<HipsLeanTowardsObjectHelper>("hipsLeanTowardsObject"); }
		}

		public ForceLeanInDirectionHelper ForceLeanInDirection
		{
			get { return GetHelper<ForceLeanInDirectionHelper>("forceLeanInDirection"); }
		}

		public ForceLeanRandomHelper ForceLeanRandom
		{
			get { return GetHelper<ForceLeanRandomHelper>("forceLeanRandom"); }
		}

		public ForceLeanToPositionHelper ForceLeanToPosition
		{
			get { return GetHelper<ForceLeanToPositionHelper>("forceLeanToPosition"); }
		}

		public ForceLeanTowardsObjectHelper ForceLeanTowardsObject
		{
			get { return GetHelper<ForceLeanTowardsObjectHelper>("forceLeanTowardsObject"); }
		}

		public SetStiffnessHelper SetStiffness
		{
			get { return GetHelper<SetStiffnessHelper>("setStiffness"); }
		}

		public SetMuscleStiffnessHelper SetMuscleStiffness
		{
			get { return GetHelper<SetMuscleStiffnessHelper>("setMuscleStiffness"); }
		}

		public SetWeaponModeHelper SetWeaponMode
		{
			get { return GetHelper<SetWeaponModeHelper>("setWeaponMode"); }
		}

		public RegisterWeaponHelper RegisterWeapon
		{
			get { return GetHelper<RegisterWeaponHelper>("registerWeapon"); }
		}

		public ShotRelaxHelper ShotRelax
		{
			get { return GetHelper<ShotRelaxHelper>("shotRelax"); }
		}

		public FireWeaponHelper FireWeapon
		{
			get { return GetHelper<FireWeaponHelper>("fireWeapon"); }
		}

		public ConfigureConstraintsHelper ConfigureConstraints
		{
			get { return GetHelper<ConfigureConstraintsHelper>("configureConstraints"); }
		}

		public StayUprightHelper StayUpright
		{
			get { return GetHelper<StayUprightHelper>("stayUpright"); }
		}

		public StopAllBehavioursHelper StopAllBehaviours
		{
			get { return GetHelper<StopAllBehavioursHelper>("stopAllBehaviours"); }
		}

		public SetCharacterStrengthHelper SetCharacterStrength
		{
			get { return GetHelper<SetCharacterStrengthHelper>("setCharacterStrength"); }
		}

		public SetCharacterHealthHelper SetCharacterHealth
		{
			get { return GetHelper<SetCharacterHealthHelper>("setCharacterHealth"); }
		}

		public SetFallingReactionHelper SetFallingReaction
		{
			get { return GetHelper<SetFallingReactionHelper>("setFallingReaction"); }
		}

		public SetCharacterUnderwaterHelper SetCharacterUnderwater
		{
			get { return GetHelper<SetCharacterUnderwaterHelper>("setCharacterUnderwater"); }
		}

		public SetCharacterCollisionsHelper SetCharacterCollisions
		{
			get { return GetHelper<SetCharacterCollisionsHelper>("setCharacterCollisions"); }
		}

		public SetCharacterDampingHelper SetCharacterDamping
		{
			get { return GetHelper<SetCharacterDampingHelper>("setCharacterDamping"); }
		}

		public SetFrictionScaleHelper SetFrictionScale
		{
			get { return GetHelper<SetFrictionScaleHelper>("setFrictionScale"); }
		}

		public AnimPoseHelper AnimPose
		{
			get { return GetHelper<AnimPoseHelper>("animPose"); }
		}

		public ArmsWindmillHelper ArmsWindmill
		{
			get { return GetHelper<ArmsWindmillHelper>("armsWindmill"); }
		}

		public ArmsWindmillAdaptiveHelper ArmsWindmillAdaptive
		{
			get { return GetHelper<ArmsWindmillAdaptiveHelper>("armsWindmillAdaptive"); }
		}

		public BalancerCollisionsReactionHelper BalancerCollisionsReaction
		{
			get { return GetHelper<BalancerCollisionsReactionHelper>("balancerCollisionsReaction"); }
		}

		public BodyBalanceHelper BodyBalance
		{
			get { return GetHelper<BodyBalanceHelper>("bodyBalance"); }
		}

		public BodyFoetalHelper BodyFoetal
		{
			get { return GetHelper<BodyFoetalHelper>("bodyFoetal"); }
		}

		public BodyRollUpHelper BodyRollUp
		{
			get { return GetHelper<BodyRollUpHelper>("bodyRollUp"); }
		}

		public BodyWritheHelper BodyWrithe
		{
			get { return GetHelper<BodyWritheHelper>("bodyWrithe"); }
		}

		public BraceForImpactHelper BraceForImpact
		{
			get { return GetHelper<BraceForImpactHelper>("braceForImpact"); }
		}

		public BuoyancyHelper Buoyancy
		{
			get { return GetHelper<BuoyancyHelper>("buoyancy"); }
		}

		public CatchFallHelper CatchFall
		{
			get { return GetHelper<CatchFallHelper>("catchFall"); }
		}

		public ElectrocuteHelper Electrocute
		{
			get { return GetHelper<ElectrocuteHelper>("electrocute"); }
		}

		public FallOverWallHelper FallOverWall
		{
			get { return GetHelper<FallOverWallHelper>("fallOverWall"); }
		}

		public GrabHelper Grab
		{
			get { return GetHelper<GrabHelper>("grab"); }
		}

		public HeadLookHelper HeadLook
		{
			get { return GetHelper<HeadLookHelper>("headLook"); }
		}

		public HighFallHelper HighFall
		{
			get { return GetHelper<HighFallHelper>("highFall"); }
		}

		public IncomingTransformsHelper IncomingTransforms
		{
			get { return GetHelper<IncomingTransformsHelper>("incomingTransforms"); }
		}

		public InjuredOnGroundHelper InjuredOnGround
		{
			get { return GetHelper<InjuredOnGroundHelper>("injuredOnGround"); }
		}

		public CarriedHelper Carried
		{
			get { return GetHelper<CarriedHelper>("carried"); }
		}

		public DangleHelper Dangle
		{
			get { return GetHelper<DangleHelper>("dangle"); }
		}

		public OnFireHelper OnFire
		{
			get { return GetHelper<OnFireHelper>("onFire"); }
		}

		public PedalLegsHelper PedalLegs
		{
			get { return GetHelper<PedalLegsHelper>("pedalLegs"); }
		}

		public PointArmHelper PointArm
		{
			get { return GetHelper<PointArmHelper>("pointArm"); }
		}

		public PointGunHelper PointGun
		{
			get { return GetHelper<PointGunHelper>("pointGun"); }
		}

		public PointGunExtraHelper PointGunExtra
		{
			get { return GetHelper<PointGunExtraHelper>("pointGunExtra"); }
		}

		public RollDownStairsHelper RollDownStairs
		{
			get { return GetHelper<RollDownStairsHelper>("rollDownStairs"); }
		}

		public ShotHelper Shot
		{
			get { return GetHelper<ShotHelper>("shot"); }
		}

		public ShotNewBulletHelper ShotNewBullet
		{
			get { return GetHelper<ShotNewBulletHelper>("shotNewBullet"); }
		}

		public ShotSnapHelper ShotSnap
		{
			get { return GetHelper<ShotSnapHelper>("shotSnap"); }
		}

		public ShotShockSpinHelper ShotShockSpin
		{
			get { return GetHelper<ShotShockSpinHelper>("shotShockSpin"); }
		}

		public ShotFallToKneesHelper ShotFallToKnees
		{
			get { return GetHelper<ShotFallToKneesHelper>("shotFallToKnees"); }
		}

		public ShotFromBehindHelper ShotFromBehind
		{
			get { return GetHelper<ShotFromBehindHelper>("shotFromBehind"); }
		}

		public ShotInGutsHelper ShotInGuts
		{
			get { return GetHelper<ShotInGutsHelper>("shotInGuts"); }
		}

		public ShotHeadLookHelper ShotHeadLook
		{
			get { return GetHelper<ShotHeadLookHelper>("shotHeadLook"); }
		}

		public ShotConfigureArmsHelper ShotConfigureArms
		{
			get { return GetHelper<ShotConfigureArmsHelper>("shotConfigureArms"); }
		}

		public SmartFallHelper SmartFall
		{
			get { return GetHelper<SmartFallHelper>("smartFall"); }
		}

		public StaggerFallHelper StaggerFall
		{
			get { return GetHelper<StaggerFallHelper>("staggerFall"); }
		}

		public TeeterHelper Teeter
		{
			get { return GetHelper<TeeterHelper>("teeter"); }
		}

		public UpperBodyFlinchHelper UpperBodyFlinch
		{
			get { return GetHelper<UpperBodyFlinchHelper>("upperBodyFlinch"); }
		}

		public YankedHelper Yanked
		{
			get { return GetHelper<YankedHelper>("yanked"); }
		}
	}
}
