using Services;
using UnityEngine;
using Zenject;

namespace Level
{
    public class Finish : MonoBehaviour
    {
        private bool onceEntered;

        private EventService eventService;

        [Inject]
        private void Construct(EventService eventServiceReference)
        {
            eventService = eventServiceReference;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (onceEntered) return;
            onceEntered = true;
            eventService.InvokePlayerWon();
        }
    }
}