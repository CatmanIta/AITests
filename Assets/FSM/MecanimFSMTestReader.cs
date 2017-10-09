using UnityEngine;
using System.Collections;

using UnityEditor;
using UnityEditor.Animations;

// Reads a Mecanim-created FSM and prints it
[RequireComponent(typeof(Animator))]
public class MecanimFSMTestReader : MonoBehaviour {

    [ContextMenu("Read Machine")]
	void ReadMachine () { 

        var animator = GetComponent<Animator>();

        // TODO: finish this
	}
	
}
