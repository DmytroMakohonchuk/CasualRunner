using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MenuUIController
{
    [Inject] private MenuUIProvider menuUIProvider;
    private MenuUIView _view;
    private MenuUIModel _model;
    public MenuUIController()
    {
        _view = menuUIProvider.MenuView;
        _model = _view.Model;
    }

    private void OnPlaySubscribe()
    {
        menuUIProvider.MenuView.Model.OnPlayButtonPressed += OnPlayButtonClicked;
    }

    public void OnPlayButtonClicked()
    {
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
