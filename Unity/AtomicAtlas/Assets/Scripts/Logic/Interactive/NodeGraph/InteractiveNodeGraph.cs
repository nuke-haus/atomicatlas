
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Atlas.WorldGen;

namespace Atlas.Logic
{
    public enum NodeGraphSortType
    {
        Y_AXIS,
        Z_AXIS
    }

    public class InteractiveNodeGraph : MonoBehaviour
    {
        [SerializeField]
        private GameObject interactiveNodePrefab;

        [SerializeField]
        private GameObject interactiveNodeGhostPrefab;

        [SerializeField]
        private GameObject interactiveConnectionPrefab;

        [SerializeField]
        private GameObject label;

        public IEnumerable<InteractiveNode> Nodes => nodes;

        private List<InteractiveConnection> connections;
        private List<InteractiveNode> nodes;
        private List<InteractiveNodeGhost> nodeGhosts;
        private Vector3 mins;
        private Vector3 maxs;

        private const float PADDING = 400f;
        private const float EDGE_TOLERANCE = 12f;

        public void Initialize(World world, WorldPlane worldPlane, int offset, NodeGraphSortType sortType)
        {
            nodes = new List<InteractiveNode>();
            connections = new List<InteractiveConnection>();
            nodeGhosts = new List<InteractiveNodeGhost>();

            transform.position = new Vector3(0f, (offset * (world.WorldSize.y + PADDING)), 0f);

            if (sortType == NodeGraphSortType.Z_AXIS)
            {
                transform.position = new Vector3(0f, 0f, (offset * PADDING));
            }

            RegenerateLabel(world, worldPlane, offset, sortType);
            RegenerateBorder(world, offset, sortType);
            RegenerateWorld(world, worldPlane, offset, sortType);
        }

        public void Destroy()
        {
            ClearPools();
            Destroy(gameObject);
        }

        public bool ContainsPosition(Vector3 point, bool includeTolerance = true)
        {
            var tolerance = includeTolerance
                ? EDGE_TOLERANCE
                : 0f;

            return point.x >= (mins.x + tolerance) && point.y >= (mins.y + tolerance) && point.x <= (maxs.x - tolerance) && point.y <= (maxs.y - tolerance);
        }

        private void RegenerateLabel(World world, WorldPlane worldPlane, int offset, NodeGraphSortType sortType)
        {
            label.GetComponent<TextMeshPro>().text = worldPlane.Name.ToUpper();

            var maxs = new Vector3(0f, world.WorldSize.y, 0f);

            if (sortType == NodeGraphSortType.Z_AXIS)
            {
                maxs = new Vector3(0f, world.WorldSize.y, (offset * PADDING));
            }

            label.transform.localPosition = maxs;
        }

        private void RegenerateBorder(World world, int offset, NodeGraphSortType sortType)
        {
            mins = new Vector3(0f, (offset * (world.WorldSize.y + PADDING)), 0f);
            maxs = new Vector3(world.WorldSize.x, (offset * (world.WorldSize.y + PADDING)) + world.WorldSize.y, 0f);

            if (sortType == NodeGraphSortType.Z_AXIS)
            {
                mins = new Vector3(0f, 0f, (offset * PADDING));
                maxs = new Vector3(world.WorldSize.x, world.WorldSize.y, (offset * PADDING));
            }

            var pts = new List<Vector3>
            {
                mins,
                new Vector3(mins.x, maxs.y, maxs.z),
                maxs,
                new Vector3(maxs.x, mins.y, mins.z)
            };

            GetComponent<LineRenderer>().SetPositions(pts.ToArray());
        }

