
namespace Cat.AI.FSM
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class FSMNodeEditor : EditorWindow
    {

        // Editor parameters
        public const string editorName = "FSM";
        public const string nodeName = "State";
        public const string menuItemLabel = "Window/" + editorName + " Editor";

        List<Node> nodes = new List<Node>();

        [MenuItem(menuItemLabel)]
        static void Launch()
        {
            GetWindow<FSMNodeEditor>().title = editorName;
        }

        void OnGUI()
        {
            GUILayout.Label("Double click to create a " + editorName, EditorStyles.wordWrappedMiniLabel);

            // Render all connections first
            if (Event.current.type == EventType.repaint)
            {
                foreach (Node node in nodes)
                {
                    foreach (Transition transition in node.Transitions)
                    {
                        NodeDrawUtilities.DrawConnection(transition.nodeFrom, transition.nodeTo);
                    }
                }
            }

            GUI.changed = false;

            // First handle all nodes
            foreach (Node node in nodes)
            {
                node.OnGUI();
            }

            // If we have a selection, we're doing an operation which requires an update each mouse move
            wantsMouseMove = Node.Selection != null;
            switch (Event.current.type)
            {
                case EventType.mouseUp:
                    // If we had a mouse up event which was not handled by the nodes, clear our selection
                    Node.Selection = null;
                    Event.current.Use();
                    break;
                case EventType.mouseDown:
                    // If we double-click and no node handles the event, create a new node there
                    if (Event.current.clickCount == 2)
                    {
                        Node.Selection = new Node(nodeName + " " + nodes.Count, Event.current.mousePosition);
                        nodes.Add(Node.Selection);
                        Event.current.Use();
                    }
                    break;
            }

            // Repaint if we changed anything
            if (GUI.changed)
            {
                Repaint();
            }
        }
    }

}