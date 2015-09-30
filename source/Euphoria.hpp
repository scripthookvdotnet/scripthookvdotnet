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

			property GTA::NaturalMotion::ArmsWindmillHelper^ ArmsWindmill {
				GTA::NaturalMotion::ArmsWindmillHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ArmsWindmillHelper^>("armsWindmillAdaptive");
				}
			}

			property GTA::NaturalMotion::ApplyImpulseHelper^ ApplyImpulse {
				GTA::NaturalMotion::ApplyImpulseHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ApplyImpulseHelper^>("applyImpulse");
				}
			}

			property GTA::NaturalMotion::BodyBalanceHelper^ BodyBalance {
				GTA::NaturalMotion::BodyBalanceHelper^ get() {
					return GetHelper<GTA::NaturalMotion::BodyBalanceHelper^>("bodyBalance");
				}
			}

			property GTA::NaturalMotion::BeingShotHelper^ BeingShot {
				GTA::NaturalMotion::BeingShotHelper^ get() {
					return GetHelper<GTA::NaturalMotion::BeingShotHelper^>("shot");
				}
			}

			property GTA::NaturalMotion::LeanInDirectionHelper^ LeanInDirection {
				GTA::NaturalMotion::LeanInDirectionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::LeanInDirectionHelper^>("leanInDirection");
				}
			}

			property GTA::NaturalMotion::HipsLeanInDirectionHelper^ HipsLeanInDirection {
				GTA::NaturalMotion::HipsLeanInDirectionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::HipsLeanInDirectionHelper^>("hipsLeanInDirection");
				}
			}

			property GTA::NaturalMotion::ForceLeanInDirectionHelper^ ForceLeanInDirection {
				GTA::NaturalMotion::ForceLeanInDirectionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ForceLeanInDirectionHelper^>("forceLeanInDirection");
				}
			}

			property GTA::NaturalMotion::LeanRandomHelper^ LeanRandom {
				GTA::NaturalMotion::LeanRandomHelper^ get() {
					return GetHelper<GTA::NaturalMotion::LeanRandomHelper^>("leanRandom");
				}
			}

			property GTA::NaturalMotion::HipsLeanRandomHelper^ HipsLeanRandom {
				GTA::NaturalMotion::HipsLeanRandomHelper^ get() {
					return GetHelper<GTA::NaturalMotion::HipsLeanRandomHelper^>("hipsLeanRandom");
				}
			}

			property GTA::NaturalMotion::ForceLeanRandomHelper^ ForceLeanRandom {
				GTA::NaturalMotion::ForceLeanRandomHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ForceLeanRandomHelper^>("forceLeanRandom");
				}
			}

			property GTA::NaturalMotion::LeanToPositionHelper^ LeanToPosition {
				GTA::NaturalMotion::LeanToPositionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::LeanToPositionHelper^>("leanToPosition");
				}
			}

			property GTA::NaturalMotion::HipsLeanToPositionHelper^ HipsLeanToPosition {
				GTA::NaturalMotion::HipsLeanToPositionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::HipsLeanToPositionHelper^>("hipsLeanToPosition");
				}
			}

			property GTA::NaturalMotion::ForceLeanToPositionHelper^ ForceLeanToPosition {
				GTA::NaturalMotion::ForceLeanToPositionHelper^ get() {
					return GetHelper<GTA::NaturalMotion::ForceLeanToPositionHelper^>("forceLeanToPosition");
				}
			}

			property GTA::NaturalMotion::PedalLegsHelper^ PedalLegs {
				GTA::NaturalMotion::PedalLegsHelper^ get() {
					return GetHelper<GTA::NaturalMotion::PedalLegsHelper^>("pedalLegs");
				}
			}


		};
	}
}
