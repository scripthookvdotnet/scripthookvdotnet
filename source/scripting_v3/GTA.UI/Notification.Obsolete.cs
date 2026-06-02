using System;
using System.ComponentModel;
using GTA.Native;

namespace GTA.UI
{
    public static partial class Notification
    {
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

    }
}
