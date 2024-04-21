
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

        private IEnumerable<InteractiveNode> selectedNodes;

        void Start()
        {

        }

        void Update()
        {

        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetNodes(IEnumerable<InteractiveNode> nodes)
        {
            selectedNodes = nodes;
        }

        public void HighlightNodes(bool highlighted)
        {
            foreach (var node in selectedNodes)
            {
                node.SetOutlineVisible(highlighted);
            }
        }
    } 
}
