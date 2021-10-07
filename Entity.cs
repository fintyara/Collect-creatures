using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CollectCreatures
{
    [System.Serializable]
    public class  EntityEvent : UnityEvent<Entity>{};
    
    public abstract class Entity : MonoBehaviour
    {
       

        #region VAR

        public EntityEvent onDeath;
        public EntityEvent onChanged;
        [SerializeField] protected List<EffectType> haveEffects;
        [SerializeField] protected EntityType entityType;
        public EntityType EntityType => entityType;
       
        [Space(10)] [SerializeField] private float _delayBeforeDestroyGo;
        [ShowOnly] [SerializeField] private bool dead;

        #endregion

        #region FUNC

        public virtual void Death()
        {
            if(dead)
                return;
            
            HaveEntityState haveEntityState = GetComponentInChildren<HaveEntityState>();

            if (haveEntityState != null)
                Destroy(haveEntityState.gameObject);

            CanMove canMove = GetComponentInChildren<CanMove>();

            if (canMove != null)
                Destroy(canMove);

            onDeath?.Invoke(GetComponent<Creature>());
            dead = true;
            Destroy(gameObject, _delayBeforeDestroyGo);
        }
        public virtual bool CheckCanDeath()
        {
            return !dead;
        }
        public virtual void AddEffect(EffectType effectType)
        {
            haveEffects.Add(effectType);
        }
        public virtual void ClearEffect(EffectType effectType)
        {
            if (haveEffects.Contains(effectType))
                haveEffects.Remove(effectType);
        }
        public virtual bool CheckHaveEffect(EffectType effectType)
        {
            return haveEffects.Contains(effectType);
        }
        #endregion
    }
}
