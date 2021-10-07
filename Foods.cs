using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CollectCreatures
{
    public class Foods : MonoBehaviour
    {
        #region VAR
        private List<HaveFood> _haveFoods = new List<HaveFood>();
        private List<HaveFood> _selected = new List<HaveFood>();
        #endregion

        #region MONO
        private void Start()
        {
            FindAllFoodPlace();
        }
        #endregion
        
        #region FUNC
        public HaveFood GetFoodPlace(FoodType foodType, Transform original, float maxDistance, float goodDistance)
        {
            if (!SelectByHaveFoodAndSortByDistance(original))
            {
                return null;
            }

            // find on goodDistance
            // find best food
            for (int i = 0; i < _selected.Count; i++)
            {
                if (_haveFoods[i].FoodType == foodType &&
                    (this.transform.position - original.position).magnitude < goodDistance)
                {
                    return _haveFoods[i];
                }
            }

            // find good food
            for (int i = 0; i < _selected.Count; i++)
            {
                if (_haveFoods[i].FoodType.FoodClass == foodType.FoodClass &&
                    (this.transform.position - original.position).magnitude < goodDistance)
                {
                    return _haveFoods[i];
                }
            }

            // find bad food
            for (int i = 0; i < _selected.Count; i++)
            {
                if ((this.transform.position - original.position).magnitude < goodDistance)
                    return _haveFoods[0];
            }

            
            // find on maxDistance
            // find best food
            for (int i = 0; i < _selected.Count; i++)
            {
                if (_haveFoods[i].FoodType == foodType &&
                    (this.transform.position - original.position).magnitude < maxDistance)
                {
                    return _haveFoods[i];
                }
            }

            // find good food
            for (int i = 0; i < _selected.Count; i++)
            {
                if (_haveFoods[i].FoodType.FoodClass == foodType.FoodClass &&
                    (this.transform.position - original.position).magnitude < maxDistance)
                {
                    return _haveFoods[i];
                }
            }

            // find bad food
            for (int i = 0; i < _selected.Count; i++)
            {
                if ((this.transform.position - original.position).magnitude < maxDistance)
                    return _haveFoods[0];
            }
            
            return null;
        }
        private bool SelectByHaveFoodAndSortByDistance(Transform original)
        {
            _selected.Clear();
            
            _haveFoods = _haveFoods
                .OrderBy(x => (this.transform.position - original.position).magnitude)
                .ToList();

            for (int i = 0; i < _haveFoods.Count; i++)
            {
                if (_haveFoods[i].CheckHaveFood())
                    _selected.Add(_haveFoods[i]);
            }
            
            return _selected.Count > 0;
        }
        private void FindAllFoodPlace()
        {
            var places = GameObject.FindObjectsOfType<HaveFood>();

            if (places.Length == 0)
                return;
            
            for (int i = 0; i < places.Length; i++)
            {
                _haveFoods.Add(places[i]);
            }
        }
        #endregion  
    }
}
