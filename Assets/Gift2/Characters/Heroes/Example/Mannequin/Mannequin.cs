using UnityEngine;

public class Mannequin : Character
{
    public override void Attack(Character target)
    {
        Debug.Log("mannequin can't attack someone");
    }
}
