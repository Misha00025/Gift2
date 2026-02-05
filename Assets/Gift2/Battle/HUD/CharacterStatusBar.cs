using UnityEngine;

public class CharacterStatusBar : MonoBehaviour
{
    public HealthBar HealthBar;

    private Character _character;

    public void SetCharacter(Character character)
    {
        _character = character;
        HealthBar.SetCharacter(_character);
    }
}
