using System.Collections.Generic;
using Gift2.Core;
using UnityEngine;

[RequireComponent(typeof(Character), typeof(CharacterMover))]
public class CharacterInitializer : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private CharacterConfig Config;
    [SerializeField] private Weapon StartWeaponPrefab => Config.StartWeaponPrefab;
    
    [SerializeField] private ItemsCollector Collector;
    [SerializeField] private RotateAroundCenter Rotator;

    void Awake()
    {
        var character = GetComponent<Character>();
        var mover = GetComponent<CharacterMover>();
        character.Initialize(Config, mover, Player);
        mover.Initialize(character);
        Collector.Initialize(character);
        Rotator.Initialize(character);
        
        for (int i = 0; i < character.Stats.StartWeapons; i++)
        {
            var weapon = Instantiate(StartWeaponPrefab, Rotator.transform);
            var dt = weapon.GetComponent<DistanceTrigger2D>();
            if (dt != null)
            {
                dt.Ignore.Add(character.gameObject);
                dt.Ignore.Add(Collector.gameObject);
            }
            weapon.SetOwner(character);
            Rotator.AddObject(weapon.transform);   
        }
    }
}
