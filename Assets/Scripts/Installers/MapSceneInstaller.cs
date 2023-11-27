using Data;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MapSceneInstaller : MonoInstaller
    {
        [SerializeField] private ItemDataContainer itemDataContainer;

        public override void InstallBindings()
        {
            Container.Bind<DataLoaderSaver>().To<DataLoaderSaver>().AsSingle().Lazy();
            Container.Bind<ItemDataContainer>().FromInstance(itemDataContainer);
        }
    }
}