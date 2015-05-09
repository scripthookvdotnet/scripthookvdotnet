#include "Native.hpp"
#include "Prop.hpp"

namespace GTA
{
	Prop::Prop(int id) : Entity(id)
	{
	}

	Prop ^Prop::Any::get()
	{
		return gcnew Prop(0);
	}
}