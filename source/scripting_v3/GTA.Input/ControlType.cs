namespace GTA.Input
{
    /// <summary>
    /// Specifies the control context used when reading or writing input values.
    /// </summary>
    public enum ControlType
    {
        /// <summary>
        /// Normal gameplay controls for the player.
        /// </summary>
        PlayerControl,

        /// <summary>
        /// Camera input controls. 
        /// </summary>
        /// <remarks>
        /// Behaves the same as <see cref="PlayerControl"/> in all public versions of the game.
        /// </remarks>
        CameraControl,

        /// <summary>
        /// UI / menu (frontend) controls.
        /// </summary>
        FrontendControl,
    }
}
