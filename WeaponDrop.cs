using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;
using GTA.Math;
using System.Windows.Forms;

public class WeaponDrop : Script
{
    private Keys dropKey = Keys.F6;
    private bool enableNotifications = true;
    private float pickupDistance = 2.5f;
    private float maxDistance = 10.0f; // New: Despawn range
    private string crateModel = "prop_box_wood01a";
    private int minAmmo = 30;
    private int maxAmmo = 150;
    private int maxCrates = 1;

    private Prop currentCrate;
    private int crateSpawnTime = 0;
    private List<WeaponHash> weaponList = new List<WeaponHash>();
    private Random rng = new Random();

    public WeaponDrop()
    {
        KeyDown += OnKeyDown;
        Tick += OnTick;
        LoadSettings();
    }

    private void LoadSettings()
    {
        string iniPath = "scripts/WeaponCrateDrop.ini";
        ScriptSettings settings = ScriptSettings.Load(iniPath);

        dropKey = ParseKey(settings.GetValue("General", "DropKey", "F6"));
        enableNotifications = settings.GetValue("General", "EnableNotifications", true);
        crateModel = settings.GetValue("General", "CrateModel", "prop_box_wood01a");
        pickupDistance = settings.GetValue("General", "PickupDistance", 2.5f);
        minAmmo = settings.GetValue("General", "MinAmmo", 30);
        maxAmmo = settings.GetValue("General", "MaxAmmo", 150);
        maxCrates = settings.GetValue("General", "MaxCrates", 1);
        maxDistance = settings.GetValue("General", "MaxDistance", 10.0f); // Optional in INI

        for (int i = 1; i <= 20; i++)
        {
            string key = "Weapon" + i;
            string value = settings.GetValue("Weapons", key, "");

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    WeaponHash weapon = (WeaponHash)Enum.Parse(typeof(WeaponHash), value, true);
                    weaponList.Add(weapon);
                }
                catch
                {
                    GTA.UI.Notification.Show("~y~Invalid weapon in INI: " + value);
                }
            }
        }

        if (weaponList.Count == 0)
        {
            weaponList.Add(WeaponHash.SMG);
            weaponList.Add(WeaponHash.CarbineRifle);
            weaponList.Add(WeaponHash.CombatPistol);
            weaponList.Add(WeaponHash.Knife);
        }
    }

    private Keys ParseKey(string keyName)
    {
        try { return (Keys)Enum.Parse(typeof(Keys), keyName, true); }
        catch { return Keys.F6; }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != dropKey)
            return;

        Ped player = Game.Player.Character;

        // Restriction checks
        if (player.IsInVehicle())
        {
            Vehicle vehicle = player.CurrentVehicle;
            if (vehicle.Model.IsHelicopter || vehicle.Model.IsPlane)
            {
                ShowWarning("flying");
                return;
            }
            else if (vehicle.Model.IsBoat)
            {
                ShowWarning("boating");
                return;
            }
            else
            {
                ShowWarning("driving");
                return;
            }
        }

        if (player.IsFalling)
        {
            ShowWarning("falling");
            return;
        }

        if (player.IsSwimming || player.IsInWater)
        {
            ShowWarning("swimming");
            return;
        }

        if (currentCrate != null)
        {
            if (enableNotifications)
                GTA.UI.Notification.Show("~r~Only 1 crate is allowed.");
            return;
        }

        Vector3 spawnPos = player.Position + player.ForwardVector * 3f;

        Model model = new Model(crateModel);
        if (!model.IsInCdImage || !model.IsValid)
        {
            if (enableNotifications)
                GTA.UI.Notification.Show("~r~Invalid crate model: " + crateModel);
            return;
        }

        model.Request();
        int timeout = Game.GameTime + 3000;
        while (!model.IsLoaded && Game.GameTime < timeout)
        {
            Script.Yield();
        }

        if (!model.IsLoaded)
        {
            if (enableNotifications)
                GTA.UI.Notification.Show("~r~Crate model failed to load.");
            return;
        }

        currentCrate = World.CreateProp(model, spawnPos, Vector3.Zero, true, true);
        model.MarkAsNoLongerNeeded();
        crateSpawnTime = Game.GameTime;

        if (enableNotifications)
            GTA.UI.Notification.Show("~b~Weapon Crate Dropped.");
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (currentCrate == null)
            return;

        float dist = Game.Player.Character.Position.DistanceTo(currentCrate.Position);

        // Despawn if player is too far
        if (dist > maxDistance)
        {
            if (enableNotifications)
                GTA.UI.Notification.Show("~o~Crate despawned: too far away.");

            currentCrate.Delete();
            currentCrate = null;
            return;
        }

        // Pickup
        if (Game.GameTime - crateSpawnTime > 1000 && dist < pickupDistance)
        {
            WeaponHash selected = weaponList[rng.Next(weaponList.Count)];
            int ammoAmount = rng.Next(minAmmo, maxAmmo + 1);

            Weapon weapon = Game.Player.Character.Weapons.Give(selected, 0, true, true);

            if (weapon != null)
            {
                weapon.Ammo += ammoAmount;
                weapon.AmmoInClip = Math.Min(weapon.MaxAmmoInClip, ammoAmount);

                if (enableNotifications)
                    GTA.UI.Notification.Show(string.Format("~g~Picked up: {0} ~s~(+{1} ammo)", selected, ammoAmount));
            }
            else
            {
                if (enableNotifications)
                    GTA.UI.Notification.Show("~r~Error giving weapon: " + selected.ToString());
            }

            currentCrate.Delete();
            currentCrate = null;
        }
    }

    private void ShowWarning(string reason)
    {
        if (enableNotifications)
            GTA.UI.Notification.Show("~r~Can't drop crate while " + reason + ".");
    }
}
