using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Wof;
using Wof.InventoryManagement;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private ItemConfig ItemConfig;
    [field: SerializeField] public int Amount = 1;
    public Item Item => ItemConfig.Build();
    
    public bool CanBeCollected { get; private set; } = false;
    public UnityEvent<CollectableItem> CanCollectEvent { get; private set; } = new();

    [Header("Drop Animation")]
    [SerializeField] private float dropRadius = 1f;
    [SerializeField] private AnimationCurve dropHeightCurve;
    [SerializeField] private float dropDuration = 0.1f;

    [Header("Collect Pull")]
    [SerializeField] private float pullMaxSpeed = 10f;
    [SerializeField] private float epsilon = 0.5f;

    private Coroutine moveCoroutine;
    
    private Collider2D[] _colliders;
    
    void Awake()
    {
        _colliders = GetComponents<Collider2D>();
    }

    /// <summary>
    /// Начинает процедурную анимацию падения предмета из указанной точки.
    /// Предмет перемещается в случайную точку в пределах радиуса, подпрыгивая по заданной кривой.
    /// </summary>
    /// <param name="position">Центр выпадения (начальная позиция).</param>
    public void Drop(Vector3 position)
    {
        // Останавливаем текущую анимацию или движение
        StopAllCoroutines();
        moveCoroutine = null;
        foreach (var collider in _colliders)
            collider.enabled = true;
        
        // Помещаем объект в центр выпадения
        transform.position = position;
        
        // Генерируем случайную конечную точку в радиусе на плоскости (X и Y для 2D)
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * dropRadius;
        Vector3 targetPosition = position + new Vector3(randomCircle.x, randomCircle.y, 0f);
        
        // Запускаем корутину анимации
        StartCoroutine(AnimateDrop(position, targetPosition));
    }

    private IEnumerator AnimateDrop(Vector3 start, Vector3 end)
    {
        // На время анимации запрещаем сбор предмета
        CanBeCollected = false;
        
        float elapsedTime = 0f;
        while (elapsedTime < dropDuration)
        {
            float t = elapsedTime / dropDuration; // прогресс от 0 до 1
            
            // Горизонтальная интерполяция (X и Y для 2D)
            Vector3 horizontalPos = Vector3.Lerp(start, end, t);
            
            // Вертикальная составляющая по кривой (по оси Z для 2D или Y для 3D, здесь используется Z как высота)
            float height = dropHeightCurve.Evaluate(t);
            horizontalPos.y += height; // предполагаем, что ось высоты - Z (для 2D с видом сверху можно использовать Y, но в оригинале используется Z)
            
            transform.position = horizontalPos;
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Финальная позиция точно на земле (в конце кривая должна быть 0, но для надёжности)
        transform.position = end;
        
        // Разрешаем сбор предмета
        CanBeCollected = true;
        CanCollectEvent.Invoke(this);
    }

    /// <summary>
    /// Начинает притягивание предмета к указанной цели с ускорением.
    /// При достижении цели вызывается callback, после чего предмет может быть уничтожен.
    /// </summary>
    /// <param name="target">Целевой трансформ, к которому летит предмет.</param>
    /// <param name="callback">Функция, вызываемая при достижении цели. Возвращает true, если предмет должен быть уничтожен.</param>
    public void Collect(Transform target, Func<CollectableItem, bool> callback = null)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        
        StopAllCoroutines();
        
        moveCoroutine = StartCoroutine(MoveToTarget(target, callback));
    }

    private IEnumerator MoveToTarget(Transform target, Func<CollectableItem, bool> callback)
    {
        CanBeCollected = false;
        foreach (var collider in _colliders)
            collider.enabled = false;
        
        var speed = pullMaxSpeed * 0.1f;

        while (Vector3.Distance(transform.position, target.position) > epsilon)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                target.position,
                speed * Time.deltaTime
            );
            if (speed < pullMaxSpeed)
                speed += pullMaxSpeed * 0.1f * Time.deltaTime;
            yield return null;
        }

        transform.position = target.position;
        bool destroy = callback == null || callback(this);
        
        if (destroy)
        {
            StopAllCoroutines();
            PoolManager.Instance.ReturnObject(gameObject);
        }
        else
        {
            CanBeCollected = true;
            moveCoroutine = null;
        }
    }
}