
using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class Property
{
    [SerializeField] private int _value = 10;
    [SerializeField] private int _maxValue = 10;

    public int Value {
        get => _value;
        set
        {
            _value = value;
            Changed.Invoke(this);
        }
    }
    
    public int MaxValue {
        get => _maxValue;
        set
        {
            _maxValue = value;
            Changed.Invoke(this);
        }
    }
    
    public UnityEvent<Property> Changed = new();
}
