using System.Collections.Generic;
using UnityEngine;

namespace Gift2
{
    public class RespawnController : MonoBehaviour
    {
        public static RespawnController Instance { get; private set; }
        
        private class RespawnInfo
        {
            public Respawnble Respawnble;
            public float RemainingTime;
        }
        
        private List<RespawnInfo> Respawns = new();
        
        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        
        void Update()
        {
            for (int i = Respawns.Count-1; i >= 0; i--)
            {
                var respawn = Respawns[i];
                respawn.RemainingTime -= Time.deltaTime;
                if (respawn.RemainingTime < 0)
                {
                    Respawns.Remove(respawn);
                    respawn.Respawnble.Respawn();
                }
            }
        }
        
        public void AddToRespawn(Respawnble respawnble, float timeToRespawn)
        {
            Respawns.Add(new(){Respawnble = respawnble, RemainingTime = timeToRespawn});
        }
    }
}
