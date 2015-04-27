#pragma once

namespace GTA
{
	namespace Native
	{
		public ref class InputArgument
		{
		public:
			InputArgument(bool value);
			InputArgument(int value);
			InputArgument(float value);
			InputArgument(double value);
			InputArgument(System::IntPtr value);
			InputArgument(System::String ^value);

			static inline operator InputArgument ^ (bool value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^ (int value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^ (int *value)
			{
				return gcnew InputArgument(System::IntPtr(value));
			}
			static inline operator InputArgument ^ (float value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^ (float *value)
			{
				return gcnew InputArgument(System::IntPtr(value));
			}
			static inline operator InputArgument ^ (double value)
			{
				return gcnew InputArgument(value);
			}
			static inline operator InputArgument ^ (System::String ^value)
			{
				return gcnew InputArgument(value);
			}

			virtual System::String ^ToString() override
			{
				return this->mData.ToString();
			}

		internal:
			System::UInt64 mData;
		};
		public ref class OutputArgument : public InputArgument
		{
		public:
			OutputArgument();
			~OutputArgument()
			{
				this->!OutputArgument();
			}

			generic <typename T>
			T GetResult();

		private:
			!OutputArgument();

			unsigned char *mStorage;
		};

		public ref class Function abstract sealed
		{
		public:
			static System::UInt64 Address(System::String ^name);

			generic <typename T>
			static T Call(System::String ^name, ... array<InputArgument ^> ^arguments);
			static void Call(System::String ^name, ... array<InputArgument ^> ^arguments);

		internal:
			generic <typename T>
			static T Call(System::UInt64 address, ... array<InputArgument ^> ^arguments);

		private:
			static System::Collections::Generic::Dictionary<System::String ^, System::UInt64> ^sAddresses = gcnew System::Collections::Generic::Dictionary<System::String ^, System::UInt64>();
		};
	}
}