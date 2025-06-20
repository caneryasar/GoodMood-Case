# ⚔️ Good Mood Games – Combat System Prototype

This is a third-person action game prototype developed in Unity for a technical case study as part of a Junior–Mid Level Game Developer position application at Good Mood Games.

The project showcases a fluid player controller, dynamic combat mechanics, a responsive camera system, and interaction with a training dummy enemy—designed to highlight technical proficiency, game feel, and integration of multiple systems.

---

## 🎮 Core Features

### ✅ Third-Person Character Controller
- **Movement**: WASD controls using `CharacterController`, rotates with movement direction
- **Camera**: Cinemachine Free Look + Lock-On system (TAB)
- **Rotation**: Character aligns with movement input
- **Reference**: Movement inspired by *The Witcher 3*

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
| **Version Control** | Git + GitHub                              |

---

## 🚀 How to Run the Project

1. Clone or download the repository
2. Open Unity Hub and click Add to select the project folder
3. Use Unity version 2022.3.15f1 or compatible LTS version
4. Open the main scene under Assets/Scenes/
5. Press ▶️ Play in the editor
