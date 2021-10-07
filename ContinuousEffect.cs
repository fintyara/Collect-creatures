using CollectCreatures;
using UnityEngine;


namespace CollectCreatures
{
    public abstract class ContinuousEffect : MonoBehaviour, IEffect
    {
        #region VAR
        public string Uid => uid;
        public bool Active
        {
            get => active;
            set => active = value;
        }
        public EffectType EffectType => effectType;
        public int Priority => priority;
        [Space(10)]
        [ShowOnly][SerializeField] protected string uid = "";
        [SerializeField] protected Sprite ico = null;
        [SerializeField] protected EffectType effectType = null;
        [SerializeField] protected int priority = 0;

        [SerializeField] protected int power = 30;
        [SerializeField] protected float duration = 10;
        [SerializeField] protected float delayApplyEffect = 0.5f;
        [ShowOnly] [SerializeField] protected bool active = false;
        [ShowOnly] [SerializeField] protected float timeElapsed = 0;
        [ShowOnly] [SerializeField] protected bool initialized = false;
        protected Entity originalEntity;
        protected Entity targetEntity;
        #endregion

        #region FUNC
        protected abstract void  DurationControl();
        protected abstract void  Clear();
        protected abstract void  Break();
        
        protected virtual void UpdateEffect(){}
        #endregion
        
        #region INTERFACES
        public abstract void Init(Entity original, Entity target);
        public abstract bool ApplyEffect();
        #endregion
    }
}
