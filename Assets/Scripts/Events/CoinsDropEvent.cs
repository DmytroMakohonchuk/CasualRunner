using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CoinsDropEvent : MonoBehaviour
{
    [Inject] private ICoinCounter coinCounter;
    public Coin coinPrefab; // ������ �������
    public Transform playerTransform; // ������� ������
    public int numberOfCoins = 4; // ʳ������ �������, �� ������ ����������
    public float spreadRadius = 2.0f; // ����� ������� �������
    public float coinDisappearTime = 2.0f; // ��� ��������� �������
    private List<Coin> spawnedCoins = new List<Coin>();


    // Start is called before the first frame update
    void Start()
    {
        GlobalEventManager.OnPlayerBump += DropCoins;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DropCoins()
    {
        DestroyPreviousCoins();
        SpawnCoins();
    }

    public void SpawnCoins()
    {
        // �������� �� �������� ������� ����� ������� �����
        //DestroyPreviousCoins();

        // �������� ������� � ������� ������ � ��������
        for (int i = 0; i < numberOfCoins; i++)
        {
            Vector3 spawnPosition = playerTransform.position + Random.insideUnitSphere * spreadRadius;
            Coin newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            spawnedCoins.Add(newCoin);
            ThrowCoinsAround();
            StartCoroutine(DisappearCoin(newCoin));
        }
    }

    private void ThrowCoinsAround()
    {
        foreach (var coin in spawnedCoins)
        {
            coin.AddRandomImpulse();
        }
    }

    IEnumerator DisappearCoin(Coin coin)
    {
        yield return new WaitForSeconds(coinDisappearTime);

        if (coin != null)
        {
            Destroy(coin.gameObject); // ��������� ���� �������
            spawnedCoins.Remove(coin);
        }
    }

    public void DestroyPreviousCoins()
    {
        foreach (Coin coin in spawnedCoins)
        {
            Destroy(coin.gameObject); // ��������� ���� �������
        }
        spawnedCoins.Clear();
    }
}
