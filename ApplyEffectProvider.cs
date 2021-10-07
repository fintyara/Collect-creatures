using UnityEngine;

namespace CollectCreatures
{
    public class ApplyEffectProvider 
    {
        public bool Provide(Entity original, Entity target, AttackType attackType)
        {
            var go =
                GameObject.Instantiate(attackType.EffectPref, target.transform, true);
            
            var effect = go.GetComponent<IEffect>();

            if (effect == null) 
                return false;
            
            effect.Init(original, target);
            
            return true;
        }
    }
}
