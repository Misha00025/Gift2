using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    
    public SummonerStatusBar PlayerStatusBar;
    public CharacterStatusBar EnemyStatusBar;
    
    public List<Character> Supports = new();
    public List<Character> Enemies = new();
    
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
        var queue = new BattleQueue();
        
        Debug.Log($"Count of enemies: {Enemies.Count}");
        
        for (int i = 0; i < Enemies.Count; i++)
            queue.Enemies.Enqueue(Enemies[i]);
        Debug.Log($"Count of enemies queue: {queue.Enemies.Count}");
        
        
        _battle.summoner = summoner;
        _battle.map = map;
        _battle.loop = loop;
        _battle.queue = queue;
        
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
        
        var conditionSystem = new BattleConditionSystem(_battle);
        _battle.conditionSystem = conditionSystem;
        conditionSystem.StepEnded.AddListener(OnStepEnded);
        conditionSystem.Completed.AddListener(OnBattleCompleted);
        SetupEnemy();
    }
    
    private void OnStepEnded()
    {
        StartCoroutine(StepChangingProcess());
    }
    
    private IEnumerator StepChangingProcess()
    {
        _battle.loop.SetPause(true);
        Battle.Enemy.Animator.Play(AnimationKey.Die);
        yield return new WaitForSeconds(1.2f);
        SetupEnemy();
        yield return new WaitForSeconds(1f);
        _battle.loop.SetPause(false);
    }
    
    private void OnBattleCompleted()
    {
        _battle.loop.SetPause(true);
        if (Battle.Enemy.Health.Value <= 0)
            Battle.Enemy.Animator.Play(AnimationKey.Die);
        else
            Battle.MainSummon.Animator.Play(AnimationKey.Die);
    }
    
    public void SetupEnemy()
    {
        // Debug.LogWarning("Start Setup Enemy");
        // Debug.Log($"Count of enemies queue: {_battle.queue.Enemies.Count}");
        var enemy = Battle.Enemy;
        if (Battle.Enemies.Count > 0)
        {
            var newEnemy = GameObject.Instantiate(Battle.Enemies.Dequeue(), Battle.Map.EnemyPosition);
            EnemyStatusBar.SetCharacter(newEnemy);
            _battle.enemy = newEnemy;
            Battle.Enemy.View?.SetOn(Battle.Map.EnemyPosition.position);
            _battle.loop.Initialize(Battle.Enemy, Battle.Summoner, _battle.conditionSystem);
        }
        if (enemy != null)
            GameObject.Destroy(enemy.gameObject); 
        // Debug.Log($"Count of enemies queue: {_battle.queue.Enemies.Count}");
        // Debug.LogWarning("End Setup Enemy");
    }
    
    private void InitializeView()
    {
        PlayerStatusBar?.SetSummoner(Battle.Summoner);
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
