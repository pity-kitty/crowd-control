using System;
using Services;
using Zenject;

namespace Spawners
{
    public class PlayerSpawner : Spawner
    {
        private EventService eventService;
        private UserDataService userDataService;

        [Inject]
        private void Construct(EventService eventServiceReference, UserDataService userDataServiceReference)
        {
            eventService = eventServiceReference;
            userDataService = userDataServiceReference;
        }

        private void Start()
        {
            SpawnBodies(userDataService.CurrentUser.StartCrowdCount);
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