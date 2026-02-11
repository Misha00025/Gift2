using UnityEngine;

public class HealthBar : PropertyView
{
    [SerializeField] private Character _character;
    
    private int _maxValue;
    
    public void SetCharacter(Character character)
    {
        _character = character;
        SetProperty(_character.Health);        
    }
    
    public void Start()
    {
        if (_character == null || Filler == null) return;
        SetCharacter(_character);
    }
}
