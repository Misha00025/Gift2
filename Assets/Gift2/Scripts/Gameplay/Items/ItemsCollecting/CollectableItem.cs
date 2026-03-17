using System.Collections;
using Gift2.Core;
using UnityEngine;
using UnityEngine.Events;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private ItemConfig ItemConfig;
    [field: SerializeField] public int Amount = 1;
    public Item Item => ItemConfig.Build();
    
    public bool CanBeCollected { get; private set; } = false;
    public UnityEvent<CollectableItem> CanCollectEvent {get; private set;} = new();

    [Header("Drop Animation")]
    [SerializeField] private float dropRadius = 1f;
    [SerializeField] private AnimationCurve dropHeightCurve;
    [SerializeField] private float dropDuration = 0.2f;

    /// <summary>
    /// Начинает процедурную анимацию падения предмета из указанной точки.
    /// Предмет перемещается в случайную точку в пределах радиуса, подпрыгивая по заданной кривой.
    /// </summary>
    /// <param name="position">Центр выпадения (начальная позиция).</param>
    public void Drop(Vector3 position)
    {
        // Останавливаем текущую анимацию, если она выполняется
        StopAllCoroutines();
        
        // Помещаем объект в центр выпадения
        transform.position = position;
        
        // Генерируем случайную конечную точку в радиусе на горизонтальной плоскости (XZ)
        Vector2 randomCircle = Random.insideUnitCircle * dropRadius;
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
            
            // Горизонтальная интерполяция (X и Z)
            Vector3 horizontalPos = Vector3.Lerp(start, end, t);
            
            // Вертикальная составляющая по кривой (по оси Y)
            float height = dropHeightCurve.Evaluate(t);
            horizontalPos.y += height;
            // Новая позиция: горизонтальная часть с высотой
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
    
    public void Collect()
    {
        Destroy(gameObject);
    }
}