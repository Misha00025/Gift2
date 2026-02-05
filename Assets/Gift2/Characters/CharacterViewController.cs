using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterViewController : MonoBehaviour
{
    [Header("View")]
    public Animator Animator {get; private set;}
    
    public Transform LeftHand;
    public Transform RightHand;
    
    public Transform Pivot;
    
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
}
