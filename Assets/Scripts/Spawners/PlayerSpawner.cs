using System;
using Services;
using Zenject;

namespace Spawners
{
    public class PlayerSpawner : Spawner
    {
        private EventService eventService;

        [Inject]
        private void Construct(EventService eventServiceReference)
        {
            eventService = eventServiceReference;
        }

        protected override void Start()
        {
            base.Start();
            eventService.GateCollected += SpawnBodies;
        }

        protected override void RemoveBodyFromList(Guid guid)
        {
            base.RemoveBodyFromList(guid);
            //TODO: if count is 0 end the game
        }

        private void OnDestroy()
        {
            eventService.GateCollected -= SpawnBodies;
        }
    }
}