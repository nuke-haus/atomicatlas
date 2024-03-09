# ☢ AtomicAtlas: An open-source map generator for Dominions 6 ☢

## 🏛 ABOUT 🏛

Atomic Atlas is a map generator for Dominions 6 that is designed to address the failings of the built-in map generator and MapNuke.

## 🤬 ISSUES WITH THE DOMINIONS 6 MAP GENERATOR 🤬

### UNFAIR STARTS

Some players start in strange positions or with bad caprings. Some examples:

* Nations with poor water access and no sailing being placed on islands in the middle of an ocean.
* Nations starting with thrones in their caprings and having 3 or less provinces available to them.
* Cave nations having few cave provinces available to them, in some cases starting on a single cave province that links to the surface.

### RANDOMNESS

This is a contentious topic since the degree of randomness in dominions maps is a matter of taste. Some people prefer more fairness in terms of capring size and distribution of thrones and valuable provinces, which is what MapNuke is tuned for. Some people prefer a high degree of randomness in their maps since it results in less samey maps and more interesting gameplay.

The problem with the indeterminism of the Dominions 6 map generator is that it's generating maps without any context regarding the nations that will play on them. Nation start positions are an afterthought and only set once the map is generated. This often results in poor outcomes.

## 😩 ISSUES WITH MAPNUKE 😩

### DETERMINISM

Players get tired of the maps being too predictable, where every player has the same amount of space and there's less interesting topography and randomness.

### INFLEXIBLE EDITOR TOOLS

Provinces and connections can be changed, but the actual map layout cannot be tweaked easily in MapNuke.

### ART GENERATION

This is mostly a development-specific issue. Dominions 6 has oppressive requirements for a complete map that uses its own art. Separate images must be output for each terrain type. The end result was a lot of development time spent tuning the art generation.

Generating art also means spending time doing spriting and tweaking visual output rather than working on the actual world generation algorithm.

## 🔄 A NEW DESIGN 🔄

### MODULARITY

With Atomic Atlas I want to lean more into a modular, extensible approach which allows the end user to have much more control over every aspect of the map generation process. This extensible approach will also ideally make it easy for people to code their own strategies and plug those into the map generator with ease.

### NO ART!

Dominions 6 introduced a new map format: d6m. This allows the game to handle the rendering of map art, all the mapper has to do is output the province information in the d6m file.

This new map format means I do not have to do any art assets and can focus more on the actual map generation logic and making robust, flexible tooling.

### CONCEPTS

Atomic Atlas has these fundamental concepts:

* __World__: A world consists of several planes. Typically only 2 planes are generated, but I plan to support the maximum amount that Dominions 6 supports (9 total).
* __World Plane__: One plane that exists in the world. A plane consists of a collection of nodes and connections.
* __Strategy__: A strategy describes how the world is generated. It governs the number of nodes on each plane, how they are placed, and how they are connected. Each strategy can have any number of strategy definitions that govern its behaviour.
* __Strategy Definition__: A strategy definition is a set of configurable parameters for a given strategy. These definitions are stored in XML and will be modifiable through the editor interface too. 

### THE EDITOR

Atomic Atlas will have a simplified, minimal interface that takes up less screen space. The settings panel will house several tabs for all of the relevant data users need to change.

World planes will be generated in 2d but if a map has several planes, you can switch to a 3d stack of planes to better visualize how provinces connect between planes. I am still working on figuring out how to nicely implement this kind of an editor flow.

Similar to MapNuke, nodes and connections can have their terrain data modified. I plan to also allow users to move the nodes around and even add/delete nodes on a plane.

There will be a validation system in place to ensure users cannot create invalid maps, such as planes where nodes have overlapping connections or links between planes that are broken.

### SOFTWARE ARCHITECTURE IMPROVEMENTS

Atomic Atlas uses dependency injection to keep systems decoupled properly with clear interfaces. The app uses a state machine to simplify its execution flow and there is a generic data loading system that it uses for handling nation data, strategy data, procedural province naming data, and any other serializable data.







