using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;

public class MenuScripts : Script
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
                new MenuToggle("Webscale", "Cool things", () => {}, () => {}),
                new MenuNumericScroller("Scroller", "Scrolls", (d) => {}, (d) => {}, -10, 10, 0.5),
                new MenuEnumScroller("Difficulty", "The difficulty of \n the game", (i) => {}, (i) => {}, new string[] {"Easy", "Normal", "Hard", "Demon"})
            });

        KeyDown += OnKeyDown;
    }

    void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F8)
            View.AddMenu(MyReallyCoolMenu);
    }
}