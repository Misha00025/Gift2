using UnityEngine;

[CreateAssetMenu(fileName = "SummonerConfig", menuName = "SummonerConfig")]
public class SummonerConfig : ScriptableObject
{
    public SummonerStats Stats = SummonerStats.Default;
}
