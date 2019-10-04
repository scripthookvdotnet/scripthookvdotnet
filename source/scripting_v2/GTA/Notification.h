/**
 * Copyright (C) 2015 crosire & contributors
 * License: https://github.com/crosire/scripthookvdotnet#license
 */

#pragma once

namespace GTA
{
	public ref class Notification
	{
	public:
		void Hide();

	internal:
		Notification(int handle);

	private:
		int _handle;
	};
}
