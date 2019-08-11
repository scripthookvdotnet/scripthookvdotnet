namespace GTA
{
	public class InvalidWeaponComponent : WeaponComponent
	{
		internal InvalidWeaponComponent() : base(null, null, WeaponComponentHash.Invalid)
		{
		}

		public override bool Active
		{
			get => false;
			set
			{
				//Setter doesn't need to do anything for the invalid component
			}
		}

		public override string DisplayName => "WCT_INVALID";

		public override string LocalizedName => Game.GetLocalizedString(DisplayName);

		public override ComponentAttachmentPoint AttachmentPoint => ComponentAttachmentPoint.Invalid;
	}
}
