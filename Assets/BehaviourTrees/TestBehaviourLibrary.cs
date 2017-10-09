using BehaviorLibrary;
using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Actions;
using BehaviorLibrary.Components.Composites;
using BehaviorLibrary.Components.Conditionals;
using BehaviorLibrary.Components.Decorators;
using BehaviorLibrary.Components.Utility;
using System;
using System.Collections;
using UnityEngine;

public class TestBehaviourLibrary : MonoBehaviour {

    public event Func<bool> testFunc1;
    public event Func<bool> testFunc2;

    bool TestFunc1()
    {
        Debug.Log("TEST FUNC 1 ");
        return false;
    }
    bool TestFunc2()
    {
        Debug.Log("TEST FUNC 2 ");
        return true;
    }


    BehaviorReturnCode TestAction1()
    {
        Debug.Log("TEST ACTION 1");
        return BehaviorReturnCode.Success;
    }
    BehaviorReturnCode TestAction2()
    {
        Debug.Log("TEST ACTION 2");
        return BehaviorReturnCode.Success;
    }

    int IndexFunction()
    {
        return 0;
    }

	void Start () {

        // Try out some delegate variables
        testFunc1 += TestFunc1;
        testFunc2 += TestFunc2;

        // Setup all conditionals and their delegate functions
        Conditional conditional1 = new Conditional(testFunc1);
        Conditional conditional2 = new Conditional(testFunc2);

        // Setup all actions and their delegate functions
        BehaviorAction action1 = new BehaviorAction(TestAction2);
        BehaviorAction action2 = new BehaviorAction(TestAction2);

        // Setup a conditional branch
        Selector conditionalSelector = new Selector(new Inverter(conditional1), new Inverter(action1), action2);

        // Setup an initilization branch
        Sequence initializeSequence = new Sequence(action1, conditionalSelector);

        // Setup root node, choose initialization phase or pathing/movement phase
        IndexSelector root = new IndexSelector(IndexFunction, initializeSequence);

        // Set a reference to the root
        Behavior behavior = new Behavior(root);

        // To execute the behavior
        behavior.Behave();
	}
	
}
