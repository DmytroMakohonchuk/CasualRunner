using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MovementController : MonoBehaviour, IMovementController
{
    [Inject]
    private IPlayerTouchMovement _playerMovement;

    [SerializeField]
    private Button jumpButton;

    [SerializeField]
    private Button bendButton;

    private void Start()
    {
        jumpButton.onClick.AddListener(JumpButtonClicked);

        bendButton.onClick.AddListener(BendButtonClicked);

        bendButton.onClick.AddListener(DashButtonClicked);
    }

    private void JumpButtonClicked()
    {
        _playerMovement.JumpButtonClicked();
    }

    private void BendButtonClicked()
    {
        _playerMovement.BendButtonClicked();
    }

    private void DashButtonClicked()
    {
        _playerMovement.DashButtonClicked();
    }

}
