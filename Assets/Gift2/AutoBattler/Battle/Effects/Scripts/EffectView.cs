using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EffectView : MonoBehaviour
{
    public Animator Animator {get; private set;}
    
    void Awake()
    {
        Animator = GetComponent<Animator>();
    }
}
