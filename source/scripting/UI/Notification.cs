using System;
using GTA.Native;

namespace GTA.UI
{
	public enum NotificationIcon
	{
		Abigail,
		AllPlayersConf,
		Amanda,
		Ammunation,
		Andreas,
		Antonia,
		Arthur,
		Ashley,
		BankBol,
		BankFleeca,
		BankMaze,
		Barry,
		Beverly,
		Bikesite,
		BlankEntry,
		Blimp,
		Blocked,
		Boatsite,
		BrokenDownGirl,
		Bugstars,
		Call911,
		Carsite,
		Carsite2,
		Castro,
		ChatCall,
		Chef,
		Cheng,
		Chengsr,
		Chop,
		Cris,
		Dave,
		Default,
		Denise,
		DetonateBomb,
		DetonatePhone,
		Devin,
		DialASub,
		Dom,
		DomesticGirl,
		Dreyfuss,
		DrFriedlander,
		Epsilon,
		EstateAgent,
		Facebook,
		FilmNoir,
		Floyd,
		Franklin,
		FrankTrevConf,
		Gaymilitary,
		Hao,
		HitcherGirl,
		Humandefault,
		Hunter,
		Jimmy,
		JimmyBoston,
		Joe,
		Josef,
		Josh,
		Lamar,
		Lazlow,
		Lester,
		LesterDeathwish,
		LesFrankConf,
		LesMikeConf,
		Lifeinvader,
		LsCustoms,
		LsTouristBoard,
		Manuel,
		Marnie,
		Martin,
		MaryAnn,
		Maude,
		Mechanic,
		Michael,
		MikeFrankConf,
		MikeTrevConf,
		Milsite,
		Minotaur,
		Molly,
		MpArmyContact,
		MpBikerBoss,
		MpBikerMechanic,
		MpBrucie,
		MpDetonatePhone,
		MpFamBoss,
		MpFIBContact,
		MpFmContact,
		MpGerald,
		MpJulio,
		MpMechanic,
		MpMerryweather,
		MpMexBoss,
		MpMexDocks,
		MpMexLt,
		MpMorsMutual,
		MpProfBoss,
		MpRayLavoy,
		MpRoberto,
		MpSnitch,
		MpStretch,
		MpStripclubPr,
		MrsThornhill,
		Multiplayer,
		Nigel,
		Omega,
		Oneil,
		Ortega,
		Oscar,
		Patricia,
		PegasusDelivery,
		Planesite,
		PropertyArmsTrafficking,
		PropertyBarAirport,
		PropertyBarBayview,
		PropertyBarCafeRojo,
		PropertyBarCockotoos,
		PropertyBarEclipse,
		PropertyBarFes,
		PropertyBarHenHouse,
		PropertyBarHiMen,
		PropertyBarHookies,
		PropertyBarIrish,
		PropertyBarLesBianco,
		PropertyBarMirrorPark,
		PropertyBarPitchers,
		PropertyBarSingletons,
		PropertyBarTequilala,
		PropertyBarUnbranded,
		PropertyCarModShop,
		PropertyCarScrapYard,
		PropertyCinemaDowntown,
		PropertyCinemaMorningwood,
		PropertyCinemaVinewood,
		PropertyGolfClub,
		PropertyPlaneScrapYard,
		PropertySonarCollections,
		PropertyTaxiLot,
		PropertyTowingImpound,
		PropertyWeedShop,
		Ron,
		Saeeda,
		Sasquatch,
		Simeon,
		SocialClub,
		Solomon,
		Steve,
		SteveMikeConf,
		SteveTrevConf,
		Stretch,
		StripperChastity,
		StripperCheetah,
		StripperFufu,
		StripperInfernus,
		StripperJuliet,
		StripperNikki,
		StripperPeach,
		StripperSapphire,
		Tanisha,
		Taxi,
		TaxiLiz,
		TennisCoach,
		TowTonya,
		Tracey,
		Trevor,
		Wade,
		Youtube,
		CreatorPortraits
	}

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
			Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, MemoryAccess.CellEmailBcon);
			Native.Function.PushLongString(message);
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
}
