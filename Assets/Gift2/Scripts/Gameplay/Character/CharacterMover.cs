using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMover : MonoBehaviour
{
    private Character _character;
    private Rigidbody2D _rb;
    private Animator _animator;

    public float AccelerationTime = 0.1f;
    public float StopMultiplier = 5f;
    public float Speed => _character.Stats.MoveSpeed;
    
    

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Character character)
    {
        _character = character;
    }

    public void Move(Vector2 direction)
    {
        if (enabled == false) {
            _rb.linearVelocity = Vector3.zero;
            return;
        }
        var speed = Speed;
        if ((direction.x + direction.y) > 1f)
            direction = direction.normalized;
        Vector3 delta = direction * speed;
        var acceleration = 1f/AccelerationTime;
        if (delta == Vector3.zero)
            acceleration *= StopMultiplier;
        var velocity = Vector2.Lerp(_rb.linearVelocity, delta, acceleration * Time.deltaTime);
        if (velocity.magnitude < 0.01f)
            velocity = Vector2.zero;
        
        if (_animator?.runtimeAnimatorController != null)
        {
            var speedPercent = velocity.magnitude / Speed;
            _animator.SetFloat("Speed", speedPercent);
        }
        
        _rb.linearVelocity = velocity;
    }
}
