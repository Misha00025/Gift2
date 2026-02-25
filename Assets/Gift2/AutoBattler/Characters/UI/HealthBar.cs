using Gift2.AutoBattler;
using UnityEngine;

public class HealthBar : PropertyView
{
    [SerializeField] private Character _character;
    
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
