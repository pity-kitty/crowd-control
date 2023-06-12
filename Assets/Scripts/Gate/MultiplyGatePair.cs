using UnityEngine;

namespace Gate
{
    public class MultiplyGatePair : MultiplyGate
    {
        [SerializeField] private MultiplyGate secondGate;

        protected override void OnTriggerEnter(Collider other)
        {
            if (secondGate.OnceEntered) return;
            secondGate.FieldCollider.enabled = false;
            base.OnTriggerEnter(other);
        }
    }
}