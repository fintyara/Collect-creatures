using System.Collections;
using UnityEngine;
using Zenject;


namespace CollectCreatures
{
    public class EatState : EntityState
    {
        #region VAR    
        public IntEvent onEat;

        [SerializeField] private FoodType foodType;
        [SerializeField] private float speedUpFactor = 1.4f;
        [SerializeField] private float maxDistanceFind = 50;
        [SerializeField] private float goodDistanceFind = 25;
        [SerializeField] private float distanceEat = 1;
        [SerializeField] private float durationPrepareAction;
        [SerializeField] private float durationAction;
        
        [ShowOnly] [SerializeField] private bool needEat;
        [ShowOnly] [SerializeField] private bool actionRun;

        private CanMove _canMove;
        [SerializeField] private Foods _foods;
        private HaveFood _foodPlace;
        #endregion
        
        [Inject]
        private void Construct(Foods foods)
        {
            _foods = foods;
        }
        
        #region MONO
        private void Start()
        {
            _foods = GameObject.Find("Foods").GetComponent<Foods>();

            if (_foods == null)
                Debug.Log("Need Foods component");
            
            haveState = GetComponent<HaveEntityState>();

            if (haveState == null)
                Debug.Log("Need HaveEntityState component");

            
            _canMove = GetComponentInParent<CanMove>();
            if (_canMove == null)
                Debug.Log("Need CanMove component");
            
            
            curPriority = normalPriority;
            FindTargetControl();
        }
        private void OnDestroy()
        {
            if (_foodPlace != null)
            {
               UnsubscribeOnFoodPlace();
            }
        }
        #endregion

        #region FUNC
        public override void EnterState()
        {   
            active = true;
            timeStarted = Time.time;
            
            if (_foodPlace != null)
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
            if(_foodPlace != null)
            {
               UnsubscribeOnFoodPlace();
            }
            
            _foodPlace = null;
            curPriority = normalPriority;
        }
        
        private void FindTargetControl()
        {
            if (needEat && _foodPlace == null)
            {
                _foodPlace = _foods.GetFoodPlace(foodType, transform.parent.transform, maxDistanceFind, goodDistanceFind);

                if (_foodPlace != null)
                {
                    curPriority = hightPriority;
                    SubscribeOnFoodPlace();
                }
            }

            Invoke(nameof(FindTargetControl), 1f);
        }
        private void StartMove()
        {
            _canMove.SetTarget(_foodPlace.transform, distanceEat, speedUpFactor);
            _canMove.onFinishMove.AddListener(StartAction);
            _canMove.onBreakMove.AddListener(NextState);
        }
        private void StartAction()
        {
            if (!actionRun)
                StartCoroutine(ActionRoutine());
        }
        private IEnumerator ActionRoutine()
        {
            actionRun = true;
            
            yield return new WaitForSeconds(durationPrepareAction);

            if (_foodPlace != null && _foodPlace.CheckHaveFood())
            {
                onEat?.Invoke(_foodPlace.GetFood(foodType));
                needEat = false;

                yield return new WaitForSeconds(durationAction);
            }
            
            NextState();
            
            actionRun = false;
        }
        private void SubscribeOnFoodPlace()
        {
            _foodPlace.onAddFood.AddListener(TargetChanged);
            _foodPlace.onGetFood.AddListener(TargetChanged);
            _foodPlace.onEmpty.AddListener(TargetChanged);
        }
        private void UnsubscribeOnFoodPlace()
        {
            _foodPlace.onAddFood.RemoveListener(TargetChanged);
            _foodPlace.onGetFood.RemoveListener(TargetChanged);
            _foodPlace.onEmpty.RemoveListener(TargetChanged);
        }
        private void TargetChanged()
        {
            if (_foodPlace.CheckHaveFood()) 
                return;
            
            UnsubscribeOnFoodPlace();
                
            _foodPlace = null;
                
            if (!actionRun)
            {
                NextState();
            }
        }
        private void NextState()
        {
            if (_foodPlace != null)
            {
                UnsubscribeOnFoodPlace();
                _foodPlace = null;
            }
          
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
        // V Code referenced by UnityeEvents only V 
        public void NeedToEat()
        {
            needEat = true;
        }
        #endregion
    }
}