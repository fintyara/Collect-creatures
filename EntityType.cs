using System.Collections;
using System.Collections.Generic;
using CollectCreatures;
using UnityEngine;

namespace CollectCreatures
{
    [CreateAssetMenu(menuName = "Types/EntityType")]
    public class EntityType : ScriptableObject
    {
        #region VAR
        [SerializeField] private List<AttackType> _attackTypes = new List<AttackType>();
        [SerializeField] private List<AttackType> _vulnerableTypes = new List<AttackType>();

        public List<AttackType> AtttackTypes { get => _attackTypes; }
        public List<AttackType> VulnerableTypes { get => _vulnerableTypes; }

        public virtual bool HaveVulnerable(List<AttackType> attackTypes)
        {
            for (int i = 0; i < _vulnerableTypes.Count; i++)
            {
                if (attackTypes.Contains(_vulnerableTypes[i]))
                    return true;
            }
            
            return false;
        }
        #endregion
    }
}
