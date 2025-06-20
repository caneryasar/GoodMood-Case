# ⚔️ Good Mood Games – Combat System Prototype

This is a third-person action game prototype developed in Unity for a technical case study as part of a Junior–Mid Level Game Developer position application at Good Mood Games.

The project showcases a fluid player controller, dynamic combat mechanics, a responsive camera system, and interaction with a training dummy enemy—designed to highlight technical proficiency, game feel, and integration of multiple systems.

---

## 🎮 Core Features

### ✅ Third-Person Character Controller
- **Movement**: WASD controls using `CharacterController`, rotates with movement direction
- **Camera**: Cinemachine Free Look + Lock-On system (TAB)
- **Rotation**: Character aligns with movement input

### ✅ Combat System
- **Attack**: Left mouse button performs sword strikes
- **Combo System**: Time-based chaining, increasing damage with each hit
- **Final Hit**: Last combo attack deals significantly more damage
- **Animations**: Smooth transitions between attack stages

### ✅ Training Dummy System
- **Stationary dummy** with:
  - **Hit reactions**
  - **Health system** with visual feedback
  - **Death animation**
  - **Automatic respawn after 5 seconds**

### ✅ UI Integration
- **Health bar** displaying dummy's current health
- **Damage pop-ups**
- **Real-time updates**

---

## 🎨 Bonus Features
- Camera shake on hit
- Visual effects: particles, weapon trails
- Audio: attack and hit SFX
- Polished animations and effects for better game feel

---

## 🛠️ Tech Stack

| Tool                | Version / Usage                           |
|---------------------|-------------------------------------------|
| **Unity**           | Unity 6 or later                          |
| **Render Pipeline** | HDRP (High Definition Render Pipeline)    |
| **Language**        | C#                                        |
| **Input System**    | Unity's New Input System                  |
| **Camera**          | Cinemachine                               |
| **Additional Packages** | UniRx, DOTween                        |
| **Version Control** | Git + GitHub                              |
| **Environment**     | Rider                                     |

---

## 🚀 How to Run the Project

1. Clone or download the repository
2. Open Unity Hub and click Add to select the project folder
3. Use Unity version 6 or newer
4. Open the main scene under Assets/Scenes/
5. Press ▶️ Play in the editor
6. Additionally you can play it from the ".exe" in Builds/ folder

---

## 🖥️ References and Credits

### VFX

- [Sword Slash](https://youtu.be/c8hijUge7IY?si=VoFZpGJ8HFqEBjCj) by Gabrial Aguiar Prod.
- [Impact Effet](https://youtu.be/jSIan1cEYTI?si=Be8ETriYpnehJEUg) by Gabriel Aguiar Prod.
- [Dissolve Effect](https://youtu.be/we406Hc_WrM?si=dyf8xlJTg_1Z3lc_) by Gabriel Aguiar Prod.


### Assets

- [Free Low Poly Modular Character Pack – Fantasy Dream](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/free-low-poly-modular-character-pack-fantasy-dream-321521**) by GanzSe
- [Human Melee Animations FREE](https://assetstore.unity.com/packages/3d/animations/human-melee-animations-free-165785) by Kevin Iglesias
- [Simple UI Elemens](https://assetstore.unity.com/packages/2d/gui/icons/simple-ui-elements-53276) by MadFireOn
- [Free Low Poly Nature Forest](https://assetstore.unity.com/packages/3d/environments/landscapes/free-low-poly-nature-forest-205742) by Pure Poly
- [Fantasy Skybox Free](https://assetstore.unity.com/packages/2d/textures-materials/sky/fantasy-skybox-free-18353) by Render Knight
- [Keyboard Icons Pack](https://defacegames.com/product/keyboard-mouse-icon-pack-free/) by Deface Games

- [Enchanted Land](https://www.dafont.com/enchanted-land.font) and [Medieval Sharp](https://www.dafont.com/medieval-sharp.font) Fonts
- [Walk](https://www.zapsplat.com/music/large-leather-sneakers-trainers-single-footstep-on-hard-ground-road-or-concrete-right-foot-7/), [Hit1](https://freesound.org/people/CosmicEmbers/sounds/387480/), [Hit2](https://freesound.org/people/magnuswaker/sounds/530117/), [Hit3](https://freesound.org/people/MinecraftGamerLR/sounds/728696/), [Attack1](https://freesound.org/people/WelvynZPorterSamples/sounds/621357/), [Attack2](https://freesound.org/people/Squirrel_404/sounds/737748/), [Attack3](https://freesound.org/people/qubodup/sounds/59992/) and [Death](https://freesound.org/people/Under7dude/sounds/163442/) SFX.
