#pragma once

namespace GTA
{
	namespace Native
	{
		using namespace System;
		using namespace System::Collections::Generic;

		public ref class Argument
		{
		public:
			Argument(bool value);
			Argument(int value);
			Argument(float value);
			Argument(double value);
			Argument(IntPtr value);
			Argument(String ^value);

			static inline operator Argument ^ (bool value)
			{
				return gcnew Argument(value);
			}
			static inline operator Argument ^ (int value)
			{
				return gcnew Argument(value);
			}
			static inline operator Argument ^ (float value)
			{
				return gcnew Argument(value);
			}
			static inline operator Argument ^ (double value)
			{
				return gcnew Argument(value);
			}
			static inline operator Argument ^ (String ^value)
			{
				return gcnew Argument(value);
			}

			virtual String ^ToString() override
			{
				return this->mData.ToString();
			}

		internal:
			UInt64 mData;
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
			static UInt64 Address(String ^name);

			generic <typename T>
			static T Call(String ^name, ... array<Argument ^> ^arguments);
			static void Call(String ^name, ... array<Argument ^> ^arguments);

		internal:
			generic <typename T>
			static T Call(UInt64 address, ... array<Argument ^> ^arguments);

		private:
			static Dictionary<String ^, UInt64> ^sAddresses = gcnew Dictionary<String ^, UInt64>();
		};
	}
}