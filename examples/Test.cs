using System;
using System.Windows.Forms;

public class TestScript : GTA.Script
{
	public TestScript()
	{
		KeyDown += OnKeyDown;
	}

	void OnKeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			GTA.Test.Func();
		}
	}
}
