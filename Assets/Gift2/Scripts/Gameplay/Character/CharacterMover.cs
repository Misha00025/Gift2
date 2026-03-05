using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    private Character _character;

    public float Speed => _character.Stats.MoveSpeed;

    public void Initialize(Character character)
    {
        _character = character;
    }


    public void Move(Vector2 direction)
    {
        var speed = Speed * Time.deltaTime;
        direction = direction.normalized;
        Vector3 delta = direction * speed;
        transform.position += delta;
    }
}
