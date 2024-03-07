
using System.Collections.Generic;
using UnityEngine;

// The world plane should consist of a 1.0 x 1.0 grid with nodes distributed within. Nodes should be connected to reasonable neighbors.
public class WorldPlane
{
    public bool IsCave => isCave;
    public string Name => name;
    public List<Node> Nodes => nodes;
    public List<Connection> Connections => connections;

    private bool isCave;
    private string name;
    private List<Node> nodes;
    private List<Connection> connections;

    public WorldPlane(string planeName, bool cave)
    {
        name = planeName;
        isCave = cave;
    }

    public void CreateNode(Vector2 position)
    {
        if (position.x > 1.0f || position.x < 0f || position.y > 1.0f || position.y < 0f)
        {
            Debug.LogError("World plane tried to create an node in a non-normalized position");
        }
        else
        {
            nodes.Add(new Node(position));
        }
    }

    public void CreateConnection(Node node1, Node node2, bool isWrap)
    {
        connections.Add(node1.CreateConnection(node2, isWrap));
    }
}
