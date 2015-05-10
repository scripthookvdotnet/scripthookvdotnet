using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;

public class MenuScripts : MenuScript
{
    GTA.Menu MyReallyCoolMenu;

    public MenuScripts()
    {
        //Use some fancy transitions
        View.MenuTransitions = true;
        //Instantiate our menu
        MyReallyCoolMenu = new GTA.Menu("Really cool menu", new GTA.MenuItem[] {
                new MenuButton("Quantize foobars", "Quantizes the foobars", () => {
                    View.AddMenu(new GTA.Menu("Next Menu", new GTA.MenuItem[] {
                        new MenuButton("Literally nothing", () => {
                            View.AddMenu(new GTA.MessageBox("Yes or no?", () => { }, () => { }));
                        })
                    }));
                }),
                new MenuButton("Frobnicate biscuits", "Frobnicates at least \n2 biscuits", () => {}),
                new MenuButton("Something", "I don't know", () => {}),
                new MenuButton("Something else", () => {})
            });

        KeyDown += OnKeyDown;
    }

    void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F8)
            View.AddMenu(MyReallyCoolMenu);
    }
}