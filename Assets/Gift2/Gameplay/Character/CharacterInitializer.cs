using UnityEngine;

[RequireComponent(typeof(Character), typeof(CharacterMover))]
public class CharacterInitializer : MonoBehaviour
{
    [SerializeField] private CharacterConfig Config;

    void Awake()
    {
        var character = GetComponent<Character>();
        var mover = GetComponent<CharacterMover>();
        character.Initialize(Config, mover);
        mover.Initialize(character);
    }
}
