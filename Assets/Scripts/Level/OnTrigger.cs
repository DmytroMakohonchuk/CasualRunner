using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class OnTrigger : MonoBehaviour
{
    [Inject] private IPlayerTouchMovement playerTouchMovement;
    [SerializeField] private Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        collider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (!playerTouchMovement.IsGameOver && other.transform.CompareTag("Player")) // Додайте перевірку на флаг
        //{
        //    collider.enabled = false;
        //    GlobalEventManager.PlayerBump();

        //    GlobalEventManager.GameOver();
        //    //playerTouchMovement.IsGameOver = true; // Змініть на true
        //}
    }
}
