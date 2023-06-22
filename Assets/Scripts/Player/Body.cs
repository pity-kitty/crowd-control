using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using Spawners;
using UnityEngine;

namespace Player
{
    public abstract class Body : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private AnimationInstancing animationInstancing;
        [SerializeField] private float moveSpeed = 0.5f;
        [SerializeField] private float distanceCoefficient = 3f;

        private readonly Guid id = Guid.NewGuid();
        private EventService eventService;
        private Spawner spawner;
        private Action<Body> killAction;
        private float distanceThreshold;
        private PlayerAnimation animationToApply;
        private Dictionary<PlayerAnimation, int> animationIndices = new()
        {
            { PlayerAnimation.Idle , 2 },
            { PlayerAnimation.Running , 1 },
            { PlayerAnimation.Cheering , 0 }
        };

        protected Spawner Spawner => spawner;
        public Guid ID => id;
        public PointPosition PointPosition;

        public PlayerAnimation AnimationToApply
        {
            get => animationToApply;
            set
            {
                animationToApply = value;
                SetAnimation(animationToApply);
            }
        }

        private void Start()
        {
            distanceThreshold = characterController.radius * 2f * distanceCoefficient;
        }

        public void InitializeSubscriptions()
        {
            eventService.FightFinished += StopBody;
            animationInstancing.AnimationPrepared += ApplyAnimationOnStart;
        }

        private void UnsubscribeEvents()
        {
            eventService.FightFinished -= StopBody;
            animationInstancing.AnimationPrepared -= ApplyAnimationOnStart;
        }

        public void Initialize(EventService eventServiceReference, Spawner spawnerReference, Action<Body> onKill)
        {
            eventService = eventServiceReference;
            spawner = spawnerReference;
            killAction = onKill;
        }

        public void SetAnimation(PlayerAnimation playerAnimation)
        {
            switch (playerAnimation)
            {
                case PlayerAnimation.Idle:
                    SetIdleAnimation();
                    break;
                case PlayerAnimation.Running:
                    SetRunningAnimation();
                    break;
                case PlayerAnimation.Cheering:
                    SetCheeringAnimation();
                    break;
            }
        }

        private void ApplyAnimationOnStart()
        {
            SetAnimation(animationToApply);
        }

        public void SetIdleAnimation()
        {
            animationInstancing.PlayAnimation(animationIndices[PlayerAnimation.Idle]);
        }

        public void SetRunningAnimation()
        {
            animationInstancing.PlayAnimation(animationIndices[PlayerAnimation.Running]);
        }

        public void SetCheeringAnimation()
        {
            animationInstancing.PlayAnimation(animationIndices[PlayerAnimation.Cheering]);
        }

        public void Move(Vector3 position, bool canDie = false)
        {
            StartCoroutine(MoveCoroutine(position, canDie));
        }

        private IEnumerator MoveCoroutine(Vector3 position, bool canDie)
        {
            var distance = float.MaxValue;
            while (distance > distanceThreshold)
            {
                var bodyPosition = transform.position;
                var direction = (position - bodyPosition).normalized;
                characterController.Move(direction * (Time.deltaTime * moveSpeed));
                distance = Vector3.Distance(position, bodyPosition);
                yield return null;
            }
            if (canDie) KillBody();
        }

        protected void KillBody()
        {
            killAction(this);
            UnsubscribeEvents();
        }

        public void StopBody()
        {
            StopAllCoroutines();
        }
    }
}