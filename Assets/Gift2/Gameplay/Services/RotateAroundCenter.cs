using UnityEngine;

public class RotateAroundCenter : MonoBehaviour
{
    [Header("Настройки расположения")]
    [Tooltip("Радиус окружности")]
    public float Radius = 5f;

    [Tooltip("Массив объектов для вращения. Если не заполнен, будут использованы дочерние объекты.")]
    public Transform[] Objects;

    [Header("Настройки вращения")]
    [Tooltip("Скорость вращения всей группы (градусов в секунду)")]
    public float RotationSpeed = 30f;

    [Tooltip("Поворачивать ли каждый объект лицевой стороной от центра (например, чтобы всегда смотрели наружу)")]
    public bool FaceAwayFromCenter = false;
    private float _currentAngle = 0f;
    private float _angleStep;
    private Character _character;

    void Start()
    {
        if (Objects == null || Objects.Length == 0)
        {
            Transform[] children = GetComponentsInChildren<Transform>();
            Objects = System.Array.FindAll(children, child => child != transform && child.parent == transform);
        }

        if (Objects.Length == 0)
        {
            Debug.LogWarning("Нет объектов для вращения!");
            enabled = false;
            return;
        }

        _angleStep = 360f / Objects.Length;

        PositionObjects();
    }

    public void Initialize(Character character)
    {
        _character = character;
    }

    private void UseStats()
    {
        Radius = _character.Stats.RotationRadius;
        RotationSpeed = _character.Stats.RotationSpeed;
    }

    void Update()
    {
        _currentAngle += RotationSpeed * Time.deltaTime;
        if (_character != null)
            UseStats();
        PositionObjects();
    }

    private void PositionObjects()
    {
        Vector3 center = transform.position;

        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i] == null) continue;

            float angle = _currentAngle + i * _angleStep;

            float rad = angle * Mathf.Deg2Rad;

            float x = center.x + Radius * Mathf.Cos(rad);
            float y = center.y + Radius * Mathf.Sin(rad);
            Vector3 newPos = new Vector3(x, y, center.z);

            Objects[i].position = newPos;

            if (FaceAwayFromCenter)
            {
                Vector3 direction = (newPos - center).normalized;

                if (direction != Vector3.zero)
                {
                    float angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Objects[i].rotation = Quaternion.Euler(0, 0, angleDeg);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}