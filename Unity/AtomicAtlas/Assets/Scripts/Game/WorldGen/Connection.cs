
using UnityEngine;

public class Connection
{
    public Node Node1 => node1;
    public Node Node2 => node2;
    public Vector2 ConnectionCenter => connectionCenter;

    private Vector2 connectionCenter;
    private Node node1;
    private Node node2;
    private bool isWrapping;

    public Connection(Node nodeA, Node nodeB, bool wrap = false)
    {
        node1 = nodeA;
        node2 = nodeB;
        connectionCenter = new Vector2((node1.NormalizedPosition.x + node2.NormalizedPosition.x) / 2f, (node1.NormalizedPosition.y + node2.NormalizedPosition.y) / 2f);
        isWrapping = wrap;
    }
}
