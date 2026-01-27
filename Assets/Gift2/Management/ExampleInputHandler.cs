using UnityEngine;

public class ExampleInputHandler : MonoBehaviour
{
    public Character character;
    
    void Update()
    {
        if (character == null) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var damage = character.CalculateDamage(new Damage(){Value = 10, Element = Element.Fire});
            character.ApplyDamage(damage);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var damage = character.CalculateDamage(new Damage(){Value = 10, Element = Element.Wind});
            character.ApplyDamage(damage);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            var damage = character.CalculateDamage(new Damage(){Value = 10, Element = Element.Stone});
            character.ApplyDamage(damage);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            var damage = character.CalculateDamage(new Damage(){Value = 10, Element = Element.Water});
            character.ApplyDamage(damage);
        }
    }
}
