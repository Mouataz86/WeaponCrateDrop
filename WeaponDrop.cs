using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;
using GTA.Math;
using System.Windows.Forms;
using iFruitAddon2;
using LemonUI;
using LemonUI.Menus;
public class WeaponDrop : Script
{
    private Keys dropKey = Keys.None;
    private bool enableNotifications = true;
    private float pickupDistance = 2.5f;
    private float maxDistance = 1000.0f; // New: Despawn range
    private string crateModel = "prop_box_wood01a";
    private int minAmmo = 30;
    private int maxAmmo = 150;
    private Prop currentCrate;
    private int crateSpawnTime = 0;
    private List<WeaponHash> weaponListT1 = new List<WeaponHash>();
    private List<WeaponHash> weaponListT2 = new List<WeaponHash>();
    private List<WeaponHash> weaponListT3 = new List<WeaponHash>();
    private Random rng = new Random();
    int ammoAmount;
    WeaponHash selected;
    int currentCrateTier = 0;
    private static readonly CustomiFruit phonememe = new CustomiFruit();
    ObjectPool pool;
    NativeItem T1CrateOption;
    NativeItem T2CrateOption;
    NativeItem T3CrateOption;
    NativeMenu menu;
    iFruitContact CrateContact;
    public WeaponDrop()
    {
        SetupMenu();
        CreateContact();
        KeyDown += OnKeyDown;
        Tick += OnTick;
        LoadSettings();
    }
    void SetupMenu()
    {
        pool = new ObjectPool();
        menu = new NativeMenu("Crate Drop", "Select a Tier:");
        menu.UseMouse = false;
        menu.Closed += MenuClosed;
        pool.Add(menu);
        T1CrateOption = new NativeItem("Request Tier 1 Crate - $5,000", "Tier 1 weapon, small health and ammo boost");
        T1CrateOption.Activated += T1CrateOption_Activated;
        menu.Add(T1CrateOption);
        T2CrateOption = new NativeItem("Request Tier 2 Crate - $20,000", "Tier 2 weapon, Full health and ammo boost");
        T2CrateOption.Activated += T2CrateOption_Activated;
        menu.Add(T2CrateOption);
        T3CrateOption = new NativeItem("Request Tier 3 Crate - $50,000", "Tier 3 weapon, Full health, armor and ammo");
        T3CrateOption.Activated += T3CrateOption_Activated;
        menu.Add(T3CrateOption);
    }
    private void MenuClosed(object sender, EventArgs e)
    {
        CloseMenu();
    }
    void CloseMenu()
    {
        pool.HideAll();
        phonememe.Close(50);
    }
    private void T1CrateOption_Activated(object sender, EventArgs e)
    {
        if (Game.Player.Money >= 5000)
        {
            Game.Player.Money = Game.Player.Money - 5000;
            summonCrate(1);
            CloseMenu();
        }
        else
        {
            GTA.UI.Notification.Show("~y~Not enough Money");
        }
    }
    private void T2CrateOption_Activated(object sender, EventArgs e)
    {
        if (Game.Player.Money >= 20000)
        {
            Game.Player.Money = Game.Player.Money - 20000;
            summonCrate(2);
            CloseMenu();
        }
        else
        {
            GTA.UI.Notification.Show("~y~Not enough Money");
        }
    }
    private void T3CrateOption_Activated(object sender, EventArgs e)
    {
        if (Game.Player.Money >= 50000)
        {
            Game.Player.Money = Game.Player.Money - 50000;
            summonCrate(3);
            CloseMenu();
        }
        else
        {
            GTA.UI.Notification.Show("~y~Not enough Money");
        }
    }
    void CreateContact()
    {
        CrateContact = new iFruitContact("Agent 14");
        CrateContact.Answered += CrateContact_Answered;
        CrateContact.DialTimeout = 5000;
        CrateContact.Icon = new ContactIcon("CHAR_AGENT14");
        CrateContact.Active = true;
        phonememe.Contacts.Add(CrateContact);
    }
    private void CrateContact_Answered(iFruitContact contact)
    {
        if (currentCrate == null)
        {
            menu.Visible = true;
        }
        else
        {
            CloseMenu();
            GTA.UI.Notification.Show("~r~Only 1 crate is allowed.");
        }
    }
    bool safetyCheck()
    {
        Ped player = Game.Player.Character;
        // Restriction checks
        if (player.IsInVehicle())
        {
            Vehicle vehicle = player.CurrentVehicle;
            if (vehicle.Model.IsHelicopter || vehicle.Model.IsPlane)
            {
                ShowWarning("flying");
                return false;
            }
            else if (vehicle.Model.IsBoat)
            {
                ShowWarning("boating");
                return false;
            }
            else
            {
                ShowWarning("driving");
                return false;
            }
        }

        if (player.IsFalling)
        {
            ShowWarning("falling");
            return false;
        }

        if (player.IsSwimming || player.IsInWater)
        {
            ShowWarning("swimming");
            return false;
        }

        if (currentCrate != null)
        {
            if (enableNotifications)
                GTA.UI.Notification.Show("~r~Only 1 crate is allowed.");
            return false;
        }
        return true;
    }
    private void LoadSettings()
    {
        string iniPath = "scripts/WeaponCrateDrop.ini";
        ScriptSettings settings = ScriptSettings.Load(iniPath);
        dropKey = ParseKey(settings.GetValue("General", "DropKey", "F6"));
        enableNotifications = settings.GetValue("General", "EnableNotifications", true);
        crateModel = settings.GetValue("General", "CrateModel", "prop_drop_crate_01");
        pickupDistance = settings.GetValue("General", "PickupDistance", 2.5f);
        minAmmo = settings.GetValue("General", "MinAmmo", 30);
        for (int i = 1; i <= 20; i++)//TIER ONE
        {
            string key = "Weapon" + i;
            string value = settings.GetValue("WeaponsT1", key, "");
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    WeaponHash weapon = (WeaponHash)Enum.Parse(typeof(WeaponHash), value, true);
                    weaponListT1.Add(weapon);
                }
                catch
                {
                    GTA.UI.Notification.Show("~y~Invalid weapon in INI: " + value);
                }
            }
        }
        if (weaponListT1.Count == 0)
        {
            weaponListT1.Add(WeaponHash.MiniSMG);
            weaponListT1.Add(WeaponHash.APPistol);
            weaponListT1.Add(WeaponHash.CombatPistol);
            weaponListT1.Add(WeaponHash.MicroSMG);
        }
        for (int i = 1; i <= 20; i++)//TIER TWO
        {
            string key = "Weapon" + i;
            string value = settings.GetValue("WeaponsT2", key, "");
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    WeaponHash weapon = (WeaponHash)Enum.Parse(typeof(WeaponHash), value, true);
                    weaponListT2.Add(weapon);
                }
                catch
                {
                    GTA.UI.Notification.Show("~y~Invalid weapon in INI: " + value);
                }
            }
        }
        if (weaponListT2.Count == 0)
        {
            weaponListT2.Add(WeaponHash.SMG);
            weaponListT2.Add(WeaponHash.CompactRifle);
            weaponListT2.Add(WeaponHash.PumpShotgun);
            weaponListT2.Add(WeaponHash.CombatShotgun);
        }
        for (int i = 1; i <= 20; i++)//TIER THREE
        {
            string key = "Weapon" + i;
            string value = settings.GetValue("WeaponsT3", key, "");
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    WeaponHash weapon = (WeaponHash)Enum.Parse(typeof(WeaponHash), value, true);
                    weaponListT3.Add(weapon);
                }
                catch
                {
                    GTA.UI.Notification.Show("~y~Invalid weapon in INI: " + value);
                }
            }
        }
        if (weaponListT3.Count == 0)
        {
            weaponListT3.Add(WeaponHash.Minigun);
            weaponListT3.Add(WeaponHash.AssaultShotgun);
            weaponListT3.Add(WeaponHash.CombatMG);
            weaponListT3.Add(WeaponHash.RPG);
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
        summonCrate(3);
    }
    void summonCrate(int tier) { 

        Ped player = Game.Player.Character;
        if (!safetyCheck())//player is not clear to summon crate
        {
            return;
        }
        Vector3 spawnPos = new Vector3(player.Position.X, player.Position.Y, player.Position.Z + 300) + player.ForwardVector * (3f * rng.Next(5));
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
        currentCrate = World.CreateProp(model, spawnPos, Vector3.Zero, true, false);///ACTUALLY SPAWN A CRATE
        currentCrate.LodDistance = 1000;
        currentCrateTier = tier;
        model.MarkAsNoLongerNeeded();
        crateSpawnTime = Game.GameTime;
        Blip crateblip = Function.Call<Blip>(Hash.ADD_BLIP_FOR_ENTITY, currentCrate);
        crateblip.Sprite = BlipSprite.CrateDrop;
        crateblip.IsFlashing = true;
        crateblip.Color = BlipColor.Green;
        Function.Call(Hash.ACTIVATE_PHYSICS, currentCrate);
        if (enableNotifications)
            GTA.UI.Notification.Show("~b~Weapon Crate Dropped.");
    }
    void DestroyCrate()
    {
        currentCrate.Delete();
        currentCrate = null;
        currentCrateTier = 0;
        Function.Call(Hash.STOP_SOUND, 1);
    }
    void CrateDespawnCheck(float dist)
    {
        // Despawn if player is too far
        if (dist > maxDistance)
        {
            if (enableNotifications)
                GTA.UI.Notification.Show("~o~Crate despawned: too far away.");
            DestroyCrate();
            return;
        }
    }
    void CratePickupCheck(float dist)
    {
        // Pickup
        if (Game.GameTime - crateSpawnTime > 1000 && dist < pickupDistance)
        {
            switch (currentCrateTier)
            {
                case 1://TIER ONE
                    selected = weaponListT1[rng.Next(weaponListT1.Count)];
                    ammoAmount = rng.Next(minAmmo, maxAmmo + 1);
                    Game.Player.Character.Health += 50;
                    break;
                case 2://TIER TWO
                    selected = weaponListT2[rng.Next(weaponListT2.Count)];
                    ammoAmount = rng.Next(minAmmo, maxAmmo + 2000);
                    Game.Player.Character.Health += 200;
                    break;
                case 3://TIER THREE
                    selected = weaponListT3[rng.Next(weaponListT3.Count)];
                    ammoAmount = 9999;
                    Game.Player.Character.Health += 200;
                    Game.Player.Character.Armor += 200;
                    break;
            }
            Weapon weapon = Game.Player.Character.Weapons.Give(selected, 0, true, true);
            Function.Call(Hash.PLAY_SOUND, -1, "PICKUP_DEFAULT", "HUD_FRONTEND_STANDARD_PICKUPS_SOUNDSET");
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
            DestroyCrate();
        }
    }
    private void OnTick(object sender, EventArgs e)
    {
        phonememe.Update();
        pool.Process();
        if (currentCrate != null)//If a crate is active
        {
            float dist = Game.Player.Character.Position.DistanceTo(currentCrate.Position);
            CrateDespawnCheck(dist);
            CratePickupCheck(dist);
            CrateBeep();
        }
    }
    void CrateBeep()
    {
        Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, 1, "Crate_Beeps", currentCrate, "MP_CRATE_DROP_SOUNDS",0,0);
        Wait(3000);
    }
    private void ShowWarning(string reason)
    {
        if (enableNotifications)
            GTA.UI.Notification.Show("~r~Can't drop crate while " + reason + ".");
    }
}