using CollectCreatures;
using UnityEngine;


namespace CollectCreatures
{
    public class DryEffect : InstantEffect
    {
        #region VAR
        [SerializeField] private State targetState;
        private HaveEntityState _haveEntityState;
        #endregion


        public override void Init(Entity original, Entity target)
        {
            transform.SetParent(target.transform);
            transform.localPosition = Vector3.zero;

            _haveEntityState = transform.parent.GetComponentInChildren<HaveEntityState>();

            if (_haveEntityState == null)
            {
                Debug.Log("Need HaveEntityState");
            }
            else
            {
                ApplyEffect();
            }
            
            Destroy(gameObject);
        }

        public override bool ApplyEffect()
        {
            _haveEntityState.SetState(targetState);
            return true;
        }
    }
}
