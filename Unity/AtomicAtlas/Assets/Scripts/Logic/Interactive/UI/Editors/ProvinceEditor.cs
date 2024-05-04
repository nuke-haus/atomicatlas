using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Atlas.WorldGen;
using Terrain = Atlas.WorldGen.Terrain;

namespace Atlas.Logic
{
    public class ProvinceEditor : MonoBehaviour
    {
        [SerializeField]
        private Toggle PlainsToggle;

        [SerializeField]
        private Toggle FarmToggle;

        [SerializeField]
        private Toggle ForestToggle;

        [SerializeField]
        private Toggle HighlandToggle;

        [SerializeField]
        private Toggle CaveToggle;

        [SerializeField]
        private Toggle SwampToggle;

        [SerializeField]
        private Toggle WasteToggle;

        [SerializeField]
        private Toggle SeaToggle;

        [SerializeField]
        private Toggle DeepSeaToggle;

        [SerializeField]
        private Toggle LargeToggle;

        [SerializeField]
        private Toggle SmallToggle;

        [SerializeField]
        private Toggle HotToggle;

        [SerializeField]
        private Toggle ColdToggle;

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
            if (FarmToggle.isOn)
            {
                terrain = Terrain.FARM;
            }
            if (ForestToggle.isOn)
            {
                terrain = Terrain.FOREST;
            }
            if (HighlandToggle.isOn)
            {
                terrain = Terrain.HIGHLAND;
            }
            if (CaveToggle.isOn)
            {
                terrain = Terrain.CAVE;
            }
            if (SwampToggle.isOn)
            {
                terrain = Terrain.SWAMP;
            }
            if (WasteToggle.isOn)
            {
                terrain = Terrain.WASTE;
            }
            if (SeaToggle.isOn)
            {
                terrain = Terrain.SEA;
            }
            if (DeepSeaToggle.isOn)
            {
                terrain = Terrain.SEA | Terrain.DEEP_SEA;
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

            // Apply temperature modifiers
            if (HotToggle.isOn)
            {
                terrain |= Terrain.WARMER;
            }
            else if (ColdToggle.isOn)
            {
                terrain |= Terrain.COLDER;
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

            FarmToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.FARM);
            ForestToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.FOREST);
            HighlandToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.HIGHLAND);
            CaveToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.CAVE);
            SwampToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.SWAMP);
            WasteToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.WASTE);
            SeaToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.SEA) && !selectedNode.Node.Terrain.HasFlag(Terrain.DEEP_SEA);
            DeepSeaToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.DEEP_SEA);

            LargeToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.LARGER);
            SmallToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.SMALLER);

            HotToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.WARMER);
            ColdToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.COLDER);

            ThroneToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.THRONE);
            MoreMagicSitesToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.MANY_SITES);
            FortToggle.isOn = selectedNode.Node.Fort != Fort.NONE;

            PlainsToggle.isOn = !FarmToggle.isOn
                && !ForestToggle.isOn
                && !HighlandToggle.isOn
                && !CaveToggle.isOn
                && !SwampToggle.isOn
                && !WasteToggle.isOn
                && !SeaToggle.isOn
                && !DeepSeaToggle.isOn;
        }
    } 
}
