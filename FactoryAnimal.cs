using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperMaxim.Messaging;

namespace CollectCreatures
{
    public class FactoryAnimal : MonoBehaviour
    {
        [SerializeField] float _heightSpawn = 0.5f;
        [SerializeField] Instancer animalInstancer;
        [SerializeField] Instancer itemInstancer;




        public void SpawnAnimal(AnimalSpawnPayload animalSpawnPayload)
        {
            Debug.Log("1");
            if (animalInstancer)
            {
                Debug.Log("2");
                animalInstancer.CreateGO(animalSpawnPayload.animalItem.Prefab, new Vector3(0, _heightSpawn, 0));
            }
        }
        public void SpawnItem(ItemSpawnPayload itemSpawnPayload)
        {
            if (itemInstancer)
                itemInstancer.CreateGO(itemSpawnPayload.item.Prefab, new Vector3(0, _heightSpawn, 0));
        }




        void Update()
        {

        }
        private void OnEnable()
        {
            Messenger.Default.Subscribe<ItemSpawnPayload>(SpawnItem);
            Messenger.Default.Subscribe<AnimalSpawnPayload>(SpawnAnimal);
        }
        private void OnDisable()
        {
            Messenger.Default.Unsubscribe<ItemSpawnPayload>(SpawnItem);
            Messenger.Default.Unsubscribe<AnimalSpawnPayload>(SpawnAnimal);
        }
    }
}
