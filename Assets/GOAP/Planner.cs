using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.AI.GOAP
{
    public class Planner
    {
        private static int maxSubTreeSize = 0;
        private int currentSubTreeSize = 0;

        public Queue<Action> GetPlan(List<Action> availableActions,
                                      Dictionary<string,int> startState,
                                      Dictionary<string,int> goal)
        {
            List<Action> usableActions = new List<Action>();
            foreach (Action a in availableActions)
            {
                usableActions.Add(a);
            }

            // Build the tree
            currentSubTreeSize = 0;
            List<Node> leaves = new List<Node>();
            Node startNode = new Node(null, 0, startState, null);
            bool canFindPlan = BuildSubTree(startNode, leaves, usableActions, goal);
            if (!canFindPlan)
            {
                return new Queue<Action>();
            }

            // Find the best action
            Node cheapest = null;
            foreach (Node leaf in leaves)
            {
                if (cheapest == null)
                    cheapest = leaf;
                else {
                    if (leaf.totalCost < cheapest.totalCost)
                        cheapest = leaf;
                }
            }

            // Find the strategy from the leaf back to the root
            List<Action> result = new List<Action>();
            Node n = cheapest;
            while (n != null)
            {
                if (n.action != null)
                {
                    result.Insert(0, n.action); // insert the action in the front
                }
                n = n.parent;
            }

            Queue<Action> plan = new Queue<Action>();
            foreach (Action a in result)
            {
                plan.Enqueue(a);   
            }

            Debug.Log("Max sub tree size: " + maxSubTreeSize);

            return plan;
        }

        private bool BuildSubTree(Node parent, List<Node> leaves, List<Action> usableActions, Dictionary<string, int> goal)
        {
            currentSubTreeSize++;
            if (currentSubTreeSize > maxSubTreeSize) maxSubTreeSize = currentSubTreeSize;

            bool foundSolution = false;
            foreach (Action action in usableActions)
            {
                if (MatchesConditions(action.preconditions, parent.state))
                {
                    Dictionary<string, int> newState = ApplyState(parent.state, action.effects);
                    Node node = new Node(parent, parent.totalCost + action.cost, newState, action);

                    if (MatchesConditions(goal, newState))
                    {
                        // we found a solution!
                        leaves.Add(node);
                        foundSolution = true;
                    }
                    else
                    {
                        //List<Action> remainingActions = RemoveAction(usableActions, action);
                        if (BuildSubTree(node, leaves, usableActions, goal))
                        {
                            foundSolution = true;
                        }
                    }
                }
            }
            return foundSolution;
        }

        private List<Action> RemoveAction(List<Action> actions, Action removeMe)
        {
            List<Action> subset = new List<Action>();
            foreach (Action a in actions)
            {
                if (!a.Equals(removeMe))
                    subset.Add(a);
            }
            return subset;
        }

        private bool MatchesConditions(Dictionary<string, int> conditions, Dictionary<string, int> state)
        {
            bool allMatch = true;
            foreach (KeyValuePair<string, int> c in conditions)
            {
                bool match = false;
                foreach (KeyValuePair<string, int> s in state)
                {
                    if (s.Key.Equals(c.Key))
                    {
                        // Zero-check condition
                        if (c.Value == 0) match = s.Value == 0;
                        // Positive condition
                        else if (c.Value > 0) match = s.Value >= c.Value;
                        break;
                    }
                }
                if (!match)
                    allMatch = false;
            }
            return allMatch;
        }

        private Dictionary<string, int> ApplyState(Dictionary<string, int> state, Dictionary<string, int> change)
        {
            // Copy the current state
            Dictionary<string, int> newState = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> s in state)
            {
                newState.Add(s.Key, s.Value);
            }

            // Add the change
            foreach (KeyValuePair<string, int> c in change)
            {
                if (!newState.ContainsKey(c.Key))
                    newState[c.Key] = 0;

                newState[c.Key] += c.Value;
            }
            return newState;
        }

        private class Node
        {
            public Node parent;
            public float totalCost;
            public Dictionary<string, int> state;
            public Action action;

            public Node(Node parent, float totalCost, Dictionary<string, int> state, Action action)
            {
                this.parent = parent;
                this.totalCost = totalCost;
                this.state = state;
                this.action = action;
            }
        }

    }
}
