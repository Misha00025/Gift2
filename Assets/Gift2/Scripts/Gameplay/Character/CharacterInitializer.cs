using System.Collections.Generic;
using Gift2.Core;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Character), typeof(CharacterMover))]
public class CharacterInitializer : MonoInstaller
{
    [Inject] private Player Player;
    [SerializeField] private CharacterConfig Config;
    private List<Weapon> StartWeapons => Config.StartWeapons;
    
    [SerializeField] private RotateAroundCenter Rotator;

    public override void InstallBindings()
    {
        var character = GetComponent<Character>();
        
        Container.Bind<Character>().FromInstance(character).AsSingle();
        
    }
    
    void Awake()
    {
        var character = GetComponent<Character>();
        var mover = GetComponent<CharacterMover>();
        character.Initialize(Config, mover, Rotator);
        Player.Character = character;
        
        foreach( var weapon in StartWeapons)
        {
            character.AddWeapon(weapon);
        }
    }
}
