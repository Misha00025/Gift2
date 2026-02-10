# Code Style Guidelines for Gift2 Unity Project

This document outlines the coding conventions and patterns used in the Gift2 Unity project.

## 1. Namespace Usage

- No explicit namespace declarations are used in the project
- All classes are defined in the global namespace
- This appears to be a deliberate choice for simplicity in a small-to-medium sized Unity project

## 2. Class Structure and Organization

### Base Classes and Inheritance

- Abstract base classes are used extensively for common functionality
- Key base classes:
  - `Character` - Abstract base for all characters
  - `Skill` - Abstract base for all skills
  - `Effect` - Base class for status effects

### MonoBehaviour Patterns

- MonoBehaviour components follow a consistent pattern:
  - Public serialized fields for editor configuration
  - Private fields for internal state
  - Unity event methods (Awake, Start, Update) for lifecycle management

## 3. Field Declaration and Serialization

### Field Modifiers and Attributes

- Use of `[field: SerializeField]` attribute for auto-properties:
  ```csharp
  [field: SerializeField] public Property Health { get; private set; }
  ```
- Separate `[SerializeField]` for private fields:
  ```csharp
  [SerializeField] private Stats _stats = new(){attackSpeed = 1f, damage = 1};
  ```
- Use of `[Header]` attribute to organize inspector view:
  ```csharp
  [Header("Character Events")]
  [field: SerializeField] public UnityEvent<Damage> DamageTaken { get; private set; } = new();
  ```
- `[RequireComponent]` attribute is used when a component depends on another:
  ```csharp
  [RequireComponent(typeof(CharacterViewController))]
  ```

## 4. Properties and Access Modifiers

### Property Patterns

- Public properties with private setters are used for read-only access
- Auto-properties are preferred when no additional logic is needed
- Properties are used to expose functionality while maintaining encapsulation

### Naming Conventions

- PascalCase for public members and types
- camelCase for private fields (with underscore prefix: `_fieldName`)
- Use of `I` prefix for interfaces (e.g., `IEffect`, `IOnHitEffect`)

## 5. Unity Event System

### Event Usage

- UnityEvent is used extensively for decoupling components
- Events are initialized in declaration:
  ```csharp
  [field: SerializeField] public UnityEvent<Damage> DamageTaken { get; private set; } = new();
  ```
- Event listeners are added in Start() or initialization methods
- Event listeners are removed when no longer needed to prevent memory leaks

## 6. Effect System Architecture

### Effect Interfaces

The effect system uses a component-based approach with multiple interfaces:

- `IEffect` - Base interface for all effects
- `IOnHitEffect` - Triggers when the character hits
- `IOnHitAttemptEffect` - Triggers during hit attempt (before hit is confirmed)
- `IOnEffectApplyEffect` - Triggers when effect is applied
- `ITikableEffect` - Updates on regular intervals
- `IBeforeTakeDamageEffect` - Modifies damage before application

### Effect Classes

- `Effect` - Base implementation with Apply/Disable methods
- `DurationEffect` - Effect with time-based duration
- `TikableEffect` - Abstract base for effects that tick on interval
- `CountedOnHitEffect` - Effect that triggers on hit and has usage count
- `OneHitEffect` - Specialization of CountedOnHitEffect for single use

## 7. Skill System

### Skill Architecture

- `ISkill` interface defines the contract for all skills
- Abstract `Skill` class provides base implementation
- Generic `Skill<TCharacter, TConfig>` class for type-safe skills
- Skills use the event pattern for completion notification

### Skill Lifecycle

- `Play()` method starts the skill execution
- `OnPlay()` abstract method for skill-specific logic
- `Complete()` method triggers the Completed event
- `InProgress` flag prevents multiple executions

## 8. Battle System

### Battle Loop

- `BattleLoop` class manages turn-based combat
- Time-based turn system using character attack speed
- Characters take turns based on remaining time calculation
- Supports normal and fast speed modes

### Character Interaction

- Characters maintain references to their target
- Damage calculation goes through the Hit system
- Events are used to communicate between systems

## 9. Code Structure and Patterns

### Composition over Inheritance

- Complex behaviors are built through composition of effects and skills
- Characters gain abilities by having effects applied rather than inheritance
- This allows for more flexible and modular design

### Single Responsibility Principle

- Classes have clear, focused responsibilities
- `BattleLoop` handles turn management
- `Character` handles character state and actions
- `Effect` classes handle status effects
- `Skill` classes handle special abilities

## 10. Initialization Patterns

### Awake vs Start

- `Awake()` is used for component initialization and dependency setup
- `Start()` is used for game logic initialization that depends on other components
- Virtual `Init()` methods are used for class-specific initialization

### Event Subscription

- Event listeners are typically added in Start() method
- Event listeners are removed when appropriate to prevent memory leaks
- Example from BattleLoop.cs:
  ```csharp
  void Start()
  {    
      Characters = new(){ MainSummon, Enemy };
      foreach (var character in Characters)
      {
          character.AttackCompleted.AddListener(OnCharacterAttackCompleted);
          character.SetTarget(GetTarget(character));
          RemainingTimes.Add(character, GetRemaining(character));
      }
  }
  ```

## 11. Performance Considerations

### Object References

- Caches references to frequently accessed components
- Example from Character.cs:
  ```csharp
  private CharacterViewController _view;
  public CharacterViewController View 
  {
      get 
      {
          if (_view == null)
              _view = GetComponent<CharacterViewController>();
          return _view;
      }
  }
  ```

### Loop Optimization

- Uses enumeration in reverse order when removing items:
  ```csharp
  for (var i = _effects.Count - 1; i >= 0; i--)
  {   
      var effect = _effects[i];
      // ...
  }
  ```

## 12. Error Handling

### Defensive Programming

- Null checks are performed before accessing references
- TryGetComponent is used as a safer alternative to GetComponent
- Example from Skill.cs:
  ```csharp
  if (Caster == null && !TryGetComponent(out Caster)) 
  {
      Debug.LogError("Caster is null");
      return;
  }
  ```

### Debug Logging

- Debug.LogError is used for critical issues
- No extensive logging framework appears to be in use

## 13. Configuration and Data

### ScriptableObjects

- Skill configurations are implemented as ScriptableObjects (SkillConfig)
- Effect configurations use ScriptableObjects (e.g., AdditionalDamage.asset)
- Character configurations use ScriptableObjects (e.g., FireElementalConfig.asset)

### Structs for Data

- Simple data containers use structs with [Serializable] attribute
- Example from Stats.cs:
  ```csharp
  [Serializable]
  public struct Stats
  {
      public float attackSpeed;
      public int damage;
  }
  ```

## 14. UI Integration

### UI Components

- UI elements are implemented as MonoBehaviour components
- Prefabs are used for UI elements
- Event system is used for user interaction

### UI Patterns

- Panels are set up with data in Start() method
- Example from BattleInputHandler.cs:
  ```csharp
  void Start()
  {
      List<Skill> skills = new();
      skills.Add(Character.MainActiveSkill);
      foreach (var sup in Supports)
      {
          skills.Add(sup.SupportActiveSkill);
      }
      SkillsPanel.SetupSkills(skills);
  }
  ```