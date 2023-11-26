using Data;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MapSceneInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            Container.Bind<DataLoaderSaver>().To<DataLoaderSaver>().AsSingle().Lazy();
        }
    }
}