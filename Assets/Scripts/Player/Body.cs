using System;
using UnityEngine;

namespace Player
{
    public abstract class Body : MonoBehaviour
    {
        private const string RunningParameterName = "IsRunning";
        private const string CheeringParameterName = "IsCheering";
        
        [SerializeField] private Rigidbody modelRigidbody;
        [SerializeField] private Animator bodyAnimator;

        private readonly Guid id = Guid.NewGuid();
        private Action<Guid> bodyDestroyed;
        public readonly int RunningAnimationParameterID = Animator.StringToHash(RunningParameterName);
        public readonly int CheeringAnimationParameterID = Animator.StringToHash(CheeringParameterName);

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
            bodyAnimator.SetBool(RunningAnimationParameterID, false);
            bodyAnimator.SetBool(CheeringAnimationParameterID, false);
        }

        public void SetRunningAnimation()
        {
            bodyAnimator.SetBool(RunningAnimationParameterID, true);
        }

        public void SetCheeringAnimation()
        {
            bodyAnimator.SetBool(CheeringAnimationParameterID, true);
            bodyAnimator.SetBool(RunningAnimationParameterID, false);
        }

        private void OnDestroy()
        {
            bodyDestroyed?.Invoke(id);
        }
    }
}