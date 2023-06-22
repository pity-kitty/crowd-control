using System.Collections.Generic;
using Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gate
{
    public class MultiplyGate : MonoBehaviour
    {
        [SerializeField] private MultiplyType multiplyType;
        [SerializeField] private int multiplyValue;
        [SerializeField] private TMP_Text valueText;
        [SerializeField] private Collider fieldCollider;

        private bool onceEntered;

        private Dictionary<MultiplyType, char> multiplyCharacter = new()
        {
            { MultiplyType.Addition, '+' },
            { MultiplyType.Multiplication, 'x' }
        };

        private EventService eventService;

        public Collider FieldCollider => fieldCollider;
        public bool OnceEntered => onceEntered;

        [Inject]
        private void Construct(EventService eventServiceReference)
        {
            eventService = eventServiceReference;
        }

        private void Start()
        {
            var multiplyText = multiplyCharacter[multiplyType] + multiplyValue.ToString();
            valueText.text = multiplyText;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (onceEntered) return;
            onceEntered = true;
            eventService.InvokeGateCollectedEvent(multiplyType, multiplyValue);
            Destroy(gameObject);
        }
    }
}