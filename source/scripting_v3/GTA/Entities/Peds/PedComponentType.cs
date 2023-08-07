//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum PedComponentType
	{
		/// <summary>
		/// The head component. Despite the symbol name, this component is not limited to face.
		/// </summary>
		Face,
		/// <summary>
		/// The beard component. Despite the symbol name, this component is supposed to be limited to beard.
		/// </summary>
		Head,
		Hair,
		/// <summary>
		/// The upper body component.
		/// </summary>
		Torso,
		/// <summary>
		/// The lower body component.
		/// </summary>
		Legs,
		Hands,
		/// <summary>
		/// The feet component.
		/// </summary>
		Shoes,
		/// <summary>
		/// The "teeth" component, which is not used for teeth geometry in practice and instead used as an accessory
		/// component instead.
		/// </summary>
		Special1,
		Special2,
		Special3,
		/// <summary>
		/// The decal component.
		/// </summary>
		Textures,
		/// <summary>
		/// The JIJB component.
		/// </summary>
		Torso2
	}
}
