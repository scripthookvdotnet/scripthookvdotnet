#include "Euphoria.hpp"

namespace GTA
{
	namespace NaturalMotion
	{
		using namespace System;
		using namespace System::Collections::Generic;

		Euphoria::Euphoria(Ped ^ped) : _ped(ped)
		{
		}

		generic <typename T> where T : CustomHelper
		T Euphoria::GetHelper(String ^message)
		{
			CustomHelper ^h;

			if (ReferenceEquals(_helperCache, nullptr))
			{
				_helperCache = gcnew Dictionary<String ^, CustomHelper ^>();
			}
			else if (_helperCache->TryGetValue(message, h))
			{
				return static_cast<T>(h);
			}

			h = static_cast<CustomHelper ^>(Activator::CreateInstance(T::typeid, _ped));

			_helperCache->Add(message, h);

			return static_cast<T>(h);
		}
	}
}