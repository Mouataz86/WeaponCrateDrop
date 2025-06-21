# Weapon Crate Drop Mod for GTA V Story Mode

📦 **Version**: 1.0  
🛠 **Author**: MO3IZO  
🌐 **GitHub**: [https://github.com/Mouataz86/WeaponCrateDrop](https://github.com/Mouataz86/WeaponCrateDrop)

> ⚠️ **DO NOT USE THIS MOD ONLINE. OFFLINE STORY MODE ONLY. Using in GTA Online can result in bans.**

---

## 📋 What is This Mod?

`WeaponDrop` is a lightweight mod for GTA V Story Mode that allows you to drop a weapon crate with a random weapon and ammo. Ideal for quick gear-ups or roleplaying. You can customize the crate model, weapon list, ammo range, and pickup key through the `WeaponCrateDrop.ini` file.

---

## 🧩 Requirements

- GTA V (PC)
- [ScriptHookV by Alexander Blade](http://www.dev-c.com/gtav/scripthookv/)
- [ScriptHookVDotNet v3.7.0+ (Nightly Recommended)](https://github.com/scripthookvdotnet/scripthookvdotnet-nightly/releases)

---

## 📁 Installation (User Version)

1. Create a `scripts` folder in your GTA V directory if it doesn’t exist.
2. Drop these files into the `scripts` folder:
   - `WeaponDrop.dll`
   - `WeaponCrateDrop.ini`
3. Launch GTA V (Story Mode).
4. Press **F6** (or your configured key) to drop a weapon crate.

---

## ⚙ Configuration (`WeaponCrateDrop.ini`)

```ini
[General]
DropKey=F6
EnableNotifications=true
CrateModel=prop_box_wood05a
PickupDistance=2.5
MinAmmo=30
MaxAmmo=150
MaxCrates=1

[Weapons]
Weapon1=SMG
Weapon2=CarbineRifle
Weapon3=CombatPistol
Weapon4=Knife
```

---

## ✨ Features

- Drop 1 weapon crate at a time.
- Crate disappears if you go too far.
- Cannot spawn while:
  - Driving
  - Flying
  - On boat
  - Falling
- Weapon + Ammo randomization.
- Fully configurable.

---

## 📃 License

MIT License – see `LICENSE` file.

---

## 🙌 Credits

- Developed by **MO3IZO**
- Powered by ScriptHookVDotNet
