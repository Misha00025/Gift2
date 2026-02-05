using System;
using UnityEngine;

public class BattleInputHandler : MonoBehaviour
{
    public Character Character;
    public BattleLoop BattleLoop;

    public OnHitAdditionalDamageEffectBuilder onHitAdditionalDamageEffectBuilder;
    public OnHitEffectOnTargetEffectBuilder tickableDamageEffectBuilder;
    private Skill _currentSkill;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            PlaySpell1();
            
        if (Input.GetKeyDown(KeyCode.Alpha2))
            PlaySpell2();
                
        if (Input.GetKeyDown(KeyCode.Alpha3))
            PlaySpell3();
            
        if (Input.GetKeyDown(KeyCode.Space))
            BattleLoop?.SetFastSpeed();
        if (Input.GetKeyUp(KeyCode.Space))
            BattleLoop?.SetNormalSpeed();
    }
    
    public void PlaySpell1()
    {
        Play(Character.MainActiveSkill);
    }

    
    private void Play(Skill skill)
    {
        if (skill == null) return;
        if (_currentSkill != null) return;
        BattleLoop.SetPause(true);
        _currentSkill = skill;
        _currentSkill.Completed.AddListener(OnCompleteSkill);
        _currentSkill.Play();
    }    
    private void OnCompleteSkill()
    {
        BattleLoop.SetPause(false);
        _currentSkill.Completed.RemoveListener(OnCompleteSkill);
        _currentSkill = null;
    }
    
    public void PlaySpell2()
    {
        Play(Character.SupportActiveSkill);
    }
    
    public void PlaySpell3()
    {
        Character.ApplyEffect(tickableDamageEffectBuilder?.Build());  
    }
}
