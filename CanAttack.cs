using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace CollectCreatures
{
    [System.Serializable]
    public class AttackTypeEvent : UnityEvent<AttackType> { };
    public class TransformEvent : UnityEvent<Transform> { };
    
    
    public class CanAttack : MonoBehaviour
    {
        #region VAR    
        public UnityEvent onStartAttack;
        public AttackTypeEvent onAttack;
        public AttackTypeEvent onAttackTypeChanged;
        public UnityEvent onBreakAttack;
        public UnityEvent onFinishAttack;

        public AttackType AttackType { get => _attackType; set => _attackType = value; }
        public float ReloadAttack { get => _reloadAttack; set => _reloadAttack = value; }
        [Space(10)]
        [SerializeField] private Entity _target;
        [SerializeField] private AttackType _attackType;
        [SerializeField] private float _reloadAttack;
        [SerializeField] private float _powerAttack;
        [SerializeField] private float _distanceAttack;
        [SerializeField] private LayerMask _layerMask;

        protected bool active;

        #endregion

        #region MONO

        private void Start()
        {
          
        }
        private void Update()
        {
            if (active)
                ControlAttack();        
        }

        #endregion

        #region FUNC
      
        public virtual void SetAttackType(AttackType attackType)
        {
            _attackType = attackType;

            BreakAttack();
        }
        public virtual bool CheckCanAttack()
        {
            return false;
        }
        public virtual bool CheckTarget()
        {
            return false;
        }
        public virtual void StartAttack()
        {
            if (_target)
                onStartAttack?.Invoke();
            else
            {
                BreakAttack();
            }
        }
        public virtual void BreakAttack()
        {
            _target = null;
            onBreakAttack.Invoke();
        }
        public virtual void FinishAttack()
        {
            _target = null;
            onFinishAttack.Invoke();
        }
        protected virtual void ControlAttack()
        {

        }
        protected virtual bool Attack()
        {
            return false;
        }
        
        #endregion

        #region CALLBAKS   
        // V Code referenced by Unityevents only V    
        public virtual void SetTarget(Entity entity)
        {
            var attackType = GetComponent<AttackType>();

            if (attackType != _attackType || entity == null)
                return;
            
            _target = entity;
        }
        #endregion
    }
}
