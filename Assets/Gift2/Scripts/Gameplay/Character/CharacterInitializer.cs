using System.Collections.Generic;
using Gift2.Core;
using UnityEngine;

[RequireComponent(typeof(Character), typeof(CharacterMover))]
public class CharacterInitializer : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private CharacterConfig Config;
    [SerializeField] private List<Weapon> StartWeapons => Config.StartWeapons;
    
    [SerializeField] private ItemsCollector Collector;
    [SerializeField] private RotateAroundCenter Rotator;

    void Awake()
    {
        var character = GetComponent<Character>();
        var mover = GetComponent<CharacterMover>();
        character.Initialize(Config, mover, Player, Rotator);
        mover.Initialize(character);
        Collector.Initialize(character);
        Rotator.Initialize(character);
        
        Player.Character = character;
        
        foreach( var weapon in StartWeapons)
        {
            character.AddWeapon(weapon);
        }
    }
}
