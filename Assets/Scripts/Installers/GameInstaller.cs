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

    public override void InstallBindings()
    {
        Container.Bind<ITileManager>().FromInstance(tileManager);
        Container.Bind<ITileSpawner>().FromInstance(tileSpawner);
        Container.Bind<ITileMovementController>().FromInstance(tileMovementController);
        //Container.Bind<PlayerMovement>().FromInstance(_playerMovement).AsSingle();
        Container.Bind<SwipeController>().FromInstance(_swipeController).AsSingle();
    }
}
