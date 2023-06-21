using System;
using System.Collections;
using Services;
using Spawners;
using UnityEngine;

namespace Player
{
    public abstract class Body : MonoBehaviour
    {
        private const string RunningParameterName = "IsRunning";
        private const string CheeringParameterName = "IsCheering";

        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator bodyAnimator;
        [SerializeField] private float moveSpeed = 0.5f;
        [SerializeField] private float distanceCoefficient = 3f;

        private readonly Guid id = Guid.NewGuid();
        private readonly int runningAnimationParameterID = Animator.StringToHash(RunningParameterName);
        private readonly int cheeringAnimationParameterID = Animator.StringToHash(CheeringParameterName);
        private EventService eventService;
        private Spawner spawner;
        private Action<Body> killAction;
        private float distanceThreshold;

        protected Spawner Spawner => spawner;
        public Guid ID => id;
        public PointPosition PointPosition;

        private void Start()
        {
            distanceThreshold = characterController.radius * 2f * distanceCoefficient;
        }

        public void InitializeSubscriptions()
        {
            eventService.FightFinished += StopBody;
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

        public void SetIdleAnimation()
        {
            bodyAnimator.SetBool(runningAnimationParameterID, false);
            bodyAnimator.SetBool(cheeringAnimationParameterID, false);
        }

        public void SetRunningAnimation()
        {
            bodyAnimator.SetBool(runningAnimationParameterID, true);
        }

        public void SetCheeringAnimation()
        {
            bodyAnimator.SetBool(cheeringAnimationParameterID, true);
            bodyAnimator.SetBool(runningAnimationParameterID, false);
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
            eventService.FightFinished -= StopBody;
        }

        public void StopBody()
        {
            StopAllCoroutines();
        }
    }
}