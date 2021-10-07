using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CollectCreatures
{
    public abstract class CanMove : MonoBehaviour
    {
        #region VAR
        public UnityEvent onStartMove;
        public UnityEvent onBreakMove;
        public UnityEvent onFinishMove;

        [SerializeField] protected bool rotate;
        [SerializeField] protected float rotateSpeed;
        [SerializeField] protected float stopDistance;
        [SerializeField] protected float moveSpeed;
        [ShowOnly] [SerializeField] protected float speedUpFactor = 1f;

        protected Vector3 targetPosition;
        protected Transform target;
        
        protected bool active;
        protected bool transformMove;
        protected Vector3 oldPosition;
        protected float curSpeed;
        #endregion

        #region MONO
        private void Update()
        {
            if (active)
                ControlMove();
            
            CalcRealSpeed();
        }
        #endregion

        #region FUNC
        public virtual float GetSpeed()
        {
             return curSpeed;
        }

        protected virtual void FinishMove()
        {
            target = null;
            active = false;
            speedUpFactor = 1f;
            transformMove = false;
            onFinishMove.Invoke();
        }
        public virtual void BreakMove()
        {
            target = null;
            active = false;
            speedUpFactor = 1f;
            transformMove = false;
            onBreakMove.Invoke();
        }
        protected virtual void CalcRealSpeed()
        {
            curSpeed = Vector3.Distance(oldPosition, transform.position) / Time.deltaTime;
            oldPosition = transform.position;
        }  
        protected virtual void ControlMove()
        {       
            if (transformMove)
            {
                if (target == null)
                {
                    BreakMove();
                    return;
                }
                if ((transform.position - target.position).magnitude <= stopDistance)
                {
                    FinishMove();
                    return;
                }

                MoveTransform(target);
                if (rotate)
                    RotateTransform(target);
            }
            else
            {
                if ((transform.position - targetPosition).magnitude <= stopDistance)
                {
                    FinishMove();
                    return;
                }
                
                MovePosition(targetPosition);
                if (rotate)
                    RotatePosition(targetPosition);
            }
        }
        protected virtual void MovePosition(Vector3 pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos,
                moveSpeed * speedUpFactor * Time.deltaTime);
        }
        protected virtual void MoveTransform(Transform t)
        {
            transform.position = Vector3.MoveTowards(transform.position, t.position, 
                moveSpeed * speedUpFactor * Time.deltaTime);
        }
        protected virtual void RotatePosition(Vector3 pos)
        {
            var targetPos = pos;
            var agentPos = transform.position;

            agentPos.y = 0;
            targetPos.y = 0;

            if (Vector3.Angle(targetPos - agentPos, transform.forward) <= 5f)
            {
                return;
            }
            
            var dir = targetPos - transform.position;
            dir.y = 0;

            var temp = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, temp, rotateSpeed * Time.deltaTime);
        }
        protected virtual void RotateTransform(Transform t)
        {
            var targetPos = t.position;
            var agentPos = transform.position;

            agentPos.y = 0;
            targetPos.y = 0;

            if (target == null || Vector3.Angle(targetPos - agentPos, transform.forward) <= 5f)
            {
                return;
            }


            var dir = target.position - transform.position;
            dir.y = 0;

            var temp = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, temp, rotateSpeed * Time.deltaTime);
        }
        public virtual void RotateForTime(Transform t,float duration)
        {
            StartCoroutine(RotateRoutine(t, duration));
        }
        
        private IEnumerator RotateRoutine(Transform t,float duration)
        {
            float lastTime = duration;

            while (lastTime > 0)
            {
                if (t == null)
                {
                    lastTime = 0;
                    continue;
                }

                var targetPos = t.position;
                var agentPos = transform.position;
                
                lastTime -= Time.deltaTime;
                
                agentPos.y = 0;
                targetPos.y = 0;

                var dir = targetPos - transform.position;
                dir.y = 0;

                var temp = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, temp, rotateSpeed * Time.deltaTime);
                
                yield return null;
            }
        }
        public virtual void SetTarget(Vector3 targetPos, float stopDist, float speedUp)
        {
            speedUpFactor = speedUp;
            stopDistance = stopDist;
            targetPosition = targetPos;
            active = true;
            transformMove = false;
            onStartMove.Invoke();
        }
        public virtual void SetTarget(Transform t, float stopDist, float speedUp)
        { 
            speedUpFactor = speedUp;
            stopDistance = stopDist;
            target = t;
            active = true;
            transformMove = true;
            onStartMove.Invoke();
        }
        public virtual void MoveWander(float maxDistance, float stopDist, float speedUp)
        {
            SetTarget(FindAroundPos(maxDistance), stopDist, speedUp);
        }
        protected virtual Vector3 FindAroundPos(float maxDistance)
        {
            var randomV = Random.insideUnitSphere * maxDistance;
            randomV.y = 0;

            return transform.position + randomV;
        }
        #endregion
    }
}
