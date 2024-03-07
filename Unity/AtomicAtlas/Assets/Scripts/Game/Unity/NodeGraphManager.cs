
using System.Collections.Generic;
using UnityEngine;

public class NodeGraphManager: MonoBehaviour
{
    public static NodeGraphManager GlobalInstance
    {
        get;
        private set;
    }

    [SerializeField]
    private GameObject nodeGraphRoot;

    [SerializeField]
    private GameObject nodeGraphPrefab;

    private List<InteractiveNodeGraph> nodeGraphs;
    private NodeGraphSortType sortType;

    void Start()
    {
        GlobalInstance = this;
        nodeGraphs = new List<InteractiveNodeGraph>();
    }

    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        
    }

    public void GenerateNodeGraphs(World world) 
    { 
        while (world.Planes.Count < nodeGraphs.Count)
        {
            var nodeGraph = nodeGraphs[0];
            nodeGraphs.RemoveAt(0);
            nodeGraph.Destroy();
        }

        for (int i = 0; i < world.Planes.Count; i++)
        {
            var plane = world.Planes[i];
            var nodeGraph = nodeGraphs[i];

            if (nodeGraph == null)
            {
                nodeGraph = Instantiate(nodeGraphPrefab, nodeGraphRoot.transform).GetComponent<InteractiveNodeGraph>();
                nodeGraphs.Add(nodeGraph);
            }

            nodeGraph.Initialize(world, plane, i, sortType);
        }
    }
}
