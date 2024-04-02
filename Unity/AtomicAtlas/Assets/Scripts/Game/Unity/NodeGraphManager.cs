
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

public class NodeGraphManager: MonoBehaviour
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
    private EventSystem eventSystem;

    private List<InteractiveNodeGraph> nodeGraphs = new();
    private NodeGraphSortType sortType;
    private bool isGeneratingNodeGraph;

    void Start()
    {
        GlobalInstance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right click
        {
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
                        Debug.Log("We selected a node");
                    }
                    else if (interactiveConnection != null)
                    {
                        Debug.Log("We selected a cnode");
                    }
                }
            }
        }
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
