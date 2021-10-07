using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CollectCreatures
{
    public class FindTargets : MonoBehaviour
    {
        #region VAR
        
        [SerializeField] private bool active;
        public bool Active
        {
            get => active;
            set => value = active;
        }
        [SerializeField] private List<IHaveTarget> _haveTargets = new List<IHaveTarget>();
        [SerializeField] protected float  distanceFind;
        [SerializeField] protected LayerMask layerMask;
        private List<Entity> _entities = new List<Entity>();
        #endregion
        
        #region MONO
        void Start()
        {
            if (FindHaveTargets())
                active = true;
        }
        void Update()
        {
            if (_haveTargets.Count == 0)
            {
               return;
            }
            if (active)
            {
                if (Time.frameCount % 10 == 0)
                    FindAllTarget();
            }
        }
        #endregion
        
        #region FUNC
        private bool FindHaveTargets()
        {
            var finded = GetComponentsInChildren<IHaveTarget>();

            if (finded.Length > 0)
            {
                _haveTargets.Clear();
                for (int i = 0; i < finded.Length; i++)
                {
                    _haveTargets.Add(finded[i]);
                }

                return true;
            }

            return false;
        }
        private void FindAllTarget()
        {
            for (int i = 0; i < _haveTargets.Count; i++)
            {
                if (_haveTargets[i].NeedTarget())
                {
                    var dist = _haveTargets[i].GetDistanceFind();
                    var attackType = _haveTargets[i].GetAttackType();

                   Entity entity =  FindTarget(attackType, dist > 0 ? distanceFind : dist);
                    
                    if (entity != null)
                    {
                        _haveTargets[i].SetTarget(entity);
                        return;
                    }
                }
            }
        }
        private Entity FindTarget(AttackType attackType, float dist)
        {  
            var hitColliders = Physics.OverlapSphere(transform.position, distanceFind, layerMask);
            if (hitColliders.Length > 0)
            {
                _entities.Clear();
                
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].transform == transform)
                       continue;
                    
                    var entity = hitColliders[i].GetComponent<Entity>();
                    

                    if (entity != null && !entity.CheckHaveEffect(attackType.EffectType) && entity.EntityType == attackType.EntityType &&
                        entity.EntityType.VulnerableTypes.Contains(attackType))
                    {
                        _entities.Add(entity);

                        return Utility.GetNearEntityMaxDistance(_entities, transform.position, distanceFind);
                    }
                }
            }

            return null;
        }
        #endregion
    }
    
    public class Target
    {
        public Entity entity;
        public EntityType entityType;
        public AttackType attackType;

        public Target(EntityType et, AttackType a)
        {
            entityType = et;
            attackType = a;
        }
    }
}
