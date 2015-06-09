// This isn't a full fledged trainer
// It's just a trainer menu to show how to use the Menu API

#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using Menu = GTA.Menu;

#endregion

public class SimpleTrainer : Script
{
    public SimpleTrainer()
    {
        //Use some fancy transitions
        this.View.MenuTransitions = true;

        this.KeyDown += this.OnKeyDown;
    }

    private void OnKeyDown( object sender, KeyEventArgs e )
    {
        if ( e.KeyCode == Keys.F8 )
        {
            if ( this.View.ActiveMenus == 0 )
            {
                this.OpenTrainerMenu();
            }
        }
    }

    #region Menu

    private void OpenTrainerMenu()
    {
        var menuItems = new List<IMenuItem> { new MenuLabel( "Categories", true ) };

        var button = new MenuButton( "Player", "Opens the menu with \nplayer commands" );
        button.Activated += ( sender, args ) => this.OpenPlayerMenu();
        menuItems.Add( button );

        button = new MenuButton( "Weapons", "Opens the menu with \nweapon commands" );
        button.Activated += ( sender, args ) => this.OpenWeaponMenu();
        menuItems.Add( button );

        button = new MenuButton( "Spawn Vehicle", "Opens the menu with\nvehicles to spawn" );
        button.Activated += ( sender, args ) => this.OpenVehicleSpawnMenu();
        menuItems.Add( button );

        menuItems.Add( new MenuLabel( "Settings", true ) );

        button = new MenuButton( "Open Settings", "Opens the trainer \nsettings menu" );
        button.Activated += ( sender, args ) => this.OpenSettingsMenu();
        menuItems.Add( button );

        this.View.AddMenu( new Menu( "Simple Trainer", menuItems.ToArray() ) );
    }

    private void OpenPlayerMenu()
    {
        var menuItems = new List<IMenuItem>();

        var toggle = new MenuToggle( "Invincible", "Makes the player \ninvincible" );
        toggle.Changed += ( sender, args ) =>
        {
            var tg = sender as MenuToggle;
            if ( tg == null )
            {
                return;
            }
            if ( tg.Value )
            {
                this.ActivateInvincibility();
            }
            else
            {
                this.DeactivateInvincibility();
            }
        };
        menuItems.Add( toggle );

        var button = new MenuButton( "Heal fully", "Gives the player \n100% health" );
        button.Activated += ( sender, args ) => this.HealPlayer();
        menuItems.Add( button );

        toggle = new MenuToggle( "Never wanted", "Makes it so you can't \nget a wanted level" );
        toggle.Changed += ( sender, args ) =>
        {
            var tg = sender as MenuToggle;
            if ( tg == null )
            {
                return;
            }
            if ( tg.Value )
            {
                this.ActivateNeverWanted();
            }
            else
            {
                this.DeactivateNeverWanted();
            }
        };
        menuItems.Add( toggle );

        this.View.AddMenu( new Menu( "Player Menu", menuItems.ToArray() ) );
    }

    private void OpenWeaponMenu()
    {
        var menuItems = new List<IMenuItem>();

        var toggle = new MenuToggle( "Unlimited ammo", "You never run out \nof ammo" );
        toggle.Changed += ( sender, args ) =>
        {
            var tg = sender as MenuToggle;
            if ( tg == null )
            {
                return;
            }
            if ( tg.Value )
            {
                this.ActivateUnlimitedAmmo();
            }
            else
            {
                this.DeactivateUnlimitedAmmo();
            }
        };
        menuItems.Add( toggle );

        var button = new MenuButton( "Unlock all", "Unlocks all weapons" );
        button.Activated += ( sender, args ) => this.UnlockAllWeapons();
        menuItems.Add( button );

        this.View.AddMenu( new Menu( "Weapon Menu", menuItems.ToArray() ) );
    }

    private void OpenVehicleSpawnMenu()
    {
        // add first 4 vehicles names to Menu
        this.View.AddMenu( new Menu( "Spawn Vehicle",
            Enum.GetNames( typeof( VehicleHash ) ).Take( 4 ).Select( vehName => new MenuButton( vehName ) ).ToArray() ) );
    }

    private void OpenSettingsMenu()
    {
    }

    private void UnlockAllWeapons()
    {
        var msgBox = new GTA.MessageBox( "Are you sure you want to \nunlock all weapons?" );
        msgBox.Yes += ( sender, args ) => this.DoUnlockAllWeapons();
        this.View.AddMenu( msgBox );
    }

    #endregion

    #region Commands

    private void ActivateInvincibility()
    {
    }

    private void DeactivateInvincibility()
    {
    }

    private void HealPlayer()
    {
    }

    private void ActivateNeverWanted()
    {
    }

    private void DeactivateNeverWanted()
    {
    }

    private void ActivateUnlimitedAmmo()
    {
    }

    private void DeactivateUnlimitedAmmo()
    {
    }

    private void DoUnlockAllWeapons()
    {
    }

    #endregion
}