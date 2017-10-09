using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.AI.GOAP
{
    public class Action_CollectBranches : Action
    {
        public override void Init()
        {
            base.Init();
            effects["wood"] = 1;
        }

        public override void Perform(Agent agent)
        {
            agent.currentWood += 1;
        }

    }
}
