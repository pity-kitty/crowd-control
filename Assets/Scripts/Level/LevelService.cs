using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelService : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private List<GameObject> levels;

        private Coroutine moveRoutine;
        private int currentIndex;
        private GameObject currentLevel;

        private UserDataService userDataService;
        private DiContainer diContainer;

        [Inject]
        private void Construct(UserDataService userDataServiceReference, DiContainer diContainerReference)
        {
            userDataService = userDataServiceReference;
            diContainer = diContainerReference;
        }

        private void Start()
        {
            currentIndex = userDataService.CurrentUser.Level;
            SpawnLevel(currentIndex);
        }

        private IEnumerator MoveLevel()
        {
            while (true)
            {
                var moveDirection = -Vector3.forward * (moveSpeed * Time.deltaTime);
                currentLevel.transform.Translate(moveDirection);
                yield return null;
            }
        }

        private void SpawnLevel(int index)
        {
            currentIndex = index;
            currentLevel = diContainer.InstantiatePrefab(levels[index], transform);
        }

        public void StartLevelMovement()
        {
            moveRoutine = StartCoroutine(MoveLevel());
        }

        public void StopMovement()
        {
            if (moveRoutine != null) StopCoroutine(moveRoutine);
        }

        public void ResetLevel()
        {
            transform.position = Vector3.zero;
            Destroy(currentLevel);
            SpawnLevel(currentIndex);
        }

        public void GoToNextLevel()
        {
            currentIndex++;
            currentIndex = Mathf.Min(currentIndex, levels.Count - 1);
            userDataService.CurrentUser.Level = currentIndex;
            userDataService.SaveUserData();
        }
    }
}