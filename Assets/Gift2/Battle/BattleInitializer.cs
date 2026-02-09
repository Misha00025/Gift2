using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BattleLoop), typeof(BattleMap), typeof(BattleInputHandler))]
public class BattleInitializer : MonoBehaviour
{
    public class StartupData
    {
        public static StartupData Instance {get; private set;}
        
        public CharacterConfig MainSummon {get; private set;}
        public IReadOnlyList<CharacterConfig> Supports {get; private set;}
        
        
        private StartupData()
        {
            Instance = this;
        }
        
        public static void RegisterData(CharacterConfig main, List<CharacterConfig> supports = null)
        {
            var data = new StartupData();
            data.MainSummon = main;
            data.Supports = supports ?? new List<CharacterConfig>();
        }
        
        public static void Clear()
        {
            Instance = null;
        }
    }

    public Character MainSummon;
    public Character Enemy;
    
    public CharacterStatusBar PlayerStatusBar;
    public CharacterStatusBar EnemyStatusBar;
    
    public List<Character> Supports = new();

    void Awake()
    {
        var loop = GetComponent<BattleLoop>();       
        var map = GetComponent<BattleMap>();       
        var inputHandler = GetComponent<BattleInputHandler>();
        
        if (StartupData.Instance != null)
        {
            var data = StartupData.Instance;
            MainSummon = data.MainSummon.Prefab;
            Supports = data.Supports.Select(e => e.Prefab).ToList();
            StartupData.Clear();
        } 
        
        loop.MainSummon = Instantiate(MainSummon, map.MainSummonPosition);
        inputHandler.Character = loop.MainSummon;
        loop.Enemy = Instantiate(Enemy, map.EnemyPosition);
        
        for (var i = 0; i < Supports.Count; i++)
        {
            if (i >= map.SupportSummonPositions.Count) continue;
        
            var point = map.SupportSummonPositions[i];
            var summon = Instantiate(Supports[i], point);
        
            summon.View.SetOn(point.position);
            summon.SetTarget(loop.MainSummon);
            inputHandler.Supports.Add(summon);
        }
            
        loop.MainSummon.View.SetOn(map.MainSummonPosition.position);
        loop.Enemy.View.SetOn(map.EnemyPosition.position);
        PlayerStatusBar?.SetCharacter(loop.MainSummon);
        EnemyStatusBar?.SetCharacter(loop.Enemy);
    }
    
    public void BackToSelectTeam()
    {
        SceneManager.LoadScene("SelectTeamScene");
    }
}
