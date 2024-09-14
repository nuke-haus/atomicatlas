
using UnityEngine;

namespace Atlas.WorldGen
{
    public class Connection
    {
        public Node Node1 { get; private set; }
        public Node Node2 { get; private set; }
        public Vector2 ConnectionCenter { get; private set; }
        public ConnectionType ConnectionType { get; private set; }
        public bool IsWrapConnection { get; private set; }

        public Connection(Node nodeA, Node nodeB, bool wrap = false)
        {
            Node1 = nodeA;
            Node2 = nodeB;
            ConnectionCenter = new Vector2((Node1.NormalizedPosition.x + Node2.NormalizedPosition.x) / 2f, (Node1.NormalizedPosition.y + Node2.NormalizedPosition.y) / 2f);
            IsWrapConnection = wrap;
        }

        public void SetConnectionType(ConnectionType connectionType)
        {
            ConnectionType = connectionType;
        }
    } 
}
