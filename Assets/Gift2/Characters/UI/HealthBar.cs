using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Character _character;
    public SlicedFilledImage healthFiller;
    
    private int _maxValue;
    
    public void SetCharacter(Character character)
    {
        _character = character;
        _maxValue = character.Health.MaxValue;
        character.Health.Changed.AddListener((e) => ChangeFiller(e.Value));
    }
    
    public void Start()
    {
        if (_character == null || healthFiller == null) return;
        SetCharacter(_character);
    }
    
    private void ChangeFiller(int value)
    {
        healthFiller.fillAmount = ((float)value)/_maxValue;
    }
}
