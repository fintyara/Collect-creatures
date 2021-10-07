using System.Collections.Generic;
using UnityEngine;
using System;


namespace CollectCreatures
{
    public abstract class LocationEvent : MonoBehaviour
    {
        #region VAR      
        public event Action<LocationEvent> OnLocationEventEnded;  

        [Space(10)]
        [SerializeField] protected bool active;    
        [SerializeField] protected bool canActivated = true;

        [Space(10)] [SerializeField] protected List<Entity> entitiesPref = new List<Entity>();
        [Space(10)]
        [SerializeField] protected int lvlHard = 1;         
        [SerializeField] protected int maxCount = 5;
        [SerializeField] protected int allCount = 20;
        [Space(10)]
        [SerializeField] protected float reloadSpawn = 4;
        [SerializeField] protected float reloadEvent = 30f;
        [SerializeField] protected float durationEvent = 120f;
        [Space(10)]
        [SerializeField] protected bool forceDestroy;
        [SerializeField] protected float reloadDestroy = 2f;
        [Space(10)]
        [ShowOnly]
        [SerializeField] protected int curCount;
        [ShowOnly]
        [SerializeField] protected bool spawn;

        public bool Active => active;
        public bool CanActivated => canActivated;

        [Space(10)]
        protected ShuffledSpawnPoints spawnPoints;      
        [Space(10)] [SerializeField] protected List<Entity> entities = new List<Entity>();
        protected float timeSpawned;
        protected float timeStarted;
        protected float timeDestroyed;
      
        protected List<Entity> selected = new List<Entity>();
        #endregion

        #region MONO
        private void Start()
        {
            spawnPoints = GetComponent<ShuffledSpawnPoints>();

            if (!spawnPoints)
                Debug.Log("Need points");
        }
        private void Update()
        {
            UpdateEvent();
        }
        #endregion

        #region FUNC
        public virtual void StartEvent()
        {
            spawn = true;
            active = true;
            canActivated = false;

            timeStarted = Time.time;
        }
        protected virtual void UpdateEvent()
        {
            if (!active)
                return;

            if (Time.time > timeStarted + durationEvent || curCount >= allCount)
            {
                spawn = false;
            }

            if (spawn)
                SpawnControl();
            else
            {
                DestroyControl();

                if (entities.Count == 0)
                {
                    StopEvent();
                }
            }
        }
        protected virtual void StopEvent()
        {
            OnLocationEventEnded.Invoke(this);
            active = false;
            Invoke(nameof(ReloadEvent), reloadEvent);
        }
        protected virtual void ReloadEvent()
        {       
            curCount = 0;
            canActivated = true;

            entities.Clear();
            spawnPoints.Shuffle();
        }
        
        protected virtual void SpawnControl()
        {
            if (Time.time < timeSpawned + reloadSpawn)
                return;

            if (curCount < maxCount && curCount < allCount)
            {
                Entity entity;
                
                if (spawnPoints == null)
                    entity = Instantiate(entitiesPref[UnityEngine.Random.Range(0, entitiesPref.Count)], transform);
                else
                {
                     entity = Instantiate(entitiesPref[UnityEngine.Random.Range(0, entitiesPref.Count)], spawnPoints.NextPoint());
                }

                entity.onDeath.AddListener(EntityDestroyed);

                entity.transform.localPosition = Vector3.zero;
                entity.transform.SetParent(transform);

                AddEntity(entity);

                timeSpawned = Time.time;
            }
        }
        protected virtual void DestroyControl()
        {    
            if (!forceDestroy)
                return;

            if (Time.time < timeDestroyed + reloadDestroy)
                return;

            int randomIndex = UnityEngine.Random.Range(0, entities.Count);

            Entity entity = entities[randomIndex];
            
            if (entity.CheckCanDeath())
            {
                entity.Death();
                RemoveEntity(entity);
            }

            timeDestroyed = Time.time;
        }   
        
        protected virtual void AddEntity(Entity entity)
        {
            entities.Add(entity);
            curCount++;
        }
        protected virtual void RemoveEntity(Entity entity)
        {
            if (entities.Contains(entity))
            {
                entities.Remove(entity);

                curCount--;
            }
        }
        public virtual Entity GetNearEntityMaxDistance(Vector3 originalPos, float maxDistance)
        {
            return Utility.GetNearEntityMaxDistance(entities, originalPos, maxDistance);
        }
        public virtual Entity NearAnyEntityMaxDistance(Vector3 originalPos, float maxDistance)
        {
            return Utility.GetAnyEntityMaxDistance(entities, originalPos, maxDistance);
        }
        #endregion

        #region CALLBAKS
        protected virtual void EntityDestroyed(Entity entity)
        {
            RemoveEntity(entity);
        }
        #endregion
    }
}
