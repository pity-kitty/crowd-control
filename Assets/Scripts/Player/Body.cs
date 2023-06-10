using System;
using UnityEngine;

namespace Player
{
    public abstract class Body : MonoBehaviour
    {
        [SerializeField] private Rigidbody modelRigidbody;

        private readonly Guid id = Guid.NewGuid();
        private Action<Guid> bodyDestroyed;

        public Guid ID => id;
        public Rigidbody Rigidbody => modelRigidbody;
        
        public event Action<Guid> BodyDestroyed
        {
            add => bodyDestroyed += value;
            remove => bodyDestroyed -= value;
        }

        private void OnDestroy()
        {
            bodyDestroyed?.Invoke(id);
        }
    }
}