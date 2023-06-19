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
        
        [SerializeField] private Animator bodyAnimator;

        private readonly Guid id = Guid.NewGuid();
        private readonly int runningAnimationParameterID = Animator.StringToHash(RunningParameterName);
        private readonly int cheeringAnimationParameterID = Animator.StringToHash(CheeringParameterName);
        private EventService eventService;
        private Action<Body> killAction;

        [HideInInspector]
        public bool CanDie = true;
        [HideInInspector]
        public bool CanKill = true;
        public Guid ID => id;

        private void Start()
        {
            eventService.FightStarted += StopMove;
            eventService.FightFinished += OnFightEnd;
            eventService.PlayerWon += StopMove;
        }

        public void Initialize(EventService eventServiceReference, Action<Body> onKill)
        {
            eventService = eventServiceReference;
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

        public void MoveConstantly()
        {
            
        }

        public void StopMove()
        {
            
        }

        private void OnFightEnd(bool state)
        {
            MoveConstantly();
        }

        // private void OnDestroy()
        // {
        //     KillBody();
        // }
        //
        // private void OnDisable()
        // {
        //     KillBody();
        // }

        protected void KillBody()
        {
            killAction(this);
            eventService.FightStarted -= StopMove;
            eventService.FightFinished -= OnFightEnd;
            eventService.PlayerWon -= StopMove;
        }

        public void StopBody()
        {
            StopAllCoroutines();
        }
    }
}