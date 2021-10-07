using UnityEngine;
using UnityEngine.Events;


namespace CollectCreatures
{
    public class MakeProfit : MonoBehaviour
    {
        #region VAR

        public IntEvent onProfitChanged;
        [SerializeField] protected int count = 10;
        [SerializeField] protected int secureCount = 0;
        [SerializeField] protected float factor = 1;
        
        [ShowOnly] [SerializeField] protected int modifyCount;
        [ShowOnly] [SerializeField] protected float modifyFactor;
        [ShowOnly] [SerializeField] protected int calculatedCount = 0;
        #endregion

        #region MONO
       
        #endregion

        #region FUNC

        public virtual void CalculateCount()
        {
            calculatedCount = (int) ((count + modifyCount) * (factor + modifyFactor) + secureCount);
        }
        public virtual int GetProfit()
        {
            return calculatedCount;
        }
        public virtual void SetModifyCount(float value)
        {
            modifyCount = (int) value;

            if (modifyCount < 0)
                modifyCount = 0;
        } 
        public virtual void ChangeModifyCount(float value)
        {
            modifyCount = (int) value;
            
            if (modifyCount < 0)
                modifyCount = 0;
        }
        public virtual void SetModifyFactor(float value)
        {
            modifyFactor = value;

            Mathf.Clamp(modifyFactor, 0, 2);
        }
        public virtual void ChangeModifyFactor(float value)
        {
            modifyFactor += value;
            
            Mathf.Clamp(modifyFactor, 0, 2);
        }
        #endregion

        #region CALLBAKS
      
        #endregion
    }
}

