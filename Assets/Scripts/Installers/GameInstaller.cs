using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    private TileManager tileManager;

    [SerializeField]
    private TileSpawner tileSpawner;

    [SerializeField]
    private TileMovementController tileMovementController;

    //private PlayerMovement _playerMovement;

    [SerializeField]
    private SwipeController _swipeController;

    [SerializeField]
    private PauseMenu _pauseMenu;

    [SerializeField]
    private CharacterMovement _characterMovement;

    [SerializeField]
    private ObstacleHolder _obstacleHolder;

    public override void InstallBindings()
    {
        Container.Bind<ITileManager>().FromInstance(tileManager);
        Container.Bind<TileSpawner>().FromInstance(tileSpawner);
        Container.Bind<TileMovementController>().FromInstance(tileMovementController);
        //Container.Bind<PlayerMovement>().FromInstance(_playerMovement).AsSingle();
        Container.Bind<SwipeController>().FromInstance(_swipeController).AsSingle();
        Container.Bind<PauseMenu>().FromInstance(_pauseMenu).AsSingle();
        Container.Bind<CharacterMovement>().FromInstance(_characterMovement).AsSingle();
        Container.Bind<ObstacleHolder>().FromInstance(_obstacleHolder).AsSingle();
    }
}
