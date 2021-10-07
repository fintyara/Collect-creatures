using UnityEngine;


namespace CollectCreatures
{
    public class WanderState : EntityState
    {
        #region VAR

        [SerializeField] private float _timeWait;
        [SerializeField] private float _distance;
        [SerializeField] private float _stopDistance; 
        public float _chanceToRepeat = 50;
        
        private CanMove canMove;
        
        #endregion

        #region MONO

        private void Start()
        {
            haveState = GetComponent<HaveEntityState>();

            if (haveState == null)
                Debug.Log("Need HaveEntityState component");
            
            canMove = GetComponentInParent<CanMove>();

            if (canMove == null)
                Debug.Log("Need CanMove component");

            curPriority = normalPriority;
        }
        
        #endregion

        #region FUNC
        
        public override void EnterState()
        {
            active = true;

            timeStarted = Time.time;

            canMove.MoveWander(_distance, _stopDistance, 1);
            canMove.onFinishMove.AddListener(FinishWander);
        }
        public override void ExitState()
        {  
            canMove.onFinishMove.RemoveListener(FinishWander);
            active = false;
        }
        public void FinishWander()
        {
            if (Random.Range(0, 100) > _chanceToRepeat)
                RepeatState();
            else
                haveState.SetState(0);
        }
        private void RepeatState()
        {
            ExitState();
            EnterState();
        }
        #endregion
    }
}

