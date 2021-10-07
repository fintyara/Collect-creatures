using UnityEngine;

namespace CollectCreatures
{
    public class HaveMood : MonoBehaviour
    {
        #region VAR
        [SerializeField]private const int MIN_MOOD = 0;
        private const int MAX_MOOD = 100;
        public IntEvent onMoodChanged = null;
        [SerializeField] private int mood;
        #endregion
        
        #region FUNC
        public void MoodUp(int value)
        {
            mood += value;
            mood = Mathf.Clamp(mood, MIN_MOOD, MAX_MOOD);
            onMoodChanged?.Invoke(mood);
        }
        public void MoodDown(int value)
        {
            mood -= value;
            if (mood < MIN_MOOD)
                mood = MIN_MOOD;
            
            onMoodChanged?.Invoke(mood);
        }
        public void ChangeMood(int value)
        {
            mood += value;
            mood = Mathf.Clamp(mood, MIN_MOOD, MAX_MOOD);
            
            onMoodChanged?.Invoke(mood);
        }
        #endregion
    }
}
