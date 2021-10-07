using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollectCreatures
{
    public class AnimationController : MonoBehaviour
    {
        #region VAR
        private Animator _animator;
        private CanMove canMove;

        [SerializeField] private float reloadControl = 0.2f;
        [SerializeField] private float startDelay = 0.5f;
        private string prevState = "";

        #endregion

        #region MONO
        private void OnEnable()
        {
            canMove = GetComponent<CanMove>();
            if (canMove == null)
                Debug.Log("Need CanMove component");

            _animator = GetComponentInChildren<Animator>();

            if (_animator == null)
                _animator = GetComponent<Animator>();

            if (_animator == null)
                Debug.Log("Need Animator component");

            Invoke(nameof(SpeedControl), startDelay);
        }
        #endregion

        #region FUNC
        private void SpeedControl()
        {
            if (_animator == null)
                return;

            _animator.SetFloat("moveSpeed", canMove.GetSpeed());

            Invoke(nameof(SpeedControl), reloadControl);
        }
        #endregion

        #region CALLBACKS   
        // V Code referenced by Unityevents only V
        public void StartAttack()
        {
            if (_animator == null)
                return;
            
            if (prevState != "")
                _animator.SetBool(prevState, false);

            _animator.SetTrigger("attack");
            prevState = "";
        } 
        public void StartEvolution(int lvl)
        {
            if (_animator == null)
                return;
            
            if (prevState != "")
                _animator.SetBool(prevState, false);

            _animator.SetTrigger("evolution");
            prevState = "";
        }
        public void StartSleep()
        {
            if (_animator == null)
                return;

            if(prevState != "")
                _animator.SetBool(prevState, false);

            _animator.SetBool("sleep", true);
            prevState = "sleep";
        }
        public void StartEat()
        {
            if (_animator == null)
                return;

            if (prevState != "")
                _animator.SetBool(prevState, false);

            _animator.SetBool("eat", true);
            prevState = "eat";
        }
        public void StartDeath(Entity e)
        {
            if (_animator == null)
                return;

            if (prevState != "")
                _animator.SetBool(prevState, false);

            _animator.SetTrigger("death");
            prevState = "";
        }
        #endregion
    }
}
