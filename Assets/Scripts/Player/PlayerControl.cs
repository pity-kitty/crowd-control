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
        private float initialXPosition;

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

        public void ResetPosition()
        {
            transform.position = Vector3.zero;
        }
        
        private IEnumerator ControlPlayer()
        {
            while (true)
            {
#if UNITY_EDITOR
                if (Input.GetMouseButtonDown(0)) initialXPosition = Input.mousePosition.x;
                if (Input.GetMouseButton(0))
                {
                    var currentXPosition = Input.mousePosition.x;
                    var delta = currentXPosition - initialXPosition;
                    CalculatePosition(delta);
                    initialXPosition = currentXPosition;
                }
#endif
                
                if (Input.touches.Length > 0)
                {
                    CalculatePosition(Input.touches[0].deltaPosition.x);
                }

                yield return null;
            }
        }

        private void CalculatePosition(float deltaPosition)
        {
            var position = transform.position;
            position.x += deltaPosition * Time.deltaTime * sensitivity;
            position.x = Mathf.Clamp(position.x, Mathf.Min(leftXBoundary + offset, 0),  Mathf.Max(rightXBoundary - offset, 0));
            transform.position = position;
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