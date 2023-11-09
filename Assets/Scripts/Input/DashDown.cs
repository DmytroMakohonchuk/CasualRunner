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

    private float dashDuration => playerSettings.dashDuration; // ��������� ����
    private Vector3 dashDirection => playerSettings.dashDirection;
    private float dashDistance => playerSettings.dashDistance; // ������� ���� ����

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
        isDashing = true; // ���������, �� ������� ������ ���.
        float startTime = Time.time; // ��� ������� ����.
        Vector3 startPos = transform.position; // ��������� ������� ������ ����� �����.
        Vector3 endPos = transform.position + dashDirection * dashDistance; // ʳ����� ������� ��� ����.
        
        // ���� �� ����� ��� ���� � ������� �� ����� ����������� �� ����:
        while (Time.time < startTime + dashDuration && !isGrounded) //TODO fix ||
        {
            float progress = (Time.time - startTime) / dashDuration; // ���������� ������� ���� (�������� �� 0 �� 1).
            transform.position = Vector3.Lerp(startPos, endPos, progress); // ������������ �� ���������� � ������� ��������� ��� �������� ���� ������.
            yield return null; // �������� ���� �������� ����� ����� ������������.
        }
        requestDash = false;
        isDashing = false; // ���������, �� ��� ���������.
    }
}
