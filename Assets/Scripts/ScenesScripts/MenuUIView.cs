using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuUIView : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _tweenDuration = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _tweenEndValue = 0f;

    private MenuUIModel _model;
    public MenuUIModel Model => _model;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _model = new MenuUIModel();
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

    public void Show()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.DOFade(1, _tweenDuration).OnComplete(() => _canvasGroup.interactable = true);
    }

    public void Hide()
    {
        //DisableInteractable();
        _canvasGroup.DOFade(_tweenEndValue, _tweenDuration / 2).OnComplete(() => gameObject.SetActive(false));
    }

    private void Subscribe()
    {
        _playButton.onClick.AddListener(_model.OnPlayPressedHandler);

    }

    private void UnSubscribe()
    {
        _playButton.onClick.RemoveListener(_model.OnPlayPressedHandler);
    }

}
