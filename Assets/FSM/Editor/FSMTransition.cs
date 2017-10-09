
namespace Cat.AI.FSM
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Transition
    {
        public Node nodeFrom;
        public Node nodeTo;
        public bool oneWay = false;


        public Transition(Node nodeFrom, Node nodeTo, bool oneWay)
        {
            this.nodeFrom = nodeFrom;
            this.nodeTo = nodeTo;
            this.oneWay = oneWay;
        }

    }


}