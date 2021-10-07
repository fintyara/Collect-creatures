using UnityEngine;
using UnityEngine.Events;


namespace CollectCreatures
{
    public class HaveFullness : MonoBehaviour
    {
        #region VAR
        private const int MAX_FULLNESS = 150;
        public UnityEvent onAddFullness;
        public UnityEvent onFullnessLvlDeath;
        public UnityEvent onFullnessLvlEat;
        [SerializeField] private float fullness = 100;
        [SerializeField] private float fullnessLvlDeath;
        [SerializeField] private float fullnessLvlEat;

        [SerializeField] private float speedHungry = 1;

        [ShowOnly] [SerializeField] private float curSpeedHungry = 1;
        [ShowOnly] [SerializeField] private bool _active;
        [ShowOnly] [SerializeField] private bool _needEat;
        #endregion

        #region MONO
        private void Start()
        {
            _active = true;
            curSpeedHungry = speedHungry;
        }
        void Update()
        {
            FullnessControl();
        }
        #endregion

        #region FUNC
        private void FullnessControl()
        {
            if (!_active)
                return;
            
            if (fullness < fullnessLvlEat)
            {
                _needEat = true;
                onFullnessLvlEat?.Invoke();
            }
            if (fullness < fullnessLvlDeath)
            {
                _active = false;
                onFullnessLvlDeath?.Invoke();
            }

            fullness -= Time.deltaTime * curSpeedHungry;
        }
        public void Reset()
        {
            _active = true;
            fullness = MAX_FULLNESS;
            _needEat = false;
        }
        #endregion
        
        #region CALLBAKS   
        // V Code referenced by UnityEvents only V    
        public void AddFullness(int count)
        {
            fullness += count;
            fullness = Mathf.Clamp(fullness, 0, MAX_FULLNESS);
            onAddFullness?.Invoke();
        }
        #endregion
    }
}
