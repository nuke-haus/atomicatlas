
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private List<InteractiveConnection> connections;
    private List<InteractiveNode> nodes;

    private const float PADDING = 10f;

    public void Initialize(World world, WorldPlane worldPlane, int offset, NodeGraphSortType sortType)
    {
        nodes = new List<InteractiveNode>();
        connections = new List<InteractiveConnection>();
        RegenerateBorder(world, offset, sortType);
        RegenerateWorld(world, worldPlane);

        transform.position = new Vector3(0f, (offset * (world.WorldSize.y + PADDING)), 0f);
    }

    public void Destroy()
    {
        RemoveConnections(connections.Count);
        RemoveNodes(nodes.Count);
        Destroy(gameObject);
    }

    private void RegenerateBorder(World world, int offset, NodeGraphSortType sortType)
    {
        var mins = new Vector3(0f, (offset * (world.WorldSize.y + PADDING)), 0f);
        var maxs = new Vector3(world.WorldSize.x, (offset * (world.WorldSize.y + PADDING)) + world.WorldSize.y, 0f);

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

    private void RegenerateWorld(World world, WorldPlane worldPlane)
    {
        ResizePools(worldPlane.Nodes.Count, worldPlane.Connections.Count);

        for (int i = 0; i < worldPlane.Nodes.Count; i++)
        {
            nodes[i].SetNode(worldPlane.Nodes[i]);
            nodes[i].ResetInteractiveConnections();
            nodes[i].UpdateNodePosition(world);
        }

        for (int i = 0; i < worldPlane.Connections.Count; i++)
        {
            connections[i].SetConnection(worldPlane.Connections[i]);
            connections[i].SetNode1(null);
            connections[i].SetNode2(null);
            connections[i].UpdateConnectionPosition(world);
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

    private void AddNodes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            nodes.Add(Instantiate(interactiveNodePrefab, gameObject.transform).GetComponent<InteractiveNode>());
        }
    }
}
