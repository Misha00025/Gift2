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
    public float Strength;
    public float MoveSpeed;
    
    
    public static CharacterStats Default => new()
    {
        Strength = 1f,
        MoveSpeed = 5f  
    };
}
