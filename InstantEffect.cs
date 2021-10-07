using CollectCreatures;
using UnityEngine;


namespace CollectCreatures
{
    public abstract class InstantEffect : MonoBehaviour, IEffect
    {
        #region VAR

        protected Entity originalEntity;
        protected Entity targetEntity;

        #endregion

        #region FUNC
        public abstract void Init(Entity original, Entity target);
        public abstract bool ApplyEffect();
        #endregion
    }
}
