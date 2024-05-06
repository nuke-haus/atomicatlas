using Atlas.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Atlas.Logic
{
    public class EditorMenuManager : MonoBehaviour
    {
        public static EditorMenuManager GlobalInstance
        {
            get;
            private set;
        }

        [SerializeField]
        private ProvinceEditor provinceEditor;

        [SerializeField]
        private CaveEditor caveEditor;

        [SerializeField]
        private ConnectionEditor connectionEditor;

        [SerializeField]
        private GameObject contextMenuPrefab;
 
        private ContextMenu contextMenu;

        void Start()
        {
            GlobalInstance = this;
        }

        void Update()
        {

        }

        public void HideContextMenu()
        {
            if (contextMenu != null && contextMenu.gameObject.activeSelf)
            {
                contextMenu.HighlightNodes(false);
                contextMenu.SetNodes(null);
                contextMenu.gameObject.SetActive(false);
            }
        }

        public void SetContextMenuActive(Vector3 position, IEnumerable<InteractiveNode> nodes, InteractiveNodeGraph nodeGraph)
        {
            if (contextMenu == null)
            {
                contextMenu = Instantiate(contextMenuPrefab).GetComponent<ContextMenu>();
            }

            contextMenu.gameObject.SetActive(true);
            contextMenu.transform.position = position;
            contextMenu.SetNodes(nodes);
            contextMenu.SetNodeGraph(nodeGraph);
            contextMenu.HighlightNodes(true);
        }

        public void HideAllPanels()
        {
            provinceEditor.HidePanel();
            caveEditor.HidePanel();
            connectionEditor.HidePanel();
        }

        public void SetProvinceEditorPanelActive(InteractiveNode node)
        {
            provinceEditor.gameObject.SetActive(true);
            caveEditor.HidePanel();
            connectionEditor.HidePanel();

            provinceEditor.Activate(node);
        }

        public void SetCaveEditorPanelActive(InteractiveNode node)
        {
            provinceEditor.HidePanel();
            caveEditor.gameObject.SetActive(true);
            connectionEditor.HidePanel();

            caveEditor.Activate(node);
        }

        public void SetConnectionEditorPanelActive(InteractiveConnection connection)
        {
            provinceEditor.HidePanel();
            caveEditor.HidePanel();
            connectionEditor.gameObject.SetActive(true);

            connectionEditor.Activate(connection);
        }
    } 
}
