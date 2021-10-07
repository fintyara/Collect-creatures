using System;
using UnityEngine;

namespace CollectCreatures
{
    [RequireComponent( typeof(MakeProfit))]
    [RequireComponent( typeof(HaveMood))]
    public class MoodToProfitConnector : MonoBehaviour
    {
        #region VAR
        [SerializeField] private TypeCalc typeCalc;
        private enum TypeCalc
        {
            Count,Factor
        }
        private MakeProfit _makeProfit;
        #endregion

        #region MONO
        private void Start()
        {
            _makeProfit = GetComponent<MakeProfit>();
        }
        #endregion
        
        #region FUNC
        public void Connect(int curMood)
        {
            switch (typeCalc)
            {
                case TypeCalc.Count:
                    _makeProfit.ChangeModifyCount(CalculateModifyCount(curMood));
                    break;
                case TypeCalc.Factor:
                    _makeProfit.ChangeModifyCount(CalculateModifyFactor(curMood));
                    break;
                default:
                    _makeProfit.ChangeModifyCount(CalculateModifyFactor(curMood));
                    break;
            }
        }
        private float CalculateModifyCount(float mood)
        {
            return -(100 - mood) / 10;
        }
        private float CalculateModifyFactor(float mood)
        {
            return -(100 - mood) / 100;
        }
        #endregion   
    }
}
