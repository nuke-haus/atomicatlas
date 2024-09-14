
using System.Collections.Generic;
using UnityEngine;
using Atlas.Core;
using Atlas.WorldGen;

namespace Atlas.Logic
{
    public class InteractiveNodeGhost : MonoBehaviour
    {
        public InteractiveNodeGraph ParentNodeGraph { get; private set; }
        public InteractiveNode ParentNode { get; private set; }
        public InteractiveConnection ParentConnection { get; private set; }

        [SerializeField]
        private MeshCollider meshCollider;

        [SerializeField]
        private MeshFilter meshFilter;

        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private GameObject meshObject;

        private List<Vector3> polygonShape = new();

        void Start()
        {
            SetOutlineVisible(false);
        }

        void Update()
        {

        }

        private void FixedUpdate()
        {

        }

        public void SetOutlineVisible(bool visible)
        {
            var outline = gameObject.GetComponent<Outline>();
            outline.OutlineWidth = visible 
                ? 2.0f 
                : 0.0f;
        }

        /*public void TrySetPosition(Vector3 position)
        {
            if (ParentNodeGraph.ContainsPosition(position))
            {
                transform.position = position;
            }
        }*/

        public void SetPosition(World world, InteractiveNode connectingNode)
        {
            var threshold = world.WorldSize * 0.5f;
            var startPosition = ParentNode.transform.position;
            var nodePosition = connectingNode.transform.position;
            var addedVector = Vector3.zero;

            if (Mathf.Abs(nodePosition.x - startPosition.x) > threshold.x)
            {
                addedVector.x = nodePosition.x < startPosition.x
                    ? -world.WorldSize.x
                    : world.WorldSize.x;
            }
            if (Mathf.Abs(nodePosition.y - startPosition.y) > threshold.y)
            {
                addedVector.y = nodePosition.y < startPosition.y
                    ? -world.WorldSize.y
                    : world.WorldSize.y;
            }

            transform.position = startPosition + addedVector;
        }

        public void SetConnection(InteractiveConnection connection)
        {
            ParentConnection = connection;
        }

        public void SetParentNode(InteractiveNode node)
        {
            ParentNode = node;
        }

        public void SetNodeGraph(InteractiveNodeGraph nodeGraph)
        {
            ParentNodeGraph = nodeGraph;
        }
    }
}


