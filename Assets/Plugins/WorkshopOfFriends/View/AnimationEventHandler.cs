using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Wof.Views
{
    public class AnimationEventHandler : MonoBehaviour
    {
        private Dictionary<string, UnityEvent> _events = new ();
        
        public void AddListener(string key, UnityAction action)
        {
            if (_events.ContainsKey(key) == false)
                _events.Add(key, new());
            _events[key].AddListener(action);
        }
        
        public void InvokeEvent(string key)
        {
            if (_events.ContainsKey(key) == false) return;
            
            _events[key].Invoke();
        }
        
        public void RemoveListener(string key, UnityAction action)
        {
            if (_events.ContainsKey(key) == false) return;
            
            _events[key].RemoveListener(action);
        }
    }
}