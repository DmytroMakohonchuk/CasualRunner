using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class ObstacleSpawner : MonoBehaviour
{
    private DiContainer _container;

    public GameObject[] obstaclePrefabs; // Масив префабів перешкод

    public Transform leftSpawn;
    public Transform rightSpawn;
    public Transform frontSpawn;

    private List<Transform> usedSpawnPoints = new List<Transform>();

    private void Start()
    {
        SpawnObstacle();
    }

    [Inject]
    private void Constructor(DiContainer container)
    {
        _container = container;
    }

    private void SpawnObstacle()
    {
        Transform[] spawnPoints = { leftSpawn, rightSpawn, frontSpawn };
        int totalSpawnPoints = spawnPoints.Length;

        // Створюємо копію масиву з доступними точками спавну
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        // Рандомно визначаємо кількість перешкод (від 1 до довжини масиву spawnPoints)
        int obstaclesCount = Random.Range(1, totalSpawnPoints + 1);

        // Спавнимо перешкоди на випадкових точках спавну
        for (int i = 0; i < obstaclesCount; i++)
        {
            // Випадково вибираємо точку спавну з доступних точок
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            // Випадково вибираємо префаб з масиву obstaclePrefabs
            GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            // Перевіряємо, чи є перекриття з вже спавнутими об'єктами у деякому радіусі
            Collider2D[] overlappingColliders = new Collider2D[1];
            int count = Physics2D.OverlapCircleNonAlloc(spawnPoint.position, 1f, overlappingColliders);

            if (count == 0 && !usedSpawnPoints.Contains(spawnPoint))
            {
                _container.InstantiatePrefab(obstacleToSpawn, spawnPoint.position, Quaternion.identity, transform);

                // Видаляємо використану точку спавну з доступних і додаємо до списку вже використаних
                availableSpawnPoints.RemoveAt(randomIndex);
                usedSpawnPoints.Add(spawnPoint);
            }
        }
    }
}