# 🔫 WeaponDrop for Grand Theft Auto V

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Language](https://img.shields.io/badge/language-C%23-blue.svg)

Welcome to WeaponDrop, a C# mod for Grand Theft Auto V that allows players to spawn collectible weapon crates. Drop a crate, pick it up, and get a random weapon!

This mod is built for story mode using the ScriptHookVDotNet3 API.

---

## 📑 Table of Contents

- [Features](#-features)
- [Requirements](#-requirements)
- [Installation (for Players)](#-installation-for-players)
- [How to Use](#-how-to-use)
- [Configuration](#-configuration)
- [For Developers](#-for-developers)
  - [Building from Source](#building-from-source)
- [Contributing](#-contributing)
- [License](#-license)

---

## ✨ Features

*   **📦 Spawn Crates:** Press a hotkey to drop a weapon crate right in front of you.
*   **🎯 One at a Time:** Only one crate can exist at a time to keep things clean.
*   **🏃‍♂️ Proximity-Based:** The crate will disappear if you move too far away (10 meters).
*   **🚫 Smart Spawning:** You can't spawn a crate while driving, flying, falling, or swimming.
*   **🔫 Random Weapons:** Walk up to the crate to receive a random weapon with a random amount of ammo.
*   **🔧 Fully Customizable:** Change the crate model, weapon list, and ammo range via the `.ini` file.

---

## 🧩 Requirements

To use this mod, you'll need:

*   Grand Theft Auto V (Story Mode)
*   [Script Hook V](http://www.dev-c.com/gtav/scripthookv/)
*   [ScriptHookVDotNet](https://github.com/crosire/scripthookvdotnet/releases) (v3.x)

---

## 🚀 Installation (for Players)

1.  Make sure you have all the [requirements](#-requirements) installed.
2.  Download the latest release from the project's releases page.
3.  Copy `WeaponDrop.dll` and `WeaponCrateDrop.ini` into your `scripts/` folder inside your Grand Theft Auto V installation directory. If the `scripts` folder doesn't exist, create it.

Your `scripts` folder should look something like this:

```
/Grand Theft Auto V
|-- /scripts
|   |-- WeaponDrop.dll
|   |-- WeaponCrateDrop.ini
|   |-- ... (other scripts)
|-- gta5.exe
|-- ... (other files)
```

---

## 🎮 How to Use

1.  Launch Grand Theft Auto V (Story Mode).
2.  Press the configured hotkey (**default: F6**) to drop a weapon crate.
3.  Walk up to the crate to pick it up and receive your new weapon!

---

## ⚙️ Configuration

You can customize the mod by editing the `WeaponCrateDrop.ini` file.

*   Change the spawn hotkey.
*   Define the list of possible weapons.
*   Set the minimum and maximum ammo for each weapon.
*   Change the model of the crate prop.

---

## 👨‍💻 For Developers

Interested in contributing or modifying the mod? Here's how to get started.

### Building from Source

**Requirements:**

*   Microsoft Visual Studio (2019 or newer recommended)
*   .NET Framework 4.8 Developer Pack
*   [ScriptHookVDotNet 3](https://github.com/crosire/scripthookvdotnet) SDK

**Steps:**

1.  **Clone the Repository:**
    ```bash
    git clone <your-repo-url>
    cd WeaponDrop
    ```
2.  **Open in Visual Studio:**
    Open the `WeaponDrop.csproj` file in Visual Studio.
3.  **Restore Dependencies:**
    The project requires a reference to `ScriptHookVDotNet3.dll`. If Visual Studio can't find it, you'll need to add it manually:
    *   Right-click `References` in the Solution Explorer.
    *   Select `Add Reference...`.
    *   Browse to the location of your `ScriptHookVDotNet3.dll` and add it.
4.  **Build the Project:**
    Use `Build > Build Solution` (or `Ctrl+Shift+B`) to compile the project. The output will be in `bin/Debug/WeaponDrop.dll`.

---

## 🤝 Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".

1.  Fork the Project
2.  Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3.  Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4.  Push to the Branch (`git push origin feature/AmazingFeature`)
5.  Open a Pull Request

Please ensure your code follows the existing style and is well-commented.

---

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for more information. 
