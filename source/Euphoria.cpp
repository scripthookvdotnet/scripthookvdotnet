#include "Euphoria.hpp"

namespace GTA
{
	namespace NaturalMotion
	{
		Euphoria::Euphoria(Ped^ ped)
		{
			this->ped = ped;
		}

		generic <typename HelperType> where HelperType: BaseHelper
			HelperType Euphoria::GetHelper(String ^MessageID)
		{
			BaseHelper^ h;
			if (ReferenceEquals(pHelperCache, nullptr))
			{
				pHelperCache = gcnew Dictionary<String ^, BaseHelper^>();
			}
			else
			{
				if (pHelperCache->TryGetValue(MessageID, h)) return (HelperType)h;
			}
			h = (BaseHelper^)System::Activator::CreateInstance(HelperType::typeid, ped);
			pHelperCache->Add(MessageID, h);
			return (HelperType)h;
		}
	}
}