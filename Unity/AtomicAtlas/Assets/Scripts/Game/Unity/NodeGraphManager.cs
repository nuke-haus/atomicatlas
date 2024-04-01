
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private List<InteractiveNodeGraph> nodeGraphs = new();
    private NodeGraphSortType sortType;
    private bool isGeneratingNodeGraph;

    void Start()
    {
        GlobalInstance = this;
    }

    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        
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
