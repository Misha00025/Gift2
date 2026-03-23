using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterMover _mover;
    private CharacterConfig _config;
    private RotateAroundCenter _rotator;

    public void Initialize(CharacterConfig config, CharacterMover mover, RotateAroundCenter rotator)
    {
        _config = config;
        _mover = mover;
        _rotator = rotator;
    }
    
    [SerializeField] private CharacterStats _multipliers = CharacterStats.One;
    
    public CharacterStats Multipliers => _multipliers;
    public CharacterStats BaseStats => _config.Stats;
    public CharacterStats Stats => GetStats();
    public CharacterStats showStats;
    
    
    public void Move(Vector2 direction) => _mover.Move(direction);
    
    
    private CharacterStats GetStats()
    {
        var stats = BaseStats;
        stats = stats * _multipliers;
        showStats = stats;
        return stats;
    }
    
    public void AddWeapon(Weapon prefab)
    {
        var weapon = Instantiate(prefab, _rotator.transform);
            var dt = weapon.GetComponent<DistanceTrigger2D>();
            if (dt != null)
            {
                dt.Ignore.Add(this.gameObject);
            }
            weapon.SetOwner(this);
            _rotator.AddObject(weapon.transform);
    }
    
    public void AddUpdate(CharacterStats update)
    {
        _multipliers = _multipliers + update;
    }
}
