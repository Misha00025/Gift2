# Gift2 Unity Project

This repository contains a Unity-based game prototype called "Gift2", featuring a turn-based battle system with elemental characters, skills, and status effects.

## Project Structure

The project follows a component-based architecture with clear separation of systems:

- `Assets/Gift2/Battle` - Core battle mechanics, loop, input handling, and effects
- `Assets/Gift2/Characters` - Character base class, stats, elements, and hero implementations
- `Assets/Gift2/Management` - Game state management and scene transitions
- `Assets/Gift2/Summoner` - Summoner-related functionality
- `Assets/Gift2/Scenes` - Game scenes (BattleScene, SelectTeamScene)

## Core Systems

### Battle System

The battle system is controlled by several key components:

- `BattleLoop.cs` - Manages the turn-based combat flow, character actions, and timing
- `BattleInitializer.cs` - Sets up the battle with player and enemy characters
- `BattleInputHandler.cs` - Handles player input for skill activation
- `BattleMap.cs` - Defines positioning for characters in battle

The battle operates on a time-based system where characters take turns based on their attack speed stat.

### Character System

- `Character.cs` - Abstract base class for all characters with health, stats, skills, and effects
- `Stats.cs` - Contains character statistics (attack speed, damage)
- `Enums.cs` - Defines elemental types: Physical, Fire, Ice, Water, Stone, Wind, Plant

Characters can have:
- Main active skill
- Support active skill
- Multiple status effects
- Elemental affinities

### Effects System

The effects system allows for various status conditions and modifiers:

- `Effect.cs` - Base interface and implementations for effects
- Implements multiple interfaces:
  - `IOnHitEffect` - Triggers when the character hits
  - `ITikableEffect` - Updates on regular intervals
  - `IBeforeTakeDamageEffect` - Modifies damage before application
  - `IOnEffectApplyEffect` - Triggers when effect is applied

Effects can be duration-based, tickable, or single/counter-based.

### Skills System

- `Skill.cs` - Base class for all skills with name, description, and icon
- Skills can be targetable and have completion events
- Implemented as MonoBehaviour for Unity integration

### UI System

- `SkillsPanel.cs` - Displays available skills for player
- `CharacterStatusBar.cs` - Shows character health and status
- `SummonPanel.cs` - Used in team selection screen

## Game Flow

1. Player selects team in `SelectTeamScene`
2. Selected characters are passed to `BattleScene` via `BattleInitializer.StartupData`
3. Battle begins with player's summons vs enemy
4. Players can use skills (number keys) and toggle speed (space)
5. Characters auto-attack based on attack speed

## Controls

- `1, 2, 3` - Use main, left support, and right support skills
- `Space` - Hold to speed up battle

## Implementation Notes

- Uses Unity's event system for character actions and skill completion
- Positioning accounts for character pivot points
- Effect views are dynamically created and managed
- Battle speed can be toggled between normal and fast modes

## Future Development

Potential areas for expansion:
- Add more elemental interactions and weaknesses
- Implement skill cooldowns
- Add visual effects for attacks and status conditions
- Expand the team selection interface
- Add sound effects and music
- Implement save/load functionality
