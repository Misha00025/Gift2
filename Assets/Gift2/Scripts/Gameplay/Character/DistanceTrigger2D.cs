using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DistanceTrigger2D : MonoBehaviour
{
    public float ThresholdDistance = 0.5f;
    public UnityEvent<GameObject> OnThresholdReached = new();
    public List<GameObject> Ignore = new();

    [SerializeField] private int _batchSize = 20; // количество объектов за кадр

    private class TrackedObject
    {
        public Vector2 previousLocalPosition;
        public float accumulatedDistance;
    }

    private Dictionary<GameObject, TrackedObject> _trackedObjects = new();
    private List<GameObject> _trackedKeys = new();          // кэшированный список ключей для итерации
    private int _nextIndex = 0;                              // следующий индекс для обработки
    private bool _keysDirty = true;                          // флаг необходимости обновить список ключей

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;

        if (!_trackedObjects.ContainsKey(obj) && !Ignore.Contains(obj))
        {
            var data = new TrackedObject
            {
                previousLocalPosition = transform.InverseTransformPoint(obj.transform.position),
                accumulatedDistance = 0f
            };
            _trackedObjects.Add(obj, data);
            _keysDirty = true;   // состав ключей изменился
            OnThresholdReached.Invoke(obj); // оригинал вызывает событие сразу при входе
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (_trackedObjects.Remove(obj))
        {
            _keysDirty = true;   // состав ключей изменился
        }
    }

    private void Update()
    {
        if (_trackedObjects.Count == 0) return;

        // Обновляем список ключей, если были изменения
        if (_keysDirty)
        {
            _trackedKeys.Clear();
            _trackedKeys.AddRange(_trackedObjects.Keys);
            _nextIndex = 0;
            _keysDirty = false;
        }

        // Определяем, сколько объектов обработать в этом кадре
        int objectsToProcess = Mathf.Min(_batchSize, _trackedKeys.Count - _nextIndex);
        if (objectsToProcess <= 0 && _trackedKeys.Count > 0)
        {
            // Дошли до конца списка – начинаем сначала
            _nextIndex = 0;
            objectsToProcess = Mathf.Min(_batchSize, _trackedKeys.Count);
        }

        if (objectsToProcess == 0) return;

        List<GameObject> toInvoke = null;
        List<GameObject> toRemove = null;

        int endIndex = _nextIndex + objectsToProcess;
        for (int i = _nextIndex; i < endIndex; i++)
        {
            GameObject obj = _trackedKeys[i];

            // Объект мог быть удалён из словаря (например, через OnTriggerExit2D) после обновления ключей
            if (!_trackedObjects.TryGetValue(obj, out TrackedObject data))
                continue;

            if (obj == null || !obj.activeInHierarchy)
            {
                (toRemove ??= new List<GameObject>()).Add(obj);
                continue;
            }

            Vector2 currentLocalPosition = transform.InverseTransformPoint(obj.transform.position);
            float distanceDelta = Vector2.Distance(currentLocalPosition, data.previousLocalPosition);
            data.accumulatedDistance += distanceDelta;

            while (data.accumulatedDistance >= ThresholdDistance)
            {
                (toInvoke ??= new List<GameObject>()).Add(obj);
                data.accumulatedDistance -= ThresholdDistance;
            }

            data.previousLocalPosition = currentLocalPosition;
        }

        // Сдвигаем индекс для следующего кадра
        _nextIndex += objectsToProcess;
        if (_nextIndex >= _trackedKeys.Count)
            _nextIndex = 0;

        // Вызываем события
        if (toInvoke != null)
        {
            foreach (var obj in toInvoke)
                OnThresholdReached?.Invoke(obj);
        }

        // Удаляем мёртвые/неактивные объекты
        if (toRemove != null)
        {
            foreach (var obj in toRemove)
                _trackedObjects.Remove(obj);
            _keysDirty = true; // после удаления ключи устарели
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
    }
}