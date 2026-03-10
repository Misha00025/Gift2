using UnityEngine;
using UnityEngine.Events;

namespace Gift2
{
    public class InteractableEvent : Interactable
    {
        public UnityEvent Interacted = new(); 
    
        public override void Use()
        {
            if (enabled)
                Interacted.Invoke();
        }
    }
}
