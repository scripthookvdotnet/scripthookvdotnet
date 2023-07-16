//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA.Graphics
{
	public enum ScriptedGfxDrawOrder
	{
		BeforeHudPriorityLow,
		BeforeHud,
		BeforeHudPriorityHigh,
		AfterHudPriorityLow,
		/// <summary>
		/// The default value.
		/// When you draw scripted graphics such as texts, rects, or textures on a render target (texture),
		/// you need to specify this value.
		/// </summary>
		AfterHud,
		AfterHudPriorityHigh,
		AfterFadePriorityLow,
		AfterFade,
		AfterFadePriorityHigh,
	}
}
