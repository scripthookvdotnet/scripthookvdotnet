﻿//This isn't a full fledged trainer
//It's just a trainer menu to show how to use the Menu API

using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;

public class SimpleTrainer : Script
{
    public SimpleTrainer()
    {
        //Use some fancy transitions
        View.MenuTransitions = true;

        KeyDown += OnKeyDown;
    }

    void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F8)
        {
            if (View.ActiveMenus == 0)
                OpenTrainerMenu();
        }
    }

    #region Menu

    void OpenTrainerMenu()
    {
        View.AddMenu(new GTA.Menu("Simple Trainer", new GTA.MenuItem[] {
            new MenuLabel("Categories", true),
            new MenuButton("Player", "Opens the menu with \nplayer commands", OpenPlayerMenu),
            new MenuButton("Weapons", "Opens the menu with \nweapon commands", OpenWeaponMenu),
            new MenuButton("Spawn Vehicle", "Opens the menu with\nvehicles to spawn", OpenVehicleSpawnMenu),
            new MenuLabel("Settings", true),
            new MenuButton("Open Settings", "Opens the trainer \nsettings menu", OpenSettingsMenu)
        }));
    }

    void OpenPlayerMenu()
    {
        View.AddMenu(new GTA.Menu("Player Menu", new GTA.MenuItem[] {
            new MenuToggle("Invincible", "Makes the player \ninvincible", ActivateInvincibility, DeactivateInvincibility),
            new MenuButton("Heal fully", "Gives the player \n100% health", HealPlayer),
            new MenuToggle("Never wanted", "Makes it so you can't \nget a wanted level", ActivateNeverWanted, DeactivateNeverWanted)
        }));
    }

    void OpenWeaponMenu()
    {
        View.AddMenu(new GTA.Menu("Weapon Menu", new GTA.MenuItem[] {
            new MenuToggle("Unlimited ammo", "You never run out \nof ammo", ActivateUnlimitedAmmo, DeactivateUnlimitedAmmo),
            new MenuButton("Unlock all", "Unlocks all weapons", UnlockAllWeapons)
        }));
    }

    void OpenVehicleSpawnMenu()
    {
        ListMenu VehicleMenu = new ListMenu("Spawn Vehicle");
        VehicleMenu.Add("Infernus", "A fast car");
        VehicleMenu.Add("LAZER", "A military jet");
        VehicleMenu.Add("BMX", "A bike");
        VehicleMenu.Add("Jetpack", "CLASSIFIED");
        View.AddMenu(VehicleMenu);
    }

    void OpenSettingsMenu()
    {
    }

    void UnlockAllWeapons()
    {
        View.AddMenu(new GTA.MessageBox("Are you sure you want to \nunlock all weapons?", DoUnlockAllWeapons, () => { }));
    }

    #endregion

    #region Commands

    void ActivateInvincibility()
    {
    }

    void DeactivateInvincibility()
    {
    }

    void HealPlayer()
    {
    }

    void ActivateNeverWanted()
    {
    }

    void DeactivateNeverWanted()
    {
    }

    void ActivateUnlimitedAmmo()
    {
    }

    void DeactivateUnlimitedAmmo()
    {
    }

    void DoUnlockAllWeapons()
    {
    }

    #endregion
}