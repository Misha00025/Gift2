using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Character/CharacterConfig")]
public class CharacterConfig : ScriptableObject
{
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
}
