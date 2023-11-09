using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CoinsDropEvent : MonoBehaviour
{
    [Inject] private ICoinCounter coinCounter;
    public Coin coinPrefab; // Префаб монетки
    public Transform playerTransform; // Позиція гравця
    public int numberOfCoins = 4; // Кількість монеток, які будуть спавнитися
    public float spreadRadius = 2.0f; // Радіус розкиду монеток
    public float coinDisappearTime = 2.0f; // Час зникнення монеток
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
        // Видалити всі попередні монетки перед спавном нових
        //DestroyPreviousCoins();

        // Спавнити монетки у позиції гравця з розкидом
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
            Destroy(coin.gameObject); // Видаляємо саму монетку
            spawnedCoins.Remove(coin);
        }
    }

    public void DestroyPreviousCoins()
    {
        foreach (Coin coin in spawnedCoins)
        {
            Destroy(coin.gameObject); // Видаляємо саму монетку
        }
        spawnedCoins.Clear();
    }
}
