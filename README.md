WeaponDrop (Developer Documentation)

Welcome to the developer documentation for WeaponDrop, a C# mod for Grand Theft Auto V that allows players to spawn collectible weapon crates.

This guide is intended for developers who wish to contribute to the project, or fork it for their own modifications. For end-user installation, please see the README.txt in the root directory.

📑 Table of Contents

Project Overview

Requirements

Setup and Building

Usage In-Game

Contributing

License

📦 Project Overview

WeaponDrop is built using the ScriptHookVDotNet3 (SHVDN3) API wrapper. It allows players to press a hotkey (default: F6) to drop a prop_box_wood05a crate in front of them. If the player approaches the crate, it is deleted and a random weapon is added to their inventory with a randomized amount of ammo.

Language: C#Framework: .NET Framework 4.8Core API: ScriptHookVDotNet 3

🔧 Features Implemented

Crate spawns in front of player using customizable model (default: prop_box_wood05a)

One crate at a time enforcement

Crate disappears if the player moves too far (10 meters)

Player cannot spawn crate if:

Driving

Flying

Falling

Swimming or boating

Customizable ammo range and weapon list via INI

Crate pickup provides weapon + ammo

🧩 Requirements

To build and develop this mod, you will need:

Microsoft Visual Studio (2019 or newer recommended)

.NET Framework 4.8 Developer Pack

ScriptHookVDotNet 3

Download SHVDN: https://github.com/crosire/scripthookvdotnet

⚙️ Setup and Building

1. Clone the Repository

git clone <your-repo-url>
cd WeaponDrop

2. Open in Visual Studio

Open the WeaponDrop.csproj file in Visual Studio.

3. Restore Dependencies

Ensure the reference to ScriptHookVDotNet3.dll is correct:

Right-click References > Add Reference

Browse to your ScriptHookVDotNet3 DLL location

4. Build the Project

Use Build > Build Solution to compile.Output DLL: bin/Debug/WeaponDrop.dll

🎮 Usage In-Game

Copy WeaponDrop.dll and WeaponCrateDrop.ini to your GTA V scripts/ folder.

Launch GTA V (Story Mode only).

Press the configured key (default: F6) to drop a weapon crate.

Walk up to the crate to receive a randomized weapon and ammo.

🤝 Contributing

Contributions are welcome!

Fork this repository

Create a feature branch: git checkout -b feature/my-feature

Commit your changes: git commit -am 'Add feature'

Push your branch: git push origin feature/my-feature

Create a Pull Request

Ensure your code follows existing formatting and is clean and well-commented.

📄 License

This project is licensed under the MIT License.See the LICENSE file in the root directory for details.
