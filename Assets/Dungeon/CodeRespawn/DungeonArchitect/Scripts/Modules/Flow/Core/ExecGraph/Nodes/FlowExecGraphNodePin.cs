//$ Copyright 2015-21, Code Respawn Technologies Pvt Ltd - All Rights Reserved $//
using UnityEngine;
using DungeonArchitect.Graphs;

namespace DungeonArchitect.Flow.Exec
{
    [System.Serializable]
    public class FlowExecGraphNodePin : GraphPin
    {
        public Vector2 Padding = new Vector2(10, 10);

        public override bool ContainsPoint(Vector2 worldPoint)
        {
            if (PinType == GraphPinType.Input)
            {
                // We don't want the user to touch this pin.  Our logic will connect output-output pins correctly
                return false;
            }

            if (base.ContainsPoint(worldPoint))
            {
                // Make sure it is not inside the body
                var bodyBounds = Node.Bounds;
                bodyBounds.position += Padding;
                bodyBounds.size -= Padding * 2;

                // make sure it is not inside the body
                return !bodyBounds.Contains(worldPoint);
            }
            return false;
        }
    }
}
