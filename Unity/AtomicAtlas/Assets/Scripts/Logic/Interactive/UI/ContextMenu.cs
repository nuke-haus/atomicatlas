
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
            NodeGraphManager.GlobalInstance.AddNewNode(nodeGraph, transform.position);
        }

        public void OnClickRemoveButton()
        {
            NodeGraphManager.GlobalInstance.DeleteNodes(nodeGraph, selectedNodes);
        }

        public void OnClickJoinButton()
        {
            NodeGraphManager.GlobalInstance.ConnectNodes(nodeGraph, selectedNodes);
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
                    removeButton.gameObject.SetActive(false);
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
