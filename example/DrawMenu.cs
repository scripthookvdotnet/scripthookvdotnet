using GTA;
using System;
using System.Drawing;
using System.Windows.Forms;

public class DrawMenu : Script
{
	public DrawMenu()
	{
		Tick += OnTick;

		this.mContainer = new UIContainer(new Point(10, 240), new Size(200, 300), Color.FromArgb(200, 237, 239, 241));
		this.mContainer.Items.Add(new UIRectangle(new Point(0, 0), new Size(200, 30), Color.FromArgb(255, 26, 188, 156)));
		this.mContainer.Items.Add(new UIText("Example Title", new Point(100, 4), 0.5f, Color.WhiteSmoke, GTA.Font.ChaletComprimeCologne, true));
		this.mContainer.Items.Add(new UIRectangle(new Point(0, 30), new Size(200, 30), Color.FromArgb(135, 26, 187, 155)));
		this.mContainer.Items.Add(new UIText("Example", new Point(100, 34), 0.4f, Color.Black, 0, true));
	}

	private UIContainer mContainer = null;

	void OnTick(object sender, EventArgs e)
	{
		this.mContainer.Draw();
	}
}