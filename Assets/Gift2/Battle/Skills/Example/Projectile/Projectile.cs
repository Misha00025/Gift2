using UnityEngine;

public class Projectile : Skill
{
    public Animator Animator;
    public Damage Damage = new(){Value = 1};

    public override void Play()
    {
        
    }
    
    public void OnComplete()
    {
        Destroy(gameObject);
    }
}
