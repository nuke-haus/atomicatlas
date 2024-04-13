
using System.Collections.Generic;
using UnityEngine;

public class InteractiveNode: MonoBehaviour
{
    public List<InteractiveConnection> Connections => connections;
    public Node Node { get; private set; }
    public bool IsCave {  get; private set; }

    private List<InteractiveConnection> connections;
    private InteractiveNodeGraph parentGraph;

    void Start()
    {
       
    }

    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        
    }

    public void TrySetPosition(Vector3 position)
    {
        if (parentGraph.ContainsPosition(position))
        {
            transform.position = position;

            foreach (var connection in Connections)
            {
                connection.UpdatePosition();
                connection.UpdateVisuals();
            }
        }
    }

    public void SetPosition(World world)
    {
        transform.localPosition = new Vector3(Node.NormalizedPosition.x * world.WorldSize.x, Node.NormalizedPosition.y * world.WorldSize.y, 0f);
    }

    public void ResetInteractiveConnections()
    {
        connections = new List<InteractiveConnection>();
    }

    public void AddInteractiveConnection(InteractiveConnection connection)
    {
        connections.Add(connection);
    }

    public void RemoveInteractiveConnection(InteractiveConnection connection)
    {
        connections.Remove(connection);
    }

    public void SetIsCaveNode(bool isCave)
    {
        IsCave = isCave;
    }

    public void SetNodeGraph(InteractiveNodeGraph nodeGraph)
    {
        parentGraph = nodeGraph;
    }

    public void SetNode(Node n)
    {
        Node = n;
    }
}
