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

		generic <typename T> where T : BaseHelper
		T Euphoria::GetHelper(String ^MessageID)
		{
			BaseHelper ^h;

			if (ReferenceEquals(_helperCache, nullptr))
			{
				_helperCache = gcnew Dictionary<String ^, BaseHelper ^>();
			}
			else
			{
				if (_helperCache->TryGetValue(MessageID, h))
				{
					return static_cast<T>(h);
				}
			}

			h = static_cast<BaseHelper ^>(Activator::CreateInstance(T::typeid, _ped));

			_helperCache->Add(MessageID, h);

			return static_cast<T>(h);
		}
	}
}