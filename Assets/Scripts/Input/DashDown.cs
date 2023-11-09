using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DashDown : MonoBehaviour
{
    [Inject] PlayerController playerController;
    //[Inject] PlayerMovement _playerMovement;
    public PlayerSettingsNew playerSettings;
    [SerializeField] GameObject _bendCollider;
    [SerializeField] GameObject _fullBodyCollider;
    Rigidbody _rb;

    public bool isDashing;
    public bool isGrounded;

    private float dashDuration => playerSettings.dashDuration; // тривалість дешу
    private Vector3 dashDirection => playerSettings.dashDirection;
    private float dashDistance => playerSettings.dashDistance; // відстань дешу вниз

    private bool requestDash;

    private void Start()
    {
        playerController.OnDash += RequestDash;
        _rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        isGrounded = playerController.IsGrounded;
        DashButtonClicked();
    }

    
    private void Update()
    {
        
    }

    private void RequestDash(bool isDashing)
    {
        requestDash = isDashing;
    }

    public void DashButtonClicked()
    {
        if (!isDashing && requestDash && !isGrounded)
        {
            StartCoroutine(Dash());
            //StartCoroutine(_playerMovement.SwitchColliderWithDelay());
            //StartCoroutine(SwitchColliderWithDelay());
        }
    }

    private IEnumerator Dash() //TODO fix dash stops animation
    {
        Debug.Log("!!!");
        isDashing = true; // Позначаємо, що гравець робить деш.
        float startTime = Time.time; // Час початку дешу.
        Vector3 startPos = transform.position; // Початкова позиція гравця перед дешем.
        Vector3 endPos = transform.position + dashDirection * dashDistance; // Кінцева позиція для дешу.
        
        // Поки не минув час дешу і гравець не знову знаходиться на землі:
        while (Time.time < startTime + dashDuration && !isGrounded) //TODO fix ||
        {
            float progress = (Time.time - startTime) / dashDuration; // Обчислюємо прогрес дешу (значення від 0 до 1).
            transform.position = Vector3.Lerp(startPos, endPos, progress); // Інтерполяція між початковою і кінцевою позиціями для плавного руху гравця.
            yield return null; // Почекати одну ітерацію циклу перед продовженням.
        }
        requestDash = false;
        isDashing = false; // Позначаємо, що деш завершено.
    }
}
