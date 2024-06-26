using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Atlas.WorldGen;
using Terrain = Atlas.WorldGen.Terrain;

namespace Atlas.Logic
{
    public class CaveEditor : MonoBehaviour
    {
        [SerializeField]
        private Toggle ImpassableToggle;

        [SerializeField]
        private Toggle CaveToggle;

        [SerializeField]
        private Toggle CaveForestToggle;

        [SerializeField]
        private Toggle CrystalCaveToggle;

        [SerializeField]
        private Toggle DripCaveToggle;

        [SerializeField]
        private Toggle SeaCaveToggle;

        [SerializeField]
        private Toggle LargeToggle;

        [SerializeField]
        private Toggle SmallToggle;

        [SerializeField]
        private Toggle FortToggle;

        [SerializeField]
        private Toggle ThroneToggle;

        [SerializeField]
        private Toggle MoreMagicSitesToggle;

        [SerializeField]
        private TMP_InputField PlaneGateNumberInput;

        [SerializeField]
        private TMP_InputField NameInput;

        private InteractiveNode selectedNode;

        void Start()
        {

        }

        void Update()
        {

        }

        public void HidePanel()
        {
            selectedNode?.SetOutlineVisible(false);
            selectedNode = null;

            gameObject.SetActive(false);
            NodeGraphManager.GlobalInstance.DeselectNode();
        }

        public void Activate(InteractiveNode node)
        {
            if (selectedNode != null)
            {
                selectedNode.SetOutlineVisible(false);
            }

            selectedNode = node;
            selectedNode.SetOutlineVisible(true);

            UpdatePanel();
        }

        public void DeleteNode()
        {
            var node = selectedNode;

            HidePanel();
            NodeGraphManager.GlobalInstance.DeleteNode(node.ParentNodeGraph, node);
        }

        public void ApplyChanges()
        {
            var terrain = Terrain.PLAINS;

            // Figure out which exclusive terrain flag to use first
            if (CaveToggle.isOn)
            {
                terrain = Terrain.CAVE;
            }
            if (CaveForestToggle.isOn)
            {
                terrain = Terrain.FOREST;
            }
            if (CrystalCaveToggle.isOn)
            {
                terrain = Terrain.HIGHLAND;
            }

            // Sea swamp not allowed for caves
            if (DripCaveToggle.isOn)
            {
                terrain = Terrain.SWAMP;
            }
            else if (SeaCaveToggle.isOn)
            {
                terrain |= Terrain.SEA;
            }

            // Apply size modifiers
            if (LargeToggle.isOn)
            {
                terrain |= Terrain.LARGER;
            }
            else if (SmallToggle.isOn)
            {
                terrain |= Terrain.SMALLER;
            }

            // Apply misc modifiers
            if (ThroneToggle.isOn)
            {
                terrain |= Terrain.THRONE;
            }
            if (MoreMagicSitesToggle.isOn)
            {
                terrain |= Terrain.MANY_SITES;
            }

            var text = PlaneGateNumberInput.text;
            if (text == string.Empty)
            {
                text = "0";
                PlaneGateNumberInput.text = string.Empty;
            }

            selectedNode.Node.SetHasFort(FortToggle.isOn);
            selectedNode.Node.SetGateNumber(int.Parse(text));
            selectedNode.Node.SetNameOverride(NameInput.text);
            selectedNode.Node.SetTerrain(terrain);

            UpdatePanel();
        }

        private void UpdatePanel()
        {
            var gate = selectedNode.Node.GateNumber == 0 
                ? string.Empty 
                : selectedNode.Node.GateNumber.ToString();

            PlaneGateNumberInput.text = gate;
            NameInput.text = selectedNode.Node.NameOverride;

            ImpassableToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.CAVE_WALL);
            CaveToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.CAVE);
            CaveForestToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.FOREST);
            CrystalCaveToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.HIGHLAND);
            DripCaveToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.SWAMP);
            SeaCaveToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.SEA);

            LargeToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.LARGER);
            SmallToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.SMALLER);

            ThroneToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.THRONE);
            MoreMagicSitesToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.MANY_SITES);
            FortToggle.isOn = selectedNode.Node.Fort != Fort.NONE;
        }
    }

}