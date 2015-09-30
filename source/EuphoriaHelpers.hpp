#pragma once
#include "EuphoriaBase.hpp"
namespace GTA
{
	namespace NaturalMotion
	{
		public ref class ArmsWindmillHelper : public CustomHelper {

		public:

			ArmsWindmillHelper(GTA::Ped^ Ped)
				:CustomHelper(Ped, "armsWindmillAdaptive") {
			}

			property bool DisableOnImpact {
				void set(bool value) {
					iSetArgument("disableOnImpact", value);
				}
			}
			property bool BendLeftElbow {
				void set(bool value) {
					iSetArgument("bendLeftElbow", value);
				}
			}
			property bool BendRightElbow {
				void set(bool value) {
					iSetArgument("bendRightElbow", value);
				}
			}

			property float AngleSpeed {
				void set(float value) {
					iSetArgument("angSpeed", value);
				}
			}
			property float BodyStiffness {
				void set(float value) {
					iSetArgument("bodyStiffness", value);
				}
			}
			property float Amplitude {
				void set(float value) {
					iSetArgument("amplitude", value);
				}
			}
			property float LeftElbowAngle {
				void set(float value) {
					iSetArgument("leftElbowAngle", value);
				}
			}
			property float RightElbowAngle {
				void set(float value) {
					iSetArgument("rightElbowAngle", value);
				}
			}
		}; 

		public ref class ApplyImpulseHelper : public CustomHelper {
		
		public:
			ApplyImpulseHelper(GTA::Ped^ Ped)
				:CustomHelper(Ped, "applyImpulse") {
			}
			property float Impulse {
				void set(float value) {
					iSetArgument("impulse", value);
				}
			}
		};

		public ref class BodyBalanceHelper : public CustomHelper {

		public:

			BodyBalanceHelper(GTA::Ped^ Ped)
				:CustomHelper(Ped, "bodyBalance") {
			}
			property float Stiffness {
				void set(float value) {
					iSetArgument("bodyStiffness", value);
				}
			}
			property float Damping {
				void set(float value) {
					iSetArgument("damping", value);
				}
			}
			property int MaxSteps {
				void set(int value) {
					iSetArgument("maxSteps", value);
				}
			}
		};

		public ref class BeingShotHelper : public CustomHelper {

		public:

			BeingShotHelper(GTA::Ped^ Ped)
				:CustomHelper(Ped, "shot") {
			}
			property bool ReachForWound {
				void set(bool value) {
					iSetArgument("reachForWound", value);
				}
			}
			property float TimeBeforeReachForWound {
				void set(float value) {
					iSetArgument("timeBeforeReachForWound", value);
				}
			}
			property float TimeBeforeCollapse {
				void set(float value) {
					iSetArgument("timeBeforeCollapseWoundLeg", value);
				}
			}

		};

		public ref class LeanInDirectionHelper : CustomHelper {

		public:
			LeanInDirectionHelper(GTA::Ped^ Ped)
				:CustomHelper(Ped, "leanInDirection") {
			}
			property float LeanAmount {
				void set(float value) {
					iSetArgument("leanAmount", value);
				}
			}
			property Math::Vector3 Direction {
				void set(Math::Vector3 value) {
					iSetArgument("dir", value);
				}
			}
		};

		public ref class LeanRandomHelper : CustomHelper {

		public:
			LeanRandomHelper(GTA::Ped^ Ped)
				:CustomHelper(Ped, "leanRandom") {
			}
			property float LeanAmountMin {
				void set(float value) {
					iSetArgument("leanAmountMin", value);
				}
			}
			property float LeanAmountMax {
				void set(float value) {
					iSetArgument("leanAmountMax", value);
				}
			}
			property float ChangeTimeMin {
				void set(float value) {
					iSetArgument("changeTimeMin", value);
				}
			}
			property float ChangeTimeMax {
				void set(float value) {
					iSetArgument("changeTimeMax", value);
				}
			}
		};

		public ref class LeanToPositionHelper : CustomHelper {
			
		public:
			LeanToPositionHelper(GTA::Ped^ Ped)
				:CustomHelper(Ped, "leanToPosition") {
			}
			property float LeanAmount {
				void set(float value) {
					iSetArgument("leanAmount", value);
				}
			}
			property Math::Vector3 Position {
				void set(Math::Vector3 value) {
					iSetArgument("pos", value);
				}
			}
		};

		public ref class PedalLegsHelper : public CustomHelper {

		public:
			PedalLegsHelper(GTA::Ped^ Ped)
				:CustomHelper(Ped, "pedalLegs") {
			}
			property bool UseLeftLeg {
				void set(bool value) {
					iSetArgument("pedalLeftLeg", value);
				}
			}
			property bool UseRightLeg {
				void set(bool value) {
					iSetArgument("pedalRightLeg", value);
				}
			}

		};
	}
}
