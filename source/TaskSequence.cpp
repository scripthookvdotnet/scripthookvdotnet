#include "TaskSequence.hpp"
#include "Ped.hpp"
#include "Native.hpp"

namespace GTA
{
	TaskSequence::TaskSequence() : mHandle(0), mCount(0), mIsClosed(false)
	{
		Native::Function::Call(Native::Hash::OPEN_SEQUENCE_TASK, &this->mHandle);

		if (System::Object::ReferenceEquals(sNullPed, nullptr))
		{
			sNullPed = gcnew Ped(0);
		}
	}
	TaskSequence::TaskSequence(int handle) : mHandle(handle), mCount(0), mIsClosed(false)
	{
		if (System::Object::ReferenceEquals(sNullPed, nullptr))
		{
			sNullPed = gcnew Ped(0);
		}
	}
	TaskSequence::~TaskSequence()
	{
		Native::Function::Call(Native::Hash::CLEAR_SEQUENCE_TASK, this->mHandle);
	}

	int TaskSequence::Handle::get()
	{
		return this->mHandle;
	}
	int TaskSequence::Count::get()
	{
		return this->mCount;
	}
	bool TaskSequence::IsClosed::get()
	{
		return this->mIsClosed;
	}
	Tasks ^TaskSequence::AddTask::get()
	{
		if (this->mIsClosed)
		{
			throw gcnew System::Exception("You can't add tasks to a closed sequence!");
		}

		this->mCount++;

		return this->sNullPed->Task;
	}

	void TaskSequence::Close()
	{
		if (this->mIsClosed)
		{
			return;
		}

		Native::Function::Call(Native::Hash::CLOSE_SEQUENCE_TASK, this->mHandle);

		this->mIsClosed = true;
	}
}