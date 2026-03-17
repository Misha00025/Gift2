
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Wof.Types
{
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
                if (_value > MaxValue)
                    _value = MaxValue;
                if (_value < 0)
                    _value = 0;
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

    public class FloatProperty : Property
    {
        private float _accumulatedValue = 0f;
        
        public void AddFloat(float value)
        {
            var sum = value + _accumulatedValue;
            Value += (int)sum;
            _accumulatedValue = sum - (int)sum;        
        }
    }
}