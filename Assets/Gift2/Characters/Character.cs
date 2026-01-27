using System;
using UnityEngine;
using UnityEngine.Events;


public class Character : MonoBehaviour
{
    [SerializeField] private int _health;

    public int Health { 
        get => _health;
        set
        {
            _health = value;
            HealthChanged.Invoke();
        } 
    }
    
    [field: SerializeField] public UnityEvent HealthChanged { get; private set; } = new UnityEvent();
    
}
