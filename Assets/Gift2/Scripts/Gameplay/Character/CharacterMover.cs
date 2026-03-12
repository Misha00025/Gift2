using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMover : MonoBehaviour
{
    private Character _character;
    private Rigidbody2D _rb;

    public float Acceleration = 2f;
    public float StopMultiplier = 5f;
    public float Speed => _character.Stats.MoveSpeed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Character character)
    {
        _character = character;
    }

    public void Move(Vector2 direction)
    {
        var speed = Speed;
        if ((direction.x + direction.y) > 1f)
            direction = direction.normalized;
        Vector3 delta = direction * speed;
        var acceleration = Acceleration;
        if (delta == Vector3.zero)
            acceleration *= StopMultiplier;
        var velocity = Vector2.Lerp(_rb.linearVelocity, delta, acceleration * Time.deltaTime);
        if (velocity.magnitude < 0.01f)
            velocity = Vector2.zero;
        _rb.linearVelocity = velocity;
    }
}
