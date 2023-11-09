using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] MenuUIProvider menuUIProvider;

    public override void InstallBindings()
    {
        Container.Bind<MenuUIProvider>().FromInstance(menuUIProvider);
    }
}