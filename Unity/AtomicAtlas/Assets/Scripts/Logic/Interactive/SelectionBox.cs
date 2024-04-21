
using Atlas.WorldGen;
using System.Collections.Generic;
using UnityEngine;

namespace Atlas.Logic
{
    public class SelectionBox : MonoBehaviour
    {
        private Vector3 startPosition;
        private Vector3 extents;
        private InteractiveNodeGraph nodeGraph;

        void Start()
        {

        }

        void Update()
        {

        }

        public void SetExtents(Vector3 position)
        {
            extents = position;
            RegenerateBorder();
        }

        private void RegenerateBorder()
        {
            var minX = Mathf.Min(startPosition.x, extents.x);
            var minY = Mathf.Min(startPosition.y, extents.y);
            var maxX = Mathf.Max(startPosition.x, extents.x);
            var maxY = Mathf.Max(startPosition.y, extents.y);

            var pts = new List<Vector3>
            {
                new Vector3(minX, minY, startPosition.z),
                new Vector3(maxX, minY, startPosition.z),
                new Vector3(maxX, maxY, startPosition.z),
                new Vector3(minX, maxY, startPosition.z)
            };

            GetComponent<LineRenderer>().positionCount = 4;
            GetComponent<LineRenderer>().SetPositions(pts.ToArray());
        }

        public void SetPosition(Vector3 position)
        {
            startPosition = position;
            transform.position = position;
        }

        public void SetNodeGraph(InteractiveNodeGraph graph)
        {
            nodeGraph = graph;
        }

        public IEnumerable<InteractiveNode> GetSelectedNodes()
        {
            var mins = new Vector3();
            var maxs = new Vector3();
            var nodes = new List<InteractiveNode>();

            mins.x = extents.x < startPosition.x
                ? extents.x 
                : startPosition.x;

            mins.y = extents.y < startPosition.y
                ? extents.y
                : startPosition.y;

            maxs.x = extents.x > startPosition.x
                ? extents.x
                : startPosition.x;

            maxs.y = extents.y > startPosition.y
                ? extents.y
                : startPosition.y;

            foreach (var node in nodeGraph.Nodes)
            {
                if (ContainsPosition(mins, maxs, node.transform.position))
                {
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        private bool ContainsPosition(Vector3 mins, Vector3 maxs, Vector3 point)
        {
            return point.x >= mins.x && point.y >= mins.y && point.x <= maxs.x && point.y <= maxs.y;
        }
    }
}