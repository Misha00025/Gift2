using Unity.VisualScripting;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Character character;
    public SlicedFilledImage healthFiller;
    
    private int _maxValue;
    
    
    public void Start()
    {
        if (character == null || healthFiller == null) return;
        _maxValue = character.Health.MaxValue;
        character.Health.Changed.AddListener((e) => ChangeFiller(e.Value));
    }
    
    private void ChangeFiller(int value)
    {
        healthFiller.fillAmount = ((float)value)/_maxValue;
    }
}
