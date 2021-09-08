//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA.UI
{
	/// <summary>
	/// Methods to manage the display of notifications above the minimap.
	/// </summary>
	public static class Notification
	{
		#region Fields
		static readonly string[] iconNames = new string[] {
			"CHAR_ABIGAIL", "CHAR_ALL_PLAYERS_CONF", "CHAR_AMANDA", "CHAR_AMMUNATION", "CHAR_ANDREAS",
			"CHAR_ANTONIA", "CHAR_ARTHUR", "CHAR_ASHLEY", "CHAR_BANK_BOL", "CHAR_BANK_FLEECA", "CHAR_BANK_MAZE",
			"CHAR_BARRY", "CHAR_BEVERLY", "CHAR_BIKESITE", "CHAR_BLANK_ENTRY", "CHAR_BLIMP", "CHAR_BLOCKED", "CHAR_BOATSITE",
			"CHAR_BROKEN_DOWN_GIRL", "CHAR_BUGSTARS", "CHAR_CALL911", "CHAR_CARSITE", "CHAR_CARSITE2", "CHAR_CASTRO",
			"CHAR_CHAT_CALL",  "CHAR_CHEF", "CHAR_CHENG", "CHAR_CHENGSR", "CHAR_CHOP",
			"CHAR_CRIS", "CHAR_DAVE", "CHAR_DEFAULT", "CHAR_DENISE", "CHAR_DETONATEBOMB", "CHAR_DETONATEPHONE",
			"CHAR_DEVIN", "CHAR_DIAL_A_SUB", "CHAR_DOM", "CHAR_DOMESTIC_GIRL", "CHAR_DREYFUSS", "CHAR_DR_FRIEDLANDER",
			"CHAR_EPSILON", "CHAR_ESTATE_AGENT", "CHAR_FACEBOOK", "CHAR_FILMNOIR", "CHAR_FLOYD", "CHAR_FRANKLIN",
			"CHAR_FRANK_TREV_CONF", "CHAR_GAYMILITARY", "CHAR_HAO", "CHAR_HITCHER_GIRL", "CHAR_HUMANDEFAULT",
			"CHAR_HUNTER", "CHAR_JIMMY", "CHAR_JIMMY_BOSTON", "CHAR_JOE", "CHAR_JOSEF", "CHAR_JOSH", "CHAR_LAMAR",
			"CHAR_LAZLOW", "CHAR_LESTER", "CHAR_LESTER_DEATHWISH", "CHAR_LEST_FRANK_CONF", "CHAR_LEST_MIKE_CONF",
			"CHAR_LIFEINVADER", "CHAR_LS_CUSTOMS", "CHAR_LS_TOURIST_BOARD", "CHAR_MANUEL", "CHAR_MARNIE",
			"CHAR_MARTIN", "CHAR_MARY_ANN", "CHAR_MAUDE", "CHAR_MECHANIC", "CHAR_MICHAEL", "CHAR_MIKE_FRANK_CONF",
			"CHAR_MIKE_TREV_CONF", "CHAR_MILSITE", "CHAR_MINOTAUR", "CHAR_MOLLY", "CHAR_MP_ARMY_CONTACT",
			"CHAR_MP_BIKER_BOSS", "CHAR_MP_BIKER_MECHANIC",  "CHAR_MP_BRUCIE", "CHAR_MP_DETONATEPHONE",
			"CHAR_MP_FAM_BOSS", "CHAR_MP_FIB_CONTACT", "CHAR_MP_FM_CONTACT", "CHAR_MP_GERALD", "CHAR_MP_JULIO",
			"CHAR_MP_MECHANIC", "CHAR_MP_MERRYWEATHER", "CHAR_MP_MEX_BOSS", "CHAR_MP_MEX_DOCKS", "CHAR_MP_MEX_LT",
			"CHAR_MP_MORS_MUTUAL", "CHAR_MP_PROF_BOSS", "CHAR_MP_RAY_LAVOY", "CHAR_MP_ROBERTO", "CHAR_MP_SNITCH",
			"CHAR_MP_STRETCH", "CHAR_MP_STRIPCLUB_PR", "CHAR_MRS_THORNHILL", "CHAR_MULTIPLAYER", "CHAR_NIGEL",
			"CHAR_OMEGA", "CHAR_ONEIL", "CHAR_ORTEGA", "CHAR_OSCAR", "CHAR_PATRICIA", "CHAR_PEGASUS_DELIVERY","CHAR_PLANESITE",
			"CHAR_PROPERTY_ARMS_TRAFFICKING", "CHAR_PROPERTY_BAR_AIRPORT", "CHAR_PROPERTY_BAR_BAYVIEW",
			"CHAR_PROPERTY_BAR_CAFE_ROJO","CHAR_PROPERTY_BAR_COCKOTOOS", "CHAR_PROPERTY_BAR_ECLIPSE", "CHAR_PROPERTY_BAR_FES",
			"CHAR_PROPERTY_BAR_HEN_HOUSE", "CHAR_PROPERTY_BAR_HI_MEN", "CHAR_PROPERTY_BAR_HOOKIES", "CHAR_PROPERTY_BAR_IRISH",
			"CHAR_PROPERTY_BAR_LES_BIANCO", "CHAR_PROPERTY_BAR_MIRROR_PARK", "CHAR_PROPERTY_BAR_PITCHERS", "CHAR_PROPERTY_BAR_SINGLETONS",
			"CHAR_PROPERTY_BAR_TEQUILALA", "CHAR_PROPERTY_BAR_UNBRANDED", "CHAR_PROPERTY_CAR_MOD_SHOP", "CHAR_PROPERTY_CAR_SCRAP_YARD",
			"CHAR_PROPERTY_CINEMA_DOWNTOWN", "CHAR_PROPERTY_CINEMA_MORNINGWOOD",
			"CHAR_PROPERTY_CINEMA_VINEWOOD", "CHAR_PROPERTY_GOLF_CLUB", "CHAR_PROPERTY_PLANE_SCRAP_YARD",
			"CHAR_PROPERTY_SONAR_COLLECTIONS", "CHAR_PROPERTY_TAXI_LOT", "CHAR_PROPERTY_TOWING_IMPOUND",
			"CHAR_PROPERTY_WEED_SHOP", "CHAR_RON", "CHAR_SAEEDA", "CHAR_SASQUATCH", "CHAR_SIMEON",
			"CHAR_SOCIAL_CLUB", "CHAR_SOLOMON", "CHAR_STEVE", "CHAR_STEVE_MIKE_CONF", "CHAR_STEVE_TREV_CONF",
			"CHAR_STRETCH", "CHAR_STRIPPER_CHASTITY", "CHAR_STRIPPER_CHEETAH",  "CHAR_STRIPPER_FUFU",
			"CHAR_STRIPPER_INFERNUS", "CHAR_STRIPPER_JULIET", "CHAR_STRIPPER_NIKKI", "CHAR_STRIPPER_PEACH",
			"CHAR_STRIPPER_SAPPHIRE", "CHAR_TANISHA", "CHAR_TAXI", "CHAR_TAXI_LIZ", "CHAR_TENNIS_COACH",
			"CHAR_TOW_TONYA", "CHAR_TRACEY", "CHAR_TREVOR", "CHAR_WADE", "CHAR_YOUTUBE", "CHAR_CREATOR_PORTRAITS",
		};
		#endregion

		/// <summary>
		/// Creates a <see cref="Notification"/> above the minimap with the given message.
		/// </summary>
		/// <param name="message">The message in the notification.</param>
		/// <param name="blinking">if set to <see langword="true" /> the notification will blink.</param>
		/// <returns>The handle of the <see cref="Notification"/> which can be used to hide it using <see cref="Notification.Hide(int)"/>.</returns>
		public static int Show(string message, bool blinking = false)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, SHVDN.NativeMemory.CellEmailBcon);
			SHVDN.NativeFunc.PushLongString(message);
			return Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER, blinking, true);
		}

		/// <summary>
		/// Creates a more advanced (SMS-alike) <see cref="Notification"/> above the minimap showing a sender icon, subject and the message.
		/// </summary>
		/// <param name="icon">The notification icon.</param>
		/// <param name="sender">The sender name.</param>
		/// <param name="subject">The subject line.</param>
		/// <param name="message">The message itself.</param>
		/// <param name="fadeIn">If <see langword="true" /> the message will fade in.</param>
		/// <param name="blinking">if set to <see langword="true" /> the notification will blink.</param>
		/// <returns>The handle of the <see cref="Notification"/> which can be used to hide it using <see cref="Notification.Hide(int)"/>.</returns>
		public static int Show(NotificationIcon icon, string sender, string subject, string message, bool fadeIn = false, bool blinking = false)
		{
			string iconName = iconNames[(int)icon];

			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, SHVDN.NativeMemory.CellEmailBcon);
			SHVDN.NativeFunc.PushLongString(message);
			Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, iconName, iconName, fadeIn, 1, sender, subject);

			return Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER, blinking, true);
		}

		/// <summary>
		/// Hides a <see cref="Notification"/> instantly.
		/// </summary>
		/// <param name="handle">The handle of the <see cref="Notification"/> to hide.</param>
		public static void Hide(int handle)
		{
			Function.Call(Hash.THEFEED_REMOVE_ITEM, handle);
		}
	}
}
