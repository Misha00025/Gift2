using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterMover _mover;
    private CharacterConfig _config;

    public void Initialize(CharacterConfig config, CharacterMover mover)
    {
        _config = config;
        _mover = mover;
    }
    
    public CharacterStats BaseStats => _config.Stats;
    public CharacterStats Stats => BaseStats;
    
    public void Move(Vector2 direction) => _mover.Move(direction);
}
