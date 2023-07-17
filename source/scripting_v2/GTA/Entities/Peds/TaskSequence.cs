//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a task sequence.
	/// After you create a <see cref="TaskSequence"/> instance, call <see cref="AddTask"/> as many as you want and call <see cref="Close()"/> or <see cref="Close(bool)"/>
	/// right after the instance creation statement.
	/// </summary>
	public sealed class TaskSequence : IDisposable
	{
		#region Fields

		private static Ped s_nullPed;
		#endregion

		public TaskSequence()
		{
			int handle;
			unsafe
			{
				Function.Call(Hash.OPEN_SEQUENCE_TASK, &handle);
			}
			Handle = handle;

			if (s_nullPed == null)
			{
				s_nullPed = new Ped(0);
			}
		}
		public TaskSequence(int handle)
		{
			Handle = handle;

			if (s_nullPed == null)
			{
				s_nullPed = new Ped(0);
			}
		}

		/// <summary>
		/// Clears the <see cref="TaskSequence"/>.
		/// You should call this method after you call <see cref="Tasks.PerformSequence(TaskSequence)"/> on some <see cref="Ped"/>.
		/// </summary>
		public void Dispose()
		{
			int handle = Handle;
			unsafe
			{
				Function.Call(Hash.CLEAR_SEQUENCE_TASK, &handle);
			}
		}

		/// <summary>
		/// Gets the <see cref="TaskSequence"/> handle.
		/// </summary>
		public int Handle
		{
			get;
		}

		/// <summary>
		/// Gets the number <see cref="TaskSequence"/> of stacked tasks.
		/// </summary>
		/// <remarks>
		/// Will return a incorrect value when you have add tasks via a variable cached from what <see cref="AddTask"/> returns.
		/// </remarks>
		public int Count
		{
			get; private set;
		}

		/// <summary>
		/// Gets the value indicating whether the <see cref="TaskSequence"/> is closed.
		/// </summary>
		public bool IsClosed
		{
			get; private set;
		}

		/// <summary>
		/// Gets the dedicated <see cref="Tasks"/>.
		/// You should invoke tasks like <c>taskSequence.AddTask.StandStill(1000)</c> (not via a cached TaskInvoker instance)
		/// so you can get the correct task count via <see cref="Count"/>.
		/// </summary>
		/// <remarks>If you need to invoke native functions for scripted tasks manually, set <c>0</c> as the ped handle argument until you close the sequence.</remarks>
		/// <exception cref="Exception">Thrown when the <see cref="TaskSequence"/> is closed.</exception>
		public Tasks AddTask
		{
			get
			{
				if (IsClosed)
				{
					throw new Exception("You can't add tasks to a closed sequence!");
				}

				Count++;
				return s_nullPed.Task;
			}
		}

		/// <inheritdoc cref="Close(bool)"/>
		public void Close()
		{
			Close(false);
		}
		/// <summary>
		/// Closes the <see cref="TaskSequence"/> if opened or does nothing if closed.
		/// </summary>
		/// <param name="repeat">Specifies whether to set the sequence to repeat.</param>
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
