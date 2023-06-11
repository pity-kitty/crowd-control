using System.Collections;
using Services;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerControl : MonoBehaviour
    {
        [Range(0, 1f)]
        [SerializeField] private float sensitivity = 0.7f;

        [SerializeField] private float leftXBoundary;
        [SerializeField] private float rightXBoundary;

        private Coroutine controlRoutine;
        private float offset;

        private EventService eventService;

        [Inject]
        private void Construct(EventService eventServiceReference)
        {
            eventService = eventServiceReference;
        }

        private void Start()
        {
            eventService.RadiusRecalculated += SetNewOffset;
        }

        public void EnableControl(bool state)
        {
            if (state)
            {
                controlRoutine = StartCoroutine(ControlPlayer());
                return;
            }
            
            if (controlRoutine != null) StopCoroutine(controlRoutine);
        }
        
        private IEnumerator ControlPlayer()
        {
            while (true)
            {
                if (Input.touches.Length > 0)
                {
                    var position = transform.position;
                    position.x += Input.touches[0].deltaPosition.x * Time.deltaTime * sensitivity;
                    position.x = Mathf.Clamp(position.x, leftXBoundary + offset, rightXBoundary - offset);
                    transform.position = position;
                }

                yield return null;
            }
        }

        private void SetNewOffset(float newOffset)
        {
            offset = newOffset;
        }

        private void OnDestroy()
        {
            eventService.RadiusRecalculated -= SetNewOffset;
        }
    }
}