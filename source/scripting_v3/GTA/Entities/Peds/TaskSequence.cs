//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	public sealed class TaskSequence : IDisposable
	{
		#region Fields
		static Ped nullPed = null;
		#endregion

		public TaskSequence()
		{
			int handle;
			unsafe
			{
				Function.Call(Hash.OPEN_SEQUENCE_TASK, &handle);
			}
			Handle = handle;

			if (nullPed == null)
			{
				nullPed = new Ped(0);
			}
		}
		public TaskSequence(int handle)
		{
			Handle = handle;

			if (nullPed == null)
			{
				nullPed = new Ped(0);
			}
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

		public TaskInvoker AddTask
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
	}
}
