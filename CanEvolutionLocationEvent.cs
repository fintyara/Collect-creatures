using System.Collections.Generic;
using UnityEngine;


namespace CollectCreatures
{
    public class CanEvolutionLocationEvent : LocationEvent
    {
        #region VAR
        [SerializeField] protected List<EvolutionState> evolutionAiStates = new List<EvolutionState>();
        #endregion

        #region MONO
        private void Start()
        {
            spawnPoints = GetComponent<ShuffledSpawnPoints>();
            if (!spawnPoints)
                Debug.Log("Need points");
        }
        void Update()
        {
            UpdateEvent();       
        }
        #endregion

        #region FUNC
        protected override void AddEntity(Entity entity)
        {
            EvolutionState canEvolution = entity.GetComponentInChildren<EvolutionState>();

            if (canEvolution != null)
            {
                curCount++;
                entities.Add(entity);
                evolutionAiStates.Add(canEvolution);
            }
        }
        protected override void RemoveEntity(Entity entity)
        {
            if (entities.Contains(entity))
            {
                curCount--;
                entities.Remove(entity);
                evolutionAiStates.Remove(entity.GetComponentInChildren<EvolutionState>());
            }
        }
        public virtual Entity GetNearCreatureByLvlMaxDistance(Vector3 originalPos, float maxDistance, int neededLvl)
        {
            selected.Clear();
            
            for (int i = 0; i < entities.Count; i++)
            {
                if (evolutionAiStates[i].Lvl == neededLvl)
                    selected.Add(entities[i]);
            }

            return selected.Count > 0 ? Utility.GetNearEntityMaxDistance(selected, originalPos, maxDistance) : null;
        }
        #endregion
    }
}
