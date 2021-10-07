using System.Collections;
using UnityEngine;
using UnityEngine.Events;


namespace CollectCreatures
{
    [System.Serializable]
    public class EntityTypeEvent : UnityEvent<EntityType>{};
    
    public class AttackState : EntityState,IHaveTarget
    {
        #region VAR    

        public UnityEvent OnAttack;
        public EntityTypeEvent OnEntityTypeChanged;
        
        public float ReloadAttack { get => reloadAttack; set => reloadAttack = value; }
        [Space(10)]
       
        [SerializeField] private AttackType attackType;
        [SerializeField] private float reloadAttack;
        [SerializeField] private float powerAttack;
        [SerializeField] private float speedUpFactor = 1.2f;
        [SerializeField] private float attackDistance;
        [SerializeField] private float durationPrepareAction;
        [SerializeField] private float durationAction;
        
        [ShowOnly] [SerializeField] private bool needTarget;
        [ShowOnly] [SerializeField] private bool actionRun;
        [ShowOnly] [SerializeField] private bool reloading;  
        [ShowOnly] [SerializeField] private float timeAttack;
        
        protected CanMove canMove;
        private Entity _parentEntity;
        private Entity _targetEntity;
        #endregion

        #region MONO
        private void Start()
        {
            _parentEntity = GetComponentInParent<Entity>();
            if (_parentEntity == null)
                Debug.Log("Need Entity component");
            
            haveState = GetComponent<IHaveState>();
            if (haveState == null)
                Debug.Log("Need IHaveState interface");
            
            canMove = GetComponentInParent<CanMove>();
            if (canMove == null)
                Debug.Log("Need CanMove component");
            
            needTarget = true;
            curPriority = normalPriority;
        }
        private void Update()
        {
            if (!reloading && Time.time > timeAttack + reloadAttack)
            {
                reloading = true;

                if (_targetEntity == null && needTarget)
                    needTarget = true;
            }
        }
        private void OnDestroy()
        {
            if (_targetEntity != null)
                _targetEntity.onDeath.RemoveListener(TargetDestroyed);
        }
        #endregion

        #region FUNC
        public override void EnterState()
        {
            active = true;
            timeStarted = Time.time;

            if (_targetEntity != null)
            {
               StartMove();
            }
            else
            {
                NextState();
            }
        }
        public override void ExitState()
        {
            if (reloading)
                needTarget = true;
            
            _targetEntity = null;
            curPriority = normalPriority;
            canMove.onFinishMove.RemoveListener(StartAction);
            canMove.onBreakMove.RemoveListener(NextState);
        }
        
        private void StartMove()
        {
            canMove.SetTarget(_targetEntity.transform, attackDistance, speedUpFactor);
            canMove.onFinishMove.AddListener(StartAction);
            canMove.onBreakMove.AddListener(NextState);
        }
        private void NextState()
        {
            curPriority = normalPriority;
            haveState.NextState();
        }
        private void BreakState()
        {
           StopAllCoroutines();
           NextState();
        }
        #endregion

        #region CALLBAKS     
        // V Code referenced by UnityEvents only V    
        public virtual void SetAttackType(AttackType a)
        {
            if (attackType == a)
                return;
            
            attackType = a;
        }
        #endregion
        
        private IEnumerator ActionRoutine()
        {
            actionRun = true;
            yield return new WaitForSeconds(durationPrepareAction);

            if (_targetEntity != null)
            {
                var applyEffectProvider = new ApplyEffectProvider();

                if (applyEffectProvider.Provide(_parentEntity, _targetEntity, attackType))
                {
                    OnAttack?.Invoke();
                    timeAttack = Time.time;

                    yield return new WaitForSeconds(durationAction);
                }
            }

            actionRun = false;
            NextState();
        }
        private void StartAction()
        {
            if (!actionRun)
                StartCoroutine(ActionRoutine());
        }
        public void TargetDestroyed(Entity e)
        {
            _targetEntity = null;
            
            if (!actionRun)
            {
                NextState();
            }
        }
        public void SetTarget(Entity entity)
        {
            _targetEntity = entity;

            _targetEntity.onDeath.AddListener(TargetDestroyed);
            curPriority = hightPriority;
            needTarget = false;
        }
        public bool NeedTarget()
        {
            return needTarget;
        }
        public float GetDistanceFind()
        {
            return 100;
        }
        public AttackType GetAttackType()
        {
            return attackType;
        }
    }
}

