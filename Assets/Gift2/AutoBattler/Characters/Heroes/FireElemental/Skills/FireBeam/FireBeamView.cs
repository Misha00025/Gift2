using System;
using UnityEngine;
using UnityEngine.Events;

public class FireBeamView : MonoBehaviour
{
    public Transform Pivot;
    public Animator Animator;
    
    public UnityEvent Completed = new();

    private Action _onEffect;

    public void Place(Vector3 position)
    {
        var delta = transform.position - Pivot.transform.position;
        transform.position = position + delta;
    }
    
    public void Cast(Action onEffect)
    {
        gameObject.SetActive(true);
        Animator.Play("Base");
        _onEffect = onEffect;
    }
    
    
    public void OnEffect() => _onEffect?.Invoke();
    
    public void Complete()
    {
        Completed.Invoke();
        gameObject.SetActive(false);
    }
}