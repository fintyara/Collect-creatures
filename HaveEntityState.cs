using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CollectCreatures
{
    public class HaveEntityState : MonoBehaviour,IHaveState
    {
        #region VAR
        public EntityStateEvent onEntityStateChanged;
        public event Action<int> OnStateIdChanged;
        public event Action<State> OnStateChanged;
        [SerializeField] protected List<EntityState> states = new List<EntityState>();
        [Space(10)]
        [SerializeField] protected EntityState startState;
        [ShowOnly] [SerializeField] protected bool locked;
        [ShowOnly] [SerializeField] protected EntityState curState;
        #endregion

        #region MONO
        private void Start()
        {
            if (startState != null)
                SetState(startState);
            else
            {
                Debug.Log("Need start state");
            }
        }
        private void Update()
        {
            if (curState != null)
                curState.State();
            else
            {
                Debug.Log("Need state");
            }
        }
        #endregion

        #region FUNC
        public State GetState()
        {
            return curState.TagState;
        }
        public int GetStateId()
        {
            return states.IndexOf(curState);
        }

        public void SetState(int index)
        {
            SetState(states[index]);
        }
        public void NextState()
        {
            if (locked)
                return;
            
            var maxPriority = 0;
            var index = 0;
            
            for (int i = 0; i < states.Count; i++)
            {
                var curPriority = states[i].CurPriority;

                if (curPriority <= maxPriority) continue;
                
                maxPriority = curPriority;
                index = i;
            }
            
            if (maxPriority > 0)
                SetState(states[index]);
            else
                SetState(states[0]);
        }
        public void SetState(State newState)
        {
           SetState(GetEntityState(newState));
        }
        
        public void LockState()
        {
            locked = true;
        } 
        public void UnlockState()
        {
            locked = false;
        }
        
        private EntityState GetEntityState(State s)
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].TagState == s)
                    return states[i];
            }

            return null;
        }
        private void SetState(EntityState newState)
        {
            if (locked || newState == null || curState == newState)
                return;
            
            if (!states.Contains(newState))
                return;

            if (curState != null)
                curState.ExitState();
            curState = newState;
            curState.EnterState();

            onEntityStateChanged?.Invoke(curState);
            OnStateIdChanged?.Invoke(states.IndexOf(curState));
            OnStateChanged?.Invoke(curState.TagState);
        }
        
        #endregion
    }
}

