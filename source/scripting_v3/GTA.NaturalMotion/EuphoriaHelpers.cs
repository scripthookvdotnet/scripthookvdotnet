//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;

namespace GTA.NaturalMotion
{
	public enum ArmDirection
	{
		Backwards = -1,
		Adaptive,
		Forwards
	}

	public enum AnimSource
	{
		CurrentItems,
		PreviousItems,
		AnimItems
	}

	public enum FallType
	{
		RampDownStiffness,
		DontChangeStep,
		ForceBalance,
		Slump
	}

	public enum Synchroisation
	{
		NotSynced,
		AlwaysSynced,
		SyncedAtStart
	}

	public enum TurnType
	{
		DontTurn,
		ToTarget,
		AwayFromTarget
	}

	public enum TorqueMode
	{
		Disabled,
		Proportional,
		Additive
	}

	public enum TorqueSpinMode
	{
		FromImpulse,
		Random,
		Flipping
	}

	public enum TorqueFilterMode
	{
		ApplyEveryBullet,
		ApplyIfLastFinished,
		ApplyIfSpinDifferent
	}

	public enum RbTwistAxis
	{
		WorldUp,
		CharacterComUp
	}

	public enum WeaponMode
	{
		None = -1,
		Pistol,
		Dual,
		Rifle,
		SideArm,
		PistolLeft,
		PistolRight
	}

	public enum Hand
	{
		Left,
		Right
	}

	public enum MirrorMode
	{
		Independant,
		Mirrored,
		Parallel
	}

	public enum AdaptiveMode
	{
		NotAdaptive,
		OnlyDirection,
		DirectionAndSpeed,
		DirectionSpeedAndStrength
	}

	public sealed class ActivePoseHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ActivePoseHelper for sending a ActivePose <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ActivePose <see cref="Message"/> to.</param>
		public ActivePoseHelper(Ped ped) : base(ped, "activePose")
		{
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see notes for explanation).
		/// </summary>
		/// <remarks>
		/// Default value = "fb".
		/// </remarks>
		public string Mask
		{
			set
			{
				SetArgument("mask", value);
			}
		}

		/// <summary>
		/// Apply gravity compensation.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseGravityCompensation
		{
			set
			{
				SetArgument("useGravityCompensation", value);
			}
		}

		/// <summary>
		/// Animation source.
		/// </summary>
		public AnimSource AnimSource
		{
			set
			{
				SetArgument("animSource", (int)value);
			}
		}
	}

	public sealed class ApplyImpulseHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ApplyImpulseHelper for sending a ApplyImpulse <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ApplyImpulse <see cref="Message"/> to.</param>
		public ApplyImpulseHelper(Ped ped) : base(ped, "applyImpulse")
		{
		}

		/// <summary>
		/// 0 means straight impulse, 1 means multiply by the mass (change in velocity).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float EqualizeAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("equalizeAmount", value);
			}
		}

		/// <summary>
		/// Index of part being hit. -1 apply impulse to COM.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = -1.
		/// Max value = 28.
		/// </remarks>
		public int PartIndex
		{
			set
			{
				if (value > 28)
				{
					value = 28;
				}

				if (value < -1)
				{
					value = -1;
				}

				SetArgument("partIndex", value);
			}
		}

		/// <summary>
		/// Impulse vector (impulse is change in momentum).
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = -4500.0f.
		/// Max value = 4500.0f.
		/// </remarks>
		public Vector3 Impulse
		{
			set
			{
				SetArgument("impulse", Vector3.Clamp(value, new Vector3(-4500.0f, -4500.0f, -4500.0f), new Vector3(4500.0f, 4500.0f, 4500.0f)));
			}
		}

		/// <summary>
		/// Optional point on part where hit. If not supplied then the impulse is applied at the part center.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 HitPoint
		{
			set
			{
				SetArgument("hitPoint", value);
			}
		}

		/// <summary>
		/// Hit point in local coordinates of body part.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool LocalHitPointInfo
		{
			set
			{
				SetArgument("localHitPointInfo", value);
			}
		}

		/// <summary>
		/// Impulse in local coordinates of body part.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool LocalImpulseInfo
		{
			set
			{
				SetArgument("localImpulseInfo", value);
			}
		}

		/// <summary>
		/// Impulse should be considered an angular impulse.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AngularImpulse
		{
			set
			{
				SetArgument("angularImpulse", value);
			}
		}
	}

	public sealed class ApplyBulletImpulseHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ApplyBulletImpulseHelper for sending a ApplyBulletImpulse <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ApplyBulletImpulse <see cref="Message"/> to.</param>
		public ApplyBulletImpulseHelper(Ped ped) : base(ped, "applyBulletImpulse")
		{
		}

		/// <summary>
		/// 0 means straight impulse, 1 means multiply by the mass (change in velocity).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float EqualizeAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("equalizeAmount", value);
			}
		}

		/// <summary>
		/// Index of part being hit.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 28.
		/// </remarks>
		public int PartIndex
		{
			set
			{
				if (value > 28)
				{
					value = 28;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("partIndex", value);
			}
		}

		/// <summary>
		/// Impulse vector (impulse is change in momentum).
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = -1000.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public Vector3 Impulse
		{
			set
			{
				SetArgument("impulse", Vector3.Clamp(value, new Vector3(-1000.0f, -1000.0f, -1000.0f), new Vector3(1000.0f, 1000.0f, 1000.0f)));
			}
		}

		/// <summary>
		/// Optional point on part where hit.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 HitPoint
		{
			set
			{
				SetArgument("hitPoint", value);
			}
		}

		/// <summary>
		/// True = hitPoint is in local coordinates of bodyPart, false = hit point is in world coordinates.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool LocalHitPointInfo
		{
			set
			{
				SetArgument("localHitPointInfo", value);
			}
		}

		/// <summary>
		/// If not 0.0 then have an extra bullet applied to spine0 (approximates the COM).  Uses setup from configureBulletsExtra.  0-1 shared 0.0 = no extra bullet, 0.5 = impulse split equally between extra and bullet,  1.0 only extra bullet.  LT 0.0 then bullet + scaled extra bullet.  Eg.-0.5 = bullet + 0.5 impulse extra bullet.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -2.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ExtraShare
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -2.0f)
				{
					value = -2.0f;
				}

				SetArgument("extraShare", value);
			}
		}
	}

	/// <summary>
	/// Set the amount of relaxation across the whole body; Used to collapse the character into a rag-doll-like state.
	/// </summary>
	public sealed class BodyRelaxHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the BodyRelaxHelper for sending a BodyRelax <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the BodyRelax <see cref="Message"/> to.</param>
		/// <remarks>
		/// Set the amount of relaxation across the whole body; Used to collapse the character into a rag-doll-like state.
		/// </remarks>
		public BodyRelaxHelper(Ped ped) : base(ped, "bodyRelax")
		{
		}

		/// <summary>
		/// How relaxed the body becomes, in percentage relaxed. 100 being totally rag-dolled, 0 being very stiff and rigid.
		/// </summary>
		/// <remarks>
		/// Default value = 50.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float Relaxation
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("relaxation", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float Damping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("damping", value);
			}
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see Active Pose notes for possible values).
		/// </summary>
		/// <remarks>
		/// Default value = "fb.
		/// </remarks>
		public string Mask
		{
			set
			{
				SetArgument("mask", value);
			}
		}

		/// <summary>
		/// Automatically hold the current pose as the character relaxes - can be used to avoid relaxing into a t-pose.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool HoldPose
		{
			set
			{
				SetArgument("holdPose", value);
			}
		}

		/// <summary>
		/// Sets the drive state to free - this reduces drifting on the ground.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool DisableJointDriving
		{
			set
			{
				SetArgument("disableJointDriving", value);
			}
		}
	}

	/// <summary>
	/// This single message allows you to configure various parameters used on any behavior that uses the dynamic balance.
	/// </summary>
	public sealed class ConfigureBalanceHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ConfigureBalanceHelper for sending a ConfigureBalance <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ConfigureBalance <see cref="Message"/> to.</param>
		/// <remarks>
		/// This single message allows you to configure various parameters used on any behavior that uses the dynamic balance.
		/// </remarks>
		public ConfigureBalanceHelper(Ped ped) : base(ped, "configureBalance")
		{
		}

		/// <summary>
		/// Maximum height that character steps vertically (above 0.2 is high ... But OK underwater).
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 0.4f.
		/// </remarks>
		public float StepHeight
		{
			set
			{
				if (value > 0.4f)
				{
					value = 0.4f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stepHeight", value);
			}
		}

		/// <summary>
		/// Added to stepHeight if going up steps.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 0.4f.
		/// </remarks>
		public float StepHeightInc4Step
		{
			set
			{
				if (value > 0.4f)
				{
					value = 0.4f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stepHeightInc4Step", value);
			}
		}

		/// <summary>
		/// If the legs end up more than (legsApartRestep + hipwidth) apart even though balanced, take another step.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LegsApartRestep
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legsApartRestep", value);
			}
		}

		/// <summary>
		/// Mmmm0.1 for drunk if the legs end up less than (hipwidth - legsTogetherRestep) apart even though balanced, take another step.  A value of 1 will turn off this feature and the max value is hipWidth = 0.23f by default but is model dependent.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LegsTogetherRestep
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legsTogetherRestep", value);
			}
		}

		/// <summary>
		/// FRICTION WORKAROUND: if the legs end up more than (legsApartMax + hipwidth) apart when balanced, adjust the feet positions to slide back so they are legsApartMax + hipwidth apart.  Needs to be less than legsApartRestep to see any effect.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float LegsApartMax
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legsApartMax", value);
			}
		}

		/// <summary>
		/// Does the knee strength reduce with angle.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool TaperKneeStrength
		{
			set
			{
				SetArgument("taperKneeStrength", value);
			}
		}

		/// <summary>
		/// Stiffness of legs.
		/// </summary>
		/// <remarks>
		/// Default value = 12.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float LegStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("legStiffness", value);
			}
		}

		/// <summary>
		/// Damping of left leg during swing phase (mmmmDrunk used 1.25 to slow legs movement).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.2f.
		/// Max value = 4.0f.
		/// </remarks>
		public float LeftLegSwingDamping
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.2f)
				{
					value = 0.2f;
				}

				SetArgument("leftLegSwingDamping", value);
			}
		}

		/// <summary>
		/// Damping of right leg during swing phase (mmmmDrunk used 1.25 to slow legs movement).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.2f.
		/// Max value = 4.0f.
		/// </remarks>
		public float RightLegSwingDamping
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.2f)
				{
					value = 0.2f;
				}

				SetArgument("rightLegSwingDamping", value);
			}
		}

		/// <summary>
		/// Gravity opposition applied to hips and knees.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float OpposeGravityLegs
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("opposeGravityLegs", value);
			}
		}

		/// <summary>
		/// Gravity opposition applied to ankles.  General balancer likes 1.0.  StaggerFall likes 0.1.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float OpposeGravityAnkles
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("opposeGravityAnkles", value);
			}
		}

		/// <summary>
		/// Multiplier on the floorAcceleration added to the lean.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanAcc
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanAcc", value);
			}
		}

		/// <summary>
		/// Multiplier on the floorAcceleration added to the leanHips.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float HipLeanAcc
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("hipLeanAcc", value);
			}
		}

		/// <summary>
		/// Max floorAcceleration allowed for lean and leanHips.
		/// </summary>
		/// <remarks>
		/// Default value = 5.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float LeanAccMax
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanAccMax", value);
			}
		}

		/// <summary>
		/// Level of cheat force added to character to resist the effect of floorAcceleration (anti-Acceleration) - added to upperbody.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ResistAcc
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("resistAcc", value);
			}
		}

		/// <summary>
		/// Max floorAcceleration allowed for anti-Acceleration. If  GT 20.0 then it is probably in a crash.
		/// </summary>
		/// <remarks>
		/// Default value = 3.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float ResistAccMax
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("resistAccMax", value);
			}
		}

		/// <summary>
		/// This parameter will be removed when footSlipCompensation preserves the foot angle on a moving floor]. If the character detects a moving floor and footSlipCompOnMovingFloor is false then it will turn off footSlipCompensation - at footSlipCompensation preserves the global heading of the feet.  If footSlipCompensation is off then the character usually turns to the side in the end although when turning the vehicle turns it looks promising for a while.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool FootSlipCompOnMovingFloor
		{
			set
			{
				SetArgument("footSlipCompOnMovingFloor", value);
			}
		}

		/// <summary>
		/// Ankle equilibrium angle used when static balancing.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float AnkleEquilibrium
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("ankleEquilibrium", value);
			}
		}

		/// <summary>
		/// Additional feet apart setting.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ExtraFeetApart
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("extraFeetApart", value);
			}
		}

		/// <summary>
		/// Amount of time at the start of a balance before the character is allowed to start stepping.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float DontStepTime
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dontStepTime", value);
			}
		}

		/// <summary>
		/// When the character gives up and goes into a fall.  Larger values mean that the balancer can lean more before failing.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float BalanceAbortThreshold
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("balanceAbortThreshold", value);
			}
		}

		/// <summary>
		/// Height between lowest foot and COM below which balancer will give up.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.5f.
		/// </remarks>
		public float GiveUpHeight
		{
			set
			{
				if (value > 1.5f)
				{
					value = 1.5f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("giveUpHeight", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float StepClampScale
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stepClampScale", value);
			}
		}

		/// <summary>
		/// Variance in clamp scale every step. If negative only takes away from clampScale.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float StepClampScaleVariance
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("stepClampScaleVariance", value);
			}
		}

		/// <summary>
		/// Amount of time (seconds) into the future that the character tries to move hip to (kind of).  Will be controlled by balancer in future but can help recover spine quicker from bending forwards to much.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float PredictionTimeHip
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("predictionTimeHip", value);
			}
		}

		/// <summary>
		/// Amount of time (seconds) into the future that the character tries to step to. Bigger values try to recover with fewer, bigger steps. Smaller values recover with smaller steps, and generally recover less.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float PredictionTime
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("predictionTime", value);
			}
		}

		/// <summary>
		/// Variance in predictionTime every step. If negative only takes away from predictionTime.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float PredictionTimeVariance
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("predictionTimeVariance", value);
			}
		}

		/// <summary>
		/// Maximum number of steps that the balancer will take.
		/// </summary>
		/// <remarks>
		/// Default value = 100.
		/// Min value = 1.
		/// </remarks>
		public int MaxSteps
		{
			set
			{
				if (value < 1)
				{
					value = 1;
				}

				SetArgument("maxSteps", value);
			}
		}

		/// <summary>
		/// Maximum time(seconds) that the balancer will balance for.
		/// </summary>
		/// <remarks>
		/// Default value = 50.0f.
		/// Min value = 1.0f.
		/// </remarks>
		public float MaxBalanceTime
		{
			set
			{
				if (value < 1.0f)
				{
					value = 1.0f;
				}

				SetArgument("maxBalanceTime", value);
			}
		}

		/// <summary>
		/// Allow the balancer to take this many more steps before hitting maxSteps. If negative nothing happens(safe default).
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int ExtraSteps
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("extraSteps", value);
			}
		}

		/// <summary>
		/// Allow the balancer to balance for this many more seconds before hitting maxBalanceTime.  If negative nothing happens(safe default).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// </remarks>
		public float ExtraTime
		{
			set
			{
				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("extraTime", value);
			}
		}

		/// <summary>
		/// How to fall after maxSteps or maxBalanceTime.
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="FallType.RampDownStiffness"/>.
		/// If <see cref="FallType.Slump"/> BCR has to be active.
		/// </remarks>
		public FallType FallType
		{
			set
			{
				SetArgument("fallType", (int)value);
			}
		}

		/// <summary>
		/// Multiply the rampDown of stiffness on falling by this amount ( GT 1 fall quicker).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float FallMult
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("fallMult", value);
			}
		}

		/// <summary>
		/// Reduce gravity compensation as the legs weaken on falling.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FallReduceGravityComp
		{
			set
			{
				SetArgument("fallReduceGravityComp", value);
			}
		}

		/// <summary>
		/// Bend over when falling after maxBalanceTime.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool RampHipPitchOnFail
		{
			set
			{
				SetArgument("rampHipPitchOnFail", value);
			}
		}

		/// <summary>
		/// Linear speed threshold for successful balance.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float StableLinSpeedThresh
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stableLinSpeedThresh", value);
			}
		}

		/// <summary>
		/// Rotational speed threshold for successful balance.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float StableRotSpeedThresh
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stableRotSpeedThresh", value);
			}
		}

		/// <summary>
		/// The upper body of the character must be colliding and other failure conditions met to fail.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FailMustCollide
		{
			set
			{
				SetArgument("failMustCollide", value);
			}
		}

		/// <summary>
		/// Ignore maxSteps and maxBalanceTime and try to balance forever.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool IgnoreFailure
		{
			set
			{
				SetArgument("ignoreFailure", value);
			}
		}

		/// <summary>
		/// Time not in contact (airborne) before step is changed. If -ve don't change step.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float ChangeStepTime
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("changeStepTime", value);
			}
		}

		/// <summary>
		/// Ignore maxSteps and maxBalanceTime and try to balance forever.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool BalanceIndefinitely
		{
			set
			{
				SetArgument("balanceIndefinitely", value);
			}
		}

		/// <summary>
		/// Temporary variable to ignore movingFloor code that generally causes the character to fall over if the feet probe a moving object e.g. treading on a gun.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool MovingFloor
		{
			set
			{
				SetArgument("movingFloor", value);
			}
		}

		/// <summary>
		/// When airborne try to step.  Set to false for e.g. shotGun reaction.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool AirborneStep
		{
			set
			{
				SetArgument("airborneStep", value);
			}
		}

		/// <summary>
		/// Velocity below which the balancer turns in the direction of the COM forward instead of the ComVel - for use with shot from running with high upright constraint use 1.9.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float UseComDirTurnVelThresh
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("useComDirTurnVelThresh", value);
			}
		}

		/// <summary>
		/// Minimum knee angle (-ve value will mean this functionality is not applied).  0.4 seems a good value.
		/// </summary>
		/// <remarks>
		/// Default value = -0.5f.
		/// Min value = -0.5f.
		/// Max value = 1.5f.
		/// </remarks>
		public float MinKneeAngle
		{
			set
			{
				if (value > 1.5f)
				{
					value = 1.5f;
				}

				if (value < -0.5f)
				{
					value = -0.5f;
				}

				SetArgument("minKneeAngle", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FlatterSwingFeet
		{
			set
			{
				SetArgument("flatterSwingFeet", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FlatterStaticFeet
		{
			set
			{
				SetArgument("flatterStaticFeet", value);
			}
		}

		/// <summary>
		/// If true then balancer tries to avoid leg2leg collisions/avoid crossing legs. Avoid tries to not step across a line of the inside of the stance leg's foot.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AvoidLeg
		{
			set
			{
				SetArgument("avoidLeg", value);
			}
		}

		/// <summary>
		/// NB. Very sensitive. Avoid tries to not step across a line of the inside of the stance leg's foot. AvoidFootWidth = how much inwards from the ankle this line is in (m).
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float AvoidFootWidth
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("avoidFootWidth", value);
			}
		}

		/// <summary>
		/// NB. Very sensitive. Avoid tries to not step across a line of the inside of the stance leg's foot. Avoid doesn't allow the desired stepping foot to cross the line.  avoidFeedback = how much of the actual crossing of that line is fedback as an error.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float AvoidFeedback
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("avoidFeedback", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanAgainstVelocity
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanAgainstVelocity", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float StepDecisionThreshold
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stepDecisionThreshold", value);
			}
		}

		/// <summary>
		/// The balancer sometimes decides to step even if balanced.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool StepIfInSupport
		{
			set
			{
				SetArgument("stepIfInSupport", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AlwaysStepWithFarthest
		{
			set
			{
				SetArgument("alwaysStepWithFarthest", value);
			}
		}

		/// <summary>
		/// Standup more with increased velocity.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool StandUp
		{
			set
			{
				SetArgument("standUp", value);
			}
		}

		/// <summary>
		/// Supposed to increase foot friction: Impact depth of a collision with the foot is changed when the balancer is running - impact.SetDepth(impact.GetDepth() - depthFudge).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float DepthFudge
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("depthFudge", value);
			}
		}

		/// <summary>
		/// Supposed to increase foot friction: Impact depth of a collision with the foot is changed when staggerFall is running - impact.SetDepth(impact.GetDepth() - depthFudgeStagger).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float DepthFudgeStagger
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("depthFudgeStagger", value);
			}
		}

		/// <summary>
		/// Foot friction multiplier is multiplied by this amount if balancer is running.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 40.0f.
		/// </remarks>
		public float FootFriction
		{
			set
			{
				if (value > 40.0f)
				{
					value = 40.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("footFriction", value);
			}
		}

		/// <summary>
		/// Foot friction multiplier is multiplied by this amount if staggerFall is running.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 40.0f.
		/// </remarks>
		public float FootFrictionStagger
		{
			set
			{
				if (value > 40.0f)
				{
					value = 40.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("footFrictionStagger", value);
			}
		}

		/// <summary>
		/// Backwards lean threshold to cut off stay upright forces. 0.0 Vertical - 1.0 horizontal.  0.6 is a sensible value.  NB: the balancer does not fail in order to give stagger that extra step as it falls.  A backwards lean of GT 0.6 will generally mean the balancer will soon fail without stayUpright forces.
		/// </summary>
		/// <remarks>
		/// Default value = 1.1f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float BackwardsLeanCutoff
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("backwardsLeanCutoff", value);
			}
		}

		/// <summary>
		/// If this value is different from giveUpHeight, actual giveUpHeight will be ramped toward this value.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.5f.
		/// </remarks>
		public float GiveUpHeightEnd
		{
			set
			{
				if (value > 1.5f)
				{
					value = 1.5f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("giveUpHeightEnd", value);
			}
		}

		/// <summary>
		/// If this value is different from balanceAbortThreshold, actual balanceAbortThreshold will be ramped toward this value.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float BalanceAbortThresholdEnd
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("balanceAbortThresholdEnd", value);
			}
		}

		/// <summary>
		/// Duration of ramp from start of behavior for above two parameters. If smaller than 0, no ramp is applied.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float GiveUpRampDuration
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("giveUpRampDuration", value);
			}
		}

		/// <summary>
		/// Lean at which to send abort message when maxSteps or maxBalanceTime is reached.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanToAbort
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanToAbort", value);
			}
		}
	}

	/// <summary>
	/// Reset the values configurable by the Configure Balance message to their defaults.
	/// </summary>
	public sealed class ConfigureBalanceResetHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ConfigureBalanceResetHelper for sending a ConfigureBalanceReset <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ConfigureBalanceReset <see cref="Message"/> to.</param>
		/// <remarks>
		/// Reset the values configurable by the Configure Balance message to their defaults.
		/// </remarks>
		public ConfigureBalanceResetHelper(Ped ped) : base(ped, "configureBalanceReset")
		{
		}
	}

	/// <summary>
	/// This single message allows to configure self avoidance for the character.BBDD Self avoidance tech.
	/// </summary>
	public sealed class ConfigureSelfAvoidanceHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ConfigureSelfAvoidanceHelper for sending a ConfigureSelfAvoidance <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ConfigureSelfAvoidance <see cref="Message"/> to.</param>
		/// <remarks>
		/// This single message allows to configure self avoidance for the character.BBDD Self avoidance tech.
		/// </remarks>
		public ConfigureSelfAvoidanceHelper(Ped ped) : base(ped, "configureSelfAvoidance")
		{
		}

		/// <summary>
		/// Enable or disable self avoidance tech.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseSelfAvoidance
		{
			set
			{
				SetArgument("useSelfAvoidance", value);
			}
		}

		/// <summary>
		/// Specify whether self avoidance tech should use original IK input target or the target that has been already modified by getStabilisedPos() tech i.e. function that compensates for rotational and linear velocity of shoulder/thigh.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool OverwriteDragReduction
		{
			set
			{
				SetArgument("overwriteDragReduction", value);
			}
		}

		/// <summary>
		/// Place the adjusted target this much along the arc between effector (wrist) and target, value in range [0,1].
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TorsoSwingFraction
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torsoSwingFraction", value);
			}
		}

		/// <summary>
		/// Max value on the effector (wrist) to adjusted target offset.
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 1.6f.
		/// </remarks>
		public float MaxTorsoSwingAngleRad
		{
			set
			{
				if (value > 1.6f)
				{
					value = 1.6f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxTorsoSwingAngleRad", value);
			}
		}

		/// <summary>
		/// Restrict self avoidance to operate on targets that are within character torso bounds only.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SelfAvoidIfInSpineBoundsOnly
		{
			set
			{
				SetArgument("selfAvoidIfInSpineBoundsOnly", value);
			}
		}

		/// <summary>
		/// Amount of self avoidance offset applied when angle from effector (wrist) to target is greater then right angle i.e. when total offset is a blend between where effector currently is to value that is a product of total arm length and selfAvoidAmount. SelfAvoidAmount is in a range between [0, 1].
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SelfAvoidAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("selfAvoidAmount", value);
			}
		}

		/// <summary>
		/// Overwrite desired IK twist with self avoidance procedural twist.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool OverwriteTwist
		{
			set
			{
				SetArgument("overwriteTwist", value);
			}
		}

		/// <summary>
		/// Use the alternative self avoidance algorithm that is based on linear and polar target blending. WARNING: It only requires "radius" in terms of parametrization.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UsePolarPathAlgorithm
		{
			set
			{
				SetArgument("usePolarPathAlgorithm", value);
			}
		}

		/// <summary>
		/// Self avoidance radius, measured out from the spine axis along the plane perpendicular to that axis. The closer is the proximity of reaching target to that radius, the more polar (curved) motion is used for offsetting the target. WARNING: Parameter only used by the alternative algorithm that is based on linear and polar target blending.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Radius
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("radius", value);
			}
		}
	}

	public sealed class ConfigureBulletsHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ConfigureBulletsHelper for sending a ConfigureBullets <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ConfigureBullets <see cref="Message"/> to.</param>
		public ConfigureBulletsHelper(Ped ped) : base(ped, "configureBullets")
		{
		}

		/// <summary>
		/// Spreads impulse across parts. Currently only for spine parts, not limbs.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ImpulseSpreadOverParts
		{
			set
			{
				SetArgument("impulseSpreadOverParts", value);
			}
		}

		/// <summary>
		/// For weaker characters subsequent impulses remain strong.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ImpulseLeakageStrengthScaled
		{
			set
			{
				SetArgument("impulseLeakageStrengthScaled", value);
			}
		}

		/// <summary>
		/// Duration that impulse is spread over (triangular shaped).
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulsePeriod
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulsePeriod", value);
			}
		}

		/// <summary>
		/// An impulse applied at a point on a body equivalent to an impulse at the center of the body and a torque.  This parameter scales the torque component. (The torque component seems to be excite the rage looseness bug which sends the character in a sometimes wildly different direction to an applied impulse).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseTorqueScale
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseTorqueScale", value);
			}
		}

		/// <summary>
		/// Fix the rage looseness bug by applying only the impulse at the center of the body unless it is a spine part then apply the twist component only of the torque as well.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool LoosenessFix
		{
			set
			{
				SetArgument("loosenessFix", value);
			}
		}

		/// <summary>
		/// Time from hit before impulses are being applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseDelay
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseDelay", value);
			}
		}

		/// <summary>
		/// By how much are subsequent impulses reduced (e.g. 0.0: no reduction, 0.1: 10% reduction each new hit).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseReductionPerShot
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseReductionPerShot", value);
			}
		}

		/// <summary>
		/// Recovery rate of impulse strength per second (impulse strength from 0.0:1.0).  At 60fps a impulseRecovery=60.0 will recover in 1 frame.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 60.0f.
		/// </remarks>
		public float ImpulseRecovery
		{
			set
			{
				if (value > 60.0f)
				{
					value = 60.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseRecovery", value);
			}
		}

		/// <summary>
		/// The minimum amount of impulse leakage allowed.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseMinLeakage
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseMinLeakage", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="TorqueMode.Disabled"/>.
		/// If <see cref="TorqueMode.Proportional"/> - proportional to character strength, can reduce impulse amount.
		/// If <see cref="TorqueMode.Additive"/> - no reduction of impulse and not proportional to character strength.
		/// </remarks>
		public TorqueMode TorqueMode
		{
			set
			{
				SetArgument("torqueMode", (int)value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="TorqueSpinMode.FromImpulse"/>.
		/// If <see cref="TorqueSpinMode.Flipping"/> a burst effect is achieved.
		/// </remarks>
		public TorqueSpinMode TorqueSpinMode
		{
			set
			{
				SetArgument("torqueSpinMode", (int)value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="TorqueFilterMode.ApplyEveryBullet"/>.
		/// </remarks>
		public TorqueFilterMode TorqueFilterMode
		{
			set
			{
				SetArgument("torqueFilterMode", (int)value);
			}
		}

		/// <summary>
		/// Always apply torques to spine3 instead of actual part hit.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool TorqueAlwaysSpine3
		{
			set
			{
				SetArgument("torqueAlwaysSpine3", value);
			}
		}

		/// <summary>
		/// Time from hit before torques are being applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TorqueDelay
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torqueDelay", value);
			}
		}

		/// <summary>
		/// Duration of torque.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TorquePeriod
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torquePeriod", value);
			}
		}

		/// <summary>
		/// Multiplies impulse magnitude to arrive at torque that is applied.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float TorqueGain
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torqueGain", value);
			}
		}

		/// <summary>
		/// Minimum ratio of impulse that remains after converting to torque (if in strength-proportional mode).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TorqueCutoff
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torqueCutoff", value);
			}
		}

		/// <summary>
		/// Ratio of torque for next tick (e.g. 1.0: not reducing over time, 0.9: each tick torque is reduced by 10%).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TorqueReductionPerTick
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torqueReductionPerTick", value);
			}
		}

		/// <summary>
		/// Amount of lift (directly multiplies torque axis to give lift force).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LiftGain
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("liftGain", value);
			}
		}

		/// <summary>
		/// Time after impulse is applied that counter impulse is applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CounterImpulseDelay
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("counterImpulseDelay", value);
			}
		}

		/// <summary>
		/// Amount of the original impulse that is countered.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CounterImpulseMag
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("counterImpulseMag", value);
			}
		}

		/// <summary>
		/// Applies the counter impulse counterImpulseDelay(secs) after counterImpulseMag of the Impulse has been applied.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool CounterAfterMagReached
		{
			set
			{
				SetArgument("counterAfterMagReached", value);
			}
		}

		/// <summary>
		/// Add a counter impulse to the pelvis.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool DoCounterImpulse
		{
			set
			{
				SetArgument("doCounterImpulse", value);
			}
		}

		/// <summary>
		/// Amount of the counter impulse applied to hips - the rest is applied to the part originally hit.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CounterImpulse2Hips
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("counterImpulse2Hips", value);
			}
		}

		/// <summary>
		/// Amount to scale impulse by if the dynamicBalance is not OK.  1.0 means this functionality is not applied.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseNoBalMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseNoBalMult", value);
			}
		}

		/// <summary>
		/// 100% LE Start to impulseBalStabMult*100% GT End. NB: Start LT End.
		/// </summary>
		/// <remarks>
		/// Default value = 3.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float ImpulseBalStabStart
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseBalStabStart", value);
			}
		}

		/// <summary>
		/// 100% LE Start to impulseBalStabMult*100% GT End. NB: Start LT End.
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float ImpulseBalStabEnd
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseBalStabEnd", value);
			}
		}

		/// <summary>
		/// 100% LE Start to impulseBalStabMult*100% GT End. NB: leaving this as 1.0 means this functionality is not applied and Start and End have no effect.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseBalStabMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseBalStabMult", value);
			}
		}

		/// <summary>
		/// 100% GE Start to impulseSpineAngMult*100% LT End. NB: Start GT End.  This the dot of hip2Head with up.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseSpineAngStart
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("impulseSpineAngStart", value);
			}
		}

		/// <summary>
		/// 100% GE Start to impulseSpineAngMult*100% LT End. NB: Start GT End.  This the dot of hip2Head with up.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseSpineAngEnd
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("impulseSpineAngEnd", value);
			}
		}

		/// <summary>
		/// 100% GE Start to impulseSpineAngMult*100% LT End. NB: leaving this as 1.0 means this functionality is not applied and Start and End have no effect.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseSpineAngMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseSpineAngMult", value);
			}
		}

		/// <summary>
		/// 100% LE Start to impulseVelMult*100% GT End. NB: Start LT End.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float ImpulseVelStart
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseVelStart", value);
			}
		}

		/// <summary>
		/// 100% LE Start to impulseVelMult*100% GT End. NB: Start LT End.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float ImpulseVelEnd
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseVelEnd", value);
			}
		}

		/// <summary>
		/// 100% LE Start to impulseVelMult*100% GT End. NB: leaving this as 1.0 means this functionality is not applied and Start and End have no effect.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseVelMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseVelMult", value);
			}
		}

		/// <summary>
		/// Amount to scale impulse by if the character is airborne and dynamicBalance is OK and impulse is above impulseAirMultStart.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseAirMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseAirMult", value);
			}
		}

		/// <summary>
		/// If impulse is above this value scale it by impulseAirMult.
		/// </summary>
		/// <remarks>
		/// Default value = 100.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ImpulseAirMultStart
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseAirMultStart", value);
			}
		}

		/// <summary>
		/// Amount to clamp impulse to if character is airborne  and dynamicBalance is OK.
		/// </summary>
		/// <remarks>
		/// Default value = 100.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ImpulseAirMax
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseAirMax", value);
			}
		}

		/// <summary>
		/// If impulse is above this amount then do not scale/clamp just let it through as is - it's a shotgun or cannon.
		/// </summary>
		/// <remarks>
		/// Default value = 399.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ImpulseAirApplyAbove
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseAirApplyAbove", value);
			}
		}

		/// <summary>
		/// Scale and/or clamp impulse if the character is airborne and dynamicBalance is OK.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ImpulseAirOn
		{
			set
			{
				SetArgument("impulseAirOn", value);
			}
		}

		/// <summary>
		/// Amount to scale impulse by if the character is contacting with one foot only and dynamicBalance is OK and impulse is above impulseAirMultStart.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseOneLegMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseOneLegMult", value);
			}
		}

		/// <summary>
		/// If impulse is above this value scale it by impulseOneLegMult.
		/// </summary>
		/// <remarks>
		/// Default value = 100.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ImpulseOneLegMultStart
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseOneLegMultStart", value);
			}
		}

		/// <summary>
		/// Amount to clamp impulse to if character is contacting with one foot only  and dynamicBalance is OK.
		/// </summary>
		/// <remarks>
		/// Default value = 100.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ImpulseOneLegMax
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseOneLegMax", value);
			}
		}

		/// <summary>
		/// If impulse is above this amount then do not scale/clamp just let it through as is - it's a shotgun or cannon.
		/// </summary>
		/// <remarks>
		/// Default value = 399.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ImpulseOneLegApplyAbove
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseOneLegApplyAbove", value);
			}
		}

		/// <summary>
		/// Scale and/or clamp impulse if the character is contacting with one leg only and dynamicBalance is OK.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ImpulseOneLegOn
		{
			set
			{
				SetArgument("impulseOneLegOn", value);
			}
		}

		/// <summary>
		/// 0.0 no rigidBody response, 0.5 half partForce half rigidBody, 1.0 = no partForce full rigidBody.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbRatio
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbRatio", value);
			}
		}

		/// <summary>
		/// Rigid body response is shared between the upper and lower body (rbUpperShare = 1-rbLowerShare). RbLowerShare=0.5 gives upper and lower share scaled by mass.  i.e. if 70% ub mass and 30% lower mass then rbLowerShare=0.5 gives actualrbShare of 0.7ub and 0.3lb. rbLowerShare GT 0.5 scales the ub share down from 0.7 and the lb up from 0.3.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbLowerShare
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbLowerShare", value);
			}
		}

		/// <summary>
		/// 0.0 only force, 0.5 = force and half the rigid body moment applied, 1.0 = force and full rigidBody moment.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbMoment
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMoment", value);
			}
		}

		/// <summary>
		/// Maximum twist arm moment of bullet applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RbMaxTwistMomentArm
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMaxTwistMomentArm", value);
			}
		}

		/// <summary>
		/// Maximum broom((everything but the twist) arm moment of bullet applied.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RbMaxBroomMomentArm
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMaxBroomMomentArm", value);
			}
		}

		/// <summary>
		/// If Airborne: 0.0 no rigidBody response, 0.5 half partForce half rigidBody, 1.0 = no partForce full rigidBody.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbRatioAirborne
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbRatioAirborne", value);
			}
		}

		/// <summary>
		/// If Airborne: 0.0 only force, 0.5 = force and half the rigid body moment applied, 1.0 = force and full rigidBody moment.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbMomentAirborne
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMomentAirborne", value);
			}
		}

		/// <summary>
		/// If Airborne: Maximum twist arm moment of bullet applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RbMaxTwistMomentArmAirborne
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMaxTwistMomentArmAirborne", value);
			}
		}

		/// <summary>
		/// If Airborne: Maximum broom((everything but the twist) arm moment of bullet applied.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RbMaxBroomMomentArmAirborne
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMaxBroomMomentArmAirborne", value);
			}
		}

		/// <summary>
		/// If only one leg in contact: 0.0 no rigidBody response, 0.5 half partForce half rigidBody, 1.0 = no partForce full rigidBody.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbRatioOneLeg
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbRatioOneLeg", value);
			}
		}

		/// <summary>
		/// If only one leg in contact: 0.0 only force, 0.5 = force and half the rigid body moment applied, 1.0 = force and full rigidBody moment.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbMomentOneLeg
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMomentOneLeg", value);
			}
		}

		/// <summary>
		/// If only one leg in contact: Maximum twist arm moment of bullet applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RbMaxTwistMomentArmOneLeg
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMaxTwistMomentArmOneLeg", value);
			}
		}

		/// <summary>
		/// If only one leg in contact: Maximum broom((everything but the twist) arm moment of bullet applied.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RbMaxBroomMomentArmOneLeg
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMaxBroomMomentArmOneLeg", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="RbTwistAxis.WorldUp"/>.
		/// </remarks>.
		public RbTwistAxis RbTwistAxis
		{
			set
			{
				SetArgument("rbTwistAxis", (int)value);
			}
		}

		/// <summary>
		/// If false pivot around COM always, if true change pivot depending on foot contact:  to feet center if both feet in contact, or foot position if 1 foot in contact or COM position if no feet in contact.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool RbPivot
		{
			set
			{
				SetArgument("rbPivot", value);
			}
		}
	}

	public sealed class ConfigureBulletsExtraHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ConfigureBulletsExtraHelper for sending a ConfigureBulletsExtra <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ConfigureBulletsExtra <see cref="Message"/> to.</param>
		public ConfigureBulletsExtraHelper(Ped ped) : base(ped, "configureBulletsExtra")
		{
		}

		/// <summary>
		/// Spreads impulse across parts. Currently only for spine parts, not limbs.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ImpulseSpreadOverParts
		{
			set
			{
				SetArgument("impulseSpreadOverParts", value);
			}
		}

		/// <summary>
		/// Duration that impulse is spread over (triangular shaped).
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulsePeriod
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulsePeriod", value);
			}
		}

		/// <summary>
		/// An impulse applied at a point on a body equivalent to an impulse at the center of the body and a torque.  This parameter scales the torque component. (The torque component seems to be excite the rage looseness bug which sends the character in a sometimes wildly different direction to an applied impulse).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseTorqueScale
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseTorqueScale", value);
			}
		}

		/// <summary>
		/// Fix the rage looseness bug by applying only the impulse at the center of the body unless it is a spine part then apply the twist component only of the torque as well.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool LoosenessFix
		{
			set
			{
				SetArgument("loosenessFix", value);
			}
		}

		/// <summary>
		/// Time from hit before impulses are being applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseDelay
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseDelay", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="TorqueMode.Disabled"/>.
		/// If <see cref="TorqueMode.Proportional"/> - proportional to character strength, can reduce impulse amount.
		/// If <see cref="TorqueMode.Additive"/> - no reduction of impulse and not proportional to character strength.
		/// </remarks>
		public TorqueMode TorqueMode
		{
			set
			{
				SetArgument("torqueMode", (int)value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="TorqueSpinMode.FromImpulse"/>.
		/// If <see cref="TorqueSpinMode.Flipping"/> a burst effect is achieved.
		/// </remarks>
		public TorqueSpinMode TorqueSpinMode
		{
			set
			{
				SetArgument("torqueSpinMode", (int)value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="TorqueFilterMode.ApplyEveryBullet"/>.
		/// </remarks>
		public TorqueFilterMode TorqueFilterMode
		{
			set
			{
				SetArgument("torqueFilterMode", (int)value);
			}
		}

		/// <summary>
		/// Always apply torques to spine3 instead of actual part hit.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool TorqueAlwaysSpine3
		{
			set
			{
				SetArgument("torqueAlwaysSpine3", value);
			}
		}

		/// <summary>
		/// Time from hit before torques are being applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TorqueDelay
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torqueDelay", value);
			}
		}

		/// <summary>
		/// Duration of torque.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TorquePeriod
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torquePeriod", value);
			}
		}

		/// <summary>
		/// Multiplies impulse magnitude to arrive at torque that is applied.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float TorqueGain
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torqueGain", value);
			}
		}

		/// <summary>
		/// Minimum ratio of impulse that remains after converting to torque (if in strength-proportional mode).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TorqueCutoff
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torqueCutoff", value);
			}
		}

		/// <summary>
		/// Ratio of torque for next tick (e.g. 1.0: not reducing over time, 0.9: each tick torque is reduced by 10%).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TorqueReductionPerTick
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torqueReductionPerTick", value);
			}
		}

		/// <summary>
		/// Amount of lift (directly multiplies torque axis to give lift force).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LiftGain
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("liftGain", value);
			}
		}

		/// <summary>
		/// Time after impulse is applied that counter impulse is applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CounterImpulseDelay
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("counterImpulseDelay", value);
			}
		}

		/// <summary>
		/// Amount of the original impulse that is countered.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CounterImpulseMag
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("counterImpulseMag", value);
			}
		}

		/// <summary>
		/// Applies the counter impulse counterImpulseDelay(secs) after counterImpulseMag of the Impulse has been applied.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool CounterAfterMagReached
		{
			set
			{
				SetArgument("counterAfterMagReached", value);
			}
		}

		/// <summary>
		/// Add a counter impulse to the pelvis.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool DoCounterImpulse
		{
			set
			{
				SetArgument("doCounterImpulse", value);
			}
		}

		/// <summary>
		/// Amount of the counter impulse applied to hips - the rest is applied to the part originally hit.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CounterImpulse2Hips
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("counterImpulse2Hips", value);
			}
		}

		/// <summary>
		/// Amount to scale impulse by if the dynamicBalance is not OK.  1.0 means this functionality is not applied.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseNoBalMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseNoBalMult", value);
			}
		}

		/// <summary>
		/// 100% LE Start to impulseBalStabMult*100% GT End. NB: Start LT End.
		/// </summary>
		/// <remarks>
		/// Default value = 3.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float ImpulseBalStabStart
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseBalStabStart", value);
			}
		}

		/// <summary>
		/// 100% LE Start to impulseBalStabMult*100% GT End. NB: Start LT End.
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float ImpulseBalStabEnd
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseBalStabEnd", value);
			}
		}

		/// <summary>
		/// 100% LE Start to impulseBalStabMult*100% GT End. NB: leaving this as 1.0 means this functionality is not applied and Start and End have no effect.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseBalStabMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseBalStabMult", value);
			}
		}

		/// <summary>
		/// 100% GE Start to impulseSpineAngMult*100% LT End. NB: Start GT End.  This the dot of hip2Head with up.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseSpineAngStart
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("impulseSpineAngStart", value);
			}
		}

		/// <summary>
		/// 100% GE Start to impulseSpineAngMult*100% LT End. NB: Start GT End.  This the dot of hip2Head with up.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseSpineAngEnd
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("impulseSpineAngEnd", value);
			}
		}

		/// <summary>
		/// 100% GE Start to impulseSpineAngMult*100% LT End. NB: leaving this as 1.0 means this functionality is not applied and Start and End have no effect.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseSpineAngMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseSpineAngMult", value);
			}
		}

		/// <summary>
		/// 100% LE Start to impulseVelMult*100% GT End. NB: Start LT End.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float ImpulseVelStart
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseVelStart", value);
			}
		}

		/// <summary>
		/// 100% LE Start to impulseVelMult*100% GT End. NB: Start LT End.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float ImpulseVelEnd
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseVelEnd", value);
			}
		}

		/// <summary>
		/// 100% LE Start to impulseVelMult*100% GT End. NB: leaving this as 1.0 means this functionality is not applied and Start and End have no effect.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseVelMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseVelMult", value);
			}
		}

		/// <summary>
		/// Amount to scale impulse by if the character is airborne and dynamicBalance is OK and impulse is above impulseAirMultStart.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseAirMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseAirMult", value);
			}
		}

		/// <summary>
		/// If impulse is above this value scale it by impulseAirMult.
		/// </summary>
		/// <remarks>
		/// Default value = 100.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ImpulseAirMultStart
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseAirMultStart", value);
			}
		}

		/// <summary>
		/// Amount to clamp impulse to if character is airborne  and dynamicBalance is OK.
		/// </summary>
		/// <remarks>
		/// Default value = 100.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ImpulseAirMax
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseAirMax", value);
			}
		}

		/// <summary>
		/// If impulse is above this amount then do not scale/clamp just let it through as is - it's a shotgun or cannon.
		/// </summary>
		/// <remarks>
		/// Default value = 399.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ImpulseAirApplyAbove
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseAirApplyAbove", value);
			}
		}

		/// <summary>
		/// Scale and/or clamp impulse if the character is airborne and dynamicBalance is OK.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ImpulseAirOn
		{
			set
			{
				SetArgument("impulseAirOn", value);
			}
		}

		/// <summary>
		/// Amount to scale impulse by if the character is contacting with one foot only and dynamicBalance is OK and impulse is above impulseAirMultStart.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpulseOneLegMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseOneLegMult", value);
			}
		}

		/// <summary>
		/// If impulse is above this value scale it by impulseOneLegMult.
		/// </summary>
		/// <remarks>
		/// Default value = 100.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ImpulseOneLegMultStart
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseOneLegMultStart", value);
			}
		}

		/// <summary>
		/// Amount to clamp impulse to if character is contacting with one foot only  and dynamicBalance is OK.
		/// </summary>
		/// <remarks>
		/// Default value = 100.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ImpulseOneLegMax
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseOneLegMax", value);
			}
		}

		/// <summary>
		/// If impulse is above this amount then do not scale/clamp just let it through as is - it's a shotgun or cannon.
		/// </summary>
		/// <remarks>
		/// Default value = 399.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ImpulseOneLegApplyAbove
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impulseOneLegApplyAbove", value);
			}
		}

		/// <summary>
		/// Scale and/or clamp impulse if the character is contacting with one leg only and dynamicBalance is OK.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ImpulseOneLegOn
		{
			set
			{
				SetArgument("impulseOneLegOn", value);
			}
		}

		/// <summary>
		/// 0.0 no rigidBody response, 0.5 half partForce half rigidBody, 1.0 = no partForce full rigidBody.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbRatio
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbRatio", value);
			}
		}

		/// <summary>
		/// Rigid body response is shared between the upper and lower body (rbUpperShare = 1-rbLowerShare). RbLowerShare=0.5 gives upper and lower share scaled by mass.  i.e. if 70% ub mass and 30% lower mass then rbLowerShare=0.5 gives actualrbShare of 0.7ub and 0.3lb. rbLowerShare GT 0.5 scales the ub share down from 0.7 and the lb up from 0.3.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbLowerShare
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbLowerShare", value);
			}
		}

		/// <summary>
		/// 0.0 only force, 0.5 = force and half the rigid body moment applied, 1.0 = force and full rigidBody moment.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbMoment
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMoment", value);
			}
		}

		/// <summary>
		/// Maximum twist arm moment of bullet applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RbMaxTwistMomentArm
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMaxTwistMomentArm", value);
			}
		}

		/// <summary>
		/// Maximum broom((everything but the twist) arm moment of bullet applied.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RbMaxBroomMomentArm
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMaxBroomMomentArm", value);
			}
		}

		/// <summary>
		/// If Airborne: 0.0 no rigidBody response, 0.5 half partForce half rigidBody, 1.0 = no partForce full rigidBody.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbRatioAirborne
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbRatioAirborne", value);
			}
		}

		/// <summary>
		/// If Airborne: 0.0 only force, 0.5 = force and half the rigid body moment applied, 1.0 = force and full rigidBody moment.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbMomentAirborne
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMomentAirborne", value);
			}
		}

		/// <summary>
		/// If Airborne: Maximum twist arm moment of bullet applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RbMaxTwistMomentArmAirborne
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMaxTwistMomentArmAirborne", value);
			}
		}

		/// <summary>
		/// If Airborne: Maximum broom((everything but the twist) arm moment of bullet applied.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RbMaxBroomMomentArmAirborne
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMaxBroomMomentArmAirborne", value);
			}
		}

		/// <summary>
		/// If only one leg in contact: 0.0 no rigidBody response, 0.5 half partForce half rigidBody, 1.0 = no partForce full rigidBody.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbRatioOneLeg
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbRatioOneLeg", value);
			}
		}

		/// <summary>
		/// If only one leg in contact: 0.0 only force, 0.5 = force and half the rigid body moment applied, 1.0 = force and full rigidBody moment.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RbMomentOneLeg
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMomentOneLeg", value);
			}
		}

		/// <summary>
		/// If only one leg in contact: Maximum twist arm moment of bullet applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RbMaxTwistMomentArmOneLeg
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMaxTwistMomentArmOneLeg", value);
			}
		}

		/// <summary>
		/// If only one leg in contact: Maximum broom((everything but the twist) arm moment of bullet applied.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RbMaxBroomMomentArmOneLeg
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rbMaxBroomMomentArmOneLeg", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="RbTwistAxis.WorldUp"/>.
		/// </remarks>.
		public RbTwistAxis RbTwistAxis
		{
			set
			{
				SetArgument("rbTwistAxis", (int)value);
			}
		}

		/// <summary>
		/// If false pivot around COM always, if true change pivot depending on foot contact:  to feet center if both feet in contact, or foot position if 1 foot in contact or COM position if no feet in contact.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool RbPivot
		{
			set
			{
				SetArgument("rbPivot", value);
			}
		}
	}

	/// <summary>
	/// Enable/disable/edit character limits in real time.  This adjusts limits in RAGE-native space and will *not* reorient the joint.
	/// </summary>
	public sealed class ConfigureLimitsHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ConfigureLimitsHelper for sending a ConfigureLimits <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ConfigureLimits <see cref="Message"/> to.</param>
		/// <remarks>
		/// Enable/disable/edit character limits in real time.  This adjusts limits in RAGE-native space and will *not* reorient the joint.
		/// </remarks>
		public ConfigureLimitsHelper(Ped ped) : base(ped, "configureLimits")
		{
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  for joint limits to configure. Ignored if index != -1.
		/// </summary>
		/// <remarks>
		/// Default value = "fb".
		/// </remarks>
		public string Mask
		{
			set
			{
				SetArgument("mask", value);
			}
		}

		/// <summary>
		/// If false, disable (set all to PI, -PI) limits.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool Enable
		{
			set
			{
				SetArgument("enable", value);
			}
		}

		/// <summary>
		/// If true, set limits to accommodate current desired angles.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ToDesired
		{
			set
			{
				SetArgument("toDesired", value);
			}
		}

		/// <summary>
		/// Return to cached defaults?.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Restore
		{
			set
			{
				SetArgument("restore", value);
			}
		}

		/// <summary>
		/// If true, set limits to the current animated limits.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ToCurAnimation
		{
			set
			{
				SetArgument("toCurAnimation", value);
			}
		}

		/// <summary>
		/// Index of effector to configure.  Set to -1 to use mask.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int Index
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("index", value);
			}
		}

		/// <summary>
		/// Custom limit values to use if not setting limits to desired. Limits are RAGE-native, not NM-wrapper-native.
		/// </summary>
		/// <remarks>
		/// Default value = 1.6f.
		/// Min value = 0.0f.
		/// Max value = 3.1f.
		/// </remarks>
		public float Lean1
		{
			set
			{
				if (value > 3.1f)
				{
					value = 3.1f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("lean1", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 1.6f.
		/// Min value = 0.0f.
		/// Max value = 3.1f.
		/// </remarks>
		public float Lean2
		{
			set
			{
				if (value > 3.1f)
				{
					value = 3.1f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("lean2", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 1.6f.
		/// Min value = 0.0f.
		/// Max value = 3.1f.
		/// </remarks>
		public float Twist
		{
			set
			{
				if (value > 3.1f)
				{
					value = 3.1f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("twist", value);
			}
		}

		/// <summary>
		/// Joint limit margin to add to current animation limits when using those to set runtime limits.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 3.1f.
		/// </remarks>
		public float Margin
		{
			set
			{
				if (value > 3.1f)
				{
					value = 3.1f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("margin", value);
			}
		}
	}

	public sealed class ConfigureSoftLimitHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ConfigureSoftLimitHelper for sending a ConfigureSoftLimit <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ConfigureSoftLimit <see cref="Message"/> to.</param>
		public ConfigureSoftLimitHelper(Ped ped) : base(ped, "configureSoftLimit")
		{
		}

		/// <summary>
		/// Select limb that the soft limit is going to be applied to.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 3.
		/// </remarks>
		public int Index
		{
			set
			{
				if (value > 3)
				{
					value = 3;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("index", value);
			}
		}

		/// <summary>
		/// Stiffness of the soft limit.
		/// Parameter is used to calculate spring term that contributes to the desired acceleration.
		/// </summary>
		/// <remarks>
		/// Default value = 15.0f.
		/// Min value = 0.0f.
		/// Max value = 30.0f.
		/// </remarks>
		public float Stiffness
		{
			set
			{
				if (value > 30.0f)
				{
					value = 30.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stiffness", value);
			}
		}

		/// <summary>
		/// Damping of the soft limit.
		/// Parameter is used to calculate damper term that contributes to the desired acceleration.
		/// To have the system critically dampened set it to 1.0.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.9f.
		/// Max value = 1.1f.
		/// </remarks>
		public float Damping
		{
			set
			{
				if (value > 1.1f)
				{
					value = 1.1f;
				}

				if (value < 0.9f)
				{
					value = 0.9f;
				}

				SetArgument("damping", value);
			}
		}

		/// <summary>
		/// Soft limit angle.
		/// Positive angle in RAD, measured relatively either from hard limit maxAngle (approach direction = -1) or minAngle (approach direction = 1).
		/// This angle will be clamped if outside the joint hard limit range.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 6.3f.
		/// </remarks>
		public float LimitAngle
		{
			set
			{
				if (value > 6.3f)
				{
					value = 6.3f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("limitAngle", value);
			}
		}

		/// <summary>
		/// Limit angle can be measured relatively to joints hard limit minAngle or maxAngle.
		/// Set it to +1 to measure soft limit angle relatively to hard limit minAngle that corresponds to the maximum stretch of the elbow.
		/// Set it to -1 to measure soft limit angle relatively to hard limit maxAngle that corresponds to the maximum stretch of the knee.
		/// </summary>
		/// <remarks>
		/// Default value = 1.
		/// Min value = -1.
		/// Max value = 1.
		/// </remarks>
		public int ApproachDirection
		{
			set
			{
				if (value > 1)
				{
					value = 1;
				}

				if (value < -1)
				{
					value = -1;
				}

				SetArgument("approachDirection", value);
			}
		}

		/// <summary>
		/// Scale stiffness based on character angular velocity.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool VelocityScaled
		{
			set
			{
				SetArgument("velocityScaled", value);
			}
		}
	}

	/// <summary>
	/// This single message allows you to configure the injured arm reaction during shot.
	/// </summary>
	public sealed class ConfigureShotInjuredArmHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ConfigureShotInjuredArmHelper for sending a ConfigureShotInjuredArm <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ConfigureShotInjuredArm <see cref="Message"/> to.</param>
		/// <remarks>
		/// This single message allows you to configure the injured arm reaction during shot.
		/// </remarks>
		public ConfigureShotInjuredArmHelper(Ped ped) : base(ped, "configureShotInjuredArm")
		{
		}

		/// <summary>
		/// Length of the reaction.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float InjuredArmTime
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("injuredArmTime", value);
			}
		}

		/// <summary>
		/// Amount of hip twist.  (Negative values twist into bullet direction - probably not what is wanted).
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = -2.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float HipYaw
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < -2.0f)
				{
					value = -2.0f;
				}

				SetArgument("hipYaw", value);
			}
		}

		/// <summary>
		/// Amount of hip roll.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -2.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float HipRoll
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < -2.0f)
				{
					value = -2.0f;
				}

				SetArgument("hipRoll", value);
			}
		}

		/// <summary>
		/// Additional height added to stepping foot.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 0.7f.
		/// </remarks>
		public float ForceStepExtraHeight
		{
			set
			{
				if (value > 0.7f)
				{
					value = 0.7f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("forceStepExtraHeight", value);
			}
		}

		/// <summary>
		/// Force a step to be taken whether pushed out of balance or not.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ForceStep
		{
			set
			{
				SetArgument("forceStep", value);
			}
		}

		/// <summary>
		/// Turn the character using the balancer.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool StepTurn
		{
			set
			{
				SetArgument("stepTurn", value);
			}
		}

		/// <summary>
		/// Start velocity where parameters begin to be ramped down to zero linearly.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float VelMultiplierStart
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("velMultiplierStart", value);
			}
		}

		/// <summary>
		/// End velocity of ramp where parameters are scaled to zero.
		/// </summary>
		/// <remarks>
		/// Default value = 5.0f.
		/// Min value = 1.0f.
		/// Max value = 40.0f.
		/// </remarks>
		public float VelMultiplierEnd
		{
			set
			{
				if (value > 40.0f)
				{
					value = 40.0f;
				}

				if (value < 1.0f)
				{
					value = 1.0f;
				}

				SetArgument("velMultiplierEnd", value);
			}
		}

		/// <summary>
		/// Velocity above which a step is not forced.
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float VelForceStep
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("velForceStep", value);
			}
		}

		/// <summary>
		/// Velocity above which a stepTurn is not asked for.
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float VelStepTurn
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("velStepTurn", value);
			}
		}

		/// <summary>
		/// Use the velocity scaling parameters.
		/// Tune for standing still then use velocity scaling to make sure a running character stays balanced (the turning tends to make the character fall over more at speed).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool VelScales
		{
			set
			{
				SetArgument("velScales", value);
			}
		}
	}

	/// <summary>
	/// This single message allows you to configure the injured leg reaction during shot.
	/// </summary>
	public sealed class ConfigureShotInjuredLegHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ConfigureShotInjuredLegHelper for sending a ConfigureShotInjuredLeg <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ConfigureShotInjuredLeg <see cref="Message"/> to.</param>
		/// <remarks>
		/// This single message allows you to configure the injured leg reaction during shot.
		/// </remarks>
		public ConfigureShotInjuredLegHelper(Ped ped) : base(ped, "configureShotInjuredLeg")
		{
		}

		/// <summary>
		/// Time before a wounded leg is set to be weak and cause the character to collapse.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float TimeBeforeCollapseWoundLeg
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("timeBeforeCollapseWoundLeg", value);
			}
		}

		/// <summary>
		/// Leg injury duration (reaction to being shot in leg).
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float LegInjuryTime
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legInjuryTime", value);
			}
		}

		/// <summary>
		/// Force a step to be taken whether pushed out of balance or not.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool LegForceStep
		{
			set
			{
				SetArgument("legForceStep", value);
			}
		}

		/// <summary>
		/// Bend the legs via the balancer by this amount if stepping on the injured leg.
		/// 0.2 seems a good default.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LegLimpBend
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legLimpBend", value);
			}
		}

		/// <summary>
		/// Leg lift duration (reaction to being shot in leg).
		/// (Lifting happens when not stepping with other leg).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float LegLiftTime
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legLiftTime", value);
			}
		}

		/// <summary>
		/// Leg injury - leg strength is reduced.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LegInjury
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legInjury", value);
			}
		}

		/// <summary>
		/// Leg injury bend forwards amount when not lifting leg.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LegInjuryHipPitch
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("legInjuryHipPitch", value);
			}
		}

		/// <summary>
		/// Leg injury bend forwards amount when lifting leg.
		/// (Lifting happens when not stepping with other leg).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LegInjuryLiftHipPitch
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("legInjuryLiftHipPitch", value);
			}
		}

		/// <summary>
		/// Leg injury bend forwards amount when not lifting leg.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LegInjurySpineBend
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("legInjurySpineBend", value);
			}
		}

		/// <summary>
		/// Leg injury bend forwards amount when lifting leg.
		/// (Lifting happens when not stepping with other leg).
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LegInjuryLiftSpineBend
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("legInjuryLiftSpineBend", value);
			}
		}
	}

	public sealed class DefineAttachedObjectHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the DefineAttachedObjectHelper for sending a DefineAttachedObject <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the DefineAttachedObject <see cref="Message"/> to.</param>
		public DefineAttachedObjectHelper(Ped ped) : base(ped, "defineAttachedObject")
		{
		}

		/// <summary>
		/// Index of part to attach to.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// Max value = 21.
		/// </remarks>
		public int PartIndex
		{
			set
			{
				if (value > 21)
				{
					value = 21;
				}

				if (value < -1)
				{
					value = -1;
				}

				SetArgument("partIndex", value);
			}
		}

		/// <summary>
		/// Mass of the attached object.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ObjectMass
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("objectMass", value);
			}
		}

		/// <summary>
		/// World position of attached object's center of mass. Must be updated each frame.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 WorldPos
		{
			set
			{
				SetArgument("worldPos", value);
			}
		}
	}

	/// <summary>
	/// Apply an impulse to a named body part.
	/// </summary>
	public sealed class ForceToBodyPartHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ForceToBodyPartHelper for sending a ForceToBodyPart <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ForceToBodyPart <see cref="Message"/> to.</param>
		/// <remarks>
		/// Apply an impulse to a named body part.
		/// </remarks>
		public ForceToBodyPartHelper(Ped ped) : base(ped, "forceToBodyPart")
		{
		}

		/// <summary>
		/// Part or link or bound index.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 28.
		/// </remarks>
		public int PartIndex
		{
			set
			{
				if (value > 28)
				{
					value = 28;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("partIndex", value);
			}
		}

		/// <summary>
		/// Force to apply.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, -50.0f, 0.0f).
		/// Min value = -100000.0f.
		/// Max value = 100000.0f.
		/// </remarks>
		public Vector3 Force
		{
			set
			{
				SetArgument("force",
					Vector3.Clamp(value, new Vector3(-100000.0f, -100000.0f, -100000.0f), new Vector3(100000.0f, 100000.0f, 100000.0f)));
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ForceDefinedInPartSpace
		{
			set
			{
				SetArgument("forceDefinedInPartSpace", value);
			}
		}
	}

	public sealed class LeanInDirectionHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the LeanInDirectionHelper for sending a LeanInDirection <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the LeanInDirection <see cref="Message"/> to.</param>
		public LeanInDirectionHelper(Ped ped) : base(ped, "leanInDirection")
		{
		}

		/// <summary>
		/// Amount of lean, 0 to about 0.5. -ve will move away from the target.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("leanAmount", value);
			}
		}

		/// <summary>
		/// Direction to lean in.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 1.0f).
		/// Min value = 0.0f.
		/// </remarks>
		public Vector3 Dir
		{
			set
			{
				SetArgument("dir", Vector3.Maximize(value, new Vector3(0.0f, 0.0f, 0.0f)));
			}
		}
	}

	public sealed class LeanRandomHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the LeanRandomHelper for sending a LeanRandom <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the LeanRandom <see cref="Message"/> to.</param>
		public LeanRandomHelper(Ped ped) : base(ped, "leanRandom")
		{
		}

		/// <summary>
		/// Minimum amount of lean.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanAmountMin
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanAmountMin", value);
			}
		}

		/// <summary>
		/// Maximum amount of lean.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanAmountMax
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanAmountMax", value);
			}
		}

		/// <summary>
		/// Minimum time until changing direction.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float ChangeTimeMin
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("changeTimeMin", value);
			}
		}

		/// <summary>
		/// Maximum time until changing direction.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float ChangeTimeMax
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("changeTimeMax", value);
			}
		}
	}

	public sealed class LeanToPositionHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the LeanToPositionHelper for sending a LeanToPosition <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the LeanToPosition <see cref="Message"/> to.</param>
		public LeanToPositionHelper(Ped ped) : base(ped, "leanToPosition")
		{
		}

		/// <summary>
		/// Amount of lean, 0 to about 0.5. -ve will move away from the target.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -0.5f.
		/// Max value = 0.5f.
		/// </remarks>
		public float LeanAmount
		{
			set
			{
				if (value > 0.5f)
				{
					value = 0.5f;
				}

				if (value < -0.5f)
				{
					value = -0.5f;
				}

				SetArgument("leanAmount", value);
			}
		}

		/// <summary>
		/// Position to head towards.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Pos
		{
			set
			{
				SetArgument("pos", value);
			}
		}
	}

	public sealed class LeanTowardsObjectHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the LeanTowardsObjectHelper for sending a LeanTowardsObject <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the LeanTowardsObject <see cref="Message"/> to.</param>
		public LeanTowardsObjectHelper(Ped ped) : base(ped, "leanTowardsObject")
		{
		}

		/// <summary>
		/// Amount of lean, 0 to about 0.5. -ve will move away from the target.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -0.5f.
		/// Max value = 0.5f.
		/// </remarks>
		public float LeanAmount
		{
			set
			{
				if (value > 0.5f)
				{
					value = 0.5f;
				}

				if (value < -0.5f)
				{
					value = -0.5f;
				}

				SetArgument("leanAmount", value);
			}
		}

		/// <summary>
		/// Offset from instance position added when calculating position to lean to.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = -100.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public Vector3 Offset
		{
			set
			{
				SetArgument("offset",
					Vector3.Clamp(value, new Vector3(-100.0f, -100.0f, -100.0f), new Vector3(100.0f, 100.0f, 100.0f)));
			}
		}

		/// <summary>
		/// LevelIndex of object to lean towards.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int InstanceIndex
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("instanceIndex", value);
			}
		}

		/// <summary>
		/// BoundIndex of object to lean towards (0 = just use instance coordinates).
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// </remarks>
		public int BoundIndex
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				SetArgument("boundIndex", value);
			}
		}
	}

	public sealed class HipsLeanInDirectionHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the HipsLeanInDirectionHelper for sending a HipsLeanInDirection <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the HipsLeanInDirection <see cref="Message"/> to.</param>
		public HipsLeanInDirectionHelper(Ped ped) : base(ped, "hipsLeanInDirection")
		{
		}

		/// <summary>
		/// Amount of lean, 0 to about 0.5. -ve will move away from the target.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("leanAmount", value);
			}
		}

		/// <summary>
		/// Direction to lean in.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 1.0f).
		/// Min value = 0.0f.
		/// </remarks>
		public Vector3 Dir
		{
			set
			{
				SetArgument("dir", Vector3.Maximize(value, new Vector3(0.0f, 0.0f, 0.0f)));
			}
		}
	}

	public sealed class HipsLeanRandomHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the HipsLeanRandomHelper for sending a HipsLeanRandom <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the HipsLeanRandom <see cref="Message"/> to.</param>
		public HipsLeanRandomHelper(Ped ped) : base(ped, "hipsLeanRandom")
		{
		}

		/// <summary>
		/// Minimum amount of lean.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanAmountMin
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanAmountMin", value);
			}
		}

		/// <summary>
		/// Maximum amount of lean.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanAmountMax
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanAmountMax", value);
			}
		}

		/// <summary>
		/// Min time until changing direction.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float ChangeTimeMin
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("changeTimeMin", value);
			}
		}

		/// <summary>
		/// Maximum time until changing direction.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float ChangeTimeMax
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("changeTimeMax", value);
			}
		}
	}

	public sealed class HipsLeanToPositionHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the HipsLeanToPositionHelper for sending a HipsLeanToPosition <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the HipsLeanToPosition <see cref="Message"/> to.</param>
		public HipsLeanToPositionHelper(Ped ped) : base(ped, "hipsLeanToPosition")
		{
		}

		/// <summary>
		/// Amount of lean, 0 to about 0.5. -ve will move away from the target.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -0.5f.
		/// Max value = 0.5f.
		/// </remarks>
		public float LeanAmount
		{
			set
			{
				if (value > 0.5f)
				{
					value = 0.5f;
				}

				if (value < -0.5f)
				{
					value = -0.5f;
				}

				SetArgument("leanAmount", value);
			}
		}

		/// <summary>
		/// Position to head towards.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Pos
		{
			set
			{
				SetArgument("pos", value);
			}
		}
	}

	public sealed class HipsLeanTowardsObjectHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the HipsLeanTowardsObjectHelper for sending a HipsLeanTowardsObject <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the HipsLeanTowardsObject <see cref="Message"/> to.</param>
		public HipsLeanTowardsObjectHelper(Ped ped) : base(ped, "hipsLeanTowardsObject")
		{
		}

		/// <summary>
		/// Amount of lean, 0 to about 0.5. -ve will move away from the target.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -0.5f.
		/// Max value = 0.5f.
		/// </remarks>
		public float LeanAmount
		{
			set
			{
				if (value > 0.5f)
				{
					value = 0.5f;
				}

				if (value < -0.5f)
				{
					value = -0.5f;
				}

				SetArgument("leanAmount", value);
			}
		}

		/// <summary>
		/// Offset from instance position added when calculating position to lean to.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = -100.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public Vector3 Offset
		{
			set
			{
				SetArgument("offset",
					Vector3.Clamp(value, new Vector3(-100.0f, -100.0f, -100.0f), new Vector3(100.0f, 100.0f, 100.0f)));
			}
		}

		/// <summary>
		/// LevelIndex of object to lean hips towards.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int InstanceIndex
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("instanceIndex", value);
			}
		}

		/// <summary>
		/// BoundIndex of object to lean hips towards (0 = just use instance coordinates).
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// </remarks>
		public int BoundIndex
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				SetArgument("boundIndex", value);
			}
		}
	}

	public sealed class ForceLeanInDirectionHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ForceLeanInDirectionHelper for sending a ForceLeanInDirection <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ForceLeanInDirection <see cref="Message"/> to.</param>
		public ForceLeanInDirectionHelper(Ped ped) : base(ped, "forceLeanInDirection")
		{
		}

		/// <summary>
		/// Amount of lean, 0 to about 0.5. -ve will move away from the target.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("leanAmount", value);
			}
		}

		/// <summary>
		/// Direction to lean in.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 1.0f).
		/// Min value = 0.0f.
		/// </remarks>
		public Vector3 Dir
		{
			set
			{
				SetArgument("dir", Vector3.Maximize(value, new Vector3(0.0f, 0.0f, 0.0f)));
			}
		}

		/// <summary>
		/// Body part that the force is applied to.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 21.
		/// </remarks>
		public int BodyPart
		{
			set
			{
				if (value > 21)
				{
					value = 21;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("bodyPart", value);
			}
		}
	}

	public sealed class ForceLeanRandomHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ForceLeanRandomHelper for sending a ForceLeanRandom <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ForceLeanRandom <see cref="Message"/> to.</param>
		public ForceLeanRandomHelper(Ped ped) : base(ped, "forceLeanRandom")
		{
		}

		/// <summary>
		/// Minimum amount of lean.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanAmountMin
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanAmountMin", value);
			}
		}

		/// <summary>
		/// Maximum amount of lean.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanAmountMax
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanAmountMax", value);
			}
		}

		/// <summary>
		/// Min time until changing direction.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float ChangeTimeMin
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("changeTimeMin", value);
			}
		}

		/// <summary>
		/// Maximum time until changing direction.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float ChangeTimeMax
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("changeTimeMax", value);
			}
		}

		/// <summary>
		/// Body part that the force is applied to.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 21.
		/// </remarks>
		public int BodyPart
		{
			set
			{
				if (value > 21)
				{
					value = 21;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("bodyPart", value);
			}
		}
	}

	public sealed class ForceLeanToPositionHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ForceLeanToPositionHelper for sending a ForceLeanToPosition <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ForceLeanToPosition <see cref="Message"/> to.</param>
		public ForceLeanToPositionHelper(Ped ped) : base(ped, "forceLeanToPosition")
		{
		}

		/// <summary>
		/// Amount of lean, 0 to about 0.5. -ve will move away from the target.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -0.5f.
		/// Max value = 0.5f.
		/// </remarks>
		public float LeanAmount
		{
			set
			{
				if (value > 0.5f)
				{
					value = 0.5f;
				}

				if (value < -0.5f)
				{
					value = -0.5f;
				}

				SetArgument("leanAmount", value);
			}
		}

		/// <summary>
		/// Position to head towards.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Pos
		{
			set
			{
				SetArgument("pos", value);
			}
		}

		/// <summary>
		/// Body part that the force is applied to.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 21.
		/// </remarks>
		public int BodyPart
		{
			set
			{
				if (value > 21)
				{
					value = 21;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("bodyPart", value);
			}
		}
	}

	public sealed class ForceLeanTowardsObjectHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ForceLeanTowardsObjectHelper for sending a ForceLeanTowardsObject <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ForceLeanTowardsObject <see cref="Message"/> to.</param>
		public ForceLeanTowardsObjectHelper(Ped ped) : base(ped, "forceLeanTowardsObject")
		{
		}

		/// <summary>
		/// Amount of lean, 0 to about 0.5. -ve will move away from the target.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -0.5f.
		/// Max value = 0.5f.
		/// </remarks>
		public float LeanAmount
		{
			set
			{
				if (value > 0.5f)
				{
					value = 0.5f;
				}

				if (value < -0.5f)
				{
					value = -0.5f;
				}

				SetArgument("leanAmount", value);
			}
		}

		/// <summary>
		/// Offset from instance position added when calculating position to lean to.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = -100.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public Vector3 Offset
		{
			set
			{
				SetArgument("offset",
					Vector3.Clamp(value, new Vector3(-100.0f, -100.0f, -100.0f), new Vector3(100.0f, 100.0f, 100.0f)));
			}
		}

		/// <summary>
		/// LevelIndex of object to move towards.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int InstanceIndex
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("instanceIndex", value);
			}
		}

		/// <summary>
		/// BoundIndex of object to move towards (0 = just use instance coordinates).
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// </remarks>
		public int BoundIndex
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				SetArgument("boundIndex", value);
			}
		}

		/// <summary>
		/// Body part that the force is applied to.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 21.
		/// </remarks>
		public int BodyPart
		{
			set
			{
				if (value > 21)
				{
					value = 21;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("bodyPart", value);
			}
		}
	}

	/// <summary>
	/// Use this message to manually set the body stiffness values -before using Active Pose to drive to an animated pose, for example.
	/// </summary>
	public sealed class SetStiffnessHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the SetStiffnessHelper for sending a SetStiffness <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the SetStiffness <see cref="Message"/> to.</param>
		/// <remarks>
		/// Use this message to manually set the body stiffness values -before using Active Pose to drive to an animated pose, for example.
		/// </remarks>
		public SetStiffnessHelper(Ped ped) : base(ped, "setStiffness")
		{
		}

		/// <summary>
		/// Stiffness of whole character.
		/// </summary>
		/// <remarks>
		/// Default value = 12.0f.
		/// Min value = 2.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float BodyStiffness
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 2.0f)
				{
					value = 2.0f;
				}

				SetArgument("bodyStiffness", value);
			}
		}

		/// <summary>
		/// Damping amount, less is underdamped.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float Damping
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("damping", value);
			}
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see Active Pose notes for possible values).
		/// </summary>
		/// <remarks>
		/// Default value = "fb".
		/// </remarks>
		public string Mask
		{
			set
			{
				SetArgument("mask", value);
			}
		}
	}

	/// <summary>
	/// Use this message to manually set the muscle stiffness values -before using Active Pose to drive to an animated pose, for example.
	/// </summary>
	public sealed class SetMuscleStiffnessHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the SetMuscleStiffnessHelper for sending a SetMuscle stiffness <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the SetMuscle stiffness <see cref="Message"/> to.</param>
		/// <remarks>
		/// Use this message to manually set the muscle stiffness values -before using Active Pose to drive to an animated pose, for example.
		/// </remarks>
		public SetMuscleStiffnessHelper(Ped ped) : base(ped, "setMuscleStiffness")
		{
		}

		/// <summary>
		/// Muscle stiffness of joint/s.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float MuscleStiffness
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("muscleStiffness", value);
			}
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see Active Pose notes for possible values).
		/// </summary>
		/// <remarks>
		/// Default value = "fb".
		/// </remarks>
		public string Mask
		{
			set
			{
				SetArgument("mask", value);
			}
		}
	}

	/// <summary>
	/// Use this message to set the character's weapon mode.  This is an alternativeto the setWeaponMode public function.
	/// </summary>
	public sealed class SetWeaponModeHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the SetWeaponModeHelper for sending a SetWeaponMode <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the SetWeaponMode <see cref="Message"/> to.</param>
		/// <remarks>
		/// Use this message to set the character's weapon mode.  This is an alternativeto the setWeaponMode public function.
		/// </remarks>
		public SetWeaponModeHelper(Ped ped) : base(ped, "setWeaponMode")
		{
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="WeaponMode.PistolRight"/>.
		/// </remarks>.
		public WeaponMode WeaponMode
		{
			set
			{
				SetArgument("weaponMode", (int)value);
			}
		}
	}

	/// <summary>
	/// Use this message to register weapon.  This is an alternativeto the registerWeapon public function.
	/// </summary>
	public sealed class RegisterWeaponHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the RegisterWeaponHelper for sending a RegisterWeapon <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the RegisterWeapon <see cref="Message"/> to.</param>
		/// <remarks>
		/// Use this message to register weapon.  This is an alternativeto the registerWeapon public function.
		/// </remarks>
		public RegisterWeaponHelper(Ped ped) : base(ped, "registerWeapon")
		{
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="Hand.Right"/>.
		/// </remarks>
		public Hand Hand
		{
			set
			{
				SetArgument("hand", (int)value);
			}
		}

		/// <summary>
		/// Level index of the weapon.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int LevelIndex
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("levelIndex", value);
			}
		}

		/// <summary>
		/// Pointer to the hand-gun constraint handle.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int ConstraintHandle
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("constraintHandle", value);
			}
		}

		/// <summary>
		/// A vector of the gunToHand matrix.  The gunToHandMatrix is the desired gunToHandMatrix in the aimingPose. (The gunToHandMatrix when pointGun starts can be different so will be blended to this desired one).
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(1.0f, 0.0f, 0.0f).
		/// Min value = 0.0f.
		/// </remarks>
		public Vector3 GunToHandA
		{
			set
			{
				SetArgument("gunToHandA", Vector3.Maximize(value, new Vector3(0.0f, 0.0f, 0.0f)));
			}
		}

		/// <summary>
		/// B vector of the gunToHand matrix.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 1.0f, 0.0f).
		/// Min value = 0.0f.
		/// </remarks>
		public Vector3 GunToHandB
		{
			set
			{
				SetArgument("gunToHandB", Vector3.Maximize(value, new Vector3(0.0f, 0.0f, 0.0f)));
			}
		}

		/// <summary>
		/// C vector of the gunToHand matrix.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 1.0f).
		/// Min value = 0.0f.
		/// </remarks>
		public Vector3 GunToHandC
		{
			set
			{
				SetArgument("gunToHandC", Vector3.Maximize(value, new Vector3(0.0f, 0.0f, 0.0f)));
			}
		}

		/// <summary>
		/// D vector of the gunToHand matrix.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = 0.0f.
		/// </remarks>
		public Vector3 GunToHandD
		{
			set
			{
				SetArgument("gunToHandD", Vector3.Maximize(value, new Vector3(0.0f, 0.0f, 0.0f)));
			}
		}

		/// <summary>
		/// Gun center to muzzle expressed in gun co-ordinates.  To get the line of sight/barrel of the gun. Assumption: the muzzle direction is always along the same primary axis of the gun.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 GunToMuzzleInGun
		{
			set
			{
				SetArgument("gunToMuzzleInGun", value);
			}
		}

		/// <summary>
		/// Gun center to butt expressed in gun co-ordinates.  The gun pivots around this point when aiming.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 GunToButtInGun
		{
			set
			{
				SetArgument("gunToButtInGun", value);
			}
		}
	}

	public sealed class ShotRelaxHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ShotRelaxHelper for sending a ShotRelax <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ShotRelax <see cref="Message"/> to.</param>
		public ShotRelaxHelper(Ped ped) : base(ped, "shotRelax")
		{
		}

		/// <summary>
		/// Time over which to relax to full relaxation for upper body.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 40.0f.
		/// </remarks>
		public float RelaxPeriodUpper
		{
			set
			{
				if (value > 40.0f)
				{
					value = 40.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("relaxPeriodUpper", value);
			}
		}

		/// <summary>
		/// Time over which to relax to full relaxation for lower body.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 40.0f.
		/// </remarks>
		public float RelaxPeriodLower
		{
			set
			{
				if (value > 40.0f)
				{
					value = 40.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("relaxPeriodLower", value);
			}
		}
	}

	/// <summary>
	/// One shot message apply a force to the hand as we fire the gun that should be in this hand.
	/// </summary>
	public sealed class FireWeaponHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the FireWeaponHelper for sending a FireWeapon <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the FireWeapon <see cref="Message"/> to.</param>
		/// <remarks>
		/// One shot message apply a force to the hand as we fire the gun that should be in this hand.
		/// </remarks>
		public FireWeaponHelper(Ped ped) : base(ped, "fireWeapon")
		{
		}

		/// <summary>
		/// The force of the gun.
		/// </summary>
		/// <remarks>
		/// Default value = 1000.0f.
		/// Min value = 0.0f.
		/// Max value = 10000.0f.
		/// </remarks>
		public float FiredWeaponStrength
		{
			set
			{
				if (value > 10000.0f)
				{
					value = 10000.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("firedWeaponStrength", value);
			}
		}

		/// <summary>
		/// Which hand is the gun in.
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="Hand.Left"/>.
		/// </remarks>
		public Hand GunHandEnum
		{
			set
			{
				SetArgument("gunHandEnum", (int)value);
			}
		}

		/// <summary>
		/// Should we apply some of the force at the shoulder. Force double handed weapons (Ak47 etc).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ApplyFireGunForceAtClavicle
		{
			set
			{
				SetArgument("applyFireGunForceAtClavicle", value);
			}
		}

		/// <summary>
		/// Minimum time before next fire impulse.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float InhibitTime
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("inhibitTime", value);
			}
		}

		/// <summary>
		/// Direction of impulse in gun frame.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Direction
		{
			set
			{
				SetArgument("direction", value);
			}
		}

		/// <summary>
		/// Split force between hand and clavicle when applyFireGunForceAtClavicle is true. 1 = all hand, 0 = all clavicle.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Split
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("split", value);
			}
		}
	}

	/// <summary>
	/// One shot to give state of constraints on character and response to constraints.
	/// </summary>
	public sealed class ConfigureConstraintsHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ConfigureConstraintsHelper for sending a ConfigureConstraints <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ConfigureConstraints <see cref="Message"/> to.</param>
		/// <remarks>
		/// One shot to give state of constraints on character and response to constraints.
		/// </remarks>
		public ConfigureConstraintsHelper(Ped ped) : base(ped, "configureConstraints")
		{
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool HandCuffs
		{
			set
			{
				SetArgument("handCuffs", value);
			}
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool HandCuffsBehindBack
		{
			set
			{
				SetArgument("handCuffsBehindBack", value);
			}
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool LegCuffs
		{
			set
			{
				SetArgument("legCuffs", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool RightDominant
		{
			set
			{
				SetArgument("rightDominant", value);
			}
		}

		/// <summary>
		/// 0 setCurrent, 1= IK to dominant, (2=pointGunLikeIK //not implemented).
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 5.
		/// </remarks>
		public int PassiveMode
		{
			set
			{
				if (value > 5)
				{
					value = 5;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("passiveMode", value);
			}
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool BespokeBehavior
		{
			set
			{
				SetArgument("bespokeBehaviour", value);
			}
		}

		/// <summary>
		/// Blend Arms to zero pose.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Blend2ZeroPose
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("blend2ZeroPose", value);
			}
		}
	}

	public sealed class StayUprightHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the StayUprightHelper for sending a StayUpright <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the StayUpright <see cref="Message"/> to.</param>
		public StayUprightHelper(Ped ped) : base(ped, "stayUpright")
		{
		}

		/// <summary>
		/// Enable force based constraint.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseForces
		{
			set
			{
				SetArgument("useForces", value);
			}
		}

		/// <summary>
		/// Enable torque based constraint.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseTorques
		{
			set
			{
				SetArgument("useTorques", value);
			}
		}

		/// <summary>
		/// Uses position/orientation control on the spine and drifts in the direction of bullets.  This ignores all other stayUpright settings.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool LastStandMode
		{
			set
			{
				SetArgument("lastStandMode", value);
			}
		}

		/// <summary>
		/// The sink rate (higher for a faster drop).
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LastStandSinkRate
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("lastStandSinkRate", value);
			}
		}

		/// <summary>
		/// Higher values for more damping.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LastStandHorizDamping
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("lastStandHorizDamping", value);
			}
		}

		/// <summary>
		/// Max time allowed in last stand mode.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float LastStandMaxTime
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("lastStandMaxTime", value);
			}
		}

		/// <summary>
		/// Use cheat torques to face the direction of bullets if not facing too far away.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool TurnTowardsBullets
		{
			set
			{
				SetArgument("turnTowardsBullets", value);
			}
		}

		/// <summary>
		/// Make strength of constraint function of COM velocity.  Uses -1 for forceDamping if the damping is positive.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool VelocityBased
		{
			set
			{
				SetArgument("velocityBased", value);
			}
		}

		/// <summary>
		/// Only apply torque based constraint when airBorne.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool TorqueOnlyInAir
		{
			set
			{
				SetArgument("torqueOnlyInAir", value);
			}
		}

		/// <summary>
		/// Strength of constraint.
		/// </summary>
		/// <remarks>
		/// Default value = 3.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ForceStrength
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("forceStrength", value);
			}
		}

		/// <summary>
		/// Damping in constraint: -1 makes it scale automagically with forceStrength.  Other negative values will scale this automagic damping.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 50.0f.
		/// </remarks>
		public float ForceDamping
		{
			set
			{
				if (value > 50.0f)
				{
					value = 50.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("forceDamping", value);
			}
		}

		/// <summary>
		/// Multiplier to the force applied to the feet.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ForceFeetMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("forceFeetMult", value);
			}
		}

		/// <summary>
		/// Share of pelvis force applied to spine3.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ForceSpine3Share
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("forceSpine3Share", value);
			}
		}

		/// <summary>
		/// How much the character lean is taken into account when reducing the force.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ForceLeanReduction
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("forceLeanReduction", value);
			}
		}

		/// <summary>
		/// Share of the feet force to the airborne foot.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ForceInAirShare
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("forceInAirShare", value);
			}
		}

		/// <summary>
		/// When min and max are greater than 0 the constraint strength is determined from character strength, scaled into the range given by min and max.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ForceMin
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("forceMin", value);
			}
		}

		/// <summary>
		/// See above.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ForceMax
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("forceMax", value);
			}
		}

		/// <summary>
		/// When in velocityBased mode, the COM velocity at which constraint reaches maximum strength (forceStrength).
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.1f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ForceSaturationVel
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("forceSaturationVel", value);
			}
		}

		/// <summary>
		/// When in velocityBased mode, the COM velocity above which constraint starts applying forces.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float ForceThresholdVel
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("forceThresholdVel", value);
			}
		}

		/// <summary>
		/// Strength of torque based constraint.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float TorqueStrength
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torqueStrength", value);
			}
		}

		/// <summary>
		/// Damping of torque based constraint.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float TorqueDamping
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torqueDamping", value);
			}
		}

		/// <summary>
		/// When in velocityBased mode, the COM velocity at which constraint reaches maximum strength (torqueStrength).
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.1f.
		/// Max value = 10.0f.
		/// </remarks>
		public float TorqueSaturationVel
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("torqueSaturationVel", value);
			}
		}

		/// <summary>
		/// When in velocityBased mode, the COM velocity above which constraint starts applying torques.
		/// </summary>
		/// <remarks>
		/// Default value = 2.5f.
		/// Min value = 0.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float TorqueThresholdVel
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("torqueThresholdVel", value);
			}
		}

		/// <summary>
		/// Distance the foot is behind Com projection that is still considered able to generate the support for the upright constraint.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = -2.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float SupportPosition
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < -2.0f)
				{
					value = -2.0f;
				}

				SetArgument("supportPosition", value);
			}
		}

		/// <summary>
		/// Still apply this fraction of the upright constaint force if the foot is not in a position (defined by supportPosition) to generate the support for the upright constraint.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float NoSupportForceMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("noSupportForceMult", value);
			}
		}

		/// <summary>
		/// Strength of cheat force applied upwards to spine3 to help the character up steps/slopes.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float StepUpHelp
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stepUpHelp", value);
			}
		}

		/// <summary>
		/// How much the cheat force takes into account the acceleration of moving platforms.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float StayUpAcc
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stayUpAcc", value);
			}
		}

		/// <summary>
		/// The maximum floorAcceleration (of a moving platform) that the cheat force takes into account.
		/// </summary>
		/// <remarks>
		/// Default value = 5.0f.
		/// Min value = 0.0f.
		/// Max value = 15.0f.
		/// </remarks>
		public float StayUpAccMax
		{
			set
			{
				if (value > 15.0f)
				{
					value = 15.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stayUpAccMax", value);
			}
		}
	}

	/// <summary>
	/// Send this message to immediately stop all behaviors from executing.
	/// </summary>
	public sealed class StopAllBehaviorsHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the StopAllBehaviorsHelper for sending a StopAllBehaviors <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the StopAllBehaviors <see cref="Message"/> to.</param>
		/// <remarks>
		/// Send this message to immediately stop all behaviors from executing.
		/// </remarks>
		public StopAllBehaviorsHelper(Ped ped) : base(ped, "stopAllBehaviours")
		{
		}
	}

	/// <summary>
	/// Sets character's strength on the dead-granny-to-healthy-terminator scale: [0..1].
	/// </summary>
	public sealed class SetCharacterStrengthHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the SetCharacterStrengthHelper for sending a SetCharacterStrength <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the SetCharacterStrength <see cref="Message"/> to.</param>
		/// <remarks>
		/// Sets character's strength on the dead-granny-to-healthy-terminator scale: [0..1].
		/// </remarks>
		public SetCharacterStrengthHelper(Ped ped) : base(ped, "setCharacterStrength")
		{
		}

		/// <summary>
		/// Strength of character.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CharacterStrength
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("characterStrength", value);
			}
		}
	}

	/// <summary>
	/// Sets character's health on the dead-to-alive scale: [0..1].
	/// </summary>
	public sealed class SetCharacterHealthHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the SetCharacterHealthHelper for sending a SetCharacterHealth <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the SetCharacterHealth <see cref="Message"/> to.</param>
		/// <remarks>
		/// Sets character's health on the dead-to-alive scale: [0..1].
		/// </remarks>
		public SetCharacterHealthHelper(Ped ped) : base(ped, "setCharacterHealth")
		{
		}

		/// <summary>
		/// Health of character.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CharacterHealth
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("characterHealth", value);
			}
		}
	}

	/// <summary>
	/// Sets the type of reaction if catchFall is called.
	/// </summary>
	public sealed class SetFallingReactionHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the SetFallingReactionHelper for sending a SetFallingReaction <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the SetFallingReaction <see cref="Message"/> to.</param>
		/// <remarks>
		/// Sets the type of reaction if catchFall is called.
		/// </remarks>
		public SetFallingReactionHelper(Ped ped) : base(ped, "setFallingReaction")
		{
		}

		/// <summary>
		/// Set to true to get handsAndKnees catchFall if catchFall called. If true allows the dynBalancer to stay on during the catchfall and modifies the catch fall to give a more alive looking performance (hands and knees for front landing or sitting up for back landing).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool HandsAndKnees
		{
			set
			{
				SetArgument("handsAndKnees", value);
			}
		}

		/// <summary>
		/// If true catchFall will call rollDownstairs if comVel GT comVelRDSThresh - prevents excessive sliding in catchFall.  Was previously only true for handsAndKnees.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool CallRDS
		{
			set
			{
				SetArgument("callRDS", value);
			}
		}

		/// <summary>
		/// ComVel above which rollDownstairs will start - prevents excessive sliding in catchFall.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float ComVelRDSThresh
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("comVelRDSThresh", value);
			}
		}

		/// <summary>
		/// For rds catchFall only: True to resist rolling motion (rolling motion is set off by ub contact and a sliding velocity), false to allow more of a continuous rolling  (rolling motion is set off at a sliding velocity).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ResistRolling
		{
			set
			{
				SetArgument("resistRolling", value);
			}
		}

		/// <summary>
		/// Strength is reduced in the catchFall when the arms contact the ground.  0.2 is good for handsAndKnees.  2.5 is good for normal catchFall, anything lower than 1.0 for normal catchFall may lead to bad catchFall poses.
		/// </summary>
		/// <remarks>
		/// Default value = 2.5f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ArmReduceSpeed
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armReduceSpeed", value);
			}
		}

		/// <summary>
		/// Reach length multiplier that scales characters arm topological length, value in range from (0, 1 GT  where 1.0 means reach length is maximum.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.3f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ReachLengthMultiplier
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.3f)
				{
					value = 0.3f;
				}

				SetArgument("reachLengthMultiplier", value);
			}
		}

		/// <summary>
		/// Time after hitting ground that the catchFall can call rds.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float InhibitRollingTime
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("inhibitRollingTime", value);
			}
		}

		/// <summary>
		/// Time after hitting ground that the catchFall can change the friction of parts to inhibit sliding.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ChangeFrictionTime
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("changeFrictionTime", value);
			}
		}

		/// <summary>
		/// 8.0 was used on yanked) Friction multiplier on body parts when on ground.  Character can look too slidy with groundFriction = 1.  Higher values give a more jerky reaction but this seems timestep dependent especially for dragged by the feet.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float GroundFriction
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("groundFriction", value);
			}
		}

		/// <summary>
		/// Min Friction of an impact with a body part (not head, hands or feet) - to increase friction of slippy environment to get character to roll better.  Applied in catchFall and rollUp(rollDownStairs).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float FrictionMin
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("frictionMin", value);
			}
		}

		/// <summary>
		/// Max Friction of an impact with a body part (not head, hands or feet) - to increase friction of slippy environment to get character to roll better.  Applied in catchFall and rollUp(rollDownStairs).
		/// </summary>
		/// <remarks>
		/// Default value = 9999.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float FrictionMax
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("frictionMax", value);
			}
		}

		/// <summary>
		/// Apply tactics to help stop on slopes.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool StopOnSlopes
		{
			set
			{
				SetArgument("stopOnSlopes", value);
			}
		}

		/// <summary>
		/// Override slope value to manually force stopping on flat ground.  Encourages character to come to rest face down or face up.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float StopManual
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stopManual", value);
			}
		}

		/// <summary>
		/// Speed at which strength reduces when stopped.
		/// </summary>
		/// <remarks>
		/// Default value = 5.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float StoppedStrengthDecay
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stoppedStrengthDecay", value);
			}
		}

		/// <summary>
		/// Bias spine post towards hunched (away from arched).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SpineLean1Offset
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spineLean1Offset", value);
			}
		}

		/// <summary>
		/// Hold rifle in a safe position to reduce complications with collision.  Only applied if holding a rifle.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool RiflePose
		{
			set
			{
				SetArgument("riflePose", value);
			}
		}

		/// <summary>
		/// Enable head ground avoidance when handsAndKnees is true.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool HkHeadAvoid
		{
			set
			{
				SetArgument("hkHeadAvoid", value);
			}
		}

		/// <summary>
		/// Discourage the character getting stuck propped up by elbows when falling backwards - by inhibiting backwards moving clavicles (keeps the arms slightly wider).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AntiPropClav
		{
			set
			{
				SetArgument("antiPropClav", value);
			}
		}

		/// <summary>
		/// Discourage the character getting stuck propped up by elbows when falling backwards - by weakening the arms as soon they hit the floor.  (Also stops the hands lifting up when flat on back).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AntiPropWeak
		{
			set
			{
				SetArgument("antiPropWeak", value);
			}
		}

		/// <summary>
		/// Head weakens as arms weaken. If false and antiPropWeak when falls onto back doesn't loosen neck so early (matches bodyStrength instead).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool HeadAsWeakAsArms
		{
			set
			{
				SetArgument("headAsWeakAsArms", value);
			}
		}

		/// <summary>
		/// When bodyStrength is less than successStrength send a success feedback - DO NOT GO OUTSIDE MIN/MAX PARAMETER VALUES OTHERWISE NO SUCCESS FEEDBACK WILL BE SENT.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.3f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SuccessStrength
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.3f)
				{
					value = 0.3f;
				}

				SetArgument("successStrength", value);
			}
		}
	}

	/// <summary>
	/// Sets viscosity applied to damping limbs.
	/// </summary>
	public sealed class SetCharacterUnderwaterHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the SetCharacterUnderwaterHelper for sending a SetCharacterUnderwater <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the SetCharacterUnderwater <see cref="Message"/> to.</param>
		/// <remarks>
		/// Sets viscosity applied to damping limbs.
		/// </remarks>
		public SetCharacterUnderwaterHelper(Ped ped) : base(ped, "setCharacterUnderwater")
		{
		}

		/// <summary>
		/// Is character underwater?.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Underwater
		{
			set
			{
				SetArgument("underwater", value);
			}
		}

		/// <summary>
		/// Viscosity applied to character's parts.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float Viscosity
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("viscosity", value);
			}
		}

		/// <summary>
		/// Gravity factor applied to character.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = -10.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float GravityFactor
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -10.0f)
				{
					value = -10.0f;
				}

				SetArgument("gravityFactor", value);
			}
		}

		/// <summary>
		/// Swimming force applied to character as a function of handVelocity and footVelocity.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1000.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float Stroke
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < -1000.0f)
				{
					value = -1000.0f;
				}

				SetArgument("stroke", value);
			}
		}

		/// <summary>
		/// Swimming force (linearStroke=true,False) = (f(v),f(v*v)).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool LinearStroke
		{
			set
			{
				SetArgument("linearStroke", value);
			}
		}
	}

	/// <summary>
	/// SetCharacterCollisions:.
	/// </summary>
	public sealed class SetCharacterCollisionsHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the SetCharacterCollisionsHelper for sending a SetCharacterCollisions <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the SetCharacterCollisions <see cref="Message"/> to.</param>
		/// <remarks>
		/// SetCharacterCollisions:.
		/// </remarks>
		public SetCharacterCollisionsHelper(Ped ped) : base(ped, "setCharacterCollisions")
		{
		}

		/// <summary>
		/// Sliding friction turned into spin 80.0 (used in demo videos) good for rest of default params below.  If 0.0 then no collision enhancement.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float Spin
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spin", value);
			}
		}

		/// <summary>
		/// Torque = spin*(relative velocity) up to this maximum for relative velocity.
		/// </summary>
		/// <remarks>
		/// Default value = 8.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float MaxVelocity
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxVelocity", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ApplyToAll
		{
			set
			{
				SetArgument("applyToAll", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ApplyToSpine
		{
			set
			{
				SetArgument("applyToSpine", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ApplyToThighs
		{
			set
			{
				SetArgument("applyToThighs", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ApplyToClavicles
		{
			set
			{
				SetArgument("applyToClavicles", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ApplyToUpperArms
		{
			set
			{
				SetArgument("applyToUpperArms", value);
			}
		}

		/// <summary>
		/// Allow foot slipping if collided.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool FootSlip
		{
			set
			{
				SetArgument("footSlip", value);
			}
		}

		/// <summary>
		/// ClassType of the object against which to enhance the collision.  All character vehicle interaction (e.g. braceForImpact glancing spins) relies on this value so EDIT WISELY. If it is used for things other than vehicles then NM should be informed.
		/// </summary>
		/// <remarks>
		/// Default value = 15.
		/// Min value = 0.
		/// Max value = 100.
		/// </remarks>
		public int VehicleClass
		{
			set
			{
				if (value > 100)
				{
					value = 100;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("vehicleClass", value);
			}
		}
	}

	/// <summary>
	/// Damp out cartwheeling and somersaulting above a certain threshold.
	/// </summary>
	public sealed class SetCharacterDampingHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the SetCharacterDampingHelper for sending a SetCharacterDamping <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the SetCharacterDamping <see cref="Message"/> to.</param>
		/// <remarks>
		/// Damp out cartwheeling and somersaulting above a certain threshold.
		/// </remarks>
		public SetCharacterDampingHelper(Ped ped) : base(ped, "setCharacterDamping")
		{
		}

		/// <summary>
		/// Somersault AngularMomentum measure above which we start damping - try 34.0.  Falling over straight backwards gives 54 on hitting ground.
		/// </summary>
		/// <remarks>
		/// Default value = 34.0f.
		/// Min value = 0.0f.
		/// Max value = 200.0f.
		/// </remarks>
		public float SomersaultThresh
		{
			set
			{
				if (value > 200.0f)
				{
					value = 200.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("somersaultThresh", value);
			}
		}

		/// <summary>
		/// Amount to damp somersaulting by (spinning around left/right axis) - try 0.45.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float SomersaultDamp
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("somersaultDamp", value);
			}
		}

		/// <summary>
		/// Cartwheel AngularMomentum measure above which we start damping - try 27.0.
		/// </summary>
		/// <remarks>
		/// Default value = 27.0f.
		/// Min value = 0.0f.
		/// Max value = 200.0f.
		/// </remarks>
		public float CartwheelThresh
		{
			set
			{
				if (value > 200.0f)
				{
					value = 200.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("cartwheelThresh", value);
			}
		}

		/// <summary>
		/// Amount to damp somersaulting by (spinning around front/back axis) - try 0.8.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float CartwheelDamp
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("cartwheelDamp", value);
			}
		}

		/// <summary>
		/// Time after impact with a vehicle to apply characterDamping. -ve values mean always apply whether collided with vehicle or not. =0.0 never apply. =timestep apply for only that frame.  A typical roll from being hit by a car lasts about 4secs.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float VehicleCollisionTime
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("vehicleCollisionTime", value);
			}
		}

		/// <summary>
		/// If true damping is proportional to Angular momentum squared.  If false proportional to Angular momentum.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool V2
		{
			set
			{
				SetArgument("v2", value);
			}
		}
	}

	/// <summary>
	/// SetFrictionScale:.
	/// </summary>
	public sealed class SetFrictionScaleHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the SetFrictionScaleHelper for sending a SetFrictionScale <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the SetFrictionScale <see cref="Message"/> to.</param>
		/// <remarks>
		/// SetFrictionScale:.
		/// </remarks>
		public SetFrictionScaleHelper(Ped ped) : base(ped, "setFrictionScale")
		{
		}

		/// <summary>
		/// Friction scale to be applied to parts in mask.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float Scale
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("scale", value);
			}
		}

		/// <summary>
		/// Character-wide minimum impact friction. Affects all parts (not just those in mask).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1000000.0f.
		/// </remarks>
		public float GlobalMin
		{
			set
			{
				if (value > 1000000.0f)
				{
					value = 1000000.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("globalMin", value);
			}
		}

		/// <summary>
		/// Character-wide maximum impact friction. Affects all parts (not just those in mask).
		/// </summary>
		/// <remarks>
		/// Default value = 999999.0f.
		/// Min value = 0.0f.
		/// Max value = 1000000.0f.
		/// </remarks>
		public float GlobalMax
		{
			set
			{
				if (value > 1000000.0f)
				{
					value = 1000000.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("globalMax", value);
			}
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see Active Pose notes for possible values).
		/// </summary>
		/// <remarks>
		/// Default value = "fb".
		/// </remarks>
		public string Mask
		{
			set
			{
				SetArgument("mask", value);
			}
		}
	}

	public sealed class AnimPoseHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the AnimPoseHelper for sending a AnimPose <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the AnimPose <see cref="Message"/> to.</param>
		public AnimPoseHelper(Ped ped) : base(ped, "animPose")
		{
		}

		/// <summary>
		/// Muscle stiffness of masked joints. -values mean don't apply (just use defaults or ones applied by behaviors - safer if you are going to return to a behavior).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.1f.
		/// Max value = 10.0f.
		/// </remarks>
		public float MuscleStiffness
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -1.1f)
				{
					value = -1.1f;
				}

				SetArgument("muscleStiffness", value);
			}
		}

		/// <summary>
		/// Stiffness of masked joints. -ve values mean don't apply stiffness or damping (just use defaults or ones applied by behaviors).  If you are using animpose fullbody on its own then this gives the opprtunity to use setStffness and setMuscle stiffness messages to set up the character's muscles. Mmmmtodo get rid of this -ve.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.1f.
		/// Max value = 16.0f.
		/// </remarks>
		public float Stiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < -1.1f)
				{
					value = -1.1f;
				}

				SetArgument("stiffness", value);
			}
		}

		/// <summary>
		/// Damping of masked joints.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float Damping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("damping", value);
			}
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see notes for explanation).
		/// </summary>
		/// <remarks>
		/// Default value = "ub".
		/// </remarks>
		public string EffectorMask
		{
			set
			{
				SetArgument("effectorMask", value);
			}
		}

		/// <summary>
		/// Overide Headlook behavior (if animPose includes the head).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool OverideHeadlook
		{
			set
			{
				SetArgument("overideHeadlook", value);
			}
		}

		/// <summary>
		/// Overide PointArm behavior (if animPose includes the arm/arms).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool OveridePointArm
		{
			set
			{
				SetArgument("overidePointArm", value);
			}
		}

		/// <summary>
		/// Overide PointGun behavior (if animPose includes the arm/arms)//mmmmtodo not used at moment.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool OveridePointGun
		{
			set
			{
				SetArgument("overidePointGun", value);
			}
		}

		/// <summary>
		/// If true then modify gravity compensation based on stance (can reduce gravity compensation to zero if cofm is outside of balance area).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseZMPGravityCompensation
		{
			set
			{
				SetArgument("useZMPGravityCompensation", value);
			}
		}

		/// <summary>
		/// Gravity compensation applied to joints in the effectorMask. If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 14.0f.
		/// </remarks>
		public float GravityCompensation
		{
			set
			{
				if (value > 14.0f)
				{
					value = 14.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("gravityCompensation", value);
			}
		}

		/// <summary>
		/// Muscle stiffness applied to left arm (applied after stiffness). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float MuscleStiffnessLeftArm
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("muscleStiffnessLeftArm", value);
			}
		}

		/// <summary>
		/// Muscle stiffness applied to right arm (applied after stiffness). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float MuscleStiffnessRightArm
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("muscleStiffnessRightArm", value);
			}
		}

		/// <summary>
		/// Muscle stiffness applied to spine (applied after stiffness). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float MuscleStiffnessSpine
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("muscleStiffnessSpine", value);
			}
		}

		/// <summary>
		/// Muscle stiffness applied to left leg (applied after stiffness). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float MuscleStiffnessLeftLeg
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("muscleStiffnessLeftLeg", value);
			}
		}

		/// <summary>
		/// Muscle stiffness applied to right leg (applied after stiffness). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float MuscleStiffnessRightLeg
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("muscleStiffnessRightLeg", value);
			}
		}

		/// <summary>
		/// Stiffness  applied to left arm (applied after stiffness). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float StiffnessLeftArm
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("stiffnessLeftArm", value);
			}
		}

		/// <summary>
		/// Stiffness applied to right arm (applied after stiffness). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float StiffnessRightArm
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("stiffnessRightArm", value);
			}
		}

		/// <summary>
		/// Stiffness applied to spine (applied after stiffness). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float StiffnessSpine
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("stiffnessSpine", value);
			}
		}

		/// <summary>
		/// Stiffness applied to left leg (applied after stiffness). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float StiffnessLeftLeg
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("stiffnessLeftLeg", value);
			}
		}

		/// <summary>
		/// Stiffness applied to right leg (applied after stiffness). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float StiffnessRightLeg
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("stiffnessRightLeg", value);
			}
		}

		/// <summary>
		/// Damping applied to left arm (applied after stiffness). If stiffness -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float DampingLeftArm
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dampingLeftArm", value);
			}
		}

		/// <summary>
		/// Damping applied to right arm (applied after stiffness). If stiffness -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float DampingRightArm
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dampingRightArm", value);
			}
		}

		/// <summary>
		/// Damping applied to spine (applied after stiffness). If stiffness-ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float DampingSpine
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dampingSpine", value);
			}
		}

		/// <summary>
		/// Damping applied to left leg (applied after stiffness). If stiffness-ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float DampingLeftLeg
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dampingLeftLeg", value);
			}
		}

		/// <summary>
		/// Damping applied to right leg (applied after stiffness). If stiffness -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float DampingRightLeg
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dampingRightLeg", value);
			}
		}

		/// <summary>
		/// Gravity compensation applied to left arm (applied after gravityCompensation). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 14.0f.
		/// </remarks>
		public float GravCompLeftArm
		{
			set
			{
				if (value > 14.0f)
				{
					value = 14.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("gravCompLeftArm", value);
			}
		}

		/// <summary>
		/// Gravity compensation applied to right arm (applied after gravityCompensation). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 14.0f.
		/// </remarks>
		public float GravCompRightArm
		{
			set
			{
				if (value > 14.0f)
				{
					value = 14.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("gravCompRightArm", value);
			}
		}

		/// <summary>
		/// Gravity compensation applied to spine (applied after gravityCompensation). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 14.0f.
		/// </remarks>
		public float GravCompSpine
		{
			set
			{
				if (value > 14.0f)
				{
					value = 14.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("gravCompSpine", value);
			}
		}

		/// <summary>
		/// Gravity compensation applied to left leg (applied after gravityCompensation). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 14.0f.
		/// </remarks>
		public float GravCompLeftLeg
		{
			set
			{
				if (value > 14.0f)
				{
					value = 14.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("gravCompLeftLeg", value);
			}
		}

		/// <summary>
		/// Gravity compensation applied to right leg (applied after gravityCompensation). If -ve then not applied (use current setting).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 14.0f.
		/// </remarks>
		public float GravCompRightLeg
		{
			set
			{
				if (value > 14.0f)
				{
					value = 14.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("gravCompRightLeg", value);
			}
		}

		/// <summary>
		/// Is the left hand constrained to the world/ an object: -1=auto decide by impact info, 0=no, 1=part fully constrained (not implemented:, 2=part point constraint, 3=line constraint).
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = -1.
		/// Max value = 2.
		/// </remarks>
		public int ConnectedLeftHand
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < -1)
				{
					value = -1;
				}

				SetArgument("connectedLeftHand", value);
			}
		}

		/// <summary>
		/// Is the right hand constrained to the world/ an object: -1=auto decide by impact info, 0=no, 1=part fully constrained (not implemented:, 2=part point constraint, 3=line constraint).
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = -1.
		/// Max value = 2.
		/// </remarks>
		public int ConnectedRightHand
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < -1)
				{
					value = -1;
				}

				SetArgument("connectedRightHand", value);
			}
		}

		/// <summary>
		/// Is the left foot constrained to the world/ an object: -2=do not set in animpose (e.g. let the balancer decide), -1=auto decide by impact info, 0=no, 1=part fully constrained (not implemented:, 2=part point constraint, 3=line constraint).
		/// </summary>
		/// <remarks>
		/// Default value = -2.
		/// Min value = -2.
		/// Max value = 2.
		/// </remarks>
		public int ConnectedLeftFoot
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < -2)
				{
					value = -2;
				}

				SetArgument("connectedLeftFoot", value);
			}
		}

		/// <summary>
		/// Is the right foot constrained to the world/ an object: -2=do not set in animpose (e.g. let the balancer decide),-1=auto decide by impact info, 0=no, 1=part fully constrained (not implemented:, 2=part point constraint, 3=line constraint).
		/// </summary>
		/// <remarks>
		/// Default value = -2.
		/// Min value = -2.
		/// Max value = 2.
		/// </remarks>
		public int ConnectedRightFoot
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < -2)
				{
					value = -2;
				}

				SetArgument("connectedRightFoot", value);
			}
		}

		/// <summary>
		/// </summary>
		public AnimSource AnimSource
		{
			set
			{
				SetArgument("animSource", (int)value);
			}
		}

		/// <summary>
		/// LevelIndex of object to dampen side motion relative to. -1 means not used.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int DampenSideMotionInstanceIndex
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("dampenSideMotionInstanceIndex", value);
			}
		}
	}

	public sealed class ArmsWindmillHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ArmsWindmillHelper for sending a ArmsWindmill <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ArmsWindmill <see cref="Message"/> to.</param>
		public ArmsWindmillHelper(Ped ped) : base(ped, "armsWindmill")
		{
		}

		/// <summary>
		/// ID of part that the circle uses as local space for positioning.
		/// </summary>
		/// <remarks>
		/// Default value = 10.
		/// Min value = 0.
		/// Max value = 21.
		/// </remarks>
		public int LeftPartID
		{
			set
			{
				if (value > 21)
				{
					value = 21;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("leftPartID", value);
			}
		}

		/// <summary>
		/// Radius for first axis of ellipse.
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeftRadius1
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leftRadius1", value);
			}
		}

		/// <summary>
		/// Radius for second axis of ellipse.
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeftRadius2
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leftRadius2", value);
			}
		}

		/// <summary>
		/// Speed of target around the circle.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = -2.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float LeftSpeed
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < -2.0f)
				{
					value = -2.0f;
				}

				SetArgument("leftSpeed", value);
			}
		}

		/// <summary>
		/// Euler Angles orientation of circle in space of part with part ID.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.2f, 0.2f).
		/// </remarks>
		public Vector3 LeftNormal
		{
			set
			{
				SetArgument("leftNormal", value);
			}
		}

		/// <summary>
		/// Centre of circle in the space of partID.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.5f, -0.1f).
		/// </remarks>
		public Vector3 LeftCentre
		{
			set
			{
				SetArgument("leftCentre", value);
			}
		}

		/// <summary>
		/// ID of part that the circle uses as local space for positioning.
		/// </summary>
		/// <remarks>
		/// Default value = 10.
		/// Min value = 0.
		/// Max value = 21.
		/// </remarks>
		public int RightPartID
		{
			set
			{
				if (value > 21)
				{
					value = 21;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("rightPartID", value);
			}
		}

		/// <summary>
		/// Radius for first axis of ellipse.
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RightRadius1
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rightRadius1", value);
			}
		}

		/// <summary>
		/// Radius for second axis of ellipse.
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RightRadius2
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rightRadius2", value);
			}
		}

		/// <summary>
		/// Speed of target around the circle.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = -2.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RightSpeed
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < -2.0f)
				{
					value = -2.0f;
				}

				SetArgument("rightSpeed", value);
			}
		}

		/// <summary>
		/// Euler Angles orientation of circle in space of part with part ID.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, -0.2f, -0.2f).
		/// </remarks>
		public Vector3 RightNormal
		{
			set
			{
				SetArgument("rightNormal", value);
			}
		}

		/// <summary>
		/// Centre of circle in the space of partID.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, -0.5f, -0.1f).
		/// </remarks>
		public Vector3 RightCentre
		{
			set
			{
				SetArgument("rightCentre", value);
			}
		}

		/// <summary>
		/// Stiffness applied to the shoulders.
		/// </summary>
		/// <remarks>
		/// Default value = 12.0f.
		/// Min value = 1.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ShoulderStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 1.0f)
				{
					value = 1.0f;
				}

				SetArgument("shoulderStiffness", value);
			}
		}

		/// <summary>
		/// Damping applied to the shoulders.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ShoulderDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("shoulderDamping", value);
			}
		}

		/// <summary>
		/// Stiffness applied to the elbows.
		/// </summary>
		/// <remarks>
		/// Default value = 12.0f.
		/// Min value = 1.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ElbowStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 1.0f)
				{
					value = 1.0f;
				}

				SetArgument("elbowStiffness", value);
			}
		}

		/// <summary>
		/// Damping applied to the elbows.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ElbowDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("elbowDamping", value);
			}
		}

		/// <summary>
		/// Minimum left elbow bend.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.7f.
		/// </remarks>
		public float LeftElbowMin
		{
			set
			{
				if (value > 1.7f)
				{
					value = 1.7f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leftElbowMin", value);
			}
		}

		/// <summary>
		/// Minimum right elbow bend.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.7f.
		/// </remarks>
		public float RightElbowMin
		{
			set
			{
				if (value > 1.7f)
				{
					value = 1.7f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rightElbowMin", value);
			}
		}

		/// <summary>
		/// Phase offset(degrees) when phase synchronization is turned on.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -360.0f.
		/// Max value = 360.0f.
		/// </remarks>
		public float PhaseOffset
		{
			set
			{
				if (value > 360.0f)
				{
					value = 360.0f;
				}

				if (value < -360.0f)
				{
					value = -360.0f;
				}

				SetArgument("phaseOffset", value);
			}
		}

		/// <summary>
		/// How much to compensate for movement of character/target.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float DragReduction
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dragReduction", value);
			}
		}

		/// <summary>
		/// Angle of elbow around twist axis ?.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -3.1f.
		/// Max value = 3.1f.
		/// </remarks>
		public float IKtwist
		{
			set
			{
				if (value > 3.1f)
				{
					value = 3.1f;
				}

				if (value < -3.1f)
				{
					value = -3.1f;
				}

				SetArgument("IKtwist", value);
			}
		}

		/// <summary>
		/// Value of character angular speed above which adaptive arm motion starts.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float AngVelThreshold
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("angVelThreshold", value);
			}
		}

		/// <summary>
		/// Multiplies angular speed of character to get speed of arms.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float AngVelGain
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("angVelGain", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="MirrorMode.Mirrored"/>.
		/// If <see cref="MirrorMode.Parallel"/> leftArm parameters are used.
		/// </remarks>
		public MirrorMode MirrorMode
		{
			set
			{
				SetArgument("mirrorMode", (int)value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="AdaptiveMode.NotAdaptive"/>.
		/// </remarks>
		public AdaptiveMode AdaptiveMode
		{
			set
			{
				SetArgument("adaptiveMode", (int)value);
			}
		}

		/// <summary>
		/// Toggles phase synchronization.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ForceSync
		{
			set
			{
				SetArgument("forceSync", value);
			}
		}

		/// <summary>
		/// Use the left arm.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseLeft
		{
			set
			{
				SetArgument("useLeft", value);
			}
		}

		/// <summary>
		/// Use the right arm.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseRight
		{
			set
			{
				SetArgument("useRight", value);
			}
		}

		/// <summary>
		/// If true, each arm will stop windmilling if it hits the ground.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool DisableOnImpact
		{
			set
			{
				SetArgument("disableOnImpact", value);
			}
		}
	}

	public sealed class ArmsWindmillAdaptiveHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ArmsWindmillAdaptiveHelper for sending a ArmsWindmillAdaptive <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ArmsWindmillAdaptive <see cref="Message"/> to.</param>
		public ArmsWindmillAdaptiveHelper(Ped ped) : base(ped, "armsWindmillAdaptive")
		{
		}

		/// <summary>
		/// Controls the speed of the windmilling.
		/// </summary>
		/// <remarks>
		/// Default value = 6.3f.
		/// Min value = 0.1f.
		/// Max value = 10.0f.
		/// </remarks>
		public float AngSpeed
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("angSpeed", value);
			}
		}

		/// <summary>
		/// Controls how stiff the rest of the body is.
		/// </summary>
		/// <remarks>
		/// Default value = 11.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float BodyStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("bodyStiffness", value);
			}
		}

		/// <summary>
		/// Controls how large the motion is, higher values means the character waves his arms in a massive arc.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float Amplitude
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("amplitude", value);
			}
		}

		/// <summary>
		/// Set to a non-zero value to desynchronize the left and right arms motion.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -4.0f.
		/// Max value = 8.0f.
		/// </remarks>
		public float Phase
		{
			set
			{
				if (value > 8.0f)
				{
					value = 8.0f;
				}

				if (value < -4.0f)
				{
					value = -4.0f;
				}

				SetArgument("phase", value);
			}
		}

		/// <summary>
		/// How stiff the arms are controls how pronounced the windmilling motion appears.
		/// </summary>
		/// <remarks>
		/// Default value = 14.1f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ArmStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("armStiffness", value);
			}
		}

		/// <summary>
		/// If not negative then left arm will blend to this angle.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 6.0f.
		/// </remarks>
		public float LeftElbowAngle
		{
			set
			{
				if (value > 6.0f)
				{
					value = 6.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("leftElbowAngle", value);
			}
		}

		/// <summary>
		/// If not negative then right arm will blend to this angle.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 6.0f.
		/// </remarks>
		public float RightElbowAngle
		{
			set
			{
				if (value > 6.0f)
				{
					value = 6.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("rightElbowAngle", value);
			}
		}

		/// <summary>
		/// 0 arms go up and down at the side. 1 circles. 0..1 elipse.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float Lean1mult
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("lean1mult", value);
			}
		}

		/// <summary>
		/// 0.f center of circle at side.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -6.0f.
		/// Max value = 6.0f.
		/// </remarks>
		public float Lean1offset
		{
			set
			{
				if (value > 6.0f)
				{
					value = 6.0f;
				}

				if (value < -6.0f)
				{
					value = -6.0f;
				}

				SetArgument("lean1offset", value);
			}
		}

		/// <summary>
		/// Rate at which elbow tries to match *ElbowAngle.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 6.0f.
		/// </remarks>
		public float ElbowRate
		{
			set
			{
				if (value > 6.0f)
				{
					value = 6.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("elbowRate", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="ArmDirection.Adaptive"/>.
		/// </remarks>
		public ArmDirection ArmDirection
		{
			set
			{
				SetArgument("armDirection", (int)value);
			}
		}

		/// <summary>
		/// If true, each arm will stop windmilling if it hits the ground.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool DisableOnImpact
		{
			set
			{
				SetArgument("disableOnImpact", value);
			}
		}

		/// <summary>
		/// If true, back angles will be set to compliment arms windmill.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool SetBackAngles
		{
			set
			{
				SetArgument("setBackAngles", value);
			}
		}

		/// <summary>
		/// If true, use angular momentum about com to choose arm circling direction. Otherwise use com angular velocity.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseAngMom
		{
			set
			{
				SetArgument("useAngMom", value);
			}
		}

		/// <summary>
		/// If true, bend the left elbow to give a stunt man type scramble look.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool BendLeftElbow
		{
			set
			{
				SetArgument("bendLeftElbow", value);
			}
		}

		/// <summary>
		/// If true, bend the right elbow to give a stunt man type scramble look.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool BendRightElbow
		{
			set
			{
				SetArgument("bendRightElbow", value);
			}
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see Active Pose notes for possible values).
		/// </summary>
		/// <remarks>
		/// Default value = "ub".
		/// </remarks>
		public string Mask
		{
			set
			{
				SetArgument("mask", value);
			}
		}
	}

	public sealed class BalancerCollisionsReactionHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the BalancerCollisionsReactionHelper for sending a BalancerCollisionsReaction <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the BalancerCollisionsReaction <see cref="Message"/> to.</param>
		public BalancerCollisionsReactionHelper(Ped ped) : base(ped, "balancerCollisionsReaction")
		{
		}

		/// <summary>
		/// Begin slump and stop stepping after this many steps.
		/// </summary>
		/// <remarks>
		/// Default value = 4.
		/// Min value = 0.
		/// </remarks>
		public int NumStepsTillSlump
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				SetArgument("numStepsTillSlump", value);
			}
		}

		/// <summary>
		/// Time after becoming stable leaning against a wall that slump starts.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float Stable2SlumpTime
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stable2SlumpTime", value);
			}
		}

		/// <summary>
		/// Steps are ihibited to not go closer to the wall than this (after impact).
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ExclusionZone
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("exclusionZone", value);
			}
		}

		/// <summary>
		/// Friction multiplier applied to feet when slump starts.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float FootFrictionMultStart
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("footFrictionMultStart", value);
			}
		}

		/// <summary>
		/// Friction multiplier reduced by this amount every second after slump starts (only if character is not slumping).
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 50.0f.
		/// </remarks>
		public float FootFrictionMultRate
		{
			set
			{
				if (value > 50.0f)
				{
					value = 50.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("footFrictionMultRate", value);
			}
		}

		/// <summary>
		/// Friction multiplier applied to back when slump starts.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float BackFrictionMultStart
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("backFrictionMultStart", value);
			}
		}

		/// <summary>
		/// Friction multiplier reduced by this amount every second after slump starts (only if character is not slumping).
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 50.0f.
		/// </remarks>
		public float BackFrictionMultRate
		{
			set
			{
				if (value > 50.0f)
				{
					value = 50.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("backFrictionMultRate", value);
			}
		}

		/// <summary>
		/// Reduce the stiffness of the legs by this much as soon as an impact is detected.
		/// </summary>
		/// <remarks>
		/// Default value = 3.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ImpactLegStiffReduction
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impactLegStiffReduction", value);
			}
		}

		/// <summary>
		/// Reduce the stiffness of the legs by this much as soon as slump starts.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float SlumpLegStiffReduction
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("slumpLegStiffReduction", value);
			}
		}

		/// <summary>
		/// Rate at which the stiffness of the legs is reduced during slump.
		/// </summary>
		/// <remarks>
		/// Default value = 8.0f.
		/// Min value = 0.0f.
		/// Max value = 50.0f.
		/// </remarks>
		public float SlumpLegStiffRate
		{
			set
			{
				if (value > 50.0f)
				{
					value = 50.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("slumpLegStiffRate", value);
			}
		}

		/// <summary>
		/// Time that the character reacts to the impact with ub flinch and writhe.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ReactTime
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("reactTime", value);
			}
		}

		/// <summary>
		/// Time that the character exaggerates impact with spine.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ImpactExagTime
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impactExagTime", value);
			}
		}

		/// <summary>
		/// Duration that the glance torque is applied for.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float GlanceSpinTime
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("glanceSpinTime", value);
			}
		}

		/// <summary>
		/// Magnitude of the glance torque.
		/// </summary>
		/// <remarks>
		/// Default value = 50.0f.
		/// Min value = 0.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float GlanceSpinMag
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("glanceSpinMag", value);
			}
		}

		/// <summary>
		/// Multiplier used when decaying torque spin over time.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float GlanceSpinDecayMult
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("glanceSpinDecayMult", value);
			}
		}

		/// <summary>
		/// Used so impact with the character that is pushing you over doesn't set off the behavior.
		/// </summary>
		/// <remarks>
		/// Default value = -2.
		/// Min value = -2.
		/// </remarks>
		public int IgnoreColWithIndex
		{
			set
			{
				if (value < -2)
				{
					value = -2;
				}

				SetArgument("ignoreColWithIndex", value);
			}
		}

		/// <summary>
		/// 0=Normal slump(less movement then slump and movement LT small), 1=fast slump, 2=less movement then slump.
		/// </summary>
		/// <remarks>
		/// Default value = 1.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int SlumpMode
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("slumpMode", value);
			}
		}

		/// <summary>
		/// 0=fall2knees/slump if shot not running, 1=stumble, 2=slump, 3=restart.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 3.
		/// </remarks>
		public int ReboundMode
		{
			set
			{
				if (value > 3)
				{
					value = 3;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("reboundMode", value);
			}
		}

		/// <summary>
		/// Collisions with non-fixed objects with mass below this will not set this behavior off (e.g. ignore guns).
		/// </summary>
		/// <remarks>
		/// Default value = 20.0f.
		/// Min value = -1.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float IgnoreColMassBelow
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("ignoreColMassBelow", value);
			}
		}

		/// <summary>
		/// 0=slump, 1=fallToKnees if shot is running, otherwise slump.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 1.
		/// </remarks>
		public int ForwardMode
		{
			set
			{
				if (value > 1)
				{
					value = 1;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("forwardMode", value);
			}
		}

		/// <summary>
		/// Time after a forwards impact before forwardMode is called (leave sometime for a rebound or brace - the min of 0.1 is to ensure fallOverWall can start although it probably needs only 1or2 frames for the probes to return).
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.1f.
		/// Max value = 2.0f.
		/// </remarks>
		public float TimeToForward
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("timeToForward", value);
			}
		}

		/// <summary>
		/// If forwards impact only: cheat force to try to get the character away from the wall.  3 is a good value.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ReboundForce
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("reboundForce", value);
			}
		}

		/// <summary>
		/// Brace against wall if forwards impact(at the moment only if bodyBalance is running/in charge of arms).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool BraceWall
		{
			set
			{
				SetArgument("braceWall", value);
			}
		}

		/// <summary>
		/// Collisions with non-fixed objects with volume below this will not set this behavior off.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = -1.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float IgnoreColVolumeBelow
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("ignoreColVolumeBelow", value);
			}
		}

		/// <summary>
		/// Use fallOverWall as the main drape reaction.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool FallOverWallDrape
		{
			set
			{
				SetArgument("fallOverWallDrape", value);
			}
		}

		/// <summary>
		/// Trigger fall over wall if hit up to spine2 else only if hit up to spine1.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FallOverHighWalls
		{
			set
			{
				SetArgument("fallOverHighWalls", value);
			}
		}

		/// <summary>
		/// Add a Snap to when you hit a wall to emphasize the hit.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Snap
		{
			set
			{
				SetArgument("snap", value);
			}
		}

		/// <summary>
		/// The magnitude of the snap reaction.
		/// </summary>
		/// <remarks>
		/// Default value = -0.6f.
		/// Min value = -10.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SnapMag
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -10.0f)
				{
					value = -10.0f;
				}

				SetArgument("snapMag", value);
			}
		}

		/// <summary>
		/// The character snaps in a prescribed way (decided by bullet direction) - Higher the value the more random this direction is.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SnapDirectionRandomness
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("snapDirectionRandomness", value);
			}
		}

		/// <summary>
		/// Snap the leftArm.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SnapLeftArm
		{
			set
			{
				SetArgument("snapLeftArm", value);
			}
		}

		/// <summary>
		/// Snap the rightArm.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SnapRightArm
		{
			set
			{
				SetArgument("snapRightArm", value);
			}
		}

		/// <summary>
		/// Snap the leftLeg.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SnapLeftLeg
		{
			set
			{
				SetArgument("snapLeftLeg", value);
			}
		}

		/// <summary>
		/// Snap the rightLeg.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SnapRightLeg
		{
			set
			{
				SetArgument("snapRightLeg", value);
			}
		}

		/// <summary>
		/// Snap the spine.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool SnapSpine
		{
			set
			{
				SetArgument("snapSpine", value);
			}
		}

		/// <summary>
		/// Snap the neck.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool SnapNeck
		{
			set
			{
				SetArgument("snapNeck", value);
			}
		}

		/// <summary>
		/// Legs are either in phase with each other or not.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool SnapPhasedLegs
		{
			set
			{
				SetArgument("snapPhasedLegs", value);
			}
		}

		/// <summary>
		/// Type of hip reaction 0=none, 1=side2side 2=steplike.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int SnapHipType
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("snapHipType", value);
			}
		}

		/// <summary>
		/// Interval before applying reverse snap.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float UnSnapInterval
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("unSnapInterval", value);
			}
		}

		/// <summary>
		/// The magnitude of the reverse snap.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float UnSnapRatio
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("unSnapRatio", value);
			}
		}

		/// <summary>
		/// Use torques to make the snap otherwise use a change in the parts angular velocity.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool SnapUseTorques
		{
			set
			{
				SetArgument("snapUseTorques", value);
			}
		}

		/// <summary>
		/// Duration for which the character's upper body stays at minimum stiffness (not quite zero).
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ImpactWeaknessZeroDuration
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impactWeaknessZeroDuration", value);
			}
		}

		/// <summary>
		/// Duration of the ramp to bring the character's upper body stiffness back to normal levels.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ImpactWeaknessRampDuration
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impactWeaknessRampDuration", value);
			}
		}

		/// <summary>
		/// How loose the character is on impact. Between 0 and 1.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ImpactLoosenessAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("impactLoosenessAmount", value);
			}
		}

		/// <summary>
		/// Detected an object behind a shot victim in the direction of a bullet?.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ObjectBehindVictim
		{
			set
			{
				SetArgument("objectBehindVictim", value);
			}
		}

		/// <summary>
		/// The intersection pos of a detected object behind a shot victim in the direction of a bullet.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 ObjectBehindVictimPos
		{
			set
			{
				SetArgument("objectBehindVictimPos", value);
			}
		}

		/// <summary>
		/// The normal of a detected object behind a shot victim in the direction of a bullet.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public Vector3 ObjectBehindVictimNormal
		{
			set
			{
				SetArgument("objectBehindVictimNormal",
					Vector3.Clamp(value, new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f)));
			}
		}
	}

	public sealed class BodyBalanceHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the BodyBalanceHelper for sending a BodyBalance <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the BodyBalance <see cref="Message"/> to.</param>
		public BodyBalanceHelper(Ped ped) : base(ped, "bodyBalance")
		{
		}

		/// <summary>
		/// NB. WAS m_bodyStiffness ClaviclesStiffness=9.0f.
		/// </summary>
		/// <remarks>
		/// Default value = 9.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ArmStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("armStiffness", value);
			}
		}

		/// <summary>
		/// How much the elbow swings based on the leg movement.
		/// </summary>
		/// <remarks>
		/// Default value = 0.9f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float Elbow
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("elbow", value);
			}
		}

		/// <summary>
		/// How much the shoulder(lean1) swings based on the leg movement.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float Shoulder
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("shoulder", value);
			}
		}

		/// <summary>
		/// NB. WAS m_damping NeckDamping=1 ClaviclesDamping=1.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armDamping", value);
			}
		}

		/// <summary>
		/// Enable and provide a look-at target to make the character's head turn to face it while balancing.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseHeadLook
		{
			set
			{
				SetArgument("useHeadLook", value);
			}
		}

		/// <summary>
		/// Position of thing to look at.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 HeadLookPos
		{
			set
			{
				SetArgument("headLookPos", value);
			}
		}

		/// <summary>
		/// Level index of thing to look at.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int HeadLookInstanceIndex
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("headLookInstanceIndex", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float SpineStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("spineStiffness", value);
			}
		}

		/// <summary>
		/// Multiplier of the somersault 'angle' (lean forward/back) for arms out (lean2).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float SomersaultAngle
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("somersaultAngle", value);
			}
		}

		/// <summary>
		/// Amount of somersault 'angle' before m_somersaultAngle is used for ArmsOut. Unless drunk - DO NOT EXCEED 0.8.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SomersaultAngleThreshold
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("somersaultAngleThreshold", value);
			}
		}

		/// <summary>
		/// Amount of side somersault 'angle' before sideSomersault is used for ArmsOut. Unless drunk - DO NOT EXCEED 0.8.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SideSomersaultAngle
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sideSomersaultAngle", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SideSomersaultAngleThreshold
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sideSomersaultAngleThreshold", value);
			}
		}

		/// <summary>
		/// Automatically turn around if moving backwards.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool BackwardsAutoTurn
		{
			set
			{
				SetArgument("backwardsAutoTurn", value);
			}
		}

		/// <summary>
		/// 0.9 is a sensible value.  If pusher within this distance then turn to get out of the way of the pusher.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float TurnWithBumpRadius
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("turnWithBumpRadius", value);
			}
		}

		/// <summary>
		/// Bend elbows, relax shoulders and inhibit spine twist when moving backwards.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool BackwardsArms
		{
			set
			{
				SetArgument("backwardsArms", value);
			}
		}

		/// <summary>
		/// Blend upper body to zero pose as the character comes to rest. If false blend to a stored pose.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool BlendToZeroPose
		{
			set
			{
				SetArgument("blendToZeroPose", value);
			}
		}

		/// <summary>
		/// Put arms out based on lean2 of legs, or angular velocity (lean or twist), or lean (front/back or side/side).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ArmsOutOnPush
		{
			set
			{
				SetArgument("armsOutOnPush", value);
			}
		}

		/// <summary>
		/// Arms out based on lean2 of the legs to simulate being pushed.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmsOutOnPushMultiplier
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armsOutOnPushMultiplier", value);
			}
		}

		/// <summary>
		/// Number of seconds before turning off the armsOutOnPush response only for Arms out based on lean2 of the legs (NOT for the angle or angular velocity).
		/// </summary>
		/// <remarks>
		/// Default value = 1.1f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmsOutOnPushTimeout
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armsOutOnPushTimeout", value);
			}
		}

		/// <summary>
		/// Range 0:1 0 = don't raise arms if returning to upright position, 0.x = 0.x*raise arms based on angvel and 'angle' settings, 1 = raise arms based on angvel and 'angle' settings.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ReturningToBalanceArmsOut
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("returningToBalanceArmsOut", value);
			}
		}

		/// <summary>
		/// Multiplier for straightening the elbows based on the amount of arms out(lean2) 0 = dont straighten elbows. Otherwise straighten elbows proportionately to armsOut.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ArmsOutStraightenElbows
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armsOutStraightenElbows", value);
			}
		}

		/// <summary>
		/// Minimum desiredLean2 applied to shoulder (to stop arms going above shoulder height or not).
		/// </summary>
		/// <remarks>
		/// Default value = -9.9f.
		/// Min value = -10.0f.
		/// Max value = 0.0f.
		/// </remarks>
		public float ArmsOutMinLean2
		{
			set
			{
				if (value > 0.0f)
				{
					value = 0.0f;
				}

				if (value < -10.0f)
				{
					value = -10.0f;
				}

				SetArgument("armsOutMinLean2", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float SpineDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spineDamping", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseBodyTurn
		{
			set
			{
				SetArgument("useBodyTurn", value);
			}
		}

		/// <summary>
		/// On contact with upperbody the desired elbow angle is set to at least this value.
		/// </summary>
		/// <remarks>
		/// Default value = 1.9f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float ElbowAngleOnContact
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("elbowAngleOnContact", value);
			}
		}

		/// <summary>
		/// Time after contact (with Upper body) that the min m_elbowAngleOnContact is applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float BendElbowsTime
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("bendElbowsTime", value);
			}
		}

		/// <summary>
		/// Minimum desired angle of elbow during non contact arm swing.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = -3.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float BendElbowsGait
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < -3.0f)
				{
					value = -3.0f;
				}

				SetArgument("bendElbowsGait", value);
			}
		}

		/// <summary>
		/// Mmmmdrunk = 0.2 multiplier of hip lean2 (star jump) to give shoulder lean2 (flapping).
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float HipL2ArmL2
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("hipL2ArmL2", value);
			}
		}

		/// <summary>
		/// Mmmmdrunk = 0.7 shoulder lean2 offset.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = -3.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float ShoulderL2
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < -3.0f)
				{
					value = -3.0f;
				}

				SetArgument("shoulderL2", value);
			}
		}

		/// <summary>
		/// Mmmmdrunk 1.1 shoulder lean1 offset (+ve frankenstein).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ShoulderL1
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("shoulderL1", value);
			}
		}

		/// <summary>
		/// Mmmmdrunk = 0.0 shoulder twist.
		/// </summary>
		/// <remarks>
		/// Default value = -0.4f.
		/// Min value = -3.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float ShoulderTwist
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < -3.0f)
				{
					value = -3.0f;
				}

				SetArgument("shoulderTwist", value);
			}
		}

		/// <summary>
		/// Probability [0-1] that headLook will be looking in the direction of velocity when stepping.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float HeadLookAtVelProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("headLookAtVelProb", value);
			}
		}

		/// <summary>
		/// Weighted probability that turn will be off. This is one of six turn type weights.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TurnOffProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("turnOffProb", value);
			}
		}

		/// <summary>
		/// Weighted probability of turning towards velocity. This is one of six turn type weights.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Turn2VelProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("turn2VelProb", value);
			}
		}

		/// <summary>
		/// Weighted probability of turning away from headLook target. This is one of six turn type weights.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TurnAwayProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("turnAwayProb", value);
			}
		}

		/// <summary>
		/// Weighted probability of turning left. This is one of six turn type weights.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TurnLeftProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("turnLeftProb", value);
			}
		}

		/// <summary>
		/// Weighted probability of turning right. This is one of six turn type weights.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TurnRightProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("turnRightProb", value);
			}
		}

		/// <summary>
		/// Weighted probability of turning towards headLook target. This is one of six turn type weights.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Turn2TargetProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("turn2TargetProb", value);
			}
		}

		/// <summary>
		/// Somersault, twist, sideSomersault) multiplier of the angular velocity  for arms out (lean2) (somersault, twist, sideSomersault).
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(4.0f, 1.0f, 4.0f).
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public Vector3 AngVelMultiplier
		{
			set
			{
				SetArgument("angVelMultiplier",
					Vector3.Clamp(value, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(20.0f, 20.0f, 20.0f)));
			}
		}

		/// <summary>
		/// Somersault, twist, sideSomersault) threshold above which angVel is used for arms out (lean2) Unless drunk - DO NOT EXCEED 7.0 for each component.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(1.2f, 3.0f, 1.2f).
		/// Min value = 0.0f.
		/// Max value = 40.0f.
		/// </remarks>
		public Vector3 AngVelThreshold
		{
			set
			{
				SetArgument("angVelThreshold", Vector3.Clamp(value, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(40.0f, 40.0f, 40.0f)));
			}
		}

		/// <summary>
		/// If -ve then do not brace.  distance from object at which to raise hands to brace 0.5 good if newBrace=true - otherwise 0.65.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float BraceDistance
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("braceDistance", value);
			}
		}

		/// <summary>
		/// Time expected to get arms up from idle.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TargetPredictionTime
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("targetPredictionTime", value);
			}
		}

		/// <summary>
		/// Larger values and he absorbs the impact more.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ReachAbsorbtionTime
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("reachAbsorbtionTime", value);
			}
		}

		/// <summary>
		/// Stiffness of character. Catch_fall stiffness scales with this too, with its defaults at this values default.
		/// </summary>
		/// <remarks>
		/// Default value = 12.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float BraceStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("braceStiffness", value);
			}
		}

		/// <summary>
		/// Minimum bracing time so the character doesn't look twitchy.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float MinBraceTime
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("minBraceTime", value);
			}
		}

		/// <summary>
		/// Time before arm brace kicks in when hit from behind.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float TimeToBackwardsBrace
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("timeToBackwardsBrace", value);
			}
		}

		/// <summary>
		/// If bracing with 2 hands delay one hand by at least this amount of time to introduce some asymmetry.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float HandsDelayMin
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("handsDelayMin", value);
			}
		}

		/// <summary>
		/// If bracing with 2 hands delay one hand by at most this amount of time to introduce some asymmetry.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float HandsDelayMax
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("handsDelayMax", value);
			}
		}

		/// <summary>
		/// BraceTarget is global headLookPos plus braceOffset m in the up direction.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -2.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float BraceOffset
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < -2.0f)
				{
					value = -2.0f;
				}

				SetArgument("braceOffset", value);
			}
		}

		/// <summary>
		/// If -ve don't move away from pusher unless moveWhenBracing is true and braceDistance  GT  0.0f.  if the pusher is closer than moveRadius then move away from it.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float MoveRadius
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("moveRadius", value);
			}
		}

		/// <summary>
		/// Amount of leanForce applied away from pusher.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float MoveAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("moveAmount", value);
			}
		}

		/// <summary>
		/// Only move away from pusher when bracing against pusher.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool MoveWhenBracing
		{
			set
			{
				SetArgument("moveWhenBracing", value);
			}
		}
	}

	public sealed class BodyFoetalHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the BodyFoetalHelper for sending a BodyFoetal <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the BodyFoetal <see cref="Message"/> to.</param>
		public BodyFoetalHelper(Ped ped) : base(ped, "bodyFoetal")
		{
		}

		/// <summary>
		/// The stiffness of the body determines how fast the character moves into the position, and how well that they hold it.
		/// </summary>
		/// <remarks>
		/// Default value = 9.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float Stiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("stiffness", value);
			}
		}

		/// <summary>
		/// Sets damping value for the character joints.
		/// </summary>
		/// <remarks>
		/// Default value = 1.4f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float DampingFactor
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dampingFactor", value);
			}
		}

		/// <summary>
		/// A value between 0-1 that controls how asymmetric the results are by varying stiffness across the body.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Asymmetry
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("asymmetry", value);
			}
		}

		/// <summary>
		/// Random seed used to generate asymmetry values.
		/// </summary>
		/// <remarks>
		/// Default value = 100.
		/// Min value = 0.
		/// </remarks>
		public int RandomSeed
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				SetArgument("randomSeed", value);
			}
		}

		/// <summary>
		/// Amount of random back twist to add.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float BackTwist
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("backTwist", value);
			}
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see Active Pose notes for possible values).
		/// </summary>
		/// <remarks>
		/// Default value = "fb".
		/// </remarks>
		public string Mask
		{
			set
			{
				SetArgument("mask", value);
			}
		}
	}

	public sealed class BodyRollUpHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the BodyRollUpHelper for sending a BodyRollUp <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the BodyRollUp <see cref="Message"/> to.</param>
		public BodyRollUpHelper(Ped ped) : base(ped, "bodyRollUp")
		{
		}

		/// <summary>
		/// Stiffness of whole body.
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float Stiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("stiffness", value);
			}
		}

		/// <summary>
		/// The degree to which the character will try to stop a barrel roll with his arms.
		/// </summary>
		/// <remarks>
		/// Default value = 1.3f.
		/// Min value = -2.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float UseArmToSlowDown
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < -2.0f)
				{
					value = -2.0f;
				}

				SetArgument("useArmToSlowDown", value);
			}
		}

		/// <summary>
		/// The likeliness of the character reaching for the ground with its arms.
		/// </summary>
		/// <remarks>
		/// Default value = 1.4f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float ArmReachAmount
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armReachAmount", value);
			}
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see Active Pose notes for possible values).
		/// </summary>
		/// <remarks>
		/// Default value = "fb".
		/// </remarks>
		public string Mask
		{
			set
			{
				SetArgument("mask", value);
			}
		}

		/// <summary>
		/// Used to keep rolling down slope, 1 is full (kicks legs out when pointing upwards).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float LegPush
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("legPush", value);
			}
		}

		/// <summary>
		/// 0 is no leg asymmetry in 'foetal' position.  greater than 0 a asymmetricalLegs-rand(30%), added/minus each joint of the legs in radians.  Random number changes about once every roll.  0.4 gives a lot of asymmetry.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -2.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float AsymmetricalLegs
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < -2.0f)
				{
					value = -2.0f;
				}

				SetArgument("asymmetricalLegs", value);
			}
		}

		/// <summary>
		/// Time that roll velocity has to be lower than rollVelForSuccess, before success message is sent.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float NoRollTimeBeforeSuccess
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("noRollTimeBeforeSuccess", value);
			}
		}

		/// <summary>
		/// Lower threshold for roll velocity at which success message can be sent.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RollVelForSuccess
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rollVelForSuccess", value);
			}
		}

		/// <summary>
		/// Contribution of linear COM velocity to roll Velocity (if 0, roll velocity equal to COM angular velocity).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RollVelLinearContribution
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rollVelLinearContribution", value);
			}
		}

		/// <summary>
		/// Scales perceived body velocity.  The higher this value gets, the more quickly the velocity measure saturates, resulting in a tighter roll at slower speeds. (NB: Set to 1 to match earlier behavior).
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float VelocityScale
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("velocityScale", value);
			}
		}

		/// <summary>
		/// Offsets perceived body velocity.  Increase to create larger "dead zone" around zero velocity where character will be less rolled. (NB: Reset to 0 to match earlier behavior).
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float VelocityOffset
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("velocityOffset", value);
			}
		}

		/// <summary>
		/// Controls whether or not behavior enforces min/max friction.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ApplyMinMaxFriction
		{
			set
			{
				SetArgument("applyMinMaxFriction", value);
			}
		}
	}

	public sealed class BodyWritheHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the BodyWritheHelper for sending a BodyWrithe <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the BodyWrithe <see cref="Message"/> to.</param>
		public BodyWritheHelper(Ped ped) : base(ped, "bodyWrithe")
		{
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 13.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ArmStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("armStiffness", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 13.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float BackStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("backStiffness", value);
			}
		}

		/// <summary>
		/// The stiffness of the character will determine how 'determined' a writhe this is - high values will make him thrash about wildly.
		/// </summary>
		/// <remarks>
		/// Default value = 13.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float LegStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("legStiffness", value);
			}
		}

		/// <summary>
		/// Damping amount, less is underdamped.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float ArmDamping
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armDamping", value);
			}
		}

		/// <summary>
		/// Damping amount, less is underdamped.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float BackDamping
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("backDamping", value);
			}
		}

		/// <summary>
		/// Damping amount, less is underdamped.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float LegDamping
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legDamping", value);
			}
		}

		/// <summary>
		/// Controls how fast the writhe is executed, smaller values make faster motions.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float ArmPeriod
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armPeriod", value);
			}
		}

		/// <summary>
		/// Controls how fast the writhe is executed, smaller values make faster motions.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float BackPeriod
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("backPeriod", value);
			}
		}

		/// <summary>
		/// Controls how fast the writhe is executed, smaller values make faster motions.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float LegPeriod
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legPeriod", value);
			}
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see Active Pose notes for possible values).
		/// </summary>
		/// <remarks>
		/// Default value = "fb".
		/// </remarks>
		public string Mask
		{
			set
			{
				SetArgument("mask", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float ArmAmplitude
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armAmplitude", value);
			}
		}

		/// <summary>
		/// Scales the amount of writhe. 0 = no writhe.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float BackAmplitude
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("backAmplitude", value);
			}
		}

		/// <summary>
		/// Scales the amount of writhe. 0 = no writhe.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float LegAmplitude
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legAmplitude", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float ElbowAmplitude
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("elbowAmplitude", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float KneeAmplitude
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("kneeAmplitude", value);
			}
		}

		/// <summary>
		/// Flag to set trying to rollOver.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool RollOverFlag
		{
			set
			{
				SetArgument("rollOverFlag", value);
			}
		}

		/// <summary>
		/// Blend the writhe arms with the current desired arms (0=don't apply any writhe, 1=only writhe).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float BlendArms
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("blendArms", value);
			}
		}

		/// <summary>
		/// Blend the writhe spine and neck with the current desired (0=don't apply any writhe, 1=only writhe).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float BlendBack
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("blendBack", value);
			}
		}

		/// <summary>
		/// Blend the writhe legs with the current desired legs (0=don't apply any writhe, 1=only writhe).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float BlendLegs
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("blendLegs", value);
			}
		}

		/// <summary>
		/// Use writhe stiffnesses if true. If false don't set any stiffnesses.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ApplyStiffness
		{
			set
			{
				SetArgument("applyStiffness", value);
			}
		}

		/// <summary>
		/// Extra shoulderBlend. Rolling:one way only, maxRollOverTime, rollOverRadius, doesn't reduce arm stiffness to help rolling. No shoulder twist.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool OnFire
		{
			set
			{
				SetArgument("onFire", value);
			}
		}

		/// <summary>
		/// Blend writhe shoulder desired lean1 with this angle in RAD. Note that onFire has to be set to true for this parameter to take any effect.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 6.3f.
		/// </remarks>
		public float ShoulderLean1
		{
			set
			{
				if (value > 6.3f)
				{
					value = 6.3f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("shoulderLean1", value);
			}
		}

		/// <summary>
		/// Blend writhe shoulder desired lean2 with this angle in RAD. Note that onFire has to be set to true for this parameter to take any effect.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 6.3f.
		/// </remarks>
		public float ShoulderLean2
		{
			set
			{
				if (value > 6.3f)
				{
					value = 6.3f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("shoulderLean2", value);
			}
		}

		/// <summary>
		/// Shoulder desired lean1 with shoulderLean1 angle blend factor. Set it to 0 to use original shoulder withe desired lean1 angle for shoulders. Note that onFire has to be set to true for this parameter to take any effect.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Lean1BlendFactor
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("lean1BlendFactor", value);
			}
		}

		/// <summary>
		/// Shoulder desired lean2 with shoulderLean2 angle blend factor. Set it to 0 to use original shoulder withe desired lean2 angle for shoulders. Note that onFire has to be set to true for this parameter to take any effect.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Lean2BlendFactor
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("lean2BlendFactor", value);
			}
		}

		/// <summary>
		/// Scale rolling torque that is applied to character spine.
		/// </summary>
		/// <remarks>
		/// Default value = 150.0f.
		/// Min value = 0.0f.
		/// Max value = 300.0f.
		/// </remarks>
		public float RollTorqueScale
		{
			set
			{
				if (value > 300.0f)
				{
					value = 300.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rollTorqueScale", value);
			}
		}

		/// <summary>
		/// Rolling torque is ramped down over time. At this time in seconds torque value converges to zero. Use this parameter to restrict time the character is rolling. Note that onFire has to be set to true for this parameter to take any effect.
		/// </summary>
		/// <remarks>
		/// Default value = 8.0f.
		/// Min value = 0.0f.
		/// Max value = 60.0f.
		/// </remarks>
		public float MaxRollOverTime
		{
			set
			{
				if (value > 60.0f)
				{
					value = 60.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxRollOverTime", value);
			}
		}

		/// <summary>
		/// Rolling torque is ramped down with distance measured from position where character hit the ground and started rolling. At this distance in meters torque value converges to zero. Use this parameter to restrict distance the character travels due to rolling. Note that onFire has to be set to true for this parameter to take any effect.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float RollOverRadius
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rollOverRadius", value);
			}
		}
	}

	public sealed class BraceForImpactHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the BraceForImpactHelper for sending a BraceForImpact <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the BraceForImpact <see cref="Message"/> to.</param>
		public BraceForImpactHelper(Ped ped) : base(ped, "braceForImpact")
		{
		}

		/// <summary>
		/// Distance from object at which to raise hands to brace 0.5 good if newBrace=true - otherwise 0.65.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float BraceDistance
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("braceDistance", value);
			}
		}

		/// <summary>
		/// Time epected to get arms up from idle.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TargetPredictionTime
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("targetPredictionTime", value);
			}
		}

		/// <summary>
		/// Larger values and he absorbs the impact more.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ReachAbsorbtionTime
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("reachAbsorbtionTime", value);
			}
		}

		/// <summary>
		/// LevelIndex of object to brace.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int InstanceIndex
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("instanceIndex", value);
			}
		}

		/// <summary>
		/// Stiffness of character. Catch_fall stiffness scales with this too, with its defaults at this values default.
		/// </summary>
		/// <remarks>
		/// Default value = 12.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float BodyStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("bodyStiffness", value);
			}
		}

		/// <summary>
		/// Once a constraint is made, keep reaching with whatever hand is allowed.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool GrabDontLetGo
		{
			set
			{
				SetArgument("grabDontLetGo", value);
			}
		}

		/// <summary>
		/// Strength in hands for grabbing (kg m/s), -1 to ignore/disable.
		/// </summary>
		/// <remarks>
		/// Default value = 40.0f.
		/// Min value = -1.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float GrabStrength
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("grabStrength", value);
			}
		}

		/// <summary>
		/// Relative distance at which the grab starts.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float GrabDistance
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("grabDistance", value);
			}
		}

		/// <summary>
		/// Angle from front at which the grab activates. If the point is outside this angle from front will not try to grab.
		/// </summary>
		/// <remarks>
		/// Default value = 1.5f.
		/// Min value = 0.0f.
		/// Max value = 3.2f.
		/// </remarks>
		public float GrabReachAngle
		{
			set
			{
				if (value > 3.2f)
				{
					value = 3.2f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("grabReachAngle", value);
			}
		}

		/// <summary>
		/// Amount of time, in seconds, before grab automatically bails.
		/// </summary>
		/// <remarks>
		/// Default value = 2.5f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float GrabHoldTimer
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("grabHoldTimer", value);
			}
		}

		/// <summary>
		/// Don't try to grab a car moving above this speed mmmmtodo make this the relative velocity of car to character?.
		/// </summary>
		/// <remarks>
		/// Default value = 95.0f.
		/// Min value = 0.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float MaxGrabCarVelocity
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxGrabCarVelocity", value);
			}
		}

		/// <summary>
		/// Balancer leg stiffness mmmmtodo remove this parameter and use configureBalance?.
		/// </summary>
		/// <remarks>
		/// Default value = 12.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float LegStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("legStiffness", value);
			}
		}

		/// <summary>
		/// Time before arm brace kicks in when hit from behind.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float TimeToBackwardsBrace
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("timeToBackwardsBrace", value);
			}
		}

		/// <summary>
		/// Position to look at, e.g. the driver.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Look
		{
			set
			{
				SetArgument("look", value);
			}
		}

		/// <summary>
		/// Location of the front part of the object to brace against. This should be the center of where his hands should meet the object.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Pos
		{
			set
			{
				SetArgument("pos", value);
			}
		}

		/// <summary>
		/// Minimum bracing time so the character doesn't look twitchy.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float MinBraceTime
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("minBraceTime", value);
			}
		}

		/// <summary>
		/// If bracing with 2 hands delay one hand by at least this amount of time to introduce some asymmetry.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float HandsDelayMin
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("handsDelayMin", value);
			}
		}

		/// <summary>
		/// If bracing with 2 hands delay one hand by at most this amount of time to introduce some asymmetry.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float HandsDelayMax
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("handsDelayMax", value);
			}
		}

		/// <summary>
		/// Move away from the car (if in reaching zone).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool MoveAway
		{
			set
			{
				SetArgument("moveAway", value);
			}
		}

		/// <summary>
		/// ForceLean away amount (-ve is lean towards).
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float MoveAwayAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("moveAwayAmount", value);
			}
		}

		/// <summary>
		/// Lean away amount (-ve is lean towards).
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = -0.5f.
		/// Max value = 0.5f.
		/// </remarks>
		public float MoveAwayLean
		{
			set
			{
				if (value > 0.5f)
				{
					value = 0.5f;
				}

				if (value < -0.5f)
				{
					value = -0.5f;
				}

				SetArgument("moveAwayLean", value);
			}
		}

		/// <summary>
		/// Amount of sideways movement if at the front or back of the car to add to the move away from car.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float MoveSideways
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("moveSideways", value);
			}
		}

		/// <summary>
		/// Use bodyBalance arms for the default (non bracing) behavior if bodyBalance is active.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool BbArms
		{
			set
			{
				SetArgument("bbArms", value);
			}
		}

		/// <summary>
		/// Use the new brace prediction code.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool NewBrace
		{
			set
			{
				SetArgument("newBrace", value);
			}
		}

		/// <summary>
		/// If true then if a shin or thigh is in contact with the car then brace. NB: newBrace must be true.  For those situations where the car has pushed the ped backwards (at the same speed as the car) before the behavior has been started and so doesn't predict an impact.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool BraceOnImpact
		{
			set
			{
				SetArgument("braceOnImpact", value);
			}
		}

		/// <summary>
		/// When rollDownStairs is running use roll2Velocity to control the helper torques (this only attempts to roll to the chaarcter's velocity not some default linear velocity mag.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Roll2Velocity
		{
			set
			{
				SetArgument("roll2Velocity", value);
			}
		}

		/// <summary>
		/// 0 = original/roll off/stay on car:  Roll with character velocity, 1 = //Gentle: roll off/stay on car = use relative velocity of character to car to roll against, 2 = //roll over car:  Roll against character velocity.  i.e. roll against any velocity picked up by hitting car, 3 = //Gentle: roll over car:  use relative velocity of character to car to roll with.
		/// </summary>
		/// <remarks>
		/// Default value = 3.
		/// Min value = 0.
		/// Max value = 3.
		/// </remarks>
		public int RollType
		{
			set
			{
				if (value > 3)
				{
					value = 3;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("rollType", value);
			}
		}

		/// <summary>
		/// Exaggerate impacts using snap.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SnapImpacts
		{
			set
			{
				SetArgument("snapImpacts", value);
			}
		}

		/// <summary>
		/// Exaggeration amount of the initial impact (legs).  +ve fold with car impact (as if pushed at hips in the car velocity direction).  -ve fold away from car impact.
		/// </summary>
		/// <remarks>
		/// Default value = 7.0f.
		/// Min value = -20.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float SnapImpact
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < -20.0f)
				{
					value = -20.0f;
				}

				SetArgument("snapImpact", value);
			}
		}

		/// <summary>
		/// Exaggeration amount of the secondary (torso) impact with bonnet. +ve fold with car impact (as if pushed at hips by the impact normal).  -ve fold away from car impact.
		/// </summary>
		/// <remarks>
		/// Default value = -7.0f.
		/// Min value = -20.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float SnapBonnet
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < -20.0f)
				{
					value = -20.0f;
				}

				SetArgument("snapBonnet", value);
			}
		}

		/// <summary>
		/// Exaggeration amount of the impact with the floor after falling off of car +ve fold with floor impact (as if pushed at hips in the impact normal direction).  -ve fold away from car impact.
		/// </summary>
		/// <remarks>
		/// Default value = 7.0f.
		/// Min value = -20.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float SnapFloor
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < -20.0f)
				{
					value = -20.0f;
				}

				SetArgument("snapFloor", value);
			}
		}

		/// <summary>
		/// Damp out excessive spin and upward velocity when on car.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool DampVel
		{
			set
			{
				SetArgument("dampVel", value);
			}
		}

		/// <summary>
		/// Amount to damp spinning by (cartwheeling and somersaulting).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 40.0f.
		/// </remarks>
		public float DampSpin
		{
			set
			{
				if (value > 40.0f)
				{
					value = 40.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dampSpin", value);
			}
		}

		/// <summary>
		/// Amount to damp upward velocity by to limit the amount of air above the car the character can get.
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 0.0f.
		/// Max value = 40.0f.
		/// </remarks>
		public float DampUpVel
		{
			set
			{
				if (value > 40.0f)
				{
					value = 40.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dampUpVel", value);
			}
		}

		/// <summary>
		/// Angular velocity above which we start damping.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float DampSpinThresh
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dampSpinThresh", value);
			}
		}

		/// <summary>
		/// Upward velocity above which we start damping.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float DampUpVelThresh
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dampUpVelThresh", value);
			}
		}

		/// <summary>
		/// Enhance a glancing spin with the side of the car by modulating body friction.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool GsHelp
		{
			set
			{
				SetArgument("gsHelp", value);
			}
		}

		/// <summary>
		/// ID for glancing spin. Min depth to be considered from either end (front/rear) of a car (-ve is inside the car area).
		/// </summary>
		/// <remarks>
		/// Default value = -0.1f.
		/// Min value = -10.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float GsEndMin
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -10.0f)
				{
					value = -10.0f;
				}

				SetArgument("gsEndMin", value);
			}
		}

		/// <summary>
		/// ID for glancing spin. Min depth to be considered on the side of a car (-ve is inside the car area).
		/// </summary>
		/// <remarks>
		/// Default value = -0.2f.
		/// Min value = -10.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float GsSideMin
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -10.0f)
				{
					value = -10.0f;
				}

				SetArgument("gsSideMin", value);
			}
		}

		/// <summary>
		/// ID for glancing spin. Max depth to be considered on the side of a car (+ve is outside the car area).
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = -10.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float GsSideMax
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -10.0f)
				{
					value = -10.0f;
				}

				SetArgument("gsSideMax", value);
			}
		}

		/// <summary>
		/// ID for glancing spin. Character has to be more upright than this value for it to be considered on the side of a car. Fully upright = 1, upsideDown = -1.  Max Angle from upright is acos(gsUpness).
		/// </summary>
		/// <remarks>
		/// Default value = 0.9f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float GsUpness
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("gsUpness", value);
			}
		}

		/// <summary>
		/// ID for glancing spin. Minimum car velocity.
		/// </summary>
		/// <remarks>
		/// Default value = 3.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float GsCarVelMin
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("gsCarVelMin", value);
			}
		}

		/// <summary>
		/// Apply gsFricScale1 to the foot if colliding with car.  (Otherwise foot friction - with the ground - is determined by gsFricScale2 if it is in gsFricMask2).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool GsScale1Foot
		{
			set
			{
				SetArgument("gsScale1Foot", value);
			}
		}

		/// <summary>
		/// Glancing spin help. Friction scale applied when to the side of the car.  e.g. make the character spin more by upping the friction against the car.
		/// </summary>
		/// <remarks>
		/// Default value = 8.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float GsFricScale1
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("gsFricScale1", value);
			}
		}

		/// <summary>
		/// Glancing spin help. Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see notes for explanation). Note gsFricMask1 and gsFricMask2 are made independent by the code so you can have fb for gsFricMask1 but gsFricScale1 will not be applied to any body parts in gsFricMask2.
		/// </summary>
		/// <remarks>
		/// Default value = "fb".
		/// </remarks>
		public string GsFricMask1
		{
			set
			{
				SetArgument("gsFricMask1", value);
			}
		}

		/// <summary>
		/// Glancing spin help. Friction scale applied when to the side of the car.  e.g. make the character spin more by lowering the feet friction. You could also lower the wrist friction here to stop the car pulling along the hands i.e. gsFricMask2 = la|uw.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float GsFricScale2
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("gsFricScale2", value);
			}
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see notes for explanation). Note gsFricMask1 and gsFricMask2 are made independent by the code so you can have fb for gsFricMask1 but gsFricScale1 will not be applied to any body parts in gsFricMask2.
		/// </summary>
		/// <remarks>
		/// Default value = la.
		/// </remarks>
		public string GsFricMask2
		{
			set
			{
				SetArgument("gsFricMask2", value);
			}
		}
	}

	/// <summary>
	/// Simple buoyancy model.  No character movement just fluid forces/torques added to parts.
	/// </summary>
	public sealed class BuoyancyHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the BuoyancyHelper for sending a Buoyancy <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the Buoyancy <see cref="Message"/> to.</param>
		/// <remarks>
		/// Simple buoyancy model.  No character movement just fluid forces/torques added to parts.
		/// </remarks>
		public BuoyancyHelper(Ped ped) : base(ped, "buoyancy")
		{
		}

		/// <summary>
		/// Arbitrary point on surface of water.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 SurfacePoint
		{
			set
			{
				SetArgument("surfacePoint", value);
			}
		}

		/// <summary>
		/// Normal to surface of water.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 1.0f).
		/// Min value = 0.0f.
		/// </remarks>
		public Vector3 SurfaceNormal
		{
			set
			{
				SetArgument("surfaceNormal", Vector3.Maximize(value, new Vector3(0.0f, 0.0f, 0.0f)));
			}
		}

		/// <summary>
		/// Buoyancy multiplier.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float Buoyancy
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("buoyancy", value);
			}
		}

		/// <summary>
		/// Buoyancy multiplier for spine2/3. Helps character float upright.
		/// </summary>
		/// <remarks>
		/// Default value = 8.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float ChestBuoyancy
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("chestBuoyancy", value);
			}
		}

		/// <summary>
		/// Damping for submerged parts.
		/// </summary>
		/// <remarks>
		/// Default value = 40.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float Damping
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("damping", value);
			}
		}

		/// <summary>
		/// Use righting torque to being character face-up in water?.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool Righting
		{
			set
			{
				SetArgument("righting", value);
			}
		}

		/// <summary>
		/// Strength of righting torque.
		/// </summary>
		/// <remarks>
		/// Default value = 25.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float RightingStrength
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rightingStrength", value);
			}
		}

		/// <summary>
		/// How long to wait after chest hits water to begin righting torque.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// </remarks>
		public float RightingTime
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rightingTime", value);
			}
		}
	}

	public sealed class CatchFallHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the CatchFallHelper for sending a CatchFall <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the CatchFall <see cref="Message"/> to.</param>
		public CatchFallHelper(Ped ped) : base(ped, "catchFall")
		{
		}

		/// <summary>
		/// Stiffness of torso.
		/// </summary>
		/// <remarks>
		/// Default value = 9.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float TorsoStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("torsoStiffness", value);
			}
		}

		/// <summary>
		/// Stiffness of legs.
		/// </summary>
		/// <remarks>
		/// Default value = 6.0f.
		/// Min value = 4.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float LegsStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 4.0f)
				{
					value = 4.0f;
				}

				SetArgument("legsStiffness", value);
			}
		}

		/// <summary>
		/// Stiffness of arms.
		/// </summary>
		/// <remarks>
		/// Default value = 15.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ArmsStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("armsStiffness", value);
			}
		}

		/// <summary>
		/// 0 will prop arms up near his shoulders. -0.3 will place hands nearer his behind.
		/// </summary>
		/// <remarks>
		/// Default value = -0.3f.
		/// Min value = -1.0f.
		/// Max value = 0.0f.
		/// </remarks>
		public float BackwardsMinArmOffset
		{
			set
			{
				if (value > 0.0f)
				{
					value = 0.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("backwardsMinArmOffset", value);
			}
		}

		/// <summary>
		/// 0 will point arms down with angled body, 0.45 will point arms forward a bit to catch nearer the head.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ForwardMaxArmOffset
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("forwardMaxArmOffset", value);
			}
		}

		/// <summary>
		/// Tries to reduce the spin around the Z axis. Scale 0 - 1.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ZAxisSpinReduction
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("zAxisSpinReduction", value);
			}
		}

		/// <summary>
		/// Scale extra-sit value 0..1. Setting to 0 helps with arched-back issues.  Set to 1 for a more alive-looking finish.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ExtraSit
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("extraSit", value);
			}
		}

		/// <summary>
		/// Toggle to use the head look in this behavior.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseHeadLook
		{
			set
			{
				SetArgument("useHeadLook", value);
			}
		}

		/// <summary>
		/// Two character body-masking value, bitwise joint mask or bitwise logic string of two character body-masking value  (see Active Pose notes for possible values).
		/// </summary>
		/// <remarks>
		/// Default value = "fb".
		/// </remarks>
		public string Mask
		{
			set
			{
				SetArgument("mask", value);
			}
		}
	}

	public sealed class ElectrocuteHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ElectrocuteHelper for sending a Electrocute <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the Electrocute <see cref="Message"/> to.</param>
		public ElectrocuteHelper(Ped ped) : base(ped, "electrocute")
		{
		}

		/// <summary>
		/// The magnitude of the reaction.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float StunMag
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stunMag", value);
			}
		}

		/// <summary>
		/// InitialMult*stunMag = The magnitude of the 1st snap reaction (other multipliers are applied after this).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float InitialMult
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("initialMult", value);
			}
		}

		/// <summary>
		/// LargeMult*stunMag = The magnitude of a random large snap reaction (other multipliers are applied after this).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float LargeMult
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("largeMult", value);
			}
		}

		/// <summary>
		/// Min time to next large random snap (about 14 snaps with stunInterval = 0.07s).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 200.0f.
		/// </remarks>
		public float LargeMinTime
		{
			set
			{
				if (value > 200.0f)
				{
					value = 200.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("largeMinTime", value);
			}
		}

		/// <summary>
		/// Max time to next large random snap (about 28 snaps with stunInterval = 0.07s).
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 200.0f.
		/// </remarks>
		public float LargeMaxTime
		{
			set
			{
				if (value > 200.0f)
				{
					value = 200.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("largeMaxTime", value);
			}
		}

		/// <summary>
		/// MovingMult*stunMag = The magnitude of the reaction if moving(comVelMag) faster than movingThresh.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float MovingMult
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("movingMult", value);
			}
		}

		/// <summary>
		/// BalancingMult*stunMag = The magnitude of the reaction if balancing = (not lying on the floor/ not upper body not collided) and not airborne.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float BalancingMult
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("balancingMult", value);
			}
		}

		/// <summary>
		/// AirborneMult*stunMag = The magnitude of the reaction if airborne.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float AirborneMult
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("airborneMult", value);
			}
		}

		/// <summary>
		/// If moving(comVelMag) faster than movingThresh then mvingMult applied to stunMag.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float MovingThresh
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("movingThresh", value);
			}
		}

		/// <summary>
		/// Direction flips every stunInterval.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float StunInterval
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stunInterval", value);
			}
		}

		/// <summary>
		/// The character vibrates in a prescribed way - Higher the value the more random this direction is.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float DirectionRandomness
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("directionRandomness", value);
			}
		}

		/// <summary>
		/// Vibrate the leftArm.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool LeftArm
		{
			set
			{
				SetArgument("leftArm", value);
			}
		}

		/// <summary>
		/// Vibrate the rightArm.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool RightArm
		{
			set
			{
				SetArgument("rightArm", value);
			}
		}

		/// <summary>
		/// Vibrate the leftLeg.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool LeftLeg
		{
			set
			{
				SetArgument("leftLeg", value);
			}
		}

		/// <summary>
		/// Vibrate the rightLeg.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool RightLeg
		{
			set
			{
				SetArgument("rightLeg", value);
			}
		}

		/// <summary>
		/// Vibrate the spine.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool Spine
		{
			set
			{
				SetArgument("spine", value);
			}
		}

		/// <summary>
		/// Vibrate the neck.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool Neck
		{
			set
			{
				SetArgument("neck", value);
			}
		}

		/// <summary>
		/// Legs are either in phase with each other or not.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool PhasedLegs
		{
			set
			{
				SetArgument("phasedLegs", value);
			}
		}

		/// <summary>
		/// Let electrocute apply a (higher generally) stiffness to the character whilst being vibrated.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ApplyStiffness
		{
			set
			{
				SetArgument("applyStiffness", value);
			}
		}

		/// <summary>
		/// Use torques to make vibration otherwise use a change in the parts angular velocity.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseTorques
		{
			set
			{
				SetArgument("useTorques", value);
			}
		}

		/// <summary>
		/// Type of hip reaction 0=none, 1=side2side 2=steplike.
		/// </summary>
		/// <remarks>
		/// Default value = 2.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int HipType
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("hipType", value);
			}
		}
	}

	public sealed class FallOverWallHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the FallOverWallHelper for sending a FallOverWall <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the FallOverWall <see cref="Message"/> to.</param>
		public FallOverWallHelper(Ped ped) : base(ped, "fallOverWall")
		{
		}

		/// <summary>
		/// Stiffness of the body, roll up stiffness scales with this and defaults at this default value.
		/// </summary>
		/// <remarks>
		/// Default value = 9.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float BodyStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("bodyStiffness", value);
			}
		}

		/// <summary>
		/// Damping in the effectors.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float Damping
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("damping", value);
			}
		}

		/// <summary>
		/// Magnitude of the falloverWall helper force.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float MagOfForce
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("magOfForce", value);
			}
		}

		/// <summary>
		/// The maximum distance away from the pelvis that hit points will be registered.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float MaxDistanceFromPelToHitPoint
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxDistanceFromPelToHitPoint", value);
			}
		}

		/// <summary>
		/// Maximum distance between hitPoint and body part at which forces are applied to part.
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float MaxForceDist
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxForceDist", value);
			}
		}

		/// <summary>
		/// Specifies extent of area in front of the wall in which balancer won't try to take another step.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float StepExclusionZone
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stepExclusionZone", value);
			}
		}

		/// <summary>
		/// Minimum height of pelvis above feet at which fallOverWall is attempted.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.1f.
		/// Max value = 2.0f.
		/// </remarks>
		public float MinLegHeight
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("minLegHeight", value);
			}
		}

		/// <summary>
		/// Amount of twist to apply to the spine as the character tries to fling himself over the wall, provides more of a believable roll but increases the amount of lateral space the character needs to successfully flip.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float BodyTwist
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("bodyTwist", value);
			}
		}

		/// <summary>
		/// Max angle the character can twist before twsit helper torques are turned off.
		/// </summary>
		/// <remarks>
		/// Default value = 3.1f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float MaxTwist
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxTwist", value);
			}
		}

		/// <summary>
		/// One end of the wall to try to fall over.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 FallOverWallEndA
		{
			set
			{
				SetArgument("fallOverWallEndA", value);
			}
		}

		/// <summary>
		/// One end of the wall over which we are trying to fall over.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 FallOverWallEndB
		{
			set
			{
				SetArgument("fallOverWallEndB", value);
			}
		}

		/// <summary>
		/// The angle abort threshold.
		/// </summary>
		/// <remarks>
		/// Default value = -0.2f.
		/// </remarks>
		public float ForceAngleAbort
		{
			set
			{
				SetArgument("forceAngleAbort", value);
			}
		}

		/// <summary>
		/// The force time out.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// </remarks>
		public float ForceTimeOut
		{
			set
			{
				SetArgument("forceTimeOut", value);
			}
		}

		/// <summary>
		/// Lift the arms up if true.  Do nothing with the arms if false (eg when using catchfall arms or brace etc).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool MoveArms
		{
			set
			{
				SetArgument("moveArms", value);
			}
		}

		/// <summary>
		/// Move the legs if true.  Do nothing with the legs if false (eg when using dynamicBalancer etc).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool MoveLegs
		{
			set
			{
				SetArgument("moveLegs", value);
			}
		}

		/// <summary>
		/// Bend spine to help falloverwall if true.  Do nothing with the spine if false.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool BendSpine
		{
			set
			{
				SetArgument("bendSpine", value);
			}
		}

		/// <summary>
		/// Maximum angle in degrees (between the direction of the velocity of the COM and the wall normal) to start to apply forces and torques to fall over the wall.
		/// </summary>
		/// <remarks>
		/// Default value = 180.0f.
		/// Min value = 0.0f.
		/// Max value = 180.0f.
		/// </remarks>
		public float AngleDirWithWallNormal
		{
			set
			{
				if (value > 180.0f)
				{
					value = 180.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("angleDirWithWallNormal", value);
			}
		}

		/// <summary>
		/// Maximum angle in degrees (between the vertical vector and a vector from pelvis to lower neck) to start to apply forces and torques to fall over the wall.
		/// </summary>
		/// <remarks>
		/// Default value = 180.0f.
		/// Min value = 0.0f.
		/// Max value = 180.0f.
		/// </remarks>
		public float LeaningAngleThreshold
		{
			set
			{
				if (value > 180.0f)
				{
					value = 180.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leaningAngleThreshold", value);
			}
		}

		/// <summary>
		/// If the angular velocity is higher than maxAngVel, the torques and forces are not applied.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = -1.0f.
		/// Max value = 30.0f.
		/// </remarks>
		public float MaxAngVel
		{
			set
			{
				if (value > 30.0f)
				{
					value = 30.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("maxAngVel", value);
			}
		}

		/// <summary>
		/// Will reduce the magnitude of the forces applied to the character to help him to fall over wall.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AdaptForcesToLowWall
		{
			set
			{
				SetArgument("adaptForcesToLowWall", value);
			}
		}

		/// <summary>
		/// Maximum height (from the lowest foot) to start to apply forces and torques to fall over the wall.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float MaxWallHeight
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("maxWallHeight", value);
			}
		}

		/// <summary>
		/// Minimum distance between the pelvis and the wall to send the success message. If negative doesn't take this parameter into account when sending feedback.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float DistanceToSendSuccessMessage
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("distanceToSendSuccessMessage", value);
			}
		}

		/// <summary>
		/// Value of the angular velocity about the wallEgde above which the character is considered as rolling backwards i.e. goes in to fow_RollingBack state.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float RollingBackThr
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rollingBackThr", value);
			}
		}

		/// <summary>
		/// On impact with the wall if the rollingPotential(calculated from the characters linear velocity w.r.t the wall) is greater than this value the character will try to go over the wall otherwise it won't try (fow_Aborted).
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = -1.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float RollingPotential
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("rollingPotential", value);
			}
		}

		/// <summary>
		/// Try to reach the wallEdge. To configure the IK : use limitAngleBack, limitAngleFront and limitAngleTotallyBack.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseArmIK
		{
			set
			{
				SetArgument("useArmIK", value);
			}
		}

		/// <summary>
		/// Distance from predicted hitpoint where each hands will try to reach the wall.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ReachDistanceFromHitPoint
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("reachDistanceFromHitPoint", value);
			}
		}

		/// <summary>
		/// Minimal distance from predicted hitpoint where each hands will try to reach the wall. Used if the hand target is outside the wall Edge.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float MinReachDistanceFromHitPoint
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("minReachDistanceFromHitPoint", value);
			}
		}

		/// <summary>
		/// Max angle in degrees (between 1.the vector between two hips and 2. WallEdge) to try to reach the wall just behind his pelvis with his arms when the character is back to the wall.
		/// </summary>
		/// <remarks>
		/// Default value = 15.0f.
		/// Min value = 0.0f.
		/// Max value = 180.0f.
		/// </remarks>
		public float AngleTotallyBack
		{
			set
			{
				if (value > 180.0f)
				{
					value = 180.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("angleTotallyBack", value);
			}
		}
	}

	public sealed class GrabHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the GrabHelper for sending a Grab <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the Grab <see cref="Message"/> to.</param>
		public GrabHelper(Ped ped) : base(ped, "grab")
		{
		}

		/// <summary>
		/// Flag to toggle use of left hand.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseLeft
		{
			set
			{
				SetArgument("useLeft", value);
			}
		}

		/// <summary>
		/// Flag to toggle the use of the Right hand.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseRight
		{
			set
			{
				SetArgument("useRight", value);
			}
		}

		/// <summary>
		/// If hasn't grabbed when weapon carrying hand is close to target, grab anyway.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool DropWeaponIfNecessary
		{
			set
			{
				SetArgument("dropWeaponIfNecessary", value);
			}
		}

		/// <summary>
		/// Distance below which a weapon carrying hand will request weapon to be dropped.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float DropWeaponDistance
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dropWeaponDistance", value);
			}
		}

		/// <summary>
		/// Strength in hands for grabbing (kg m/s), -1 to ignore/disable.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 10000.0f.
		/// </remarks>
		public float GrabStrength
		{
			set
			{
				if (value > 10000.0f)
				{
					value = 10000.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("grabStrength", value);
			}
		}

		/// <summary>
		/// Strength of cheat force on hands to pull towards target and stick to target ("cleverHandIK" strength).
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float StickyHands
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stickyHands", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="TurnType.ToTarget"/>.
		/// </remarks>
		public TurnType TurnToTarget
		{
			set
			{
				SetArgument("turnToTarget", (int)value);
			}
		}

		/// <summary>
		/// Amount of time, in seconds, before grab automatically bails.
		/// </summary>
		/// <remarks>
		/// Default value = 100.0f.
		/// Min value = 0.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float GrabHoldMaxTimer
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("grabHoldMaxTimer", value);
			}
		}

		/// <summary>
		/// Time to reach the full pullup strength.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float PullUpTime
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("pullUpTime", value);
			}
		}

		/// <summary>
		/// Strength to pull up with the right arm. 0 = no pull up.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float PullUpStrengthRight
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("pullUpStrengthRight", value);
			}
		}

		/// <summary>
		/// Strength to pull up with the left arm. 0 = no pull up.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float PullUpStrengthLeft
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("pullUpStrengthLeft", value);
			}
		}

		/// <summary>
		/// Grab pos1, right hand if not using line or surface grab.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Pos1
		{
			set
			{
				SetArgument("pos1", value);
			}
		}

		/// <summary>
		/// Grab pos2, left hand if not using line or surface grab.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Pos2
		{
			set
			{
				SetArgument("pos2", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Pos3
		{
			set
			{
				SetArgument("pos3", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Pos4
		{
			set
			{
				SetArgument("pos4", value);
			}
		}

		/// <summary>
		/// Normal for the right grab point.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public Vector3 NormalR
		{
			set
			{
				SetArgument("normalR", Vector3.Clamp(value, new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f)));
			}
		}

		/// <summary>
		/// Normal for the left grab point.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public Vector3 NormalL
		{
			set
			{
				SetArgument("normalL", Vector3.Clamp(value, new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f)));
			}
		}

		/// <summary>
		/// Normal for the 2nd right grab point (if pointsX4grab=true).
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public Vector3 NormalR2
		{
			set
			{
				SetArgument("normalR2", Vector3.Clamp(value, new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f)));
			}
		}

		/// <summary>
		/// Normal for the 3rd left grab point (if pointsX4grab=true).
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public Vector3 NormalL2
		{
			set
			{
				SetArgument("normalL2", Vector3.Clamp(value, new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f)));
			}
		}

		/// <summary>
		/// Hand collisions on when grabbing (false turns off hand collisions making grab more stable esp. To grab points slightly inside geometry).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool HandsCollide
		{
			set
			{
				SetArgument("handsCollide", value);
			}
		}

		/// <summary>
		/// Flag to toggle between grabbing and bracing.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool JustBrace
		{
			set
			{
				SetArgument("justBrace", value);
			}
		}

		/// <summary>
		/// Use the line grab, Grab along the line (x-x2).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseLineGrab
		{
			set
			{
				SetArgument("useLineGrab", value);
			}
		}

		/// <summary>
		/// Use 2 point.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool PointsX4grab
		{
			set
			{
				SetArgument("pointsX4grab", value);
			}
		}

		/// <summary>
		/// Use 2 point.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FromEA
		{
			set
			{
				SetArgument("fromEA", value);
			}
		}

		/// <summary>
		/// Toggle surface grab on. Requires pos1,pos2,pos3 and pos4 to be specified.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SurfaceGrab
		{
			set
			{
				SetArgument("surfaceGrab", value);
			}
		}

		/// <summary>
		/// LevelIndex of instance to grab (-1 = world coordinates).
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int InstanceIndex
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("instanceIndex", value);
			}
		}

		/// <summary>
		/// BoundIndex of part on instance to grab (0 = just use instance coordinates).
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// </remarks>
		public int InstancePartIndex
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				SetArgument("instancePartIndex", value);
			}
		}

		/// <summary>
		/// Once a constraint is made, keep reaching with whatever hand is allowed - no matter what the angle/distance and whether or not the constraint has broken due to constraintForce  GT  grabStrength.  mmmtodo this is a badly named parameter.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool DontLetGo
		{
			set
			{
				SetArgument("dontLetGo", value);
			}
		}

		/// <summary>
		/// Stiffness of upper body. Scales the arm grab such that the armStiffness is default when this is at default value.
		/// </summary>
		/// <remarks>
		/// Default value = 11.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float BodyStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("bodyStiffness", value);
			}
		}

		/// <summary>
		/// Angle from front at which the grab activates. If the point is outside this angle from front will not try to grab.
		/// </summary>
		/// <remarks>
		/// Default value = 2.8f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float ReachAngle
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("reachAngle", value);
			}
		}

		/// <summary>
		/// Angle at which we will only reach with one hand.
		/// </summary>
		/// <remarks>
		/// Default value = 1.4f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float OneSideReachAngle
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("oneSideReachAngle", value);
			}
		}

		/// <summary>
		/// Relative distance at which the grab starts.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float GrabDistance
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("grabDistance", value);
			}
		}

		/// <summary>
		/// Relative distance (additional to grabDistance - doesn't try to move inside grabDistance)at which the grab tries to use the balancer to move to the grab point.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 14.0f.
		/// </remarks>
		public float Move2Radius
		{
			set
			{
				if (value > 14.0f)
				{
					value = 14.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("move2Radius", value);
			}
		}

		/// <summary>
		/// Stiffness of the arm.
		/// </summary>
		/// <remarks>
		/// Default value = 14.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ArmStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("armStiffness", value);
			}
		}

		/// <summary>
		/// Distance to reach out towards the grab point.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float MaxReachDistance
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxReachDistance", value);
			}
		}

		/// <summary>
		/// Scale torque used to rotate hands to face normals.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float OrientationConstraintScale
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("orientationConstraintScale", value);
			}
		}

		/// <summary>
		/// When we are grabbing the max angle the wrist ccan be at before we break the grab.
		/// </summary>
		/// <remarks>
		/// Default value = 3.1f.
		/// Min value = 0.0f.
		/// Max value = 3.2f.
		/// </remarks>
		public float MaxWristAngle
		{
			set
			{
				if (value > 3.2f)
				{
					value = 3.2f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxWristAngle", value);
			}
		}

		/// <summary>
		/// If true, the character will look at targetForHeadLook after a hand grabs until the end of the behavior. (Before grabbing it looks at the grab target).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseHeadLookToTarget
		{
			set
			{
				SetArgument("useHeadLookToTarget", value);
			}
		}

		/// <summary>
		/// If true, the character will look at the grab.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool LookAtGrab
		{
			set
			{
				SetArgument("lookAtGrab", value);
			}
		}

		/// <summary>
		/// Only used if useHeadLookToTarget is true, the target in world space to look at.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 TargetForHeadLook
		{
			set
			{
				SetArgument("targetForHeadLook", value);
			}
		}
	}

	public sealed class HeadLookHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the HeadLookHelper for sending a HeadLook <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the HeadLook <see cref="Message"/> to.</param>
		public HeadLookHelper(Ped ped) : base(ped, "headLook")
		{
		}

		/// <summary>
		/// Damping of the muscles.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float Damping
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("damping", value);
			}
		}

		/// <summary>
		/// Stiffness of the muscles.
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float Stiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("stiffness", value);
			}
		}

		/// <summary>
		/// LevelIndex of object to be looked at. Vel parameters are ignored if this is non -1.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int InstanceIndex
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("instanceIndex", value);
			}
		}

		/// <summary>
		/// The velocity of the point being looked at.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = -100.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public Vector3 Vel
		{
			set
			{
				SetArgument("vel", Vector3.Clamp(value, new Vector3(-100.0f, -100.0f, -100.0f), new Vector3(100.0f, 100.0f, 100.0f)));
			}
		}

		/// <summary>
		/// The point being looked at.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Pos
		{
			set
			{
				SetArgument("pos", value);
			}
		}

		/// <summary>
		/// Flag to force always to look.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AlwaysLook
		{
			set
			{
				SetArgument("alwaysLook", value);
			}
		}

		/// <summary>
		/// Keep the eyes horizontal.  Use true for impact with cars.  Use false if you want better look at target accuracy when the character is on the floor or leaned over alot.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool EyesHorizontal
		{
			set
			{
				SetArgument("eyesHorizontal", value);
			}
		}

		/// <summary>
		/// Keep the eyes horizontal.  Use true for impact with cars.  Use false if you want better look at target accuracy when the character is on the floor or leaned over (when not leaned over the eyes are still kept horizontal if eyesHorizontal=true ) alot.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool AlwaysEyesHorizontal
		{
			set
			{
				SetArgument("alwaysEyesHorizontal", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool KeepHeadAwayFromGround
		{
			set
			{
				SetArgument("keepHeadAwayFromGround", value);
			}
		}

		/// <summary>
		/// Allow head look to twist spine.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool TwistSpine
		{
			set
			{
				SetArgument("twistSpine", value);
			}
		}
	}

	public sealed class HighFallHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the HighFallHelper for sending a HighFall <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the HighFall <see cref="Message"/> to.</param>
		public HighFallHelper(Ped ped) : base(ped, "highFall")
		{
		}

		/// <summary>
		/// Stiffness of body. Value feeds through to bodyBalance (synced with defaults), to armsWindmill (14 for this value at default ), legs pedal, head look and roll down stairs directly.
		/// </summary>
		/// <remarks>
		/// Default value = 11.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float BodyStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("bodyStiffness", value);
			}
		}

		/// <summary>
		/// The damping of the joints.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float Bodydamping
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("bodydamping", value);
			}
		}

		/// <summary>
		/// The length of time before the impact that the character transitions to the landing.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Catchfalltime
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("catchfalltime", value);
			}
		}

		/// <summary>
		/// 0.52angle is 0.868 dot//A threshold for deciding how far away from upright the character needs to be before bailing out (going into a foetal) instead of trying to land (keeping stretched out).  NB: never does bailout if ignorWorldCollisions true.
		/// </summary>
		/// <remarks>
		/// Default value = 0.9f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CrashOrLandCutOff
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("crashOrLandCutOff", value);
			}
		}

		/// <summary>
		/// Strength of the controller to keep the character at angle aimAngleBase from vertical.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float PdStrength
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("pdStrength", value);
			}
		}

		/// <summary>
		/// Damping multiplier of the controller to keep the character at angle aimAngleBase from vertical.  The actual damping is pdDamping*pdStrength*constant*angVel.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float PdDamping
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("pdDamping", value);
			}
		}

		/// <summary>
		/// Arm circling speed in armWindMillAdaptive.
		/// </summary>
		/// <remarks>
		/// Default value = 7.9f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float ArmAngSpeed
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armAngSpeed", value);
			}
		}

		/// <summary>
		/// In armWindMillAdaptive.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ArmAmplitude
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armAmplitude", value);
			}
		}

		/// <summary>
		/// In armWindMillAdaptive 3.1 opposite for stuntman.
		/// 1.0 old default. 0.0 in phase.
		/// </summary>
		/// <remarks>
		/// Default value = 3.1f.
		/// Min value = 0.0f.
		/// Max value = 6.3f.
		/// </remarks>
		public float ArmPhase
		{
			set
			{
				if (value > 6.3f)
				{
					value = 6.3f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armPhase", value);
			}
		}

		/// <summary>
		/// In armWindMillAdaptive bend the elbows as a function of armAngle.
		/// For stunt man true otherwise false.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ArmBendElbows
		{
			set
			{
				SetArgument("armBendElbows", value);
			}
		}

		/// <summary>
		/// Radius of legs on pedal.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 0.5f.
		/// </remarks>
		public float LegRadius
		{
			set
			{
				if (value > 0.5f)
				{
					value = 0.5f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legRadius", value);
			}
		}

		/// <summary>
		/// In pedal.
		/// </summary>
		/// <remarks>
		/// Default value = 7.9f.
		/// Min value = 0.0f.
		/// Max value = 15.0f.
		/// </remarks>
		public float LegAngSpeed
		{
			set
			{
				if (value > 15.0f)
				{
					value = 15.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legAngSpeed", value);
			}
		}

		/// <summary>
		/// 0.0 for stuntman.  Random offset applied per leg to the angular speed to desynchronize the pedaling - set to 0 to disable, otherwise should be set to less than the angularSpeed value.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = -10.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float LegAsymmetry
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -10.0f)
				{
					value = -10.0f;
				}

				SetArgument("legAsymmetry", value);
			}
		}

		/// <summary>
		/// Phase angle between the arms and legs circling angle.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 6.5f.
		/// </remarks>
		public float Arms2LegsPhase
		{
			set
			{
				if (value > 6.5f)
				{
					value = 6.5f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("arms2LegsPhase", value);
			}
		}

		/// <summary>
		/// Syncs the arms angle to what the leg angle is.
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="Synchroisation.AlwaysSynced"/>.
		/// All speed/direction parameters of armswindmill are overwritten if = <see cref="Synchroisation.AlwaysSynced"/>.
		/// If <see cref="Synchroisation.SyncedAtStart"/> and you want synced arms/legs then armAngSpeed=legAngSpeed, legAsymmetry = 0.0 (to stop randomizations of the leg cicle speed).
		/// </remarks>
		public Synchroisation Arms2LegsSync
		{
			set
			{
				SetArgument("arms2LegsSync", (int)value);
			}
		}

		/// <summary>
		/// Where to put the arms when preparing to land.
		/// Approx 1 = above head, 0 = head height, -1 = down.
		/// LT -2.0 use catchFall arms, LT -3.0 use prepare for landing pose if Agent is due to land vertically, feet first.
		/// </summary>
		/// <remarks>
		/// Default value = -3.1f.
		/// Min value = -4.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmsUp
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < -4.0f)
				{
					value = -4.0f;
				}

				SetArgument("armsUp", value);
			}
		}

		/// <summary>
		/// Toggle to orientate to fall direction.
		/// i.e. orientate so that the character faces the horizontal velocity direction.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool OrientateBodyToFallDirection
		{
			set
			{
				SetArgument("orientateBodyToFallDirection", value);
			}
		}

		/// <summary>
		/// If false don't worry about the twist angle of the character when orientating the character.
		/// If false this allows the twist axis of the character to be free (You can get a nice twisting highFall like the one in dieHard 4 when the car goes into the helicopter).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool OrientateTwist
		{
			set
			{
				SetArgument("orientateTwist", value);
			}
		}

		/// <summary>
		/// DEVEL parameter - suggest you don't edit it.
		/// Maximum torque the orientation controller can apply.
		/// If 0 then no helper torques will be used.
		/// 300 will orientate the character softly for all but extreme angles away from aimAngleBase.
		/// If abs (current -aimAngleBase) is getting near 3.0 then this can be reduced to give a softer feel.
		/// </summary>
		/// <remarks>
		/// Default value = 300.0f.
		/// Min value = 0.0f.
		/// Max value = 2000.0f.
		/// </remarks>
		public float OrientateMax
		{
			set
			{
				if (value > 2000.0f)
				{
					value = 2000.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("orientateMax", value);
			}
		}

		/// <summary>
		/// If true then orientate the character to face the point from where it started falling.
		/// High fall like the one in "Die Hard" with Alan Rickman.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AlanRickman
		{
			set
			{
				SetArgument("alanRickman", value);
			}
		}

		/// <summary>
		/// Try to execute a forward Roll on landing.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FowardRoll
		{
			set
			{
				SetArgument("fowardRoll", value);
			}
		}

		/// <summary>
		/// Blend to a zero pose when forward roll is attempted.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseZeroPose_withFowardRoll
		{
			set
			{
				SetArgument("useZeroPose_withFowardRoll", value);
			}
		}

		/// <summary>
		/// Angle from vertical the pdController is driving to (positive = forwards).
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -3.1f.
		/// Max value = 3.1f.
		/// </remarks>
		public float AimAngleBase
		{
			set
			{
				if (value > 3.1f)
				{
					value = 3.1f;
				}

				if (value < -3.1f)
				{
					value = -3.1f;
				}

				SetArgument("aimAngleBase", value);
			}
		}

		/// <summary>
		/// Scale to add/subtract from aimAngle based on forward speed (Internal).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FowardVelRotation
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("fowardVelRotation", value);
			}
		}

		/// <summary>
		/// Scale to change to amount of vel that is added to the foot ik from the velocity (Internal).
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FootVelCompScale
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("footVelCompScale", value);
			}
		}

		/// <summary>
		/// Side offset for the feet during prepareForLanding. +ve = right.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SideD
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("sideD", value);
			}
		}

		/// <summary>
		/// Forward offset for the feet during prepareForLanding.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FowardOffsetOfLegIK
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("fowardOffsetOfLegIK", value);
			}
		}

		/// <summary>
		/// Leg Length for ik (Internal)//unused.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float LegL
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legL", value);
			}
		}

		/// <summary>
		/// 0.5angle is 0.878 dot. Cutoff to go to the catchFall ( internal) //mmmtodo do like crashOrLandCutOff.
		/// </summary>
		/// <remarks>
		/// Default value = 0.9f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CatchFallCutOff
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("catchFallCutOff", value);
			}
		}

		/// <summary>
		/// Strength of the legs at landing.
		/// </summary>
		/// <remarks>
		/// Default value = 12.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float LegStrength
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("legStrength", value);
			}
		}

		/// <summary>
		/// If true have enough strength to balance.  If false not enough strength in legs to balance (even though bodyBlance called).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool Balance
		{
			set
			{
				SetArgument("balance", value);
			}
		}

		/// <summary>
		/// Never go into bailout (foetal).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool IgnorWorldCollisions
		{
			set
			{
				SetArgument("ignorWorldCollisions", value);
			}
		}

		/// <summary>
		/// Stunt man type fall.
		/// Arm and legs circling direction controlled by angmom and orientation.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool AdaptiveCircling
		{
			set
			{
				SetArgument("adaptiveCircling", value);
			}
		}

		/// <summary>
		/// With stunt man type fall.
		/// Hula reaction if can't see floor and not rotating fast.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool Hula
		{
			set
			{
				SetArgument("hula", value);
			}
		}

		/// <summary>
		/// Character needs to be moving less than this speed to consider fall as a recoverable one.
		/// </summary>
		/// <remarks>
		/// Default value = 15.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float MaxSpeedForRecoverableFall
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxSpeedForRecoverableFall", value);
			}
		}

		/// <summary>
		/// Character needs to be moving at least this fast horizontally to start bracing for impact if there is an object along its trajectory.
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float MinSpeedForBrace
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("minSpeedForBrace", value);
			}
		}

		/// <summary>
		/// Ray-cast normal doted with up direction has to be greater than this number to consider object flat enough to land on it.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LandingNormal
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("landingNormal", value);
			}
		}
	}

	public sealed class IncomingTransformsHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the IncomingTransformsHelper for sending a IncomingTransforms <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the IncomingTransforms <see cref="Message"/> to.</param>
		public IncomingTransformsHelper(Ped ped) : base(ped, "incomingTransforms")
		{
		}
	}

	/// <summary>
	/// InjuredOnGround.
	/// </summary>
	public sealed class InjuredOnGroundHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the InjuredOnGroundHelper for sending a InjuredOnGround <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the InjuredOnGround <see cref="Message"/> to.</param>
		/// <remarks>
		/// InjuredOnGround.
		/// </remarks>
		public InjuredOnGroundHelper(Ped ped) : base(ped, "injuredOnGround")
		{
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int NumInjuries
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("numInjuries", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// </remarks>
		public int Injury1Component
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				SetArgument("injury1Component", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// </remarks>
		public int Injury2Component
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				SetArgument("injury2Component", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Injury1LocalPosition
		{
			set
			{
				SetArgument("injury1LocalPosition", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Injury2LocalPosition
		{
			set
			{
				SetArgument("injury2LocalPosition", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(1.0f, 0.0f, 0.0f).
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public Vector3 Injury1LocalNormal
		{
			set
			{
				SetArgument("injury1LocalNormal", Vector3.Clamp(value, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(1.0f, 0.0f, 0.0f).
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public Vector3 Injury2LocalNormal
		{
			set
			{
				SetArgument("injury2LocalNormal", Vector3.Clamp(value, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(1.0f, 0.0f, 0.0f).
		/// Min value = 0.0f.
		/// </remarks>
		public Vector3 AttackerPos
		{
			set
			{
				SetArgument("attackerPos", Vector3.Maximize(value, new Vector3(0.0f, 0.0f, 0.0f)));
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool DontReachWithLeft
		{
			set
			{
				SetArgument("dontReachWithLeft", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool DontReachWithRight
		{
			set
			{
				SetArgument("dontReachWithRight", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool StrongRollForce
		{
			set
			{
				SetArgument("strongRollForce", value);
			}
		}
	}

	/// <summary>
	/// Carried.
	/// </summary>
	public sealed class CarriedHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the CarriedHelper for sending a Carried <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the Carried <see cref="Message"/> to.</param>
		/// <remarks>
		/// Carried.
		/// </remarks>
		public CarriedHelper(Ped ped) : base(ped, "carried")
		{
		}
	}

	/// <summary>
	/// Dangle.
	/// </summary>
	public sealed class DangleHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the DangleHelper for sending a Dangle <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the Dangle <see cref="Message"/> to.</param>
		/// <remarks>
		/// Dangle.
		/// </remarks>
		public DangleHelper(Ped ped) : base(ped, "dangle")
		{
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool DoGrab
		{
			set
			{
				SetArgument("doGrab", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float GrabFrequency
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("grabFrequency", value);
			}
		}
	}

	public sealed class OnFireHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the OnFireHelper for sending a OnFire <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the OnFire <see cref="Message"/> to.</param>
		public OnFireHelper(Ped ped) : base(ped, "onFire")
		{
		}

		/// <summary>
		/// Max time for stumbling around before falling to ground.
		/// </summary>
		/// <remarks>
		/// Default value = 2.5f.
		/// Min value = 0.0f.
		/// Max value = 30.0f.
		/// </remarks>
		public float StaggerTime
		{
			set
			{
				if (value > 30.0f)
				{
					value = 30.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("staggerTime", value);
			}
		}

		/// <summary>
		/// How quickly the character leans hips when staggering.
		/// </summary>
		/// <remarks>
		/// Default value = 0.9f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float StaggerLeanRate
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("staggerLeanRate", value);
			}
		}

		/// <summary>
		/// Max the character leans hips back when staggering.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 1.5f.
		/// </remarks>
		public float StumbleMaxLeanBack
		{
			set
			{
				if (value > 1.5f)
				{
					value = 1.5f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stumbleMaxLeanBack", value);
			}
		}

		/// <summary>
		/// Max the character leans hips forwards when staggering.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.5f.
		/// </remarks>
		public float StumbleMaxLeanForward
		{
			set
			{
				if (value > 1.5f)
				{
					value = 1.5f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stumbleMaxLeanForward", value);
			}
		}

		/// <summary>
		/// Blend armsWindmill with the bodyWrithe arms when character is upright.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ArmsWindmillWritheBlend
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armsWindmillWritheBlend", value);
			}
		}

		/// <summary>
		/// Blend spine stumble with the bodyWrithe spine when character is upright.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SpineStumbleWritheBlend
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spineStumbleWritheBlend", value);
			}
		}

		/// <summary>
		/// Blend legs stumble with the bodyWrithe legs when character is upright.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LegsStumbleWritheBlend
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legsStumbleWritheBlend", value);
			}
		}

		/// <summary>
		/// Blend the bodyWrithe arms with the current desired pose from on fire behavior when character is on the floor.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ArmsPoseWritheBlend
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armsPoseWritheBlend", value);
			}
		}

		/// <summary>
		/// Blend the bodyWrithe back with the current desired pose from on fire behavior when character is on the floor.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SpinePoseWritheBlend
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spinePoseWritheBlend", value);
			}
		}

		/// <summary>
		/// Blend the bodyWrithe legs with the current desired pose from on fire behavior when character is on the floor.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LegsPoseWritheBlend
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legsPoseWritheBlend", value);
			}
		}

		/// <summary>
		/// Flag to set bodyWrithe trying to rollOver.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool RollOverFlag
		{
			set
			{
				SetArgument("rollOverFlag", value);
			}
		}

		/// <summary>
		/// Scale rolling torque that is applied to character spine by bodyWrithe. Torque magnitude is calculated with the following formula: m_rollOverDirection*rollOverPhase*rollTorqueScale.
		/// </summary>
		/// <remarks>
		/// Default value = 25.0f.
		/// Min value = 0.0f.
		/// Max value = 300.0f.
		/// </remarks>
		public float RollTorqueScale
		{
			set
			{
				if (value > 300.0f)
				{
					value = 300.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rollTorqueScale", value);
			}
		}

		/// <summary>
		/// Character pose depends on character facing direction that is evaluated from its COMTM orientation.
		/// Set this value to 0 to use no orientation prediction i.e. current character COMTM orientation will be used to determine character facing direction and finally the pose bodyWrithe is blending to.
		/// Set this value to  GT  0 to predict character COMTM orientation this amount of time in seconds to the future.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float PredictTime
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("predictTime", value);
			}
		}

		/// <summary>
		/// Rolling torque is ramped down over time. At this time in seconds torque value converges to zero. Use this parameter to restrict time the character is rolling.
		/// </summary>
		/// <remarks>
		/// Default value = 8.0f.
		/// Min value = 0.0f.
		/// Max value = 60.0f.
		/// </remarks>
		public float MaxRollOverTime
		{
			set
			{
				if (value > 60.0f)
				{
					value = 60.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxRollOverTime", value);
			}
		}

		/// <summary>
		/// Rolling torque is ramped down with distance measured from position where character hit the ground and started rolling. At this distance in meters torque value converges to zero. Use this parameter to restrict distance the character travels due to rolling.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float RollOverRadius
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rollOverRadius", value);
			}
		}
	}

	public sealed class PedalLegsHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the PedalLegsHelper for sending a PedalLegs <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the PedalLegs <see cref="Message"/> to.</param>
		public PedalLegsHelper(Ped ped) : base(ped, "pedalLegs")
		{
		}

		/// <summary>
		/// Pedal with this leg or not.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool PedalLeftLeg
		{
			set
			{
				SetArgument("pedalLeftLeg", value);
			}
		}

		/// <summary>
		/// Pedal with this leg or not.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool PedalRightLeg
		{
			set
			{
				SetArgument("pedalRightLeg", value);
			}
		}

		/// <summary>
		/// Pedal forwards or backwards.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool BackPedal
		{
			set
			{
				SetArgument("backPedal", value);
			}
		}

		/// <summary>
		/// Base radius of pedal action.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float Radius
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("radius", value);
			}
		}

		/// <summary>
		/// Rate of pedaling. If adaptivePedal4Dragging is true then the legsAngularSpeed calculated to match the linear speed of the character can have a maximum value of angularSpeed (this max used to be hard coded to 13.0).
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float AngularSpeed
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("angularSpeed", value);
			}
		}

		/// <summary>
		/// Stiffness of legs.
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float LegStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("legStiffness", value);
			}
		}

		/// <summary>
		/// Move the center of the pedal for the left leg up by this amount, the right leg down by this amount.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float PedalOffset
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("pedalOffset", value);
			}
		}

		/// <summary>
		/// Random seed used to generate speed changes.
		/// </summary>
		/// <remarks>
		/// Default value = 100.
		/// Min value = 0.
		/// </remarks>
		public int RandomSeed
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				SetArgument("randomSeed", value);
			}
		}

		/// <summary>
		/// Random offset applied per leg to the angular speed to desynchronize the pedaling - set to 0 to disable, otherwise should be set to less than the angularSpeed value.
		/// </summary>
		/// <remarks>
		/// Default value = 8.0f.
		/// Min value = -10.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SpeedAsymmetry
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -10.0f)
				{
					value = -10.0f;
				}

				SetArgument("speedAsymmetry", value);
			}
		}

		/// <summary>
		/// Will pedal in the direction of travel (if backPedal = false, against travel if backPedal = true) and with an angular velocity relative to speed upto a maximum of 13(rads/sec).  Use when being dragged by a car.  Overrides angularSpeed.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AdaptivePedal4Dragging
		{
			set
			{
				SetArgument("adaptivePedal4Dragging", value);
			}
		}

		/// <summary>
		/// NewAngularSpeed = Clamp(angSpeedMultiplier4Dragging * linear_speed/pedalRadius, 0.0, angularSpeed).
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float AngSpeedMultiplier4Dragging
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("angSpeedMultiplier4Dragging", value);
			}
		}

		/// <summary>
		/// 0-1 value used to add variance to the radius value while pedalling, to desynchonize the legs' movement and provide some variety.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float RadiusVariance
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("radiusVariance", value);
			}
		}

		/// <summary>
		/// 0-1 value used to vary the angle of the legs from the hips during the pedal.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LegAngleVariance
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legAngleVariance", value);
			}
		}

		/// <summary>
		/// Move the center of the pedal for both legs sideways (+ve = right).  NB: not applied to hula.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CentreSideways
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("centreSideways", value);
			}
		}

		/// <summary>
		/// Move the center of the pedal for both legs forward (or backward -ve).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CentreForwards
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("centreForwards", value);
			}
		}

		/// <summary>
		/// Move the center of the pedal for both legs up (or down -ve).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CentreUp
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("centreUp", value);
			}
		}

		/// <summary>
		/// Turn the circle into an ellipse.  Ellipse has horizontal radius a and vertical radius b.  If ellipse is +ve then a=radius*ellipse and b=radius.  If ellipse is -ve then a=radius and b = radius*ellipse.  0.0 = vertical line of length 2*radius, 0.0:1.0 circle squashed horizontally (vertical radius = radius), 1.0=circle.  -0.001 = horizontal line of length 2*radius, -0.0:-1.0 circle squashed vertically (horizontal radius = radius), -1.0 = circle.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Ellipse
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("ellipse", value);
			}
		}

		/// <summary>
		/// How much to account for the target moving through space rather than being static.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float DragReduction
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("dragReduction", value);
			}
		}

		/// <summary>
		/// Spread legs.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Spread
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("spread", value);
			}
		}

		/// <summary>
		/// If true circle the legs in a hula motion.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Hula
		{
			set
			{
				SetArgument("hula", value);
			}
		}
	}

	/// <summary>
	/// BEHAVIOURS REFERENCED: AnimPose - allows animPose to override body parts: Arms (useLeftArm, useRightArm).
	/// </summary>
	public sealed class PointArmHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the PointArmHelper for sending a PointArm <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the PointArm <see cref="Message"/> to.</param>
		/// <remarks>
		/// BEHAVIOURS REFERENCED: AnimPose - allows animPose to override body parts: Arms (useLeftArm, useRightArm).
		/// </remarks>
		public PointArmHelper(Ped ped) : base(ped, "pointArm")
		{
		}

		/// <summary>
		/// Point to point to (in world space).
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 TargetLeft
		{
			set
			{
				SetArgument("targetLeft", value);
			}
		}

		/// <summary>
		/// Twist of the arm around point direction.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TwistLeft
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("twistLeft", value);
			}
		}

		/// <summary>
		/// Values less than 1 can give the arm a more bent look.
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmStraightnessLeft
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armStraightnessLeft", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseLeftArm
		{
			set
			{
				SetArgument("useLeftArm", value);
			}
		}

		/// <summary>
		/// Stiffness of arm.
		/// </summary>
		/// <remarks>
		/// Default value = 15.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ArmStiffnessLeft
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("armStiffnessLeft", value);
			}
		}

		/// <summary>
		/// Damping value for arm used to point.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmDampingLeft
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armDampingLeft", value);
			}
		}

		/// <summary>
		/// Level index of thing to point at, or -1 for none. if -1, target is specified in world space, otherwise it is an offset from the object specified by this index.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int InstanceIndexLeft
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("instanceIndexLeft", value);
			}
		}

		/// <summary>
		/// Swing limit.
		/// </summary>
		/// <remarks>
		/// Default value = 1.5f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float PointSwingLimitLeft
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("pointSwingLimitLeft", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseZeroPoseWhenNotPointingLeft
		{
			set
			{
				SetArgument("useZeroPoseWhenNotPointingLeft", value);
			}
		}

		/// <summary>
		/// Point to point to (in world space).
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 TargetRight
		{
			set
			{
				SetArgument("targetRight", value);
			}
		}

		/// <summary>
		/// Twist of the arm around point direction.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TwistRight
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("twistRight", value);
			}
		}

		/// <summary>
		/// Values less than 1 can give the arm a more bent look.
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmStraightnessRight
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armStraightnessRight", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseRightArm
		{
			set
			{
				SetArgument("useRightArm", value);
			}
		}

		/// <summary>
		/// Stiffness of arm.
		/// </summary>
		/// <remarks>
		/// Default value = 15.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ArmStiffnessRight
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("armStiffnessRight", value);
			}
		}

		/// <summary>
		/// Damping value for arm used to point.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmDampingRight
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armDampingRight", value);
			}
		}

		/// <summary>
		/// Level index of thing to point at, or -1 for none. if -1, target is specified in world space, otherwise it is an offset from the object specified by this index.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int InstanceIndexRight
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("instanceIndexRight", value);
			}
		}

		/// <summary>
		/// Swing limit.
		/// </summary>
		/// <remarks>
		/// Default value = 1.5f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float PointSwingLimitRight
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("pointSwingLimitRight", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseZeroPoseWhenNotPointingRight
		{
			set
			{
				SetArgument("useZeroPoseWhenNotPointingRight", value);
			}
		}
	}

	public sealed class PointGunHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the PointGunHelper for sending a PointGun <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the PointGun <see cref="Message"/> to.</param>
		public PointGunHelper(Ped ped) : base(ped, "pointGun")
		{
		}

		/// <summary>
		/// Allow right hand to point/support?.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool EnableRight
		{
			set
			{
				SetArgument("enableRight", value);
			}
		}

		/// <summary>
		/// Allow right hand to point/support?.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool EnableLeft
		{
			set
			{
				SetArgument("enableLeft", value);
			}
		}

		/// <summary>
		/// Target for the left Hand.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 LeftHandTarget
		{
			set
			{
				SetArgument("leftHandTarget", value);
			}
		}

		/// <summary>
		/// Index of the object that the left hand target is specified in, -1 is world space.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// </remarks>
		public int LeftHandTargetIndex
		{
			set
			{
				SetArgument("leftHandTargetIndex", value);
			}
		}

		/// <summary>
		/// Target for the right Hand.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 RightHandTarget
		{
			set
			{
				SetArgument("rightHandTarget", value);
			}
		}

		/// <summary>
		/// Index of the object that the right hand target is specified in, -1 is world space.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// </remarks>
		public int RightHandTargetIndex
		{
			set
			{
				SetArgument("rightHandTargetIndex", value);
			}
		}

		/// <summary>
		/// NB: Only Applied to single handed weapons (some more work is required to have this tech on two handed weapons). Amount to lead target based on target velocity relative to the chest.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float LeadTarget
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leadTarget", value);
			}
		}

		/// <summary>
		/// Stiffness of the arm.
		/// </summary>
		/// <remarks>
		/// Default value = 14.0f.
		/// Min value = 2.0f.
		/// Max value = 15.0f.
		/// </remarks>
		public float ArmStiffness
		{
			set
			{
				if (value > 15.0f)
				{
					value = 15.0f;
				}

				if (value < 2.0f)
				{
					value = 2.0f;
				}

				SetArgument("armStiffness", value);
			}
		}

		/// <summary>
		/// Stiffness of the arm on pointing arm when a support arm is detached from a two-handed weapon.
		/// </summary>
		/// <remarks>
		/// Default value = 8.0f.
		/// Min value = 2.0f.
		/// Max value = 15.0f.
		/// </remarks>
		public float ArmStiffnessDetSupport
		{
			set
			{
				if (value > 15.0f)
				{
					value = 15.0f;
				}

				if (value < 2.0f)
				{
					value = 2.0f;
				}

				SetArgument("armStiffnessDetSupport", value);
			}
		}

		/// <summary>
		/// Damping.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.1f.
		/// Max value = 5.0f.
		/// </remarks>
		public float ArmDamping
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("armDamping", value);
			}
		}

		/// <summary>
		/// Amount of gravity opposition on pointing arm.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float GravityOpposition
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("gravityOpposition", value);
			}
		}

		/// <summary>
		/// Amount of gravity opposition on pointing arm when a support arm is detached from a two-handed weapon.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float GravOppDetachedSupport
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("gravOppDetachedSupport", value);
			}
		}

		/// <summary>
		/// Amount of mass of weapon taken into account by gravity opposition on pointing arm when a support arm is detached from a two-handed weapon.  The lower the value the more the character doesn't know about the weapon mass and therefore is more affected by it.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float MassMultDetachedSupport
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("massMultDetachedSupport", value);
			}
		}

		/// <summary>
		/// Allow shot to set a lower arm muscleStiffness than pointGun normally would.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AllowShotLooseness
		{
			set
			{
				SetArgument("allowShotLooseness", value);
			}
		}

		/// <summary>
		/// How much of blend should come from incoming transforms 0(all IK) .. 1(all ITMs)   For pointing arms only.  (Support arm uses the IK solution as is for clavicles).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ClavicleBlend
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("clavicleBlend", value);
			}
		}

		/// <summary>
		/// Controls arm twist. (except in pistolIK).
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ElbowAttitude
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("elbowAttitude", value);
			}
		}

		/// <summary>
		/// Type of constraint between the support hand and gun.  0=no constraint, 1=hard distance constraint, 2=Force based constraint, 3=hard spherical constraint.
		/// </summary>
		/// <remarks>
		/// Default value = 1.
		/// Min value = 0.
		/// Max value = 3.
		/// </remarks>
		public int SupportConstraint
		{
			set
			{
				if (value > 3)
				{
					value = 3;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("supportConstraint", value);
			}
		}

		/// <summary>
		/// For supportConstraint = 1: Support hand constraint distance will be slowly reduced until it hits this value.  This is for stability and also allows the pointing arm to lead a little.  Don't set lower than NM_MIN_STABLE_DISTANCECONSTRAINT_DISTANCE 0.001f.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 0.1f.
		/// </remarks>
		public float ConstraintMinDistance
		{
			set
			{
				if (value > 0.1f)
				{
					value = 0.1f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("constraintMinDistance", value);
			}
		}

		/// <summary>
		/// For supportConstraint = 1:  Minimum distance within which support hand constraint will be made.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float MakeConstraintDistance
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("makeConstraintDistance", value);
			}
		}

		/// <summary>
		/// For supportConstraint = 1:  Velocity at which to reduce the support hand constraint length.
		/// </summary>
		/// <remarks>
		/// Default value = 1.5f.
		/// Min value = 0.1f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ReduceConstraintLengthVel
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("reduceConstraintLengthVel", value);
			}
		}

		/// <summary>
		/// For supportConstraint = 1: strength of the supporting hands constraint (kg m/s), -1 to ignore/disable.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float BreakingStrength
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("breakingStrength", value);
			}
		}

		/// <summary>
		/// Once constraint is broken then do not try to reconnect/support for this amount of time.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float BrokenSupportTime
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("brokenSupportTime", value);
			}
		}

		/// <summary>
		/// Probability that the when a constraint is broken that during brokenSupportTime a side pose will be selected.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float BrokenToSideProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("brokenToSideProb", value);
			}
		}

		/// <summary>
		/// If gunArm has been controlled by other behaviors for this time when it could have been pointing but couldn't due to pointing only allowed if connected, change gunArm pose to something that could connect for connectFor seconds.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float ConnectAfter
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("connectAfter", value);
			}
		}

		/// <summary>
		/// Time to try to reconnect for.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float ConnectFor
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("connectFor", value);
			}
		}

		/// <summary>
		/// 0 = don't allow, 1= allow for kPistol(two handed pistol) only, 2 = allow for kRifle only, 3 = allow for kPistol and kRifle. Allow one handed pointing - no constraint if cant be supported .  If not allowed then gunHand does not try to point at target if it cannot be supported - the constraint will be controlled by always support.
		/// </summary>
		/// <remarks>
		/// Default value = 1.
		/// Min value = 0.
		/// Max value = 3.
		/// </remarks>
		public int OneHandedPointing
		{
			set
			{
				if (value > 3)
				{
					value = 3;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("oneHandedPointing", value);
			}
		}

		/// <summary>
		/// Support a non pointing gunHand i.e. if in zero pose (constrain as well  if constraint possible).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AlwaysSupport
		{
			set
			{
				SetArgument("alwaysSupport", value);
			}
		}

		/// <summary>
		/// Apply neutral pose when a gun arm isn't in use.  NB: at the moment Rifle hand is always controlled by pointGun.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool PoseUnusedGunArm
		{
			set
			{
				SetArgument("poseUnusedGunArm", value);
			}
		}

		/// <summary>
		/// Apply neutral pose when a support arm isn't in use.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool PoseUnusedSupportArm
		{
			set
			{
				SetArgument("poseUnusedSupportArm", value);
			}
		}

		/// <summary>
		/// Apply neutral pose to the non-gun arm (otherwise it is always under the control of other behaviors or not set). If the non-gun hand is a supporting hand it is not controlled by this parameter but by poseUnusedSupportArm.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool PoseUnusedOtherArm
		{
			set
			{
				SetArgument("poseUnusedOtherArm", value);
			}
		}

		/// <summary>
		/// Max aiming angle(deg) sideways across body midline measured from chest forward that the character will try to point.
		/// </summary>
		/// <remarks>
		/// Default value = 90.0f.
		/// Min value = 0.0f.
		/// Max value = 180.0f.
		/// </remarks>
		public float MaxAngleAcross
		{
			set
			{
				if (value > 180.0f)
				{
					value = 180.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxAngleAcross", value);
			}
		}

		/// <summary>
		/// Max aiming angle(deg) sideways away from body midline measured from chest forward that the character will try to point.
		/// </summary>
		/// <remarks>
		/// Default value = 90.0f.
		/// Min value = 0.0f.
		/// Max value = 180.0f.
		/// </remarks>
		public float MaxAngleAway
		{
			set
			{
				if (value > 180.0f)
				{
					value = 180.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxAngleAway", value);
			}
		}

		/// <summary>
		/// 0= don't apply limits.  1=apply the limits below only when the character is falling.  2 =  always apply these limits (instead of applying maxAngleAcross and maxAngleAway which only limits the horizontal angle but implicity limits the updown (the limit shape is a vertical hinge).
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int FallingLimits
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("fallingLimits", value);
			}
		}

		/// <summary>
		/// Max aiming angle(deg) sideways across body midline measured from chest forward that the character will try to point.  i.e. for rightHanded gun this is the angle left of the midline.
		/// </summary>
		/// <remarks>
		/// Default value = 90.0f.
		/// Min value = 0.0f.
		/// Max value = 180.0f.
		/// </remarks>
		public float AcrossLimit
		{
			set
			{
				if (value > 180.0f)
				{
					value = 180.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("acrossLimit", value);
			}
		}

		/// <summary>
		/// Max aiming angle(deg) sideways away from body midline measured from chest forward that the character will try to point.  i.e. for rightHanded gun this is the angle right of the midline.
		/// </summary>
		/// <remarks>
		/// Default value = 90.0f.
		/// Min value = 0.0f.
		/// Max value = 180.0f.
		/// </remarks>
		public float AwayLimit
		{
			set
			{
				if (value > 180.0f)
				{
					value = 180.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("awayLimit", value);
			}
		}

		/// <summary>
		/// Max aiming angle(deg) upwards from body midline measured from chest forward that the character will try to point.
		/// </summary>
		/// <remarks>
		/// Default value = 90.0f.
		/// Min value = 0.0f.
		/// Max value = 180.0f.
		/// </remarks>
		public float UpLimit
		{
			set
			{
				if (value > 180.0f)
				{
					value = 180.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("upLimit", value);
			}
		}

		/// <summary>
		/// Max aiming angle(deg) downwards from body midline measured from chest forward that the character will try to point.
		/// </summary>
		/// <remarks>
		/// Default value = 45.0f.
		/// Min value = 0.0f.
		/// Max value = 180.0f.
		/// </remarks>
		public float DownLimit
		{
			set
			{
				if (value > 180.0f)
				{
					value = 180.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("downLimit", value);
			}
		}

		/// <summary>
		/// Pose the rifle hand to reduce complications with collisions. 0 = false, 1 = always when falling, 2 = when falling except if falling backwards.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int RifleFall
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("rifleFall", value);
			}
		}

		/// <summary>
		/// Allow supporting of a rifle(or two handed pistol) when falling. 0 = false, 1 = support if allowed, 2 = support until constraint not active (don't allow support to restart), 3 = support until constraint not effective (support hand to support distance must be less than 0.15 - don't allow support to restart).
		/// </summary>
		/// <remarks>
		/// Default value = 1.
		/// Min value = 0.
		/// Max value = 3.
		/// </remarks>
		public int FallingSupport
		{
			set
			{
				if (value > 3)
				{
					value = 3;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("fallingSupport", value);
			}
		}

		/// <summary>
		/// What is considered a fall by fallingSupport). Apply fallingSupport 0=never(will support if allowed), 1 = falling, 2 = falling except if falling backwards, 3 = falling and collided, 4 = falling and collided except if falling backwards, 5 = falling except if falling backwards until collided.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 5.
		/// </remarks>
		public int FallingTypeSupport
		{
			set
			{
				if (value > 5)
				{
					value = 5;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("fallingTypeSupport", value);
			}
		}

		/// <summary>
		/// 0 = byFace, 1=acrossFront, 2=bySide.  NB: bySide is not connectible so be careful if combined with kPistol and oneHandedPointing = 0 or 2.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int PistolNeutralType
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("pistolNeutralType", value);
			}
		}

		/// <summary>
		/// NOT IMPLEMENTED YET KEEP=false - use pointing for neutral targets in pistol modes.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool NeutralPoint4Pistols
		{
			set
			{
				SetArgument("neutralPoint4Pistols", value);
			}
		}

		/// <summary>
		/// Use pointing for neutral targets in rifle mode.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool NeutralPoint4Rifle
		{
			set
			{
				SetArgument("neutralPoint4Rifle", value);
			}
		}

		/// <summary>
		/// Check the neutral pointing is pointable, if it isn't then choose a neutral pose instead.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool CheckNeutralPoint
		{
			set
			{
				SetArgument("checkNeutralPoint", value);
			}
		}

		/// <summary>
		/// Side, up, back) side is left for left arm, right for right arm mmmmtodo.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(5.0f, -5.0f, -2.0f).
		/// </remarks>
		public Vector3 Point2Side
		{
			set
			{
				SetArgument("point2Side", value);
			}
		}

		/// <summary>
		/// Add to weaponDistance for point2Side neutral pointing (to straighten the arm).
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = -1.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float Add2WeaponDistSide
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("add2WeaponDistSide", value);
			}
		}

		/// <summary>
		/// Side, up, back) side is left for left arm, right for rght arm mmmmtodo.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(-1.0f, -0.9f, -0.2f).
		/// </remarks>
		public Vector3 Point2Connect
		{
			set
			{
				SetArgument("point2Connect", value);
			}
		}

		/// <summary>
		/// Add to weaponDistance for point2Connect neutral pointing (to straighten the arm).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float Add2WeaponDistConnect
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("add2WeaponDistConnect", value);
			}
		}

		/// <summary>
		/// Enable new ik for pistol pointing.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UsePistolIK
		{
			set
			{
				SetArgument("usePistolIK", value);
			}
		}

		/// <summary>
		/// Use spine twist to orient chest?.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseSpineTwist
		{
			set
			{
				SetArgument("useSpineTwist", value);
			}
		}

		/// <summary>
		/// Turn balancer to help gun point at target.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseTurnToTarget
		{
			set
			{
				SetArgument("useTurnToTarget", value);
			}
		}

		/// <summary>
		/// Use head look to drive head?.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseHeadLook
		{
			set
			{
				SetArgument("useHeadLook", value);
			}
		}

		/// <summary>
		/// Angular difference between pointing direction and target direction above which feedback will be generated.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 3.1f.
		/// </remarks>
		public float ErrorThreshold
		{
			set
			{
				if (value > 3.1f)
				{
					value = 3.1f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("errorThreshold", value);
			}
		}

		/// <summary>
		/// Duration of arms relax following firing weapon.  NB:This is clamped (0,5) in pointGun.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float FireWeaponRelaxTime
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("fireWeaponRelaxTime", value);
			}
		}

		/// <summary>
		/// Relax multiplier following firing weapon. Recovers over relaxTime.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.1f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FireWeaponRelaxAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("fireWeaponRelaxAmount", value);
			}
		}

		/// <summary>
		/// Range of motion for ik-based recoil.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 0.3f.
		/// </remarks>
		public float FireWeaponRelaxDistance
		{
			set
			{
				if (value > 0.3f)
				{
					value = 0.3f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("fireWeaponRelaxDistance", value);
			}
		}

		/// <summary>
		/// Use the incoming transforms to inform the pointGun of the primaryWeaponDistance, poleVector for the arm.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseIncomingTransforms
		{
			set
			{
				SetArgument("useIncomingTransforms", value);
			}
		}

		/// <summary>
		/// If useIncomingTransforms = true and measureParentOffset=true then measure the Pointing-from offset from parent effector, using itms - this should point the barrel of the gun to the target.  This is added to the rightHandParentOffset. NB NOT used if rightHandParentEffector LT 0.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool MeasureParentOffset
		{
			set
			{
				SetArgument("measureParentOffset", value);
			}
		}

		/// <summary>
		/// Pointing-from offset from parent effector, expressed in spine3's frame, x = back/forward, y = right/left, z = up/down.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 LeftHandParentOffset
		{
			set
			{
				SetArgument("leftHandParentOffset", value);
			}
		}

		/// <summary>
		/// 1 = Use leftShoulder. Effector from which the left hand pointing originates. Ie, point from this part to the target. -1 causes default offset for active weapon mode to be applied.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// Max value = 21.
		/// </remarks>
		public int LeftHandParentEffector
		{
			set
			{
				if (value > 21)
				{
					value = 21;
				}

				if (value < -1)
				{
					value = -1;
				}

				SetArgument("leftHandParentEffector", value);
			}
		}

		/// <summary>
		/// Pointing-from offset from parent effector, expressed in spine3's frame, x = back/forward, y = right/left, z = up/down. This is added to the measured one if useIncomingTransforms=true and measureParentOffset=true.  NB NOT used if rightHandParentEffector LT 0.  Pistol(0,0,0) Rifle(0.0032, 0.0, -0.0).
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 RightHandParentOffset
		{
			set
			{
				SetArgument("rightHandParentOffset", value);
			}
		}

		/// <summary>
		/// 1 = Use rightShoulder.. Effector from which the right hand pointing originates. Ie, point from this part to the target. -1 causes default offset for active weapon mode to be applied.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// Max value = 21.
		/// </remarks>
		public int RightHandParentEffector
		{
			set
			{
				if (value > 21)
				{
					value = 21;
				}

				if (value < -1)
				{
					value = -1;
				}

				SetArgument("rightHandParentEffector", value);
			}
		}

		/// <summary>
		/// Distance from the shoulder to hold the weapon. If -1 and useIncomingTransforms then weaponDistance is read from ITMs. WeaponDistance=primaryHandWeaponDistance clamped [0.2f:m_maxArmReach=0.65] if useIncomingTransforms = false. pistol 0.60383, rifle 0.336.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float PrimaryHandWeaponDistance
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("primaryHandWeaponDistance", value);
			}
		}

		/// <summary>
		/// Use hard constraint to keep rifle stock against shoulder?.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ConstrainRifle
		{
			set
			{
				SetArgument("constrainRifle", value);
			}
		}

		/// <summary>
		/// Rifle constraint distance. Deliberately kept large to create a flat constraint surface where rifle meets the shoulder.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// </remarks>
		public float RifleConstraintMinDistance
		{
			set
			{
				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rifleConstraintMinDistance", value);
			}
		}

		/// <summary>
		/// Disable collisions between right hand/forearm and the torso/legs.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool DisableArmCollisions
		{
			set
			{
				SetArgument("disableArmCollisions", value);
			}
		}

		/// <summary>
		/// Disable collisions between right hand/forearm and spine3/spine2 if in rifle mode.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool DisableRifleCollisions
		{
			set
			{
				SetArgument("disableRifleCollisions", value);
			}
		}
	}

	/// <summary>
	/// Seldom set parameters for pointGun - just to keep number of parameters in any message less than or equal to 64.
	/// </summary>
	public sealed class PointGunExtraHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the PointGunExtraHelper for sending a PointGunExtra <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the PointGunExtra <see cref="Message"/> to.</param>
		/// <remarks>
		/// Seldom set parameters for pointGun - just to keep number of parameters in any message less than or equal to 64.
		/// </remarks>
		public PointGunExtraHelper(Ped ped) : base(ped, "pointGunExtra")
		{
		}

		/// <summary>
		/// For supportConstraint = 2: force constraint strength of the supporting hands - it gets shaky at about 4.0.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float ConstraintStrength
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("constraintStrength", value);
			}
		}

		/// <summary>
		/// For supportConstraint = 2:  Like makeConstraintDistance. Force starts acting when the hands are  LT  3.0*thresh apart but is maximum strength  LT  thresh. For comparison: 0.1 is used for reachForWound in shot, 0.25 is used in grab.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ConstraintThresh
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("constraintThresh", value);
			}
		}

		/// <summary>
		/// Currently unused - no intoWorldTest. RAGE bit mask to exclude weapons from ray probe - currently defaults to MP3 weapon flag.
		/// </summary>
		/// <remarks>
		/// Default value = 1024.
		/// Min value = 0.
		/// </remarks>
		public int WeaponMask
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				SetArgument("weaponMask", value);
			}
		}

		/// <summary>
		/// Is timeWarpActive enabled?.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool TimeWarpActive
		{
			set
			{
				SetArgument("timeWarpActive", value);
			}
		}

		/// <summary>
		/// Scale for arm and helper strength when timewarp is enabled. 1 = normal compensation.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.1f.
		/// Max value = 2.0f.
		/// </remarks>
		public float TimeWarpStrengthScale
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("timeWarpStrengthScale", value);
			}
		}

		/// <summary>
		/// Hand stabilization controller stiffness.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float OriStiff
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("oriStiff", value);
			}
		}

		/// <summary>
		/// Hand stabilization controller damping.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float OriDamp
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("oriDamp", value);
			}
		}

		/// <summary>
		/// Hand stabilization controller stiffness.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float PosStiff
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("posStiff", value);
			}
		}

		/// <summary>
		/// Hand stabilization controller damping.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float PosDamp
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("posDamp", value);
			}
		}
	}

	public sealed class RollDownStairsHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the RollDownStairsHelper for sending a RollDownStairs <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the RollDownStairs <see cref="Message"/> to.</param>
		public RollDownStairsHelper(Ped ped) : base(ped, "rollDownStairs")
		{
		}

		/// <summary>
		/// Effector Stiffness. Value feeds through to rollUp directly.
		/// </summary>
		/// <remarks>
		/// Default value = 11.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float Stiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("stiffness", value);
			}
		}

		/// <summary>
		/// Effector  Damping.
		/// </summary>
		/// <remarks>
		/// Default value = 1.4f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float Damping
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("damping", value);
			}
		}

		/// <summary>
		/// Helper force strength.  Do not go above 1 for a rollDownStairs/roll along ground reaction.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float Forcemag
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("forcemag", value);
			}
		}

		/// <summary>
		/// The degree to which the character will try to stop a barrel roll with his arms.
		/// </summary>
		/// <remarks>
		/// Default value = -1.9f.
		/// Min value = -3.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float M_useArmToSlowDown
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < -3.0f)
				{
					value = -3.0f;
				}

				SetArgument("m_useArmToSlowDown", value);
			}
		}

		/// <summary>
		/// Blends between a zeroPose and the Rollup, Faster the character is rotating the less the zeroPose.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseZeroPose
		{
			set
			{
				SetArgument("useZeroPose", value);
			}
		}

		/// <summary>
		/// Applied cheat forces to spin the character when in the air, the forces are 40% of the forces applied when touching the ground.  Be careful little bunny rabbits, the character could spin unnaturally in the air.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SpinWhenInAir
		{
			set
			{
				SetArgument("spinWhenInAir", value);
			}
		}

		/// <summary>
		/// How much the character reaches with his arms to brace against the ground.
		/// </summary>
		/// <remarks>
		/// Default value = 1.4f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float M_armReachAmount
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("m_armReachAmount", value);
			}
		}

		/// <summary>
		/// Amount that the legs push outwards when tumbling.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float M_legPush
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("m_legPush", value);
			}
		}

		/// <summary>
		/// Blends between a zeroPose and the Rollup, Faster the character is rotating the less the zeroPose.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool TryToAvoidHeadButtingGround
		{
			set
			{
				SetArgument("tryToAvoidHeadButtingGround", value);
			}
		}

		/// <summary>
		/// The length that the arm reaches and so how much it straightens.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ArmReachLength
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armReachLength", value);
			}
		}

		/// <summary>
		/// Pass in a custom direction in to have the character try and roll in that direction.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 1.0f).
		/// Min value = 1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public Vector3 CustomRollDir
		{
			set
			{
				SetArgument("customRollDir", Vector3.Clamp(value, new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f)));
			}
		}

		/// <summary>
		/// Pass in true to use the customRollDir parameter.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseCustomRollDir
		{
			set
			{
				SetArgument("useCustomRollDir", value);
			}
		}

		/// <summary>
		/// The target linear velocity used to start the rolling.
		/// </summary>
		/// <remarks>
		/// Default value = 9.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float StiffnessDecayTarget
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stiffnessDecayTarget", value);
			}
		}

		/// <summary>
		/// Time, in seconds, to decay stiffness down to the stiffnessDecayTarget value (or -1 to disable).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float StiffnessDecayTime
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("stiffnessDecayTime", value);
			}
		}

		/// <summary>
		/// 0 is no leg asymmetry in 'foetal' position.  greater than 0 a asymmetricalLegs-rand(30%), added/minus each joint of the legs in radians.  Random number changes about once every roll.  0.4 gives a lot of asymmetry.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float AsymmetricalLegs
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("asymmetricalLegs", value);
			}
		}

		/// <summary>
		/// Tries to reduce the spin around the z axis. Scale 0 - 1.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ZAxisSpinReduction
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("zAxisSpinReduction", value);
			}
		}

		/// <summary>
		/// Time for the targetlinearVelocity to decay to zero.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float TargetLinearVelocityDecayTime
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("targetLinearVelocityDecayTime", value);
			}
		}

		/// <summary>
		/// Helper torques are applied to match the spin of the character to the max of targetLinearVelocity and COMVelMag.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float TargetLinearVelocity
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("targetLinearVelocity", value);
			}
		}

		/// <summary>
		/// Don't use rollup if true.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool OnlyApplyHelperForces
		{
			set
			{
				SetArgument("onlyApplyHelperForces", value);
			}
		}

		/// <summary>
		/// Scale applied cheat forces/torques to (zero) if object underneath character has velocity greater than 1.f.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseVelocityOfObjectBelow
		{
			set
			{
				SetArgument("useVelocityOfObjectBelow", value);
			}
		}

		/// <summary>
		/// UseVelocityOfObjectBelow uses a relative velocity of the character to the object underneath.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseRelativeVelocity
		{
			set
			{
				SetArgument("useRelativeVelocity", value);
			}
		}

		/// <summary>
		/// If true, use rollup for upper body and a kind of foetal behavior for legs.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ApplyFoetalToLegs
		{
			set
			{
				SetArgument("applyFoetalToLegs", value);
			}
		}

		/// <summary>
		/// Only used if applyFoetalToLegs = true : define the variation of angles for the joints of the legs.
		/// </summary>
		/// <remarks>
		/// Default value = 1.3f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float MovementLegsInFoetalPosition
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("movementLegsInFoetalPosition", value);
			}
		}

		/// <summary>
		/// Only used if applyNewRollingCheatingTorques or applyHelPerTorqueToAlign defined to true : maximal angular velocity around frontward axis of the pelvis to apply cheating torques.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = -1.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float MaxAngVelAroundFrontwardAxis
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("maxAngVelAroundFrontwardAxis", value);
			}
		}

		/// <summary>
		/// Only used if applyNewRollingCheatingTorques or applyHelPerTorqueToAlign defined to true : minimal angular velocity of the roll to apply cheating torques.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float MinAngVel
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("minAngVel", value);
			}
		}

		/// <summary>
		/// If true will use the new way to apply cheating torques (like in fallOverWall), otherwise will use the old way.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ApplyNewRollingCheatingTorques
		{
			set
			{
				SetArgument("applyNewRollingCheatingTorques", value);
			}
		}

		/// <summary>
		/// Only used if applyNewRollingCheatingTorques defined to true : maximal angular velocity of the roll to apply cheating torque.
		/// </summary>
		/// <remarks>
		/// Default value = 5.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float MaxAngVel
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxAngVel", value);
			}
		}

		/// <summary>
		/// Only used if applyNewRollingCheatingTorques defined to true : magnitude of the torque to roll down the stairs.
		/// </summary>
		/// <remarks>
		/// Default value = 50.0f.
		/// Min value = 0.0f.
		/// Max value = 500.0f.
		/// </remarks>
		public float MagOfTorqueToRoll
		{
			set
			{
				if (value > 500.0f)
				{
					value = 500.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("magOfTorqueToRoll", value);
			}
		}

		/// <summary>
		/// Apply torque to align the body orthogonally to the direction of the roll.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ApplyHelPerTorqueToAlign
		{
			set
			{
				SetArgument("applyHelPerTorqueToAlign", value);
			}
		}

		/// <summary>
		/// Only used if applyHelPerTorqueToAlign defined to true : delay to start to apply torques.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float DelayToAlignBody
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("delayToAlignBody", value);
			}
		}

		/// <summary>
		/// Only used if applyHelPerTorqueToAlign defined to true : magnitude of the torque to align orthogonally the body.
		/// </summary>
		/// <remarks>
		/// Default value = 50.0f.
		/// Min value = 0.0f.
		/// Max value = 500.0f.
		/// </remarks>
		public float MagOfTorqueToAlign
		{
			set
			{
				if (value > 500.0f)
				{
					value = 500.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("magOfTorqueToAlign", value);
			}
		}

		/// <summary>
		/// Ordinarily keep at 0.85.  Make this lower if you want spinning in the air.
		/// </summary>
		/// <remarks>
		/// Default value = 0.9f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float AirborneReduction
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("airborneReduction", value);
			}
		}

		/// <summary>
		/// Pass-through to Roll Up. Controls whether or not behavior enforces min/max friction.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ApplyMinMaxFriction
		{
			set
			{
				SetArgument("applyMinMaxFriction", value);
			}
		}

		/// <summary>
		/// Scale zAxisSpinReduction back when rotating end-over-end (somersault) to give the body a chance to align with the axis of rotation.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool LimitSpinReduction
		{
			set
			{
				SetArgument("limitSpinReduction", value);
			}
		}
	}

	public sealed class ShotHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ShotHelper for sending a Shot <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the Shot <see cref="Message"/> to.</param>
		public ShotHelper(Ped ped) : base(ped, "shot")
		{
		}

		/// <summary>
		/// Stiffness of body. Feeds through to roll_up.
		/// </summary>
		/// <remarks>
		/// Default value = 11.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float BodyStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("bodyStiffness", value);
			}
		}

		/// <summary>
		/// Stiffness of body. Feeds through to roll_up.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.1f.
		/// Max value = 2.0f.
		/// </remarks>
		public float SpineDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("spineDamping", value);
			}
		}

		/// <summary>
		/// Arm stiffness.
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ArmStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("armStiffness", value);
			}
		}

		/// <summary>
		/// Initial stiffness of neck after being shot.
		/// </summary>
		/// <remarks>
		/// Default value = 14.0f.
		/// Min value = 3.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float InitialNeckStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 3.0f)
				{
					value = 3.0f;
				}

				SetArgument("initialNeckStiffness", value);
			}
		}

		/// <summary>
		/// Intial damping of neck after being shot.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.1f.
		/// Max value = 10.0f.
		/// </remarks>
		public float InitialNeckDamping
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("initialNeckDamping", value);
			}
		}

		/// <summary>
		/// Stiffness of neck.
		/// </summary>
		/// <remarks>
		/// Default value = 14.0f.
		/// Min value = 3.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float NeckStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 3.0f)
				{
					value = 3.0f;
				}

				SetArgument("neckStiffness", value);
			}
		}

		/// <summary>
		/// Damping of neck.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.1f.
		/// Max value = 2.0f.
		/// </remarks>
		public float NeckDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("neckDamping", value);
			}
		}

		/// <summary>
		/// How much to add to upperbody stiffness dependent on looseness.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float KMultOnLoose
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("kMultOnLoose", value);
			}
		}

		/// <summary>
		/// How much to add to leg stiffnesses dependent on looseness.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float KMult4Legs
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("kMult4Legs", value);
			}
		}

		/// <summary>
		/// How loose the character is made by a newBullet. Between 0 and 1.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LoosenessAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("loosenessAmount", value);
			}
		}

		/// <summary>
		/// How loose the character is made by a newBullet if falling.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Looseness4Fall
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("looseness4Fall", value);
			}
		}

		/// <summary>
		/// How loose the upperBody of the character is made by a newBullet if staggerFall is running (and not falling).  Note atm the neck ramp values are ignored in staggerFall.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Looseness4Stagger
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("looseness4Stagger", value);
			}
		}

		/// <summary>
		/// Minimum looseness to apply to the arms.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float MinArmsLooseness
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("minArmsLooseness", value);
			}
		}

		/// <summary>
		/// Minimum looseness to apply to the Legs.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float MinLegsLooseness
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("minLegsLooseness", value);
			}
		}

		/// <summary>
		/// How long to hold for before returning to relaxed arm position.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float GrabHoldTime
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("grabHoldTime", value);
			}
		}

		/// <summary>
		/// True: spine is blended with zero pose, false: spine is blended with zero pose if not setting exag or cpain.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SpineBlendExagCPain
		{
			set
			{
				SetArgument("spineBlendExagCPain", value);
			}
		}

		/// <summary>
		/// Spine is always blended with zero pose this much and up to 1 as the character become stationary.  If negative no blend is ever applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = -0.1f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SpineBlendZero
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -0.1f)
				{
					value = -0.1f;
				}

				SetArgument("spineBlendZero", value);
			}
		}

		/// <summary>
		/// Looseness applied to spine is different if bulletProofVest is true.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool BulletProofVest
		{
			set
			{
				SetArgument("bulletProofVest", value);
			}
		}

		/// <summary>
		/// Looseness always reset on shotNewBullet even if previous looseness ramp still running.  Except for the neck which has it's own ramp.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool AlwaysResetLooseness
		{
			set
			{
				SetArgument("alwaysResetLooseness", value);
			}
		}

		/// <summary>
		/// Neck looseness always reset on shotNewBullet even if previous looseness ramp still running.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool AlwaysResetNeckLooseness
		{
			set
			{
				SetArgument("alwaysResetNeckLooseness", value);
			}
		}

		/// <summary>
		/// How much to scale the angular velocity coming in from animation of a part if it is in angVelScaleMask (otherwise scale by 1.0).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float AngVelScale
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("angVelScale", value);
			}
		}

		/// <summary>
		/// Parts to scale the initial angular velocity by angVelScale (otherwize scale by 1.0).
		/// </summary>
		/// <remarks>
		/// Default value = "fb".
		/// </remarks>
		public string AngVelScaleMask
		{
			set
			{
				SetArgument("angVelScaleMask", value);
			}
		}

		/// <summary>
		/// Width of the fling behavior.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FlingWidth
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("flingWidth", value);
			}
		}

		/// <summary>
		/// Duration of the fling behavior.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FlingTime
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("flingTime", value);
			}
		}

		/// <summary>
		/// Time, in seconds, before the character begins to grab for the wound on the first hit.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float TimeBeforeReachForWound
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("timeBeforeReachForWound", value);
			}
		}

		/// <summary>
		/// Exaggerate bullet duration (at exagMag/exagTwistMag).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ExagDuration
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("exagDuration", value);
			}
		}

		/// <summary>
		/// Exaggerate bullet spine Lean magnitude.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ExagMag
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("exagMag", value);
			}
		}

		/// <summary>
		/// Exaggerate bullet spine Twist magnitude.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ExagTwistMag
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("exagTwistMag", value);
			}
		}

		/// <summary>
		/// Exaggerate bullet duration ramping to zero after exagDuration.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ExagSmooth2Zero
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("exagSmooth2Zero", value);
			}
		}

		/// <summary>
		/// Exaggerate bullet time spent at 0 spine lean/twist after exagDuration + exagSmooth2Zero.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ExagZeroTime
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("exagZeroTime", value);
			}
		}

		/// <summary>
		/// Conscious pain duration ramping from zero to cpainMag/cpainTwistMag.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float CpainSmooth2Time
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("cpainSmooth2Time", value);
			}
		}

		/// <summary>
		/// Conscious pain duration at cpainMag/cpainTwistMag after cpainSmooth2Time.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float CpainDuration
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("cpainDuration", value);
			}
		}

		/// <summary>
		/// Conscious pain spine Lean(back/Forward) magnitude (Replaces spinePainMultiplier).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float CpainMag
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("cpainMag", value);
			}
		}

		/// <summary>
		/// Conscious pain spine Twist/Lean2Side magnitude Replaces spinePainTwistMultiplier).
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float CpainTwistMag
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("cpainTwistMag", value);
			}
		}

		/// <summary>
		/// Conscious pain ramping to zero after cpainSmooth2Time + cpainDuration (Replaces spinePainTime).
		/// </summary>
		/// <remarks>
		/// Default value = 1.5f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float CpainSmooth2Zero
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("cpainSmooth2Zero", value);
			}
		}

		/// <summary>
		/// Is the guy crouching or not.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Crouching
		{
			set
			{
				SetArgument("crouching", value);
			}
		}

		/// <summary>
		/// Type of reaction.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ChickenArms
		{
			set
			{
				SetArgument("chickenArms", value);
			}
		}

		/// <summary>
		/// Type of reaction.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ReachForWound
		{
			set
			{
				SetArgument("reachForWound", value);
			}
		}

		/// <summary>
		/// Type of reaction.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Fling
		{
			set
			{
				SetArgument("fling", value);
			}
		}

		/// <summary>
		/// Injured arm code runs if arm hit (turns and steps and bends injured arm).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AllowInjuredArm
		{
			set
			{
				SetArgument("allowInjuredArm", value);
			}
		}

		/// <summary>
		/// When false injured leg is not bent and character does not bend to reach it.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool AllowInjuredLeg
		{
			set
			{
				SetArgument("allowInjuredLeg", value);
			}
		}

		/// <summary>
		/// When false don't try to reach for injured Lower Legs (shins/feet).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AllowInjuredLowerLegReach
		{
			set
			{
				SetArgument("allowInjuredLowerLegReach", value);
			}
		}

		/// <summary>
		/// When false don't try to reach for injured Thighs.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool AllowInjuredThighReach
		{
			set
			{
				SetArgument("allowInjuredThighReach", value);
			}
		}

		/// <summary>
		/// Additional stability for hands and neck (less loose).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool StableHandsAndNeck
		{
			set
			{
				SetArgument("stableHandsAndNeck", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Melee
		{
			set
			{
				SetArgument("melee", value);
			}
		}

		/// <summary>
		/// 0=Rollup, 1=Catchfall, 2=rollDownStairs, 3=smartFall.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 3.
		/// </remarks>
		public int FallingReaction
		{
			set
			{
				if (value > 3)
				{
					value = 3;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("fallingReaction", value);
			}
		}

		/// <summary>
		/// Keep the character active instead of relaxing at the end of the catch fall.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseExtendedCatchFall
		{
			set
			{
				SetArgument("useExtendedCatchFall", value);
			}
		}

		/// <summary>
		/// Duration for which the character's upper body stays at minimum stiffness (not quite zero).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float InitialWeaknessZeroDuration
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("initialWeaknessZeroDuration", value);
			}
		}

		/// <summary>
		/// Duration of the ramp to bring the character's upper body stiffness back to normal levels.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float InitialWeaknessRampDuration
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("initialWeaknessRampDuration", value);
			}
		}

		/// <summary>
		/// Duration for which the neck stays at intial stiffness/damping.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float InitialNeckDuration
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("initialNeckDuration", value);
			}
		}

		/// <summary>
		/// Duration of the ramp to bring the neck stiffness/damping back to normal levels.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float InitialNeckRampDuration
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("initialNeckRampDuration", value);
			}
		}

		/// <summary>
		/// If enabled upper and lower body strength scales with character strength, using the range given by parameters below.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseCStrModulation
		{
			set
			{
				SetArgument("useCStrModulation", value);
			}
		}

		/// <summary>
		/// Proportions to what the strength would be normally.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.1f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CStrUpperMin
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("cStrUpperMin", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.1f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CStrUpperMax
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("cStrUpperMax", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.1f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CStrLowerMin
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("cStrLowerMin", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.1f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CStrLowerMax
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.1f)
				{
					value = 0.1f;
				}

				SetArgument("cStrLowerMax", value);
			}
		}

		/// <summary>
		/// Time to death (HACK for underwater). If -ve don't ever die.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float DeathTime
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("deathTime", value);
			}
		}
	}

	/// <summary>
	/// Send new wound information to the shot.  Can cause shot to restart it's performance in part or in whole.
	/// </summary>
	public sealed class ShotNewBulletHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ShotNewBulletHelper for sending a ShotNewBullet <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ShotNewBullet <see cref="Message"/> to.</param>
		/// <remarks>
		/// Send new wound information to the shot.  Can cause shot to restart it's performance in part or in whole.
		/// </remarks>
		public ShotNewBulletHelper(Ped ped) : base(ped, "shotNewBullet")
		{
		}

		/// <summary>
		/// Part ID on the body where the bullet hit.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 21.
		/// </remarks>
		public int BodyPart
		{
			set
			{
				if (value > 21)
				{
					value = 21;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("bodyPart", value);
			}
		}

		/// <summary>
		/// If true then normal and hitPoint should be supplied in local coordinates of bodyPart.  If false then normal and hitPoint should be supplied in World coordinates.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool LocalHitPointInfo
		{
			set
			{
				SetArgument("localHitPointInfo", value);
			}
		}

		/// <summary>
		/// Normal coming out of impact point on character.  Can be local or global depending on localHitPointInfo.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, -1.0f).
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public Vector3 Normal
		{
			set
			{
				SetArgument("normal", Vector3.Clamp(value, new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f)));
			}
		}

		/// <summary>
		/// Position of impact on character. Can be local or global depending on localHitPointInfo.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 HitPoint
		{
			set
			{
				SetArgument("hitPoint", value);
			}
		}

		/// <summary>
		/// Bullet velocity in world coordinates.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// Min value = -2000.0f.
		/// Max value = 2000.0f.
		/// </remarks>
		public Vector3 BulletVel
		{
			set
			{
				SetArgument("bulletVel",
					Vector3.Clamp(value, new Vector3(-2000.0f, -2000.0f, -2000.0f), new Vector3(2000.0f, 2000.0f, 2000.0f)));
			}
		}
	}

	public sealed class ShotSnapHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ShotSnapHelper for sending a ShotSnap <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ShotSnap <see cref="Message"/> to.</param>
		public ShotSnapHelper(Ped ped) : base(ped, "shotSnap")
		{
		}

		/// <summary>
		/// Add a Snap to shot.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Snap
		{
			set
			{
				SetArgument("snap", value);
			}
		}

		/// <summary>
		/// The magnitude of the reaction.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = -10.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SnapMag
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -10.0f)
				{
					value = -10.0f;
				}

				SetArgument("snapMag", value);
			}
		}

		/// <summary>
		/// MovingMult*snapMag = The magnitude of the reaction if moving(comVelMag) faster than movingThresh.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float SnapMovingMult
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("snapMovingMult", value);
			}
		}

		/// <summary>
		/// BalancingMult*snapMag = The magnitude of the reaction if balancing = (not lying on the floor/ not upper body not collided) and not airborne.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float SnapBalancingMult
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("snapBalancingMult", value);
			}
		}

		/// <summary>
		/// AirborneMult*snapMag = The magnitude of the reaction if airborne.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float SnapAirborneMult
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("snapAirborneMult", value);
			}
		}

		/// <summary>
		/// If moving(comVelMag) faster than movingThresh then mvingMult applied to stunMag.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float SnapMovingThresh
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("snapMovingThresh", value);
			}
		}

		/// <summary>
		/// The character snaps in a prescribed way (decided by bullet direction) - Higher the value the more random this direction is.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SnapDirectionRandomness
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("snapDirectionRandomness", value);
			}
		}

		/// <summary>
		/// Snap the leftArm.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SnapLeftArm
		{
			set
			{
				SetArgument("snapLeftArm", value);
			}
		}

		/// <summary>
		/// Snap the rightArm.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SnapRightArm
		{
			set
			{
				SetArgument("snapRightArm", value);
			}
		}

		/// <summary>
		/// Snap the leftLeg.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SnapLeftLeg
		{
			set
			{
				SetArgument("snapLeftLeg", value);
			}
		}

		/// <summary>
		/// Snap the rightLeg.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SnapRightLeg
		{
			set
			{
				SetArgument("snapRightLeg", value);
			}
		}

		/// <summary>
		/// Snap the spine.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool SnapSpine
		{
			set
			{
				SetArgument("snapSpine", value);
			}
		}

		/// <summary>
		/// Snap the neck.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool SnapNeck
		{
			set
			{
				SetArgument("snapNeck", value);
			}
		}

		/// <summary>
		/// Legs are either in phase with each other or not.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool SnapPhasedLegs
		{
			set
			{
				SetArgument("snapPhasedLegs", value);
			}
		}

		/// <summary>
		/// Type of hip reaction 0=none, 1=side2side 2=steplike.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int SnapHipType
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("snapHipType", value);
			}
		}

		/// <summary>
		/// Legs are either in phase with each other or not.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool SnapUseBulletDir
		{
			set
			{
				SetArgument("snapUseBulletDir", value);
			}
		}

		/// <summary>
		/// Snap only around the wounded part//mmmmtodo check whether bodyPart doesn't have to be remembered for unSnap.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool SnapHitPart
		{
			set
			{
				SetArgument("snapHitPart", value);
			}
		}

		/// <summary>
		/// Interval before applying reverse snap.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float UnSnapInterval
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("unSnapInterval", value);
			}
		}

		/// <summary>
		/// The magnitude of the reverse snap.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float UnSnapRatio
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("unSnapRatio", value);
			}
		}

		/// <summary>
		/// Use torques to make the snap otherwise use a change in the parts angular velocity.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool SnapUseTorques
		{
			set
			{
				SetArgument("snapUseTorques", value);
			}
		}
	}

	/// <summary>
	/// Configure the shockSpin effect in shot.  Spin/Lift the character using cheat torques/forces.
	/// </summary>
	public sealed class ShotShockSpinHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ShotShockSpinHelper for sending a ShotShockSpin <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ShotShockSpin <see cref="Message"/> to.</param>
		/// <remarks>
		/// Configure the shockSpin effect in shot.  Spin/Lift the character using cheat torques/forces.
		/// </remarks>
		public ShotShockSpinHelper(Ped ped) : base(ped, "shotShockSpin")
		{
		}

		/// <summary>
		/// If enabled, add a short 'shock' of torque to the character's spine to exaggerate bullet impact.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AddShockSpin
		{
			set
			{
				SetArgument("addShockSpin", value);
			}
		}

		/// <summary>
		/// For use with close-range shotgun blasts, or similar.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool RandomizeShockSpinDirection
		{
			set
			{
				SetArgument("randomizeShockSpinDirection", value);
			}
		}

		/// <summary>
		/// If true, apply the shock spin no matter which body component was hit. Otherwise only apply if the spine or clavicles get hit.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AlwaysAddShockSpin
		{
			set
			{
				SetArgument("alwaysAddShockSpin", value);
			}
		}

		/// <summary>
		/// Minimum amount of torque to add if using shock-spin feature.
		/// </summary>
		/// <remarks>
		/// Default value = 50.0f.
		/// Min value = 0.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float ShockSpinMin
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("shockSpinMin", value);
			}
		}

		/// <summary>
		/// Maximum amount of torque to add if using shock-spin feature.
		/// </summary>
		/// <remarks>
		/// Default value = 90.0f.
		/// Min value = 0.0f.
		/// Max value = 1000.0f.
		/// </remarks>
		public float ShockSpinMax
		{
			set
			{
				if (value > 1000.0f)
				{
					value = 1000.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("shockSpinMax", value);
			}
		}

		/// <summary>
		/// If greater than 0, apply a force to lift the character up while the torque is applied, trying to produce a dramatic spun/twist shotgun-to-the-chest effect. This is a scale of the torque applied, so 8.0 or so would give a reasonable amount of lift.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ShockSpinLiftForceMult
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("shockSpinLiftForceMult", value);
			}
		}

		/// <summary>
		/// Multiplier used when decaying torque spin over time.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ShockSpinDecayMult
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("shockSpinDecayMult", value);
			}
		}

		/// <summary>
		/// Torque applied is scaled by this amount across the spine components - spine2 recieving the full amount, then 3 and 1 and finally 0. Each time, this value is used to scale it down. 0.5 means half the torque each time.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ShockSpinScalePerComponent
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("shockSpinScalePerComponent", value);
			}
		}

		/// <summary>
		/// Shock spin ends when twist velocity is greater than this value (try 6.0).  If set to -1 does not stop.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 200.0f.
		/// </remarks>
		public float ShockSpinMaxTwistVel
		{
			set
			{
				if (value > 200.0f)
				{
					value = 200.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("shockSpinMaxTwistVel", value);
			}
		}

		/// <summary>
		/// Shock spin scales by lever arm of bullet i.e. bullet impact point to center line.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ShockSpinScaleByLeverArm
		{
			set
			{
				SetArgument("shockSpinScaleByLeverArm", value);
			}
		}

		/// <summary>
		/// ShockSpin's torque is multipied by this value when both the character's feet are not in contact.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ShockSpinAirMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("shockSpinAirMult", value);
			}
		}

		/// <summary>
		/// ShockSpin's torque is multipied by this value when the one of the character's feet are not in contact.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ShockSpin1FootMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("shockSpin1FootMult", value);
			}
		}

		/// <summary>
		/// ShockSpin scales the torques applied to the feet by footSlipCompensation.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float ShockSpinFootGripMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("shockSpinFootGripMult", value);
			}
		}

		/// <summary>
		/// If shot on a side with a forward foot and both feet are on the ground and balanced, increase the shockspin to compensate for the balancer naturally resisting spin to that side.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 1.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float BracedSideSpinMult
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 1.0f)
				{
					value = 1.0f;
				}

				SetArgument("bracedSideSpinMult", value);
			}
		}
	}

	/// <summary>
	/// Configure the fall to knees shot.
	/// </summary>
	public sealed class ShotFallToKneesHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ShotFallToKneesHelper for sending a ShotFallToKnees <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ShotFallToKnees <see cref="Message"/> to.</param>
		/// <remarks>
		/// Configure the fall to knees shot.
		/// </remarks>
		public ShotFallToKneesHelper(Ped ped) : base(ped, "shotFallToKnees")
		{
		}

		/// <summary>
		/// Type of reaction.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FallToKnees
		{
			set
			{
				SetArgument("fallToKnees", value);
			}
		}

		/// <summary>
		/// Always change fall behavior.  If false only change when falling forward.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FtkAlwaysChangeFall
		{
			set
			{
				SetArgument("ftkAlwaysChangeFall", value);
			}
		}

		/// <summary>
		/// How long the balancer runs for before fallToKnees starts.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float FtkBalanceTime
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("ftkBalanceTime", value);
			}
		}

		/// <summary>
		/// Hip helper force magnitude - to help character lean over balance point of line between toes.
		/// </summary>
		/// <remarks>
		/// Default value = 200.0f.
		/// Min value = 0.0f.
		/// Max value = 2000.0f.
		/// </remarks>
		public float FtkHelperForce
		{
			set
			{
				if (value > 2000.0f)
				{
					value = 2000.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("ftkHelperForce", value);
			}
		}

		/// <summary>
		/// Helper force applied to spine3 as well.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool FtkHelperForceOnSpine
		{
			set
			{
				SetArgument("ftkHelperForceOnSpine", value);
			}
		}

		/// <summary>
		/// Help balancer lean amount - to help character lean over balance point of line between toes.
		/// Half of this is also applied as hipLean.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 0.3f.
		/// </remarks>
		public float FtkLeanHelp
		{
			set
			{
				if (value > 0.3f)
				{
					value = 0.3f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("ftkLeanHelp", value);
			}
		}

		/// <summary>
		/// Bend applied to spine when falling from knees. (+ve forward - try -0.1) (only if rds called).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -0.2f.
		/// Max value = 0.3f.
		/// </remarks>
		public float FtkSpineBend
		{
			set
			{
				if (value > 0.3f)
				{
					value = 0.3f;
				}

				if (value < -0.2f)
				{
					value = -0.2f;
				}

				SetArgument("ftkSpineBend", value);
			}
		}

		/// <summary>
		/// Stiffen spine when falling from knees (only if rds called).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FtkStiffSpine
		{
			set
			{
				SetArgument("ftkStiffSpine", value);
			}
		}

		/// <summary>
		/// Looseness (muscleStiffness = 1.01f - m_parameters.ftkImpactLooseness) applied to upperBody on knee impacts.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FtkImpactLooseness
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("ftkImpactLooseness", value);
			}
		}

		/// <summary>
		/// Time that looseness is applied after knee impacts.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -0.1f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FtkImpactLoosenessTime
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -0.1f)
				{
					value = -0.1f;
				}

				SetArgument("ftkImpactLoosenessTime", value);
			}
		}

		/// <summary>
		/// Rate at which the legs are bent to go from standing to on knees.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float FtkBendRate
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("ftkBendRate", value);
			}
		}

		/// <summary>
		/// Blend from current hip to balancing on knees hip angle.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FtkHipBlend
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("ftkHipBlend", value);
			}
		}

		/// <summary>
		/// Probability that a lunge reaction will be allowed.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FtkLungeProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("ftkLungeProb", value);
			}
		}

		/// <summary>
		/// When on knees allow some spinning of the character.
		/// If false then the balancers' footSlipCompensation remains on and tends to keep the character facing the same way as when it was balancing.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FtkKneeSpin
		{
			set
			{
				SetArgument("ftkKneeSpin", value);
			}
		}

		/// <summary>
		/// Multiplier on the reduction of friction for the feet based on angle away from horizontal - helps the character fall to knees quicker.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float FtkFricMult
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("ftkFricMult", value);
			}
		}

		/// <summary>
		/// Apply this hip angle when the character starts to fall backwards when on knees.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FtkHipAngleFall
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("ftkHipAngleFall", value);
			}
		}

		/// <summary>
		/// Hip pitch applied (+ve forward, -ve backwards) if character is falling forwards on way down to it's knees.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = -0.5f.
		/// Max value = 0.5f.
		/// </remarks>
		public float FtkPitchForwards
		{
			set
			{
				if (value > 0.5f)
				{
					value = 0.5f;
				}

				if (value < -0.5f)
				{
					value = -0.5f;
				}

				SetArgument("ftkPitchForwards", value);
			}
		}

		/// <summary>
		/// Hip pitch applied (+ve forward, -ve backwards) if character is falling backwards on way down to it's knees.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = -0.5f.
		/// Max value = 0.5f.
		/// </remarks>
		public float FtkPitchBackwards
		{
			set
			{
				if (value > 0.5f)
				{
					value = 0.5f;
				}

				if (value < -0.5f)
				{
					value = -0.5f;
				}

				SetArgument("ftkPitchBackwards", value);
			}
		}

		/// <summary>
		/// Balancer instability below which the character starts to bend legs even if it isn't going to fall on to it's knees (i.e. if going backwards).
		/// 0.3 almost ensures a fall to knees but means the character will keep stepping backward until it slows down enough.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 15.0f.
		/// </remarks>
		public float FtkFallBelowStab
		{
			set
			{
				if (value > 15.0f)
				{
					value = 15.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("ftkFallBelowStab", value);
			}
		}

		/// <summary>
		/// When the character gives up and goes into a fall.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float FtkBalanceAbortThreshold
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("ftkBalanceAbortThreshold", value);
			}
		}

		/// <summary>
		/// Type of arm response when on knees falling forward 0=useFallArms (from RollDownstairs or catchFall), 1= armsIn, 2=armsOut.
		/// </summary>
		/// <remarks>
		/// Default value = 2.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int FtkOnKneesArmType
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("ftkOnKneesArmType", value);
			}
		}

		/// <summary>
		/// Release the reachForWound this amount of time after the knees have hit.
		/// If LT 0.0 then keep reaching for wound regardless of fall/onground state.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float FtkReleaseReachForWound
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("ftkReleaseReachForWound", value);
			}
		}

		/// <summary>
		/// True = Keep reaching for wound regardless of fall/onground state.
		/// false = respect the shotConfigureArms params: reachFalling, reachFallingWithOneHand, reachOnFloor.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool FtkReachForWound
		{
			set
			{
				SetArgument("ftkReachForWound", value);
			}
		}

		/// <summary>
		/// Override the pointGun when knees hit.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FtkReleasePointGun
		{
			set
			{
				SetArgument("ftkReleasePointGun", value);
			}
		}

		/// <summary>
		/// The upper body of the character must be colliding and other failure conditions met to fail.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool FtkFailMustCollide
		{
			set
			{
				SetArgument("ftkFailMustCollide", value);
			}
		}
	}

	/// <summary>
	/// Configure the shot from behind reaction.
	/// </summary>
	public sealed class ShotFromBehindHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ShotFromBehindHelper for sending a ShotFromBehind <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ShotFromBehind <see cref="Message"/> to.</param>
		/// <remarks>
		/// Configure the shot from behind reaction.
		/// </remarks>
		public ShotFromBehindHelper(Ped ped) : base(ped, "shotFromBehind")
		{
		}

		/// <summary>
		/// Type of reaction.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ShotFromBehind
		{
			set
			{
				SetArgument("shotFromBehind", value);
			}
		}

		/// <summary>
		/// SpineBend.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SfbSpineAmount
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sfbSpineAmount", value);
			}
		}

		/// <summary>
		/// Neck Bend.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SfbNeckAmount
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sfbNeckAmount", value);
			}
		}

		/// <summary>
		/// Hip Pitch.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SfbHipAmount
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sfbHipAmount", value);
			}
		}

		/// <summary>
		/// Knee bend.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SfbKneeAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sfbKneeAmount", value);
			}
		}

		/// <summary>
		/// ShotFromBehind reaction period after being shot.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SfbPeriod
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sfbPeriod", value);
			}
		}

		/// <summary>
		/// Amount of time not taking a step.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SfbForceBalancePeriod
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sfbForceBalancePeriod", value);
			}
		}

		/// <summary>
		/// Amount of time before applying spread out arms pose.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SfbArmsOnset
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sfbArmsOnset", value);
			}
		}

		/// <summary>
		/// Amount of time before bending knees a bit.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SfbKneesOnset
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sfbKneesOnset", value);
			}
		}

		/// <summary>
		/// Controls additional independent randomized bending of left/right elbows.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float SfbNoiseGain
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sfbNoiseGain", value);
			}
		}

		/// <summary>
		/// 0 = balancer fails as normal,
		/// 1 = ignore backArchedBack and leanedTooFarBack balancer failures,
		/// 2 = ignore backArchedBack balancer failure only,
		/// 3 = ignore leanedTooFarBack balancer failure only.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 3.
		/// </remarks>
		public int SfbIgnoreFail
		{
			set
			{
				if (value > 3)
				{
					value = 3;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("sfbIgnoreFail", value);
			}
		}
	}

	/// <summary>
	/// Configure the shot in guts reaction.
	/// </summary>
	public sealed class ShotInGutsHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ShotInGutsHelper for sending a ShotInGuts <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ShotInGuts <see cref="Message"/> to.</param>
		/// <remarks>
		/// Configure the shot in guts reaction.
		/// </remarks>
		public ShotInGutsHelper(Ped ped) : base(ped, "shotInGuts")
		{
		}

		/// <summary>
		/// Type of reaction.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ShotInGuts
		{
			set
			{
				SetArgument("shotInGuts", value);
			}
		}

		/// <summary>
		/// SpineBend.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SigSpineAmount
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sigSpineAmount", value);
			}
		}

		/// <summary>
		/// Neck Bend.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SigNeckAmount
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sigNeckAmount", value);
			}
		}

		/// <summary>
		/// Hip Pitch.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SigHipAmount
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sigHipAmount", value);
			}
		}

		/// <summary>
		/// Knee bend.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SigKneeAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sigKneeAmount", value);
			}
		}

		/// <summary>
		/// Active time after being shot.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SigPeriod
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sigPeriod", value);
			}
		}

		/// <summary>
		/// Amount of time not taking a step.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SigForceBalancePeriod
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sigForceBalancePeriod", value);
			}
		}

		/// <summary>
		/// Amount of time not taking a step.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SigKneesOnset
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("sigKneesOnset", value);
			}
		}
	}

	public sealed class ShotHeadLookHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ShotHeadLookHelper for sending a ShotHeadLook <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ShotHeadLook <see cref="Message"/> to.</param>
		public ShotHeadLookHelper(Ped ped) : base(ped, "shotHeadLook")
		{
		}

		/// <summary>
		/// Use head look.
		/// Default: looks at provided target or if this is zero - looks forward or in velocity direction.
		/// If reachForWound is enabled, switches between looking at the wound and at the default target.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseHeadLook
		{
			set
			{
				SetArgument("useHeadLook", value);
			}
		}

		/// <summary>
		/// Position to look at with headlook flag.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 HeadLook
		{
			set
			{
				SetArgument("headLook", value);
			}
		}

		/// <summary>
		/// Min time to look at wound.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float HeadLookAtWoundMinTimer
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("headLookAtWoundMinTimer", value);
			}
		}

		/// <summary>
		/// Max time to look at wound.
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float HeadLookAtWoundMaxTimer
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("headLookAtWoundMaxTimer", value);
			}
		}

		/// <summary>
		/// Min time to look headLook or if zero - forward or in velocity direction.
		/// </summary>
		/// <remarks>
		/// Default value = 1.7f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float HeadLookAtHeadPosMaxTimer
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("headLookAtHeadPosMaxTimer", value);
			}
		}

		/// <summary>
		/// Max time to look headLook or if zero - forward or in velocity direction.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float HeadLookAtHeadPosMinTimer
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("headLookAtHeadPosMinTimer", value);
			}
		}
	}

	/// <summary>
	/// Configure the arm reactions in shot.
	/// </summary>
	public sealed class ShotConfigureArmsHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the ShotConfigureArmsHelper for sending a ShotConfigureArms <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the ShotConfigureArms <see cref="Message"/> to.</param>
		/// <remarks>
		/// Configure the arm reactions in shot.
		/// </remarks>
		public ShotConfigureArmsHelper(Ped ped) : base(ped, "shotConfigureArms")
		{
		}

		/// <summary>
		/// Blind brace with arms if appropriate.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool Brace
		{
			set
			{
				SetArgument("brace", value);
			}
		}

		/// <summary>
		/// Point gun if appropriate.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool PointGun
		{
			set
			{
				SetArgument("pointGun", value);
			}
		}

		/// <summary>
		/// ArmsWindmill if going backwards fast enough.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseArmsWindmill
		{
			set
			{
				SetArgument("useArmsWindmill", value);
			}
		}

		/// <summary>
		/// Release wound if going sideways/forward fast enough.
		/// 0 = don't.
		/// 1 = only if bracing.
		/// 2 = any default arm reaction.
		/// </summary>
		/// <remarks>
		/// Default value = 1.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int ReleaseWound
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("releaseWound", value);
			}
		}

		/// <summary>
		/// Reach for wound when falling.
		/// 0 = false,
		/// 1 = true,
		/// 2 = once per shot performance.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int ReachFalling
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("reachFalling", value);
			}
		}

		/// <summary>
		/// Force character to reach for wound with only one hand when falling or fallen.
		/// 0 = allow two-handed reach,
		/// 1 = left only if two-handed possible,
		/// 2 = right only if two-handed possible,
		/// 3 = one handed but automatic (allows switching of hands).
		/// </summary>
		/// <remarks>
		/// Default value = 3.
		/// Min value = 0.
		/// Max value = 3.
		/// </remarks>
		public int ReachFallingWithOneHand
		{
			set
			{
				if (value > 3)
				{
					value = 3;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("reachFallingWithOneHand", value);
			}
		}

		/// <summary>
		/// ReachForWound when on floor - 0 = false, 1 = true, 2 = once per shot performance.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int ReachOnFloor
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("reachOnFloor", value);
			}
		}

		/// <summary>
		/// Inhibit arms brace for this amount of time after reachForWound has begun.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float AlwaysReachTime
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("alwaysReachTime", value);
			}
		}

		/// <summary>
		/// For armsWindmill, multiplier on character speed - increase of speed of circling is proportional to character speed (max speed of circliing increase = 1.5). Eg. lowering the value increases the range of velocity that the 0-1.5 is applied over.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float AWSpeedMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("AWSpeedMult", value);
			}
		}

		/// <summary>
		/// For armsWindmill, multiplier on character speed - increase of radii is proportional to character speed (max radius increase = 0.45).
		/// E.g. lowering the value increases the range of velocity that the 0-0.45 is applied over.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float AWRadiusMult
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("AWRadiusMult", value);
			}
		}

		/// <summary>
		/// For armsWindmill, added arm stiffness ranges from 0 to AWStiffnessAdd.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float AWStiffnessAdd
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("AWStiffnessAdd", value);
			}
		}

		/// <summary>
		/// Force character to reach for wound with only one hand.
		/// 0 = allow two-handed reach,
		/// 1 = left only if two-handed possible,
		/// 2 = right only if two-handed possible.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 2.
		/// </remarks>
		public int ReachWithOneHand
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("reachWithOneHand", value);
			}
		}

		/// <summary>
		/// Allow character to reach for wound with left hand if holding a pistol.
		/// It never will for a rifle.
		/// If pointGun is running this will only happen if the hand cannot point and pointGun:poseUnusedGunArm = false.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool AllowLeftPistolRFW
		{
			set
			{
				SetArgument("allowLeftPistolRFW", value);
			}
		}

		/// <summary>
		/// Allow character to reach for wound with right hand if holding a pistol.
		/// It never will for a rifle.
		/// If pointGun is running this will only happen if the hand cannot point and pointGun:poseUnusedGunArm = false.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AllowRightPistolRFW
		{
			set
			{
				SetArgument("allowRightPistolRFW", value);
			}
		}

		/// <summary>
		/// Override pointGun and reachForWound if desired if holding a pistol.
		/// It never will for a rifle.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool RfwWithPistol
		{
			set
			{
				SetArgument("rfwWithPistol", value);
			}
		}

		/// <summary>
		/// Type of reaction.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Fling2
		{
			set
			{
				SetArgument("fling2", value);
			}
		}

		/// <summary>
		/// Fling the left arm.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool Fling2Left
		{
			set
			{
				SetArgument("fling2Left", value);
			}
		}

		/// <summary>
		/// Fling the right arm.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool Fling2Right
		{
			set
			{
				SetArgument("fling2Right", value);
			}
		}

		/// <summary>
		/// Override stagger arms even if staggerFall:m_upperBodyReaction = true.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Fling2OverrideStagger
		{
			set
			{
				SetArgument("fling2OverrideStagger", value);
			}
		}

		/// <summary>
		/// Time after hit that the fling will start (allows for a bit of loose arm movement from bullet impact.snap etc).
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Fling2TimeBefore
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("fling2TimeBefore", value);
			}
		}

		/// <summary>
		/// Duration of the fling behavior.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Fling2Time
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("fling2Time", value);
			}
		}

		/// <summary>
		/// Muscle stiffness of the left arm.
		/// If negative then uses the shots underlying muscle stiffness from controlStiffness (i.e. respects looseness).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = -1.0f.
		/// Max value = 1.5f.
		/// </remarks>
		public float Fling2MStiffL
		{
			set
			{
				if (value > 1.5f)
				{
					value = 1.5f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("fling2MStiffL", value);
			}
		}

		/// <summary>
		/// Muscle stiffness of the right arm.
		/// If negative then uses the shots underlying muscle stiffness from controlStiffness (i.e. respects looseness).
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 1.5f.
		/// </remarks>
		public float Fling2MStiffR
		{
			set
			{
				if (value > 1.5f)
				{
					value = 1.5f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("fling2MStiffR", value);
			}
		}

		/// <summary>
		/// Maximum time before the left arm relaxes in the fling.
		/// It will relax automatically when the arm has completed it's bent arm fling.
		/// This is what causes the arm to straighten.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Fling2RelaxTimeL
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("fling2RelaxTimeL", value);
			}
		}

		/// <summary>
		/// Maximum time before the right arm relaxes in the fling.
		/// It will relax automatically when the arm has completed it's bent arm fling.
		/// This is what causes the arm to straighten.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Fling2RelaxTimeR
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("fling2RelaxTimeR", value);
			}
		}

		/// <summary>
		/// Minimum fling angle for left arm.
		/// Fling angle is random in the range fling2AngleMin:fling2AngleMax.
		/// Angle of fling in radians measured from the body horizontal sideways from shoulder.
		/// Positive is up, 0 shoulder level, negative down.
		/// </summary>
		/// <remarks>
		/// Default value = -1.5f.
		/// Min value = -1.5f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Fling2AngleMinL
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.5f)
				{
					value = -1.5f;
				}

				SetArgument("fling2AngleMinL", value);
			}
		}

		/// <summary>
		/// Maximum fling angle for left arm.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = -1.5f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Fling2AngleMaxL
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.5f)
				{
					value = -1.5f;
				}

				SetArgument("fling2AngleMaxL", value);
			}
		}

		/// <summary>
		/// Minimum fling angle for right arm.
		/// </summary>
		/// <remarks>
		/// Default value = -1.5f.
		/// Min value = -1.5f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Fling2AngleMinR
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.5f)
				{
					value = -1.5f;
				}

				SetArgument("fling2AngleMinR", value);
			}
		}

		/// <summary>
		/// Maximum fling angle for right arm.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = -1.5f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Fling2AngleMaxR
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.5f)
				{
					value = -1.5f;
				}

				SetArgument("fling2AngleMaxR", value);
			}
		}

		/// <summary>
		/// Minimum left arm length.
		/// Arm length is random in the range fling2LengthMin:fling2LengthMax.
		/// Arm length maps one to one with elbow angle.
		/// These values are scaled internally for the female character.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.3f.
		/// Max value = 0.6f.
		/// </remarks>
		public float Fling2LengthMinL
		{
			set
			{
				if (value > 0.6f)
				{
					value = 0.6f;
				}

				if (value < 0.3f)
				{
					value = 0.3f;
				}

				SetArgument("fling2LengthMinL", value);
			}
		}

		/// <summary>
		/// Maximum left arm length.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.3f.
		/// Max value = 0.6f.
		/// </remarks>
		public float Fling2LengthMaxL
		{
			set
			{
				if (value > 0.6f)
				{
					value = 0.6f;
				}

				if (value < 0.3f)
				{
					value = 0.3f;
				}

				SetArgument("fling2LengthMaxL", value);
			}
		}

		/// <summary>
		/// Min right arm length.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.3f.
		/// Max value = 0.6f.
		/// </remarks>
		public float Fling2LengthMinR
		{
			set
			{
				if (value > 0.6f)
				{
					value = 0.6f;
				}

				if (value < 0.3f)
				{
					value = 0.3f;
				}

				SetArgument("fling2LengthMinR", value);
			}
		}

		/// <summary>
		/// Max right arm length.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.3f.
		/// Max value = 0.6f.
		/// </remarks>
		public float Fling2LengthMaxR
		{
			set
			{
				if (value > 0.6f)
				{
					value = 0.6f;
				}

				if (value < 0.3f)
				{
					value = 0.3f;
				}

				SetArgument("fling2LengthMaxR", value);
			}
		}

		/// <summary>
		/// Has the character got a bust.
		/// If so then cupBust (move bust reach targets below bust) or bustElbowLift and cupSize (stop upperArm penetrating bust and move bust targets to surface of bust) are implemented.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Bust
		{
			set
			{
				SetArgument("bust", value);
			}
		}

		/// <summary>
		/// Lift the elbows up this much extra to avoid upper arm penetrating the bust (when target hits spine2 or spine3).
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float BustElbowLift
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("bustElbowLift", value);
			}
		}

		/// <summary>
		/// Amount reach target to bust (spine2) will be offset forward by.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CupSize
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("cupSize", value);
			}
		}

		/// <summary>
		/// All reach targets above or on the bust will cause a reach below the bust.
		/// (specifically moves spine3 and spine2 targets to spine1).
		/// BustElbowLift and cupSize are ignored.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool CupBust
		{
			set
			{
				SetArgument("cupBust", value);
			}
		}
	}

	/// <summary>
	/// Clone of High Fall with a wider range of operating conditions.
	/// </summary>
	public sealed class SmartFallHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the SmartFallHelper for sending a SmartFall <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the SmartFall <see cref="Message"/> to.</param>
		/// <remarks>
		/// Clone of High Fall with a wider range of operating conditions.
		/// </remarks>
		public SmartFallHelper(Ped ped) : base(ped, "smartFall")
		{
		}

		/// <summary>
		/// Stiffness of body.
		/// Value feeds through to bodyBalance (synced with defaults), to armsWindmill (14 for this value at default ), legs pedal, head look and roll down stairs directly.
		/// </summary>
		/// <remarks>
		/// Default value = 11.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float BodyStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("bodyStiffness", value);
			}
		}

		/// <summary>
		/// The damping of the joints.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 3.0f.
		/// </remarks>
		public float Bodydamping
		{
			set
			{
				if (value > 3.0f)
				{
					value = 3.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("bodydamping", value);
			}
		}

		/// <summary>
		/// The length of time before the impact that the character transitions to the landing.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Catchfalltime
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("catchfalltime", value);
			}
		}

		/// <summary>
		/// 0.52angle is 0.868 dot//A threshold for deciding how far away from upright the character needs to be before bailing out (going into a foetal) instead of trying to land (keeping stretched out).
		/// NB: never does bailout if ignorWorldCollisions true.
		/// </summary>
		/// <remarks>
		/// Default value = 0.9f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CrashOrLandCutOff
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("crashOrLandCutOff", value);
			}
		}

		/// <summary>
		/// Strength of the controller to keep the character at angle aimAngleBase from vertical.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float PdStrength
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("pdStrength", value);
			}
		}

		/// <summary>
		/// Damping multiplier of the controller to keep the character at angle aimAngleBase from vertical.
		/// The actual damping is pdDamping*pdStrength*constant*angVel.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float PdDamping
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("pdDamping", value);
			}
		}

		/// <summary>
		/// Arm circling speed in armWindMillAdaptive.
		/// </summary>
		/// <remarks>
		/// Default value = 7.9f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float ArmAngSpeed
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armAngSpeed", value);
			}
		}

		/// <summary>
		/// In armWindMillAdaptive.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float ArmAmplitude
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armAmplitude", value);
			}
		}

		/// <summary>
		/// In armWindMillAdaptive 3.1 opposite for stuntman.
		/// 1.0 old default. 0.0 in phase.
		/// </summary>
		/// <remarks>
		/// Default value = 3.1f.
		/// Min value = 0.0f.
		/// Max value = 6.3f.
		/// </remarks>
		public float ArmPhase
		{
			set
			{
				if (value > 6.3f)
				{
					value = 6.3f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armPhase", value);
			}
		}

		/// <summary>
		/// In armWindMillAdaptive bend the elbows as a function of armAngle.
		/// For stunt man true otherwise false.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ArmBendElbows
		{
			set
			{
				SetArgument("armBendElbows", value);
			}
		}

		/// <summary>
		/// Radius of legs on pedal.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 0.5f.
		/// </remarks>
		public float LegRadius
		{
			set
			{
				if (value > 0.5f)
				{
					value = 0.5f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legRadius", value);
			}
		}

		/// <summary>
		/// In pedal.
		/// </summary>
		/// <remarks>
		/// Default value = 7.9f.
		/// Min value = 0.0f.
		/// Max value = 15.0f.
		/// </remarks>
		public float LegAngSpeed
		{
			set
			{
				if (value > 15.0f)
				{
					value = 15.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legAngSpeed", value);
			}
		}

		/// <summary>
		/// 0.0 for stunt man.
		/// Random offset applied per leg to the angular speed to desynchronize the pedaling - set to 0 to disable, otherwise should be set to less than the angularSpeed value.
		/// </summary>
		/// <remarks>
		/// Default value = 4.0f.
		/// Min value = -10.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float LegAsymmetry
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -10.0f)
				{
					value = -10.0f;
				}

				SetArgument("legAsymmetry", value);
			}
		}

		/// <summary>
		/// Phase angle between the arms and legs circling angle.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 6.5f.
		/// </remarks>
		public float Arms2LegsPhase
		{
			set
			{
				if (value > 6.5f)
				{
					value = 6.5f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("arms2LegsPhase", value);
			}
		}

		/// <summary>
		/// Syncs the arms angle to what the leg angle is.
		/// </summary>
		/// <remarks>
		/// Default value = <see cref="Synchroisation.AlwaysSynced"/>.
		/// All speed/direction parameters of armswindmill are overwritten if = <see cref="Synchroisation.AlwaysSynced"/>.
		/// If <see cref="Synchroisation.SyncedAtStart"/> and you want synced arms/legs then armAngSpeed=legAngSpeed, legAsymmetry = 0.0 (to stop randomizations of the leg cicle speed).
		/// </remarks>
		public Synchroisation Arms2LegsSync
		{
			set
			{
				SetArgument("arms2LegsSync", (int)value);
			}
		}

		/// <summary>
		/// Where to put the arms when preparing to land.
		/// Approx 1 = above head, 0 = head height, -1 = down.
		/// LT -2.0 use catchFall arms, LT -3.0 use prepare for landing pose if Agent is due to land vertically, feet first.
		/// </summary>
		/// <remarks>
		/// Default value = -3.1f.
		/// Min value = -4.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmsUp
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < -4.0f)
				{
					value = -4.0f;
				}

				SetArgument("armsUp", value);
			}
		}

		/// <summary>
		/// Toggle to orientate to fall direction.  i.e. orientate so that the character faces the horizontal velocity direction.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool OrientateBodyToFallDirection
		{
			set
			{
				SetArgument("orientateBodyToFallDirection", value);
			}
		}

		/// <summary>
		/// If false don't worry about the twist angle of the character when orientating the character.
		/// If false this allows the twist axis of the character to be free (You can get a nice twisting highFall like the one in dieHard 4 when the car goes into the helicopter).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool OrientateTwist
		{
			set
			{
				SetArgument("orientateTwist", value);
			}
		}

		/// <summary>
		/// DEVEL parameter - suggest you don't edit it.  Maximum torque the orientation controller can apply.
		/// If 0 then no helper torques will be used.
		/// 300 will orientate the character softly for all but extreme angles away from aimAngleBase.
		/// If abs (current -aimAngleBase) is getting near 3.0 then this can be reduced to give a softer feel.
		/// </summary>
		/// <remarks>
		/// Default value = 300.0f.
		/// Min value = 0.0f.
		/// Max value = 2000.0f.
		/// </remarks>
		public float OrientateMax
		{
			set
			{
				if (value > 2000.0f)
				{
					value = 2000.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("orientateMax", value);
			}
		}

		/// <summary>
		/// If true then orientate the character to face the point from where it started falling.
		/// High fall like the one in "Die Hard" with Alan Rickman.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AlanRickman
		{
			set
			{
				SetArgument("alanRickman", value);
			}
		}

		/// <summary>
		/// Try to execute a forward Roll on landing.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool FowardRoll
		{
			set
			{
				SetArgument("fowardRoll", value);
			}
		}

		/// <summary>
		/// Blend to a zero pose when forward roll is attempted.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseZeroPose_withFowardRoll
		{
			set
			{
				SetArgument("useZeroPose_withFowardRoll", value);
			}
		}

		/// <summary>
		/// Angle from vertical the pdController is driving to (positive = forwards).
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -3.1f.
		/// Max value = 3.1f.
		/// </remarks>
		public float AimAngleBase
		{
			set
			{
				if (value > 3.1f)
				{
					value = 3.1f;
				}

				if (value < -3.1f)
				{
					value = -3.1f;
				}

				SetArgument("aimAngleBase", value);
			}
		}

		/// <summary>
		/// Scale to add/subtract from aimAngle based on forward speed (Internal).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FowardVelRotation
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("fowardVelRotation", value);
			}
		}

		/// <summary>
		/// Scale to change to amount of vel that is added to the foot ik from the velocity (Internal).
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FootVelCompScale
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("footVelCompScale", value);
			}
		}

		/// <summary>
		/// Sideoffset for the feet during prepareForLanding. +ve = right.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SideD
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("sideD", value);
			}
		}

		/// <summary>
		/// Forward offset for the feet during prepareForLanding.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float FowardOffsetOfLegIK
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("fowardOffsetOfLegIK", value);
			}
		}

		/// <summary>
		/// Leg Length for ik (Internal)//unused.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float LegL
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("legL", value);
			}
		}

		/// <summary>
		/// 0.5angle is 0.878 dot. Cutoff to go to the catchFall (internal) //mmmtodo do like crashOrLandCutOff.
		/// </summary>
		/// <remarks>
		/// Default value = 0.9f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CatchFallCutOff
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("catchFallCutOff", value);
			}
		}

		/// <summary>
		/// Strength of the legs at landing.
		/// </summary>
		/// <remarks>
		/// Default value = 12.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float LegStrength
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("legStrength", value);
			}
		}

		/// <summary>
		/// If true have enough strength to balance.
		/// If false not enough strength in legs to balance (even though bodyBlance called).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool Balance
		{
			set
			{
				SetArgument("balance", value);
			}
		}

		/// <summary>
		/// Never go into bailout (foetal).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool IgnorWorldCollisions
		{
			set
			{
				SetArgument("ignorWorldCollisions", value);
			}
		}

		/// <summary>
		/// Stunt man type fall.  Arm and legs circling direction controlled by angmom and orientation.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool AdaptiveCircling
		{
			set
			{
				SetArgument("adaptiveCircling", value);
			}
		}

		/// <summary>
		/// With stunt man type fall.  Hula reaction if can't see floor and not rotating fast.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool Hula
		{
			set
			{
				SetArgument("hula", value);
			}
		}

		/// <summary>
		/// Character needs to be moving less than this speed to consider fall as a recoverable one.
		/// </summary>
		/// <remarks>
		/// Default value = 15.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float MaxSpeedForRecoverableFall
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("maxSpeedForRecoverableFall", value);
			}
		}

		/// <summary>
		/// Character needs to be moving at least this fast horizontally to start bracing for impact if there is an object along its trajectory.
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float MinSpeedForBrace
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("minSpeedForBrace", value);
			}
		}

		/// <summary>
		/// Ray-cast normal doted with up direction has to be greater than this number to consider object flat enough to land on it.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LandingNormal
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("landingNormal", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 0.8f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float RdsForceMag
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rdsForceMag", value);
			}
		}

		/// <summary>
		/// RDS: Time for the targetlinearVelocity to decay to zero.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float RdsTargetLinVeDecayTime
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rdsTargetLinVeDecayTime", value);
			}
		}

		/// <summary>
		/// RDS: Helper torques are applied to match the spin of the character to the max of targetLinearVelocity and COMVelMag.
		/// -1 to use initial character velocity.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 30.0f.
		/// </remarks>
		public float RdsTargetLinearVelocity
		{
			set
			{
				if (value > 30.0f)
				{
					value = 30.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rdsTargetLinearVelocity", value);
			}
		}

		/// <summary>
		/// Start Catch Fall/RDS state with specified friction.
		/// Catch fall will overwrite based on setFallingReaction.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool RdsUseStartingFriction
		{
			set
			{
				SetArgument("rdsUseStartingFriction", value);
			}
		}

		/// <summary>
		/// Catch Fall/RDS starting friction.
		/// Catch fall will overwrite based on setFallingReaction.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float RdsStartingFriction
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rdsStartingFriction", value);
			}
		}

		/// <summary>
		/// Catch Fall/RDS starting friction minimum.
		/// Catch fall will overwrite based on setFallingReaction.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float RdsStartingFrictionMin
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rdsStartingFrictionMin", value);
			}
		}

		/// <summary>
		/// Velocity threshold under which RDS force mag will be applied.
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float RdsForceVelThreshold
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rdsForceVelThreshold", value);
			}
		}

		/// <summary>
		/// Force initial state (used in vehicle bail out to start SF_CatchFall (6) earlier.
		/// </summary>
		/// <remarks>
		/// Default value = 0.
		/// Min value = 0.
		/// Max value = 7.
		/// </remarks>
		public int InitialState
		{
			set
			{
				if (value > 7)
				{
					value = 7;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("initialState", value);
			}
		}

		/// <summary>
		/// Allow friction changes to be applied to the hands and feet.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ChangeExtremityFriction
		{
			set
			{
				SetArgument("changeExtremityFriction", value);
			}
		}

		/// <summary>
		/// Set up an immediate teeter in the direction of trave if initial state is SF_Balance.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool Teeter
		{
			set
			{
				SetArgument("teeter", value);
			}
		}

		/// <summary>
		/// Offset the default Teeter edge in the direction of travel.
		/// Will need to be tweaked depending on how close to the real edge AI tends to trigger the behavior.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TeeterOffset
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("teeterOffset", value);
			}
		}

		/// <summary>
		/// Time in seconds before ped should start actively trying to stop rolling.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float StopRollingTime
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("stopRollingTime", value);
			}
		}

		/// <summary>
		/// Scale for rebound assistance.
		/// 0 = off,
		/// 1 = very bouncy,
		/// 2 = jbone crazy.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ReboundScale
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("reboundScale", value);
			}
		}

		/// <summary>
		/// Part mask to apply rebound assistance.
		/// </summary>
		/// <remarks>
		/// Default value = uk.
		/// </remarks>
		public string ReboundMask
		{
			set
			{
				SetArgument("reboundMask", value);
			}
		}

		/// <summary>
		/// Force head avoid to be active during Catch Fall even when character is not on the ground.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ForceHeadAvoid
		{
			set
			{
				SetArgument("forceHeadAvoid", value);
			}
		}

		/// <summary>
		/// Pass-through parameter for Catch Fall spin reduction.  Increase to stop more spin. 0..1.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float CfZAxisSpinReduction
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("cfZAxisSpinReduction", value);
			}
		}

		/// <summary>
		/// Transition to splat state when com vel is below value, regardless of character health or fall velocity.
		/// Set to zero to disable.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float SplatWhenStopped
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("splatWhenStopped", value);
			}
		}

		/// <summary>
		/// Blend head to neutral pose com vel approaches zero.
		/// Linear between zero and value.
		/// Set to zero to disable.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float BlendHeadWhenStopped
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("blendHeadWhenStopped", value);
			}
		}

		/// <summary>
		/// Spread legs amount for pedal during fall.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SpreadLegs
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("spreadLegs", value);
			}
		}
	}

	public sealed class StaggerFallHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the StaggerFallHelper for sending a StaggerFall <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the StaggerFall <see cref="Message"/> to.</param>
		public StaggerFallHelper(Ped ped) : base(ped, "staggerFall")
		{
		}

		/// <summary>
		/// Stiffness of arms. Catch_fall's stiffness scales with this value, but has default values when this is default.
		/// </summary>
		/// <remarks>
		/// Default value = 12.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ArmStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armStiffness", value);
			}
		}

		/// <summary>
		/// Sets damping value for the arms.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armDamping", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float SpineDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spineDamping", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float SpineStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spineStiffness", value);
			}
		}

		/// <summary>
		/// ArmStiffness during the yanked timescale i.e. timeAtStartValues.
		/// </summary>
		/// <remarks>
		/// Default value = 3.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ArmStiffnessStart
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armStiffnessStart", value);
			}
		}

		/// <summary>
		/// ArmDamping during the yanked timescale i.e. timeAtStartValues.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmDampingStart
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armDampingStart", value);
			}
		}

		/// <summary>
		/// SpineDamping during the yanked timescale i.e. timeAtStartValues.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float SpineDampingStart
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spineDampingStart", value);
			}
		}

		/// <summary>
		/// SpineStiffness during the yanked timescale i.e. timeAtStartValues.
		/// </summary>
		/// <remarks>
		/// Default value = 3.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float SpineStiffnessStart
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spineStiffnessStart", value);
			}
		}

		/// <summary>
		/// Time spent with Start values for arms and spine stiffness and damping i.e. for whiplash effect.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float TimeAtStartValues
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("timeAtStartValues", value);
			}
		}

		/// <summary>
		/// Time spent ramping from Start to end values for arms and spine stiffness and damping i.e. for whiplash effect (occurs after timeAtStartValues).
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RampTimeFromStartValues
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rampTimeFromStartValues", value);
			}
		}

		/// <summary>
		/// Probability per step of time spent in a stagger step.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float StaggerStepProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("staggerStepProb", value);
			}
		}

		/// <summary>
		/// Steps taken before lowerBodyStiffness starts ramping down by perStepReduction1.
		/// </summary>
		/// <remarks>
		/// Default value = 2.
		/// Min value = 0.
		/// Max value = 100.
		/// </remarks>
		public int StepsTillStartEnd
		{
			set
			{
				if (value > 100)
				{
					value = 100;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("stepsTillStartEnd", value);
			}
		}

		/// <summary>
		/// Time from start of behavior before lowerBodyStiffness starts ramping down for rampTimeToEndValues to endValues.
		/// </summary>
		/// <remarks>
		/// Default value = 100.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float TimeStartEnd
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("timeStartEnd", value);
			}
		}

		/// <summary>
		/// Time spent ramping from lowerBodyStiffness to lowerBodyStiffnessEnd.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float RampTimeToEndValues
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rampTimeToEndValues", value);
			}
		}

		/// <summary>
		/// LowerBodyStiffness should be 12.
		/// </summary>
		/// <remarks>
		/// Default value = 13.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float LowerBodyStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("lowerBodyStiffness", value);
			}
		}

		/// <summary>
		/// LowerBodyStiffness at end.
		/// </summary>
		/// <remarks>
		/// Default value = 8.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float LowerBodyStiffnessEnd
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("lowerBodyStiffnessEnd", value);
			}
		}

		/// <summary>
		/// Amount of time (seconds) into the future that the character tries to step to.
		/// Bigger values try to recover with fewer, bigger steps.
		/// Smaller values recover with smaller steps, and generally recover less.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float PredictionTime
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("predictionTime", value);
			}
		}

		/// <summary>
		/// LowerBody stiffness will be reduced every step to make the character fallover.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float PerStepReduction1
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("perStepReduction1", value);
			}
		}

		/// <summary>
		/// LeanInDirection will be increased from 0 to leanInDirMax linearly at this rate.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float LeanInDirRate
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanInDirRate", value);
			}
		}

		/// <summary>
		/// Max of leanInDirection magnitude when going forwards.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanInDirMaxF
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanInDirMaxF", value);
			}
		}

		/// <summary>
		/// Max of leanInDirection magnitude when going backwards.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanInDirMaxB
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanInDirMaxB", value);
			}
		}

		/// <summary>
		/// Max of leanInDirectionHips magnitude when going forwards.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanHipsMaxF
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanHipsMaxF", value);
			}
		}

		/// <summary>
		/// Max of leanInDirectionHips magnitude when going backwards.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanHipsMaxB
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanHipsMaxB", value);
			}
		}

		/// <summary>
		/// Lean of spine to side in side velocity direction when going forwards.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -5.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float Lean2multF
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < -5.0f)
				{
					value = -5.0f;
				}

				SetArgument("lean2multF", value);
			}
		}

		/// <summary>
		/// Lean of spine to side in side velocity direction when going backwards.
		/// </summary>
		/// <remarks>
		/// Default value = -2.0f.
		/// Min value = -5.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float Lean2multB
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < -5.0f)
				{
					value = -5.0f;
				}

				SetArgument("lean2multB", value);
			}
		}

		/// <summary>
		/// Amount stance foot is behind com in the direction of velocity before the leg tries to pushOff to increase momentum.
		/// Increase to lower the probability of the pushOff making the character bouncy.
		/// </summary>
		/// <remarks>
		/// Default value = 0.2f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float PushOffDist
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("pushOffDist", value);
			}
		}

		/// <summary>
		/// Stance leg will only pushOff to increase momentum if the vertical hip velocity is less than this value. 0.4 seems like a good value.
		/// The higher it is the less this functionality is applied.
		/// If it is very low or negative this can stop the pushOff altogether.
		/// </summary>
		/// <remarks>
		/// Default value = 20.0f.
		/// Min value = -20.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float MaxPushoffVel
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < -20.0f)
				{
					value = -20.0f;
				}

				SetArgument("maxPushoffVel", value);
			}
		}

		/// <summary>
		/// HipBend scaled with velocity.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = -10.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float HipBendMult
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -10.0f)
				{
					value = -10.0f;
				}

				SetArgument("hipBendMult", value);
			}
		}

		/// <summary>
		/// Bend forwards at the hip (hipBendMult) whether moving backwards or forwards.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool AlwaysBendForwards
		{
			set
			{
				SetArgument("alwaysBendForwards", value);
			}
		}

		/// <summary>
		/// Spine bend scaled with velocity.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = -10.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float SpineBendMult
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < -10.0f)
				{
					value = -10.0f;
				}

				SetArgument("spineBendMult", value);
			}
		}

		/// <summary>
		/// Enable and provide a look-at target to make the character's head turn to face it while balancing, balancer default is 0.2.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseHeadLook
		{
			set
			{
				SetArgument("useHeadLook", value);
			}
		}

		/// <summary>
		/// Position of thing to look at.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 HeadLookPos
		{
			set
			{
				SetArgument("headLookPos", value);
			}
		}

		/// <summary>
		/// Level index of thing to look at.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int HeadLookInstanceIndex
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("headLookInstanceIndex", value);
			}
		}

		/// <summary>
		/// Probability [0-1] that headLook will be looking in the direction of velocity when stepping.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float HeadLookAtVelProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("headLookAtVelProb", value);
			}
		}

		/// <summary>
		/// Weighted probability that turn will be off.
		/// This is one of six turn type weights.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TurnOffProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("turnOffProb", value);
			}
		}

		/// <summary>
		/// Weighted probability of turning towards headLook target.
		/// This is one of six turn type weights.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Turn2TargetProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("turn2TargetProb", value);
			}
		}

		/// <summary>
		/// Weighted probability of turning towards velocity.
		/// This is one of six turn type weights.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float Turn2VelProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("turn2VelProb", value);
			}
		}

		/// <summary>
		/// Weighted probability of turning away from headLook target.
		/// This is one of six turn type weights.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TurnAwayProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("turnAwayProb", value);
			}
		}

		/// <summary>
		/// Weighted probability of turning left.
		/// This is one of six turn type weights.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TurnLeftProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("turnLeftProb", value);
			}
		}

		/// <summary>
		/// Weighted probability of turning right.
		/// This is one of six turn type weights.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TurnRightProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("turnRightProb", value);
			}
		}

		/// <summary>
		/// Enable and provide a positive bodyTurnTimeout and provide a look-at target to make the character turn to face it while balancing.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseBodyTurn
		{
			set
			{
				SetArgument("useBodyTurn", value);
			}
		}

		/// <summary>
		/// Enable upper body reaction i.e. blindBrace and armswindmill.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UpperBodyReaction
		{
			set
			{
				SetArgument("upperBodyReaction", value);
			}
		}
	}

	public sealed class TeeterHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the TeeterHelper for sending a Teeter <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the Teeter <see cref="Message"/> to.</param>
		public TeeterHelper(Ped ped) : base(ped, "teeter")
		{
		}

		/// <summary>
		/// Defines the left edge point (left of character facing edge).
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(39.5f, 38.9f, 21.1f).
		/// Min value = 0.0f.
		/// </remarks>
		public Vector3 EdgeLeft
		{
			set
			{
				SetArgument("edgeLeft", Vector3.Maximize(value, new Vector3(0.0f, 0.0f, 0.0f)));
			}
		}

		/// <summary>
		/// Defines the right edge point (right of character facing edge).
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(39.5f, 39.9f, 21.1f).
		/// Min value = 0.0f.
		/// </remarks>
		public Vector3 EdgeRight
		{
			set
			{
				SetArgument("edgeRight", Vector3.Maximize(value, new Vector3(0.0f, 0.0f, 0.0f)));
			}
		}

		/// <summary>
		/// Stop stepping across the line defined by edgeLeft and edgeRight.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseExclusionZone
		{
			set
			{
				SetArgument("useExclusionZone", value);
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseHeadLook
		{
			set
			{
				SetArgument("useHeadLook", value);
			}
		}

		/// <summary>
		/// Call highFall if fallen over the edge.
		/// If false just call blended writhe (to go over the top of the fall behavior of the underlying behavior e.g. bodyBalance).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool CallHighFall
		{
			set
			{
				SetArgument("callHighFall", value);
			}
		}

		/// <summary>
		/// Lean away from the edge based on velocity towards the edge (if closer than 2m from edge).
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool LeanAway
		{
			set
			{
				SetArgument("leanAway", value);
			}
		}

		/// <summary>
		/// Time-to-edge threshold to start pre-teeter (windmilling, etc).
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float PreTeeterTime
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("preTeeterTime", value);
			}
		}

		/// <summary>
		/// Time-to-edge threshold to start leaning away from a potential fall.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float LeanAwayTime
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanAwayTime", value);
			}
		}

		/// <summary>
		/// Scales stay upright lean and hip pitch.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float LeanAwayScale
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("leanAwayScale", value);
			}
		}

		/// <summary>
		/// Time-to-edge threshold to start full-on teeter (more aggressive lean, drop-and-twist, etc).
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float TeeterTime
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("teeterTime", value);
			}
		}
	}

	public sealed class UpperBodyFlinchHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the UpperBodyFlinchHelper for sending a UpperBodyFlinch <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the UpperBodyFlinch <see cref="Message"/> to.</param>
		public UpperBodyFlinchHelper(Ped ped) : base(ped, "upperBodyFlinch")
		{
		}

		/// <summary>
		/// Left-Right distance between the hands.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float HandDistanceLeftRight
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("handDistanceLeftRight", value);
			}
		}

		/// <summary>
		/// Front-Back distance between the hands.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float HandDistanceFrontBack
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("handDistanceFrontBack", value);
			}
		}

		/// <summary>
		/// Vertical distance between the hands.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float HandDistanceVertical
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("handDistanceVertical", value);
			}
		}

		/// <summary>
		/// Stiffness of body. Value carries over to head look, spine twist.
		/// </summary>
		/// <remarks>
		/// Default value = 11.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float BodyStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("bodyStiffness", value);
			}
		}

		/// <summary>
		/// Damping value used for upper body.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float BodyDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("bodyDamping", value);
			}
		}

		/// <summary>
		/// Amount to bend the back during the flinch.
		/// </summary>
		/// <remarks>
		/// Default value = -0.6f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float BackBendAmount
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("backBendAmount", value);
			}
		}

		/// <summary>
		/// Toggle to use the right arm.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseRightArm
		{
			set
			{
				SetArgument("useRightArm", value);
			}
		}

		/// <summary>
		/// Toggle to Use the Left arm.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseLeftArm
		{
			set
			{
				SetArgument("useLeftArm", value);
			}
		}

		/// <summary>
		/// Amplitude of the perlin noise applied to the arms positions in the flinch to the front part of the behavior.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float NoiseScale
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("noiseScale", value);
			}
		}

		/// <summary>
		/// Relaxes the character for 1 frame if set.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool NewHit
		{
			set
			{
				SetArgument("newHit", value);
			}
		}

		/// <summary>
		/// Always protect head.
		/// Note if false then character flinches if target is in front, protects head if target is behind.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool ProtectHeadToggle
		{
			set
			{
				SetArgument("protectHeadToggle", value);
			}
		}

		/// <summary>
		/// Don't protect head only brace from front. Turned on by bcr.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool DontBraceHead
		{
			set
			{
				SetArgument("dontBraceHead", value);
			}
		}

		/// <summary>
		/// Turned of by bcr.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool ApplyStiffness
		{
			set
			{
				SetArgument("applyStiffness", value);
			}
		}

		/// <summary>
		/// Look away from target (unless protecting head then look between feet).
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool HeadLookAwayFromTarget
		{
			set
			{
				SetArgument("headLookAwayFromTarget", value);
			}
		}

		/// <summary>
		/// Use headlook.
		/// </summary>
		/// <remarks>
		/// Default value = True.
		/// </remarks>
		public bool UseHeadLook
		{
			set
			{
				SetArgument("useHeadLook", value);
			}
		}

		/// <summary>
		/// Ve balancer turn Towards, negative balancer turn Away, 0 balancer won't turn.
		/// There is a 50% chance that the character will not turn even if this parameter is set to turn.
		/// </summary>
		/// <remarks>
		/// Default value = 1.
		/// Min value = -2.
		/// Max value = 2.
		/// </remarks>
		public int TurnTowards
		{
			set
			{
				if (value > 2)
				{
					value = 2;
				}

				if (value < -2)
				{
					value = -2;
				}

				SetArgument("turnTowards", value);
			}
		}

		/// <summary>
		/// Position in world-space of object to flinch from.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 Pos
		{
			set
			{
				SetArgument("pos", value);
			}
		}
	}

	public sealed class YankedHelper : CustomHelper
	{
		/// <summary>
		/// Creates a new Instance of the YankedHelper for sending a Yanked <see cref="Message"/> to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> to send the Yanked <see cref="Message"/> to.</param>
		public YankedHelper(Ped ped) : base(ped, "yanked")
		{
		}

		/// <summary>
		/// Stiffness of arms when upright.
		/// </summary>
		/// <remarks>
		/// Default value = 11.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ArmStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("armStiffness", value);
			}
		}

		/// <summary>
		/// Sets damping value for the arms when upright.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armDamping", value);
			}
		}

		/// <summary>
		/// Spine damping when upright.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float SpineDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spineDamping", value);
			}
		}

		/// <summary>
		/// Spine stiffness when upright.
		/// </summary>
		/// <remarks>
		/// Default value = 10.0f.
		/// Min value = 6.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float SpineStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 6.0f)
				{
					value = 6.0f;
				}

				SetArgument("spineStiffness", value);
			}
		}

		/// <summary>
		/// Arm stiffness during the yanked timescale i.e. timeAtStartValues.
		/// </summary>
		/// <remarks>
		/// Default value = 3.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float ArmStiffnessStart
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armStiffnessStart", value);
			}
		}

		/// <summary>
		/// Arm damping during the yanked timescale i.e. timeAtStartValues.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float ArmDampingStart
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("armDampingStart", value);
			}
		}

		/// <summary>
		/// Spine damping during the yanked timescale i.e. timeAtStartValues.
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float SpineDampingStart
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spineDampingStart", value);
			}
		}

		/// <summary>
		/// Spine stiffness during the yanked timescale i.e. timeAtStartValues.
		/// </summary>
		/// <remarks>
		/// Default value = 3.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float SpineStiffnessStart
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spineStiffnessStart", value);
			}
		}

		/// <summary>
		/// Time spent with Start values for arms and spine stiffness and damping i.e. for whiplash effect.
		/// </summary>
		/// <remarks>
		/// Default value = 0.4f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float TimeAtStartValues
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("timeAtStartValues", value);
			}
		}

		/// <summary>
		/// Time spent ramping from Start to end values for arms and spine stiffness and damping i.e. for whiplash effect (occurs after timeAtStartValues).
		/// </summary>
		/// <remarks>
		/// Default value = 0.1f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RampTimeFromStartValues
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rampTimeFromStartValues", value);
			}
		}

		/// <summary>
		/// Steps taken before lowerBodyStiffness starts ramping down.
		/// </summary>
		/// <remarks>
		/// Default value = 2.
		/// Min value = 0.
		/// Max value = 100.
		/// </remarks>
		public int StepsTillStartEnd
		{
			set
			{
				if (value > 100)
				{
					value = 100;
				}

				if (value < 0)
				{
					value = 0;
				}

				SetArgument("stepsTillStartEnd", value);
			}
		}

		/// <summary>
		/// Time from start of behavior before lowerBodyStiffness starts ramping down by perStepReduction1.
		/// </summary>
		/// <remarks>
		/// Default value = 100.0f.
		/// Min value = 0.0f.
		/// Max value = 100.0f.
		/// </remarks>
		public float TimeStartEnd
		{
			set
			{
				if (value > 100.0f)
				{
					value = 100.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("timeStartEnd", value);
			}
		}

		/// <summary>
		/// Time spent ramping from lowerBodyStiffness to lowerBodyStiffnessEnd.
		/// </summary>
		/// <remarks>
		/// Default value = 0.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float RampTimeToEndValues
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rampTimeToEndValues", value);
			}
		}

		/// <summary>
		/// LowerBodyStiffness should be 12.
		/// </summary>
		/// <remarks>
		/// Default value = 12.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float LowerBodyStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("lowerBodyStiffness", value);
			}
		}

		/// <summary>
		/// LowerBodyStiffness at end.
		/// </summary>
		/// <remarks>
		/// Default value = 8.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float LowerBodyStiffnessEnd
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("lowerBodyStiffnessEnd", value);
			}
		}

		/// <summary>
		/// LowerBody stiffness will be reduced every step to make the character fallover.
		/// </summary>
		/// <remarks>
		/// Default value = 1.5f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float PerStepReduction
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("perStepReduction", value);
			}
		}

		/// <summary>
		/// Amount to bend forward at the hips (+ve forward, -ve backwards).
		/// Behavior switches between hipPitchForward and hipPitchBack.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = -1.3f.
		/// Max value = 1.3f.
		/// </remarks>
		public float HipPitchForward
		{
			set
			{
				if (value > 1.3f)
				{
					value = 1.3f;
				}

				if (value < -1.3f)
				{
					value = -1.3f;
				}

				SetArgument("hipPitchForward", value);
			}
		}

		/// <summary>
		/// Amount to bend backwards at the hips (+ve backwards, -ve forwards).
		/// Behavior switches between hipPitchForward and hipPitchBack.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = -1.3f.
		/// Max value = 1.3f.
		/// </remarks>
		public float HipPitchBack
		{
			set
			{
				if (value > 1.3f)
				{
					value = 1.3f;
				}

				if (value < -1.3f)
				{
					value = -1.3f;
				}

				SetArgument("hipPitchBack", value);
			}
		}

		/// <summary>
		/// Bend/Twist the spine amount.
		/// </summary>
		/// <remarks>
		/// Default value = 0.7f.
		/// Min value = 0.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float SpineBend
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spineBend", value);
			}
		}

		/// <summary>
		/// Foot friction when standing/stepping.  0.5 gives a good slide sometimes.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float FootFriction
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("footFriction", value);
			}
		}

		/// <summary>
		/// Min angle at which the turn with toggle to the other direction (actual toggle angle is chosen randomly in range min to max).
		/// If it is 1 then it will never toggle.
		/// If negative then no turn is applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = -0.1f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TurnThresholdMin
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -0.1f)
				{
					value = -0.1f;
				}

				SetArgument("turnThresholdMin", value);
			}
		}

		/// <summary>
		/// Max angle at which the turn with toggle to the other direction (actual toggle angle is chosen randomly in range min to max).
		/// If it is 1 then it will never toggle.
		/// If negative then no turn is applied.
		/// </summary>
		/// <remarks>
		/// Default value = 0.6f.
		/// Min value = -0.1f.
		/// Max value = 1.0f.
		/// </remarks>
		public float TurnThresholdMax
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -0.1f)
				{
					value = -0.1f;
				}

				SetArgument("turnThresholdMax", value);
			}
		}

		/// <summary>
		/// Enable and provide a look-at target to make the character's head turn to face it while balancing.
		/// </summary>
		/// <remarks>
		/// Default value = False.
		/// </remarks>
		public bool UseHeadLook
		{
			set
			{
				SetArgument("useHeadLook", value);
			}
		}

		/// <summary>
		/// Position of thing to look at.
		/// </summary>
		/// <remarks>
		/// Default value = Vector3(0.0f, 0.0f, 0.0f).
		/// </remarks>
		public Vector3 HeadLookPos
		{
			set
			{
				SetArgument("headLookPos", value);
			}
		}

		/// <summary>
		/// Level index of thing to look at.
		/// </summary>
		/// <remarks>
		/// Default value = -1.
		/// Min value = -1.
		/// </remarks>
		public int HeadLookInstanceIndex
		{
			set
			{
				if (value < -1)
				{
					value = -1;
				}

				SetArgument("headLookInstanceIndex", value);
			}
		}

		/// <summary>
		/// Probability [0-1] that headLook will be looking in the direction of velocity when stepping.
		/// </summary>
		/// <remarks>
		/// Default value = -1.0f.
		/// Min value = -1.0f.
		/// Max value = 1.0f.
		/// </remarks>
		public float HeadLookAtVelProb
		{
			set
			{
				if (value > 1.0f)
				{
					value = 1.0f;
				}

				if (value < -1.0f)
				{
					value = -1.0f;
				}

				SetArgument("headLookAtVelProb", value);
			}
		}

		/// <summary>
		/// For handsAndKnees catchfall ONLY: comVel above which rollDownstairs will start.
		/// </summary>
		/// <remarks>
		/// Default value = 2.0f.
		/// Min value = 0.0f.
		/// Max value = 20.0f.
		/// </remarks>
		public float ComVelRDSThresh
		{
			set
			{
				if (value > 20.0f)
				{
					value = 20.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("comVelRDSThresh", value);
			}
		}

		/// <summary>
		/// 0.25 A complete wiggle will take 4*hulaPeriod.
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float HulaPeriod
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("hulaPeriod", value);
			}
		}

		/// <summary>
		/// Amount of hip movement.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float HipAmplitude
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("hipAmplitude", value);
			}
		}

		/// <summary>
		/// Amount of spine movement.
		/// </summary>
		/// <remarks>
		/// Default value = 1.0f.
		/// Min value = 0.0f.
		/// Max value = 4.0f.
		/// </remarks>
		public float SpineAmplitude
		{
			set
			{
				if (value > 4.0f)
				{
					value = 4.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("spineAmplitude", value);
			}
		}

		/// <summary>
		/// Wriggle relaxes for a minimum of minRelaxPeriod (if it is negative it is a multiplier on the time previously spent wriggling).
		/// </summary>
		/// <remarks>
		/// Default value = 0.3f.
		/// Min value = -5.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float MinRelaxPeriod
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < -5.0f)
				{
					value = -5.0f;
				}

				SetArgument("minRelaxPeriod", value);
			}
		}

		/// <summary>
		/// Wriggle relaxes for a maximum of maxRelaxPeriod (if it is negative it is a multiplier on the time previously spent wriggling).
		/// </summary>
		/// <remarks>
		/// Default value = 1.5f.
		/// Min value = -5.0f.
		/// Max value = 5.0f.
		/// </remarks>
		public float MaxRelaxPeriod
		{
			set
			{
				if (value > 5.0f)
				{
					value = 5.0f;
				}

				if (value < -5.0f)
				{
					value = -5.0f;
				}

				SetArgument("maxRelaxPeriod", value);
			}
		}

		/// <summary>
		/// Amount of cheat torque applied to turn the character over.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float RollHelp
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("rollHelp", value);
			}
		}

		/// <summary>
		/// Leg Stiffness when on the ground.
		/// </summary>
		/// <remarks>
		/// Default value = 11.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float GroundLegStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("groundLegStiffness", value);
			}
		}

		/// <summary>
		/// Arm Stiffness when on the ground.
		/// </summary>
		/// <remarks>
		/// Default value = 11.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float GroundArmStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("groundArmStiffness", value);
			}
		}

		/// <summary>
		/// Spine Stiffness when on the ground.
		/// </summary>
		/// <remarks>
		/// Default value = 14.0f.
		/// Min value = 0.0f.
		/// Max value = 16.0f.
		/// </remarks>
		public float GroundSpineStiffness
		{
			set
			{
				if (value > 16.0f)
				{
					value = 16.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("groundSpineStiffness", value);
			}
		}

		/// <summary>
		/// Leg Damping when on the ground.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float GroundLegDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("groundLegDamping", value);
			}
		}

		/// <summary>
		/// Arm Damping when on the ground.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float GroundArmDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("groundArmDamping", value);
			}
		}

		/// <summary>
		/// Spine Damping when on the ground.
		/// </summary>
		/// <remarks>
		/// Default value = 0.5f.
		/// Min value = 0.0f.
		/// Max value = 2.0f.
		/// </remarks>
		public float GroundSpineDamping
		{
			set
			{
				if (value > 2.0f)
				{
					value = 2.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("groundSpineDamping", value);
			}
		}

		/// <summary>
		/// Friction multiplier on body parts when on ground.
		/// Character can look too slidy with groundFriction = 1.
		/// Higher values give a more jerky reaction but this seems timestep dependent especially for dragged by the feet.
		/// </summary>
		/// <remarks>
		/// Default value = 8.0f.
		/// Min value = 0.0f.
		/// Max value = 10.0f.
		/// </remarks>
		public float GroundFriction
		{
			set
			{
				if (value > 10.0f)
				{
					value = 10.0f;
				}

				if (value < 0.0f)
				{
					value = 0.0f;
				}

				SetArgument("groundFriction", value);
			}
		}
	}
}
