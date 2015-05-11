#pragma once

namespace GTA
{
	ref class Ped;
	ref class Tasks;

	public ref class TaskSequence sealed
	{
	private:
		int seqHandle;
		int tasksCount = 0;
		bool hasNullPed = false;
		bool isClosed = false;


		void CreateNullPed();
		static Ped ^nullPed;

	public:
		TaskSequence();
		TaskSequence(int handle);
		~TaskSequence();

		property int Handle
		{
			int get();
		}
		property Tasks ^AddTask
		{
			Tasks ^get();
		}
		property bool IsClosed
		{
			bool get();
		}
		property int TasksCount
		{
			int get();
		}

		void CloseSequence();
	};
}