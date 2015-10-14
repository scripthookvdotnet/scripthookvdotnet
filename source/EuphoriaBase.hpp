#pragma once

#include "Vector3.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Ped;
	#pragma endregion

	namespace NaturalMotion
	{
		public ref class BaseMessage abstract
		{
		public:
			virtual System::String ^ToString() override;

		internal:
			BaseMessage(System::String ^Message);

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

			System::String ^_message;
			System::Collections::Generic::Dictionary<System::String ^, bool> ^lBool;
			System::Collections::Generic::Dictionary<System::String ^, int> ^lInt;
			System::Collections::Generic::Dictionary<System::String ^, float> ^lFloat;
			System::Collections::Generic::Dictionary<System::String ^, System::String ^> ^lString;
			System::Collections::Generic::Dictionary<System::String ^, Math::Vector3> ^lVec;
		};

		public ref class BaseHelper abstract : public BaseMessage
		{
		public:
			void Stop();

		internal:
			BaseHelper(Ped ^ped, System::String ^message);

			void Start();
			void Start(int duration);

		protected:
			Ped ^_ped;
		};
		public ref class CustomHelper : public BaseHelper
		{
		public:
			CustomHelper(Ped ^ped, System::String ^message);

			void Start();
			void Start(int duration);

			void SetArgument(System::String ^message, bool value);
			void SetArgument(System::String ^message, int value);
			void SetArgument(System::String ^message, float value);
			void SetArgument(System::String ^message, double value);
			void SetArgument(System::String ^message, System::String ^value);
			void SetArgument(System::String ^message, Math::Vector3 value);
			void ResetArguments();
		};
		public ref class CustomMessage : public BaseMessage
		{
		public:
			CustomMessage(System::String ^message);

			property System::String ^Message
			{
				System::String ^get();
			}

			void Abort(Ped ^target);
			void SendTo(Ped ^target);
			void SendTo(Ped ^target, int duration);

			void SetArgument(System::String ^message, bool value);
			void SetArgument(System::String ^message, int value);
			void SetArgument(System::String ^message, float value);
			void SetArgument(System::String ^message, double value);
			void SetArgument(System::String ^message, System::String ^value);
			void SetArgument(System::String ^message, Math::Vector3 value);
		};
	}
}