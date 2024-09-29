
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Atlas.WorldGen
{
    public class Node
    {
        public IEnumerable<Connection> Connections => connections;
        public Vector2 NormalizedPosition { get; private set; }
        public int GateNumber { get; private set; }
        public Terrain Terrain { get; private set; }
        public Fort Fort { get; private set; }
        public string NameOverride { get; private set; }
        public Node ParentNode { get; private set; }
        public bool IsPlaceholderNode => ParentNode != null;
        public bool HasWrapConnection => connections.Any(conn => conn.IsWrapConnection);

        private List<Connection> connections = new();

        public Node(Vector2 position, Terrain terrain, Node parentNode = null)
        {
            NormalizedPosition = position;
            Terrain = terrain;
            ParentNode = parentNode;
        }

        public IEnumerable<Node> GetConnectedNodes()
        {
            foreach (var connection in connections)
            {
                if (connection.Node1 != this)
                {
                    yield return connection.Node1;
                }
                else
                {
                    yield return connection.Node2;
                }
            }
        }

        public void SetHasFort(bool hasFort)
        {
            Fort = hasFort
                ? FortHelper.GetFort(this)
                : Fort.NONE;
        }

        public void SetTerrain(Terrain terrain)
        {
            Terrain = terrain;
        }

        public void SetGateNumber(int gate)
        {
            GateNumber = gate;
        }

        public void SetNameOverride(string nameOverride)
        {
            NameOverride = nameOverride;
        }

        public void SetNormalizedPosition(Vector2 position)
        {
            NormalizedPosition = position;
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

}