using UnityEngine;

namespace Gift2
{
    public abstract class Respawnble : MonoBehaviour
    {
        [SerializeField] private float TimeToRespawn = 300f;
        [Range(0f, 1f)]
        [SerializeField] private float RandomDeltaPercent = 0.1f;
        [SerializeField] private Vector2 RandomOffset = Vector2.one;
        private float _timeAfterKill;
        private float RandomDelta => TimeToRespawn * RandomDeltaPercent;
        private Vector3 _startPosition = Vector3.zero;
    
        private Vector3 GetRandomPosition()
        {
            var position = _startPosition;
            var delta = Vector3.zero;
            delta.x = Random.Range(-RandomOffset.x, RandomOffset.x);
            delta.y = Random.Range(-RandomOffset.y, RandomOffset.y);
            position = position + delta;
            return position;
        }
    
        public void Respawn()
        {
            if (_startPosition == Vector3.zero)
                _startPosition = transform.position;
            Debug.Log($"{name} respawned after {_timeAfterKill} seconds");
            transform.position = GetRandomPosition();
            gameObject.SetActive(true);
            OnRespawn();
        }
        
        public void AddDelta(float delta)
        {
            _timeAfterKill += delta;
        }
        
        protected abstract void OnRespawn();
        
        public void Kill()
        {
            _timeAfterKill = 0f;
            gameObject.SetActive(false);
            var timeToRespawn = TimeToRespawn;
            timeToRespawn += Random.Range(-RandomDelta, RandomDelta);
            RespawnController.Instance?.AddToRespawn(this, timeToRespawn);
        }
    }
}
