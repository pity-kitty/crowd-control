using System.Collections;
using UnityEngine;

namespace Animations
{
    public class RotateAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 rotationAxis;
        [SerializeField] private float angle;

        private void Start()
        {
            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            while (true)
            {
                transform.Rotate(rotationAxis, angle * Time.deltaTime);
                yield return null;
            }
        }
    }
}