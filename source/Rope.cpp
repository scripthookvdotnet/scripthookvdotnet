#include "Rope.hpp"
#include "Native.hpp"

namespace GTA
{
	Rope::Rope(int handle) : mHandle(handle)
	{
	}

	int Rope::Handle::get()
	{
		return this->mHandle;
	}

	void Rope::Delete()
	{
		int handle = this->Handle;
		Native::Function::Call(Native::Hash::DELETE_ROPE, &handle);
	}
}