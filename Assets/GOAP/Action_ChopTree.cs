using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.AI.GOAP
{
    public class Action_ChopTree : Action
    {
        public override void Init()
        {
            base.Init();
            preconditions["durability"] = 1;
            effects["wood"] = 4;
            effects["durability"] = -1;
        }

        public override void Perform(Agent agent)
        {
            agent.currentWood += 4;
            agent.axeDurability -= 1;
        }
    }
}
