
using System.Collections.Generic;
using UnityEngine;
using Atlas.WorldGen;
using Atlas.Core;
using System.Linq;

namespace Atlas.Logic
{
    public class InteractiveNode : MonoBehaviour
    {
        public InteractiveNodeGraph ParentNodeGraph => parentGraph;
        public List<InteractiveConnection> Connections => connections;
        public Node Node { get; private set; }
        public bool IsCave { get; private set; }

        [SerializeField]
        private MeshCollider meshCollider;

        [SerializeField]
        private MeshFilter meshFilter;

        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private GameObject meshObject;

        private List<InteractiveConnection> connections = new();
        private InteractiveNodeGraph parentGraph;

        private List<Vector3> polygonShape = new();

        void Start()
        {
            SetOutlineVisible(false);
        }

        void Update()
        {

        }

        private void FixedUpdate()
        {

        }

        public void SetOutlineVisible(bool visible)
        {
            var outline = gameObject.GetComponent<Outline>();
            outline.OutlineWidth = visible 
                ? 2.0f 
                : 0.0f;
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

        public bool HasConnection(InteractiveNode node)
        {
            return connections.Any(x => x.Node1 == node || x.Node2 == node);
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

        private Vector2[] GetVector2Array(List<Vector3> list)
        {
            return list.Select(vector => new Vector2(vector.x, vector.y)).ToArray();
        }

        private bool ValidateMeshExists(Vector3 pt)
        {
            RaycastHit hit;
            pt.z = -900f;

            if (Physics.Raycast(pt, Vector3.forward, out hit, 9000))
            {
                if (hit.collider == meshCollider)
                {
                    return true;
                }
            }

            return false;
        }

        public void ConstructMesh()
        {
            // TODO generate the shape first

            var triangulator = new Triangulator(GetVector2Array(polygonShape));
            var indices = triangulator.Triangulate();
            var uv = new Vector2[polygonShape.Count];

            for (var i = 0; i < polygonShape.Count; i++)
            {
                uv[i] = new Vector2(polygonShape[i].x, polygonShape[i].y);
            }

            var mesh = new Mesh();
            mesh.vertices = polygonShape.ToArray();
            mesh.uv = uv;
            mesh.triangles = indices;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            var norms = mesh.normals;
            var valid = ValidateMeshExists(transform.position);

            for (var i = 0; i < norms.Length - 1; i++)
            {
                if (valid)
                {
                    norms[i] = Vector3.back;
                }
                else
                {
                    norms[i] = Vector3.forward;
                }
            }

            mesh.normals = norms;

            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
            meshObject.transform.localPosition = transform.position * -1f;

            //assign_mat(GenerationManager.s_generation_manager.Season);
        }
    }
}


