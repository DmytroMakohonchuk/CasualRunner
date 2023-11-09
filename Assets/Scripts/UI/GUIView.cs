using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIView : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _jumpButton;
    [SerializeField] private Button _bendButton;
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField, Range(0f, 1f)]
    private float _tweenEndValue = 0f;

    [SerializeField]
    private float _tweenDuration = 0.5f;

    private GUIModel _model;
    public GUIModel Model => _model;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        Show();
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }


    public void Hide()
    {
        //DisableInteractable();
        _canvasGroup.DOFade(_tweenEndValue, _tweenDuration / 2).OnComplete(() => gameObject.SetActive(false));
    }

    private void DisableInteractable()
    {
        _canvasGroup.interactable = false;
    }

    public void Show()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.DOFade(1, _tweenDuration).OnComplete(() => _canvasGroup.interactable = true);
    }

    private void Subscribe()
    {
        //_pauseButton.onClick.AddListener(_model.OnPausePressedHandler);
        //_jumpButton.onClick.AddListener(_model.OnJumpPressedHandler);
        //_bendButton.onClick.AddListener(_model.OnBendPressedHandler);
    }

    private void UnSubscribe()
    {
        //_pauseButton.onClick.RemoveListener(_model.OnPausePressedHandler);
        //_jumpButton.onClick.RemoveListener(_model.OnJumpPressedHandler);
        //_bendButton.onClick.RemoveListener(_model.OnBendPressedHandler);
    }

    private void Init()
    {
        _model = new GUIModel();
    }
}
