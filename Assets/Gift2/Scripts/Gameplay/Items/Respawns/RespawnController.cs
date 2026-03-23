using System.Collections.Generic;
using Gift2.Core;
using UnityEngine;
using Zenject;

namespace Gift2
{
    public class RespawnController : MonoBehaviour
    {
        private class RespawnInfo
        {
            public Respawnble Respawnble;
            public float RemainingTime;
        }
        
        private List<RespawnInfo> Respawns = new();
        [Inject] private Player _player;
        
        void Update()
        {
            var deltaTime = Time.deltaTime;
                
            if (_player?.Character != null)
            {
                deltaTime *= _player.Character.Stats.ResourcesRespawnSpeed;
            }
            
            for (int i = Respawns.Count-1; i >= 0; i--)
            {
                var respawn = Respawns[i];
                
                respawn.Respawnble.AddDelta(Time.deltaTime);
                                                                
                respawn.RemainingTime -= deltaTime;
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
