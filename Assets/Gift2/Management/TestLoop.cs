using UnityEngine;

public class TestLoop : MonoBehaviour
{
    public float Delay = 1.0f;
    public float RemainingDelay;
    
    public Character Player;
    public Character Enemy;
    
    private Character CurrentCharacter;
    
    public void Start()
    {
        RemainingDelay = Delay;
        CurrentCharacter = Player;
    }
    
    
    void Update()
    {
        
        if (RemainingDelay < 0)
        {
            if (CurrentCharacter == Player)
            {
                CurrentCharacter.Attack(Enemy);
                CurrentCharacter = Enemy;
            }
            else
            {
                CurrentCharacter.Attack(Player);
                CurrentCharacter = Player;
            }
        }
        else
        {
            RemainingDelay -= Time.deltaTime;
            
        }
    }
}
