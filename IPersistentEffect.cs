using UnityEngine;


namespace CollectCreatures
{
    public interface  IPersistentEffect : IEffect
    {
        #region VAR
     
        
        #endregion
        
        #region FUNC
        void Clear();
        void Destroy();
        #endregion

    }
}
