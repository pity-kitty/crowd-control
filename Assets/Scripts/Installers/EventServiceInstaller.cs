using Services;
using UnityEngine;
using Zenject;

public class EventServiceInstaller : MonoInstaller
{
    [SerializeField] private EventService eventServicePrefab;
    
    public override void InstallBindings()
    {
        Container.Bind<EventService>().FromComponentInNewPrefab(eventServicePrefab).AsSingle().NonLazy();
    }
}