
using UnityEngine;
using Zenject;

public class CoinCollection : MonoBehaviour
{
    [Inject] private ICoinCounter coinCounter;

    public int coinValue = 1; // Кількість очок, яку дає ця монетка

    private void OnTriggerEnter(Collider other)
    {
        if(coinCounter != null)
        {
            if (other.CompareTag("Player")) // Перевірка, чи монетка стикається з гравцем
            {
                Destroy(gameObject); // Знищити об'єкт монетки
                coinCounter.AddCoins(coinValue); // Додати очки гравцю
                
            }
        }      
    }
}
