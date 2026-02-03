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
            Character.ApplyEffect(onHitAdditionalDamageEffectBuilder?.Build());
            
        if (Input.GetKeyDown(KeyCode.Alpha2))
            for (int i = 0; i < 5; i++)
                Character.ApplyEffect(onHitAdditionalDamageEffectBuilder?.Build());
                
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Character.ApplyEffect(tickableDamageEffectBuilder?.Build());  
    }
}
