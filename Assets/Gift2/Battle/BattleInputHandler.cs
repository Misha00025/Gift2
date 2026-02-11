using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleInputHandler : MonoBehaviour
{
    public SkillsPanel SkillsPanel;

    public Summoner Summoner;

    public Character Character => Summoner.MainCharacter;
    public IReadOnlyList<Character> Supports => Summoner.Supports; 
    public BattleLoop BattleLoop;
    private Skill _currentSkill;

    void Start()
    {
        List<Skill> skills = new();
        skills.Add(Character.MainActiveSkill);
        foreach (var sup in Supports)
        {
            skills.Add(sup.SupportActiveSkill);
        }
        SkillsPanel.SetupSkills(skills);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            PlaySpell1();
            
        if (Input.GetKeyDown(KeyCode.Alpha2))
            PlaySpell2();
                
        if (Input.GetKeyDown(KeyCode.Alpha3))
            PlaySpell3();
            
        if (Input.GetKeyDown(KeyCode.Space))
            BattleLoop?.TimeScaler?.SetFastSpeed();
        if (Input.GetKeyUp(KeyCode.Space))
            BattleLoop?.TimeScaler?.SetNormalSpeed();
    }
    
    public void PlaySpell1()
    {
        Play(Character.MainActiveSkill);
    }

    
    private void Play(Skill skill)
    {
        if (skill == null) return;
        if (_currentSkill != null) return;
        if (Summoner.CanCast() == false) return;
        
        BattleLoop.SetPause(true);
        _currentSkill = skill;
        _currentSkill.Completed.AddListener(OnCompleteSkill);
        Summoner.Cast(skill);
    }    
    private void OnCompleteSkill()
    {
        BattleLoop.SetPause(false);
        _currentSkill.Completed.RemoveListener(OnCompleteSkill);
        _currentSkill = null;
    }
    
    public void PlaySpell2()
    {
        if (Supports.Count < 1) return;
        Play(Supports[0].SupportActiveSkill);
    }
    
    public void PlaySpell3()
    {
        if (Supports.Count < 2) return;
        Play(Supports[1].SupportActiveSkill);
    }
}
