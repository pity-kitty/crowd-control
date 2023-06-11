using System;
using System.Collections.Generic;
using Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gate
{
    public class MultiplyGate : MonoBehaviour
    {
        [SerializeField] private GateParameters gateParameters;
        [SerializeField] private TMP_Text valueText;

        private bool onceEntered;
        private Dictionary<MultiplyType, char> multiplyCharacter = new Dictionary<MultiplyType, char>()
        {
            { MultiplyType.Addition, '+' },
            { MultiplyType.Multiplication, 'x' }
        };

        private EventService eventService;

        [Inject]
        private void Construct(EventService eventServiceReference)
        {
            eventService = eventServiceReference;
        }

        private void Start()
        {
            var multiplyText = multiplyCharacter[gateParameters.MultiplyType] + gateParameters.Value.ToString();
            valueText.text = multiplyText;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (onceEntered) return;
            onceEntered = true;
            eventService.InvokeGateCollectedEvent(gateParameters.Value);
            Destroy(gameObject);
        }
    }
}