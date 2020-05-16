using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public static class MobilePhone
	{

		[Flags]
		public enum MobilePhoneType
		{
			Michael = 0,
			Trevor = 1,
			Franklin = 2,
			Prologue = 4
		}

		/// <summary>
		/// Creates a mobile phone of the specified type.
		/// </summary>
		/// <param name="mobilePhoneType">type of phone to create</param>
		public static void CreateMobilePhone(MobilePhoneType mobilePhoneType)
		{
			Function.Call(Hash.CREATE_MOBILE_PHONE, (int)mobilePhoneType);
		}

		/// <summary>
		/// Destroys the currently active mobile phone.
		/// </summary>
		public static void DestroyMobilePhone()
		{
			Function.Call(Hash.DESTROY_MOBILE_PHONE);
		}

		/// <summary>
		/// Sets mobile phone display position
		/// </summary>
		/// <param name="position">position x y z</param>
		public static void SetPosition(Vector3 position)
		{
			Function.Call(Hash.SET_MOBILE_PHONE_POSITION, position.X, position.Y, position.Z);
		}

		/// <summary>
		/// Sets mobile phone display scale
		/// </summary>
		/// <param name="scale">scale factor 0.0 - 1.0</param>
		public static void SetScale(float scale)
		{
			Function.Call(Hash.SET_MOBILE_PHONE_SCALE, scale);
		}

		/// <summary>
		/// Sets mobile phone display rotation. Last parameter is unknown and always zero.
		/// </summary>
		/// <param name="rotation">rotation x y z</param>
		/// <param name="any">unknown</param>
		public static void SetRotation(Vector3 rotation, float any)
		{
			Function.Call(Hash.SET_MOBILE_PHONE_ROTATION, rotation.X, rotation.Y, rotation.Z, any);
		}

		/// <summary>
		/// Disables access to phone for one frame.
		/// When the phone is not out, and this method is called with false as it's parameter, you will not be able to bring up the phone. Although the up arrow key still works for whatever functionality it's used for, just not for the phone.
		/// This can be used for creating menu's when trying to disable the phone from being used.
		/// When the phone is out, and this method is called with false as it's parameter, the phone will not be able to scroll up. However, when you use the down arrow key, it's functionality still, works on the phone.
		/// You do not have to call the function again with true as a parameter, as soon as the function stops being called, the phone will again be usable.
		/// </summary>
		/// <param name="toggle">true if the phone should be available for this frame. false if the phone should be not available for this frame.</param>
		public static void DisablePhoneThisFrame(bool toggle)
		{
			Function.Call(Hash._DISABLE_PHONE_THIS_FRAME, toggle);
		}
	}
}
