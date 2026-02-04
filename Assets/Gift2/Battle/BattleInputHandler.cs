using System;
using UnityEngine;

public class BattleInputHandler : MonoBehaviour
{
    public Character Character;
    

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
