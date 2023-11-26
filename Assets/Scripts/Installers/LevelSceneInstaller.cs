using Ads;
using Data;
using DynamicMeshCutter;
using Game;
using MeshGenerators;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LevelSceneInstaller : MonoInstaller
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private FinalScreen finalScreen;
        [SerializeField] private HintBaseRewardedAdsButton hintBaseRewardedAdsButton;
        [SerializeField] private MouseBehaviour mouseBehaviour;
        [SerializeField] private ItemDataContainer itemDataContainer;
        [SerializeField] private LevelBuilder levelBuilder;
        [SerializeField] private MeshGenerator meshGenerator;
        [SerializeField] private HintLineDrawer hintLineDrawer;
        public override void InstallBindings()
        {
            BindServices();
            BindData();
            BindUI();
            BindADS();
        }
    
        private void BindServices()
        {
            Container.Bind<LevelBuilder>().FromInstance(levelBuilder).AsSingle();
            Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
            Container.Bind<MeshGenerator>().FromInstance(meshGenerator).AsSingle();
            Container.Bind<MouseBehaviour>().FromInstance(mouseBehaviour).AsSingle();

        }       
        private void BindData()
        {
            Container.Bind<ItemDataContainer>().FromInstance(itemDataContainer).AsSingle();
            Container.Bind<DataLoaderSaver>().To<DataLoaderSaver>().AsSingle().Lazy();
        }
        private void BindUI()
        {
            Container.Bind<FinalScreen>().FromInstance(finalScreen).AsSingle();
            Container.Bind<HintLineDrawer>().FromInstance(hintLineDrawer).AsSingle();
        }
        private void BindADS()
        {
            Container.Bind<HintBaseRewardedAdsButton>().FromInstance(hintBaseRewardedAdsButton).AsSingle();
        }
    }
}