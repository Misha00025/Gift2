using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Characters/CharacterConfig")]
public class CharacterConfig : ScriptableObject
{
    public Character Prefab;
    public Sprite Icon;
    
    public string Name;
    
    [Multiline(5)]
    public string Description;
}
