//using UnityEngine;
//using Zenject;

//public class TilesInstaller : MonoInstaller
//{
//    [SerializeField]
//    private TileSpawner tileSpawner;

//    [SerializeField]
//    private TileMovementController tileMovementController;

//    [SerializeField]
//    private TilePauseManager tilePauseManager;

//    [SerializeField]
//    private TileListManager tileListManager;

//    public override void InstallBindings()
//    {
//        Container.Bind<TileSpawner>().FromInstance(tileSpawner);
//        Container.Bind<TileMovementController>().FromInstance(tileMovementController);
//        Container.Bind<TilePauseManager>().FromInstance(tilePauseManager);
//        Container.Bind<TileListManager>().FromInstance(tileListManager);
//    }
//}