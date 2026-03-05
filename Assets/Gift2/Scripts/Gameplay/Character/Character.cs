using Gift2.Core;
using Gift2.Gameplay;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterMover _mover;
    private CharacterConfig _config;
    private Player _player;
    private RotateAroundCenter _rotator;

    public void Initialize(CharacterConfig config, CharacterMover mover, Player player, RotateAroundCenter rotator)
    {
        _config = config;
        _mover = mover;
        _player = player;
        _rotator = rotator;
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
    
    public void AddWeapon(Weapon prefab)
    {
        var weapon = Instantiate(prefab, _rotator.transform);
            var dt = weapon.GetComponent<DistanceTrigger2D>();
            if (dt != null)
            {
                dt.Ignore.Add(this.gameObject);
                // dt.Ignore.Add(Collector.gameObject);
            }
            weapon.SetOwner(this);
            _rotator.AddObject(weapon.transform);
    }
}
