#pragma once

#include "Vector3.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Ped;
	#pragma endregion

	namespace NaturalMotion
	{
		public ref class Message
		{
		public:
			Message(System::String ^message);

			void Abort(Ped ^target);
			void SendTo(Ped ^target);
			void SendTo(Ped ^target, int duration);

			void SetArgument(System::String ^message, bool value);
			void SetArgument(System::String ^message, int value);
			void SetArgument(System::String ^message, float value);
			void SetArgument(System::String ^message, double value);
			void SetArgument(System::String ^message, System::String ^value);
			void SetArgument(System::String ^message, Math::Vector3 value);
			void ResetArguments();

			virtual System::String ^ToString() override;

		internal:
			System::String ^_message;
			System::Collections::Generic::Dictionary<System::String ^, Object ^> ^_arguments;
		};

		public ref class CustomHelper abstract : public Message
		{
		public:
			CustomHelper(Ped ^target, System::String ^message);

			void Start();
			void Start(int duration);
			void Stop();
			void Abort();

		protected:
			Ped ^_ped;
		};
	}
}