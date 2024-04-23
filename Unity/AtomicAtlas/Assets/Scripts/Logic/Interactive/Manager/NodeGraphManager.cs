
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Atlas.WorldGen;
using System.Linq;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;
using UnityEditor.Experimental.GraphView;

namespace Atlas.Logic
{
    public class NodeGraphManager : MonoBehaviour
    {
        public static NodeGraphManager GlobalInstance
        {
            get;
            private set;
        }

        public bool IsGeneratingNodeGraph => isGeneratingNodeGraph;

        [SerializeField]
        private GameObject nodeGraphRoot;

        [SerializeField]
        private GameObject nodeGraphPrefab;

        [SerializeField]
        private GameObject selectionBoxPrefab;

        [SerializeField]
        private EventSystem eventSystem;

        private List<InteractiveNode> selectedNodes = new();
        private List<InteractiveNodeGraph> nodeGraphs = new();
        private NodeGraphSortType sortType;
        private bool isGeneratingNodeGraph;
        private InteractiveNode selectedNode;
        private SelectionBox selectionBox;

        void Start()
        {
            GlobalInstance = this;
        }

        void Update()
        {
            // RIGHT CLICK LOGIC - SELECTION LOGIC LIVES HERE

            if (Input.GetMouseButtonDown(1)) // Right click
            {
                EditorMenuManager.GlobalInstance.HideContextMenu();

                if (!eventSystem.IsPointerOverGameObject())
                {
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = 5f;

                    var gameObject = RayTrace(mousePos);

                    if (gameObject != null)
                    {
                        var interactiveNode = gameObject.GetComponentInParent<InteractiveNode>();
                        var interactiveConnection = gameObject.GetComponentInParent<InteractiveConnection>();

                        if (interactiveNode != null)
                        {
                            if (interactiveNode.IsCave)
                            {
                                SetActiveCaveNode(interactiveNode);
                            }
                            else
                            {
                                SetActiveNode(interactiveNode);
                            }

                            selectedNode = interactiveNode;
                        }
                        else if (interactiveConnection != null)
                        {
                            DeselectNode();
                            SetActiveConnection(interactiveConnection);
                        }
                    }
                    else
                    {
                        EditorMenuManager.GlobalInstance.HideAllPanels();
                        DeselectNode();                    
                        CreateSelectionBox();
                    }
                }
            }
            else if (Input.GetMouseButton(1)) // Hold right click
            {
                if (selectionBox != null)
                {
                    var plane = new Plane(new Vector3(0f, 0f, 1f), selectionBox.transform.position);
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    var enter = 0f;

                    if (plane.Raycast(ray, out enter))
                    {
                        selectionBox.SetExtents(ray.GetPoint(enter));
                    }                    
                }
            }
            else if (Input.GetMouseButtonUp(1)) // Release right click
            {
                if (selectionBox != null)
                {
                    var plane = new Plane(new Vector3(0f, 0f, 1f), selectionBox.transform.position);
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    var enter = 0f;

                    if (plane.Raycast(ray, out enter))
                    {
                        EditorMenuManager.GlobalInstance.SetContextMenuActive(ray.GetPoint(enter), selectionBox.GetSelectedNodes(), selectionBox.InteractiveNodeGraph);
                    }

                    DestroySelectionBox();
                }
            }

            // LEFT CLICK LOGIC - NODE MOVEMENT LOGIC LIVES HERE

            if (Input.GetMouseButtonDown(0)) // Left click
            {
                if (!eventSystem.IsPointerOverGameObject())
                {
                    EditorMenuManager.GlobalInstance.HideContextMenu();

                    if (selectedNode != null)
                    {
                        var plane = new Plane(new Vector3(0f, 0f, 1f), selectedNode.transform.position);
                        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        var enter = 0f;

                        if (plane.Raycast(ray, out enter))
                        {
                            selectedNode.TrySetPosition(ray.GetPoint(enter));
                        }
                    }     
                }
            }
            else if (Input.GetMouseButtonUp(0)) // Release left click
            {

            }
        }