        private void RegenerateWorld(World world, WorldPlane worldPlane, int offset, NodeGraphSortType sortType)
        {
            ClearPools();

            for (int i = 0; i < worldPlane.Nodes.Count; i++)
            {
                var node = AddNode();
                node.SetNodeGraph(this);
                node.SetIsCaveNode(worldPlane.IsCave);
                node.SetNode(worldPlane.Nodes[i]);
                node.SetPosition(world);
            }

            for (int i = 0; i < worldPlane.Connections.Count; i++)
            {
                var connection = AddConnection();
                connection.SetConnection(worldPlane.Connections[i]);
                connection.SetNode1(null);
                connection.SetNode2(null);
                connection.SetPosition(world);
            }

            foreach (var node in nodes)
            {
                foreach (var connection in connections)
                {
                    if (node.Node.HasConnection(connection.Connection))
                    {
                        node.AddInteractiveConnection(connection);
                        connection.SetInteractiveNode(node);
                    }
                }
            }

            foreach (var node in nodes)
            {
                foreach (var connection in node.Connections)
                {
                    if (connection.IsWrapConnection)
                    {
                        var otherNode = connection.Node1 == node
                            ? connection.Node2
                            : connection.Node1;

                        var ghost = AddNodeGhost();
                        ghost.SetParentNode(node);
                        ghost.SetConnection(connection);
                        ghost.SetPosition(world, otherNode);
                        node.AddNodeGhost(ghost);

                        if (node == connection.Node1)
                        {
                            connection.SetNode2Ghost(ghost);
                        }
                        else
                        {
                            connection.SetNode1Ghost(ghost);
                        }
                    }
                }
            }
        }

        private InteractiveNode AddNode()
        {
            var node = Instantiate(interactiveNodePrefab, gameObject.transform).GetComponent<InteractiveNode>();
            nodes.Add(node);
            return node;
        }

        private InteractiveNodeGhost AddNodeGhost()
        {
            var ghost = Instantiate(interactiveNodeGhostPrefab, gameObject.transform).GetComponent<InteractiveNodeGhost>();
            nodeGhosts.Add(ghost);
            return ghost;
        }

        private InteractiveConnection AddConnection()
        {
            var conn = Instantiate(interactiveConnectionPrefab, gameObject.transform).GetComponent<InteractiveConnection>();
            connections.Add(conn);
            return conn;
        }

        private void ClearPools()
        {
            foreach (var node in nodes)
            {
                Destroy(node.gameObject);
            }
            nodes.Clear();

            foreach (var connection in connections)
            {
                Destroy(connection.gameObject);
            }
            connections.Clear();

            foreach (var ghost in nodeGhosts)
            {
                Destroy(ghost.gameObject);   
            }
            nodeGhosts.Clear();
        }

        public void ConnectNodes(InteractiveNode node1, InteractiveNode node2)
        {
            if (!node1.HasConnection(node2))
            {
                var conn = new Connection(node1.Node, node2.Node);
                node1.Node.AddConnection(conn);
                node2.Node.AddConnection(conn);

                var connection = Instantiate(interactiveConnectionPrefab, gameObject.transform).GetComponent<InteractiveConnection>();
                connection.SetConnection(conn);
                connection.SetNode1(node1);
                connection.SetNode2(node2);

                node1.AddInteractiveConnection(connection);
                node2.AddInteractiveConnection(connection);

                connections.Add(connection);
            }
        }

        public void DeleteConnections(IEnumerable<InteractiveConnection> connectionsToRemove)
        {
            foreach (var connection in connectionsToRemove)
            {
                connections.Remove(connection);
                connection.Node1.RemoveInteractiveConnection(connection);
                connection.Node2.RemoveInteractiveConnection(connection);

                Destroy(connection.gameObject);
            }
        }

        public void DeleteNodes(IEnumerable<InteractiveNode> nodesToRemove)
        {
            var connectionsToDelete = new HashSet<InteractiveConnection>();

            foreach (var node in nodesToRemove)
            {
                nodes.Remove(node);

                foreach (var conn in node.Connections)
                {
                    connectionsToDelete.Add(conn);
                }

                Destroy(node.gameObject);
            }

            foreach (var conn in connectionsToDelete)
            {
                connections.Remove(conn);

                Destroy(conn.gameObject);
            }
        }

        public void AddNode(Vector3 position)
        {
            var node = Instantiate(interactiveNodePrefab, gameObject.transform).GetComponent<InteractiveNode>();
            node.transform.position = position;
            node.SetNode(new Node(Vector2.zero, WorldGen.Terrain.PLAINS));
            node.SetNodeGraph(this);

            nodes.Add(node);
        }
    }
}


