using UnityEngine;
using UnityEngine.Events;

namespace Gift2
{
    public abstract class Interactable : MonoBehaviour
    {
        public UnityEvent Selected = new();
        public UnityEvent Deselected = new();
    
        public virtual void Select() { Debug.Log("Selected!"); }
        public virtual void Deselect() { Debug.Log("Deselected!"); }
        public abstract void Use();
    }
}