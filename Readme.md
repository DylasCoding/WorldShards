# World Shards

![Unity](https://img.shields.io/badge/Unity-2022.3.35f1-black?style=flat-square&logo=unity)
![C#](https://img.shields.io/badge/C%23-.NET-black?style=flat-square&logo=c-sharp)
![Android](https://img.shields.io/badge/Android-API%2021+-black?style=flat-square&logo=android)

A turn-based RPG developed in Unity for Android, featuring strategic combat with elemental and class-based mechanics.

## Overview

World Shards is a mobile RPG that implements a turn-based combat system with character progression and gacha mechanics. The game uses Unity Cloud Services for authentication and persistent data storage across devices.

## Core Systems

### Combat System
- **Turn-based mechanics** with action queue and elemental advantage resolution
- **Class-based interactions** - Warriors, Mages, Archers, Rogues, Healers, Tanks, Monks with defined matchups
- **Elemental system** - Wood, Water, Fire, Neutral elements affecting damage calculations and ability effects
- **Skill mechanics** - Each character has 3 unique skills with individual cooldowns and effects

### Character Management
- Hero roster with level progression and stat customization
- Gacha summoning system for acquiring new characters
- Team composition with 5-character lineup
- Persistent character state via cloud save

### Progression
- Campaign-based story with progressive difficulty
- Character leveling and stat enhancement
- Team synergy balancing (offense/defense/support roles)

## Technical Implementation

| Component | Details |
|-----------|---------|
| **Game Engine** | Unity 2022.3.35f1 |
| **Language** | C# |
| **Backend** | Unity Cloud Services (authentication + save data) |
| **Platform** | Android (API 21+) |
| **UI** | Mobile-optimized touch controls |
| **Audio** | Adaptive soundtrack with spatial sound effects |

### Key Features Implemented
- Cloud authentication with persistent player identity
- Cross-device save synchronization
- Performance optimization for mobile devices
- Dynamic audio system adapted to gameplay state
- Responsive mobile UI

## Screenshots

| Battle System | Team Lineup |
|:---:|:---:|
| ![Battle Scene](docs/BattleScene.png) | ![Line Up Scene](docs/LineUpScene.png) |
| Turn-based combat with elemental effects | Strategic team composition interface |

## Demo

[![Watch Gameplay Demo](https://img.shields.io/badge/YouTube-FF0000?style=flat-square&logo=youtube&logoColor=white)](https://youtu.be/c0y-Zopl5LU)

## Setup

### Requirements
- Unity 2022.3.35f1+
- Android SDK (API 21+)
- C# 9.0+

### Installation
```bash
git clone https://github.com/DylasCoding/WorldShards.git
```

Open the project in Unity and build for Android.

### Build
```bash
APK available at
https://drive.google.com/drive/folders/1yEmvUFmDVFLH-hOxPSviUjv1IoZnIZHb?usp=sharing
```

## Links

- [GitHub Repository](https://github.com/DylasCoding/WorldShards)
- [Issue Tracker](https://github.com/DylasCoding/WorldShards/issues)
- [Demo Video](https://www.youtube.com/watch?v=XgiOgB1fSRA)
