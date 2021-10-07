using UnityEngine;
using Zenject;

namespace CollectCreatures
{
    public class LocationInstaller : MonoInstaller
    {
        #region VAR   
        [SerializeField] private Foods foods;
        #endregion

        #region FUNC     
        public override void InstallBindings()
        {
           BindFoods();
        }
        private void BindFoods()
        {
            Container
                .Bind<Foods>()
                .FromInstance(foods)
                .AsSingle();
        }
        #endregion   
    }
}
