using UnityEngine;

namespace Gift2
{
    public abstract class Respawnble : MonoBehaviour
    {
        [SerializeField] private float TimeToRespawn = 300f;
        private float _timeAfterKill;
    
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
            RespawnController.Instance?.AddToRespawn(this, TimeToRespawn);
        }
    }
}
