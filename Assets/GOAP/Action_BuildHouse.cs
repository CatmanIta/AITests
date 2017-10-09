using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.AI.GOAP
{
    public class Action_BuildHouse : Action
    {
        public override void Init()
        {
            base.Init();
            preconditions["wood"] = 32;
            effects["wood"] = -32;
            effects["house"] = 1;
        }

        public override void Perform(Agent agent)
        {
            agent.currentWood -= 32;
            agent.houses += 1;
        }

    }
}
