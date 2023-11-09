using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class ObstacleSpawner : MonoBehaviour
{
    private DiContainer _container;

    public GameObject[] obstaclePrefabs; // ����� ������� ��������

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

        // ��������� ���� ������ � ���������� ������� ������
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        // �������� ��������� ������� �������� (�� 1 �� ������� ������ spawnPoints)
        int obstaclesCount = Random.Range(1, totalSpawnPoints + 1);

        // �������� ��������� �� ���������� ������ ������
        for (int i = 0; i < obstaclesCount; i++)
        {
            // ��������� �������� ����� ������ � ��������� �����
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            // ��������� �������� ������ � ������ obstaclePrefabs
            GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            // ����������, �� � ���������� � ��� ���������� ��'������ � ������� �����
            Collider2D[] overlappingColliders = new Collider2D[1];
            int count = Physics2D.OverlapCircleNonAlloc(spawnPoint.position, 1f, overlappingColliders);

            if (count == 0 && !usedSpawnPoints.Contains(spawnPoint))
            {
                _container.InstantiatePrefab(obstacleToSpawn, spawnPoint.position, Quaternion.identity, transform);

                // ��������� ����������� ����� ������ � ��������� � ������ �� ������ ��� ������������
                availableSpawnPoints.RemoveAt(randomIndex);
                usedSpawnPoints.Add(spawnPoint);
            }
        }
    }
}