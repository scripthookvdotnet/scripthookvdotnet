#pragma once
#include "Vector3.hpp"


namespace GTA
{
	ref class Ped;
	namespace NaturalMotion
	{
		using namespace System;
		using namespace System::Collections::Generic;


		public ref class BaseMessage abstract
		{
		internal:
			BaseMessage(String ^Message);
			void pApplyTo(GTA::Ped ^TargetPed);
			void pApplyTo(GTA::Ped ^TargetPed, int duration);
			void pAbortTo(GTA::Ped ^TargetPed);
			void pSetArgument(System::String ^message, bool value);
			void pSetArgument(System::String ^message, int value);
			void pSetArgument(System::String ^message, float value);
			void pSetArgument(System::String ^message, double value) { pSetArgument(message, static_cast<float>(value)); }
			void pSetArgument(System::String ^message, System::String ^value);
			void pSetArgument(System::String ^message, Math::Vector3 value);
			String ^pMessage;
			void pGiveMessage(GTA::Ped ^TargetPed);
			void pResetArguments();
		private:
			Dictionary<String ^, bool> ^lBool;
			Dictionary<String ^, int> ^lInt;
			Dictionary<String ^, float> ^lFloat;
			Dictionary<String ^, String ^> ^lString;
			Dictionary<String ^, Math::Vector3> ^lVec;
		};

		public ref class BaseHelper abstract : public BaseMessage {

		protected:

			GTA::Ped^ pPed;
		internal:

			BaseHelper(GTA::Ped^ Ped, String ^Message);
			void pStart(int Duration);
			void pStart();
		public:
			void Stop();
		};
		public ref class CustomMessage : public BaseMessage 
		{
		public:
			CustomMessage(String ^Message);

			property String ^Message {
				String ^get();
			}

			void SendTo(GTA::Ped^ TargetPed, int Duration);
			void SendTo(GTA::Ped^ TargetPed);

			void Abort(GTA::Ped^ TargetPed);

			void SetArgument(System::String ^message, bool value) { pSetArgument(message, value); }
			void SetArgument(System::String ^message, int value) { pSetArgument(message, value); }
			void SetArgument(System::String ^message, float value) { pSetArgument(message, value); }
			void SetArgument(System::String ^message, double value) { pSetArgument(message, value); }
			void SetArgument(System::String ^message, System::String ^value) { pSetArgument(message, value); }
			void SetArgument(System::String ^message, Math::Vector3 value) { pSetArgument(message, value); }

		};

		public ref class CustomHelper : public BaseHelper {

		public:

			CustomHelper(GTA::Ped^ Ped, String ^Message);
			void Start(int Duration);
			void Start();

			void SetArgument(System::String ^message, bool value) { pSetArgument(message, value); }
			void SetArgument(System::String ^message, int value) { pSetArgument(message, value); }
			void SetArgument(System::String ^message, float value) { pSetArgument(message, value); }
			void SetArgument(System::String ^message, double value) { pSetArgument(message, value); }
			void SetArgument(System::String ^message, System::String ^value) { pSetArgument(message, value); }
			void SetArgument(System::String ^message, Math::Vector3 value) { pSetArgument(message, value); }
			void ResetArguments() { pResetArguments(); }

		};
	}
}
