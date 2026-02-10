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
    
    private Battle _battle;

    void Awake()
    {    
        if (StartupData.Instance != null)
        {
            var data = StartupData.Instance;
            MainSummon = data.MainSummon.Prefab;
            Supports = data.Supports.Select(e => e.Prefab).ToList();
            StartupData.Clear();
        } 
                
        InitializeBattle();
        InitializeView();
        InitializeInputs();
    }
    
    private void InitializeBattle()
    {
        _battle = new();
        var loop = GetComponent<BattleLoop>();       
        var map = GetComponent<BattleMap>(); 
        
        _battle.mainSummon = Instantiate(MainSummon, map.MainSummonPosition);
        _battle.enemy = Instantiate(Enemy, map.EnemyPosition);
        
        _battle.map = map;
        
        for (var i = 0; i < Supports.Count; i++)
        {
            if (i >= map.SupportSummonPositions.Count) continue;
        
            var point = map.SupportSummonPositions[i];
            var summon = Instantiate(Supports[i], point);
        
            summon.View.SetOn(point.position);
            summon.SetTarget(Battle.MainSummon);
            _battle.supports.Add(summon);
        }
        
        loop.MainSummon = Battle.MainSummon;
        loop.Enemy = Battle.Enemy;
        loop.MainSummon.View.SetOn(map.MainSummonPosition.position);
        loop.Enemy.View.SetOn(map.EnemyPosition.position);
    }
    
    private void InitializeView()
    {
        PlayerStatusBar?.SetCharacter(Battle.MainSummon);
        EnemyStatusBar?.SetCharacter(Battle.Enemy);
    }
    
    private void InitializeInputs()
    {
        var inputHandler = GetComponent<BattleInputHandler>();
        inputHandler.Character = Battle.MainSummon;
        inputHandler.Supports.AddRange(Battle.Supports);
    }
    
    public void BackToSelectTeam()
    {
        SceneManager.LoadScene("SelectTeamScene");
    }
    
    void OnDestroy()
    {
        _battle.Clear();
    }
}
