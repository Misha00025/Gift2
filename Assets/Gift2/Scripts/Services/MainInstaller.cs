using Gift2.Core;
using Zenject;

namespace Gift2
{
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        public override void InstallBindings()
        {
            var player = GetComponent<Player>();
            var respawnController = GetComponent<RespawnController>();
            
            Container.Bind<Player>().FromInstance(player).AsSingle();
            Container.Bind<RespawnController>().FromInstance(respawnController).AsSingle();
        }
        
    }
}
