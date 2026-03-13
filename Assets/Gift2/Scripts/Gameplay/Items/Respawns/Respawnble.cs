using UnityEngine;

namespace Gift2
{
    public abstract class Respawnble : MonoBehaviour
    {
        [SerializeField] private float TimeToRespawn = 300f;
        [Range(0f, 1f)]
        [SerializeField] private float RandomDeltaPercent = 0.1f;
        private float _timeAfterKill;
        private float RandomDelta => TimeToRespawn * RandomDeltaPercent;
    
        public void Respawn()
        {
            Debug.Log($"{name} respawned after {_timeAfterKill} seconds");            
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
