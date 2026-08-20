# AR-APP

An interactive Augmented Reality musical instrument application built with Unity. The project provides virtual instruments that users can interact with through an intuitive interface.

## Features

- Interactive 3D Piano
- Interactive Drum Kit
- Piano keys with individual sounds
- Multiple drum pads and sounds
- Instrument selection
- Volume and interaction sensitivity controls
- Interactive UI
- Unity XR/AR support

## Tech Stack

- **Unity:** 6000.4.10f1
- **Language:** C#
- **Input:** Unity Input System
- **XR:** Unity XR
- **Audio:** Unity Audio System
- **Version Control:** Git + Git LFS

## Getting Started

### Requirements

- Unity **6000.4.10f1**
- Unity Hub
- Git
- Git LFS
- AR/XR-compatible device for AR features

### Installation

Clone the repository:

```bash
git clone https://github.com/JayantaKundu17/AR-APP.git
cd AR-APP

Install and download Git LFS assets:

git lfs install
git lfs pull

Open the project in Unity 6000.4.10f1 through Unity Hub.

Open the main scene:

Assets/Scenes/Final.unity

Press Play to run the application.

Using the Application
Main Menu
Play — Start the application
Instrument Selection — Choose an instrument
Settings — Adjust volume and interaction sensitivity
Exit — Exit the application
Piano

Select Piano and interact with individual keys to play different notes.

Drum Kit

Select Drum and interact with the available drum pads to produce different sounds.

Project Structure
Assets/
├── Audio/          # Instrument and UI sounds
├── Models/         # Piano and drum models
├── Scenes/         # Unity scenes
├── Scripts/        # C# scripts
└── Settings/       # Project settings/assets


Packages/            # Unity packages
ProjectSettings/     # Unity project configuration
Git LFS

Git LFS is used for large assets such as:

*.fbx
*.mp3
*.wav
*.png
*.jpg
*.jpeg

If assets are missing after cloning, run:

git lfs pull

The Unity Library, Temp, Logs, and other generated files are intentionally excluded from the repository.

Author

Jayanta Kundu

GitHub: https://github.com/JayantaKundu17
