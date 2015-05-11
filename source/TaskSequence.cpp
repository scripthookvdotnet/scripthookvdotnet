#include "TaskSequence.hpp"
#include "Native.hpp"
#include "Ped.hpp"

namespace GTA
{
	TaskSequence::TaskSequence()
	{
		int task = 0;
		Native::Function::Call(Native::Hash::OPEN_SEQUENCE_TASK, &task);
		this->seqHandle = task;
		this->CreateNullPed();
	}
	TaskSequence::TaskSequence(int handle)
	{
		this->seqHandle = handle;
		this->CreateNullPed();
	}
	TaskSequence::~TaskSequence()
	{
		Native::Function::Call(Native::Hash::CLEAR_SEQUENCE_TASK, this->seqHandle);
	}

	void TaskSequence::CreateNullPed()
	{
		if (hasNullPed)
		{
			return;
		}
		hasNullPed = true;
		nullPed = gcnew Ped(0);
	}

	int TaskSequence::Handle::get()
	{
		return this->seqHandle;
	}

	Tasks ^TaskSequence::AddTask::get()
	{
		if (this->isClosed)
		{
			throw gcnew System::Exception("You can't add tasks to a closed sequence!");
			return nullptr;
		}
		this->tasksCount++;
		return this->nullPed->Task;
	}
	bool TaskSequence::IsClosed::get()
	{
		return this->isClosed;
	}
	int TaskSequence::TasksCount::get()
	{
		return this->tasksCount;
	}

	void TaskSequence::CloseSequence()
	{
		if (!this->isClosed)
		{
			this->isClosed = true;
			Native::Function::Call(Native::Hash::CLOSE_SEQUENCE_TASK, this->seqHandle);
		}
	}
}