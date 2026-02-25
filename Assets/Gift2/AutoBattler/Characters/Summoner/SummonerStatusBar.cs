using UnityEngine;

public class SummonerStatusBar : MonoBehaviour
{
    public CharacterStatusBar CharacterBar;
    public PropertyView ManaBar;
    
    public void SetSummoner(Summoner summoner)
    {
        CharacterBar.SetCharacter(summoner.MainCharacter);
        ManaBar.SetProperty(summoner.Mana);
    }
}
