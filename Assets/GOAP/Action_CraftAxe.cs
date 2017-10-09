using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.AI.GOAP
{
    public class Action_CraftAxe : Action
    {
        public override void Init()
        {
            base.Init();
            preconditions["durability"] = 0;
            effects["wood"] = -4;
            effects["durability"] = 5;
        }

        public override void Perform(Agent agent)
        {
            agent.currentWood -= 4;
            agent.axeDurability += 5;
        }

    }
}
