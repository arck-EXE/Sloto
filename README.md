# Slot Machine Game

This project is a simple slot machine game built in Unity. It was developed as part of an internship application for Underpin Services to demonstrate knowledge of Unity game development, object-oriented programming, and clean project structure.

## Features

- **Spinning Reels**: Three reels that spin with adjustable speed and duration.
- **Weighted Symbols**: Each symbol has a weight, allowing for customizable rarity and probability.
- **Win Detection**: Checks the center row after each spin to determine if a winning combination is present.
- **Symbol Spawning**: Symbols are dynamically spawned on reels with proper positioning and spacing.
- **Expandable Design**: Easily extendable to support more reels, additional symbols, or new game rules.

## Project Structure

- **Scripts/Controllers**: Core game logic, including the `GameManager`, `Reel`, and `ReelSymbolSpawner`.
- **ScriptableObjects**: Symbol data, including sprite and weight.
- **Prefabs**: Reel symbols and UI elements for reusability.
- **Scenes**: A main scene to test and play the game.

## How to Play

1. Press the **Spin** button to start the reels.
2. The reels will spin for a set amount of time before stopping.
3. If all center symbols match, the game declares a win.
4. Symbol probabilities can be adjusted by editing the weights in the inspector.

## Technical Notes

- Uses **ScriptableObjects** for clean data-driven symbol management.
- Designed with modularity in mind: each reel manages its own symbols and communicates results back to the game manager.
- Can be extended with sound, UI polish, or reward systems.

## Future Improvements

- Add visual and audio feedback for winning spins.
- Implement coin balance and betting system.
- Expand to support paylines and multiple winning patterns.
- Add animations for reel spins and symbol highlights.

## Purpose

This project was created to showcase practical Unity development skills, including working with prefabs, scriptable objects, probability systems, and game loop management. The code emphasizes clarity, modularity, and maintainability.

## Known Bugs

- The win condition are not working as intended.
