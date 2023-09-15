//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;
using GTA.Graphics;
using GTA.Native;

namespace GTA.UI
{
	/// <summary>
	/// Methods to manage the display of notifications above the minimap.
	/// </summary>
	public static class Notification
	{
		#region Fields
		static readonly string[] s_iconNames = new string[] {
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
		/// Displays the ticker message string in the top left of the HUD.
		/// </summary>
		/// <param name="message">
		/// The message body.
		/// </param>
		/// <param name="isImportant">
		/// If set to <see langword="true"/>, the message will flash and may have a custom background color
		/// or vibrate the controller.
		/// </param>
		/// <param name="cacheMessage">
		/// If set to <see langword="true"/>, the message will be cached in the pause menu.
		/// </param>
		/// <returns>
		/// A <see cref="FeedItem"/> if successfully posted a feed item; otherwise, <see langword="null"/>.
		/// </returns>
		public static FeedItem PostTicker(string message, bool isImportant, bool cacheMessage = true)
		{
			BeginTextCommandForFeedPostAndPushLongString(message);

			int handle = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER, isImportant, cacheMessage);
			return handle != -1 ? new FeedItem(handle) : null;
		}
		/// <summary>
		/// Displays the ticker message string in the top left of the HUD even if feed is paused.
		/// </summary>
		/// <inheritdoc cref="PostTicker(string, bool, bool)"/>
		public static FeedItem PostTickerForced(string message, bool isImportant, bool cacheMessage = true)
		{
			BeginTextCommandForFeedPostAndPushLongString(message);

			int handle = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER_FORCED, isImportant, cacheMessage);
			return handle != -1 ? new FeedItem(handle) : null;
		}
		/// <summary>
		/// Displays the ticker message string in the top left of the HUD
		/// containing tokens (i.e. <c>~BLIP_INFO_ICON~</c>).
		/// </summary>
		/// <inheritdoc cref="PostTicker(string, bool, bool)"/>
		public static FeedItem PostTickerWithTokens(string message, bool isImportant, bool cacheMessage = true)
		{
			BeginTextCommandForFeedPostAndPushLongString(message);

			int handle = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER_WITH_TOKENS, isImportant, cacheMessage);
			return handle != -1 ? new FeedItem(handle) : null;
		}
		/// <summary>
		/// Displays the text message contact image and localised text message string in the top left of the HUD.
		/// </summary>
		/// <param name="message">
		/// The message body.
		/// </param>
		/// <param name="texAsset">
		/// The <see cref="TextureAsset"/> for the contact image used in the text.
		/// message.
		/// </param>
		/// <param name="isImportant">
		/// If set to <see langword="true"/>, the message will flash and may have a custom background color
		/// or vibrate the controller.
		/// </param>
		/// <param name="icon">
		/// The icon type.
		/// </param>
		/// <param name="characterName">
		/// The character name, that is to say the sender.
		/// </param>
		/// <param name="subtitle">
		/// The subtitle (subject) of the text message.
		/// </param>
		/// <returns>
		/// A <see cref="FeedItem"/> if successfully posted a feed item; otherwise, <see langword="null"/>.
		/// </returns>
		public static FeedItem PostMessageText(string message, TextureAsset texAsset, bool isImportant, FeedTextIcon icon,
			string characterName, string subtitle = null)
		{
			BeginTextCommandForFeedPostAndPushLongString(message);

			(Txd txd, string texName) = texAsset;
			int handle = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, txd, texName,
				isImportant, (int)icon, characterName, subtitle);
			return handle != -1 ? new FeedItem(handle) : null;
		}
		/// <inheritdoc cref="PostUnlockTitleUpdateWithColor(string, string, FeedUnlockIcon, bool, HudColor, bool)"/>
		public static FeedItem PostUnlock(string message, string title, FeedUnlockIcon iconType)
		{
			BeginTextCommandForFeedPostAndPushLongString(message);

			int handle = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_UNLOCK, title, (int)iconType, null);
			return handle != -1 ? new FeedItem(handle) : null;
		}
		/// <inheritdoc cref="PostUnlockTitleUpdateWithColor(string, string, FeedUnlockIcon, bool, HudColor, bool)"/>
		public static FeedItem PostUnlockTitleUpdate(string message, string title, FeedUnlockIcon iconType, bool isImportant = true)
		{
			BeginTextCommandForFeedPostAndPushLongString(message);

			int handle = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_UNLOCK, title, (int)iconType, null, isImportant);
			return handle != -1 ? new FeedItem(handle) : null;
		}
		/// <summary>
		/// Displays the unlock component.
		/// </summary>
		/// <param name="message">
		/// The message body.
		/// </param>
		/// <param name="title">
		/// The unlock title.
		/// </param>
		/// <param name="iconType">
		/// The icon type.
		/// </param>
		/// <param name="isImportant">
		/// If set to <see langword="true"/>, the message will flash and may have a custom background color
		/// or vibrate the controller.
		/// </param>
		/// <param name="titleColor">
		/// The text color of <paramref name="title"/>.
		/// </param>
		/// <param name="titleIsLiteral">
		/// If set to <see langword="true"/>, the title string will be marked as the literal string,
		/// which is the same as in <see cref="PostUnlock(string, string, FeedUnlockIcon)"/> and
		/// <see cref="PostUnlockTitleUpdate(string, string, FeedUnlockIcon, bool)"/>.
		/// </param>
		/// <returns>
		/// A <see cref="FeedItem"/> if successfully posted a feed item; otherwise, <see langword="null"/>.
		/// </returns>
		public static FeedItem PostUnlockTitleUpdateWithColor(string message, string title, FeedUnlockIcon iconType, bool isImportant = true, HudColor titleColor = HudColor.PureWhite, bool titleIsLiteral = true)
		{
			BeginTextCommandForFeedPostAndPushLongString(message);

			int handle = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_UNLOCK, title, (int)iconType, null, isImportant,
				(int)titleColor, titleIsLiteral);
			return handle != -1 ? new FeedItem(handle) : null;
		}
		/// <summary>
		/// Displays the MP Versus feed component used when you die in multiplayer.
		/// </summary>
		/// <param name="char1TexAsset">
		/// The <see cref="TextureAsset"/> for the character 1 (left side).
		/// </param>
		/// <param name="val1">The integral value for the character 1 (left side).</param>
		/// <param name="char2TexAsset">
		/// The <see cref="TextureAsset"/> for the character 2 (right side).
		/// </param>
		/// <param name="val2">The integral value for the character 2 (right side).</param>
		/// <param name="customColor1">
		/// The custom color for the character 1 (left side) if set to a valid HUD color.
		/// </param>
		/// <param name="customColor2">
		/// The custom color for the character 2 (right side) if set to a valid HUD color.
		/// </param>
		/// <returns>
		/// A <see cref="FeedItem"/> if successfully posted a feed item; otherwise, <see langword="null"/>.
		/// </returns>
		public static FeedItem PostVersusTitleUpdate(TextureAsset char1TexAsset, int val1,
			TextureAsset char2TexAsset, int val2, HudColor customColor1 = HudColor.Invalid,
			HudColor customColor2 = HudColor.Invalid)
		{
			// We can't use a custom string for versus feed, so just use the empty string
			// In the exe, it is hardcoded to insert the string for VERSUS_SHORT at the center of the versus feed string template
			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, SHVDN.NativeMemory.NullString);

			(Txd char1Txd, string char1Txn) = char1TexAsset;
			(Txd char2Txd, string char2Txn) = char2TexAsset;
			int handle = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_VERSUS_TU, char1Txd, char1Txn, val1,
				char2Txd, char2Txn, val2, (int)customColor1, (int)customColor2);
			return handle != -1 ? new FeedItem(handle) : null;
		}
		/// <summary>
		/// Displays the award component.
		/// </summary>
		/// <param name="message">
		/// The message body.
		/// </param>
		/// <param name="texAsset">
		/// The <see cref="TextureAsset"/> for the icon on the left.
		/// </param>
		/// <param name="xp">
		/// The gained "RP" amount that will be displayed on the right side of the feed item.
		/// </param>
		/// <param name="awardColor">
		/// The award color. Specifies the text color and filters the texture.
		/// </param>
		/// <param name="title">
		/// The text label that will be displayed at the top of the feed item (title).
		/// Note that long text will not expand the space vertically and color tokens such as <c>~r~</c> will be
		/// shown as HTML tags.
		/// </param>
		/// <returns>
		/// A <see cref="FeedItem"/> if successfully posted a feed item; otherwise, <see langword="null"/>.
		/// </returns>
		public static FeedItem PostAward(string message, TextureAsset texAsset, int xp,
			HudColor awardColor, string title = null)
		{
			BeginTextCommandForFeedPostAndPushLongString(message);

			(Txd txd, string txn) = texAsset;
			int handle = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_AWARD, txd, txn, xp, (int)awardColor, title);
			return handle != -1 ? new FeedItem(handle) : null;
		}

		private static void BeginTextCommandForFeedPostAndPushLongString(string message)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, SHVDN.NativeMemory.CellEmailBcon);
			SHVDN.NativeFunc.PushLongString(message);
		}

		/// <summary>
		/// Creates a <see cref="Notification"/> above the minimap with the given message.
		/// </summary>
		/// <param name="message">The message in the notification.</param>
		/// <param name="blinking">if set to <see langword="true" /> the notification will blink.</param>
		/// <returns>The handle of the <see cref="Notification"/> which can be used to hide it using <see cref="Notification.Hide(int)"/>.</returns>
		[Obsolete("Use Notification.PostTicker instead.")]
		public static int Show(string message, bool blinking = false)
		{
			BeginTextCommandForFeedPostAndPushLongString(message);
			return Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER, blinking, true);
		}

		/// <summary>
		/// Creates a more advanced (SMS-alike) <see cref="Notification"/> above the minimap showing a sender icon, subject and the message.
		/// </summary>
		/// <param name="icon">
		/// The notification icon.
		/// Although you can use any pair of a texture dictionary (txd) and a texture as long as the txd is loaded
		/// and the txd contains the texture in <c>END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT</c>, you can only specify
		/// the textures chosen for <see cref="NotificationIcon"/> in this overload.
		/// </param>
		/// <param name="sender">The sender name.</param>
		/// <param name="subject">The subject line.</param>
		/// <param name="message">The message itself.</param>
		/// <param name="fadeIn">If <see langword="true" /> the message will fade in.</param>
		/// <param name="blinking">if set to <see langword="true" /> the notification will blink.</param>
		/// <returns>The handle of the <see cref="Notification"/> which can be used to hide it using <see cref="Notification.Hide(int)"/>.</returns>
		[Obsolete("Notification.Show is obsolete since it may fail to draw a texture icon for a text message." +
			"Use Notification.PostMessageText instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public static int Show(NotificationIcon icon, string sender, string subject, string message, bool fadeIn = false, bool blinking = false)
		{
			string iconName = s_iconNames[(int)icon];

			BeginTextCommandForFeedPostAndPushLongString(message);
			return Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, iconName, iconName, fadeIn, 1, sender, subject);
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
