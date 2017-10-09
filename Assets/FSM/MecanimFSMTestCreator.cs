using UnityEngine;
using System.Collections;

using UnityEditor;
using UnityEditor.Animations;

// Creates a complete FSM from code
public class MecanimFSMTestCreator : MonoBehaviour {
    
    [ContextMenu("Create Machine")]
	void CreateMachine () { 

        // Creates the controller
        var controller = AnimatorController.CreateAnimatorControllerAtPath ("Assets/Mecanim/StateMachineTransitions.controller");
      
        // Add parameters
        controller.AddParameter("TransitionNow", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("Reset", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("GotoB1", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("GotoC", AnimatorControllerParameterType.Trigger);
        
        // Add StateMachines
        var rootStateMachine = controller.layers[0].stateMachine;
        var stateMachineA = rootStateMachine.AddStateMachine("smA");
        var stateMachineB = rootStateMachine.AddStateMachine("smB");
        var stateMachineC = stateMachineB.AddStateMachine("smC");

        // Add States
        var stateA1 = stateMachineA.AddState("stateA1");
        var stateB1 = stateMachineB.AddState("stateB1");
        var stateB2 = stateMachineB.AddState("stateB2");
        stateMachineC.AddState("stateC1");
        var stateC2 = stateMachineC.AddState("stateC2"); // don’t add an entry transition, should entry to state by default

        // Add Transitions
        var exitTransition = stateA1.AddExitTransition();
        exitTransition.AddCondition(AnimatorConditionMode.If, 0, "TransitionNow");
        exitTransition.duration = 0;

        var resetTransition = stateMachineA.AddAnyStateTransition(stateA1);
        resetTransition.AddCondition(AnimatorConditionMode.If, 0, "Reset");
        resetTransition.duration = 0;

        var transitionB1 = stateMachineB.AddEntryTransition(stateB1);
        transitionB1.AddCondition(AnimatorConditionMode.If, 0, "GotoB1");
        stateMachineB.AddEntryTransition(stateB2);
        stateMachineC.defaultState = stateC2;
        var exitTransitionC2 = stateC2.AddExitTransition();
        exitTransitionC2.AddCondition(AnimatorConditionMode.If, 0, "TransitionNow");
        exitTransitionC2.duration = 0;

        var stateMachineTransition = rootStateMachine.AddStateMachineTransition(stateMachineA, stateMachineC);
        stateMachineTransition.AddCondition(AnimatorConditionMode.If, 0, "GotoC");
        rootStateMachine.AddStateMachineTransition(stateMachineA, stateMachineB);
	}
	
}
