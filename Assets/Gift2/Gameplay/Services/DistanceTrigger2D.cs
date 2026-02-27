using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DistanceTrigger2D : MonoBehaviour
{
    public float ThresholdDistance = 0.5f;
    public UnityEvent<GameObject> OnThresholdReached = new();
    public List<GameObject> Ignore = new();

    private class TrackedObject
    {
        public Vector2 previousLocalPosition;
        public float accumulatedDistance;
    }

    void Awake()
    {
        OnThresholdReached.AddListener((e) => Debug.Log($"Triggered: {e.name}"));
    }

    private Dictionary<GameObject, TrackedObject> _trackedObjects = new Dictionary<GameObject, TrackedObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;

        if (!_trackedObjects.ContainsKey(obj) && Ignore.Contains(obj) == false)
        {
            TrackedObject data = new TrackedObject
            {
                previousLocalPosition = transform.InverseTransformPoint(obj.transform.position),
                accumulatedDistance = 0f
            };
            _trackedObjects.Add(obj, data);
            OnThresholdReached.Invoke(obj);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (_trackedObjects.ContainsKey(obj))
        {
            _trackedObjects.Remove(obj);
        }
    }

    private void Update()
    {
        List<GameObject> toRemove = null;

        foreach (var kvp in _trackedObjects)
        {
            GameObject obj = kvp.Key;
            TrackedObject data = kvp.Value;

            if (obj == null || !obj.activeInHierarchy)
            {
                if (toRemove == null) toRemove = new List<GameObject>();
                toRemove.Add(obj);
                continue;
            }

            Vector2 currentLocalPosition = transform.InverseTransformPoint(obj.transform.position);
            float distanceDelta = Vector2.Distance(currentLocalPosition, data.previousLocalPosition);
            data.accumulatedDistance += distanceDelta;

            while (data.accumulatedDistance >= ThresholdDistance)
            {
                OnThresholdReached?.Invoke(obj);
                data.accumulatedDistance -= ThresholdDistance;
            }

            data.previousLocalPosition = currentLocalPosition;
        }

        if (toRemove != null)
        {
            foreach (var obj in toRemove)
            {
                _trackedObjects.Remove(obj);
            }
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