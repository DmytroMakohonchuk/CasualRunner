
using UnityEngine;
using Zenject;

public class CoinCollection : MonoBehaviour
{
    [Inject] private ICoinCounter coinCounter;

    public int coinValue = 1; // ʳ������ ����, ��� �� �� �������

    private void OnTriggerEnter(Collider other)
    {
        if(coinCounter != null)
        {
            if (other.CompareTag("Player")) // ��������, �� ������� ��������� � �������
            {
                Destroy(gameObject); // ������� ��'��� �������
                coinCounter.AddCoins(coinValue); // ������ ���� ������
                
            }
        }      
    }
}
