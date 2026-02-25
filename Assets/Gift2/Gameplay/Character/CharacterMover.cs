using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public float Speed = 5f;


    public void Move(Vector2 direction)
    {
        var speed = Speed * Time.deltaTime;
        direction = direction.normalized;
        Vector3 delta = direction * speed;
        transform.position += delta;
    }
}
