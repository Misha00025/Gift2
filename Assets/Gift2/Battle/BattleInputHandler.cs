using System;
using UnityEngine;

public class BattleInputHandler : MonoBehaviour
{
    public Character Character;
    public BattleLoop BattleLoop;

    public OnHitAdditionalDamageEffectBuilder onHitAdditionalDamageEffectBuilder;
    public OnHitEffectOnTargetEffectBuilder tickableDamageEffectBuilder;

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
        Character.ApplyEffect(onHitAdditionalDamageEffectBuilder?.Build());
    }
    
    public void PlaySpell2()
    {
        for (int i = 0; i < 5; i++)
                Character.ApplyEffect(onHitAdditionalDamageEffectBuilder?.Build());
    }
    
    public void PlaySpell3()
    {
        Character.ApplyEffect(tickableDamageEffectBuilder?.Build());  
    }
}
