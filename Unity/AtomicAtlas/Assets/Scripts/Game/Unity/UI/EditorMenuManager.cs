using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using TMPro;
using UnityEngine.UI;


public class EditorMenuManager : MonoBehaviour
{
    public static EditorMenuManager GlobalInstance
    {
        get;
        private set;
    }

    [SerializeField]
    private GameObject editNodePanel;

    [SerializeField]
    private GameObject editConnectionPanel;

    private InteractiveNode selectedNode;
    private InteractiveConnection selectedConnection;

    void Start()
    {
        GlobalInstance = this;
    }

    void Update()
    {
        
    }

    public void SetNodeEditorPanelActive(InteractiveNode node)
    {
        editNodePanel.SetActive(true);
        selectedNode = node;
    }

    public void SetConnectionEditorPanelActive(InteractiveConnection connection)
    {
        editNodePanel.SetActive(true);
        selectedConnection = connection;
    }
}
