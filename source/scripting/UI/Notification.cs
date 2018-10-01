using System;
using GTA.Native;

namespace GTA.UI
{
	/// <summary>
	/// Methods to manage the display of notifications above the minimap.
	/// </summary>
	public static class Notification
	{
		/// <summary>
		/// Creates a <see cref="Notification"/> above the minimap with the given message.
		/// </summary>
		/// <param name="message">The message in the notification.</param>
		/// <param name="blinking">if set to <c>true</c> the notification will blink.</param>
		/// <returns>The handle of the <see cref="Notification"/> which can be used to hide it using <see cref="Notification.Hide(int)"/>.</returns>
		public static int Show(string message, bool blinking = false)
		{
			Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "CELL_EMAIL_BCON");

			const int maxStringLength = 99;

			for (int i = 0; i < message.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, message.Substring(i, System.Math.Min(maxStringLength, message.Length - i)));
			}

			return Function.Call<int>(Hash._DRAW_NOTIFICATION, blinking, true);
		}

		/// <summary>
		/// Creates a more advanced (SMS-alike) <see cref="Notification"/> above the minimap showing a sender icon, subject and the message.
		/// </summary>
		/// <param name="icon">The notification icon.</param>
		/// <param name="sender">The sender name.</param>
		/// <param name="subject">The subject line.</param>
		/// <param name="message">The message itself.</param>
		/// <param name="fadeIn">If <c>true</c> the message will fade in.</param>
		/// <param name="blinking">if set to <c>true</c> the notification will blink.</param>
		/// <returns>The handle of the <see cref="Notification"/> which can be used to hide it using <see cref="Notification.Hide(int)"/>.</returns>
		public static int Show(NotificationIcon icon, string sender, string subject, string message, bool fadeIn, bool blinking = false)
		{
			Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "STRING");

			const int maxStringLength = 99;

			for (int i = 0; i < message.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, message.Substring(i, System.Math.Min(maxStringLength, message.Length - i)));
			}

			Function.Call(Hash._SET_NOTIFICATION_MESSAGE_2, "CHAR_" + icon.ToString(), "CHAR_" + icon.ToString(), fadeIn, 1, sender, subject);

			return Function.Call<int>(Hash._DRAW_NOTIFICATION_4, blinking, true);
		}

		/// <summary>
		/// Hides a <see cref="Notification"/> instantly.
		/// </summary>
		/// <param name="handle">The handle of the <see cref="Notification"/> to hide.</param>
		public static void Hide(int handle)
		{
			Function.Call(Hash._REMOVE_NOTIFICATION, handle);
		}
	}

	public enum NotificationIcon
	{
		ABIGAIL, ALL_PLAYERS_CONF, AMANDA, AMMUNATION, ANDREAS,
		ANTONIA, ARTHUR, ASHLEY, BANK_BOL, BANK_FLEECA, BANK_MAZE,
		BARRY, BEVERLY, BIKESITE, BLANK_ENTRY, BLIMP, BLOCKED, BOATSITE,
		BROKEN_DOWN_GIRL, BUGSTARS, CALL911, CARSITE, CARSITE2, CASTRO,
		CHAT_CALL, CHEF, CHENG, CHENGSR, CHOP,
		CRIS, DAVE, DEFAULT, DENISE, DETONATEBOMB, DETONATEPHONE,
		DEVIN, DIAL_A_SUB, DOM, DOMESTIC_GIRL, DREYFUSS, DR_FRIEDLANDER,
		EPSILON, ESTATE_AGENT, FACEBOOK, FILMNOIR, FLOYD, FRANKLIN,
		FRANK_TREV_CONF, GAYMILITARY, HAO, HITCHER_GIRL, HUMANDEFAULT,
		HUNTER, JIMMY, JIMMY_BOSTON, JOE, JOSEF, JOSH, LAMAR,
		LAZLOW, LESTER, LESTER_DEATHWISH, LEST_FRANK_CONF, LEST_MIKE_CONF,
		LIFEINVADER, LS_CUSTOMS, LS_TOURIST_BOARD, MANUEL, MARNIE,
		MARTIN, MARY_ANN, MAUDE, MECHANIC, MICHAEL, MIKE_FRANK_CONF,
		MIKE_TREV_CONF, MILSITE, MINOTAUR, MOLLY, MP_ARMY_CONTACT,
		MP_BIKER_BOSS, MP_BIKER_MECHANIC, MP_BRUCIE, MP_DETONATEPHONE,
		MP_FAM_BOSS, MP_FIB_CONTACT, MP_FM_CONTACT, MP_GERALD, MP_JULIO,
		MP_MECHANIC, MP_MERRYWEATHER, MP_MEX_BOSS, MP_MEX_DOCKS, MP_MEX_LT,
		MP_MORS_MUTUAL, MP_PROF_BOSS, MP_RAY_LAVOY, MP_ROBERTO, MP_SNITCH,
		MP_STRETCH, MP_STRIPCLUB_PR, MRS_THORNHILL, MULTIPLAYER, NIGEL,
		OMEGA, ONEIL, ORTEGA, OSCAR, PATRICIA, PEGASUS_DELIVERY, PLANESITE,
		PROPERTY_ARMS_TRAFFICKING, PROPERTY_BAR_AIRPORT, PROPERTY_BAR_BAYVIEW,
		PROPERTY_BAR_CAFE_ROJO, PROPERTY_BAR_COCKOTOOS, PROPERTY_BAR_ECLIPSE, PROPERTY_BAR_FES,
		PROPERTY_BAR_HEN_HOUSE, PROPERTY_BAR_HI_MEN, PROPERTY_BAR_HOOKIES, PROPERTY_BAR_IRISH,
		PROPERTY_BAR_LES_BIANCO, PROPERTY_BAR_MIRROR_PARK, PROPERTY_BAR_PITCHERS, PROPERTY_BAR_SINGLETONS,
		PROPERTY_BAR_TEQUILALA, PROPERTY_BAR_UNBRANDED, PROPERTY_CAR_MOD_SHOP, PROPERTY_CAR_SCRAP_YARD,
		PROPERTY_CINEMA_DOWNTOWN, PROPERTY_CINEMA_MORNINGWOOD,
		PROPERTY_CINEMA_VINEWOOD, PROPERTY_GOLF_CLUB, PROPERTY_PLANE_SCRAP_YARD,
		PROPERTY_SONAR_COLLECTIONS, PROPERTY_TAXI_LOT, PROPERTY_TOWING_IMPOUND,
		PROPERTY_WEED_SHOP, RON, SAEEDA, SASQUATCH, SIMEON,
		SOCIAL_CLUB, SOLOMON, STEVE, STEVE_MIKE_CONF, STEVE_TREV_CONF,
		STRETCH, STRIPPER_CHASTITY, STRIPPER_CHEETAH, STRIPPER_FUFU,
		STRIPPER_INFERNUS, STRIPPER_JULIET, STRIPPER_NIKKI, STRIPPER_PEACH,
		STRIPPER_SAPPHIRE, TANISHA, TAXI, TAXI_LIZ, TENNIS_COACH,
		TOW_TONYA, TRACEY, TREVOR, WADE, YOUTUBE, CREATOR_PORTRAITS
	}
}
