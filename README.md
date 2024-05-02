# ☢ Atomic Atlas ☢

## 🏛 ABOUT 🏛

Atomic Atlas is an open-source map generator for Dominions 6 that is designed to address the shortcomings of other map generators.

## 🏗 IMPROVEMENTS 🏗

* A simplified, minimal interface that takes up less screen space. The settings panel will house several tabs for all of the relevant data users need to change.
* Support for 1-9 planes.
* Nodes can be manually added, moved around, and deleted.
* Connections can be manually added and deleted.
* Runtime validation for maps to ensure bad data isn't being generated.
* Atomic Atlas uses dependency injection to keep systems decoupled properly with clear interfaces. The app uses a state machine to simplify its execution flow and there is a generic data loading system that it uses for handling nation data, strategy data, procedural province naming data, and any other serializable data.
* Modular software design means new custom map generation algorithms will be easy to add and use.
* Dominions 6 introduced a new map format: **d6m**. This allows the game to handle the rendering of map art, all the map generator has to do is output the province information in the d6m file.

## 🗺 ROADMAP 🗺

Work is underway on the map generator and several features are complete. Here is a mostly un-ordered list of features that need to be completed. Once all MVP features are complete, I can release a 1.0 version of this map generator.

### 🏅 Features For MVP 🏅

- [x] Core architecture: Data loading
- [x] Core architecture: Control inversion
- [x] Core architecture: App state machine
- [x] Core architecture: Manager classes
- [x] Core architecture: Modular world generator system
- [x] UI: Main HUD
- [x] UI: Context menu for manual editor tools
- [ ] UI: Game settings menu
- [ ] UI: Generator settings menu
- [ ] Modular interface for configuring map generation
- [x] Camera controls (zoom, pan)
- [x] Interactive node and connection generation
- [ ] A simple default map generation strategy that is configurable
- [x] Basic map editor tooling - Ability to modify nodes
- [x] Basic map editor tooling - Ability to modify connections
- [ ] Province visualization with 2D generated meshes
- [ ] Visual representation of vertical and horizontal wrapped provinces
- [ ] Connection visualization with color coding
- [ ] .D6M file output
- [ ] .MAP file output

### 🏆 Advanced Features 🏆

- [ ] Runtime validation for manual editor tools to ensure users aren't creating bad data
- [ ] UI: Errors and warnings menu
