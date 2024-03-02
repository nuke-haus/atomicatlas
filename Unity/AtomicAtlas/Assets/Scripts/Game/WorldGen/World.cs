
using System.Collections.Generic;
using System.Numerics;

// The world should consist of a 1.0 x 1.0 grid with nodes distributed within. Nodes should be connected to reasonable neighbors.
public class World
{
    public List<Node> Nodes => nodes;
    public List<Connection> Connections => connections;

    private List<Node> nodes;
    private List<Connection> connections;

    public World()
    {

    }

    public void CreateNode(Vector2 position)
    {
        nodes.Add(new Node(position));
    }

    public void CreateConnection(Node node1, Node node2, bool isWrap)
    {
        connections.Add(node1.CreateConnection(node2, isWrap));
    }
}
