
namespace Cat.AI.FSM
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Node
    {
        public const float NODE_SIZE = 80.0f;

        static Node selection = null;
        static bool connecting = false;

        Vector2 position;
        Rect nodeRect;
        Rect textFieldRect;

        string name;
        List<Node> targets = new List<Node>();
        List<Transition> transitions = new List<Transition>();

        public Node(string name, Vector2 position)
        {
            this.name = name;
            Position = position;
        }

        public static Node Selection
        {
            get
            {
                return selection;
            }
            set
            {
                selection = value;
                if (selection == null)
                {
                    connecting = false;
                }
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;

                nodeRect = new Rect(
                    position.x - Node.NODE_SIZE * 0.5f,
                    position.y - Node.NODE_SIZE * 0.5f,
                    Node.NODE_SIZE,
                    Node.NODE_SIZE
                );

                textFieldRect = new Rect(
                    position.x - Node.NODE_SIZE * 0.5f,
                    position.y - Node.NODE_SIZE * 0.2f,
                    Node.NODE_SIZE,
                    Node.NODE_SIZE*0.4f
                );
            }
        }

        /*public ReadOnlyCollection<Node> Targets
        {
            get
            {
                return targets.AsReadOnly();
            }
        }*/

        public ReadOnlyCollection<Transition> Transitions
        {
            get
            {
                return transitions.AsReadOnly();
            }
        }

        public void ConnectTo(Node target)
        {
            Transition transition = new Transition(this, target, false);
            transitions.Add(transition);
        }

        public void OnGUI()
        {

            // First handle the textfield
            Debug.Log(Event.current);

            //name = GUI.TextField(textFieldRect, name, 10);
            //Event.current.Use();
            //return;

            switch (Event.current.type)
            {
                case EventType.mouseDown:

                    // Select the textfield if we press on it
                    if (textFieldRect.Contains(Event.current.mousePosition))
                    {
                        Debug.Log("TEXT FIELD!");
                        name = GUI.TextField(textFieldRect, name, 10);
                        Event.current.Use();
                    }
                    else
                    // Select this node if we press on it
                    if (nodeRect.Contains(Event.current.mousePosition))
                    {
                        selection = this;

                        // If we right-clicked it, enter connect mode
                        if (Event.current.button == 1)
                        {
                            connecting = true;
                        }
                        // If we double-clicked it, rename
                        else if (Event.current.clickCount == 2)
                        {
                            this.name = Input.inputString;
                        }
                        Event.current.Use();
                    }
                    break;

                case EventType.mouseUp:
                    // If we released the mouse button...
                    // ... with no active selection, ignore the event
                    if (selection == null)
                    {
                        break;
                    }
                    // ... while this node was active selection...
                    else if (selection == this)
                    {
                        // ... and we were not in connect mode, clear the selection
                        if (!connecting)
                        {
                            Selection = null;
                            Event.current.Use();
                        }
                    }
                    // ... over this component while in connect mode, connect selection to this node and clear selection
                    else if (connecting && nodeRect.Contains(Event.current.mousePosition))
                    {
                        selection.ConnectTo(this);
                        Selection = null;
                        Event.current.Use();
                    }
                    break;
                case EventType.mouseDrag:
                    // If doing a mouse drag with this component selected...
                    if (selection == this)
                    {
                        // ... and in connect mode, just use the event as we'll be painting the new connection
                        if (connecting)
                        {
                            Event.current.Use();
                        }
                        // ... and not in connect mode, drag the component
                        else
                        {
                            Position += Event.current.delta;
                            Event.current.Use();
                        }
                    }
                    break;
                case EventType.repaint:

                    // The component box
                    if (selection == this) GUI.color = Color.cyan;
                    //GUI.skin.box.Draw(tmpRect, new GUIContent(name), false, false, false, true);
                    GUI.Box(nodeRect, new GUIContent(name));
                    GUI.color = Color.white;

                    name = GUI.TextField(textFieldRect, name, 10);
                    //GUI.skin.box.DrawWithTextSelection(nodeRect, new GUIContent(myString), 0, 0, 4);

                    // The active new connection being created
                    if (selection == this && connecting)
                    {
                        GUI.color = Color.red;
                        NodeDrawUtilities.DrawConnection(position, Event.current.mousePosition);
                        GUI.color = Color.white;
                    }
                    break;
            }
        }

    }


}