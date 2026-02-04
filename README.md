# Global Game Jam 2026 | Can Hue See It? (Co-op multiplayer game)

**Global Game Jam 2026 – Immersive Sweden**
**Theme: Masks**

## 🎭 Game Description & How to Play

A **cooperative puzzle game built around communication and trust**.

Each player exists in a different version of the same level.
The twist? **The blocks you see can only be interacted with by your partner — and vice versa.**

To escape each level, players must:

* Describe what they see
* Guide their partner’s actions
* Coordinate movement and timing across grid-based puzzle rooms

If you don’t communicate, you don’t escape.

### Controls

* **WASD** – Move
* **Space** – Jump

---

## 🧩 General Architecture

The project follows a lightweight, manager-driven architecture suitable for rapid iteration during a game jam.

| Class Name             | Responsibility                                                                                    |
| ---------------------- | ------------------------------------------------------------------------------------------------- |
| **Singleton<T>**       | Generic base class enforcing a single instance pattern for global managers.                       |
| **GameEventManager**   | Centralized event system used to broadcast and listen for gameplay events without tight coupling. |
| **GameState**          | Holds and manages the current game state (e.g. menu, playing, win, lose).                         |
| **ConnectionManager**  | Manages player connections, session state, and connection lifecycle.                              |
| **SceneLoaderManager** | Responsible for loading, unloading, and transitioning between scenes.                             |
| **InputManager**       | Abstracts player input and maps raw input to gameplay actions.                                    |

---

## 🛠 Dependencies

* **Unity** (Game Engine)
* Unity Networking solution (`NetworkManager`)
* New Unity Input System
