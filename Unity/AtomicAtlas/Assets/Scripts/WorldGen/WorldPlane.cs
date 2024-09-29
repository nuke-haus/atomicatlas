
using System.Collections.Generic;
using UnityEngine;

namespace Atlas.WorldGen
{
    // The world plane should consist of a 1.0 x 1.0 grid with nodes distributed within. Nodes should be connected to reasonable neighbors.
    public class WorldPlane
    {
        public bool IsCave => isCave;
        public string Name => name;
        public List<Node> Nodes => nodes;
        public List<Node> PlaceholderNodes => placeholderNodes;
        public List<Connection> Connections => connections;

        private bool isCave;
        private string name;
        private List<Node> nodes = new();
        private List<Node> placeholderNodes = new();
        private List<Connection> connections = new();

        public WorldPlane(string planeName, bool cave)
        {
            name = planeName;
            isCave = cave;
        }

        private Node CreateNode(Vector2 position, Terrain terrain, Node parent = null)
        {
            if (parent == null && (position.x > 1.0f || position.x < 0f || position.y > 1.0f || position.y < 0f))
            {
                Debug.LogError("World plane tried to create an node in a non-normalized position");
            }
            else
            {
                var node = new Node(position, terrain, parent);

                if (parent == null)
                {
                    nodes.Add(node);
                }
                else
                {
                    placeholderNodes.Add(node);
                }

                return node;
            }
            return null;
        }

        public Node CreatePlaceholderNode(Vector2 position, Node parent)
        {
            return CreateNode(position, Terrain.PLAINS, parent);
        }

        public Node CreateNode(Vector2 position)
        {
            return CreateNode(position, Terrain.PLAINS);
        }

        public Connection CreateConnection(Node node1, Node node2, bool isWrap)
        {
            var connection = node1.CreateConnection(node2, isWrap);
            connections.Add(connection);
            return connection;
        }

        public void DeletePlaceholderNodes()
        {
            placeholderNodes.Clear();
        }
    } 
}
