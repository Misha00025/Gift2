using System.Collections.Generic;
using UnityEngine;

namespace Gift2
{
    public class DamageScaler : MonoBehaviour
    {
        public static DamageScaler Instance { get; private set; }

        public float Scale = 0.5f;
    
        private Dictionary<string, float> _scales = new();
    
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }
        
        void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
        
        public void AddScale(string key)
        {
            if (_scales.ContainsKey(key) == false)
                _scales.Add(key, 1f);
            _scales[key] += Scale;
        }
        
        public float GetScale(string key)
        {
            if (_scales.ContainsKey(key))
                return _scales[key];
            else
                return 1f;
        }
    }
}
