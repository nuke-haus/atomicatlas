using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveConnection: MonoBehaviour
{
    public Connection Connection => connection;
    public InteractiveNode Node1 => node1;
    public InteractiveNode Node2 => node2;

    private Connection connection;
    private InteractiveNode node1;
    private InteractiveNode node2;

    void Start()
    {
       
    }

    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        
    }

    public void UpdateConnectionPosition(World world)
    {
        transform.position = new Vector3(Connection.ConnectionCenter.x * world.WorldSize.x, Connection.ConnectionCenter.y * world.WorldSize.y, 0f);
    }

    public void SetNode1(InteractiveNode node)
    {
        node1 = node;
    }

    public void SetNode2(InteractiveNode node)
    {
        node2 = node;
    }

    public void SetInteractiveNode(InteractiveNode node)
    {
        if (node1 == null)
        {
            node1 = node;
        }
        else
        {
            node2 = node;
        }
    }

    public void SetConnection(Connection c)
    {
        connection = c;
    }
}
