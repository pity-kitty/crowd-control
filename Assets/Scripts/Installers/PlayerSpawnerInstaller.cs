using Spawners;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerSpawnerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerSpawner playerSpawner;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerSpawner>().FromInstance(playerSpawner).AsSingle().NonLazy();
        }
    }
}