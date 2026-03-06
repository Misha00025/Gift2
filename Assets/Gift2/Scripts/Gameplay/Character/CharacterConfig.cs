using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Character/CharacterConfig")]
public class CharacterConfig : ScriptableObject
{
    [field: SerializeField] public List<Weapon> StartWeapons { get; private set; }
    [field: SerializeField] public CharacterInfo Info { get; private set; }
    [field: SerializeField] public CharacterStats Stats { get; private set; } = CharacterStats.Default;
}

[Serializable]
public struct CharacterInfo
{
    public string Name;
    public string Description;
    public Sprite Icon;
}

[Serializable]
public struct CharacterStats 
{
    public float CollectingRadiusScale;

    public float Strength;
    
    public float RotationSpeed;
    public float RotationRadius;
    
    public float MoveSpeed;
    
    
    public static CharacterStats Default => new()
    {
        CollectingRadiusScale = 1f, 
        Strength = 1f,
        RotationSpeed = 60f,
        RotationRadius = 2f,
        MoveSpeed = 5f  
    };
    
    public static CharacterStats One => new()
    {
        CollectingRadiusScale = 1f, 
        Strength = 1f,
        RotationSpeed = 1f,
        RotationRadius = 1f,
        MoveSpeed = 1f  
    };
    
    public static CharacterStats operator *(CharacterStats left, CharacterStats right)
    {
        var result = left;
        
        result.CollectingRadiusScale *= right.CollectingRadiusScale;
        result.Strength *= right.Strength;
        result.RotationSpeed *= right.RotationSpeed;
        result.RotationRadius *= right.RotationRadius;
        result.MoveSpeed *= right.MoveSpeed;
        
        return result;
    }
    
    public static CharacterStats operator +(CharacterStats left, CharacterStats right)
    {
        var result = left;
        
        result.CollectingRadiusScale += right.CollectingRadiusScale;
        result.Strength += right.Strength;
        result.RotationSpeed += right.RotationSpeed;
        result.RotationRadius += right.RotationRadius;
        result.MoveSpeed += right.MoveSpeed;
        
        return result;
    }
}
