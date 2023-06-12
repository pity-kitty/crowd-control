using System;
using System.Collections;
using UnityEngine;

namespace Animations
{
    public class HammerAnimation : MonoBehaviour
    {
        [SerializeField] private float animationTime = 1.5f;
        [SerializeField] private float hitInterval = 3f;
        [SerializeField] private float delayBetweenAnimation = 1f;
        [SerializeField] private Transform hammerTransform;
        [SerializeField] private float endRotationAngle = -90f;

        private void Start()
        {
            StartCoroutine(Animate());
        }
        
        private IEnumerator Animate()
        {
            while (true)
            {
                yield return HitAnimation(0f, endRotationAngle, EaseOutBounce);
                yield return new WaitForSeconds(delayBetweenAnimation);
                yield return HitAnimation(endRotationAngle, 0f, NoEase);
                yield return new WaitForSeconds(hitInterval);
            }
        }

        private IEnumerator HitAnimation(float startAngle, float endRotation, Func<float, float> easeFunction)
        {
            var eulerAngle = hammerTransform.eulerAngles;
            for (float i = 0; i < 1; i += Time.deltaTime / animationTime)
            {
                eulerAngle.z = Mathf.Lerp(startAngle, endRotation, easeFunction(i));
                hammerTransform.eulerAngles = eulerAngle;
                yield return null;
            }

            eulerAngle.z = endRotation;
            hammerTransform.eulerAngles = eulerAngle;
        }

        private float NoEase(float x)
        {
            return x;
        }

        private float EaseOutBounce(float x)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (x < 1 / d1)
            {
                return n1 * x * x;
            }

            if (x < 2 / d1)
            {
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            }

            if (x < 2.5 / d1)
            {
                return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            }

            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }
}