#pragma once

namespace GTA
{
	namespace Native
	{
		public ref class Argument
		{
		public:
			Argument(bool value);
			Argument(int value);
			Argument(float value);
			Argument(double value);
			Argument(System::IntPtr value);
			Argument(System::String ^value);

			static inline operator Argument ^ (bool value)
			{
				return gcnew Argument(value);
			}
			static inline operator Argument ^ (int value)
			{
				return gcnew Argument(value);
			}
			static inline operator Argument ^ (int *value)
			{
				return gcnew Argument(System::IntPtr(value));
			}
			static inline operator Argument ^ (float value)
			{
				return gcnew Argument(value);
			}
			static inline operator Argument ^ (float *value)
			{
				return gcnew Argument(System::IntPtr(value));
			}
			static inline operator Argument ^ (double value)
			{
				return gcnew Argument(value);
			}
			static inline operator Argument ^ (System::String ^value)
			{
				return gcnew Argument(value);
			}

			virtual System::String ^ToString() override
			{
				return this->mData.ToString();
			}

		internal:
			System::UInt64 mData;
		};
		public ref class OutArgument : public Argument
		{
		public:
			OutArgument();

			generic <typename T>
			T GetResult();
		};

		public ref class Function abstract sealed
		{
		public:
			static System::UInt64 Address(System::String ^name);

			generic <typename T>
			static T Call(System::String ^name, ... array<Argument ^> ^arguments);
			static void Call(System::String ^name, ... array<Argument ^> ^arguments);

		internal:
			generic <typename T>
			static T Call(System::UInt64 address, ... array<Argument ^> ^arguments);

		private:
			static System::Collections::Generic::Dictionary<System::String ^, System::UInt64> ^sAddresses = gcnew System::Collections::Generic::Dictionary<System::String ^, System::UInt64>();
		};
	}
}