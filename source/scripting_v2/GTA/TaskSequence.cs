using GTA.Native;
using System;

namespace GTA
{
	public sealed class TaskSequence : IDisposable
	{
		static Ped nullPed = null;

		public TaskSequence()
		{
			int handle;
			unsafe
			{
				Function.Call(Hash.OPEN_SEQUENCE_TASK, &handle);
			}
			Handle = handle;

			if (nullPed is null)
			{
				nullPed = new Ped(0);
			}
		}
		public TaskSequence(int handle)
		{
			Handle = handle;

			if (nullPed is null)
			{
				nullPed = new Ped(0);
			}
		}

		public int Handle
		{
			get; private set;
		}

		public int Count
		{
			get; private set;
		}

		public bool IsClosed
		{
			get; private set;
		}

		public Tasks AddTask
		{
			get
			{
				if (IsClosed)
				{
					throw new Exception("You can't add tasks to a closed sequence!");
				}

				Count++;
				return nullPed.Task;
			}
		}

		public void Close()
		{
			Close(false);
		}
		public void Close(bool repeat)
		{
			if (IsClosed)
			{
				return;
			}

			Function.Call(Hash.SET_SEQUENCE_TO_REPEAT, Handle, repeat);
			Function.Call(Hash.CLOSE_SEQUENCE_TASK, Handle);

			IsClosed = true;
		}

		public void Dispose()
		{
			int handle = Handle;
			unsafe
			{
				Function.Call(Hash.CLEAR_SEQUENCE_TASK, &handle);
			}
			Handle = handle;
			GC.SuppressFinalize(this);
		}
	}
}
