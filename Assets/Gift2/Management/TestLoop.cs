using UnityEngine;

public class TestLoop : MonoBehaviour
{
    public float Delay = 1.0f;
    public float RemainingDelay;
    
    public Character Attacker;
    public Character Target;
    
    public void Start()
    {
        RemainingDelay = Delay;
    }
    
    
    void Update()
    {
        
        if (RemainingDelay < 0)
        {
            Attacker.Attack(Target);
            RemainingDelay = Delay;
        }
        else
        {
            RemainingDelay -= Time.deltaTime;
            
        }
    }
}
