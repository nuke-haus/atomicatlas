
using UnityEngine;
using Atlas.WorldGen;
using Atlas.Core;
using Codice.CM.Client.Differences;

namespace Atlas.Logic
{
    public class InteractiveConnection : MonoBehaviour
    {
        public bool IsWrapConnection { get; private set; }
        public Connection Connection { get; private set; }
        public InteractiveNode Node1 { get; private set; }
        public InteractiveNode Node2 { get; private set; }
        public InteractiveNodeGhost Node1Ghost { get; private set; }
        public InteractiveNodeGhost Node2Ghost { get; private set; }

        [SerializeField]
        private LineRenderer lineRenderer1;

        [SerializeField]
        private LineRenderer lineRenderer2;

        [SerializeField]
        private GameObject sphere1;

        [SerializeField]
        private GameObject sphere2;

        private Vector2 worldSize;
        private const float WRAP_CONNECTION_LENGTH = 100f;

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

        public void UpdateVisuals()
        {
            if (Node1 == null || Node2 == null)
            {
                lineRenderer1.SetPositions(new Vector3[] { });
                lineRenderer2.SetPositions(new Vector3[] { });
                return;
            }
            if (IsWrapConnection && (Node1Ghost == null || Node2Ghost == null))
            {
                lineRenderer1.SetPositions(new Vector3[] { });
                lineRenderer2.SetPositions(new Vector3[] { });
                return;
            }

            var pos1 = Node1.transform.position;
            var pos2 = Node2.transform.position;

            if (IsWrapConnection)
            {
                var ghost1Pos = Node1Ghost.transform.position;
                var ghost2Pos = Node2Ghost.transform.position;

                lineRenderer1.SetPositions(new Vector3[] { pos1, ghost2Pos });
                lineRenderer2.SetPositions(new Vector3[] { pos2, ghost1Pos });
            }
            else
            {
                lineRenderer1.SetPositions(new Vector3[] { pos1, pos2 });
                lineRenderer2.SetPositions(new Vector3[] { });
            }
        }

        public void UpdatePosition()
        {
            if (Node1 == null || Node2 == null)
            {
                return;
            }
            if (IsWrapConnection && (Node1Ghost == null || Node2Ghost == null))
            {
                return;
            }

            var pos1 = Node1.transform.position;
            var pos2 = Node2.transform.position;

            if (IsWrapConnection)
            {
                var ghost1Pos = Node1Ghost.transform.position;
                var ghost2Pos = Node2Ghost.transform.position;

                sphere1.transform.position = (pos1 + ghost2Pos) / 2;
                sphere2.transform.position = (pos2 + ghost1Pos) / 2;
            }
            else
            {
                transform.position = (pos1 + pos2) / 2;
                sphere1.transform.localPosition = Vector3.zero;
                sphere2.transform.localPosition = Vector3.zero;
            }
        }

        public void SetPosition(World world)
        {
            worldSize = world.WorldSize;
            transform.localPosition = new Vector3(Connection.ConnectionCenter.x * world.WorldSize.x, Connection.ConnectionCenter.y * world.WorldSize.y, 0f);
        }

        public void SetNode1Ghost(InteractiveNodeGhost nodeGhost)
        {
            Node1Ghost = nodeGhost;

            UpdatePosition();
            UpdateVisuals();
        }

        public void SetNode2Ghost(InteractiveNodeGhost nodeGhost)
        {
            Node2Ghost = nodeGhost;

            UpdatePosition();
            UpdateVisuals();
        }

        public void SetNode1(InteractiveNode node)
        {
            Node1 = node;

            UpdatePosition();
            UpdateVisuals();
        }

        public void SetNode2(InteractiveNode node)
        {
            Node2 = node;

            UpdatePosition();
            UpdateVisuals();
        }

        public void SetInteractiveNode(InteractiveNode node)
        {
            if (Node1 == null)
            {
                Node1 = node;
            }
            else
            {
                Node2 = node;
            }

            UpdatePosition();
            UpdateVisuals();
        }

        public void SetConnection(Connection c)
        {
            IsWrapConnection = c.IsWrapConnection;
            Connection = c;
            UpdateVisuals();
        }
    }
}
