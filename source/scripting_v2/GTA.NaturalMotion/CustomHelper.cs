//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA.NaturalMotion
{
	/// <summary>
	/// A helper class for building a <seealso cref="Message"/> and sending it to a given <see cref="Ped"/>.
	/// </summary>
	public abstract class CustomHelper : Message
	{
		#region Fields
		private readonly Ped _ped;
		#endregion

		/// <summary>
		/// Creates a helper class for building a NaturalMotion Euphoria message to send to a given <see cref="Ped"/>.
		/// </summary>
		/// <param name="target">The <see cref="Ped"/> that the message will be applied to.</param>
		/// <param name="message">The name of the natural motion message.</param>
		protected CustomHelper(Ped target, string message) : base(message)
		{
			_ped = target;
		}

		/// <summary>
		/// Starts this behavior on the <see cref="Ped"/> and loop it until manually aborted.
		/// </summary>
		public void Start()
		{
			SendTo(_ped);
		}
		/// <summary>
		/// Starts this behavior on the <see cref="Ped"/> for a specified duration.
		/// </summary>
		/// <param name="duration">How long to apply the behavior for (-1 for looped).</param>
		public void Start(int duration)
		{
			SendTo(_ped, duration);
		}

		/// <summary>
		/// Stops this behavior on the <see cref="Ped"/>.
		/// </summary>
		public void Stop()
		{
			Abort(_ped);
		}

		/// <summary>
		/// Aborts this behavior on the <see cref="Ped"/>.
		/// </summary>
		public void Abort()
		{
			Abort(_ped);
		}
	}
}
