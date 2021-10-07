using UnityEngine;
using UnityEngine.Events;


namespace CollectCreatures
{
    public abstract class EntityState : SimpleState
    {
        #region VAR
        public int CurPriority => curPriority;

        [SerializeField] protected int lowPriority = -1000;
        [SerializeField] protected int normalPriority = 10;
        [SerializeField] protected int hightPriority = 1000;
        
        [ShowOnly] [SerializeField] protected int curPriority;
        #endregion
    }
}
