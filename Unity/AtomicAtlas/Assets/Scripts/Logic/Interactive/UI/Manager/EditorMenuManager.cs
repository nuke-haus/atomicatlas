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
            if (contextMenu != null)
            {
                contextMenu.HighlightNodes(false);
                contextMenu.gameObject.SetActive(false);
            }
        }

        public void SetContextMenuActive(Vector3 position, IEnumerable<InteractiveNode> nodes)
        {
            if (contextMenu == null)
            {
                contextMenu = Instantiate(contextMenuPrefab).GetComponent<ContextMenu>();
            }

            contextMenu.gameObject.SetActive(true);
            contextMenu.transform.position = position;
            contextMenu.SetNodes(nodes);
            contextMenu.HighlightNodes(true);
        }

        public void SetProvinceEditorPanelActive(InteractiveNode node)
        {
            provinceEditor.gameObject.SetActive(true);
            caveEditor.gameObject.SetActive(false);
            connectionEditor.gameObject.SetActive(false);

            provinceEditor.Activate(node);
        }

        public void SetCaveEditorPanelActive(InteractiveNode node)
        {
            provinceEditor.gameObject.SetActive(false);
            caveEditor.gameObject.SetActive(true);
            connectionEditor.gameObject.SetActive(false);

            caveEditor.Activate(node);
        }

        public void SetConnectionEditorPanelActive(InteractiveConnection connection)
        {
            provinceEditor.gameObject.SetActive(false);
            caveEditor.gameObject.SetActive(false);
            connectionEditor.gameObject.SetActive(true);

            connectionEditor.Activate(connection);
        }
    } 
}
