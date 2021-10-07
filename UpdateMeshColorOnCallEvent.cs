using System;
using System.Collections.Generic;
using UnityEngine;

namespace CollectCreatures
{
    public class UpdateMeshColorOnCallEvent : MonoBehaviour
    {
        #region VAR

        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private List<Color32> colors = new List<Color32>();
        [Space] [SerializeField] private float _delayChange;
        [ShowOnly] [SerializeField] private bool _isUpdating;
        private int _index;
        #endregion
        
        #region FUNC
        private void ChangeColor()
        {
            if (meshRenderer == null)
            {
                Debug.Log("Need MeshRenderer component");
                return;
            }
            if (colors.Count < _index)
            {
                Debug.Log("Need more colors");
                return;
            }
            
            meshRenderer.material.color = colors[_index - 1];
        }
        #endregion
        
        
        #region CALLBAKS    
        // V Code referenced by UnityEvents only V
        public void UpdateState(int index)
        {
            if(_isUpdating)
                return;
            
            _index = index;
            Invoke(nameof(ChangeColor), _delayChange);;
        }
        #endregion
    }
}