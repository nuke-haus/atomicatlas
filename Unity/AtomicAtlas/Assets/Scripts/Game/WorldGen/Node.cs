
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public List<Connection> Connections => connections;
    public Vector2 NormalizedPosition => normalizedPosition;
    public int GateNumber => gateNumber;

    private List<Connection> connections;
    private Vector2 normalizedPosition;
    private int gateNumber = -1;

    public Node(Vector2 position)
    {
        normalizedPosition = position;
    }

    public void SetGateNumber(int gate)
    {
        gateNumber = gate;
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
