using UnityEngine;

namespace Gift2
{
    public abstract class Respawnble : MonoBehaviour
    {
        [SerializeField] private float TimeToRespawn = 300f;
    
        public void Respawn()
        {
            gameObject.SetActive(true);
            OnRespawn();
        }
        
        protected abstract void OnRespawn();
        
        public void Kill()
        {
            gameObject.SetActive(false);
            RespawnController.Instance?.AddToRespawn(this, TimeToRespawn);
        }
    }
}
