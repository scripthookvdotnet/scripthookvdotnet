#pragma once

namespace GTA
{

	public interface class IHandleable
	{
		property int Handle
		{
			 int get();
		}

		bool Exists();
	};
}
