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
    
    public SummonerStatusBar PlayerStatusBar;
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
        var effectRegister = new EffectsRegister(loop.TickRate);
        
        var mainSummon = Instantiate(MainSummon, map.MainSummonPosition);
        var summoner = new Summoner(mainSummon);
        _battle.summoner = summoner;
        _battle.enemy = Instantiate(Enemy, map.EnemyPosition);
        _battle.map = map;
        _battle.loop = loop;
        
        for (var i = 0; i < Supports.Count; i++)
        {
            if (i >= map.SupportSummonPositions.Count) continue;
        
            var point = map.SupportSummonPositions[i];
            var summon = Instantiate(Supports[i], point);
        
            summon.View.SetOn(point.position);
            summon.SetTarget(Battle.MainSummon);
            summoner.AddSupport(summon);
        }
        
        Battle.MainSummon.View?.SetOn(Battle.Map.MainSummonPosition.position);
        Battle.Enemy.View?.SetOn(Battle.Map.EnemyPosition.position);
        loop.Initialize(Battle.Enemy, Battle.Summoner);
    }
    
    private void InitializeView()
    {
        PlayerStatusBar?.SetSummoner(Battle.Summoner);
        EnemyStatusBar?.SetCharacter(Battle.Enemy);
    }
    
    private void InitializeInputs()
    {
        var inputHandler = GetComponent<BattleInputHandler>();
        inputHandler.Summoner = Battle.Summoner;
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
