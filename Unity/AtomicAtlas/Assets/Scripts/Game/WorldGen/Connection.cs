
public class Connection
{
    public Node Node1 => node1;
    public Node Node2 => node2;

    private Node node1;
    private Node node2;
    private bool isWrapping;

    public Connection(Node nodeA, Node nodeB, bool wrap = false)
    {
        node1 = nodeA;
        node2 = nodeB;
        isWrapping = wrap;
    }
}
