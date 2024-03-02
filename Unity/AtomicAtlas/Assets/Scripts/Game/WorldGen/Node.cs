
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class Node
{
    public List<Connection> Connections => connections;
    public Vector2 NormalizedPosition => normalizedPosition;

    private List<Connection> connections;
    private Vector2 normalizedPosition;

    public Node(Vector2 position)
    {
        normalizedPosition = position;
    }

    public void SetNormalizedPosition(Vector2 position)
    {
        normalizedPosition = position; 
    }

    public bool HasConnection(Connection connection)
    {
        return connections.Contains(connection);
    }

    public void AddConnection(Connection connection)
    {
        if (!HasConnection(connection))
        {
            connections.Add(connection);
        }
    }

    public bool IsConnectedTo(Node node)
    {
        return connections.Any(connection => connection.Node1 == node || connection.Node2 == node);
    }

    public Connection CreateConnection(Node otherNode, bool isWrap = false)
    {
        var connection = new Connection(this, otherNode, isWrap);

        AddConnection(connection);
        otherNode.AddConnection(connection);

        return connection;
    }
}
