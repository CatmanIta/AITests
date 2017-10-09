using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.AI.GOAP
{
    public abstract class Action : MonoBehaviour
    {
        public float cost = 1.0f;

        public Dictionary<string, int> preconditions;
        public Dictionary<string, int> effects;

        void Awake()
        {
            Init();
        }

        public virtual void Init()
        {
            preconditions = new Dictionary<string, int>();
            effects = new Dictionary<string, int>();
        }

        public abstract void Perform(Agent agent);

    }
}
