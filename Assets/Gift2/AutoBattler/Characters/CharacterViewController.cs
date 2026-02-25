using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class CharacterViewController : MonoBehaviour, IPlayable
{
    [Header("View")]
    private Animator Animator {get; set;}
    
    private Dictionary<string, UnityEvent> _animationEvents = new();
    
    public Transform LeftHand;
    public Transform RightHand;
    
    public Transform Pivot;
    
    private string CurrentAnimation = "";
    
    void Awake()
    {
        if (Pivot == null)
            Pivot = transform;
        if (Animator == null)
            Animator = GetComponent<Animator>();
    }
    
    public void SetOn(Vector3 position)
    {
        var delta = transform.position - Pivot.position;
        transform.position = position + delta;
    }
    
    public void Play(string key)
    {
        if (Animator.runtimeAnimatorController == null)
            StartCoroutine(SimulateAnimation(key));
        else
            Animator.Play(key);
    }
    
    private IEnumerator SimulateAnimation(string key)
    {
        var isAttack = key == AnimationKey.Attack;
        var timer = isAttack ? 0.5f : 1f;
        yield return new WaitForSeconds(timer);
        if (isAttack)
        {
            AnimationEvent(AnimationEventKey.Hit);
            yield return new WaitForSeconds(timer);
        }
        AnimationEvent(AnimationEventKey.Completed);
    } 
    
    public void AddListener(string key, UnityAction action)
    {
        if (_animationEvents.ContainsKey(key) == false)
            _animationEvents.Add(key, new());
        _animationEvents[key].AddListener(action);        
    }
    
    public void RemoveListener(string key, UnityAction action)
    {
        if (_animationEvents.ContainsKey(key))
            _animationEvents[key].RemoveListener(action);        
    }
    
    public void AnimationEvent(string key)
    {
        Debug.Log($"AnimationEvent: {key} on {gameObject.name}");
        if (_animationEvents.ContainsKey(key))
            _animationEvents[key].Invoke();
    }
    
    public void CompleteAttack()
    {
        AnimationEvent(AnimationEventKey.Completed);
    }
    
    public void CancelAnimation()
    {
        AnimationEvent(AnimationEventKey.Completed);
    }
}

public interface IPlayable
{
    void Play(string key);
}

public static class AnimationKey
{
    public const string Attack = "BaseAttack";
    public const string DamageTaken = "DamageTaken";
    public const string Idle = "Idle";
    public const string Cast = "Cast";
    public const string Die = "Die";
}

public static class AnimationEventKey
{
    public const string Cast = "Cast";
    public const string Hit = "Hit";
    public const string Completed = "Completed";
}
