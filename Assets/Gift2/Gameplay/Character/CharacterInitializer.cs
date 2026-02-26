using UnityEngine;

[RequireComponent(typeof(Character), typeof(CharacterMover))]
public class CharacterInitializer : MonoBehaviour
{
    [SerializeField] private CharacterConfig Config;
    [SerializeField] private ItemsCollector Collector;
    [SerializeField] private RotateAroundCenter Rotator;

    void Awake()
    {
        var character = GetComponent<Character>();
        var mover = GetComponent<CharacterMover>();
        character.Initialize(Config, mover);
        mover.Initialize(character);
        Collector.Initialize(character);
        Rotator.Initialize(character);
    }
}
