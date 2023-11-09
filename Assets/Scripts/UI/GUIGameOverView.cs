using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GUIGameOverView : MonoBehaviour
{
    public event Action OnHide;
    public event Action OnShow;

    [SerializeField] private GameObject _GameOverPanel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField, Range(0f, 1f)]
    private float _tweenEndValue = 0f;

    [SerializeField]
    private float _tweenDuration = 0.5f;

    private GUIGameOverModel _model;
    public GUIGameOverModel Model => _model;

    public void ShowGameOverPanel(bool active)
    {
        //_GameOverPanel.SetActive(active);
    }

    private void Start()
    {
        _canvasGroup.alpha = 0f;
    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        //DOTween.Kill(transform);
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void Hide()
    {
        _canvasGroup.DOFade(_tweenEndValue, _tweenDuration).OnComplete(OnHideHandler);
    }

    private void OnHideHandler()
    {
        gameObject.SetActive(false);
        OnHide?.Invoke();
    }

    private void Show()
    {
        _canvasGroup.DOFade(1, _tweenDuration).OnComplete(OnShowHandler);
    }

    public void SetActive(bool active)
    {
        DOTween.Kill(_canvasGroup);

        if (active)
        {
            Show();
        }

        else
        {
            Hide();
        }
    }

    private void OnShowHandler()
    {
        _canvasGroup.interactable = true;
        OnShow?.Invoke();
    }

    private void Subscribe()
    {
        //_restartButton.onClick.AddListener(_model.OnRestartPressedHandler);

    }

    private void UnSubscribe()
    {
        _restartButton.onClick.RemoveListener(_model.OnRestartPressedHandler);
    }

    private void Init()
    {
        _model = new GUIGameOverModel();
    }

    private void Update()
    {

    }
}
