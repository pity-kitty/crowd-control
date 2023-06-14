using System;
using System.Collections;
using Services;
using UnityEngine;

namespace Player
{
    public abstract class Body : MonoBehaviour
    {
        private const string RunningParameterName = "IsRunning";
        private const string CheeringParameterName = "IsCheering";
        
        [SerializeField] private Rigidbody modelRigidbody;
        [SerializeField] private Animator bodyAnimator;
        [SerializeField] private float moveInterval = 0.1f;

        private readonly Guid id = Guid.NewGuid();
        private readonly int runningAnimationParameterID = Animator.StringToHash(RunningParameterName);
        private readonly int cheeringAnimationParameterID = Animator.StringToHash(CheeringParameterName);
        private Action<Guid> bodyDestroyed;
        private Coroutine moveCoroutine;
        private EventService eventService;

        [HideInInspector]
        public bool CanDie = true;
        [HideInInspector]
        public bool CanKill = true;
        public Guid ID => id;
        public Rigidbody Rigidbody => modelRigidbody;
        
        public event Action<Guid> BodyDestroyed
        {
            add => bodyDestroyed += value;
            remove => bodyDestroyed -= value;
        }

        private void Start()
        {
            eventService.FightStarted += StopMove;
            eventService.FightFinished += OnFightEnd;
            eventService.PlayerWon += StopMove;
        }

        public void SetEventService(EventService eventServiceReference) => eventService = eventServiceReference;

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

        public void MoveConstantly()
        {
            moveCoroutine = StartCoroutine(Move());
        }

        public void StopMove()
        {
            StopCoroutine(moveCoroutine);
        }

        private IEnumerator Move()
        {
            while (true)
            {
                var bodyTransform = transform;
                var direction = bodyTransform.parent.position - bodyTransform.position;
                modelRigidbody.AddForce(direction, ForceMode.Impulse);
                yield return new WaitForSeconds(moveInterval);
            }
        }

        private void OnFightEnd(bool state)
        {
            MoveConstantly();
        }

        private void OnDestroy()
        {
            bodyDestroyed?.Invoke(id);
            eventService.FightStarted -= StopMove;
            eventService.FightFinished -= OnFightEnd;
            eventService.PlayerWon -= StopMove;
        }
    }
}