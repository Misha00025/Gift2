using UnityEngine;
using UnityEngine.Events;

namespace Gift2
{
    public abstract class Interactable : MonoBehaviour
    {
        public UnityEvent Selected = new();
        public UnityEvent Deselected = new();
    
        public virtual void Select() { Debug.Log("Selected!"); Selected.Invoke(); }
        public virtual void Deselect() { Debug.Log("Deselected!"); Deselected.Invoke(); }
        public abstract void Use();
    }
}