
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
        private GameObject interactiveConnectionPrefab;

        [SerializeField]
        private GameObject label;

        public IEnumerable<InteractiveNode> Nodes => nodes;

        private List<InteractiveConnection> connections;
        private List<InteractiveNode> nodes;
        private Vector3 mins;
        private Vector3 maxs;

        private const float PADDING = 400f;
        private const float EDGE_TOLERANCE = 12f;

        public void Initialize(World world, WorldPlane worldPlane, int offset, NodeGraphSortType sortType)
        {
            nodes = new List<InteractiveNode>();
            connections = new List<InteractiveConnection>();

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
            RemoveConnections(connections.Count);
            RemoveNodes(nodes.Count);
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
            ResizePools(worldPlane.Nodes.Count, worldPlane.Connections.Count);

            for (int i = 0; i < worldPlane.Nodes.Count; i++)
            {
                nodes[i].SetNodeGraph(this);
                nodes[i].SetIsCaveNode(worldPlane.IsCave);
                nodes[i].SetNode(worldPlane.Nodes[i]);
                nodes[i].ResetInteractiveConnections();
                nodes[i].SetPosition(world);
            }

            for (int i = 0; i < worldPlane.Connections.Count; i++)
            {
                connections[i].SetConnection(worldPlane.Connections[i]);
                connections[i].SetNode1(null);
                connections[i].SetNode2(null);
                connections[i].SetPosition(world);
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
        }

        private void ResizePools(int count, int connectionCount)
        {
            if (nodes.Any())
            {
                if (nodes.Count > count)
                {
                    RemoveNodes(nodes.Count - count);
                }
                else if (nodes.Count < count)
                {
                    AddNodes(count - nodes.Count);
                }
            }
            else
            {
                AddNodes(count);
            }

            if (connections.Any())
            {
                if (connections.Count > connectionCount)
                {
                    RemoveConnections(nodes.Count - connectionCount);
                }
                else if (connections.Count < connectionCount)
                {
                    AddConnections(connectionCount - nodes.Count);
                }
            }
            else
            {
                AddConnections(connectionCount);
            }
        }

        private void RemoveConnections(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var connection = connections[0];
                connections.RemoveAt(0);

                Destroy(connection.gameObject);
            }
        }

        private void AddConnections(int count)
        {
            for (int i = 0; i < count; i++)
            {
                connections.Add(Instantiate(interactiveConnectionPrefab, gameObject.transform).GetComponent<InteractiveConnection>());
            }
        }

        private void RemoveNodes(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var node = nodes[0];
                nodes.RemoveAt(0);

                Destroy(node.gameObject);
            }
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
                connection.UpdatePosition();
                connection.UpdateVisuals();

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

        private void AddNodes(int count)
        {
            for (int i = 0; i < count; i++)
            {
                nodes.Add(Instantiate(interactiveNodePrefab, gameObject.transform).GetComponent<InteractiveNode>());
            }
        }
    }
}


