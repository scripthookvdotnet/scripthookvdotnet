#pragma once

namespace GTA
{
	ref class Ped;
	ref class Tasks;

	public ref class TaskSequence sealed
	{
	public:
		TaskSequence();
		TaskSequence(int handle);
		~TaskSequence();

		property int Handle
		{
			int get();
		}
		property int Count
		{
			int get();
		}
		property bool IsClosed
		{
			bool get();
		}
		property Tasks ^AddTask
		{
			Tasks ^get();
		}

		void Close();

	private:
		static Ped ^sNullPed;
		int mHandle, mCount;
		bool mIsClosed;
	};
}