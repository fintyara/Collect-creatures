using System.Collections;
using System.Collections.Generic;
using CollectCreatures;
using UnityEngine;

namespace CollectCreatures
{
    public interface IHaveTarget
    {
        void SetTarget(Entity entity);
        bool NeedTarget();
        float GetDistanceFind();
        AttackType GetAttackType();
    }
}
