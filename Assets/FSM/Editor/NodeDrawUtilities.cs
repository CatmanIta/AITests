
namespace Cat.AI.FSM
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections;

    public class NodeDrawUtilities : MonoBehaviour
    {
        // Render a node connection between the two given nodes
        public static void DrawConnection(Node from, Node to)
        {
            bool left = from.Position.x > to.Position.x;
            int dir = left ? -1 : 1;

            Vector3 fromP = from.Position;
            fromP.x += dir * 0.5f * Node.NODE_SIZE;
            Vector3 toP = to.Position;
            toP.x -= dir * 0.5f * Node.NODE_SIZE;

            DrawConnection(from.Position, to.Position);
        }


        // Render a node connection between the two given nodes
        public static void DrawConnection(Vector2 from, Vector2 to)
        {
            bool left = from.x > to.x;
            int dir = left ? -1 : 1;

            Handles.DrawBezier(
                new Vector3(from.x, from.y, 0.0f),
                new Vector3(to.x, to.y, 0.0f),
                new Vector3(from.x, from.y, 0.0f) + Vector3.right * 50.0f * dir,
                new Vector3(to.x, to.y, 0.0f) + Vector3.right * -50.0f * dir,
                GUI.color,
                null,
                2.0f
            );

        }
    }

}