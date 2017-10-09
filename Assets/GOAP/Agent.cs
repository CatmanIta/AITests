using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cat.AI.GOAP
{
    public class Agent : MonoBehaviour
    {
        private List<Action> availableActions;
        private Planner planner;

        // Goal
        public int targetWood = 30;
        public int targetHouses = 1;

        // State
        public int currentWood = 0;
        public int axeDurability = 0;
        public int houses = 0;

        void Start()
        {
            availableActions = GetComponents<Action>().ToList();
            planner = new Planner();
            StartCoroutine(LifeCO());
        }

        IEnumerator LifeCO()
        {
            var lifeGoal = new Dictionary<string, int>();
            lifeGoal.Add("wood", targetWood);
            lifeGoal.Add("houses", targetHouses);

            var currentState = new Dictionary<string, int>();
            currentState.Add("wood", currentWood);
            currentState.Add("durability", axeDurability);
            currentState.Add("houses", houses);
            Queue<Action> plan = planner.GetPlan(availableActions, currentState, lifeGoal);
            Debug.Log("We have a plan with " + plan.Count + " actions");

            while (true)
            {
                //Debug.Log("Current wood: " + currentState["wood"]);
                //Debug.Log("Lifegoal wood: " + lifeGoal["wood"]);
                if (currentState["wood"] >= lifeGoal["wood"]
                    && currentState["houses"] >= lifeGoal["houses"])
                {
                    break;
                }

                if (plan.Count > 0)
                {
                    var chosenAction = plan.Dequeue();
                    Debug.LogWarning("Performing action " + chosenAction.GetType().Name);
                    chosenAction.Perform(this);

                    currentState["wood"] = currentWood;
                    currentState["durability"] = axeDurability;
                    currentState["houses"] = houses;
                }
                else
                {
                    Debug.LogWarning("No action was planned!");
                }
                yield return new WaitForSeconds(0.1f);
            }

            Debug.Log("We got our wood!");
        }
    }

}
