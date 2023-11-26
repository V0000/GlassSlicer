using Market;
using UnityEngine;
using Zenject;
namespace Installers
{
    public class MarketSceneInstaller : MonoInstaller
    {
        [SerializeField] private MarketManager marketManager;
        [SerializeField] private ItemDataContainer itemDataContainer;
        public override void InstallBindings()
        {
            Container.Bind<MarketManager>().FromInstance(marketManager);
            Container.Bind<ItemDataContainer>().FromInstance(itemDataContainer);
        }
    }
}