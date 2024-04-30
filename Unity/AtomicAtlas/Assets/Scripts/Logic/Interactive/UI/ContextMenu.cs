
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Atlas.Logic
{
    public class ContextMenu : MonoBehaviour
    {
        [SerializeField]
        private Button addButton;

        [SerializeField]
        private Button removeButton;

        [SerializeField]
        private Button joinButton;

        [SerializeField]
        private RectTransform panelTransform;

        private IEnumerable<InteractiveNode> selectedNodes;
        private InteractiveNodeGraph nodeGraph;

        void Start()
        {

        }

        void Update()
        {

        }

        public void OnClickAddButton()
        {
            EditorMenuManager.GlobalInstance.HideContextMenu();
            NodeGraphManager.GlobalInstance.AddNewNode(nodeGraph, transform.position);
        }

        public void OnClickRemoveButton()
        {
            var nodes = selectedNodes.ToList();

            EditorMenuManager.GlobalInstance.HideContextMenu();
            NodeGraphManager.GlobalInstance.DeleteNodes(nodeGraph, nodes);
        }

        public void OnClickJoinButton()
        {
            var nodes = selectedNodes.ToList();

            EditorMenuManager.GlobalInstance.HideContextMenu();

            if (nodes.Count() == 2)
            {
                NodeGraphManager.GlobalInstance.ConnectNodes(nodeGraph, nodes.ElementAt(0), nodes.ElementAt(1));
            }
        }

        public void SetNodeGraph(InteractiveNodeGraph graph)
        {
            nodeGraph = graph;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetNodes(IEnumerable<InteractiveNode> nodes)
        {
            if (nodes == null)
            {
                nodes = new List<InteractiveNode>();
            }

            selectedNodes = nodes;
            UpdateVisibleButtons();
        }

        public void HighlightNodes(bool highlighted)
        {
            foreach (var node in selectedNodes)
            {
                node.SetOutlineVisible(highlighted);
            }
        }

        private void UpdateVisibleButtons()
        {
            addButton.gameObject.SetActive(false);
            removeButton.gameObject.SetActive(false);
            joinButton.gameObject.SetActive(false);
            var count = 1;

            switch (selectedNodes.Count())
            {
                case 0:
                    addButton.gameObject.SetActive(true);
                    break;
                case 1:
                    removeButton.gameObject.SetActive(true);
                    break;
                case 2:
                    count = 2;
                    removeButton.gameObject.SetActive(true);
                    joinButton.gameObject.SetActive(true);
                    break;
                default:
                    removeButton.gameObject.SetActive(true);
                    break;
            }

            if (count == 1)
            {
                panelTransform.sizeDelta = new Vector2(48, 50);
            }
            else
            {
                panelTransform.sizeDelta = new Vector2(88, 50);
            }
        }
    } 
}
