using Gift2.Core;
using Gift2.Gameplay;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterMover _mover;
    private CharacterConfig _config;
    private Player _player;

    public void Initialize(CharacterConfig config, CharacterMover mover, Player player)
    {
        _config = config;
        _mover = mover;
        _player = player;
    }
    
    public CharacterStats BaseStats => _config.Stats;
    public CharacterStats Stats => GetStats();
    public CharacterStats showStats;
    
    
    public void Move(Vector2 direction) => _mover.Move(direction);
    
    
    private CharacterStats GetStats()
    {
        if (_player?.Items == null) 
            return BaseStats;
        
        var stats = BaseStats;
        
        var items = _player.Items;
        foreach(var item in items)
        {
            if (item.Item is IBuff)
                for (int i = 0; i < item.Amount; i++)
                    stats = ((IBuff)item.Item).Apply(stats);
            showStats = stats;
        }
        
        return stats;
    }
}
