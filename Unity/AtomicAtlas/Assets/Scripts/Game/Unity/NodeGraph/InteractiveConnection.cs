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

    private void UpdateVisuals()
    {
        if (node1 == null || node2 == null)
        {
            GetComponent<LineRenderer>().SetPositions(new Vector3[]{});
        }
        else
        {
            var pos1 = node1.transform.position;
            var pos2 = node2.transform.position;

            GetComponent<LineRenderer>().SetPositions(new Vector3[] {pos1, pos2});
        }
    }

    public void UpdateConnectionPosition(World world)
    {
        transform.localPosition = new Vector3(Connection.ConnectionCenter.x * world.WorldSize.x, Connection.ConnectionCenter.y * world.WorldSize.y, 0f);
    }

    public void SetNode1(InteractiveNode node)
    {
        node1 = node;
        UpdateVisuals();
    }

    public void SetNode2(InteractiveNode node)
    {
        node2 = node;
        UpdateVisuals();
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

        UpdateVisuals();
    }

    public void SetConnection(Connection c)
    {
        connection = c;
        UpdateVisuals();
    }
}