        public void AddNewNode(InteractiveNodeGraph interactiveNodeGraph, Vector3 position)
        {
            interactiveNodeGraph.AddNode(position);
        }

        public void DeleteNodes(InteractiveNodeGraph interactiveNodeGraph, IEnumerable<InteractiveNode> nodes)
        {
            interactiveNodeGraph.DeleteNodes(nodes);
        }

        public void ConnectNodes(InteractiveNodeGraph interactiveNodeGraph, IEnumerable<InteractiveNode> nodes)
        {
            interactiveNodeGraph.ConnectNodes(nodes);
        }

        private InteractiveNodeGraph GetNodeGraphContainingCursor()
        {
            if (eventSystem.IsPointerOverGameObject())
            {
                return null;
            }

            foreach (var nodeGraph in nodeGraphs)
            {
                var plane = new Plane(new Vector3(0f, 0f, 1f), nodeGraph.gameObject.transform.position);
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var enter = 0f;

                if (plane.Raycast(ray, out enter))
                {
                    var position = ray.GetPoint(enter);
                    if (nodeGraph.ContainsPosition(position, false))
                    {
                        return nodeGraph;
                    }
                }
            }

            return null;
        }

        private void DestroySelectionBox()
        {
            if (selectionBox != null)
            {
                Destroy(selectionBox.gameObject);
                selectionBox = null;
            }
        }

        private void CreateSelectionBox() 
        {
            var nodeGraph = GetNodeGraphContainingCursor();

            if (nodeGraph == null)
            {
                return;
            }

            var plane = new Plane(new Vector3(0f, 0f, 1f), nodeGraph.transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var enter = 0f;

            if (plane.Raycast(ray, out enter))
            {
                var position = ray.GetPoint(enter);

                selectionBox = Instantiate(selectionBoxPrefab).GetComponent<SelectionBox>();
                selectionBox.SetPosition(position);
                selectionBox.SetNodeGraph(nodeGraph);
            }
        }

        public void DeselectNode()
        {
            selectedNode = null;
        }

        private void SetActiveConnection(InteractiveConnection connection)
        {
            EditorMenuManager.GlobalInstance.SetConnectionEditorPanelActive(connection);
        }

        private void SetActiveNode(InteractiveNode node)
        {
            EditorMenuManager.GlobalInstance.SetProvinceEditorPanelActive(node);
        }

        private void SetActiveCaveNode(InteractiveNode node)
        {
            EditorMenuManager.GlobalInstance.SetCaveEditorPanelActive(node);
        }

        private void FixedUpdate()
        {
        }

        private GameObject RayTrace(Vector3 mousePos)
        {
            var cameraPos = Camera.main.transform.position;
            var cameraForwardPos = Camera.main.ScreenToWorldPoint(mousePos);
            var rayDirection = cameraForwardPos - cameraPos;
            RaycastHit hit;

            bool isHit = Physics.Raycast(cameraPos, rayDirection, out hit, Mathf.Infinity);

            if (isHit)
            {
                return hit.collider.gameObject;
            }

            return null;
        }

        public void GenerateNodeGraphs(World world)
        {
            StartCoroutine(GenerateNodeGraphAsync(world));
        }

        private IEnumerator GenerateNodeGraphAsync(World world)
        {
            isGeneratingNodeGraph = true;

            while (world.Planes.Count < nodeGraphs.Count)
            {
                var nodeGraph = nodeGraphs[0];
                nodeGraphs.RemoveAt(0);
                nodeGraph.Destroy();
            }

            for (int i = 0; i < world.Planes.Count; i++)
            {
                var plane = world.Planes[i];
                InteractiveNodeGraph nodeGraph = null;

                if (nodeGraphs.Count < world.Planes.Count)
                {
                    nodeGraph = Instantiate(nodeGraphPrefab, nodeGraphRoot.transform).GetComponent<InteractiveNodeGraph>();
                    nodeGraphs.Add(nodeGraph);
                }
                else
                {
                    nodeGraph = nodeGraphs[i];
                }

                nodeGraph.Initialize(world, plane, -i, sortType);
            }

            isGeneratingNodeGraph = false;
            yield return null;
        }
    }
}