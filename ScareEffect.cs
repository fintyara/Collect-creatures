using UnityEngine;

namespace CollectCreatures
{
    public class ScareEffect : ContinuousEffect
    {
        #region VAR
        private HaveMood _haveMood;
        private FleeAiState _fleeAiState;
        private Entity _entity;
        #endregion

       
        #region MONO
        private void Update()
        {
            if (active)
            {
                DurationControl();
            }
        }
        private void OnDisable()
        {
           Clear();
        }
        #endregion


        #region FUNC
       
        protected override void DurationControl()
        {
            if (timeElapsed > duration)
            {
                active = false;
                Destroy(gameObject);
            }
            else
            {
                timeElapsed += Time.deltaTime;
            }
        }
        protected override void Clear()
        {
            if (_entity != null)
                _entity.ClearEffect(effectType);
            if (_haveMood != null)
                _haveMood.MoodUp(power);
        }
        protected override void Break()
        {
            duration = 0;
        }
        #endregion
        
        #region INTERFACES
        public override void Init(Entity original, Entity target)
        {
            originalEntity = original;
            targetEntity = target;
            
            _fleeAiState = transform.parent.GetComponentInChildren<FleeAiState>();
            _haveMood = transform.GetComponentInParent<HaveMood>();

            if (_fleeAiState == null && _haveMood == null)
            {
                Debug.Log("Need FleeAiState or HaveMood component");
                Destroy(gameObject);
            }

            _entity = GetComponentInParent<Entity>();
                 
             initialized = true;


            Invoke("ApplyEffect", delayApplyEffect);
        }
        public override bool ApplyEffect()
        {
            if (_fleeAiState != null)
                _fleeAiState.SetTarget(originalEntity);
            if (_haveMood != null)
                _haveMood.MoodDown(power);
            if (_entity != null)
                _entity.AddEffect(effectType);
            
            active = true;
            originalEntity = null;
            
            return true;
        }
        #endregion


      
    }
}
