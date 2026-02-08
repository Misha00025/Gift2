using UnityEngine;
using UnityEngine.Events;

public class FireElementalCastHandler : MonoBehaviour
{
    public UnityEvent Casted = new();
    public UnityEvent Ended = new();


    public void OnCast()
    {
        Casted.Invoke();
    }
    
    public void OnCatsEnd()
    {
        Ended.Invoke();
    }
}
